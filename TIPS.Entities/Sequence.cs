using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities
{
    public class Sequence
    {
        public virtual long Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual Int64 Value { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
