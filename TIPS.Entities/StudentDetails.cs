using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    public class StudentDetails
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string id_no { get; set; }
        [DataMember]
        public virtual string name { get; set; }
        [DataMember]
        public virtual string campus_name { get; set; }
        [DataMember]
        public virtual string section { get; set; }
        [DataMember]
        public virtual string grade { get; set; }  
        [DataMember]
        public virtual bool IsHosteller { get; set; }
        [DataMember]
        public virtual string application_no { get; set; }
        [DataMember]
        public virtual string academicyear { get; set; }
        [DataMember]
        public virtual string feestructure { get; set; }
        [DataMember]
        public virtual string father_name { get; set; }
        [DataMember]
        public virtual Int64 father_mobile { get; set; }
        [DataMember]
        public virtual string mother_name { get; set; }
        [DataMember]
        public virtual Int64 mother_mobile { get; set; }
        [DataMember]
        public virtual string guardian_name { get; set; }
        [DataMember]
        public virtual Int64 guardian_mobile { get; set; }

        [DataMember]
        public virtual string BoardingType { get; set; }
        
    }
}
