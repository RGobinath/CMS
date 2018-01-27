using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StoreEntities
{
    [DataContract]
   public class StudentMaterialDistribution_vw
    {        
        [DataMember]
        public virtual long MaterialviewId { get; set; }
        [DataMember]
        public virtual long StudId { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string MaterialSubGroup { get; set; }
        [DataMember]
        public virtual long MaterialSubGroupId { get; set; }
        [DataMember]
        public virtual string Material { get; set; }
        [DataMember]
        public virtual long Quantity { get; set; }
        [DataMember]
        public virtual string Gender { get; set; }
        [DataMember]
        public virtual bool IsHosteller { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual long IssueId { get; set; }
        [DataMember]
        public virtual long StudentId { get; set; }
        [DataMember]
        public virtual long MaterialId { get; set; }
        [DataMember]
        public virtual long IssuedQty { get; set; }
        [DataMember]
        public virtual long ReceivedQty { get; set; }
        [DataMember]
        public virtual string PendingItems { get; set; }
        [DataMember]
        public virtual long ExtraQty { get; set; }
        [DataMember]
        public virtual long TotalQty { get; set; }
        [DataMember]
        public virtual long MaterialDistributionId { get; set; }
     
    }
}

