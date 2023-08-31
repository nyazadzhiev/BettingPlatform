using Newtonsoft.Json;
using System.Xml.Serialization;

namespace BettingPlatform.DAL.Entities
{
    [XmlRoot(ElementName = "Sport")]
    public class Sport : BaseEntity
    {

        [XmlElement(ElementName = "Event")]
        public virtual List<Event> Event { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
    }

}
