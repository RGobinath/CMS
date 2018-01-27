using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace TIPS.Entities.AdmissionEntities
{
    public class PreRegDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual int PreRegNum { get; set; }
        [DataMember]
        public virtual DateTime Date { get; set; }
        [DataMember]
        public virtual string ProcessedBy { get; set; }
        [DataMember]
        public virtual string UserRole { get; set; }
        [DataMember]
        public virtual string RegistrationStatus { get; set; }
    }
}
