using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AdmissionEntities
{
    public class PastSchoolDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long StudentId { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string FromDate { get; set; }
        [DataMember]
        public virtual string ToDate { get; set; }
        [DataMember]
        public virtual string SchoolName { get; set; }
        [DataMember]
        public virtual string City { get; set; }
        [DataMember]
        public virtual string FromGrade { get; set; }
        [DataMember]
        public virtual string ToGrade { get; set; }
    }
}
