using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffManagementEntities
{
    [DataContract]
    public class StaffExperience
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int32 PreRegNum { get; set; }
        [DataMember]
        public virtual string EmployerName { get; set; }
        [DataMember]
        public virtual string Location { get; set; }
        [DataMember]
        public virtual string StartDate { get; set; }
        [DataMember]
        public virtual string TillDate { get; set; }
        [DataMember]
        public virtual string LastDesignation { get; set; }
        [DataMember]
        public virtual string SpecificReasonForLeaving { get; set; }

        [DataMember]
        public virtual string Achievments { get; set; }

        //Newly Added
        [DataMember]
        public virtual string TotalYearsOfExp { get; set; }
        [DataMember]
        public virtual string TotalYearsOfTeachingExp { get; set; }
        [DataMember]
        public virtual string SyllabusHandled { get; set; }
        [DataMember]
        public virtual string SubjectsTaught { get; set; }
        [DataMember]
        public virtual string GradesTaught { get; set; }
        [DataMember]
        public virtual string Fresher { get; set; }
        [DataMember]
        public virtual string JobResponsibilities { get; set; }


    }
}
