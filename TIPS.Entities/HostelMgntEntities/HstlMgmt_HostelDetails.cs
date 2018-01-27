using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.HostelMgntEntities
{
    public class HstlMgmt_HostelDetails
    {
        public virtual Int64 HostDts_Id { get; set; }
        public virtual Int64 HstlMst_Id { get; set; }
        public virtual string HostelName { get; set; }
        public virtual string HostelType { get; set; }
        public virtual string Floor { get; set; }
        public virtual Int64 RoomMst_Id { get; set; }
        public virtual string RoomNumber { get; set; }
        public virtual Int64 BedMst_Id { get; set; }
        public virtual string BedNumber { get; set; }
        public virtual Int64 Stud_Id { get; set; }
        public virtual Int64 PreRegNum { get; set; }
        public virtual string SIPNo { get; set; }
        public virtual string InCharge { get; set; }
        public virtual bool IsRoomAllocate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime DateOfJoining { get; set; } // Hostel admission Date

        public virtual string Name { get; set; }
        public virtual string NewId { get; set; }
        public virtual string Campus { get; set; } // get records from student template into hostel details
        public virtual string Grade { get; set; } // ''
        public virtual string Section { get; set; } //''
        public virtual string AcademicYear { get; set; }// ''
        public virtual string BoardingType { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? DateModified { get; set; }

    }
}
