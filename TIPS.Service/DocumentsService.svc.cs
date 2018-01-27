using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TIPS.ServiceContract;
using TIPS.Entities;
using TIPS.Component;
using TIPS.Entities.StaffManagementEntities;
using TIPS.Entities.StaffEntities;
using TIPS.Entities.AdmissionEntities;

namespace TIPS.ServiceContract
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Documents" in code, svc and config file together.
    public class DocumentsService : IDocumentsSC
    {
        public long CreateOrUpdateDocuments(Documents doc)
        {
            try
            {
                DocumentsBC DocumentsBC = new DocumentsBC();
                DocumentsBC.CreateOrUpdateDocuments(doc);
                return doc.EntityRefId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
        public Documents GetDocumentsById(long EntityRefId)
        {
            try
            {
                DocumentsBC DocumentsBC = new DocumentsBC();
                return DocumentsBC.GetDocumentsById(EntityRefId);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<Documents>> GetDocumentsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                DocumentsBC DocumentsBC = new DocumentsBC();
                return DocumentsBC.GetDocumentsListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        public Dictionary<long, IList<DocumentReport_Vw>> GetDocumentsReportListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                DocumentsBC DocumentsBC = new DocumentsBC();
                return DocumentsBC.GetDocumentsReportListWithPaging(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #region Added By Prabakaran Documents as List
        public IList<Documents> GetDocumentsByEntityRefId(long Id)
        {
            try
            {
                DocumentsBC DocumentsBC = new DocumentsBC();
                return DocumentsBC.GetDocumentsByEntityRefId(Id);
            }
            catch (Exception)
            {
                throw;
            }
            finally { }
        }
        #endregion
        
    }
}
