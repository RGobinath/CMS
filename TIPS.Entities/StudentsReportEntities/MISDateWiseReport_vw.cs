using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TIPS.Entities.StudentsReportEntities
{
    public class MISDateWiseReport_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string CreatedDate { get; set; }
        [DataMember]
        public virtual long Total { get; set; }



        [DataMember]
        public virtual long CurrentTotal { get; set; }
        [DataMember]
        public virtual long CurrentToddlerTotal { get; set; }
        [DataMember]
        public virtual long PreviousTotal { get; set; }
        [DataMember]
        public virtual long PreviousToddlerTotal { get; set; }



        [DataMember]
        public virtual string DateString { get; set; }
        [DataMember]
        public virtual string Campus1 { get; set; }
        [DataMember]
        public virtual string Campus1Boys { get; set; }
        [DataMember]
        public virtual string Campus1Girls { get; set; }
        [DataMember]
        public virtual string Campus2 { get; set; }
        [DataMember]
        public virtual string Campus2Boys { get; set; }
        [DataMember]
        public virtual string Campus2Girls { get; set; }
        [DataMember]
        public virtual string Campus3 { get; set; }
        [DataMember]
        public virtual string Campus3Boys { get; set; }
        [DataMember]
        public virtual string Campus3Girls { get; set; }
        [DataMember]
        public virtual string Campus4 { get; set; }
        [DataMember]
        public virtual string Campus4Boys { get; set; }
        [DataMember]
        public virtual string Campus4Girls { get; set; }
        [DataMember]
        public virtual string Campus5 { get; set; }
        [DataMember]
        public virtual string Campus5Boys { get; set; }
        [DataMember]
        public virtual string Campus5Girls { get; set; }
        [DataMember]
        public virtual string Campus6 { get; set; }
        [DataMember]
        public virtual string Campus6Boys { get; set; }
        [DataMember]
        public virtual string Campus6Girls { get; set; }
        [DataMember]
        public virtual string Campus7 { get; set; }
        [DataMember]
        public virtual string Campus7Boys { get; set; }
        [DataMember]
        public virtual string Campus7Girls { get; set; }
        [DataMember]
        public virtual string Campus8 { get; set; }
        [DataMember]
        public virtual string Campus8Boys { get; set; }
        [DataMember]
        public virtual string Campus8Girls { get; set; }
        [DataMember]
        public virtual string Campus9 { get; set; }
        [DataMember]
        public virtual string Campus9Boys { get; set; }
        [DataMember]
        public virtual string Campus9Girls { get; set; }
        [DataMember]
        public virtual string Campus10 { get; set; }
        [DataMember]
        public virtual string Campus10Boys { get; set; }
        [DataMember]
        public virtual string Campus10Girls { get; set; }
        [DataMember]
        public virtual string Campus11 { get; set; }
        [DataMember]
        public virtual string Campus11Boys { get; set; }
        [DataMember]
        public virtual string Campus11Girls { get; set; }
        [DataMember]
        public virtual string Campus12 { get; set; }
        [DataMember]
        public virtual string Campus12Boys { get; set; }
        [DataMember]
        public virtual string Campus12Girls { get; set; }
        [DataMember]
        public virtual string Campus13 { get; set; }
        [DataMember]
        public virtual string Campus13Boys { get; set; }
        [DataMember]
        public virtual string Campus13Girls { get; set; }
        //For Total
        [DataMember]
        public virtual string Campus1Total { get; set; }
        [DataMember]
        public virtual string Campus1BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus1GirlsTotal { get; set; }
        [DataMember]
        public virtual string Campus2Total { get; set; }
        [DataMember]
        public virtual string Campus2BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus2GirlsTotal { get; set; }
        [DataMember]
        public virtual string Campus3Total { get; set; }
        [DataMember]
        public virtual string Campus3BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus3GirlsTotal { get; set; }
        [DataMember]
        public virtual string Campus4Total { get; set; }
        [DataMember]
        public virtual string Campus4BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus4GirlsTotal { get; set; }
        [DataMember]
        public virtual string Campus5Total { get; set; }
        [DataMember]
        public virtual string Campus5BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus5GirlsTotal { get; set; }
        [DataMember]
        public virtual string Campus6Total { get; set; }
        [DataMember]
        public virtual string Campus6BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus6GirlsTotal { get; set; }
        [DataMember]
        public virtual string Campus7Total { get; set; }
        [DataMember]
        public virtual string Campus7BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus7GirlsTotal { get; set; }
        [DataMember]
        public virtual string Campus8Total { get; set; }
        [DataMember]
        public virtual string Campus8BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus8GirlsTotal { get; set; }
        [DataMember]
        public virtual string Campus9Total { get; set; }
        [DataMember]
        public virtual string Campus9BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus9GirlsTotal { get; set; }
        [DataMember]
        public virtual string Campus10Total { get; set; }
        [DataMember]
        public virtual string Campus10BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus10GirlsTotal { get; set; }
        [DataMember]
        public virtual string Campus11Total { get; set; }
        [DataMember]
        public virtual string Campus11BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus11GirlsTotal { get; set; }
        [DataMember]
        public virtual string Campus12Total { get; set; }
        [DataMember]
        public virtual string Campus12BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus12GirlsTotal { get; set; }
        [DataMember]
        public virtual string Campus13Total { get; set; }
        [DataMember]
        public virtual string Campus13BoysTotal { get; set; }
        [DataMember]
        public virtual string Campus13GirlsTotal { get; set; }

        [DataMember]
        public virtual long Toddler { get; set; }
        [DataMember]
        public virtual long Boys { get; set; }
        [DataMember]
        public virtual long Girls { get; set; }
        [DataMember]
        public virtual string BoysString { get; set; }
        [DataMember]
        public virtual string GirlsString { get; set; }
        [DataMember]
        public virtual string CampusShowName { get; set; }
    }
}
