using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.AdmissionEntities
{
    [DataContract]
    public class StudentTemplateAndFamilyDetails_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long StudentTemplateId { get; set; }
        [DataMember]
        public virtual long PreRegNum { get; set; }
        [DataMember]
        public virtual string NewId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string AdmissionStatus { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }
        [DataMember]
        public virtual string General_EmailId { get; set; }
        [DataMember]
        public virtual long Father_Id { get; set; }
        [DataMember]
        public virtual string Father_Mobile { get; set; }
        [DataMember]
        public virtual string Father_EmailId { get; set; }
        [DataMember]
        public virtual long Mother_Id { get; set; }
        [DataMember]
        public virtual string Mother_Mobile { get; set; }
        [DataMember]
        public virtual string Mother_EmailId{ get; set; }
        //Added by rajkumar
        [DataMember]
        public virtual string VanNo { get; set; }
        [DataMember]
        public virtual string BoardingType { get; set; }
        [DataMember]
        public virtual string FoodPreference { get; set; }
        //Added By GObi
        [DataMember]
        public virtual string Initial { get; set; }
        public virtual string SecondLanguage { get; set; }
        //For Transport Route Configuration Added by Gobi
        [DataMember]
        public virtual string LocationName { get; set; }
        [DataMember]
        public virtual string LocationTamilDescription { get; set; }
        //Locality Added by Micheal

        [DataMember]
        public virtual string Locality { get; set; }
        [DataMember]
        public virtual string Place { get; set; }
        [DataMember]
        public virtual bool IsAdded { get; set; }
        [DataMember]
        public virtual string Kilometer { get; set; }
        [DataMember]
        public virtual string PickUpTime { get; set; }
        [DataMember]
        public virtual string DropTime { get; set; }
    }
}
