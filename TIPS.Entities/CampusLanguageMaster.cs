using System.Collections.Generic; 
using System.Text; 
using System; 


namespace TIPS.Entities {
    
    public class CampusLanguageMaster {
        public virtual int Id { get; set; }
        public virtual string LanguageName { get; set; }
        public virtual string Description { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
    }
}
