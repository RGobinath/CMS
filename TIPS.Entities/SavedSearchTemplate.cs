using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    [DataContract]
    public class SavedSearchTemplate
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string UserId { get; set; }
        [DataMember]
        public virtual string SearchName { get; set; }
        [DataMember]
        public virtual string Application { get; set; }
        [DataMember]
        public virtual string SavedSearch { get; set; }
        [DataMember]
        public virtual bool IsDefault { get; set; }
        [DataMember]
        public virtual DateTime? DateCreated { get; set; }
        [DataMember]
        public virtual DateTime? DateModified { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
    }
}
