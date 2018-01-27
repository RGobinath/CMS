using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.ServiceContract;
using TIPS.Entities;
using TIPS.Component;
using TIPS.Entities.StaffEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Comments" in code, svc and config file together.
    public class CommentsService : ICommentsSC
    {
        public long CreateOrUpdateComments(Comments com)
        {
            try
            {
                CommentsBC CommentsBC = new CommentsBC();
                CommentsBC.CreateOrUpdateComments(com);
                return com.EntityRefId;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        public Comments GetCommentsById(long EntityRefId)
        {
            try
            {
                CommentsBC CommentsBC = new CommentsBC();
                return CommentsBC.GetCommentsById(EntityRefId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Comments>> GetCommentsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                CommentsBC CommentsBC = new CommentsBC();
                return CommentsBC.GetCommentsListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }


    }
}
