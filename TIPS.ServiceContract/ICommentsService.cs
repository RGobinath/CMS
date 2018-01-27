using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IComments" in both code and config file together.
    [ServiceContract]
    public interface ICommentsSC
    {
        long CreateOrUpdateComments(Comments com);
        Comments GetCommentsById(long EntityRefId);
        Dictionary<long, IList<Comments>> GetCommentsListWithPaging(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria);
    }
}
