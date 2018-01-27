using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TIPS.Entities.AdmissionEntities
{
    public class SMSTemplate
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string TemplateName { get; set; }
        [DataMember]
        public virtual string TemplateContent { get; set; }
        [DataMember]
        public virtual string CreatedDate { get; set; }
    }
}
