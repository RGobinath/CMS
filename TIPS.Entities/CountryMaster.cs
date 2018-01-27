using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Collections;
using System.Web.UI;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using System.ComponentModel.DataAnnotations;

namespace TIPS.Entities
{
    [DataContract]
    public class CountryMaster
    {
        [DataMember]
        public virtual long FormId { get; set; }
        [DataMember]
        public virtual string FormCode { get; set; }
        [DataMember]
        public virtual string CountryCode { get; set; }
        [DataMember]
        public virtual string CountryName { get; set; }
        [DataMember]
        public virtual string RegionCode { get; set; }
        //   [DataMember]
        //public virtual string CreatedBy { get; set; }
        //[DataMember]
        //public virtual string CreatedDate { get; set; }
        //[DataMember]
        //public virtual string UpdatedBy { get; set; }
        //[DataMember]
        //public virtual string UpdatedDate { get; set; }
    }
}
