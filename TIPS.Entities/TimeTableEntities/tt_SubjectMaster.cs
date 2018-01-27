using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TIPS.Entities.TimeTableEntities
{
    public class tt_SubjectMaster
    {
        [DataMember]
        public virtual long subject_id { get; set; }
        [DataMember]
        public virtual string subject_code { get; set; }
        [DataMember]
        public virtual string subject_name { get; set; }
        [DataMember]
        public virtual string priority { get; set; }
        [DataMember]
        public virtual string subject_colour { get; set; }
        [DataMember]
        public virtual string description { get; set; }
        [DataMember]
        public virtual string background_color { get; set; }
    }
}
