using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities
{
    [DataContract]
    public enum StudentDetails_Enum
    {
        [EnumMember]
        Id,
        [EnumMember]
        id_no,
        [EnumMember]
        name,
        [EnumMember]
        campus_name,
        [EnumMember]
        section,
        [EnumMember]
        grade,
        [EnumMember]
        BoardingType,
        [EnumMember]
        IsHosteller
    }
}
