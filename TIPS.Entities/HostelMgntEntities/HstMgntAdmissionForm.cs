using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.HostelMgntEntities
{
    public class HstMgntAdmissionForm
    {
        public virtual long Id { get; set; }
        public virtual string HstIdNo { get; set; }
        public virtual string Hst_Name { get; set; }
        public virtual long HstRoom_No { get; set; }
        public virtual long HstBed_No { get; set; }
        public virtual string Stud_Name { get; set; }
        public virtual long Stud_Id { get; set; }
        public virtual long Stud_PreRegNum { get; set; }
        public virtual string Stud_NewId { get; set; }
        public virtual string Stud_Campus { get; set; }
        public virtual string Stud_Grade { get; set; }
        public virtual string Stud_Section { get; set; }
        public virtual string Stud_AcademicYear { get; set; }
        public virtual string Hst_Incharge { get; set; }
        public virtual string Hst_CreatedBy { get; set; }
        public virtual DateTime Hst_DateCreated { get; set; }
        public virtual string Hst_ModifiedBy { get; set; }
        public virtual DateTime? Hst_DateModified { get; set; }
    }
}
