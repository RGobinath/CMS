using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TIPS.Entities.AdmissionEntities
{
    public class FamilyDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
   
        [DataMember]
        public virtual long StudentId { get; set; }
        [DataMember]
        public virtual string FamilyDetailType { get; set; }  //Father or Mother or Guardian

        [DataMember]
        public virtual long PreRegNum { get; set; }

        //[Required]
        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual string Education { get; set; }
        [DataMember]
        public virtual string Mobile { get; set; }
        [DataMember]
        public virtual string EmpType { get; set; }
        [DataMember]
        public virtual int Age { get; set; }
        [DataMember]
        public virtual string Email { get; set; }
        [DataMember]
        public virtual string TestEmail { get; set; }
        [DataMember]
        public virtual string Occupation { get; set; }
        [DataMember]
        public virtual string CompName { get; set; }
        [DataMember]
        public virtual string CompAddress { get; set; }
        [DataMember]
        public virtual bool StayingWithChild { get; set; }
        [DataMember]
        public virtual bool TransportReq { get; set; } 

    }
}
