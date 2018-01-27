using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TIPS.Entities.AdmissionEntities
{
    public class SMS
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string PreRegNo { get; set; }
        [DataMember]
        public virtual string SuccessSMSNos { get; set; }
        [DataMember]
        public virtual string FailedSMSNos { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string Message { get; set; }
        [DataMember]
        public virtual string CreatedDate { get; set; }
        [DataMember]
        public virtual bool Father { get; set; }
        [DataMember]
        public virtual bool Mother { get; set; }
        [DataMember]
        public virtual bool General { get; set; }
        [DataMember]
        public virtual string SMSTemplate { get; set; }
        [DataMember]
        public virtual string SMSTemplate1 { get; set; }
        [DataMember]
        public virtual string TemplateTerm { get; set; }
        [DataMember]
        public virtual string TemplateDate { get; set; }
        [DataMember]
        public virtual string Flag { get; set; }
        [DataMember]
        public virtual string url { get; set; }
        [DataMember]
        public virtual string Reason { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string StudName { get; set; }
    }
}
