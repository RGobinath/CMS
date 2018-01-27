using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace TIPS.Entities.TimeTableEntities
{
    public class PeriodTimeMaster
    {
        [DataMember]
        public virtual long Periodtime_Id { get; set; }
        [DataMember]
        public virtual string Periodtime_Code { get; set; }
        [DataMember]
        public virtual long Period_Number { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Start_Time { get; set; }
        [DataMember]
        public virtual string End_Time { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }

    }
}
