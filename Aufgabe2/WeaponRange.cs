using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Aufgabe2
{
    [DataContract]
    public enum WeaponRange
    {
        [JsonProperty]
        Short,
        [JsonProperty]
        Middle,
        [JsonProperty]
        Long
    }
}
