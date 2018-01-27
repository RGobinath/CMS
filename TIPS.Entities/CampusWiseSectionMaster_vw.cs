using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities
{
    public class CampusWiseSectionMaster_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long CampusWiseSectionMasterId { get; set; }
        [DataMember]
        public virtual long AcademicYearId { get; set; }
        [DataMember]
        public virtual long CampusId { get; set; }
        [DataMember]
        public virtual long GradeId { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string UpdateBy { get; set; }
        [DataMember]
        public virtual DateTime? UpdateDate { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
    }
}
