using System.Xml.Serialization;

namespace BettingPlatform.DAL.Entities
{
    [XmlRoot(ElementName = "Event")]
    public class Event : BaseEntity
    {

        [XmlElement(ElementName = "Match")]
        public virtual List<Match> Match { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "IsLive")]
        public bool IsLive { get; set; }

        [XmlAttribute(AttributeName = "CategoryID")]
        public string CategoryID { get; set; }
    }

}
