﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.TransportEntities
{
    public class RouteMasterConfig_vw
    {
        public virtual long Id{get;set;}
        public virtual long RouteId { get; set; }
        public virtual string RouteStudCode{get;set;}
        public virtual string RouteNo{get;set;}
        public virtual string Campus{get;set;}
        public virtual string Source{get;set;}
        public virtual string Destination{get;set;}
        public virtual string Via { get; set; }
        public virtual long NoOfStudents { get; set; }
    }
}
