using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class ReportCardCBSECo_ScholasticLifeSkills
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long RptId { get; set; }
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
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Term { get; set; }
        [DataMember]
        public virtual string LS_SA_1 { get; set; }
        [DataMember]
        public virtual string LS_SA_2 { get; set; }
        [DataMember]
        public virtual string LS_SA_3 { get; set; }
        [DataMember]
        public virtual string LS_SA_4 { get; set; }
        [DataMember]
        public virtual string LS_SA_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_SA_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_SA_Average { get; set; }
        [DataMember]
        public virtual string LS_SA_Grade { get; set; }
        [DataMember]
        public virtual string LS_PS_1 { get; set; }
        [DataMember]
        public virtual string LS_PS_2 { get; set; }
        [DataMember]
        public virtual string LS_PS_3 { get; set; }
        [DataMember]
        public virtual string LS_PS_4 { get; set; }
        [DataMember]
        public virtual string LS_PS_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_PS_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_PS_Average { get; set; }
        [DataMember]
        public virtual string LS_PS_Grade { get; set; }
        [DataMember]
        public virtual string LS_DM_1 { get; set; }
        [DataMember]
        public virtual string LS_DM_2 { get; set; }
        [DataMember]
        public virtual string LS_DM_3 { get; set; }
        [DataMember]
        public virtual string LS_DM_4 { get; set; }
        [DataMember]
        public virtual string LS_DM_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_DM_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_DM_Average { get; set; }
        [DataMember]
        public virtual string LS_DM_Grade { get; set; }
        [DataMember]
        public virtual string LS_CriT_1 { get; set; }
        [DataMember]
        public virtual string LS_CriT_2 { get; set; }
        [DataMember]
        public virtual string LS_CriT_3 { get; set; }
        [DataMember]
        public virtual string LS_CriT_4 { get; set; }
        [DataMember]
        public virtual string LS_CriT_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_CriT_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_CriT_Average { get; set; }
        [DataMember]
        public virtual string LS_CriT_Grade { get; set; }
        [DataMember]
        public virtual string LS_CreT_1 { get; set; }
        [DataMember]
        public virtual string LS_CreT_2 { get; set; }
        [DataMember]
        public virtual string LS_CreT_3 { get; set; }
        [DataMember]
        public virtual string LS_CreT_4 { get; set; }
        [DataMember]
        public virtual string LS_CreT_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_CreT_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_CreT_Average { get; set; }
        [DataMember]
        public virtual string LS_CreT_Grade { get; set; }
        [DataMember]
        public virtual string LS_IR_1 { get; set; }
        [DataMember]
        public virtual string LS_IR_2 { get; set; }
        [DataMember]
        public virtual string LS_IR_3 { get; set; }
        [DataMember]
        public virtual string LS_IR_4 { get; set; }
        [DataMember]
        public virtual string LS_IR_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_IR_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_IR_Average { get; set; }
        [DataMember]
        public virtual string LS_IR_Grade { get; set; }
        [DataMember]
        public virtual string LS_EC_1 { get; set; }
        [DataMember]
        public virtual string LS_EC_2 { get; set; }
        [DataMember]
        public virtual string LS_EC_3 { get; set; }
        [DataMember]
        public virtual string LS_EC_4 { get; set; }
        [DataMember]
        public virtual string LS_EC_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_EC_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_EC_Average { get; set; }
        [DataMember]
        public virtual string LS_EC_Grade { get; set; }
        [DataMember]
        public virtual string LS_Emp_1 { get; set; }
        [DataMember]
        public virtual string LS_Emp_2 { get; set; }
        [DataMember]
        public virtual string LS_Emp_3 { get; set; }
        [DataMember]
        public virtual string LS_Emp_4 { get; set; }
        [DataMember]
        public virtual string LS_Emp_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_Emp_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_Emp_Average { get; set; }
        [DataMember]
        public virtual string LS_Emp_Grade { get; set; }
        [DataMember]
        public virtual string LS_ME_1 { get; set; }
        [DataMember]
        public virtual string LS_ME_2 { get; set; }
        [DataMember]
        public virtual string LS_ME_3 { get; set; }
        [DataMember]
        public virtual string LS_ME_4 { get; set; }
        [DataMember]
        public virtual string LS_ME_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_ME_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_ME_Average { get; set; }
        [DataMember]
        public virtual string LS_ME_Grade { get; set; }
        [DataMember]
        public virtual string LS_MwthS_1 { get; set; }
        [DataMember]
        public virtual string LS_MwthS_2 { get; set; }
        [DataMember]
        public virtual string LS_MwthS_3 { get; set; }
        [DataMember]
        public virtual string LS_MwthS_4 { get; set; }
        [DataMember]
        public virtual string LS_MwthS_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_MwthS_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_MwthS_Average { get; set; }
        [DataMember]
        public virtual string LS_MwthS_Grade { get; set; }
        /// <summary>
        /// Created details and Modified details
        /// </summary>
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
    }
}
