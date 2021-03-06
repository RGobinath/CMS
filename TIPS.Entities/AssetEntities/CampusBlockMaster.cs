﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
    public class CampusBlockMaster
    {
        [DataMember]
        public virtual long BlockId { get; set; }
        [DataMember]
        public virtual string BlockCode { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string BlockName { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
