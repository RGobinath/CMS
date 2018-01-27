using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Test()
        {
            return View();
        }

            //select  Row_number() OVER (ORDER BY Name ASC) AS Id,name,Campus,Grade,Section 
            //from Assess360 Where IsActive=1 and
            //Campus='" + cam + "' 
            //and Grade='" + gra + "' 
            //and Section='" + sect + "' and AcademicYear='" + acayear + "'
            //and name not in 
            //( 
            //select distinct  a3.Name from Assess360 a3 
            //inner join Assess360Component ac 
            //on a3.Id=ac.Assess360Id 
            //where a3.IsActive=1 and
            //a3.Campus='" + cam + "' 
            //and a3.Grade='" + gra + "' 
            //and a3.Section='" + sect + "' and a3.AcademicYear='" + acayear + "' 
            // and AssessCompGroup=" + assType + " 
            // and Subject='" + sub + "' 
            // and AssignmentName=N'" + AssignmentName + "' 
            // and Semester=N'" + sem + "' 
            //) 


    }
}
