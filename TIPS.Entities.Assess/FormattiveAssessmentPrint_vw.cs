using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class FormattiveAssessmentPrint_vw
    {
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string IdNo { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual IList<FormattiveAssessment_Vw> FAMarkList { get; set; }
        [DataMember]
        public virtual IList<FA_HW_Vw> FAHWMarkList { get; set; }
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
