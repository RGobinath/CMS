

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.AdmissionEntities
{
    public class StudentProfilePrint
    {
        
        public virtual string Campus { get; set; }
        public virtual string Section { get; set; }
        public virtual string Grade { get; set; }
        public virtual string AcademicYear { get; set; }
        /// <summary>
        /// StudentList For Profile printing
        /// </summary>
        public IList<StudentTemplateView> stdDtls { get; set; }
    }
}
