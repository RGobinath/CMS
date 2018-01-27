using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.HostelMgntEntities
{
    public class StudHostelDtls_Vw
    {
        public virtual Int64 Id { get; set; }
        public virtual Int64 Stud_Id { get; set; }
        public virtual string StudStaffIdNumber { get; set; }
        public virtual string StudName { get; set; }
        public virtual Int64 PreRegNum { get; set; }
        public virtual Int64 HostDts_Id { get; set; }
        public virtual Int64 HstlMst_Id { get; set; }
        public virtual string HostelName { get; set; }
        public virtual string HostelType { get; set; }
        //public virtual string Blocks { get; set; }
        public virtual string Floor { get; set; }
        public virtual string Location { get; set; }
        public virtual string Campus { get; set; }
        public virtual Int64 RoomMst_Id { get; set; }
        //public virtual string RoomName { get; set; }
        public virtual string RoomNumber { get; set; }
        public virtual Int64 BedMst_Id { get; set; }
        //public virtual string BedName { get; set; }
        public virtual string BedNumber { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual string InCharge { get; set; }
        public virtual bool IsRoomAllocate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? DateModified { get; set; }

    }
}
