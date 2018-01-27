using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.AdmissionEntities
{
    public class TCRequestDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string ApplicationNo { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Initial { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string TransferedDate { get; set; }
        [DataMember]
        public virtual string AdmittedClass { get; set; }
        [DataMember]
        public virtual string AdmissionDate { get; set; }
        [DataMember]
        public virtual string QualifiedForPromotion { get; set; }
        [DataMember]
        public virtual string LastDateOfAttendance { get; set; }
        [DataMember]
        public virtual string ReasonForLeaving { get; set; }
        [DataMember]
        public virtual bool IsOtherReason { get; set; }
        [DataMember]
        public virtual string ReasonForTCRequest { get; set; }
        [DataMember]
        public virtual string Conduct { get; set; }

        ///Newly Added 
        ///For TC Reques
        [DataMember]
        public virtual string TCRequestedDate { get; set; }
        [DataMember]
        public virtual string TCRequiredOnDate { get; set; }
        [DataMember]
        public virtual bool IsNoDueForm { get; set; }
        [DataMember]
        public virtual bool IsCounselingNeeded { get; set; }
        [DataMember]
        public virtual string CounselingDate { get; set; }
        [DataMember]
        public virtual string NoDueFormDate { get; set; }
        [DataMember]
        public virtual string CoordComments { get; set; }
        [DataMember]
        public virtual string ADMCordComments { get; set; }
        [DataMember]
        public virtual string RCordComments { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }

        [DataMember]
        public virtual string DocumentType { get; set; }
        [DataMember]
        public virtual DateTime LastAttendanceDateCopy { get; set; }

    }
}
