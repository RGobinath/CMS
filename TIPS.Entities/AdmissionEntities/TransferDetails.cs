using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TIPS.Entities.AdmissionEntities
{
    public class TransferDetails
    {
        [DataMember]
        public virtual long Id { get; set; }

        [DataMember]
        public virtual string Type { get; set; }

        [DataMember]
        public virtual string ApplicationNo { get; set; }

        [DataMember]
        public virtual long PreRegNum { get; set; }

        //[Required]
        [DataMember]
        public virtual string Name { get; set; }

        //[Required]
        [DataMember]
        public virtual string Gender { get; set; }

        [DataMember]
        public virtual string FeeStructYear { get; set; }
        [DataMember]
        public virtual string BeforeId { get; set; }
        [DataMember]
        public virtual string AfterId { get; set; }

        [DataMember]
        public virtual string BeforeCampus { get; set; }
        [DataMember]
        public virtual string AfterCampus { get; set; }

        [DataMember]
        public virtual string BeforeSection { get; set; }
        [DataMember]
        public virtual string AfterSection { get; set; }

        [DataMember]
        public virtual string BeforeGrade { get; set; }
        [DataMember]
        public virtual string AfterGrade { get; set; }

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
        public virtual string Conduct { get; set; }
    }
}
