using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    public class MaterialIssueReportView
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string RequiredForStore { get; set; }
        [DataMember]
        public virtual string RequiredFromStore { get; set; }
        [DataMember]
        public virtual string RequiredForCampus { get; set; }
        [DataMember]
        public virtual string MaterialGroup { get; set; }
        [DataMember]
        public virtual string MaterialSubGroup { get; set; }
        [DataMember]
        public virtual string Material { get; set; }
        [DataMember]
        public virtual int IssuedMonth { get; set; }
        [DataMember]
        public virtual int IssuedYear { get; set; }
        [DataMember]
        public virtual int IssuedQty { get; set; }
        [DataMember]
        public virtual decimal TotalPrice { get; set; }
    }
}
