using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace TIPS.Entities.AdmissionEntities
{
    public class ApproveAssign
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long StudentId { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string FeeStructYear { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string IdNumber { get; set; }
        [DataMember]
        public virtual string AssignSection { get; set; }
        [DataMember]
        public virtual string AlottedLabel { get; set; }
    }
}

