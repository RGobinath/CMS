using System;
using System.Runtime.Serialization;
namespace TIPS.Entities.Assess
{
    [DataContract]
    public class NotEnteredCount
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string AssignmentName { get; set; }
        [DataMember]
        public virtual int AssessCompGroup { get; set; }
        [DataMember]
        public virtual string Subject { get; set; }
         
    }
}
