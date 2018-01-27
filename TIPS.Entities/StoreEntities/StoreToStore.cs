using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.StoreEntities {
    
    public class StoreToStore
    {
        public virtual int Id { get; set; }
        public virtual string IssueNumber { get; set; }
        public virtual string FromStore { get; set; }
        public virtual string ToStore { get; set; }
        public virtual string DeliveredThrough { get; set; }
        public virtual string DeliveryDetails { get; set; }
        public virtual DateTime? DeliveryDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string Status { get; set; }
    }
}
