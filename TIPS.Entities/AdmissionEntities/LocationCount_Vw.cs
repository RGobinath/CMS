using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AdmissionEntities
{
    public class LocationCount_Vw
    {
        public virtual long Id { get; set; }
        public virtual string Campus { get; set; }
        public virtual int LocationCount { get; set; }
        public virtual string Locality { get; set; }
    }
}
