using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    public class ReportCardCBSECo_Sch_Values
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
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Term { get; set; }
        //For Abide by the Constitution
        [DataMember]
        public virtual string VS_ToABC_1 { get; set; }
        [DataMember]
        public virtual string VS_ToABC_2 { get; set; }
        [DataMember]
        public virtual string VS_ToABC_3 { get; set; }
        [DataMember]
        public virtual string VS_ToABC_4 { get; set; }
        [DataMember]
        public virtual string VS_ToABC_Total { get; set; }
        [DataMember]
        public virtual string VS_ToABC_Average { get; set; }
        [DataMember]
        public virtual string VS_ToABC_Grade { get; set; }

        //For To Cherish and Follow the Noble Ideas
        [DataMember]
        public virtual string VS_ToCFNI_1 { get; set; }
        [DataMember]
        public virtual string VS_ToCFNI_2 { get; set; }
        [DataMember]
        public virtual string VS_ToCFNI_3 { get; set; }
        [DataMember]
        public virtual string VS_ToCFNI_4 { get; set; }
        [DataMember]
        public virtual string VS_ToCFNI_Total { get; set; }
        [DataMember]
        public virtual string VS_ToCFNI_Average { get; set; }
        [DataMember]
        public virtual string VS_ToCFNI_Grade { get; set; }

        //For To Upload & Project the Sovereignty, Unity & Intergrity
        [DataMember]
        public virtual string VS_ToUPSUI_1 { get; set; }
        [DataMember]
        public virtual string VS_ToUPSUI_2 { get; set; }
        [DataMember]
        public virtual string VS_ToUPSUI_3 { get; set; }
        [DataMember]
        public virtual string VS_ToUPSUI_4 { get; set; }
        [DataMember]
        public virtual string VS_ToUPSUI_Total { get; set; }
        [DataMember]
        public virtual string VS_ToUPSUI_Average { get; set; }
        [DataMember]
        public virtual string VS_ToUPSUI_Grade { get; set; }

        //For To Render National Service when Called Upon
        [DataMember]
        public virtual string VS_ToRNSWCU_1 { get; set; }
        [DataMember]
        public virtual string VS_ToRNSWCU_2 { get; set; }
        [DataMember]
        public virtual string VS_ToRNSWCU_3 { get; set; }
        [DataMember]
        public virtual string VS_ToRNSWCU_4 { get; set; }
        [DataMember]
        public virtual string VS_ToRNSWCU_Total { get; set; }
        [DataMember]
        public virtual string VS_ToRNSWCU_Average { get; set; }
        [DataMember]
        public virtual string VS_ToRNSWCU_Grade { get; set; }


        //For To Promote Harmony, Unity & BrotherHood
        [DataMember]
        public virtual string VS_ToPHUB_1 { get; set; }
        [DataMember]
        public virtual string VS_ToPHUB_2 { get; set; }
        [DataMember]
        public virtual string VS_ToPHUB_3 { get; set; }
        [DataMember]
        public virtual string VS_ToPHUB_4 { get; set; }
        [DataMember]
        public virtual string VS_ToPHUB_Total { get; set; }
        [DataMember]
        public virtual string VS_ToPHUB_Average { get; set; }
        [DataMember]
        public virtual string VS_ToPHUB_Grade { get; set; }


        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }

    }
}
