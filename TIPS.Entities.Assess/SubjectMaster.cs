using System.Collections.Generic; 
using System.Text; 
using System; 


namespace TIPS.Entities.Assess {
    
    public class SubjectMaster {
        public virtual int Id { get; set; }
        public virtual string SubjectName { get; set; }
        public virtual string Description { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
    }
}
