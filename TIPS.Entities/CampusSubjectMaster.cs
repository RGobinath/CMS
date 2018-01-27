using System.Collections.Generic; 
using System.Text; 
using System; 


namespace TIPS.Entities {
    
    public class CampusSubjectMaster {
        public virtual int Id { get; set; }
        public virtual string SubjectName { get; set; }
        public virtual string Description { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual bool   IsAcademicSubject { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
