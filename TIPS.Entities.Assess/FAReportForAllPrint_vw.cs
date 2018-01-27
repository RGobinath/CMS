using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class FAReportForAllPrint_vw
    {
        public virtual IList<Assess360> FAMarkNameList { get; set; }
        public virtual IList<FormattiveAssessment_Vw> FAMarkList { get; set; }
        ///<summary>
        /// For PDF
        ///</summary>
        [DataMember]
        public virtual string TipsLogo { get; set; }
        [DataMember]
        public virtual string TipsNaceLogo { get; set; }
        [DataMember]
        public virtual string TipsName { get; set; }
        [DataMember]
        public virtual string FileName { get; set; }
        [DataMember]
        public virtual string Exam { get; set; }
    }
}
