using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class ReportCardCBSEView
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long RptRequestId { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual decimal FA1ASlip { get; set; }
        [DataMember]
        public virtual decimal FA1ASlipTotal { get; set; }
        [DataMember]
        public virtual decimal FA1BTotal { get; set; }
        [DataMember]
        public virtual decimal FA1CTotal { get; set; }
        [DataMember]
        public virtual decimal FA1DTotal { get; set; }
        [DataMember]
        public virtual decimal FA1Total { get; set; }
        [DataMember]
        public virtual decimal FA1 { get; set; }
        [DataMember]
        public virtual string FA1Grade { get; set; }
        [DataMember]
        public virtual decimal FA2ASlip { get; set; }
        [DataMember]
        public virtual decimal FA2ASlipTotal { get; set; }
        [DataMember]
        public virtual decimal FA2BTotal { get; set; }
        [DataMember]
        public virtual decimal FA2CTotal { get; set; }
        [DataMember]
        public virtual decimal FA2DTotal { get; set; }
        [DataMember]
        public virtual decimal FA2Total { get; set; }
        [DataMember]
        public virtual decimal FA2 { get; set; }
        [DataMember]
        public virtual string FA2Grade { get; set; }
        [DataMember]
        public virtual decimal SA1 { get; set; }
        [DataMember]
        public virtual decimal SA1Total { get; set; }
        [DataMember]
        public virtual decimal Term1Total { get; set; }
        [DataMember]
        public virtual decimal FA3ASlip { get; set; }
        [DataMember]
        public virtual decimal FA3ASlipTotal { get; set; }
        [DataMember]
        public virtual decimal FA3BTotal { get; set; }
        [DataMember]
        public virtual decimal FA3CTotal { get; set; }
        [DataMember]
        public virtual decimal FA3DTotal { get; set; }
        [DataMember]
        public virtual decimal FA3Total { get; set; }
        [DataMember]
        public virtual decimal FA3 { get; set; }
        [DataMember]
        public virtual string FA3Grade { get; set; }
        [DataMember]
        public virtual decimal FA4ASlip { get; set; }
        [DataMember]
        public virtual decimal FA4ASlipTotal { get; set; }
        [DataMember]
        public virtual decimal FA4BTotal { get; set; }
        [DataMember]
        public virtual decimal FA4CTotal { get; set; }
        [DataMember]
        public virtual decimal FA4DTotal { get; set; }
        [DataMember]
        public virtual decimal FA4Total { get; set; }
        [DataMember]
        public virtual decimal FA4 { get; set; }
        [DataMember]
        public virtual string FA4Grade { get; set; }
        [DataMember]
        public virtual decimal SA2 { get; set; }
        [DataMember]
        public virtual decimal SA2Total { get; set; }
        [DataMember]
        public virtual decimal Term2Total { get; set; }
        
        //only Properties which doesn't store in DB

        [DataMember]
        public virtual string Language { get; set; }
    }
}
