using System;
using System.Runtime.Serialization;


namespace TIPS.Entities.ParentPortalEntities
{
    [DataContract]
    public class Notification
    {
        [DataMember]
        public virtual Int64 Id { get; set; }
        [DataMember]
        public virtual string Topic { get; set; }
        [DataMember]
        public virtual string Message { get; set; }
        [DataMember]
        public virtual string PublishDate { get; set; }
        [DataMember]
        public virtual Int64 ExpireAfter { get; set; }
        [DataMember]
        public virtual string PublishTo { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string ExpireDate { get; set; }
        [DataMember]
        public virtual string Valid { get; set; }
        [DataMember]
        public virtual Int64 NotePreId { get; set; }
        [DataMember]
        public virtual string NoteType { get; set; }
        [DataMember]
        public virtual string NewIds { get; set; }
        [DataMember]
        public virtual string PublishedOn { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string CampusGroup { get; set; }
       
        [DataMember]
        public virtual string PreRegNos { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Type { get; set; }
    }
}
