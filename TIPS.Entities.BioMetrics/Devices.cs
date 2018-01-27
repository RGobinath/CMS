
namespace TIPS.Entities.BioMetrics
{
    public class Devices
    {
        public virtual long DeviceId { get; set; }
        public virtual string DeviceFName { get; set; }
        public virtual string DeviceSName { get; set; }
        public virtual long IsRealTime { get; set; }
    }
}
