using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TIPS.Entities.AdmissionEntities;

namespace TIPS.Entities.TransportEntities
{
    public class RouteStudentListConfigView
    {
        public virtual long Id { get; set; }
        public virtual string RouteStudCode { get; set; }
        public virtual long RouteId { get; set; }
        public virtual long LocationId { get; set; }
        public virtual long PreRegNum { get; set; }
        //public virtual DateTime DateCreated { get; set; }
        //public virtual string CreatedBy { get; set; }
        public virtual string Name { get; set; }

        public virtual IList<RouteStudentListConfigView> StudentDetailsList { get; set; }
        public virtual IList<StudentTemplateView> StudTempList { get; set; }
        public virtual string LocationName { get; set; }
    }
}
