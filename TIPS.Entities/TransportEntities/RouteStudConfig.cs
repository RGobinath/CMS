using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class RouteStudConfig
    {
        public virtual long Id { get; set; }
        public virtual string RouteStudCode { get; set; }
        public virtual long RouteId { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual string CreatedBy { get; set; }

        //Added For Dropdown
        public virtual string Campus { get; set; }
        public virtual string Route { get; set; }
    }
}
