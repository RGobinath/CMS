using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.AssetEntities
{
    public class AssetDetailsReport_vw
    {
        public virtual long Id { get; set; }
        public virtual string AssetType { get; set; }
        public virtual long Using { get; set; }
        public virtual long Scrap { get; set; }
        public virtual long Service { get; set; }
        public virtual long Stock { get; set; }
        public virtual long TotalAsset { get; set; }
    }
}
