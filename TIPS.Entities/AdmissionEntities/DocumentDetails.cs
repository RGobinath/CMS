using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AdmissionEntities
{
    public class DocumentDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long StudentId { get; set; }
        [DataMember]
        public virtual string DocumentType { get; set; }
        [DataMember]
        public virtual string DocumentName { get; set; }

        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Phone { get; set; }
    }
}
