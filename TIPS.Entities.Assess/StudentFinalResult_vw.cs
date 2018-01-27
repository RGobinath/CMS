using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{[DataContract]
  public class StudentFinalResult_vw
    {
    [DataMember]
    public virtual long Id { get; set; }
    [DataMember]
    public virtual long PreRegNum { get; set; }
    [DataMember]
    public virtual string AcademicYear { get; set; }
    [DataMember]
    public virtual string NewId { get; set; }
    [DataMember]
    public virtual string Name { get; set; }
    [DataMember]
    public virtual string CreatedBy { get; set; }
    [DataMember]
    public virtual string EngSemI { get; set; }
    [DataMember]
    public virtual string EngSemII { get; set; }
    [DataMember]
    public virtual string EngTotal { get; set; }
    [DataMember]
    public virtual string EngGrade { get; set; }
    [DataMember]
    public virtual string LangSemI { get; set; }
    [DataMember]
    public virtual string LangSemII { get; set; }
    [DataMember]
    public virtual string LangTotal { get; set; }
    [DataMember]
    public virtual string LangGrade { get; set; }
    [DataMember]
    public virtual string HcSemI { get; set; }
    [DataMember]
    public virtual string HcSemII { get; set; }
    [DataMember]
    public virtual string HcTotal { get; set; }
    [DataMember]
    public virtual string HcGrade { get; set; }
    [DataMember]
    public virtual string MathsSemI { get; set; }
    [DataMember]
    public virtual string MathsSemII { get; set; }
    [DataMember]
    public virtual string MathsTotal { get; set; }
    [DataMember]
    public virtual string MathsGrade { get; set; }
    [DataMember]
    public virtual string BioSemI { get; set; }
    [DataMember]
    public virtual string BioSemII { get; set; }
    [DataMember]
    public virtual string BioTotal { get; set; }
    [DataMember]
    public virtual string BioGrade { get; set; }
    [DataMember]
    public virtual string PhySemI { get; set; }
    [DataMember]
    public virtual string PhySemII { get; set; }
    [DataMember]
    public virtual string PhyTotal { get; set; }
    [DataMember]
    public virtual string PhyGrade { get; set; }
    [DataMember]
    public virtual string CheSemI { get; set; }
    [DataMember]
    public virtual string CheSemII { get; set; }
    [DataMember]
    public virtual string CheTotal { get; set; }
    [DataMember]
    public virtual string CheGrade { get; set; }
    [DataMember]
    public virtual string IctSemI { get; set; }
    [DataMember]
    public virtual string IctSemII { get; set; }
    [DataMember]
    public virtual string IctTotal { get; set; }
    [DataMember]
    public virtual string IctGrade { get; set; }
    [DataMember]
    public virtual string GrandTotal { get; set; }
    [DataMember]
    public virtual string Percentage { get; set; }
    [DataMember]
    public virtual string OverallGrade { get; set; }

    [DataMember]
    public virtual string Campus { get; set; }
    [DataMember]
    public virtual string Grade { get; set; }
    [DataMember]
    public virtual string Section { get; set; }
    [DataMember]
    public virtual string Semester { get; set; }
    [DataMember]
    public virtual string Subject { get; set; }

    }
}
