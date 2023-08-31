using Newtonsoft.Json;
using System.Xml.Serialization;

namespace BettingPlatform.DAL.Entities
{
    [XmlRoot(ElementName = "XmlSports")]
    public class XmlSports
    {

        [XmlElement(ElementName = "Sport")]
        public Sport Sport { get; set; }

        [XmlAttribute(AttributeName = "xsd")]
        public string Xsd { get; set; }

        [XmlAttribute(AttributeName = "xsi")]
        public string Xsi { get; set; }

        [XmlAttribute(AttributeName = "CreateDate")]
        public DateTime CreateDate { get; set; }
    }

}
