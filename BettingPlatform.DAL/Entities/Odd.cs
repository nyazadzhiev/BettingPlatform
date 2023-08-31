using Newtonsoft.Json;
using System.Xml.Serialization;

namespace BettingPlatform.DAL.Entities
{
    [XmlRoot(ElementName = "Odd")]
    public class Odd : BaseEntity
    {

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "Value")]
        public double Value { get; set; }

        [XmlAttribute(AttributeName = "SpecialBetValue")]
        public double SpecialBetValue { get; set; }
    }
}
