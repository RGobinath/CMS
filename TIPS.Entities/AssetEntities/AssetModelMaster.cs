﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
    public class AssetModelMaster
    {
        [DataMember]
        public virtual long ModelMasterId { get; set; }
        [DataMember]
        public virtual string Brand { get; set; }
        [DataMember]
        public virtual string Model { get; set; }
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
