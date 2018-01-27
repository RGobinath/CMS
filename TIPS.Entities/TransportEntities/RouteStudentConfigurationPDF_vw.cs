using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.TransportEntities
{
   public class RouteStudentConfigurationPDF_vw
    {
       [DataMember]
       public virtual long Id { get; set; }   
       [DataMember]
       public virtual string RouteStudCode { get; set; }   
       [DataMember]
       public virtual string Name { get; set; }
       [DataMember]
       public virtual string Grade { get; set; }
       [DataMember]
       public virtual string Section { get; set; }
       [DataMember]
       public virtual string TamilDescription { get; set; }
       [DataMember]
       public virtual string LocationName { get; set; }
       [DataMember]
       public virtual string RouteId { get; set; }
       [DataMember]
       public virtual long LocationId { get; set; }
       [DataMember]
       public virtual string Campus { get; set; }
       [DataMember]
       public virtual long StopOrderNumber { get; set; }
       [DataMember]
       public virtual long NoOfStudents { get; set; }
       [DataMember]
       public virtual long PreRegNum { get; set; }
    }
}
