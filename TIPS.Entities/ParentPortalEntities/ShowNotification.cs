using System.Runtime.Serialization;

namespace TIPS.Entities.ParentPortalEntities
{
    [DataContract]
   public class ShowNotification
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long NotePreId { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string ViewStatus { get; set; }
        [DataMember]
        public virtual string ViewedOn { get; set; }
        [DataMember]
        public virtual string NoteValid { get; set; }
    }
}
