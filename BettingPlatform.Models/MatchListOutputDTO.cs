using BettingPlatform.DAL.Entities;

namespace BettingPlatform.Models
{
    public class MatchListOutputDTO
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public List<BetOutputDTO> Bets { get; set; } 
    }
}
