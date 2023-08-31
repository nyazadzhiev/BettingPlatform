using BettingPlatform.BLL.Contracts;
using BettingPlatform.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace BettingPlatform.Infrastructure
{
    public class PeriodicRequestService : BackgroundService
    {
        private readonly HttpClient HttpClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly UpdateManager _updateManager;

        public PeriodicRequestService(HttpClient httpClient, IServiceProvider serviceProvider, IMemoryCache memoryCache, UpdateManager updateManager)
        {
            HttpClient = httpClient;
            _serviceProvider = serviceProvider;
            _memoryCache = memoryCache;
            _updateManager = updateManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await MakeRequest("https://sports.ultraplay.net/sportsxml?clientKey=9C5E796D-4D54-42FD-A535-D7E77906541A&sportId=2357&days=7");

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }

        private async Task MakeRequest(string endpointUrl)
        {
            try
            {
                HttpResponseMessage response = await HttpClient.GetAsync(endpointUrl);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Request to {endpointUrl} successful at {DateTime.Now}");
                    var content = await response.Content.ReadAsStringAsync();
                    XmlSerializer serializer = new XmlSerializer(typeof(XmlSports));
                    using (StringReader reader = new StringReader(content))
                    {
                        var newSportData = (XmlSports)serializer.Deserialize(reader);

                        if (_memoryCache.TryGetValue("MatchResults", out Sport cachedResults))
                        {
                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var repository = scope.ServiceProvider.GetRequiredService<IRepository<Sport>>();

                                // Compare and apply changes to the database
                                await CompareAndApplyChanges(repository, cachedResults, newSportData.Sport);
                                await repository.SaveChangesAsync();

                                // Update cache with the new data
                                _memoryCache.Set("MatchResults", newSportData.Sport, TimeSpan.FromMinutes(5));
                            }
                        }
                        else
                        {
                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var repository = scope.ServiceProvider.GetRequiredService<IRepository<Sport>>();
                                await repository.AddAsync(newSportData.Sport);
                                await repository.SaveChangesAsync();

                                _memoryCache.Set("MatchResults", newSportData.Sport, TimeSpan.FromMinutes(5));
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Request to {endpointUrl} failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred for {endpointUrl}: {ex.Message}");
            }
        }

        private async Task CompareAndApplyChanges(IRepository<Sport> repository, Sport cachedResults, Sport newResults)
        {
            if(cachedResults == null || cachedResults.ID != newResults.ID)
            {
                repository.AddAsync(newResults);
            }

            await CheckEvents(cachedResults.Event, newResults.Event);
        }

        private async Task CheckEvents(List<Event> cachedResult, List<Event> newResult)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var eventRepository = scope.ServiceProvider.GetRequiredService<IRepository<Event>>();
                foreach (Event e in newResult)
                {
                    if (!cachedResult.Any(c => c.ID == e.ID))
                    {
                        await eventRepository.AddAsync(e);

                        _updateManager.RaiseUpdateEvent(new UpdateEventArgs
                        {
                            Entity = EntityType.Event,
                            EntityId = e.ID
                        });
                    }
                    else
                    {
                        await CheckMatches(cachedResult, e.Match);
                    }
                }
            }
        }

        private async Task CheckMatches(List<Event> cachedResult, List<DAL.Entities.Match> match)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var matchRepository = scope.ServiceProvider.GetRequiredService<IRepository<DAL.Entities.Match>>();
                foreach (Event @event in cachedResult)
                {
                    if (!AreListsEqualIgnoringOrder(@event.Match, match))
                    {
                        await matchRepository.DeleteAll();
                        await matchRepository.AddRangeAsync(match);

                        _updateManager.RaiseUpdateEvent(new UpdateEventArgs
                        {
                            Entity = EntityType.Match,
                            EntityId = @event.ID
                        });
                    }
                    else
                    {
                        await CheckBets(@event.Match, match);
                    }
                }
            }
        }

        private async Task CheckBets(List<DAL.Entities.Match> cachedResult, List<DAL.Entities.Match> newResult)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var betRepository = scope.ServiceProvider.GetRequiredService<IRepository<Bet>>();
                foreach (var match in cachedResult)
                {
                    var bets = match.Bet;
                    foreach (var newMatch in newResult)
                    {
                        if (!AreListsEqualIgnoringOrder(bets, newMatch.Bet))
                        {
                            await betRepository.DeleteAll();
                            await betRepository.AddRangeAsync(newMatch.Bet);

                            _updateManager.RaiseUpdateEvent(new UpdateEventArgs
                            {
                                Entity = EntityType.Bet,
                                EntityId = match.ID
                            });
                        }
                        else
                        {
                            await CheckOdds(bets, newMatch.Bet);
                        }
                    }
                }
            }
        }

        private async Task CheckOdds(List<Bet> cachedResult, List<Bet> newResult)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var oddRepository = scope.ServiceProvider.GetRequiredService<IRepository<Odd>>();
                foreach (var bet in cachedResult)
                {
                    var odds = bet.Odd;
                    foreach(var newBet in newResult)
                    {
                        if (!AreListsEqualIgnoringOrder(odds, newBet.Odd))
                        {
                            await oddRepository.DeleteAll();
                            await oddRepository.AddRangeAsync(newBet.Odd);

                            _updateManager.RaiseUpdateEvent(new UpdateEventArgs
                            {
                                Entity = EntityType.Odd,
                                EntityId = bet.ID
                            });
                        }
                    }
                }
            }
        }

        private bool AreListsEqualIgnoringOrder<T>(List<T> list1, List<T> list2)
        {
            if (list1.Count != list2.Count)
                return false;

            var exceptList1 = list1.Except(list2);
            var exceptList2 = list2.Except(list1);

            return !exceptList1.Any() && !exceptList2.Any();
        }
    }


}
