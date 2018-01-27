using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    [DataContract]
    public class StudFamilyDetailsEmail
    {
        [DataMember]
        public virtual long Id {get;set;}
        [DataMember]
        public virtual string PreRegNum {get;set;}
        [DataMember]
        public virtual string ApplicationNo {get;set;} 
        [DataMember]
        public virtual string Name {get;set;} 
        [DataMember]
        public virtual string Gender {get;set;} 
        [DataMember]
        public virtual string FeeStructYear {get;set;} 
        [DataMember]
        public virtual string AdmissionStatus {get;set;} 
        [DataMember]
        public virtual string NewId {get;set;} 
        [DataMember]
        public virtual string FamilyDetailType {get;set;} 
        [DataMember]
        public virtual string ParentName {get;set;} 
        [DataMember]
        public virtual string Education {get;set;} 
        [DataMember]
        public virtual string ParentEmail {get;set;}
        [DataMember]
        public virtual string ParentGender { get; set; }
    }
}
