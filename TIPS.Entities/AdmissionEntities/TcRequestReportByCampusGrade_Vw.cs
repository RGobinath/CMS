using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIPS.Entities.AdmissionEntities
{
    public class TcRequestReportByCampusGrade_Vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
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

