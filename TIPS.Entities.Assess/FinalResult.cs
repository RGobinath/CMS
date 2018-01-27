using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class FinalResult
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual long EngSemI { get; set; }
        [DataMember]
        public virtual long EngSemII { get; set; }
        [DataMember]
        public virtual string EngTotal { get; set; }
        [DataMember]
        public virtual string EngGrade { get; set; }
        [DataMember]
        public virtual long LangSemI { get; set; }
        [DataMember]
        public virtual long LangSemII { get; set; }
        [DataMember]
        public virtual string LangTotal { get; set; }
        [DataMember]
        public virtual string LangGrade { get; set; }
        [DataMember]
        public virtual long HcSemI { get; set; }
        [DataMember]
        public virtual long HcSemII { get; set; }
        [DataMember]
        public virtual string HcTotal { get; set; }
        [DataMember]
        public virtual string HcGrade { get; set; }
        [DataMember]
        public virtual long MathsSemI { get; set; }
        [DataMember]
        public virtual long MathsSemII { get; set; }
        [DataMember]
        public virtual string MathsTotal { get; set; }
        [DataMember]
        public virtual string MathsGrade { get; set; }
        [DataMember]
        public virtual long BioSemI { get; set; }
        [DataMember]
        public virtual long BioSemII { get; set; }
        [DataMember]
        public virtual string BioTotal { get; set; }
        [DataMember]
        public virtual string BioGrade { get; set; }
        [DataMember]
        public virtual long PhySemI { get; set; }
        [DataMember]
        public virtual long PhySemII { get; set; }
        [DataMember]
        public virtual string PhyTotal { get; set; }
        [DataMember]
        public virtual string PhyGrade { get; set; }
        [DataMember]
        public virtual long CheSemI { get; set; }
        [DataMember]
        public virtual long CheSemII { get; set; }
        [DataMember]
        public virtual string CheTotal { get; set; }
        [DataMember]
        public virtual string CheGrade { get; set; }
        [DataMember]
        public virtual long IctSemI { get; set; }
        [DataMember]
        public virtual long IctSemII { get; set; }
        [DataMember]
        public virtual string IctTotal { get; set; }
        [DataMember]
        public virtual string IctGrade { get; set; }
        [DataMember]
        public virtual string GrandTotal { get; set; }
        [DataMember]
        public virtual string Persentage { get; set; }
        [DataMember]
        public virtual string GradeResult { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }

    }
}
