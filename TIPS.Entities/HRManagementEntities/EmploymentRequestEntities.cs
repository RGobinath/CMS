using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.HRManagementEntities
{
    [DataContract]
    public class EmploymentRequestEntities
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string IssuNo { get; set; }
        [DataMember]
        public virtual string Date { get; set; }
        [DataMember]
        public virtual string Time { get; set; }
        [DataMember]
        public virtual string ProcessedBy { get; set; }
        [DataMember]
        public virtual string UserRole { get; set; }
        [DataMember]
        public virtual string RequestStatus { get; set; }
        [DataMember]
        public virtual string NameOfCanditate { get; set; }
        [DataMember]
        public virtual string Gender { get; set; }
        [DataMember]
        public virtual string DateOfBirth { get; set; }
        [DataMember]
        public virtual string Qualification { get; set; }
        [DataMember]
        public virtual string Experience { get; set; }
        [DataMember]
        public virtual string PostApplyingFor { get; set; }
        [DataMember]
        public virtual string RefferedBy { get; set; }
        [DataMember]
        public virtual string PresentlyWorking { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        [DataMember]
        public virtual string FileType { get; set; }
        
  

        [DataMember]
        public virtual string Year { get; set; }
        [DataMember]
        public virtual string Month { get; set; }
        [DataMember]
        public virtual string RequestNumber { get; set; }

        //[DataMember]
        //public virtual string UploadedBy { get; set; }


        //[DataMember]
        //public virtual string UploadedOn { get; set; }

 


    }
}
