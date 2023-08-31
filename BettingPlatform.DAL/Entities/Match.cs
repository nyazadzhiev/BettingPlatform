using Newtonsoft.Json;
using System.Xml.Serialization;

namespace BettingPlatform.DAL.Entities
{
    [XmlRoot(ElementName = "Match")]
    public class Match : BaseEntity
    {

        [XmlElement(ElementName = "Bet")]
        public virtual List<Bet> Bet { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "StartDate")]
        public DateTime StartDate { get; set; }

        [XmlAttribute(AttributeName = "MatchType")]
        public string MatchType { get; set; }
    }

}
