﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.HostelMgntEntities
{
    public class HstlMgmt_FoodExpenses
    {
        public virtual long Id { get; set; }
        public virtual long Stud_Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual long Amount { get; set; }
        public virtual string Venue { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string Name { get; set; }
        public virtual string NewId { get; set; }
        public virtual string DummyRec { get; set; }

    }
}
