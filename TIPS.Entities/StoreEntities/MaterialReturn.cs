using System;
using System.Text;
using System.Collections.Generic;


namespace TIPS.Entities.StoreEntities
{
    public class MaterialReturn {
        public virtual int MatRetId { get; set; }
        public virtual string ReturnRefNum { get; set; }
        public virtual string Campus { get; set; }
        public virtual string FromStore { get; set; }
        public virtual string ToStore { get; set; }
        public virtual string DCNumber { get; set; }
        public virtual string Description { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ReturnStatus { get; set; }
        public virtual string AcceptedStatus { get; set; }
    }
}
