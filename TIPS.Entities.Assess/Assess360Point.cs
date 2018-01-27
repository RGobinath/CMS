using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace TIPS.Entities.Assess
{
    [DataContract]
    public class Assess360Point
    {
        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual string StudName { get; set; }
        [DataMember]
        public virtual DateTime? ReportGenDate { get; set; }
        private string date = DateTime.Now.ToString("dd-MMM-yyy");
        [DataMember]
        public virtual string Date
        {
            get { return date; }
            set { date = value; }
        }
        [DataMember]
        public virtual string Character { get; set; }
        [DataMember]
        public virtual string AttPunctuality { get; set; }
        [DataMember]
        public virtual string HwCompletion { get; set; }
        [DataMember]
        public virtual string HwAccuracy { get; set; }
        [DataMember]
        public virtual string WkChapterTests { get; set; }
        [DataMember]
        public virtual string SLCParentAssessment { get; set; }
        [DataMember]
        public virtual string TermAssessment { get; set; }
        [DataMember]
        public virtual string Total { get; set; }
        //newly added for future purposes
        [DataMember]
        public virtual string RequestNo { get; set; }
        [DataMember]
        public virtual DateTime? DateCreated { get; set; }
        [DataMember]
        public virtual long StudentId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string IdNo { get; set; }

        /// for Desending order
        /// 
        [DataMember]
        public virtual decimal? Consolidation { get; set; }
    }
}
