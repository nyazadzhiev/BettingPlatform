using BettingPlatform.DAL.Entities;
using System.Xml.Serialization;

namespace BettingPlatform.Models
{
    public class BetOutputDTO : BaseEntity
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public bool IsLive { get; set; }
        public List<OddGroupOutputDTO> Odds { get; set; }
    }
}
