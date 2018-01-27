using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
namespace TIPS.Entities.ParentPortalEntities
{
    public class PhotoGalleryFolder
    {
        [DataMember]
        public virtual long Folder_Id { get; set; }
        [DataMember]
        public virtual string FolderName {get;set;}
        [DataMember]
        public virtual Int64 PGPreId { get; set; }
    }
}
