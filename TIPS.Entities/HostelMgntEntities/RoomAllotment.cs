using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.HostelMgntEntities
{
    public class RoomAllotment
    {
        public virtual long Id { get; set; }
        public virtual long StudID { get; set; }
        public virtual string Name { get; set; }
        public virtual string NewId { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Section { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual string BoardingType { get; set; }
        public virtual long HstlMst_Id { get; set; }
        public virtual string HostelName { get; set; }
        public virtual string HostelType { get; set; }
        public virtual string Floor { get; set; }
        public virtual long RoomMst_Id { get; set; }
        public virtual long BedMst_Id { get; set; }
        public virtual string BedNumber { get; set; }
        public virtual string SIPNo { get; set; }
        public virtual string Room_Allotment { get; set; }
        public virtual string ChangeRoomAllotment { get; set; }

    }
}
