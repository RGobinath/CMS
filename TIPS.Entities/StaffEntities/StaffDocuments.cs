using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StaffEntities
{
    [DataContract]
  public class StaffDocuments
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long EntityRefId { get; set; }
        [DataMember]
        public virtual string FileId { get; set; }
        [DataMember]
        public virtual string FileName { get; set; }
        [DataMember]
        public virtual DateTime UploadedOn { get; set; }
        [DataMember]
        public virtual string UploadedBy { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual string ActualDocument { get; set; }

        [DataMember]
        public virtual byte[] DocumentData { get; set; }
        [DataMember]
        public virtual string DocumentSize { get; set; }
    }
}
