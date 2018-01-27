using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
   public class StudentDetailsExport
    {
       [DataMember]
       public virtual long Id { get; set; }
       [DataMember]
       public virtual string NewId { get; set; }
       [DataMember]
       public virtual string Name { get; set; }
       [DataMember]
       public virtual string Campus { get; set; }
       [DataMember]
       public virtual string AdmissionStatus { get; set; }
       [DataMember]
       public virtual string Gender { get; set; }
       [DataMember]
       public virtual string DOB { get; set; }
       [DataMember]
       public virtual string Grade { get; set; }
       [DataMember]
       public virtual string Section { get; set; }
       [DataMember]
       public virtual string BoardingType { get; set; }
       [DataMember]
       public virtual string FoodType { get; set; }
       [DataMember]
       public virtual string TransportRequired { get; set; }
       [DataMember]
       public virtual long PreRegNum { get; set; }

       [DataMember]
       public virtual string FatherName { get; set; }
       [DataMember]
       public virtual string MotherName { get; set; }
       [DataMember]
       public virtual string FatherMobileNumber { get; set; }
       [DataMember]
       public virtual string MotherMobileNumber { get; set; }
       [DataMember]
       public virtual string Address { get; set; }
       [DataMember]
       public virtual string City { get; set; }
       [DataMember]
       public virtual string FatherEmail { get; set; }
       [DataMember]
       public virtual string MotherEmail { get; set; }

       [DataMember]
       public virtual int Sequence { get; set; }

       [DataMember]
       public virtual string AcademicYear { get; set; }

       [DataMember]
       public virtual string EmailId { get; set; }
       [DataMember]
       public virtual string CreatedDate { get; set; }
       [DataMember]
       public virtual string FatherOccupation { get; set; }
       [DataMember]
       public virtual string MotherOccupation { get; set; }
       [DataMember]
       public virtual string BGRP { get; set; }
       [DataMember]
       public virtual string VanNo { get; set; }
       [DataMember]
       public virtual string MobileNo { get; set; }
    }
}
