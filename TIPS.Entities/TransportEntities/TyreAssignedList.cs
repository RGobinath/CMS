using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.TransportEntities {
    
    public class TyreAssignedList {
        public virtual int Id { get; set; }
        public virtual int? VehicleId { get; set; }
        public virtual int? TyreId { get; set; }
        public virtual string TyreNo { get; set; }
        public virtual string AssignedDate { get; set; }
    }
}
