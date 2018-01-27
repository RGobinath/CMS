using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class SubjectTeacherDetails
    {
        [DataMember]
        public virtual string TeacherName { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual long NoOfPrds { get; set; }
        [DataMember]
        public virtual long RemainingPrds { get; set; }
    }
}
