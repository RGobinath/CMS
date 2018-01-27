using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StudentsReportEntities
{
    [DataContract]
    public class MISReportMailReport
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string EmailId { get; set; }
        [DataMember]
        public virtual string RoleCode { get; set; }
        [DataMember]
        public virtual long IsMailSent { get; set; }
        [DataMember]
        public virtual DateTime MailDateTime { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
    }
}
