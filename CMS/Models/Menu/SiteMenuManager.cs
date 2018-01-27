using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace CMS.Models.Menu
{
    public class SiteMenuManager
    {
        public List<ISiteLink> GetSitemMenuItems()
        {
            var items = new List<ISiteLink>();

            //string sqlstr = " select Menuid,Parentid,caption,url,sortorder from dynmenu where Parentid in(0,8,4,27,29,23,24,25,26,28) AND Menuid<>16 order by parentid";

            //DataRepository objRepository = new DataRepository();
            //List<DataRow> list = objRepository.GetList(sqlstr);

            ////List<SiteMenuItem> items = list.Select(a => new SiteMenuItem { Id = Convert.ToInt16(a.ItemArray[0].ToString()), ParentId = Convert.ToInt16(a.ItemArray[1].ToString()), Text = a.ItemArray[2].ToString(), Url = a.ItemArray[3].ToString(), OpenInNewWindow = false, SortOrder = Convert.ToInt16(a.ItemArray[4].ToString()) }).AsEnumerable().ToList();

            //list.ToList().ForEach(a => items.Add(new SiteMenuItem { Id = Convert.ToInt16(a.ItemArray[0].ToString()), ParentId = Convert.ToInt16(a.ItemArray[1].ToString()), Text = a.ItemArray[2].ToString(), Url = a.ItemArray[3].ToString(), OpenInNewWindow = false, SortOrder = Convert.ToInt16(a.ItemArray[4].ToString()) }));


            // Top Level 
            items.Add(new SiteMenuItem { Id = 1, ParentId = 0, Text = "Home", Url = "/Home/Home", OpenInNewWindow = false, SortOrder = 0 });
            //items.Add(new SiteMenuItem { Id = 2, ParentId = 0, Text = "My TIPS Page", Url = "/Home/Home", OpenInNewWindow = false, SortOrder = 1 });
            items.Add(new SiteMenuItem { Id = 3, ParentId = 0, Text = "Admission Management",  OpenInNewWindow = false, SortOrder = 2 });
            //items.Add(new SiteMenuItem { Id = 4, ParentId = 0, Text = "Schedule Management", Url = "/Home/Home", OpenInNewWindow = false, SortOrder = 3 });
            items.Add(new SiteMenuItem { Id = 5, ParentId = 0, Text = "Call Management", Url = "", OpenInNewWindow = false, SortOrder = 4 });
           // items.Add(new SiteMenuItem { Id = 6, ParentId = 0, Text = "Transport Management", Url = "/Home/Home", OpenInNewWindow = false, SortOrder = 5 });
            //items.Add(new SiteMenuItem { Id = 7, ParentId = 0, Text = "Boarding", Url = "/Home/Home", OpenInNewWindow = false, SortOrder = 6 });
           // items.Add(new SiteMenuItem { Id = 8, ParentId = 0, Text = "Asset Management", Url = "/Home/Home", OpenInNewWindow = false, SortOrder = 7 });
           items.Add(new SiteMenuItem { Id = 9, ParentId = 0, Text = "System Management", Url = "/Home/SystemManagement", OpenInNewWindow = false, SortOrder = 8 });
            items.Add(new SiteMenuItem { Id = 10, ParentId = 0, Text = "Reports", Url = "/Home/Home", OpenInNewWindow = false, SortOrder = 9 });
            //items.Add(new SiteMenuItem { Id = 11, ParentId = 0, Text = "Dashboard", Url = "/Home/Home", OpenInNewWindow = false, SortOrder = 10 });

            // Student Registration
            items.Add(new SiteMenuItem { Id = 11, ParentId = 3, Text = "Admission Management", Url = "/Admission/AdmissionManagement", OpenInNewWindow = false, SortOrder = 0 });
            //items.Add(new SiteMenuItem { Id = 12, ParentId = 3, Text = "New Registration", Url = "/Admission/NewRegistration", OpenInNewWindow = false, SortOrder = 0 });
            items.Add(new SiteMenuItem { Id = 12, ParentId = 3, Text = "Student Management", Url = "/Admission/StudentManagement", OpenInNewWindow = false, SortOrder = 0 });

            // Student Administration  
            //items.Add(new SiteMenuItem { Id = 13,ParentId = 4, Text = "Admission Approval", Url = "/Services/Tech-Writing", OpenInNewWindow = false, SortOrder = 0 });
            //items.Add(new SiteMenuItem { Id = 14, ParentId = 4, Text = "Section Grouping", Url = "/Services/Consulting", OpenInNewWindow = false, SortOrder = 1 });
            //items.Add(new SiteMenuItem { Id = 15, ParentId = 4, Text = "Promotion", Url = "/Services/Training", OpenInNewWindow = false, SortOrder = 2 });
            //items.Add(new SiteMenuItem { Id = 15, ParentId = 4, Text = "Transfer/Discontinue", Url = "/Services/Training", OpenInNewWindow = false, SortOrder = 2 });

            // Call Management 
           // items.Add(new SiteMenuItem { Id = 16, ParentId = 5, Text = "Call Register", Url = "/Home/CallRegister", OpenInNewWindow = false, SortOrder = 0 });
           // items.Add(new SiteMenuItem { Id = 17, ParentId = 5, Text = "Call Forward", Url = "/Home/CallForward", OpenInNewWindow = false, SortOrder = 1 });

            // System Management 

            items.Add(new SiteMenuItem { Id = 18, ParentId = 9, Text = "System Settings", Url = "/Home/Home", OpenInNewWindow = false, SortOrder = 0 });
            items.Add(new SiteMenuItem { Id = 19, ParentId = 9, Text = "User Management", Url = "/Home/Home", OpenInNewWindow = false, SortOrder = 1 });
           
            items.Add(new SiteMenuItem { Id = 20, ParentId = 5, Text = "Call Mgmt Status Report", Url = "/Home/StatusReport", OpenInNewWindow = false, SortOrder = 2 });
            items.Add(new SiteMenuItem { Id = 21, ParentId = 10, Text = "Student List", Url = "/Home/StudentInfo", OpenInNewWindow = false, SortOrder = 1 });
            items.Add(new SiteMenuItem { Id = 22, ParentId = 5, Text = "Call Management", Url = "/Home/CallManagement", OpenInNewWindow = false, SortOrder = 1 });
            return items;


        }
    }
}