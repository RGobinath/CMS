﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
    public class ITAccessoriesModelMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual ITAccessoriesBrandMaster ITAccessoriesBrandMaster { get; set; }
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
