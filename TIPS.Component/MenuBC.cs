using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities.MenuEntities;


namespace TIPS.Component
{
    public class MenuBC
    {
          PersistenceServiceFactory PSF = null;
        public MenuBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        public Menu GetMenuItemsById(long Id)
        {
            try
            {
                if (Id > 0)
                    return PSF.Get<Menu>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<Menu> GetMenusById(long[] Ids)
        {
            try
            {
                if (Ids!=null && Ids.Count() > 0)
                    return PSF.GetListByIds<Menu>(Ids);
                else { throw new Exception("Id is required and it cannot be 0"); }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Menu>> GetMenuListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<Menu>(page, pageSize, sortby, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Added By Gobi For BreadCrumbing
        public Menu GetMenuByControllerAndAction(string Controller, string Action)
        {
            try
            {
                Menu Menu = null;
                if (Controller != null && Controller != "")
                    Menu = PSF.Get<Menu>("Controller", Controller, "Action", Action);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return Menu;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
