using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.ReportEntities
{
    public class CampusWiseModuleUsageReport_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Module { get; set; }
        //[DataMember]
        //public virtual Int64 MCS_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual Int64 MHSS_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual Int64 MMS_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual Int64 MCOE_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual Int64 MTTI_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual Int64 RPS_KOTAGIRI { get; set; }


         [DataMember]
         public virtual Int64 IBMain { get; set; }
         [DataMember]
         public virtual Int64 IBKG { get; set; }
         [DataMember]
         public virtual Int64 ChennaiMain { get; set; }
         [DataMember]
         public virtual Int64 ChennaiCity { get; set; }
         [DataMember]
         public virtual Int64 Ernakulam { get; set; }
         [DataMember]
         public virtual Int64 ErnakulamKG { get; set; }
         [DataMember]
         public virtual Int64 Karur { get; set; }
         [DataMember]
         public virtual Int64 KarurKG { get; set; }
         [DataMember]
         public virtual Int64 Tirupur { get; set; }
         [DataMember]
         public virtual Int64 TirupurKG { get; set; }
         [DataMember]
         public virtual Int64 TipsSaran { get; set; }
         
    }
}

