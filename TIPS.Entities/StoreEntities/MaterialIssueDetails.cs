using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StoreEntities
{
     [DataContract]
    public class MaterialIssueDetails
    {
            [DataMember]
            public virtual long IssueId { get; set; }
            [DataMember]
            public virtual long StudentId { get; set; }
            [DataMember]
            public virtual long MaterialSubGroupId { get; set; }
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
            public virtual string CreatedBy { get; set; }
            [DataMember]
            public virtual DateTime? CreatedDate { get; set; }
            [DataMember]
            public virtual string ModifiedBy { get; set; }
            [DataMember]
            public virtual DateTime? ModifiedDate { get; set; }
            [DataMember]
            public virtual long Mat_Id { get; set; }//only for reference
            public virtual long StudId { get; set; }//only for reference
            public virtual string Material { get; set; }//only for reference

            //public virtual StudentMaterialDistribution_vw StudentMaterialDistribution_vw { get; set; }
            public virtual long MaterialDistributionId { get; set; }
            
              
    }
}
