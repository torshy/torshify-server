using System.Runtime.Serialization;

namespace Torshify.Origo.Contracts.V1
{
    [DataContract(
        Name = "PlayerStatus",
        Namespace = "http://schemas.torshify/v1")]
    public class PlayerStatus : IExtensibleDataObject
    {
        [DataMember]
        public Track CurrentTrack { get; set; }
        [DataMember]
        public double ElapsedTime { get; set; }
        [DataMember]
        public double TotalTime { get; set; }
        [DataMember]
        public bool IsPlaying { get; set; }
        [DataMember]
        public float Volume { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}