using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.AdmissionEntities
{
    public class SecondLanguageMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string SecondLanguageCode { get; set; }
        [DataMember]
        public virtual string SecondLanguageText { get; set; }
        [DataMember]
        public virtual long Flag { get; set; }
    }
}
