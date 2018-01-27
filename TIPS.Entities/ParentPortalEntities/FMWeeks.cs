using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.ParentPortalEntities
{
    [DataContract]
    public class FMWeeks
    {
        [DataMember]
        public virtual Int64 Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Month { get; set; }
        [DataMember]
        public virtual string Notes { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
        [DataMember]
        public virtual DateTime CreatedOn { get; set; }
       [DataMember]
        public virtual string AcademicYear { get; set; }

        [DataMember]
        public virtual IList<FMDays> FMDaysList { get; set; }

    }
}
