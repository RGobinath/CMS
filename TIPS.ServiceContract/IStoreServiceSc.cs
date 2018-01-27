using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.Entities.StoreEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IStoreService" in both code and config file together.
    [ServiceContract]
    public interface IStoreServiceSc
    {
        long CreateOrUpdateMaterialRequest(MaterialRequest mr);
        MaterialRequest GetMaterialRequestById(long Id);
        MaterialInward GetMaterialInwardById(long Id);
    }
}
