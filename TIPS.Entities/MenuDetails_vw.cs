using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities
{
    public class MenuDetails_vw
    {
        [DataMember]
        public virtual long MenuDetails_Id { get; set; }
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string MenuName { get; set; }
        [DataMember]
        public virtual string MenuLevel { get; set; }
        [DataMember]
        public virtual bool ParentORChild { get; set; }
        [DataMember]
        public virtual string Role { get; set; }
        [DataMember]
        public virtual long ParentRefId { get; set; }
        [DataMember]
        public virtual long OrderNo { get; set; }
        [DataMember]
        public virtual string Controller { get; set; }
        [DataMember]
        public virtual string Action { get; set; }
        [DataMember]
        public virtual long MainMenu_Id { get; set; }
    }
}
