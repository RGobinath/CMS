using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.TimeTableEntities
{
    public class tt_timetable
    {
        [DataMember]
        public virtual long timetable_id { get; set; }
        [DataMember]
        public virtual long allotment_id { get; set; }
        [DataMember]
        public virtual long teacher_id { get; set; }
        [DataMember]
        public virtual long subject_id { get; set; }
        [DataMember]
        public virtual long division_id { get; set; }
        [DataMember]
        public virtual string lab_division_id { get; set; }
        [DataMember]
        public virtual long day_id { get; set; }
        [DataMember]
        public virtual string combined_master_id { get; set; }
        [DataMember]
        public virtual string bifurcation_master_id { get; set; }
        [DataMember]
        public virtual Int32 period_no { get; set; }
        [DataMember]
        public virtual bool block { get; set; }
        //[DataMember]
        //public virtual bool lock { get; set; }
        [DataMember]
        public virtual string description { get; set; }
        [DataMember]
        public virtual DateTime start_date { get; set; }
        [DataMember]
        public virtual DateTime end_date { get; set; }


        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string ClassCharge { get; set; }
        [DataMember]
        public virtual IList<tt_SubjectTeacherPeriodCount> SubjTecherList { get; set; }
        [DataMember]
        public virtual IList<TeachersTimeTable> TeachersTimeTableList { get; set; }
        [DataMember]
        public virtual string teacher { get; set; }
        [DataMember]
        public virtual string SubjectColor { get; set; }
        [DataMember]
        public virtual string background_color { get; set; }

    }
}
