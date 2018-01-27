using System.Collections.Generic;
using System.Text;
using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class Assess360Component
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long Assess360Id { get; set; }
        [DataMember]
        public virtual int AssessCompGroup { get; set; }
        [DataMember]
        public virtual bool? IsCredit { get; set; }
        [DataMember]
        public virtual string GroupName { get; set; }
        [DataMember]
        public virtual DateTime? IncidentDate { get; set; }
        [DataMember]
        public virtual DateTime? DateCreated { get; set; }
        [DataMember]
        public virtual decimal? Mark { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
        [DataMember]
        public virtual string Staff { get; set; }
        [DataMember]
        public virtual string AssignmentName { get; set; }
        [DataMember]
        public virtual string MarksOutOff { get; set; }
        [DataMember]
        public virtual string EnteredBy { get; set; }
        [DataMember]
        public virtual DateTime? DateModified { get; set; }
        [DataMember]
        public virtual string Semester { get; set; }
    }
}