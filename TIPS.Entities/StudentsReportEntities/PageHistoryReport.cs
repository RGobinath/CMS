using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.StudentsReportEntities
{
    public class PageHistoryReport
    {
        [DataMember]
        public virtual long PageHistoryReport_Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string ControllerName { get; set; }
        [DataMember]
        public virtual long ControllerHit { get; set; }
        [DataMember]
        public virtual string ActionName { get; set; }
        [DataMember]
        public virtual long ActionHit { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
