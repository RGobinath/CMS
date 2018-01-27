using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class ReportCardCBSECo_Scholastic
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
        public virtual decimal? LS_SA_1 { get; set; }
        [DataMember]
        public virtual decimal? LS_SA_2 { get; set; }
        [DataMember]
        public virtual decimal? LS_SA_3 { get; set; }
        [DataMember]
        public virtual decimal? LS_SA_4 { get; set; }
        [DataMember]
        public virtual decimal? LS_SA_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_SA_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_SA_Average { get; set; }
        [DataMember]
        public virtual string LS_SA_Grade { get; set; }
        [DataMember]
        public virtual decimal? LS_PS_1 { get; set; }
        [DataMember]
        public virtual decimal? LS_PS_2 { get; set; }
        [DataMember]
        public virtual decimal? LS_PS_3 { get; set; }
        [DataMember]
        public virtual decimal? LS_PS_4 { get; set; }
        [DataMember]
        public virtual decimal? LS_PS_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_PS_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_PS_Average { get; set; }
        [DataMember]
        public virtual string LS_PS_Grade { get; set; }
        [DataMember]
        public virtual decimal? LS_DM_1 { get; set; }
        [DataMember]
        public virtual decimal? LS_DM_2 { get; set; }
        [DataMember]
        public virtual decimal? LS_DM_3 { get; set; }
        [DataMember]
        public virtual decimal? LS_DM_4 { get; set; }
        [DataMember]
        public virtual decimal? LS_DM_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_DM_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_DM_Average { get; set; }
        [DataMember]
        public virtual string LS_DM_Grade { get; set; }
        [DataMember]
        public virtual decimal? LS_CriT_1 { get; set; }
        [DataMember]
        public virtual decimal? LS_CriT_2 { get; set; }
        [DataMember]
        public virtual decimal? LS_CriT_3 { get; set; }
        [DataMember]
        public virtual decimal? LS_CriT_4 { get; set; }
        [DataMember]
        public virtual decimal? LS_CriT_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_CriT_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_CriT_Average { get; set; }
        [DataMember]
        public virtual string LS_CriT_Grade { get; set; }
        [DataMember]
        public virtual decimal? LS_CreT_1 { get; set; }
        [DataMember]
        public virtual decimal? LS_CreT_2 { get; set; }
        [DataMember]
        public virtual decimal? LS_CreT_3 { get; set; }
        [DataMember]
        public virtual decimal? LS_CreT_4 { get; set; }
        [DataMember]
        public virtual decimal? LS_CreT_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_CreT_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_CreT_Average { get; set; }
        [DataMember]
        public virtual string LS_CreT_Grade { get; set; }
        [DataMember]
        public virtual decimal? LS_IR_1 { get; set; }
        [DataMember]
        public virtual decimal? LS_IR_2 { get; set; }
        [DataMember]
        public virtual decimal? LS_IR_3 { get; set; }
        [DataMember]
        public virtual decimal? LS_IR_4 { get; set; }
        [DataMember]
        public virtual decimal? LS_IR_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_IR_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_IR_Average { get; set; }
        [DataMember]
        public virtual string LS_IR_Grade { get; set; }
        [DataMember]
        public virtual decimal? LS_EC_1 { get; set; }
        [DataMember]
        public virtual decimal? LS_EC_2 { get; set; }
        [DataMember]
        public virtual decimal? LS_EC_3 { get; set; }
        [DataMember]
        public virtual decimal? LS_EC_4 { get; set; }
        [DataMember]
        public virtual decimal? LS_EC_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_EC_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_EC_Average { get; set; }
        [DataMember]
        public virtual string LS_EC_Grade { get; set; }
        [DataMember]
        public virtual decimal? LS_Emp_1 { get; set; }
        [DataMember]
        public virtual decimal? LS_Emp_2 { get; set; }
        [DataMember]
        public virtual decimal? LS_Emp_3 { get; set; }
        [DataMember]
        public virtual decimal? LS_Emp_4 { get; set; }
        [DataMember]
        public virtual decimal? LS_Emp_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_Emp_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_Emp_Average { get; set; }
        [DataMember]
        public virtual string LS_Emp_Grade { get; set; }
        [DataMember]
        public virtual decimal? LS_ME_1 { get; set; }
        [DataMember]
        public virtual decimal? LS_ME_2 { get; set; }
        [DataMember]
        public virtual decimal? LS_ME_3 { get; set; }
        [DataMember]
        public virtual decimal? LS_ME_4 { get; set; }
        [DataMember]
        public virtual decimal? LS_ME_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_ME_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_ME_Average { get; set; }
        [DataMember]
        public virtual string LS_ME_Grade { get; set; }
        [DataMember]
        public virtual decimal? LS_MwthS_1 { get; set; }
        [DataMember]
        public virtual decimal? LS_MwthS_2 { get; set; }
        [DataMember]
        public virtual decimal? LS_MwthS_3 { get; set; }
        [DataMember]
        public virtual decimal? LS_MwthS_4 { get; set; }
        [DataMember]
        public virtual decimal? LS_MwthS_5 { get; set; }
        [DataMember]
        public virtual decimal? LS_MwthS_Total { get; set; }
        [DataMember]
        public virtual decimal? LS_MwthS_Average { get; set; }
        [DataMember]
        public virtual string LS_MwthS_Grade { get; set; }
        [DataMember]
        public virtual decimal? WE_1 { get; set; }
        [DataMember]
        public virtual decimal? WE_2 { get; set; }
        [DataMember]
        public virtual decimal? WE_3 { get; set; }
        [DataMember]
        public virtual decimal? WE_4 { get; set; }
        [DataMember]
        public virtual decimal? WE_5 { get; set; }
        [DataMember]
        public virtual decimal? WE_6 { get; set; }
        [DataMember]
        public virtual decimal? WE_7 { get; set; }
        [DataMember]
        public virtual decimal? WE_8 { get; set; }
        [DataMember]
        public virtual decimal? WE_9 { get; set; }
        [DataMember]
        public virtual decimal? WE_10 { get; set; }
        [DataMember]
        public virtual decimal? WE_Total { get; set; }
        [DataMember]
        public virtual decimal? WE_Average { get; set; }
        [DataMember]
        public virtual string WE_Grade { get; set; }
        [DataMember]
        public virtual decimal? VA_1 { get; set; }
        [DataMember]
        public virtual decimal? VA_2 { get; set; }
        [DataMember]
        public virtual decimal? VA_3 { get; set; }
        [DataMember]
        public virtual decimal? VA_4 { get; set; }
        [DataMember]
        public virtual decimal? VA_5 { get; set; }
        [DataMember]
        public virtual decimal? VA_6 { get; set; }
        [DataMember]
        public virtual decimal? VA_7 { get; set; }
        [DataMember]
        public virtual decimal? VA_8 { get; set; }
        [DataMember]
        public virtual decimal? VA_9 { get; set; }
        [DataMember]
        public virtual decimal? VA_10 { get; set; }
        [DataMember]
        public virtual decimal? VA_Total { get; set; }
        [DataMember]
        public virtual decimal? VA_Average { get; set; }
        [DataMember]
        public virtual string VA_Grade { get; set; }
        [DataMember]
        public virtual decimal? PA_1 { get; set; }
        [DataMember]
        public virtual decimal? PA_2 { get; set; }
        [DataMember]
        public virtual decimal? PA_3 { get; set; }
        [DataMember]
        public virtual decimal? PA_4 { get; set; }
        [DataMember]
        public virtual decimal? PA_5 { get; set; }
        [DataMember]
        public virtual decimal? PA_6 { get; set; }
        [DataMember]
        public virtual decimal? PA_7 { get; set; }
        [DataMember]
        public virtual decimal? PA_8 { get; set; }
        [DataMember]
        public virtual decimal? PA_9 { get; set; }
        [DataMember]
        public virtual decimal? PA_10 { get; set; }
        [DataMember]
        public virtual decimal? PA_Total { get; set; }
        [DataMember]
        public virtual decimal? PA_Average { get; set; }
        [DataMember]
        public virtual string PA_Grade { get; set; }
        [DataMember]
        public virtual decimal? AT_AToT_1 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToT_2 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToT_3 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToT_4 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToT_5 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToT_6 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToT_7 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToT_8 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToT_9 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToT_10 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToT_Total { get; set; }
        [DataMember]
        public virtual decimal? AT_AToT_Average { get; set; }
        [DataMember]
        public virtual string AT_AToT_Grade { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSM_1 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSM_2 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSM_3 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSM_4 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSM_5 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSM_6 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSM_7 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSM_8 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSM_9 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSM_10 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSM_Total { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSM_Average { get; set; }
        [DataMember]
        public virtual string AT_AToSM_Grade { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSPE_1 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSPE_2 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSPE_3 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSPE_4 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSPE_5 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSPE_6 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSPE_7 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSPE_8 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSPE_9 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSPE_10 { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSPE_Total { get; set; }
        [DataMember]
        public virtual decimal? AT_AToSPE_Average { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_Grade { get; set; }
        [DataMember]
        public virtual decimal? LndCS_1 { get; set; }
        [DataMember]
        public virtual decimal? LndCS_2 { get; set; }
        [DataMember]
        public virtual decimal? LndCS_3 { get; set; }
        [DataMember]
        public virtual decimal? LndCS_4 { get; set; }
        [DataMember]
        public virtual decimal? LndCS_5 { get; set; }
        [DataMember]
        public virtual decimal? LndCS_6 { get; set; }
        [DataMember]
        public virtual decimal? LndCS_7 { get; set; }
        [DataMember]
        public virtual decimal? LndCS_8 { get; set; }
        [DataMember]
        public virtual decimal? LndCS_9 { get; set; }
        [DataMember]
        public virtual decimal? LndCS_10 { get; set; }
        [DataMember]
        public virtual decimal? LndCS_Total { get; set; }
        [DataMember]
        public virtual decimal? LndCS_Average { get; set; }
        [DataMember]
        public virtual string LndCS_Grade { get; set; }
        [DataMember]
        public virtual decimal? ICT_1 { get; set; }
        [DataMember]
        public virtual decimal? ICT_2 { get; set; }
        [DataMember]
        public virtual decimal? ICT_3 { get; set; }
        [DataMember]
        public virtual decimal? ICT_4 { get; set; }
        [DataMember]
        public virtual decimal? ICT_5 { get; set; }
        [DataMember]
        public virtual decimal? ICT_6 { get; set; }
        [DataMember]
        public virtual decimal? ICT_7 { get; set; }
        [DataMember]
        public virtual decimal? ICT_8 { get; set; }
        [DataMember]
        public virtual decimal? ICT_9 { get; set; }
        [DataMember]
        public virtual decimal? ICT_10 { get; set; }
        [DataMember]
        public virtual decimal? ICT_Total { get; set; }
        [DataMember]
        public virtual decimal? ICT_Average { get; set; }
        [DataMember]
        public virtual string ICT_Grade { get; set; }
       [DataMember]
        public virtual decimal? VS_ToABC_1 { get; set; }
        [DataMember]
       public virtual decimal? VS_ToABC_2 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToABC_3 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToABC_4 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToABC_Total { get; set; }
        [DataMember]
        public virtual decimal? VS_ToABC_Average { get; set; }
        [DataMember]
        public virtual string VS_ToABC_Grade { get; set; }
        [DataMember]
        public virtual decimal? VS_ToCFNI_1 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToCFNI_2 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToCFNI_3 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToCFNI_4 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToCFNI_Total { get; set; }
        [DataMember]
        public virtual decimal? VS_ToCFNI_Average { get; set; }
        [DataMember]
        public virtual string VS_ToCFNI_Grade { get; set; }
        [DataMember]
        public virtual decimal? VS_ToUPSUI_1 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToUPSUI_2 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToUPSUI_3 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToUPSUI_4 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToUPSUI_Total { get; set; }
        [DataMember]
        public virtual decimal? VS_ToUPSUI_Average { get; set; }
        [DataMember]
        public virtual string VS_ToUPSUI_Grade { get; set; }
        [DataMember]
        public virtual decimal? VS_ToRNSWCU_1 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToRNSWCU_2 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToRNSWCU_3 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToRNSWCU_4 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToRNSWCU_Total { get; set; }
        [DataMember]
        public virtual decimal? VS_ToRNSWCU_Average { get; set; }
        [DataMember]
        public virtual string VS_ToRNSWCU_Grade { get; set; }
        [DataMember]
        public virtual decimal? VS_ToPHUB_1 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToPHUB_2 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToPHUB_3 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToPHUB_4 { get; set; }
        [DataMember]
        public virtual decimal? VS_ToPHUB_Total { get; set; }
        [DataMember]
        public virtual decimal? VS_ToPHUB_Average { get; set; }
        [DataMember]
        public virtual string VS_ToPHUB_Grade { get; set; }
        [DataMember]
        public virtual decimal? HPET_1 { get; set; }
        [DataMember]
        public virtual decimal? HPET_2 { get; set; }
        [DataMember]
        public virtual decimal? HPET_3 { get; set; }
        [DataMember]
        public virtual decimal? HPET_4 { get; set; }
        [DataMember]
        public virtual decimal? HPET_5 { get; set; }
        [DataMember]
        public virtual decimal? HPET_6 { get; set; }
        [DataMember]
        public virtual decimal? HPET_7 { get; set; }
        [DataMember]
        public virtual decimal? HPET_8 { get; set; }
        [DataMember]
        public virtual decimal? HPET_9 { get; set; }
        [DataMember]
        public virtual decimal? HPET_10 { get; set; }
        [DataMember]
        public virtual decimal? HPET_Total { get; set; }
        [DataMember]
        public virtual decimal? HPET_Average { get; set; }
        [DataMember]
        public virtual string HPET_Grade { get; set; }

        [DataMember]
        public virtual decimal? SciSkills_1 { get; set; }
        [DataMember]
        public virtual decimal? SciSkills_2 { get; set; }
        [DataMember]
        public virtual decimal? SciSkills_3 { get; set; }
        [DataMember]
        public virtual decimal? SciSkills_4 { get; set; }
        [DataMember]
        public virtual decimal? SciSkills_5 { get; set; }
        [DataMember]
        public virtual decimal? SciSkills_6 { get; set; }
        [DataMember]
        public virtual decimal? SciSkills_7 { get; set; }
        [DataMember]
        public virtual decimal? SciSkills_8 { get; set; }
        [DataMember]
        public virtual decimal? SciSkills_9 { get; set; }
        [DataMember]
        public virtual decimal? SciSkills_10 { get; set; }
        [DataMember]
        public virtual decimal? SciSkills_Total { get; set; }
        [DataMember]
        public virtual decimal? SciSkills_Average { get; set; }
        [DataMember]
        public virtual string SciSkills_Grade { get; set; }

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
