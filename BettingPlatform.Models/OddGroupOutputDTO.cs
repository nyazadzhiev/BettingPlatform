using BettingPlatform.DAL.Entities;

namespace BettingPlatform.Models
{
    public class OddGroupOutputDTO
    {
        public double SpecialValue { get; set; }
        public List<Odd> ActiveOdds { get; set; }
    }
}
