using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    [DataContract]
   public class AdmissionManagement
    {
        [DataMember]
        public virtual long ApplicationNo { get; set; }
        [DataMember]
        public virtual string PreRegNo { get; set; }
        [DataMember]
        public virtual string ApplicantName { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual DateTime? AppliedDate { get; set; }
       
    }
}
