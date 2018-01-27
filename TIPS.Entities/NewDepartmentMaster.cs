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
    public class NewDepartmentMaster
    {

        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual string DesignationName { get; set; }
        [DataMember]
        public virtual string StaffType { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string CreatedDate { get; set; }
        [DataMember]
        public virtual string UpdateBy { get; set; }
        [DataMember]
        public virtual string UpdateDate { get; set; }
    }

}
