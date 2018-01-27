using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    public class ReportCardCBSECo_Sch_Attitude
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
        //For Attitude towards teachers
        [DataMember]
        public virtual string AT_AToT_1 { get; set; }
        [DataMember]
        public virtual string AT_AToT_2 { get; set; }
        [DataMember]
        public virtual string AT_AToT_3 { get; set; }
        [DataMember]
        public virtual string AT_AToT_4 { get; set; }
        [DataMember]
        public virtual string AT_AToT_5 { get; set; }
        [DataMember]
        public virtual string AT_AToT_6 { get; set; }
        [DataMember]
        public virtual string AT_AToT_7 { get; set; }
        [DataMember]
        public virtual string AT_AToT_8 { get; set; }
        [DataMember]
        public virtual string AT_AToT_9 { get; set; }
        [DataMember]
        public virtual string AT_AToT_10 { get; set; }
        [DataMember]
        public virtual string AT_AToT_Total { get; set; }
        [DataMember]
        public virtual string AT_AToT_Average { get; set; }
        [DataMember]
        public virtual string AT_AToT_Grade { get; set; }

        //For Attitude Towards School Mates
        [DataMember]
        public virtual string AT_AToSM_1 { get; set; }
        [DataMember]
        public virtual string AT_AToSM_2 { get; set; }
        [DataMember]
        public virtual string AT_AToSM_3 { get; set; }
        [DataMember]
        public virtual string AT_AToSM_4 { get; set; }
        [DataMember]
        public virtual string AT_AToSM_5 { get; set; }
        [DataMember]
        public virtual string AT_AToSM_6 { get; set; }
        [DataMember]
        public virtual string AT_AToSM_7 { get; set; }
        [DataMember]
        public virtual string AT_AToSM_8 { get; set; }
        [DataMember]
        public virtual string AT_AToSM_9 { get; set; }
        [DataMember]
        public virtual string AT_AToSM_10 { get; set; }
        [DataMember]
        public virtual string AT_AToSM_Total { get; set; }
        [DataMember]
        public virtual string AT_AToSM_Average { get; set; }
        [DataMember]
        public virtual string AT_AToSM_Grade { get; set; }

        //Attitude Towards School Programme & Environment
        [DataMember]
        public virtual string AT_AToSPE_1 { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_2 { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_3 { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_4 { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_5 { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_6 { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_7 { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_8 { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_9 { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_10 { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_Total { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_Average { get; set; }
        [DataMember]
        public virtual string AT_AToSPE_Grade { get; set; }
    }
}
