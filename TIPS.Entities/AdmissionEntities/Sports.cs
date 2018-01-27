using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AdmissionEntities
{
    public class Sports
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string acady { get; set; }
        [DataMember]
        public virtual string Event { get; set; }
        [DataMember]
        public virtual string Award { get; set; }
        [DataMember]
        public virtual string Date { get; set; }
        [DataMember]
        public virtual string Place { get; set; }
        [DataMember]
        public virtual int Preregno { get; set; }
        [DataMember]
        public virtual string Preregno1 { get; set; }
        [DataMember]
        public virtual int SportsId { get; set; }
        [DataMember]
        public virtual string CreatedDate { get; set; }
        [DataMember]
        public virtual string Sport { get; set; }
    }
}
