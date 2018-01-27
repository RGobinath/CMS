using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.StoreEntities
{
   public  class MaterialDistributionReportByDate
    {
        public virtual long Id { get; set; }

        public virtual string Campus { get; set; }

        public virtual string AcademicYear { get; set; }

        public virtual string Material { get; set; }

        public virtual long IssuedTotal { get; set; }

        public virtual DateTime? DatePart { get; set; }

    }
}
