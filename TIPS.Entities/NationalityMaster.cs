using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    public class NationalityMaster
    {
        [DataMember]
        public virtual long Id { get; set; }

        [DataMember]
        public virtual string Nationality { get; set; }
    }
}
