using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities;
using TIPS.Entities.StaffEntities;

namespace TIPS.Component
{
    public class CommentsBC
    {
        PersistenceServiceFactory PSF = null;
        public CommentsBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        public long CreateOrUpdateComments(Comments doc)
        {
            try
            {
                if (doc != null)
                    PSF.SaveOrUpdate<Comments>(doc);
                else { throw new Exception("Comments is required and it cannot be null.."); }
                return doc.EntityRefId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Comments GetCommentsById(long EntityRefId)
        {
            try
            {
                Comments Comments = null;
                if (EntityRefId > 0)
                    Comments = PSF.Get<Comments>(EntityRefId);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return Comments;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<Comments>> GetCommentsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<Comments>> retValue = new Dictionary<long, IList<Comments>>();
                return PSF.GetListWithExactSearchCriteriaCount<Comments>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
