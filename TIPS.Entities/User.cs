﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
namespace TIPS.Entities
{
    [DataContract]
    public class User
    {
        [DataMember]
        public virtual Int32 Id { get; set; }
        [DataMember]
        public virtual string UserId { get; set; }
        [DataMember]
        public virtual string EmployeeId { get; set; }
        [DataMember]
        public virtual string UserName { get; set; }
        [DataMember]
        public virtual string Role { get; set; }
        [DataMember]
        public virtual string Type { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        //[Required(ErrorMessage = "Please enter your email")]
        //[RegularExpression(@"^[A-Za-z0-9_\-\.]+@(([A-Za-z0-9\-])+\.)+([A-Za-z\-])+$", ErrorMessage = "Please enter a valid email address.")]
        public virtual string EmailId { get; set; }
        [DataMember]
        public virtual bool IsActive { get; set; }
        [DataMember]
        public virtual DateTime? CreatedDate { get; set; }
        [DataMember]
        public virtual DateTime? ModifiedDate { get; set; }
        [DataMember]
        public virtual string CreatedBy { get; set; }
        [DataMember]
        public virtual string ModifiedBy { get; set; }
        [DataMember]
        public virtual string UserType { get; set; }
        [DataMember]
       // [StringLength(10, ErrorMessage = "Password should be minimum of 6 characters length", MinimumLength = 6)]
        public virtual string Password { get; set; }
        [DataMember]
        public virtual string PasswordQuestion { get; set; }
        [DataMember]
        public virtual string PasswordAnswer { get; set; }
        [DataMember]
        public virtual string ConfirmPassword { get; set; }
        [DataMember]
      //  [StringLength(10, ErrorMessage = "Password should be minimum of 6 characters length", MinimumLength = 6)]
        public virtual string NewPassword { get; set; }

        public virtual bool RememberMe { get; set; }
    }
}
