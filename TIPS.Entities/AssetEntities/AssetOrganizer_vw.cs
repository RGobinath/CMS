using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AssetEntities
{
   public class AssetOrganizer_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string RequestNo { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string AssetName { get; set; }
        [DataMember]
        public virtual string StaffIncharge { get; set; }
        [DataMember]
        public virtual string Status { get; set; }
        [DataMember]
        public virtual DateTime Date { get; set; }
        //[DataMember]
        //public virtual DateTime StartTime { get; set; }
        [DataMember]
        public virtual string StartTimeString { get; set; }
        [DataMember]
        public virtual string ReasonForBooking { get; set; }
        //[DataMember]
        //public virtual DateTime EndTime { get; set; }
        [DataMember]
        public virtual string EndTimeString { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual DateTime CreatedDate { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual DateTime ModifiedDate { get; set; }
        [DataMember]
        public virtual string AssetColor { get; set; }
   }
}
