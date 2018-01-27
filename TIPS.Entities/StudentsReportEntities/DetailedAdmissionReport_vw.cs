using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StudentsReportEntities
{
    public class DetailedAdmissionReport_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string AdmissionStatus { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual long Count { get; set; }
        [DataMember]
        public virtual string CreatedDate { get; set; }

        [DataMember]
        public virtual long DeclinedCnt { get; set; }
        [DataMember]
        public virtual long DeletedCnt { get; set; }
        [DataMember]
        public virtual long DiscontinuedCnt { get; set; }
        [DataMember]
        public virtual long InactiveCnt { get; set; }

        [DataMember]
        public virtual long NewEnquiryCnt { get; set; }
        [DataMember]
        public virtual long NewRegistrationCnt { get; set; }
        [DataMember]
        public virtual long NotInterestedCnt { get; set; }
        [DataMember]
        public virtual long NotJoinedCnt { get; set; }

        [DataMember]
        public virtual long RegisteredCnt { get; set; }
        [DataMember]
        public virtual long SentForApprovalCnt { get; set; }
        [DataMember]
        public virtual long SentForClearanceCnt { get; set; }
        [DataMember]
        public virtual DateTime FromDate { get; set; }
    }
}
