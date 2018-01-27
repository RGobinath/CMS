using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    public class SummativeMarkAnalysisVIVIII_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string ntytohund { get; set; }
        [DataMember]
        public virtual string etytonty { get; set; }
        [DataMember]
        public virtual string svtytoety { get; set; }
        [DataMember]
        public virtual string sxtytosvty { get; set; }
        [DataMember]
        public virtual string ftytosxty { get; set; }
        [DataMember]
        public virtual string frtytofty { get; set; }
        [DataMember]
        public virtual string blwfrty { get; set; }
        [DataMember]
        public virtual string semester { get; set; }
        [DataMember]
        public virtual long TotalStudents { get; set; }
    }
}
