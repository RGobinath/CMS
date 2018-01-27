using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace TIPS.Entities
{
    [DataContract]
    public class UserAppRole_Vw
    {
        [DataMember]
        public virtual Int32 Id { get; set; }
        [DataMember]
        public virtual string UserId { get; set; }
        [DataMember]
        public virtual string UserName { get; set; }
        [DataMember]
        public virtual string AppCode { get; set; }
        [DataMember]
        public virtual string RoleCode { get; set; }
        [DataMember]
        public virtual string DeptCode { get; set; }
        [DataMember]
        public virtual string BranchCode { get; set; }
        [DataMember]
        public virtual string AppName { get; set; }
        [DataMember]
        public virtual string RoleName { get; set; }
        [DataMember]
        public virtual string Email { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
       
    }
}
