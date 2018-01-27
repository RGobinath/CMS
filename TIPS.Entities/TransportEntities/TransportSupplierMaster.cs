using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class TransportSupplierMaster
    {
        public virtual long SupplierId { get; set; }
        public virtual string SupplierName { get; set; }
        public virtual string SupplierType { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
