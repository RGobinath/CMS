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
    public class DesignationMaster
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string Designation { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Code { get; set; }
    }
}
