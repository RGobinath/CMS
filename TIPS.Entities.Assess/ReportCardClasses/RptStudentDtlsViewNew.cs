using System.Runtime.Serialization;
using System;
namespace TIPS.Entities.Assess.ReportCardClasses
{
    [DataContract]
    public class RptStudentDtlsViewNew
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long RptId { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual bool IsHosteller { get; set; }
        [DataMember]
        public virtual string ApplicationNo { get; set; }
        [DataMember]
        public virtual string AdmissionStatus { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string FeeStructYear { get; set; }
        [DataMember]
        public virtual string BoardingType { get; set; }
        //added for Reportcard for only show 
        [DataMember]
        public virtual long RptRequestId { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string PA1 { get; set; }
        [DataMember]
        public virtual string NoteBook1 { get; set; }
        [DataMember]
        public virtual string SEA1 { get; set; }
        [DataMember]
        public virtual string HalfYearly { get; set; }
        [DataMember]
        public virtual string Term1Total { get; set; }
        [DataMember]
        public virtual string Term1Grade { get; set; }
        [DataMember]
        public virtual string PA2 { get; set; }
        [DataMember]
        public virtual string NoteBook2 { get; set; }
        [DataMember]
        public virtual string SEA2 { get; set; }
        [DataMember]
        public virtual string AnnualExam { get; set; }
        [DataMember]
        public virtual string Term2Total { get; set; }
        [DataMember]
        public virtual string Term2Grade { get; set; }
        [DataMember]
        public virtual string Absent { get; set; }

        public virtual decimal? T1T2Total { get; set; }
        [DataMember]
        public virtual string T1T2Grade { get; set; }
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
        public virtual string TermAbsents { get; set; }
        [DataMember]
        public virtual long EditRowId { get; set; }
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