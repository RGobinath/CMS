using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    public class AdmissionCountReport_vw
    {
       [DataMember]
       public virtual long Id { get; set; }

       [DataMember]
       public virtual string AcademicYear { get; set; }
       [DataMember]
       public virtual string Campus { get; set; }
       [DataMember]
       public virtual string Grade { get; set; }
       [DataMember]
       public virtual string Section { get; set; }
       [DataMember]
       public virtual int TotalCount { get; set; }
       [DataMember]
       public virtual int Vacancy { get; set; }
    }
}
