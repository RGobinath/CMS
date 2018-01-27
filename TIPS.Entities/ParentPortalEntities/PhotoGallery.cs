using System.Runtime.Serialization;

namespace TIPS.Entities.ParentPortalEntities
{
    [DataContract]
    public class PhotoGallery
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PGPreId { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        //Added By Prabakaran
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string AlbumName { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        [DataMember]
        public virtual string Performer { get; set; }
        [DataMember]
        public virtual string CreatedOn { get; set; }
        [DataMember]
        public virtual string PublishedTo { get; set; }
        [DataMember]
        public virtual string Status { get; set; }        
        [DataMember]
        public virtual bool IsActive { get; set; }
        [DataMember]
        public virtual long ParentRefId { get; set; }
        [DataMember]
        public virtual string FolderName { get; set; }        
        //[DataMember]
        //public virtual long Folder_Id { get; set; }
        //[DataMember]
        //public virtual long OrderNo { get; set; }
        
    }
}
