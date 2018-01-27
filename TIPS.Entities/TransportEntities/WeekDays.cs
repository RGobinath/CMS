using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GEMS.Entities.TimeTable
{
    public class WeekDays
    {
        [DataMember]
        public virtual long WeekDay_Id { get; set; }
        [DataMember]
        public virtual string WeekDay_Code { get; set; }
        [DataMember]
        public virtual string WeekDayName { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
