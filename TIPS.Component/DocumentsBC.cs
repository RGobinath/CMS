using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities;
using TIPS.Entities.StaffEntities;
using TIPS.Entities.StaffManagementEntities;
using TIPS.Entities.AdmissionEntities;

namespace TIPS.Component
{
   public class DocumentsBC
    {
       PersistenceServiceFactory PSF = null;
       public DocumentsBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
       public long CreateOrUpdateDocuments(Documents doc)
       {
           try
           {
               if (doc != null)
                   PSF.SaveOrUpdate<Documents>(doc);
               else { throw new Exception("Documents is required and it cannot be null.."); }
               return doc.EntityRefId;
           }
           catch (Exception)
           {

               throw;
           }
       }
       public Documents GetDocumentsById(long EntityRefId)
       {
           try
           {
               Documents Documents = null;
               if (EntityRefId > 0)
                   Documents = PSF.Get<Documents>(EntityRefId);
               else { throw new Exception("Id is required and it cannot be 0"); }
               return Documents;
           }
           catch (Exception)
           {

               throw;
           }
       }
       public Dictionary<long, IList<Documents>> GetDocumentsListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
       {
           try
           {
               Dictionary<long, IList<Documents>> retValue = new Dictionary<long, IList<Documents>>();
               return PSF.GetListWithExactSearchCriteriaCount<Documents>(page, pageSize, sortBy, sortType, criteria);
           }
           catch (Exception)
           {

               throw;
           }
       }

       public void DeleteUploadedFileById(Documents doc)
       {
           if (doc.Upload_Id > 0)
           {
               PSF.Delete<Documents>(doc);
           }
       }
       public Dictionary<long, IList<DocumentReport_Vw>> GetDocumentsReportListWithPaging(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
       {
           try
           {
               Dictionary<long, IList<DocumentReport_Vw>> retValue = new Dictionary<long, IList<DocumentReport_Vw>>();
               return PSF.GetListWithExactSearchCriteriaCount<DocumentReport_Vw>(page, pageSize, sortBy, sortType, criteria);
           }
           catch (Exception)
           {

               throw;
           }
       }
       //public UploadedFiles GetStaffManagementByPreRegNum(long PreRegNum)
       //{
       //    try
       //    {
       //        if (PreRegNum > 0)
       //            return PSF.Get<UploadedFiles>(PreRegNum);
       //        else throw new Exception("Id is required and it cannot be zero.");
       //    }
       //    catch (Exception) { throw; }
       //    finally { if (PSF != null) PSF.Dispose(); }
        //}

        #region Added By Prabakaran for CallManagement
       public IList<Documents> GetDocumentsByEntityRefId(long Id)
       {
           try
           {
               IList<Documents> documents = PSF.GetListById<Documents>("EntityRefId", Id);
               return documents;
           }
           catch (Exception)
           {

               throw;
           }
       }
        #endregion

    }
}
