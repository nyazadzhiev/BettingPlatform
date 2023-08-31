using System.Xml.Serialization;

namespace BettingPlatform.DAL.Entities
{
    [XmlRoot(ElementName = "Bet")]
    public class Bet : BaseEntity
    {

        [XmlElement(ElementName = "Odd")]
        public virtual List<Odd> Odd { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "IsLive")]
        public bool IsLive { get; set; }
    }


}
