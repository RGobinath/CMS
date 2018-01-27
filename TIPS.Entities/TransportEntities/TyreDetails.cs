using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.TransportEntities {
    
    public class TyreDetails {
        public virtual int Id { get; set; }
        public virtual int? InvoiceId { get; set; }
        public virtual string TyreNo { get; set; }
        public virtual string Make { get; set; }
        public virtual string Model { get; set; }
        public virtual string Size { get; set; }
        public virtual string Type { get; set; }
        public virtual decimal? TubeCost { get; set; }
        public virtual decimal? TyreCost { get; set; }
        public virtual decimal? TotalCost { get; set; }
        public virtual bool IsAssigned { get; set; }
        public virtual string AssignedTo { get; set; }
    }
}
