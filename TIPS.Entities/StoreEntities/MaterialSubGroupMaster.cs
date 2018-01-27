using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
   [DataContract]
   public class MaterialSubGroupMaster
    {
         [DataMember]
         public virtual long Id { get; set; }
         [DataMember]
         public virtual long MaterialGroupId { get; set; }
         [DataMember]
         public virtual string MaterialSubGroup { get; set; }
         [DataMember]
         public virtual string MatSubGrpCode { get; set; }
    }
}
