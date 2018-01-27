using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using TIPS.Entities.StoreEntities;
using TIPS.Entities;

namespace TIPS.Component
{
    public class StoreBC
    {

        PersistenceServiceFactory PSF = null;
        public StoreBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }

        public long CreateOrUpdateMaterialRequest(MaterialRequest mr)
        {
            try
            {
                if (mr != null)
                    PSF.SaveOrUpdate<MaterialRequest>(mr);
                else { throw new Exception("MaterialRequest is required and it cannot be null.."); }
                return mr.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MaterialRequest GetMaterialRequestById(long Id)
        {
            try
            {
                MaterialRequest MaterialRequest = null;
                if (Id > 0)
                    MaterialRequest = PSF.Get<MaterialRequest>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return MaterialRequest;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MaterialInward GetMaterialInwardById(long Id)
        {
            try
            {
                MaterialInward MaterialInward = null;
                if (Id > 0)
                    MaterialInward = PSF.Get<MaterialInward>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return MaterialInward;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public Dictionary<long, IList<StoreMgmntActivity>> GetMaterialRequestListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, string ColumnName, string[] values, Dictionary<string, object> criteria, string[] alias)
        {
            try
            {
                Dictionary<long, IList<StoreMgmntActivity>> retValue = new Dictionary<long, IList<StoreMgmntActivity>>();
                return PSF.GetListWithSearchCriteriaCountArrayExactSearch<StoreMgmntActivity>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateMaterialRequestList(MaterialRequestList mrl)
        {
            try
            {
                if (mrl != null)
                    PSF.SaveOrUpdate<MaterialRequestList>(mrl);
                else { throw new Exception("MaterialRequestList is required and it cannot be null.."); }
                return mrl.MatReqRefId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MaterialRequestList>> GetMaterialRequestListListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialRequestList>> retValue = new Dictionary<long, IList<MaterialRequestList>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialRequestList>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<StoreUnits>> GetStoreUnitsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StoreUnits>> retValue = new Dictionary<long, IList<StoreUnits>>();
                return PSF.GetListWithSearchCriteriaCount<StoreUnits>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<MaterialInward>> GetMaterialInwardlistWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialInward>> retValue = new Dictionary<long, IList<MaterialInward>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialInward>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }


        public long CreateOrUpdateMaterialInward(MaterialInward mi)
        {
            try
            {
                if (mi != null)
                    PSF.SaveOrUpdate<MaterialInward>(mi);
                else { throw new Exception("MaterialInward is required and it cannot be null.."); }
                return mi.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateSku(SkuList sl)
        {
            try
            {
                if (sl != null)
                    PSF.SaveOrUpdate<SkuList>(sl);
                else { throw new Exception("SkuList is required and it cannot be null.."); }
                return sl.SkuId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<SkuList>> GetSkulistWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<SkuList>> retValue = new Dictionary<long, IList<SkuList>>();
                return PSF.GetListWithSearchCriteriaCount<SkuList>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public long StartStoreManagement(MaterialRequest m, string Template, string userId)
        {
            try
            {
                //return 121312;
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                //creating process instance
                ProcessInstance pi = new ProcessInstance();
                pi.CreatedBy = userId;
                pi.DateCreated = DateTime.Now;
                pi.TemplateId = WorkFlowTemplate.Id;
                pi.Status = "Activated";
                PSF.SaveOrUpdate<ProcessInstance>(pi);
                long pid = pi.Id;
                //create object with the pid for FFExport
                m.InstanceId = pid;
                PSF.SaveOrUpdate<MaterialRequest>(m);
                IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "ActivityOrder", 2);
                foreach (WorkFlowStatus wfs in WorkFlowStatusList)
                {
                    Activity NextActivity = new Activity();
                    NextActivity.ActivityName = wfs.WFStatus;
                    NextActivity.ActivityFullName = wfs.Description;
                    NextActivity.AppRole = wfs.Performer;
                    NextActivity.Completed = false;
                    NextActivity.Available = true;
                    NextActivity.Assigned = false;
                    NextActivity.Performer = userId;
                    NextActivity.TemplateId = WorkFlowTemplate.Id;
                    NextActivity.InstanceId = pid;
                    NextActivity.NextActOrder = wfs.NextActOrder;
                    NextActivity.ActivityOrder = wfs.ActivityOrder;
                    NextActivity.PreviousActOrder = 1;
                    NextActivity.ProcessRefId = m.Id;
                    NextActivity.CreatedDate = DateTime.Now;
                    NextActivity.BranchCode = m.Campus;
                    NextActivity.DeptCode = m.Department;
                    PSF.SaveOrUpdate<Activity>(NextActivity);
                }

                return m.Id;
            }
            catch (Exception) { throw; }
        }

        public bool CompleteActivityStoreManagement(MaterialRequest mr, string Template, string userId, string ActivityName, bool isRejection)
        {
            bool retValue = false;
            try
            {
                PSF.SaveOrUpdate<MaterialRequest>(mr);
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                //close the current activity
                Activity CurrentActivity = PSF.Get<Activity>("InstanceId", mr.InstanceId, "ActivityName", ActivityName, "Completed", false);
                if (CurrentActivity != null)
                {
                    //if current activity doesnt need any rejection and there is no rejection then it will be completed
                    //otherwise once the rejection resolved then it will be completed
                    //this need to be obdated as waiting
                    if (CurrentActivity.IsRejApplicable == true && isRejection == true)
                    {
                        CurrentActivity.BranchCode = mr.Campus;
                        CurrentActivity.DeptCode = mr.Department;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        PSF.SaveOrUpdate(CurrentActivity);
                    }
                    else
                    {
                        Activity WaitingAct = PSF.Get<Activity>("WaitingFor", CurrentActivity.Id);
                        if (WaitingAct != null)
                        {
                            CurrentActivity.BranchCode = mr.Campus;
                            CurrentActivity.DeptCode = mr.Department;
                            CurrentActivity.Waiting = false;
                            PSF.SaveOrUpdate(CurrentActivity);
                            WaitingAct.Completed = true;
                            WaitingAct.Assigned = false;
                            WaitingAct.Available = false;
                        }
                        CurrentActivity.BranchCode = mr.Campus;
                        CurrentActivity.DeptCode = mr.Department;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        CurrentActivity.Performer = userId;
                        if (CurrentActivity.ActivityName == "Complete")
                        {
                            //  mr.IsIssueCompleted = true;
                            PSF.Update<MaterialRequest>(mr);
                            ProcessInstance pi = PSF.Get<ProcessInstance>(mr.InstanceId);
                            pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                            mr.RequestStatus = "Completed";
                            PSF.SaveOrUpdate<MaterialRequest>(mr);
                        }
                        PSF.SaveOrUpdate(CurrentActivity);
                    }
                }
                //trigger the next activity //we need to check whether parallel activities are there to complete
                //activities that are coming in the next order
                IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "ActivityOrder", CurrentActivity.NextActOrder);
                //checking parallel activities get the current order of activities
                Dictionary<string, object> WaitCriteria = new Dictionary<string, object>();
                WaitCriteria.Add("TemplateId", WorkFlowTemplate.Id);
                WaitCriteria.Add("ActivityOrder", CurrentActivity.ActivityOrder);
                WaitCriteria.Add("InstanceId", CurrentActivity.InstanceId);
                Dictionary<long, IList<Activity>> conditionList = PSF.GetListWithSearchCriteriaCount<Activity>(0, 100, string.Empty, string.Empty, WaitCriteria);
                if (conditionList != null && conditionList.Count > 0)
                {
                    IList<Activity> conditionWaitList = conditionList.First().Value;
                    bool? waiting = false;
                    foreach (Activity a in conditionWaitList)
                    {
                        if (a.Completed == false && waiting == false)
                        {
                            waiting = true;
                        }
                    } retValue = true;
                    if (waiting == true)
                    {
                        if (CurrentActivity.IsRejApplicable == true && isRejection == true)
                        { }
                        else
                            return retValue;
                    }
                }
                if (WorkFlowStatusList != null && WorkFlowStatusList.Count > 0)
                {
                    //if it is rejection flow then build the logic here
                    //{logic goes here }


                    foreach (WorkFlowStatus wfs in WorkFlowStatusList)
                    {
                        //Rejection Activity
                        if (CurrentActivity.IsRejApplicable == true && isRejection == true)
                        {
                            WorkFlowStatus wfsRej = PSF.Get<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "RejectionFor", CurrentActivity.ActivityOrder);
                            if (wfsRej != null)
                            {
                                Activity NextActivityRej = new Activity();
                                NextActivityRej.CreatedDate = DateTime.Now;
                                NextActivityRej.ActivityName = wfsRej.WFStatus;
                                if (NextActivityRej.ActivityName == "Complete")
                                {
                                    NextActivityRej.Completed = true;
                                    ProcessInstance pi = PSF.Get<ProcessInstance>(mr.InstanceId);
                                    pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                                    mr.RequestStatus = NextActivityRej.ActivityName;
                                    PSF.SaveOrUpdate<MaterialRequest>(mr);
                                }
                                else NextActivityRej.Completed = false;
                                NextActivityRej.ActivityFullName = wfsRej.Description;
                                NextActivityRej.AppRole = wfsRej.Performer;

                                NextActivityRej.Performer = userId;
                                NextActivityRej.TemplateId = WorkFlowTemplate.Id;
                                NextActivityRej.InstanceId = mr.InstanceId;
                                NextActivityRej.NextActOrder = wfsRej.NextActOrder;
                                NextActivityRej.ActivityOrder = wfsRej.ActivityOrder;
                                NextActivityRej.PreviousActOrder = wfsRej.PreviousActOrder;
                                NextActivityRej.ProcessRefId = mr.Id;
                                NextActivityRej.RejectionFor = CurrentActivity.Id;
                                NextActivityRej.Completed = false;
                                NextActivityRej.Available = true;
                                NextActivityRej.Assigned = false;
                                NextActivityRej.BranchCode = mr.Campus;
                                NextActivityRej.DeptCode = mr.Department;
                                PSF.SaveOrUpdate<Activity>(NextActivityRej);
                                mr.RequestStatus = NextActivityRej.ActivityName;
                                PSF.SaveOrUpdate<MaterialRequest>(mr);
                                //CurrentActivity.WaitingFor = NextActivityRej.Id;
                                //PSF.SaveOrUpdate(CurrentActivity);
                            }
                        }
                        else
                        {
                            Activity NextActivity = new Activity();
                            NextActivity.CreatedDate = DateTime.Now;
                            if (wfs.IsRejectionRequired == true)
                            { NextActivity.IsRejApplicable = true; }

                            NextActivity.ActivityName = wfs.WFStatus;
                            NextActivity.ActivityFullName = wfs.Description;
                            NextActivity.AppRole = wfs.Performer;
                            NextActivity.Completed = wfs.WFStatus == "Complete" ? true : false;
                            NextActivity.Available = wfs.WFStatus != "Complete" ? true : false;
                            //NextActivity.Performer = userId;
                            NextActivity.TemplateId = WorkFlowTemplate.Id;
                            NextActivity.InstanceId = mr.InstanceId;
                            NextActivity.NextActOrder = wfs.NextActOrder;
                            NextActivity.ActivityOrder = wfs.ActivityOrder;
                            NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                            NextActivity.ProcessRefId = mr.Id;
                            NextActivity.Available = true;
                            NextActivity.Assigned = false;
                            NextActivity.Completed = false;
                            NextActivity.BranchCode = mr.Campus;
                            NextActivity.DeptCode = mr.Department;
                            PSF.SaveOrUpdate<Activity>(NextActivity);
                            mr.RequestStatus = NextActivity.ActivityName;
                            PSF.SaveOrUpdate<MaterialRequest>(mr);
                        }
                    } retValue = true;
                }
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public SkuList GetSkuListById(long Id)
        {
            try
            {
                SkuList SkuList = null;
                if (Id > 0)
                    SkuList = PSF.Get<SkuList>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return SkuList;
            }
            catch (Exception)
            {

                throw;
            }

        }


        public long CreateOrUpdateStock(Stock st)
        {
            try
            {
                if (st != null)
                    PSF.SaveOrUpdate<Stock>(st);
                else { throw new Exception("Stock is required and it cannot be null.."); }
                return st.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<Stock>> GetStockListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<Stock>> retValue = new Dictionary<long, IList<Stock>>();
                return PSF.GetListWithSearchCriteriaCount<Stock>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public IList<SkuList> CreateOrUpdateSKUList(IList<SkuList> skuLst)
        {
            try
            {
                if (skuLst != null)
                    PSF.SaveOrUpdate<SkuList>(skuLst);
                else { throw new Exception("MaterialRequest is required and it cannot be null.."); }
                return skuLst;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<StoreMaster> GetStoreByCampus(string Campus) //Administrative
        {

            try
            {
                return PSF.GetListByName<StoreMaster>("from " + typeof(StoreMaster) + " where Campus ='" + Campus + "'");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public SkuList DeleteSKUbyId(SkuList sku)
        {
            try
            {
                if (sku != null)
                    PSF.Delete<SkuList>(sku);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return sku;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<Stock_vw>> GetStockListWithPagingAndCriteria_vw(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<Stock_vw>> retValue = new Dictionary<long, IList<Stock_vw>>();
                return PSF.GetListWithSearchCriteriaCount<Stock_vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<MaterialInward_vw>> GetMaterialInwardlistWithPagingAndCriteria_vw(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialInward_vw>> retValue = new Dictionary<long, IList<MaterialInward_vw>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialInward_vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public IList<MaterialRequestList> CreateOrUpdateSKUList(IList<MaterialRequestList> MatReqLst)
        {
            try
            {
                if (MatReqLst != null)
                    PSF.SaveOrUpdate<MaterialRequestList>(MatReqLst);
                else { throw new Exception("MaterialRequestList is required and it cannot be null.."); }
                return MatReqLst;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MaterialRequestList GetMaterialRequestListById(long Id)
        {
            try
            {
                MaterialRequestList MaterialRequest = null;
                if (Id > 0)
                    MaterialRequest = PSF.Get<MaterialRequestList>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return MaterialRequest;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public MaterialRequestList DeleteMaterialRequestListById(MaterialRequestList mrl)
        {
            try
            {
                if (mrl != null)
                    PSF.Delete<MaterialRequestList>(mrl);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return mrl;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateMaterialIssueList(MaterialIssueList mil)
        {
            try
            {
                if (mil != null)
                    PSF.SaveOrUpdate<MaterialIssueList>(mil);
                else { throw new Exception("MaterialIssueList is required and it cannot be null.."); }
                return mil.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MaterialIssueList GetMaterialIssueListById(long Id)
        {
            try
            {
                MaterialIssueList MaterialIssue = null;
                if (Id > 0)
                    MaterialIssue = PSF.Get<MaterialIssueList>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return MaterialIssue;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MaterialIssueList>> GetMaterialIssuelistWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialIssueList>> retValue = new Dictionary<long, IList<MaterialIssueList>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialIssueList>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateMaterialIssueNote(MaterialIssueNote min)
        {
            try
            {
                if (min != null)
                    PSF.SaveOrUpdate<MaterialIssueNote>(min);
                else { throw new Exception("MaterialIssueNote is required and it cannot be null.."); }
                return min.IssNoteId;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public MaterialIssueNote GetMaterialIssueNoteById(long Id)
        {
            try
            {
                MaterialIssueNote MaterialIssueNote = null;
                if (Id > 0)
                    MaterialIssueNote = PSF.Get<MaterialIssueNote>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return MaterialIssueNote;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<MaterialIssueNote>> GetMaterialMaterialIssueNoteListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialIssueNote>> retValue = new Dictionary<long, IList<MaterialIssueNote>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialIssueNote>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MaterialIssueList_vw>> GetMaterialIssuelist_vwWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialIssueList_vw>> retValue = new Dictionary<long, IList<MaterialIssueList_vw>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialIssueList_vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<MaterialIssueNote_vw>> GetMaterialMaterialIssueNote_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialIssueNote_vw>> retValue = new Dictionary<long, IList<MaterialIssueNote_vw>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialIssueNote_vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MatInward_SkuList_vw>> GetMatInward_SkuList_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MatInward_SkuList_vw>> retValue = new Dictionary<long, IList<MatInward_SkuList_vw>>();
                return PSF.GetListWithSearchCriteriaCount<MatInward_SkuList_vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MatReq_ReqList_vw>> GetMatReq_ReqList_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MatReq_ReqList_vw>> retValue = new Dictionary<long, IList<MatReq_ReqList_vw>>();
                return PSF.GetListWithSearchCriteriaCount<MatReq_ReqList_vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MatIssNote_RequestList_vw>> GetMatIssNote_RequestList_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MatIssNote_RequestList_vw>> retValue = new Dictionary<long, IList<MatIssNote_RequestList_vw>>();
                return PSF.GetListWithSearchCriteriaCount<MatIssNote_RequestList_vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateStockTransaction(StockTransaction st)
        {
            try
            {
                if (st != null)
                    PSF.SaveOrUpdate<StockTransaction>(st);
                else { throw new Exception("StockTransaction is required and it cannot be null.."); }
                return st.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MaterialsMaster GetMaterialsMasterByMaterial(string Material)
        {
            try
            {
                return PSF.Get<MaterialsMaster>("Material", Material);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public MaterialIssueNote GetMaterialIssueNoteById(long Id)
        //{
        //    try
        //    {
        //        MaterialIssueNote MaterialIssueNote = null;
        //        if (Id > 0)
        //            MaterialIssueNote = PSF.Get<MaterialIssueNote>(Id);
        //        else { throw new Exception("Id is required and it cannot be 0"); }
        //        return MaterialIssueNote;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public IList<MaterialRequestList> GetMaterialRequestListListWithPagingAndCriteria1(long MatReqRefId)
        {
            IList<MaterialRequestList> retValue = new List<MaterialRequestList>();
            MaterialRequestList mrl = new MaterialRequestList();

            return retValue;
        }

        public Dictionary<long, IList<StoreStockBalance>> GetStoreStockBalanceListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StoreStockBalance>> retValue = new Dictionary<long, IList<StoreStockBalance>>();
                return PSF.GetListWithSearchCriteriaCount<StoreStockBalance>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<StoreMaster>> GetStoreMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StoreMaster>> retValue = new Dictionary<long, IList<StoreMaster>>();
                return PSF.GetListWithSearchCriteriaCount<StoreMaster>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        #region"Store"
        public Dictionary<long, IList<MaterialGroupMaster>> GetMaterialGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialGroupMaster>> retValue = new Dictionary<long, IList<MaterialGroupMaster>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialGroupMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<MaterialSubGroupMaster> GetMaterialSubGroupByMaterialGroup(long MaterialGroupId) //Administrative
        {

            try
            {
                return PSF.GetListByName<MaterialSubGroupMaster>("from " + typeof(MaterialSubGroupMaster) + " where MaterialGroupId ='" + MaterialGroupId + "'");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public IList<MaterialsMaster> GetMaterialByMaterialGroupAndMaterialSubGroup(long MaterialGroupId, long MaterialSubGroupId) //Administrative
        {

            try
            {
                return PSF.GetListByName<MaterialsMaster>("from " + typeof(MaterialsMaster) + " where MaterialGroupId ='" + MaterialGroupId + "' and MaterialSubGroupId ='" + MaterialSubGroupId + "' ");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public Dictionary<long, IList<StoreSupplierMaster>> GetStoreSupplierMasterlistWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StoreSupplierMaster>> retValue = new Dictionary<long, IList<StoreSupplierMaster>>();
                return PSF.GetListWithSearchCriteriaCount<StoreSupplierMaster>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateStoreSupplierMaster(StoreSupplierMaster ssm)
        {
            try
            {
                if (ssm != null)
                    PSF.SaveOrUpdate<StoreSupplierMaster>(ssm);
                else { throw new Exception("StoreSupplierMaster is required and it cannot be null.."); }
                return ssm.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long CreateOrUpdateMaterialGroupSupplier(MaterialGroupSupplier mgs)
        {
            try
            {
                if (mgs != null)
                    PSF.SaveOrUpdate<MaterialGroupSupplier>(mgs);
                else { throw new Exception("Material Group Supplier is required and it cannot be null.."); }
                return mgs.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StoreUnits>> GetStoreUnitslistWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StoreUnits>> retValue = new Dictionary<long, IList<StoreUnits>>();
                return PSF.GetListWithSearchCriteriaCount<StoreUnits>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateStoreUnitsMaster(StoreUnits su)
        {
            try
            {
                if (su != null)
                    PSF.SaveOrUpdate<StoreUnits>(su);
                else { throw new Exception("StoreSupplierMaster is required and it cannot be null.."); }
                return su.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public long CreateOrUpdateStoreMaterialGroupMaster(MaterialGroupMaster mgm)
        {
            try
            {
                if (mgm != null)
                    PSF.SaveOrUpdate<MaterialGroupMaster>(mgm);
                else { throw new Exception("MaterialGroupMaster is required and it cannot be null.."); }
                return mgm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MaterialGroupSupplier>> GetMaterialGroupSupplierlistWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialGroupSupplier>> retValue = new Dictionary<long, IList<MaterialGroupSupplier>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialGroupSupplier>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MaterialSubGroupMaster>> GetMaterialSubGroupListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialSubGroupMaster>> retValue = new Dictionary<long, IList<MaterialSubGroupMaster>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialSubGroupMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long CreateOrUpdateStoreMaterialSubGroupMaster(MaterialSubGroupMaster msgm)
        {
            try
            {
                if (msgm != null)
                    PSF.SaveOrUpdate<MaterialSubGroupMaster>(msgm);
                else { throw new Exception("MaterialSubGroupMaster is required and it cannot be null.."); }
                return msgm.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<MaterialsMaster>> GetMaterialsMasterlistWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialsMaster>> retValue = new Dictionary<long, IList<MaterialsMaster>>();
                return PSF.GetListWithEQSearchCriteriaCount<MaterialsMaster>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public long CreateOrUpdateMaterialsMaster(MaterialsMaster mm)
        {
            try
            {
                if (mm != null)
                    PSF.SaveOrUpdate<MaterialsMaster>(mm);
                else { throw new Exception("MaterialsMaster is required and it cannot be null.."); }
                return mm.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MaterialGroupMaster GetMaterialGroupById(long Id)
        {
            try
            {
                MaterialGroupMaster mg = null;
                if (Id > 0)
                    mg = PSF.Get<MaterialGroupMaster>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return mg;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<MaterialSubGroupMaster_vw>> GetMaterialSubGroupListWithPagingAndCriteriaUsingView(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialSubGroupMaster_vw>> retValue = new Dictionary<long, IList<MaterialSubGroupMaster_vw>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialSubGroupMaster_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<MaterialsMaster_vw>> GetMaterialsMasterlistWithPagingAndCriteriaUsingView(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialsMaster_vw>> retValue = new Dictionary<long, IList<MaterialsMaster_vw>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialsMaster_vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<MaterialGroupSupplier_vw>> GetMaterialGroupSupplierlistWithPagingAndCriteriaUsingView(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialGroupSupplier_vw>> retValue = new Dictionary<long, IList<MaterialGroupSupplier_vw>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialGroupSupplier_vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateUOM_ConversionMatrix(UOM_ConversionMatrix ucm)
        {
            try
            {
                if (ucm != null)
                    PSF.SaveOrUpdate<UOM_ConversionMatrix>(ucm);
                else { throw new Exception("UOM_ConversionMatrix is required and it cannot be null.."); }
                return ucm.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<UOM_ConversionMatrix>> GetUOMConversionlistWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<UOM_ConversionMatrix>> retValue = new Dictionary<long, IList<UOM_ConversionMatrix>>();
                return PSF.GetListWithSearchCriteriaCount<UOM_ConversionMatrix>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MaterialsMaster_vw_Stock_vw>> GetMaterialsMasterAndStockBalancelistWithPagingAndCriteriaUsingView(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialsMaster_vw_Stock_vw>> retValue = new Dictionary<long, IList<MaterialsMaster_vw_Stock_vw>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialsMaster_vw_Stock_vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion"Store"
        #region Store Report Added By Micheal
        public Dictionary<long, IList<MaterialInwardOutwardView>> GetMaterialIOListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialInwardOutwardView>> retValue = new Dictionary<long, IList<MaterialInwardOutwardView>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialInwardOutwardView>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<MonthMaster>> GetMonthMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MonthMaster>> retValue = new Dictionary<long, IList<MonthMaster>>();
                return PSF.GetListWithSearchCriteriaCount<MonthMaster>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public Dictionary<long, IList<MaterialInwardReport_Vw>> GetMaterialInwardReport_VwListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialInwardReport_Vw>> retValue = new Dictionary<long, IList<MaterialInwardReport_Vw>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialInwardReport_Vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {
                throw;
            }
        }

        #region Material Issued Report added by Micheal
        public Dictionary<long, IList<MaterialIssueReportView>> GetMaterialIssueListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialIssueReportView>> retValue = new Dictionary<long, IList<MaterialIssueReportView>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialIssueReportView>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Store to Store Material Transfer


        public long CreateOrUpdateMaterialIssue(StoreToStore mi)
        {
            try
            {
                if (mi != null)
                    PSF.SaveOrUpdate<StoreToStore>(mi);
                else { throw new Exception("StoreToStore is required and it cannot be null.."); }
                return mi.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<StoreToStoreIssuedMaterials> CreateOrUpdateStoreToStoreIssuedMaterialsList(IList<StoreToStoreIssuedMaterials> IssueLst)
        {
            try
            {
                if (IssueLst != null)
                    PSF.SaveOrUpdate<StoreToStoreIssuedMaterials>(IssueLst);
                else { throw new Exception("StoreToStoreIssuedMaterials is required and it cannot be null.."); }
                return IssueLst;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<StoreToStoreIssuedMaterials>> GetStoreToStoreIssuedMaterialsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StoreToStoreIssuedMaterials>> retValue = new Dictionary<long, IList<StoreToStoreIssuedMaterials>>();
                return PSF.GetListWithSearchCriteriaCount<StoreToStoreIssuedMaterials>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public StoreToStore GetMaterialIssueById(int Id)
        {
            try
            {
                StoreToStore MaterialIssue = null;
                if (Id > 0)
                    MaterialIssue = PSF.Get<StoreToStore>(Id);
                else { throw new Exception("Id is required and it cannot be 0"); }
                return MaterialIssue;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public Dictionary<long, IList<SkuList_MaterialPrice_vw>> GetSkuList_MaterialPrice_vwWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<SkuList_MaterialPrice_vw>> retValue = new Dictionary<long, IList<SkuList_MaterialPrice_vw>>();
                return PSF.GetListWithSearchCriteriaCount<SkuList_MaterialPrice_vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<StoreToStore>> GetStoreToStoreListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<StoreToStore>> retValue = new Dictionary<long, IList<StoreToStore>>();
                return PSF.GetListWithSearchCriteriaCount<StoreToStore>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<SkuList_MaterialInward>> GetSkuList_MaterialInwardListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                Dictionary<long, IList<SkuList_MaterialInward>> retValue = new Dictionary<long, IList<SkuList_MaterialInward>>();
                return PSF.GetListWithSearchCriteriaCountArrayExactSearch<SkuList_MaterialInward>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public MaterialReturn CreateOrUpdateMaterialReturn(MaterialReturn mr)
        {
            try
            {
                if (mr != null)
                    PSF.SaveOrUpdate<MaterialReturn>(mr);
                else { throw new Exception("MaterialReturn is required and it cannot be null.."); }
                return mr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MaterialReturn GetMaterialReturnById(int MatRetId)
        {
            try
            {
                MaterialReturn MaterialReturn = null;
                if (MatRetId > 0)
                    MaterialReturn = PSF.Get<MaterialReturn>(MatRetId);
                else { throw new Exception("MatRetId is required and it cannot be 0"); }
                return MaterialReturn;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IList<MaterialReturnList> CreateOrUpdateMaterialReturnList(IList<MaterialReturnList> mrl)
        {
            try
            {
                if (mrl != null)
                    PSF.SaveOrUpdate<MaterialReturnList>(mrl);
                else { throw new Exception("MaterialReturnList is required and it cannot be null.."); }
                return mrl;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MaterialReturnList>> GetMaterialReturnListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialReturnList>> retValue = new Dictionary<long, IList<MaterialReturnList>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialReturnList>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MaterialReturn>> GetMaterialReturnWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialReturn>> retValue = new Dictionary<long, IList<MaterialReturn>>();
                return PSF.GetListWithSearchCriteriaCount<MaterialReturn>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        #region Admin Template Report
        public Dictionary<long, IList<StoreReportForAdminTemplate_vw>> GetStoreReportForAdminTemplate_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StoreReportForAdminTemplate_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StoreInwardReportForAdminTemplate_vw>> GetStoreInwardReportForAdminTemplate_vwListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StoreInwardReportForAdminTemplate_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Added on 21/10/2014
        public Dictionary<long, IList<MaterialGroupMaster>> GetMaterialGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<MaterialGroupMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<MaterialSubGroupMaster>> GetMaterialSubGroupMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<MaterialSubGroupMaster>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //public Dictionary<long, IList<MaterialsMaster>> GetMaterialsMasterListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, Dictionary<string, object> criteria)
        //{
        //    try
        //    {
        //        return PSF.GetListWithEQSearchCriteriaCount<MaterialsMaster>(page, pageSize, sortType, sortby, criteria);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        #endregion

        public Dictionary<long, IList<MaterialsMaster>> GetAutoCompleteMaterialsMasterlistWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialsMaster>> retValue = new Dictionary<long, IList<MaterialsMaster>>();
                return PSF.GetListWithLikeSearchCriteriaCount<MaterialsMaster>(page, pageSize, sortBy, sortType, criteria);
            }

            catch (Exception)
            {

                throw;
            }
        }

        # region Material Distribution by vinoth
        public long CreateOrUpdateMaterialDistribution(MaterialDistribution md)
        {
            try
            {
                if (md != null)
                    PSF.SaveOrUpdate<MaterialDistribution>(md);
                else { throw new Exception("MaterialDistributionId is required and it cannot be null.."); }
                return md.MaterialDistributionId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<MaterialDistribution_Vw>> GetMaterialDistributionListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                Dictionary<long, IList<MaterialDistribution_Vw>> retValue = new Dictionary<long, IList<MaterialDistribution_Vw>>();
                return PSF.GetListWithEQSearchCriteriaCount<MaterialDistribution_Vw>(page, pageSize, sortType, sortBy, criteria);
            }

            catch (Exception)
            {
                throw;
            }
        }


        public Dictionary<long, IList<StudentMaterialDistribution_vw>> GetStudentMaterialDistribution_vwListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentMaterialDistribution_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Dictionary<long, IList<MaterialIssueDetails>> GetMaterialIssueDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<MaterialIssueDetails>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public MaterialIssueDetails GetMaterialIssueDetailsById(long IssueId)
        {
            try
            {
                return PSF.Get<MaterialIssueDetails>("IssueId", IssueId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public long CreateOrUpdateMaterialIssueDetails(MaterialIssueDetails materialissue)
        {
            try
            {
                if (materialissue != null)
                    PSF.SaveOrUpdate<MaterialIssueDetails>(materialissue);
                else { throw new Exception("Issue is required and it cannot be null.."); }
                return materialissue.IssueId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteIssue(long[] IssueId)
        {
            try
            {
                IList<MaterialIssueDetails> tasksList = PSF.GetListByIds<MaterialIssueDetails>(IssueId);
                PSF.DeleteAll<MaterialIssueDetails>(tasksList);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Dictionary<long, IList<MaterialDistribution>> GetMaterialDistributionDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<MaterialDistribution>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MaterialDistribution GetMaterialConfigurationonMaterial(string AcademicYear, string Campus, string Grade, string Gender, string IsHosteller, string MaterialSubGroup)
        {
            try
            {
                return PSF.Get<MaterialDistribution>("AcademicYear", AcademicYear, "Campus", Campus, "Grade", Grade, "Gender", Gender, "IsHosteller", IsHosteller, "MaterialSubGroup", MaterialSubGroup);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<long, IList<StudentWiseMaterialReport>> GetStudentWiseMaterialReportListWithPagingAndCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentWiseMaterialReport>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StudentMaterialDistribution_vw GetStudentMaterialDistribution_vwStudentId(long StudId)
        {
            try
            {
                return PSF.Get<StudentMaterialDistribution_vw>("StudId", StudId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion        
        #region MaterialDistributionReport by Dhanabalan
        public Dictionary<long, IList<MaterialDistributionReport>> GetMaterialdistributionReportListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                Dictionary<long, IList<MaterialDistributionReport>> retValue = new Dictionary<long, IList<MaterialDistributionReport>>();
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<MaterialDistributionReport>(page, pageSize, sortType, sortBy, criteria, Likecriteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<MaterialDistributionReportByCampus>> GetMaterialdistributionReportByCampusListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                Dictionary<long, IList<MaterialDistributionReportByCampus>> retValue = new Dictionary<long, IList<MaterialDistributionReportByCampus>>();
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<MaterialDistributionReportByCampus>(page, pageSize, sortType, sortBy, criteria, Likecriteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<long, IList<MaterialDistributionReportByDate>> GetMaterialDistributionReportByDateListWithPagingAndCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> Likecriteria)
        {
            try
            {
                Dictionary<long, IList<MaterialDistributionReportByDate>> retValue = new Dictionary<long, IList<MaterialDistributionReportByDate>>();
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<MaterialDistributionReportByDate>(page, pageSize, sortType, sortBy, criteria, Likecriteria);
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Material Distribution over view by john naveen
        public Dictionary<long, IList<StudentMaterialOverView_vw>> GetStudentMaterialOverView_vwListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentMaterialOverView_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StudentMaterialOverView_vw GetStudentMaterialOverView_vwById(long Id)
        {
            try
            {
                StudentMaterialOverView_vw MaterialView = null;
                if (Id > 0)
                    MaterialView = PSF.Get<StudentMaterialOverView_vw>(Id);
                else { throw new Exception("MatRetId is required and it cannot be 0"); }
                return MaterialView;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
        #region Student Material Sub group View by john naveen
        public Dictionary<long, IList<StudentMaterialSubGroupView_vw>> GetStudentMaterialSubGroupView_vwListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentMaterialSubGroupView_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Material Distribution SubOver View
        public Dictionary<long, IList<StudentMaterialSubOverView_vw>> GetStudentMaterialSubOverView_vwListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StudentMaterialSubOverView_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Material Issue Modify by john naveen
        public MaterialsMaster GetMaterialsMasterById(long Id)
        {
            try
            {
                return PSF.Get<MaterialsMaster>("Id", Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public StoreStockBalance GetStoreStockBalanceById(long ItemId)
        {
            try
            {
                return PSF.Get<StoreStockBalance>("ItemId", ItemId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MaterialDistribution GetMaterialDistributionById(long Id)
        {
            try
            {
                return PSF.Get<MaterialDistribution>("MaterialDistributionId", Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<SkuList_vw>> GetSkuList_vwListWithExcactAndLikeSearchCriteria(int? page, int? pageSize, string sortType, string sortby, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<SkuList_vw>(page, pageSize, sortType, sortby, criteria);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
