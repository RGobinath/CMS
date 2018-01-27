using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    [DataContract]
    public partial class FileContentSharing
    {
  
        [DataMember]
        public virtual string FileName { get; set; }
        [DataMember]
        public virtual long FileSize { get; set; }
        [DataMember]
        public virtual string FilePath { get; set; }
    }

    public class ResponseResult
    {
        public bool isValid { get; set; }
        public string Result { get; set; }
    }
}
