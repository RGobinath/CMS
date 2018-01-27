using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class ReportCardCBSEPrintViewNew
    {
        [DataMember]
        public virtual string TipsLogo { get; set; }
        [DataMember]
        public virtual string TipsNaceLogo { get; set; }
        [DataMember]
        public virtual string TipsName { get; set; }
        [DataMember]
        public virtual string TipsAddress { get; set; }
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string IsTerm { get; set; }
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
        public virtual string DOB { get; set; }
        [DataMember]
        public virtual string FatherName { get; set; }
        [DataMember]
        public virtual string MotherName { get; set; }
        [DataMember]
        public virtual string Address { get; set; }
        /// <summary>
        /// Common Entries 
        /// </summary>
        [DataMember]
        public virtual long TofWorkingDayT1 { get; set; }
        [DataMember]
        public virtual long TofWorkingDayT2 { get; set; }
        [DataMember]
        public virtual decimal TotalAttendenceT1 { get; set; }
        [DataMember]
        public virtual decimal TotalAttendenceT2 { get; set; }
        [DataMember]
        public virtual decimal HeightT1 { get; set; }
        [DataMember]
        public virtual decimal HeightT2 { get; set; }
        [DataMember]
        public virtual decimal WeightT1 { get; set; }
        [DataMember]
        public virtual decimal WeightT2 { get; set; }

        [DataMember]
        public virtual string MobileNumber { get; set; }
        [DataMember]
        public virtual string RptCardOverAllGrade { get; set; }
        /// <summary>
        /// ReportCard Marks entries
        /// </summary>
        /// 
        [DataMember]
        public virtual IList<ReportCardCBSENew> RptCardObjList { get; set; }
        [DataMember]
        public virtual IList<CoSch_Item_Vw> RptCardCoSchObjList { get; set; }
        [DataMember]
        public virtual string[] TitleName { get; set; }
        [DataMember]
        public virtual string[] CoSchName { get; set; }
        [DataMember]
        public virtual string FileName {get;set;}
    }
}
