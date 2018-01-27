using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
 public   class StudentLaptopDistribution_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long studentId { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string AssetCode { get; set; }
        [DataMember]
        public virtual string TransactionType { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ReceivedAcademicYr { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string AdmissionStatus { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string SpecificationsDetails { get; set; }

        [DataMember]
        public virtual string ReceivedCampus { get; set; }
        [DataMember]
        public virtual string ReceivedGrade { get; set; }
        [DataMember]
        public virtual DateTime? ReceivedDate { get; set; }
        [DataMember]
        public virtual string LTSize { get; set; }
        [DataMember]
        public virtual string OperatingSystemDtls { get; set; }

       

     
    }
}
