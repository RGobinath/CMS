using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.TransportEntities {
    
    public class VehicleDistanceFuelReport_vw {
        public virtual int Id { get; set; }
        public virtual int VehicleId { get; set; }
        public virtual string Type { get; set; }
        public virtual string VehicleNo { get; set; }
        public virtual string Campus { get; set; }
        public virtual decimal? DistanceCovered { get; set; }
        public virtual decimal? FuelConsumed { get; set; }
        public virtual decimal? Mileage { get; set; }
        public virtual int? Month { get; set; }
        public virtual int? Year { get; set; }
        
    }
}
