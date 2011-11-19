using System.Runtime.Serialization;

namespace Torshify.Origo.Contracts.V1
{
    [DataContract(
        Name = "PlayerStatus",
        Namespace = "http://schemas.torshify/v1")]
    public class PlayerStatus : IExtensibleDataObject
    {
        [DataMember(Name = "CurrentTrack", Order = 0)]
        public Track CurrentTrack { get; set; }
        [DataMember(Name = "ElapsedTime", Order = 1)]
        public double ElapsedTime { get; set; }
        [DataMember(Name = "TotalTime", Order = 2)]
        public double TotalTime { get; set; }
        [DataMember(Name = "IsPlaying", Order = 3)]
        public bool IsPlaying { get; set; }
        [DataMember(Name = "Volume", Order = 4)]
        public float Volume { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}