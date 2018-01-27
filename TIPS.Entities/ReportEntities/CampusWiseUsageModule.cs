using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.ReportEntities
{
    public class CampusWiseUsageModule
    {
        [DataMember]
        public virtual long CampusWiseUsageModule_Id { get; set; }
        [DataMember]
        public virtual string Module { get; set; }
        [DataMember]
        public virtual bool IBMain { get; set; }
        [DataMember]
        public virtual bool IBKG { get; set; }
        [DataMember]
        public virtual bool ChennaiMain { get; set; }
        [DataMember]
        public virtual bool ChennaiCity { get; set; }
        [DataMember]
        public virtual bool Ernakulam { get; set; }
        [DataMember]
        public virtual bool ErnakulamKG { get; set; }
        [DataMember]
        public virtual bool Karur { get; set; }
        [DataMember]
        public virtual bool KarurKG{ get; set; }
        [DataMember]
        public virtual bool Tirupur { get; set; }
        [DataMember]
        public virtual bool TirupurKG { get; set; }
        [DataMember]
        public virtual bool TipsSaran { get; set; }
        //[DataMember]
        //public virtual bool MCS_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual bool MHSS_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual bool MMS_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual bool MCOE_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual bool MTTI_ANTHIYUR { get; set; }
        //[DataMember]
        //public virtual bool RPS_KOTAGIRI { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual long Menu_Id { get; set; }

    }
}
