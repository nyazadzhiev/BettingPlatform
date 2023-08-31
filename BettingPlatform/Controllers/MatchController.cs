using BettingPlatform.BLL.Contracts;
using BettingPlatform.DAL.Entities;
using BettingPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BettingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IRepository<DAL.Entities.Match> _matchRepository;
        public MatchController(IRepository<DAL.Entities.Match> matchRepository)
        {
            _matchRepository = matchRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetMatches()
        {
            var matches = await _matchRepository.GetMatchesAsync();

            List<MatchListOutputDTO> result = new List<MatchListOutputDTO>();

            foreach (DAL.Entities.Match matchItem in matches)
            {
                MatchListOutputDTO dto = new MatchListOutputDTO();
                dto.Name = matchItem.Name;
                dto.StartDate = matchItem.StartDate;

                dto.Bets = matchItem.Bet
                    .Where(bet => bet.IsLive &&
                        (bet.Name == "Match Winner" || bet.Name == "Map Advantage" || bet.Name == "Total Maps Played"))
                    .Select(bet => new BetOutputDTO
                    {
                        Name = bet.Name,
                        ID = bet.ID,
                        IsLive = bet.IsLive,
                        Odds = bet.Odd
                            .Where(odd => odd.SpecialBetValue != 0)
                            .GroupBy(odd => odd.SpecialBetValue)
                            .Select(group => new OddGroupOutputDTO
                            {
                                SpecialValue = group.Key,
                                ActiveOdds = group.ToList()
                            })
                            .ToList()
                    })
                    .ToList();

                result.Add(dto);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatch(string id)
        {
            var match = await _matchRepository.GetById(id);

            if (match == null)
            {
                return NotFound();
            }

            MatchListOutputDTO dto = new MatchListOutputDTO();
            dto.Name = match.Name;
            dto.StartDate = match.StartDate;
            dto.Bets = match.Bet
                .Select(bet => new BetOutputDTO
                {
                    Name = bet.Name,
                    ID = bet.ID,
                    IsLive = bet.IsLive,
                    Odds = bet.Odd
                        .Select(odd => new OddGroupOutputDTO
                        {
                            SpecialValue = odd.SpecialBetValue,
                            ActiveOdds = new List<Odd> { odd }
                        })
                        .ToList()
                })
                .ToList();

            return Ok(dto);
        }
    }
}
