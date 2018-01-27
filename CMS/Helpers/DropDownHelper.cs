namespace CMS.Helpers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Linq.Expressions;
    using System;

    public static class DropDownHelper
    {
        //public static MvcHtmlString GetDropDown(this HtmlHelper helper,string name, string selectedID)
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    list.Add(new SelectListItem { Text = "--", Value = "", Selected = (selectedID == "" || string.IsNullOrEmpty(selectedID)) ? true : false });
        //    list.Add(new SelectListItem { Text = "A", Value = "A", Selected = selectedID == "A" ? true : false });
        //    list.Add(new SelectListItem { Text = "B", Value = "B", Selected = selectedID == "B" ? true : false });
        //    list.Add(new SelectListItem { Text = "B*", Value = "B*", Selected = selectedID == "B*" ? true : false });
        //    list.Add(new SelectListItem { Text = "C", Value = "C", Selected = selectedID == "C" ? true : false });
        //    list.Add(new SelectListItem { Text = "D", Value = "D", Selected = selectedID == "D" ? true : false });
        //    return helper.DropDownList(name, list);
        //}

        public static MvcHtmlString GetDropDown(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, string selectedID)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            //list.Add(new SelectListItem { Text = "--", Value = "", Selected = (selectedID == "" || string.IsNullOrEmpty(selectedID)) ? true : false });

            foreach (var item in selectList)
            {
                if (string.IsNullOrEmpty(item.Value) || item.Value == "")
                { list.Add(new SelectListItem { Text = item.Text, Value = item.Value, Selected = true }); }
                else
                { list.Add(new SelectListItem { Text = item.Text, Value = item.Value, Selected = selectedID == item.Value ? true : false }); }
            }

            return helper.DropDownList(name, list);
        }

        public static MvcHtmlString GetDropDown(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, long selectedID)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            //list.Add(new SelectListItem { Text = "--", Value = "", Selected = (selectedID == "" || string.IsNullOrEmpty(selectedID)) ? true : false });

            foreach (var item in selectList)
            {
                if (string.IsNullOrEmpty(item.Value) || item.Value == "")
                { list.Add(new SelectListItem { Text = item.Text, Value = item.Value, Selected = true }); }
                else
                { list.Add(new SelectListItem { Text = item.Text, Value = item.Value, Selected = selectedID.ToString() == item.Value ? true : false }); }
            }

            return helper.DropDownList(name, list);
        }
    }
}