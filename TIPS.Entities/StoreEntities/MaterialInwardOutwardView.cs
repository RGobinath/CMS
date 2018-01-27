using System.Runtime.Serialization;

namespace TIPS.Entities.StoreEntities
{
    public class MaterialInwardOutwardView
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string MaterialGroup { get; set; }
        [DataMember]
        public virtual string MaterialSubGroup { get; set; }
        [DataMember]
        public virtual string Material { get; set; }
        [DataMember]
        public virtual int AMonth { get; set; }
        [DataMember]
        public virtual int AYear { get; set; }
        [DataMember]
        public virtual int OpeningBalance { get; set; }
        [DataMember]
        public virtual int Inward { get; set; }
        [DataMember]
        public virtual int Outward { get; set; }
        [DataMember]
        public virtual int ClosingBalance { get; set; }
        [DataMember]
        public virtual string Store { get; set; }
    }
}
