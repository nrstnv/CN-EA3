using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Aufgabe2
{
    /// <summary>
    /// 
    /// </summary>    
    [Serializable()]
    [XmlRoot]
    [DataContract]
    public class Warrior
    {
        [XmlElement]
        [DataMember]
        public string Name { get; set; }

        [XmlArray]
        [DataMember]
        public Weapon[] Weapons { get; set; }
    }
}
