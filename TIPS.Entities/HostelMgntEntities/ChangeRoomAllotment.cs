using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.HostelMgntEntities
{
    public class ChangeRoomAllotment
    {
        public virtual long Id { get; set; }
        public virtual long Before_HostDts_Id { get; set; }
        public virtual long Before_HstlMst_Id { get; set; }
        public virtual string Before_HostelName { get; set; }
        public virtual string Before_HostelType { get; set; }
        public virtual string Before_Floor { get; set; }
        public virtual string Before_Campus { get; set; }
        public virtual long Before_RoomMst_Id { get; set; }
        public virtual string Before_RoomNumber { get; set; }
        public virtual long Before_BedMst_Id { get; set; }
        public virtual string Before_BedNumber { get; set; }
        public virtual long Stud_Id { get; set; }
        //public virtual long PreRegNum { get; set; }
        public virtual string Before_AcademicYear { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual string Before_Grade { get; set; }
        public virtual string Before_Section { get; set; }
        public virtual string Before_BoardingType { get; set; }

    }
}
