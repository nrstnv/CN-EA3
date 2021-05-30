using System.Xml.Serialization;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;
using System;

namespace Aufgabe2
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable()]
    public class Weapon
    {
        [XmlElement]
        [DataMember]
        public string Type { get; set; }

        [XmlElement]
        [JsonConverter(typeof(StringEnumConverter))]
        public WeaponRange Range { get; set; }
    }
}
