using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.ReportEntities
{
    public class CampusWiseUsageModule_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long CampusWiseUsageModule_Id { get; set; }
        [DataMember]
        public virtual string Module { get; set; }
        [DataMember]
        public virtual long IBMain_Count { get; set; }
        [DataMember]
        public virtual bool IBMain { get; set; }
        [DataMember]
        public virtual long IBKG_Count { get; set; }
        [DataMember]
        public virtual bool IBKG { get; set; }
        [DataMember]
        public virtual long ChennaiMain_Count { get; set; }
        [DataMember]
        public virtual bool ChennaiMain { get; set; }
        [DataMember]
        public virtual long ChennaiCity_Count { get; set; }
        [DataMember]
        public virtual bool ChennaiCity { get; set; }
        [DataMember]
        public virtual long Ernakulam_Count { get; set; }
        [DataMember]
        public virtual bool Ernakulam { get; set; }
        [DataMember]
        public virtual long ErnakulamKG_Count { get; set; }
        [DataMember]
        public virtual bool ErnakulamKG { get; set; }
        [DataMember]
        public virtual long Karur_Count { get; set; }
        [DataMember]
        public virtual bool Karur { get; set; }
        [DataMember]
        public virtual long KarurKG_Count { get; set; }
        [DataMember]
        public virtual bool KarurKG { get; set; }
        [DataMember]
        public virtual long Tirupur_Count { get; set; }
        [DataMember]
        public virtual bool Tirupur { get; set; }
        [DataMember]
        public virtual long TirupurKG_Count { get; set; }
        [DataMember]
        public virtual bool TirupurKG { get; set; }
        [DataMember]
        public virtual long TipsSaran_Count { get; set; }
        [DataMember]
        public virtual bool TipsSaran { get; set; }
        //[DataMember]
        //public virtual long MCS_ANTHIYUR_Count { get; set; }
        //[DataMember]
        //public virtual bool MCS_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual long MHSS_ANTHIYUR_Count { get; set; }
        //[DataMember]
        //public virtual bool MHSS_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual long MMS_ANTHIYUR_Count { get; set; }
        //[DataMember]
        //public virtual bool MMS_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual long MCOE_ANTHIYUR_Count { get; set; }
        //[DataMember]
        //public virtual bool MCOE_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual long MTTI_ANTHIYUR_Count { get; set; }
        //[DataMember]
        //public virtual bool MTTI_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual long RPS_KOTAGIRI_Count { get; set; }
        //[DataMember]
        //public virtual bool RPS_KOTAGIRI { get; set; }
    }
}
