using System;
using System.Runtime.Serialization;

namespace TIPS.Entities.ParentPortalEntities
{
    [DataContract]
    public class PhotoGalleryPhotos
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual Int64 PGPreId { get; set; }
        [DataMember]
        public virtual byte[] Photo { get; set; }
        [DataMember]
        public virtual string PhotoName { get; set; }
        [DataMember]
        public virtual long Folder_Id { get; set; }
    }
}
