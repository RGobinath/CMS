using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AdmissionEntities
{
    public class UploadedFilesView
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string DocumentFor { get; set; }
        [DataMember]
        public virtual string DocumentType { get; set; }
        [DataMember]
        public virtual string DocumentName { get; set; }
        [DataMember]
        public virtual string DocumentSize { get; set; }
        [DataMember]
        public virtual string UploadedDate { get; set; }
        [DataMember]
        public virtual string Type { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Phone { get; set; }

        [DataMember]
        public virtual int OldFiles { get; set; }
        [DataMember]
        public virtual long PicturesTabRefId { get; set; }
        [DataMember]
        public virtual string FileDirectory { get; set; }
        [DataMember]
        public virtual Int32 MonthOfSalary { get; set; }
    }
}
