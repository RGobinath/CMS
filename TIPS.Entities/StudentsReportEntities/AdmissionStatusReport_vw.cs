using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StudentsReportEntities
{
    [DataContract]
    public class AdmissionStatusReport_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual long NewEnquiry { get; set; }
        [DataMember]
        public virtual long NewRegistration { get; set; }
        [DataMember]
        public virtual long SentForClearance { get; set; }
        [DataMember]
        public virtual long NotInterested { get; set; }
        [DataMember]
        public virtual long SentForApproval { get; set; }
        [DataMember]
        public virtual long Registered { get; set; }
        [DataMember]
        public virtual long Discontinued { get; set; }
    }
}
