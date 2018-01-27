using System.Collections.Generic; 
using System.Text; 
using System; 


namespace TIPS.Entities.Assess {
    
    public class StaffMaster {
        public virtual int Id { get; set; }
        public virtual string StaffName { get; set; }
        public virtual string SubjectTeaching { get; set; }
        public virtual DateTime? DateJoined { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Campus { get; set; }
    }
}
