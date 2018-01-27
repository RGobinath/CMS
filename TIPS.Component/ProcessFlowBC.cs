using System;
using System.Collections.Generic;
using System.Linq;
using PersistenceFactory;
using TIPS.Entities;
using TIPS.Entities.HRManagementEntities;
using TIPS.Entities.StaffEntities;
using TIPS.Entities.TicketingSystem;
using TIPS.Entities.TaskManagement;

namespace TIPS.Component
{
    public class ProcessFlowBC : IDisposable
    {
        PersistenceServiceFactory PSF;
        public ProcessFlowBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);
        }
        protected virtual void Dispose(bool disposing)
        {
            //this.Dispose();  
        }

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        #region CreateMethods
        public long CreateCallManagement(CallManagement CallManagement)
        {
            try
            {
                if (CallManagement != null)
                { PSF.SaveOrUpdate<CallManagement>(CallManagement); return CallManagement.Id; }
                else throw new Exception("FFExport Object is required and it cannot be empty.");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public long StartCallManagement(CallManagement CallManagement, string Template, string userId)
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
                CallManagement.InstanceId = pid;
                PSF.SaveOrUpdate<CallManagement>(CallManagement);
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
                    NextActivity.ProcessRefId = CallManagement.Id;
                    NextActivity.CreatedDate = DateTime.Now;
                    NextActivity.BranchCode = CallManagement.BranchCode;
                    NextActivity.DeptCode = CallManagement.DeptCode;
                    PSF.SaveOrUpdate<Activity>(NextActivity);
                }

                return CallManagement.Id;
            }
            catch (Exception) { throw; }
        }




        public bool RequestCallManagement(CallManagement CallManagement, string Template, string userId)
        {
            bool retValue = false;
            try
            {
                PSF.SaveOrUpdate<CallManagement>(CallManagement);
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "ActivityOrder", (long)3);
                Activity CurrentActivity = PSF.Get<Activity>("InstanceId", CallManagement.InstanceId, "ActivityName", "RequestExportJob");
                if (CurrentActivity != null)
                {
                    CurrentActivity.Completed = true;
                    PSF.SaveOrUpdate(CurrentActivity);
                }
                foreach (WorkFlowStatus wfs in WorkFlowStatusList)
                {
                    Activity NextActivity = new Activity();
                    NextActivity.ActivityName = wfs.WFStatus;
                    NextActivity.ActivityFullName = wfs.Description;
                    NextActivity.AppRole = wfs.Performer;
                    NextActivity.Completed = false;
                    NextActivity.Performer = userId;
                    NextActivity.TemplateId = WorkFlowTemplate.Id;
                    NextActivity.InstanceId = CallManagement.InstanceId;
                    NextActivity.CreatedDate = DateTime.Now;
                    //NextActivity.NextActivity = "RequestExportJob";
                    PSF.SaveOrUpdate<Activity>(NextActivity);
                }
                return retValue;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool JobCreationInControl(CallManagement CallManagement, string Template, string userId)
        {
            bool retValue = false;
            try
            {
                PSF.SaveOrUpdate<CallManagement>(CallManagement);
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                //close the current activity
                Activity CurrentActivity = PSF.Get<Activity>("InstanceId", CallManagement.InstanceId, "ActivityName", "JobCreationInControl");
                if (CurrentActivity != null)
                {
                    CurrentActivity.Completed = true;
                    PSF.SaveOrUpdate(CurrentActivity);
                }
                //trigger the next activity //we need to check whether parallel activities are there to complete
                //activities that are coming in the next order
                IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "ActivityOrder", (long)4);
                //checking parallel activities get the current order of activities
                IList<Activity> conditionWaitList = PSF.GetList<Activity>("TemplateId", WorkFlowTemplate.Id, "ActivityOrder", (long)3);
                if (conditionWaitList != null && conditionWaitList.Count > 0)
                {
                    bool? waiting = false;
                    foreach (Activity a in conditionWaitList)
                    {
                        if (a.Completed == false && waiting == false)
                        {
                            waiting = true;
                        }
                    } if (waiting == true) return retValue;
                }
                if (WorkFlowStatusList != null && WorkFlowStatusList.Count > 0)
                {
                    foreach (WorkFlowStatus wfs in WorkFlowStatusList)
                    {
                        Activity NextActivity = new Activity();
                        NextActivity.ActivityName = wfs.WFStatus;
                        NextActivity.ActivityFullName = wfs.Description;
                        NextActivity.AppRole = wfs.Performer;
                        NextActivity.Completed = false;
                        NextActivity.Performer = userId;
                        NextActivity.TemplateId = WorkFlowTemplate.Id;
                        NextActivity.InstanceId = CallManagement.InstanceId;
                        //NextActivity.NextActivity = "RequestExportJob";
                        PSF.SaveOrUpdate<Activity>(NextActivity);
                    }
                }
                return retValue;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //log issue 
        public bool CreateInformationActivity(CallManagement CallManagement, string Template, string userId, string ActivityName, string Notification, bool isHosteller)
        {
            PSF.SaveOrUpdate<CallManagement>(CallManagement);
            WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
            //close the current activity
            Activity CurrentActivity = PSF.Get<Activity>("InstanceId", CallManagement.InstanceId, "ActivityName", ActivityName, "Completed", false);
            if (CurrentActivity != null)
            {
                CurrentActivity.Completed = true;
                CurrentActivity.Available = false;
                CurrentActivity.Assigned = false;
                CurrentActivity.Performer = userId;
                CurrentActivity.CreatedDate = DateTime.Now;
                CurrentActivity.BranchCode = CallManagement.BranchCode;
                // CurrentActivity.DeptCode = Notification == "SickLeaveNotification" ? isHosteller ? "HOSTEL" : "DAYSCHOLAR" : "TRANSPORT";
                PSF.SaveOrUpdate(CurrentActivity);
                IList<WorkFlowStatus> WorkFlowStatusList = null;
                //get the information activity
                if (Notification == "ParentVisit")
                {
                    WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "WFStatus", "ParentVisit");
                }
                else if (Notification == "LeaveNotification")
                {
                    if (isHosteller)
                        WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "WFStatus", "LeaveNotification-Hostel");
                    else WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "WFStatus", "LeaveNotification-Dayscholar");
                }
                else if (Notification == "ParentPickup")
                {
                    WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "WFStatus", "ParentPickup");
                }
                else if (Notification == "TransportPickup")
                {
                    WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "WFStatus", "TransportPickup");
                }
                else if (Notification == "GeneralInfo")
                {
                    WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "WFStatus", "GeneralInfo");
                }
                //WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "WFStatus", "Information");
                if (WorkFlowStatusList != null && WorkFlowStatusList.Count > 0)
                {
                    foreach (WorkFlowStatus wfs in WorkFlowStatusList)
                    {
                        Activity NextActivity = new Activity();
                        if (wfs.IsRejectionRequired == true)
                        { NextActivity.IsRejApplicable = true; }

                        NextActivity.ActivityName = wfs.WFStatus;
                        NextActivity.ActivityFullName = wfs.Description;
                        NextActivity.AppRole = wfs.Performer;
                        NextActivity.Completed = wfs.WFStatus == "Complete" ? true : false;
                        NextActivity.Available = wfs.WFStatus != "Complete" ? true : false;
                        NextActivity.Performer = userId;
                        NextActivity.TemplateId = WorkFlowTemplate.Id;
                        NextActivity.InstanceId = CallManagement.InstanceId;
                        NextActivity.NextActOrder = wfs.NextActOrder;
                        NextActivity.ActivityOrder = wfs.ActivityOrder;
                        NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                        NextActivity.ProcessRefId = CallManagement.Id;
                        NextActivity.CreatedDate = DateTime.Now;
                        NextActivity.BranchCode = CallManagement.BranchCode;
                        // NextActivity.DeptCode = Notification == "SickLeaveNotification" ? isHosteller ? "SNHOSTEL" : "DAYSCHOLAR" : "TRANSPORTPICKUP";
                        if (Notification == "LeaveNotification" && isHosteller == true)
                            NextActivity.DeptCode = "SNHOSTEL";

                        else if (Notification == "ParentVisit")
                            NextActivity.DeptCode = "PARENT";

                        else if (Notification == "ParentPickup")
                            NextActivity.DeptCode = "PARENTPICKUP";

                        else if (Notification == "TransportPickup")
                            NextActivity.DeptCode = "TRANSPORTPICKUP";

                        else if (Notification == "GeneralInfo")
                            NextActivity.DeptCode = "GeneralInfo";
                        else
                            NextActivity.DeptCode = "DAYSCHOLAR";
                        PSF.SaveOrUpdate<Activity>(NextActivity);
                    }
                }
            }
            return true;
        }
        public bool CompleteInformationActivity(CallManagement CallManagement, string Template, string userId, string ActivityName)
        {
            bool retValue = false;
            PSF.SaveOrUpdate<CallManagement>(CallManagement);
            //WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template, "ActivityName", ActivityName);
            //close the current activity
            Activity CurrentActivity = PSF.Get<Activity>("InstanceId", CallManagement.InstanceId, "ActivityName", ActivityName, "Completed", false);
            if (CurrentActivity != null)
            {
                CurrentActivity.Completed = true;
                CurrentActivity.Available = false;
                CurrentActivity.Assigned = false;
                CurrentActivity.Performer = userId;
                PSF.SaveOrUpdate(CurrentActivity);
                ProcessInstance pi = PSF.Get<ProcessInstance>(CurrentActivity.InstanceId);
                pi.Status = "Completed";
                PSF.SaveOrUpdate(pi);
                retValue = true;
            }
            return retValue;
        }
        public bool BulkCompleteInformationActivity(string Template, string userId, long[] ActivityId)
        {
            bool retValue = false;
            IList<Activity> CurrentActivity = PSF.GetListById<Activity>("Id", ActivityId);
            if (CurrentActivity != null && CurrentActivity.Count > 0 && CurrentActivity.First() != null)
            {
                foreach (Activity ac in CurrentActivity)
                {
                    ac.Completed = true;
                    ac.Available = false;
                    ac.Assigned = false;
                    ac.Performer = userId;
                    PSF.SaveOrUpdate(ac);
                    ProcessInstance pi = PSF.Get<ProcessInstance>(ac.InstanceId);
                    pi.Status = "Completed";
                    PSF.SaveOrUpdate(pi);
                    retValue = true;
                }
            }
            return retValue;
        }

        //public bool CompleteActivityCallManagement(CallManagement CallManagement, string Template, string userId, string ActivityName, bool isRejection)
        //{
        //    bool retValue = false;
        //    try
        //    {
        //        PSF.SaveOrUpdate<CallManagement>(CallManagement);
        //        WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
        //        //close the current activity
        //        Activity CurrentActivity = PSF.Get<Activity>("InstanceId", CallManagement.InstanceId, "ActivityName", ActivityName, "Completed", false);
        //        if (CurrentActivity != null)
        //        {
        //            //if current activity doesnt need any rejection and there is no rejection then it will be completed
        //            //otherwise once the rejection resolved then it will be completed
        //            //this need to be obdated as waiting
        //            if (CurrentActivity.IsRejApplicable == true && isRejection == true)
        //            {
        //                CurrentActivity.Completed = true;
        //                CurrentActivity.Available = false;
        //                CurrentActivity.Assigned = false;
        //                PSF.SaveOrUpdate(CurrentActivity);
        //            }
        //            else
        //            {
        //                Activity WaitingAct = PSF.Get<Activity>("WaitingFor", CurrentActivity.Id);
        //                if (WaitingAct != null)
        //                {
        //                    CurrentActivity.Waiting = false;
        //                    PSF.SaveOrUpdate(CurrentActivity);
        //                    WaitingAct.Completed = true;
        //                    WaitingAct.Assigned = false;
        //                    WaitingAct.Available = false;
        //                }

        //                CurrentActivity.Completed = true;
        //                CurrentActivity.Available = false;
        //                CurrentActivity.Assigned = false;
        //                CurrentActivity.Performer = userId;
        //                PSF.SaveOrUpdate(CurrentActivity);
        //            }
        //        }
        //        //trigger the next activity //we need to check whether parallel activities are there to complete
        //        //activities that are coming in the next order
        //        IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "ActivityOrder", CurrentActivity.NextActOrder);
        //        //checking parallel activities get the current order of activities
        //        Dictionary<string, object> WaitCriteria = new Dictionary<string, object>();
        //        WaitCriteria.Add("TemplateId", WorkFlowTemplate.Id);
        //        WaitCriteria.Add("ActivityOrder", CurrentActivity.ActivityOrder);
        //        WaitCriteria.Add("InstanceId", CurrentActivity.InstanceId);
        //        Dictionary<long, IList<Activity>> conditionList = PSF.GetListWithSearchCriteriaCount<Activity>(0, 100, string.Empty, string.Empty, WaitCriteria);
        //        if (conditionList != null && conditionList.Count > 0)
        //        {
        //            IList<Activity> conditionWaitList = conditionList.First().Value;
        //            bool? waiting = false;
        //            foreach (Activity a in conditionWaitList)
        //            {
        //                if (a.Completed == false && waiting == false)
        //                {
        //                    waiting = true;
        //                }
        //            } retValue = true;
        //            if (waiting == true)
        //            {
        //                if (CurrentActivity.IsRejApplicable == true && isRejection == true)
        //                { }
        //                else
        //                    return retValue;
        //            }
        //        }
        //        if (WorkFlowStatusList != null && WorkFlowStatusList.Count > 0)
        //        {
        //            //if it is rejection flow then build the logic here
        //            //{logic goes here }


        //            foreach (WorkFlowStatus wfs in WorkFlowStatusList)
        //            {
        //                //Rejection Activity
        //                if (CurrentActivity.IsRejApplicable == true && isRejection == true)
        //                {
        //                    WorkFlowStatus wfsRej = PSF.Get<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "RejectionFor", CurrentActivity.ActivityOrder);
        //                    if (wfsRej != null)
        //                    {
        //                        Activity NextActivityRej = new Activity();
        //                        NextActivityRej.ActivityName = wfsRej.WFStatus;
        //                        if (NextActivityRej.ActivityName == "Complete")
        //                        {
        //                            NextActivityRej.Completed = true;
        //                            ProcessInstance pi = PSF.Get<ProcessInstance>(CallManagement.InstanceId);
        //                            pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
        //                        }
        //                        else NextActivityRej.Completed = false;
        //                        NextActivityRej.ActivityFullName = wfsRej.Description;
        //                        NextActivityRej.AppRole = wfsRej.Performer;

        //                        NextActivityRej.Performer = userId;
        //                        NextActivityRej.TemplateId = WorkFlowTemplate.Id;
        //                        NextActivityRej.InstanceId = CallManagement.InstanceId;
        //                        NextActivityRej.NextActOrder = wfsRej.NextActOrder;
        //                        NextActivityRej.ActivityOrder = wfsRej.ActivityOrder;
        //                        NextActivityRej.PreviousActOrder = wfsRej.PreviousActOrder;
        //                        NextActivityRej.ProcessRefId = CallManagement.Id;
        //                        NextActivityRej.RejectionFor = CurrentActivity.Id;
        //                        PSF.SaveOrUpdate<Activity>(NextActivityRej);
        //                        //CurrentActivity.WaitingFor = NextActivityRej.Id;
        //                        //PSF.SaveOrUpdate(CurrentActivity);
        //                    }
        //                }
        //                else
        //                {
        //                    Activity NextActivity = new Activity();
        //                    if (wfs.IsRejectionRequired == true)
        //                    { NextActivity.IsRejApplicable = true; }

        //                    NextActivity.ActivityName = wfs.WFStatus;
        //                    NextActivity.ActivityFullName = wfs.Description;
        //                    NextActivity.AppRole = wfs.Performer;
        //                    NextActivity.Completed = wfs.WFStatus == "Complete" ? true : false;
        //                    NextActivity.Available = wfs.WFStatus != "Complete" ? true : false;
        //                    NextActivity.Performer = userId;
        //                    NextActivity.TemplateId = WorkFlowTemplate.Id;
        //                    NextActivity.InstanceId = CallManagement.InstanceId;
        //                    NextActivity.NextActOrder = wfs.NextActOrder;
        //                    NextActivity.ActivityOrder = wfs.ActivityOrder;
        //                    NextActivity.PreviousActOrder = wfs.PreviousActOrder;
        //                    NextActivity.ProcessRefId = CallManagement.Id;
        //                    PSF.SaveOrUpdate<Activity>(NextActivity);
        //                }
        //            } retValue = true;
        //        }
        //        return retValue;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public bool CompleteActivityCallManagement(CallManagement CallManagement, string Template, string userId, string ActivityName, bool isRejection)
        {
            bool retValue = false;
            try
            {
                PSF.SaveOrUpdate<CallManagement>(CallManagement);
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                //close the current activity
                Activity CurrentActivity = PSF.Get<Activity>("InstanceId", CallManagement.InstanceId, "ActivityName", ActivityName, "Completed", false);
                if (CurrentActivity != null)
                {
                    //if current activity doesnt need any rejection and there is no rejection then it will be completed
                    //otherwise once the rejection resolved then it will be completed
                    //this need to be obdated as waiting
                    if (CurrentActivity.IsRejApplicable == true && isRejection == true)
                    {
                        CurrentActivity.BranchCode = CallManagement.BranchCode;
                        CurrentActivity.DeptCode = CallManagement.DeptCode;
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
                            CurrentActivity.BranchCode = CallManagement.BranchCode;
                            CurrentActivity.DeptCode = CallManagement.DeptCode;
                            CurrentActivity.Waiting = false;
                            PSF.SaveOrUpdate(CurrentActivity);
                            WaitingAct.Completed = true;
                            WaitingAct.Assigned = false;
                            WaitingAct.Available = false;
                        }
                        CurrentActivity.BranchCode = CallManagement.BranchCode;
                        CurrentActivity.DeptCode = CallManagement.DeptCode;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        CurrentActivity.Performer = userId;
                        if (CurrentActivity.ActivityName == "Complete")
                        {
                            CallManagement.IsIssueCompleted = true;
                            PSF.Update<CallManagement>(CallManagement);
                            ProcessInstance pi = PSF.Get<ProcessInstance>(CallManagement.InstanceId);
                            pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                            CallManagement.Status = "Completed";
                            PSF.SaveOrUpdate<CallManagement>(CallManagement);
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
                                    //ProcessInstance pi = PSF.Get<ProcessInstance>(CallManagement.InstanceId);
                                    //pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                                    CallManagement.Status = NextActivityRej.ActivityName;
                                    PSF.SaveOrUpdate<CallManagement>(CallManagement);
                                }
                                else NextActivityRej.Completed = false;
                                NextActivityRej.ActivityFullName = wfsRej.Description;
                                NextActivityRej.AppRole = wfsRej.Performer;

                                NextActivityRej.Performer = userId;
                                NextActivityRej.TemplateId = WorkFlowTemplate.Id;
                                NextActivityRej.InstanceId = CallManagement.InstanceId;
                                NextActivityRej.NextActOrder = wfsRej.NextActOrder;
                                NextActivityRej.ActivityOrder = wfsRej.ActivityOrder;
                                NextActivityRej.PreviousActOrder = wfsRej.PreviousActOrder;
                                NextActivityRej.ProcessRefId = CallManagement.Id;
                                NextActivityRej.RejectionFor = CurrentActivity.Id;
                                NextActivityRej.Completed = false;
                                NextActivityRej.Available = true;
                                NextActivityRej.Assigned = false;
                                NextActivityRej.BranchCode = CallManagement.BranchCode;
                                NextActivityRej.DeptCode = CallManagement.DeptCode;
                                PSF.SaveOrUpdate<Activity>(NextActivityRej);
                                CallManagement.Status = NextActivityRej.ActivityName;
                                PSF.SaveOrUpdate<CallManagement>(CallManagement);
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
                            NextActivity.InstanceId = CallManagement.InstanceId;
                            NextActivity.NextActOrder = wfs.NextActOrder;
                            NextActivity.ActivityOrder = wfs.ActivityOrder;
                            NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                            NextActivity.ProcessRefId = CallManagement.Id;
                            NextActivity.Available = true;
                            NextActivity.Assigned = false;
                            NextActivity.Completed = false;
                            NextActivity.BranchCode = CallManagement.BranchCode;
                            NextActivity.DeptCode = CallManagement.DeptCode;
                            PSF.SaveOrUpdate<Activity>(NextActivity);
                            CallManagement.Status = NextActivity.ActivityName;
                            PSF.SaveOrUpdate<CallManagement>(CallManagement);
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
        public bool BulkCompleteActivityCallManagement(long[] ActivityId, string Template, string userId)
        {
            try
            {
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                Criteria.Add("Id", ActivityId);
                //add the criteria of activity name itself so that it will get only those avtivities
                Criteria.Add("ActivityName", "Complete");

                Dictionary<long, IList<Activity>> ActivityList = PSF.GetListWithSearchCriteriaCount<Activity>(0, 100, string.Empty, string.Empty, Criteria);
                if (ActivityList != null && ActivityList.First().Value != null && ActivityList.First().Value.Count > 0)
                {
                    foreach (Activity a in ActivityList.First().Value)
                    {
                        //set the status of call management
                        CallManagement cm = PSF.Get<CallManagement>("InstanceId", a.InstanceId);
                        cm.IsIssueCompleted = true;
                        cm.Status = "Completed";
                        PSF.SaveOrUpdate<CallManagement>(cm);
                        //set the status of process instance to complete
                        ProcessInstance pi = PSF.Get<ProcessInstance>(a.InstanceId);
                        pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                        //complete the activity
                        a.Completed = true;
                        a.Available = false;
                        a.Assigned = false;
                        a.Performer = userId;
                        PSF.SaveOrUpdate<Activity>(a);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public bool BulkInfoCompleteActivityCallManagement(long[] ActivityIds, string Template, string userId)
        {
            try
            {
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                Criteria.Add("Id", ActivityIds);
                // Criteria.Add("Performer", userId);
                //add the criteria of activity name itself so that it will get only those avtivities
                Dictionary<long, IList<Activity>> ActivityList = PSF.GetListWithSearchCriteriaCount<Activity>(0, 100, string.Empty, string.Empty, Criteria);
                if (ActivityList != null && ActivityList.First().Value != null && ActivityList.First().Value.Count > 0)
                {
                    foreach (Activity a in ActivityList.First().Value)
                    {
                        //set the status of call management
                        CallManagement cm = PSF.Get<CallManagement>("InstanceId", a.InstanceId);
                        cm.IsIssueCompleted = true;
                        cm.Status = "Completed";
                        PSF.SaveOrUpdate<CallManagement>(cm);
                        //set the status of process instance to complete
                        ProcessInstance pi = PSF.Get<ProcessInstance>(a.InstanceId);

                        pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                        //complete the activity
                        a.Completed = true;
                        a.Available = false;
                        a.Assigned = false;
                        a.Performer = userId;
                        PSF.SaveOrUpdate<Activity>(a);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public long CreateProcessInstance(ProcessInstance ProcessInstance)
        {
            try
            {
                if (ProcessInstance != null)
                { PSF.SaveOrUpdate<ProcessInstance>(ProcessInstance); return ProcessInstance.Id; }
                else throw new Exception("ProcessInstance Object is required and it cannot be empty.");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public long CreateWorkFlowStatus(WorkFlowStatus WorkFlowStatus)
        {
            try
            {
                if (WorkFlowStatus != null)
                { PSF.SaveOrUpdate<WorkFlowStatus>(WorkFlowStatus); return WorkFlowStatus.Id; }
                else throw new Exception("WorkFlowStatus Object is required and it cannot be empty.");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public long CreateActivity(Activity Activity)
        {
            try
            {
                if (Activity != null)
                { PSF.SaveOrUpdate<Activity>(Activity); return Activity.Id; }
                else throw new Exception("Activity Object is required and it cannot be empty.");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public bool AssignActivity(long activityId, string userId)
        {
            if (activityId > 0 && !string.IsNullOrEmpty(userId))
            {
                Activity AssignActivity = PSF.Get<Activity>(activityId);
                if (AssignActivity.Completed != true)
                {
                    AssignActivity.Assigned = true;
                }
                AssignActivity.Available = false;
                AssignActivity.Performer = userId;
                PSF.SaveOrUpdate<Activity>(AssignActivity);
                return true;
            }
            else return false;
        }
        public bool AssignActivityCheckBeforeAssigning(long activityId, string userId)
        {
            try
            {
                if (activityId > 0 && !string.IsNullOrEmpty(userId))
                {
                    Activity AssignActivity = PSF.Get<Activity>(activityId);
                    if (AssignActivity.Assigned == true && !string.IsNullOrEmpty(AssignActivity.Performer) && AssignActivity.Performer.ToUpper().Trim() != userId.ToUpper().Trim())
                    {
                        throw new Exception("This activity already assigned to " + AssignActivity.Performer + "");
                    }
                    if (AssignActivity.Completed != true)
                    {
                        AssignActivity.Assigned = true;
                    }
                    AssignActivity.Available = false;
                    AssignActivity.Performer = userId;
                    PSF.SaveOrUpdate<Activity>(AssignActivity);
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetMethods
        public ProcessInstance GetProcessInstanceById(long Id)
        {
            try
            {
                if (Id > 0)
                    return PSF.Get<ProcessInstance>(Id);
                else throw new Exception("Id is required and it cannot be zero.");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public WorkFlowStatus GetWorkFlowStatusById(long Id)
        {
            try
            {
                if (Id > 0)
                    return PSF.Get<WorkFlowStatus>(Id);
                else throw new Exception("Id is required and it cannot be zero.");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Activity GetActivityById(long Id)
        {
            try
            {
                if (Id > 0)
                    return PSF.Get<Activity>(Id);
                else throw new Exception("Id is required and it cannot be zero.");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public CallManagement GetCallManagementById(long Id)
        {
            try
            {
                if (Id > 0)
                    return PSF.Get<CallManagement>(Id);
                else throw new Exception("Id is required and it cannot be zero.");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<ProcessInstance>> GetProcessInstanceListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<ProcessInstance>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<CallMgmntPIView>> GetProcessInstanceViewListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithExactSearchCriteriaCount<CallMgmntPIView>(page, pageSize, sortBy, sortType, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<WorkFlowStatus>> GetWorkFlowStatusListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<WorkFlowStatus>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<CallMgmntActivity>> GetActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArrayExactSearch<CallMgmntActivity>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<TicketSystemActivity>> GetTicketSystemActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<TicketSystemActivity>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        #endregion

        public string StartETicketingSystem(TicketSystem TicketSystem, string Template, string userId)
        {
            try
            {
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
                TicketSystem.InstanceId = pid;
                PSF.SaveOrUpdate<TicketSystem>(TicketSystem);
                string ticketNo = "ETicket-" + TicketSystem.Id;
                TicketSystem.TicketNo = ticketNo;
                PSF.SaveOrUpdate<TicketSystem>(TicketSystem);
                IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "ActivityOrder", 1);
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
                    NextActivity.ProcessRefId = TicketSystem.Id;
                    NextActivity.CreatedDate = DateTime.Now;
                    NextActivity.BranchCode = TicketSystem.BranchCode;
                    NextActivity.DeptCode = TicketSystem.DeptCode;
                    if (wfs.IsRejectionRequired == true)
                    { NextActivity.IsRejApplicable = true; }
                    PSF.SaveOrUpdate<Activity>(NextActivity);
                }
                return TicketSystem.TicketNo;
            }
            catch (Exception) { throw; }
        }

        public string ReopenActivityTicketSystem(TicketSystem TicketSystem, string Template, string userId, string ActivityName, bool isRejection)
        {
            //bool retValue = false;
            try
            {
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                //creating process instance
                ProcessInstance pi = new ProcessInstance();
                pi.CreatedBy = userId;
                pi.DateCreated = DateTime.Now;
                pi.TemplateId = WorkFlowTemplate.Id;
                pi.Status = "Activated";
                PSF.SaveOrUpdate<ProcessInstance>(pi);
                long pid = TicketSystem.InstanceId;

                Activity CurrentActivity = PSF.Get<Activity>("InstanceId", TicketSystem.InstanceId, "ActivityName", ActivityName, "Completed", true);

                if (CurrentActivity.IsRejApplicable == true && isRejection == true)
                {
                    CurrentActivity.BranchCode = TicketSystem.BranchCode;
                    CurrentActivity.DeptCode = TicketSystem.DeptCode;
                    CurrentActivity.Completed = false;
                    CurrentActivity.Available = false;
                    CurrentActivity.Assigned = false;
                    CurrentActivity.Performer = userId;
                    PSF.SaveOrUpdate(CurrentActivity);
                }
                IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "ActivityOrder", 7);
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
                    NextActivity.PreviousActOrder = 6;
                    NextActivity.ProcessRefId = TicketSystem.Id;
                    NextActivity.CreatedDate = DateTime.Now;
                    NextActivity.BranchCode = TicketSystem.BranchCode;
                    NextActivity.DeptCode = TicketSystem.DeptCode;
                    if (wfs.IsRejectionRequired == true)
                    { NextActivity.IsRejApplicable = true; }
                    PSF.SaveOrUpdate<Activity>(NextActivity);
                }
                return TicketSystem.TicketNo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CompleteActivityTicketSystem(TicketSystem TicketSystem, string Template, string userId, string ActivityName, bool isRejection)
        {
            bool retValue = false;
            try
            {
                PSF.SaveOrUpdate<TicketSystem>(TicketSystem);
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                //close the current activity
                if (TicketSystem.TicketStatus == "RESOLVED" && ActivityName == "ReopenETicket")
                {
                    Activity ReopenCompleteActivity = PSF.Get<Activity>("InstanceId", TicketSystem.InstanceId, "ActivityName", "CloseETicket", "Completed", true);
                    if (ReopenCompleteActivity != null)
                    {
                        ReopenCompleteActivity.BranchCode = TicketSystem.BranchCode;
                        ReopenCompleteActivity.DeptCode = TicketSystem.DeptCode;
                        ReopenCompleteActivity.Completed = false;
                        ReopenCompleteActivity.Available = true;
                        ReopenCompleteActivity.Assigned = false;
                        ReopenCompleteActivity.Performer = userId;
                        PSF.SaveOrUpdate(ReopenCompleteActivity);
                        TicketSystem.Status = ReopenCompleteActivity.ActivityName;
                        PSF.SaveOrUpdate<TicketSystem>(TicketSystem);
                    }

                    Activity ReopenCloseActivity = PSF.Get<Activity>("InstanceId", TicketSystem.InstanceId, "ActivityName", "ReopenETicket", "Assigned", true);
                    if (ReopenCloseActivity != null)
                    {
                        ReopenCloseActivity.BranchCode = TicketSystem.BranchCode;
                        ReopenCloseActivity.DeptCode = TicketSystem.DeptCode;
                        ReopenCloseActivity.Completed = true;
                        ReopenCloseActivity.Available = false;
                        ReopenCloseActivity.Assigned = false;
                        ReopenCloseActivity.Performer = userId;
                        PSF.SaveOrUpdate(ReopenCloseActivity);
                        TicketSystem.Status = ReopenCloseActivity.ActivityName;
                        PSF.SaveOrUpdate<TicketSystem>(TicketSystem);
                    }
                    return true;
                }
                Activity CurrentActivity = PSF.Get<Activity>("InstanceId", TicketSystem.InstanceId, "ActivityName", ActivityName, "Completed", false);
                if (CurrentActivity != null)
                {
                    //if current activity doesnt need any rejection and there is no rejection then it will be completed
                    //otherwise once the rejection resolved then it will be completed
                    //this need to be obdated as waiting
                    if (CurrentActivity.IsRejApplicable == true && isRejection == true)
                    {
                        CurrentActivity.BranchCode = TicketSystem.BranchCode;
                        CurrentActivity.DeptCode = TicketSystem.DeptCode;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        CurrentActivity.Performer = userId;
                        PSF.SaveOrUpdate(CurrentActivity);

                        TicketSystem.Status = CurrentActivity.ActivityName;
                        PSF.SaveOrUpdate<TicketSystem>(TicketSystem);

                    }
                    if (ActivityName == "ReopenETicket" && isRejection == false)
                    {
                        Activity ReopenActivity = PSF.Get<Activity>("InstanceId", TicketSystem.InstanceId, "ActivityName", "ReopenETicket", "Completed", false);
                        ReopenActivity.BranchCode = TicketSystem.BranchCode;
                        ReopenActivity.DeptCode = TicketSystem.DeptCode;
                        ReopenActivity.Completed = true;
                        ReopenActivity.Available = false;
                        ReopenActivity.Assigned = false;
                        ReopenActivity.Performer = userId;
                        PSF.SaveOrUpdate(ReopenActivity);

                        Activity ReopenCloseActivity = PSF.Get<Activity>("InstanceId", TicketSystem.InstanceId, "ActivityName", "CloseETicket", "Completed", true);
                        ReopenCloseActivity.BranchCode = TicketSystem.BranchCode;
                        ReopenCloseActivity.DeptCode = TicketSystem.DeptCode;
                        ReopenCloseActivity.Completed = false;
                        ReopenCloseActivity.Available = true;
                        ReopenCloseActivity.Assigned = false;
                        ReopenCloseActivity.Performer = userId;
                        PSF.SaveOrUpdate(ReopenCloseActivity);
                        return true;
                    }

                    else
                    {
                        Activity WaitingAct = PSF.Get<Activity>("WaitingFor", CurrentActivity.Id);
                        if (WaitingAct != null)
                        {
                            CurrentActivity.BranchCode = TicketSystem.BranchCode;
                            CurrentActivity.DeptCode = TicketSystem.DeptCode;
                            CurrentActivity.Waiting = false;
                            PSF.SaveOrUpdate(CurrentActivity);
                            WaitingAct.Completed = true;
                            WaitingAct.Assigned = false;
                            WaitingAct.Available = false;
                        }
                        CurrentActivity.BranchCode = TicketSystem.BranchCode;
                        CurrentActivity.DeptCode = TicketSystem.DeptCode;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        CurrentActivity.Performer = userId;


                        if (CurrentActivity.ActivityName == "CompleteETicket")
                        {
                            TicketSystem.IsTicketCompleted = true;
                            PSF.Update<TicketSystem>(TicketSystem);
                            ProcessInstance pi = PSF.Get<ProcessInstance>(TicketSystem.InstanceId);
                            pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                            TicketSystem.Status = "Completed";
                            PSF.SaveOrUpdate<TicketSystem>(TicketSystem);
                        }
                        PSF.SaveOrUpdate(CurrentActivity);
                    }
                }
                //trigger the next activity //we need to check whether parallel activities are there to complete
                //activities that are coming in the next order
                if (CurrentActivity != null)
                {
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
                                    if (NextActivityRej.ActivityName == "CompleteETicket")
                                    {
                                        NextActivityRej.Completed = true;
                                        //ProcessInstance pi = PSF.Get<ProcessInstance>(CallManagement.InstanceId);
                                        //pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                                        TicketSystem.Status = NextActivityRej.ActivityName;
                                        PSF.SaveOrUpdate<TicketSystem>(TicketSystem);
                                    }
                                    else NextActivityRej.Completed = false;
                                    NextActivityRej.ActivityFullName = wfsRej.Description;
                                    NextActivityRej.AppRole = wfsRej.Performer;

                                    NextActivityRej.Performer = userId;
                                    NextActivityRej.TemplateId = WorkFlowTemplate.Id;
                                    NextActivityRej.InstanceId = TicketSystem.InstanceId;
                                    NextActivityRej.NextActOrder = wfsRej.NextActOrder;
                                    NextActivityRej.ActivityOrder = wfsRej.ActivityOrder;
                                    NextActivityRej.PreviousActOrder = wfsRej.PreviousActOrder;
                                    NextActivityRej.ProcessRefId = TicketSystem.Id;
                                    NextActivityRej.RejectionFor = CurrentActivity.Id;
                                    NextActivityRej.Completed = false;
                                    NextActivityRej.Available = true;
                                    NextActivityRej.Assigned = false;
                                    NextActivityRej.BranchCode = TicketSystem.BranchCode;
                                    NextActivityRej.DeptCode = TicketSystem.DeptCode;

                                    Activity CheckCurrentActivity = PSF.Get<Activity>("InstanceId", TicketSystem.InstanceId, "ActivityName", "CompleteETicket", "Completed", true);
                                    if (CheckCurrentActivity == null)
                                    {
                                        PSF.SaveOrUpdate<Activity>(NextActivityRej);
                                    }
                                    else
                                    {
                                        CheckCurrentActivity.Performer = userId;
                                        CheckCurrentActivity.TemplateId = WorkFlowTemplate.Id;
                                        CheckCurrentActivity.InstanceId = TicketSystem.InstanceId;
                                        CheckCurrentActivity.NextActOrder = wfsRej.NextActOrder;
                                        CheckCurrentActivity.ActivityOrder = wfsRej.ActivityOrder;
                                        CheckCurrentActivity.PreviousActOrder = wfsRej.PreviousActOrder;
                                        CheckCurrentActivity.ProcessRefId = TicketSystem.Id;
                                        CheckCurrentActivity.RejectionFor = CurrentActivity.Id;
                                        CheckCurrentActivity.Completed = true;
                                        CheckCurrentActivity.Available = true;
                                        CheckCurrentActivity.Assigned = false;
                                        CheckCurrentActivity.BranchCode = TicketSystem.BranchCode;
                                        CheckCurrentActivity.DeptCode = TicketSystem.DeptCode;
                                        PSF.SaveOrUpdate<Activity>(CheckCurrentActivity);
                                    }
                                    TicketSystem.Status = NextActivityRej.ActivityName;
                                    PSF.SaveOrUpdate<TicketSystem>(TicketSystem);
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
                                NextActivity.InstanceId = TicketSystem.InstanceId;
                                NextActivity.NextActOrder = wfs.NextActOrder;
                                NextActivity.ActivityOrder = wfs.ActivityOrder;
                                NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                                NextActivity.ProcessRefId = TicketSystem.Id;
                                NextActivity.Available = true;
                                NextActivity.Assigned = false;
                                NextActivity.Completed = false;
                                NextActivity.BranchCode = TicketSystem.BranchCode;
                                NextActivity.DeptCode = TicketSystem.DeptCode;
                                Activity CheckCurrentActivity = PSF.Get<Activity>("InstanceId", TicketSystem.InstanceId, "ActivityName", "CompleteETicket", "Completed", false);
                                if (CheckCurrentActivity == null)
                                {
                                    PSF.SaveOrUpdate<Activity>(NextActivity);
                                }
                                else
                                {
                                    CheckCurrentActivity.Completed = true;
                                    CheckCurrentActivity.Available = false;
                                    CheckCurrentActivity.Assigned = false;
                                    PSF.SaveOrUpdate<Activity>(CheckCurrentActivity);
                                }
                                TicketSystem.Status = NextActivity.ActivityName;
                                PSF.SaveOrUpdate<TicketSystem>(TicketSystem);
                            }
                        } retValue = true;
                    }
                }
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long StartStaffIssueManagement(StaffIssues StaffIssues, string Template, string userId)
        {
            try
            {
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
                StaffIssues.InstanceId = pid;
                PSF.SaveOrUpdate<StaffIssues>(StaffIssues);
                IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "ActivityOrder", 2);
                foreach (WorkFlowStatus wfs in WorkFlowStatusList)
                {
                    StaffActivities NextActivity = new StaffActivities();
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
                    NextActivity.ProcessRefId = StaffIssues.Id;
                    NextActivity.CreatedDate = DateTime.Now;
                    NextActivity.BranchCode = StaffIssues.BranchCode;
                    NextActivity.DeptCode = StaffIssues.DeptCode;
                    PSF.SaveOrUpdate<StaffActivities>(NextActivity);
                }

                return StaffIssues.Id;
            }
            catch (Exception) { throw; }
        }
        //Commented By Prabakaran

        //public bool CompleteActivityStaffIssueManagement(StaffIssues StaffIssues, string Template, string userId, string ActivityName, bool isRejection)
        //{
        //    bool retValue = false;
        //    try
        //    {
        //        PSF.SaveOrUpdate<StaffIssues>(StaffIssues);
        //        WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
        //        //close the current activity
        //        StaffActivities CurrentActivity = PSF.Get<StaffActivities>("InstanceId", StaffIssues.InstanceId, "ActivityName", ActivityName, "Completed", false);
        //        if (CurrentActivity != null)
        //        {
        //            //if current activity doesnt need any rejection and there is no rejection then it will be completed
        //            //otherwise once the rejection resolved then it will be completed
        //            //this need to be obdated as waiting
        //            if (CurrentActivity.IsRejApplicable == true && isRejection == true)
        //            {
        //                CurrentActivity.BranchCode = StaffIssues.BranchCode;
        //                CurrentActivity.DeptCode = StaffIssues.DeptCode;
        //                CurrentActivity.Completed = true;
        //                CurrentActivity.Available = false;
        //                CurrentActivity.Assigned = false;
        //                PSF.SaveOrUpdate(CurrentActivity);
        //            }
        //            else
        //            {
        //                StaffActivities WaitingAct = PSF.Get<StaffActivities>("WaitingFor", CurrentActivity.Id);
        //                if (WaitingAct != null)
        //                {
        //                    CurrentActivity.BranchCode = StaffIssues.BranchCode;
        //                    CurrentActivity.DeptCode = StaffIssues.DeptCode;
        //                    CurrentActivity.Waiting = false;
        //                    PSF.SaveOrUpdate(CurrentActivity);
        //                    WaitingAct.Completed = true;
        //                    WaitingAct.Assigned = false;
        //                    WaitingAct.Available = false;
        //                }
        //                CurrentActivity.BranchCode = StaffIssues.BranchCode;
        //                CurrentActivity.DeptCode = StaffIssues.DeptCode;
        //                CurrentActivity.Completed = true;
        //                CurrentActivity.Available = false;
        //                CurrentActivity.Assigned = false;
        //                CurrentActivity.Performer = userId;
        //                if (CurrentActivity.ActivityName == "Complete")
        //                {
        //                    StaffIssues.IsIssueCompleted = true;
        //                    PSF.Update<StaffIssues>(StaffIssues);
        //                    ProcessInstance pi = PSF.Get<ProcessInstance>(StaffIssues.InstanceId);
        //                    pi.Status = "Completed";
        //                    PSF.SaveOrUpdate<ProcessInstance>(pi);
        //                    StaffIssues.Status = "Completed";
        //                    StaffIssues.ActivityFullName = "Completed";
        //                    PSF.SaveOrUpdate<StaffIssues>(StaffIssues);
        //                }
        //                PSF.SaveOrUpdate(CurrentActivity);
        //            }
        //        }
        //        //trigger the next activity //we need to check whether parallel activities are there to complete
        //        //activities that are coming in the next order
        //        IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "ActivityOrder", CurrentActivity.NextActOrder);
        //        //checking parallel activities get the current order of activities
        //        Dictionary<string, object> WaitCriteria = new Dictionary<string, object>();
        //        WaitCriteria.Add("TemplateId", WorkFlowTemplate.Id);
        //        WaitCriteria.Add("ActivityOrder", CurrentActivity.ActivityOrder);
        //        WaitCriteria.Add("InstanceId", CurrentActivity.InstanceId);
        //        Dictionary<long, IList<StaffActivities>> conditionList = PSF.GetListWithSearchCriteriaCount<StaffActivities>(0, 100, string.Empty, string.Empty, WaitCriteria);
        //        if (conditionList != null && conditionList.Count > 0)
        //        {
        //            IList<StaffActivities> conditionWaitList = conditionList.First().Value;
        //            bool? waiting = false;
        //            foreach (StaffActivities a in conditionWaitList)
        //            {
        //                if (a.Completed == false && waiting == false)
        //                {
        //                    waiting = true;
        //                }
        //            } retValue = true;
        //            if (waiting == true)
        //            {
        //                if (CurrentActivity.IsRejApplicable == true && isRejection == true)
        //                { }
        //                else
        //                    return retValue;
        //            }
        //        }
        //        if (WorkFlowStatusList != null && WorkFlowStatusList.Count > 0)
        //        {
        //            //if it is rejection flow then build the logic here
        //            //{logic goes here }


        //            foreach (WorkFlowStatus wfs in WorkFlowStatusList)
        //            {
        //                //Rejection Activity
        //                if (CurrentActivity.IsRejApplicable == true && isRejection == true)
        //                {
        //                    WorkFlowStatus wfsRej = PSF.Get<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "RejectionFor", CurrentActivity.ActivityOrder);
        //                    if (wfsRej != null)
        //                    {
        //                        StaffActivities NextActivityRej = new StaffActivities();
        //                        NextActivityRej.CreatedDate = DateTime.Now;
        //                        NextActivityRej.ActivityName = wfsRej.WFStatus;
        //                        if (NextActivityRej.ActivityName == "Complete")
        //                        {
        //                            NextActivityRej.Completed = true;
        //                            //ProcessInstance pi = PSF.Get<ProcessInstance>(CallManagement.InstanceId);
        //                            //pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
        //                            StaffIssues.Status = NextActivityRej.ActivityName;
        //                            StaffIssues.ActivityFullName = NextActivityRej.ActivityFullName;
        //                            PSF.SaveOrUpdate<StaffIssues>(StaffIssues);
        //                        }
        //                        else NextActivityRej.Completed = false;
        //                        NextActivityRej.ActivityFullName = wfsRej.Description;
        //                        NextActivityRej.AppRole = wfsRej.Performer;

        //                        //NextActivityRej.Performer = userId;
        //                        NextActivityRej.TemplateId = WorkFlowTemplate.Id;
        //                        NextActivityRej.InstanceId = StaffIssues.InstanceId;
        //                        NextActivityRej.NextActOrder = wfsRej.NextActOrder;
        //                        NextActivityRej.ActivityOrder = wfsRej.ActivityOrder;
        //                        NextActivityRej.PreviousActOrder = wfsRej.PreviousActOrder;
        //                        NextActivityRej.ProcessRefId = StaffIssues.Id;
        //                        NextActivityRej.RejectionFor = CurrentActivity.Id;
        //                        NextActivityRej.Completed = false;
        //                        NextActivityRej.Available = true;
        //                        NextActivityRej.Assigned = false;
        //                        NextActivityRej.BranchCode = StaffIssues.BranchCode;
        //                        NextActivityRej.DeptCode = StaffIssues.DeptCode;
        //                        PSF.SaveOrUpdate<StaffActivities>(NextActivityRej);
        //                        StaffIssues.Status = NextActivityRej.ActivityName;
        //                        StaffIssues.ActivityFullName = NextActivityRej.ActivityFullName;

        //                        PSF.SaveOrUpdate<StaffIssues>(StaffIssues);
        //                        //CurrentActivity.WaitingFor = NextActivityRej.Id;
        //                        //PSF.SaveOrUpdate(CurrentActivity);
        //                    }
        //                }
        //                else
        //                {
        //                    StaffActivities NextActivity = new StaffActivities();
        //                    NextActivity.CreatedDate = DateTime.Now;
        //                    if (wfs.IsRejectionRequired == true)
        //                    { NextActivity.IsRejApplicable = true; }

        //                    NextActivity.ActivityName = wfs.WFStatus;
        //                    NextActivity.ActivityFullName = wfs.Description;
        //                    NextActivity.AppRole = wfs.Performer;
        //                    NextActivity.Completed = wfs.WFStatus == "Complete" ? true : false;
        //                    NextActivity.Available = wfs.WFStatus != "Complete" ? true : false;
        //                    NextActivity.Performer = wfs.WFStatus == "Complete" ? userId : null;
        //                    NextActivity.TemplateId = WorkFlowTemplate.Id;
        //                    NextActivity.InstanceId = StaffIssues.InstanceId;
        //                    NextActivity.NextActOrder = wfs.NextActOrder;
        //                    NextActivity.ActivityOrder = wfs.ActivityOrder;
        //                    NextActivity.PreviousActOrder = wfs.PreviousActOrder;
        //                    NextActivity.ProcessRefId = StaffIssues.Id;
        //                    //NextActivity.Available = true;
        //                    NextActivity.Assigned = false;
        //                   // NextActivity.Completed = false;
        //                    NextActivity.BranchCode = StaffIssues.BranchCode;
        //                    NextActivity.DeptCode = StaffIssues.DeptCode;
        //                    if (NextActivity.ActivityName == "Complete")
        //                    {
        //                        StaffIssues.IsIssueCompleted = true;
        //                        PSF.Update<StaffIssues>(StaffIssues);
        //                        ProcessInstance pi = PSF.Get<ProcessInstance>(StaffIssues.InstanceId);
        //                        pi.Status = "Completed";
        //                        PSF.SaveOrUpdate<ProcessInstance>(pi);
        //                        StaffIssues.Status = "Completed";
        //                        StaffIssues.ActivityFullName = "Completed";
        //                        PSF.SaveOrUpdate<StaffIssues>(StaffIssues);
        //                    }
        //                    else
        //                    {
        //                        StaffIssues.Status = NextActivity.ActivityName;
        //                        StaffIssues.ActivityFullName = NextActivity.ActivityFullName;
        //                        PSF.SaveOrUpdate<StaffIssues>(StaffIssues);
        //                    }
        //                    PSF.SaveOrUpdate<StaffActivities>(NextActivity);
        //                }
        //            } retValue = true;
        //        }
        //        return retValue;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public bool CompleteActivityStaffIssueManagement(StaffIssues StaffIssues, string Template, string userId, string ActivityName, bool isRejection)
        {
            bool retValue = false;
            try
            {
                PSF.SaveOrUpdate<StaffIssues>(StaffIssues);
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                //close the current activity
                StaffActivities CurrentActivity = PSF.Get<StaffActivities>("InstanceId", StaffIssues.InstanceId, "ActivityName", ActivityName, "Completed", false);
                if (CurrentActivity != null)
                {
                    //if current activity doesnt need any rejection and there is no rejection then it will be completed
                    //otherwise once the rejection resolved then it will be completed
                    //this need to be obdated as waiting
                    if (CurrentActivity.IsRejApplicable == true && isRejection == true)
                    {
                        CurrentActivity.BranchCode = StaffIssues.BranchCode;
                        CurrentActivity.DeptCode = StaffIssues.DeptCode;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        CurrentActivity.ModifiedBy = userId;//Added By Prabakaran
                        CurrentActivity.ModifiedDate = DateTime.Now;//Added By Prabakaran
                        PSF.SaveOrUpdate(CurrentActivity);
                    }
                    else
                    {
                        StaffActivities WaitingAct = PSF.Get<StaffActivities>("WaitingFor", CurrentActivity.Id);
                        if (WaitingAct != null)
                        {
                            CurrentActivity.BranchCode = StaffIssues.BranchCode;
                            CurrentActivity.DeptCode = StaffIssues.DeptCode;
                            CurrentActivity.Waiting = false;
                            PSF.SaveOrUpdate(CurrentActivity);
                            WaitingAct.Completed = true;
                            WaitingAct.Assigned = false;
                            WaitingAct.Available = false;
                        }
                        CurrentActivity.BranchCode = StaffIssues.BranchCode;
                        CurrentActivity.DeptCode = StaffIssues.DeptCode;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        CurrentActivity.Performer = userId;
                        CurrentActivity.ModifiedBy = userId;//Added By Prabakaran
                        CurrentActivity.ModifiedDate = DateTime.Now;//Added By Prabakaran
                        if (CurrentActivity.ActivityName == "Complete")
                        {
                            StaffIssues.IsIssueCompleted = true;
                            PSF.Update<StaffIssues>(StaffIssues);
                            ProcessInstance pi = PSF.Get<ProcessInstance>(StaffIssues.InstanceId);
                            pi.Status = "Completed";
                            PSF.SaveOrUpdate<ProcessInstance>(pi);
                            StaffIssues.Status = "Completed";
                            StaffIssues.ActivityFullName = "Completed";
                            PSF.SaveOrUpdate<StaffIssues>(StaffIssues);
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
                Dictionary<long, IList<StaffActivities>> conditionList = PSF.GetListWithSearchCriteriaCount<StaffActivities>(0, 100, string.Empty, string.Empty, WaitCriteria);
                if (conditionList != null && conditionList.Count > 0)
                {
                    IList<StaffActivities> conditionWaitList = conditionList.First().Value;
                    bool? waiting = false;
                    foreach (StaffActivities a in conditionWaitList)
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
                                StaffActivities NextActivityRej = new StaffActivities();
                                NextActivityRej.CreatedDate = DateTime.Now;
                                NextActivityRej.ActivityName = wfsRej.WFStatus;
                                if (NextActivityRej.ActivityName == "Complete")
                                {
                                    NextActivityRej.Completed = true;
                                    //ProcessInstance pi = PSF.Get<ProcessInstance>(CallManagement.InstanceId);
                                    //pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                                    StaffIssues.Status = NextActivityRej.ActivityName;
                                    StaffIssues.ActivityFullName = NextActivityRej.ActivityFullName;
                                    PSF.SaveOrUpdate<StaffIssues>(StaffIssues);
                                }
                                else NextActivityRej.Completed = false;
                                NextActivityRej.ActivityFullName = wfsRej.Description;
                                NextActivityRej.AppRole = wfsRej.Performer;

                                NextActivityRej.Performer = userId;
                                NextActivityRej.TemplateId = WorkFlowTemplate.Id;
                                NextActivityRej.InstanceId = StaffIssues.InstanceId;
                                NextActivityRej.NextActOrder = wfsRej.NextActOrder;
                                NextActivityRej.ActivityOrder = wfsRej.ActivityOrder;
                                NextActivityRej.PreviousActOrder = wfsRej.PreviousActOrder;
                                NextActivityRej.ProcessRefId = StaffIssues.Id;
                                NextActivityRej.RejectionFor = CurrentActivity.Id;
                                NextActivityRej.Completed = false;
                                NextActivityRej.Available = true;
                                NextActivityRej.Assigned = false;
                                NextActivityRej.BranchCode = StaffIssues.BranchCode;
                                NextActivityRej.DeptCode = StaffIssues.DeptCode;
                                NextActivityRej.ModifiedDate = DateTime.Now;//Added By Prabakaran
                                NextActivityRej.ModifiedBy = userId;//Added By Prabakaran
                                PSF.SaveOrUpdate<StaffActivities>(NextActivityRej);
                                StaffIssues.Status = NextActivityRej.ActivityName;
                                StaffIssues.ActivityFullName = NextActivityRej.ActivityFullName;

                                PSF.SaveOrUpdate<StaffIssues>(StaffIssues);
                                //CurrentActivity.WaitingFor = NextActivityRej.Id;
                                //PSF.SaveOrUpdate(CurrentActivity);
                            }
                        }
                        else
                        {
                            StaffActivities NextActivity = new StaffActivities();
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
                            NextActivity.InstanceId = StaffIssues.InstanceId;
                            NextActivity.NextActOrder = wfs.NextActOrder;
                            NextActivity.ActivityOrder = wfs.ActivityOrder;
                            NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                            NextActivity.ProcessRefId = StaffIssues.Id;
                            NextActivity.Available = true;
                            NextActivity.Assigned = false;
                            NextActivity.Completed = false;
                            NextActivity.BranchCode = StaffIssues.BranchCode;
                            NextActivity.DeptCode = StaffIssues.DeptCode;
                            PSF.SaveOrUpdate<StaffActivities>(NextActivity);
                            StaffIssues.Status = NextActivity.ActivityName;
                            StaffIssues.ActivityFullName = NextActivity.ActivityFullName;
                            PSF.SaveOrUpdate<StaffIssues>(StaffIssues);
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
        public Dictionary<long, IList<StaffIssues>> GetStaffViewListWithsearchCriteria(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithEQSearchCriteriaCount<StaffIssues>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public Dictionary<long, IList<StaffMgmntActivity>> GetStaffIssueActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<StaffMgmntActivity>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public bool BulkIssueCompleteActivityStaffManagement(long[] ActivityId, string Template, string userId)
        {
            try
            {
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                Criteria.Add("Id", ActivityId);
                //add the criteria of activity name itself so that it will get only those avtivities
                Criteria.Add("ActivityName", "Complete");
                Dictionary<long, IList<StaffActivities>> ActivityList = PSF.GetListWithSearchCriteriaCount<StaffActivities>(0, 100, string.Empty, string.Empty, Criteria);
                if (ActivityList != null && ActivityList.First().Value != null && ActivityList.First().Value.Count > 0)
                {
                    foreach (StaffActivities a in ActivityList.First().Value)
                    {
                        //set the status of call management
                        StaffIssues sm = PSF.Get<StaffIssues>("InstanceId", a.InstanceId);
                        sm.IsIssueCompleted = true;
                        sm.Status = "Completed";
                        PSF.SaveOrUpdate<StaffIssues>(sm);
                        //set the status of process instance to complete
                        ProcessInstance pi = PSF.Get<ProcessInstance>(a.InstanceId);
                        pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                        //complete the activity
                        a.Completed = true;
                        a.Available = false;
                        a.Assigned = false;
                        a.Performer = userId;
                        PSF.SaveOrUpdate<StaffActivities>(a);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        //public bool AssignStaffActivity(long activityId, string userId)
        //{
        //    if (activityId > 0 && !string.IsNullOrEmpty(userId))
        //    {
        //        StaffActivities AssignActivity = PSF.Get<StaffActivities>(activityId);
        //        if (AssignActivity.Completed != true)
        //        {
        //            AssignActivity.Assigned = true;
        //        }
        //        AssignActivity.Available = false;
        //        AssignActivity.Performer = userId;
        //        PSF.SaveOrUpdate<StaffActivities>(AssignActivity);
        //        return true;
        //    }
        //    else return false;
        //}

        public Dictionary<long, IList<StaffMgmntActivity>> GetStaffIssueActivityListWithsearchCriteriaOnly(int? page, int? pageSize, string sortType, string sortBy, Dictionary<string, object> criteria)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCount<StaffMgmntActivity>(page, pageSize, sortType, sortBy, criteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public bool AssignStaffActivity(long activityId, string userId)
        {
            if (activityId > 0 && !string.IsNullOrEmpty(userId))
            {
                StaffActivities AssignActivity = PSF.Get<StaffActivities>(activityId);
                if (AssignActivity.Completed != true)
                {
                    if (AssignActivity.ActivityName == "ResolveIssue" && AssignActivity.Available == true && AssignActivity.Completed == false && AssignActivity.Assigned == false)
                    {
                        //string sqlquery = "";
                        //sqlquery = "update StaffIssues set AssignedDate=" + DateTime.Now + "where Id='" + AssignActivity.ProcessRefId + "'";
                        //PSF.ExecuteSqlUsingSQLCommand(sqlquery);
                        StaffIssuesBC sib = new StaffIssuesBC();
                        StaffIssues si = sib.GetStaffIssuesById(AssignActivity.ProcessRefId);
                        if (si != null)
                        {
                            si.AssignedDate = DateTime.Now;
                            sib.CreateOrUpdateStaffManagement(si);
                        }
                    }
                    AssignActivity.Assigned = true;
                }
                AssignActivity.Available = false;
                AssignActivity.Performer = userId;
                PSF.SaveOrUpdate<StaffActivities>(AssignActivity);
                return true;
            }
            else return false;
        }

        public long StartHRManagement(LeaveRequest lrequest, string Template, string userId)
        {
            try
            {
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
                lrequest.InstanceId = pid;
                PSF.SaveOrUpdate<LeaveRequest>(lrequest);
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
                    NextActivity.ProcessRefId = lrequest.Id;
                    NextActivity.CreatedDate = DateTime.Now;
                    NextActivity.BranchCode = lrequest.BranchCode;
                    //NextActivity.DeptCode = lrequest.DeptCode;
                    //NextActivity.AppName = "HRM";
                    PSF.SaveOrUpdate<Activity>(NextActivity);
                }

                return lrequest.Id;
            }
            catch (Exception) { throw; }


        }
        public Dictionary<long, IList<HRMgmntActivity>> GetHRActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<HRMgmntActivity>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public bool BulkCompleteActivityHRManagement(long[] ActivityId, string Template, string userId)
        {
            try
            {
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                Criteria.Add("Id", ActivityId);
                //add the criteria of activity name itself so that it will get only those avtivities
                Criteria.Add("ActivityName", "ApproveLeave");

                Dictionary<long, IList<Activity>> ActivityList = PSF.GetListWithSearchCriteriaCount<Activity>(0, 100, string.Empty, string.Empty, Criteria);
                if (ActivityList != null && ActivityList.First().Value != null && ActivityList.First().Value.Count > 0)
                {
                    foreach (Activity ac in ActivityList.First().Value)
                    {
                        LeaveRequest lr = PSF.Get<LeaveRequest>("InstanceId", ac.InstanceId);
                        Activity NextActivity = new Activity();
                        lr.ActivityFullName = "Complete";
                        lr.Status = "Complete";
                        lr.Performer = userId;
                        PSF.SaveOrUpdate<LeaveRequest>(lr);


                        ac.Completed = true;
                        ac.Available = false;
                        ac.Assigned = false;
                        ac.Performer = userId;
                        PSF.SaveOrUpdate<Activity>(ac);

                        IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", ac.TemplateId, "ActivityOrder", ac.NextActOrder);
                        foreach (WorkFlowStatus wfs in WorkFlowStatusList)
                        {
                            NextActivity.ActivityName = wfs.WFStatus;
                            NextActivity.ActivityFullName = wfs.WFStatus;
                            NextActivity.Available = true;
                            NextActivity.Assigned = false;
                            NextActivity.Completed = false;
                            NextActivity.TemplateId = wfs.TemplateId;
                            NextActivity.ProcessRefId = ac.ProcessRefId;
                            NextActivity.AppRole = wfs.Performer;
                            NextActivity.CreatedDate = ac.CreatedDate;
                            NextActivity.BranchCode = ac.BranchCode;
                            //NextActivity.AppName = ac.AppName;
                            NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                            NextActivity.NextActOrder = wfs.NextActOrder;
                            NextActivity.InstanceId = ac.InstanceId;
                            NextActivity.ActivityOrder = wfs.ActivityOrder;

                            PSF.SaveOrUpdate<Activity>(NextActivity);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public bool CompleteActivityHRManagement(LeaveRequest HRManagement, string Template, string userId, string ReportManger, string CheckAppRole, string UserIdDetails, string ActivityName, bool isRejection)
        {
            bool retValue = false;
            try
            {
                PSF.SaveOrUpdate<LeaveRequest>(HRManagement);
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                //close the current activity
                Activity CurrentActivity = PSF.Get<Activity>("InstanceId", HRManagement.InstanceId, "ActivityName", ActivityName, "Completed", false);
                if (CurrentActivity != null)
                {
                    //if current activity doesnt need any rejection and there is no rejection then it will be completed
                    //otherwise once the rejection resolved then it will be completed
                    //this need to be obdated as waiting
                    if (CurrentActivity.IsRejApplicable == true && isRejection == true)
                    {
                        CurrentActivity.BranchCode = HRManagement.BranchCode;
                        // CurrentActivity.DeptCode = HRManagement.DeptCode;
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
                            CurrentActivity.BranchCode = HRManagement.BranchCode;
                            // CurrentActivity.DeptCode = HRManagement.DeptCode;
                            CurrentActivity.Waiting = false;
                            PSF.SaveOrUpdate(CurrentActivity);
                            WaitingAct.Completed = true;
                            WaitingAct.Assigned = false;
                            WaitingAct.Available = false;
                        }
                        CurrentActivity.BranchCode = HRManagement.BranchCode;
                        // CurrentActivity.DeptCode = HRManagement.DeptCode;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        CurrentActivity.Performer = userId;
                        if (CurrentActivity.ActivityName == "Complete")
                        {
                            HRManagement.IsIssueCompleted = true;
                            PSF.Update<LeaveRequest>(HRManagement);
                            ProcessInstance pi = PSF.Get<ProcessInstance>(HRManagement.InstanceId);
                            pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                            HRManagement.Status = "Completed";
                            HRManagement.ActivityFullName = "Completed";
                            PSF.SaveOrUpdate<LeaveRequest>(HRManagement);
                            PSF.SaveOrUpdate(CurrentActivity);
                            return true;
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
                                    //ProcessInstance pi = PSF.Get<ProcessInstance>(CallManagement.InstanceId);
                                    //pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                                    HRManagement.Status = NextActivityRej.ActivityName;
                                    PSF.SaveOrUpdate<LeaveRequest>(HRManagement);
                                }
                                else NextActivityRej.Completed = false;
                                NextActivityRej.ActivityFullName = wfsRej.Description;
                                NextActivityRej.AppRole = wfsRej.Performer;
                                NextActivityRej.Performer = HRManagement.CreatedBy;
                                NextActivityRej.TemplateId = WorkFlowTemplate.Id;
                                NextActivityRej.InstanceId = HRManagement.InstanceId;
                                NextActivityRej.NextActOrder = wfsRej.NextActOrder;
                                NextActivityRej.ActivityOrder = wfsRej.ActivityOrder;
                                NextActivityRej.PreviousActOrder = wfsRej.PreviousActOrder;
                                NextActivityRej.ProcessRefId = HRManagement.Id;
                                NextActivityRej.RejectionFor = CurrentActivity.Id;
                                NextActivityRej.Completed = false;
                                NextActivityRej.Available = false;
                                NextActivityRej.Assigned = true;
                                NextActivityRej.BranchCode = HRManagement.BranchCode;

                                //NextActivityRej.DeptCode = HRManagement.DeptCode;
                                PSF.SaveOrUpdate<Activity>(NextActivityRej);
                                // HRManagement.Status = NextActivityRej.ActivityName;

                                if ((CheckAppRole == "HRMHEAD") && (UserIdDetails == "HRMSTAFF"))
                                {
                                    NextActivityRej.AppRole = HRManagement.CreatedBy;
                                }

                                HRManagement.Status = NextActivityRej.ActivityName;
                                HRManagement.ActivityFullName = NextActivityRej.ActivityFullName;


                                PSF.SaveOrUpdate<LeaveRequest>(HRManagement);
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

                            if ((CheckAppRole == "HRMRM") && (UserIdDetails == "HRMSTAFF"))
                            {

                                NextActivity.ActivityName = wfs.WFStatus;
                                NextActivity.ActivityFullName = wfs.Description;
                                NextActivity.AppRole = wfs.Performer;
                                NextActivity.Completed = wfs.WFStatus == "Complete" ? true : false;
                                NextActivity.Available = wfs.WFStatus != "Complete" ? true : false;
                                NextActivity.Performer = ReportManger;
                                NextActivity.TemplateId = WorkFlowTemplate.Id;
                                NextActivity.InstanceId = HRManagement.InstanceId;
                                NextActivity.NextActOrder = wfs.NextActOrder;
                                NextActivity.ActivityOrder = wfs.ActivityOrder;
                                NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                                NextActivity.ProcessRefId = HRManagement.Id;
                                NextActivity.Available = false;
                                NextActivity.Assigned = true;
                                NextActivity.Completed = false;
                                NextActivity.BranchCode = HRManagement.BranchCode;
                                PSF.SaveOrUpdate<Activity>(NextActivity);
                            }
                            else if ((CheckAppRole == "HRMHEAD") && (UserIdDetails == "HRMRM"))
                            {
                                NextActivity.ActivityName = wfs.WFStatus;
                                NextActivity.ActivityFullName = wfs.Description;
                                NextActivity.AppRole = wfs.Performer;
                                NextActivity.Completed = wfs.WFStatus == "Complete" ? true : false;
                                NextActivity.Available = wfs.WFStatus != "Complete" ? true : false;
                                NextActivity.Performer = ReportManger;
                                NextActivity.TemplateId = WorkFlowTemplate.Id;
                                NextActivity.InstanceId = HRManagement.InstanceId;
                                NextActivity.NextActOrder = wfs.NextActOrder;
                                NextActivity.ActivityOrder = wfs.ActivityOrder;
                                NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                                NextActivity.ProcessRefId = HRManagement.Id;
                                NextActivity.Available = true;
                                NextActivity.Assigned = false;
                                NextActivity.Completed = false;
                                NextActivity.BranchCode = HRManagement.BranchCode;
                                PSF.SaveOrUpdate<Activity>(NextActivity);
                            }
                            else if ((CheckAppRole == "HRMHEAD") && (UserIdDetails == "HRMSTAFF"))
                            {
                                NextActivity.ActivityName = "ApproveLeaveRequest";
                                NextActivity.ActivityFullName = "Approve Leave Request";
                                NextActivity.AppRole = "HRMHEAD";
                                NextActivity.Performer = ReportManger;
                                NextActivity.TemplateId = WorkFlowTemplate.Id;
                                NextActivity.InstanceId = HRManagement.InstanceId;
                                NextActivity.NextActOrder = wfs.NextActOrder;
                                NextActivity.ActivityOrder = wfs.ActivityOrder;
                                NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                                NextActivity.ProcessRefId = HRManagement.Id;
                                NextActivity.Available = true;
                                NextActivity.Assigned = false;
                                NextActivity.Completed = false;
                                NextActivity.BranchCode = HRManagement.BranchCode;
                                PSF.SaveOrUpdate<Activity>(NextActivity);
                            }
                            else if ((CheckAppRole == "HRMSTAFF") && (UserIdDetails == "HRMHEAD") && (ReportManger == null))
                            {
                                NextActivity.ActivityName = "Complete";
                                NextActivity.ActivityFullName = "Complete";
                                NextActivity.AppRole = "HRMSTAFF";
                                NextActivity.Performer = HRManagement.CreatedBy;
                                NextActivity.TemplateId = WorkFlowTemplate.Id;
                                NextActivity.InstanceId = HRManagement.InstanceId;
                                NextActivity.NextActOrder = wfs.NextActOrder;
                                NextActivity.ActivityOrder = wfs.ActivityOrder;
                                NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                                NextActivity.ProcessRefId = HRManagement.Id;
                                NextActivity.Available = false;
                                NextActivity.Assigned = true;
                                NextActivity.Completed = false;
                                NextActivity.BranchCode = HRManagement.BranchCode;
                                PSF.SaveOrUpdate<Activity>(NextActivity);


                            }
                            else
                            {

                                NextActivity.ActivityName = wfs.WFStatus;
                                NextActivity.ActivityFullName = wfs.Description;
                                NextActivity.AppRole = wfs.Performer;
                                NextActivity.Completed = wfs.WFStatus == "Complete" ? true : false;
                                NextActivity.Available = wfs.WFStatus != "Complete" ? true : false;
                                NextActivity.Performer = HRManagement.CreatedBy;
                                NextActivity.TemplateId = WorkFlowTemplate.Id;
                                NextActivity.InstanceId = HRManagement.InstanceId;
                                NextActivity.NextActOrder = wfs.NextActOrder;
                                NextActivity.ActivityOrder = wfs.ActivityOrder;
                                NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                                NextActivity.ProcessRefId = HRManagement.Id;
                                NextActivity.Available = false;
                                NextActivity.Assigned = true;
                                NextActivity.Completed = false;
                                NextActivity.BranchCode = HRManagement.BranchCode;
                                PSF.SaveOrUpdate<Activity>(NextActivity);

                            }
                            HRManagement.Status = NextActivity.ActivityName;
                            HRManagement.ActivityFullName = NextActivity.ActivityFullName;
                            PSF.SaveOrUpdate<LeaveRequest>(HRManagement);
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
        // Bank Account Details...

        public long StartBankHRManagement(BankAccount account, string Template, string userId)
        {
            try
            {
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
                account.InstanceId = pid;
                PSF.SaveOrUpdate<BankAccount>(account);
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
                    NextActivity.ProcessRefId = account.Id;
                    NextActivity.CreatedDate = DateTime.Now;
                    NextActivity.BranchCode = account.BranchCode;
                    //NextActivity.DeptCode = lrequest.DeptCode;
                    //NextActivity.AppName = "HRM";
                    PSF.SaveOrUpdate<Activity>(NextActivity);
                }

                return account.Id;
            }
            catch (Exception) { throw; }
        }
        public bool CompleteBankActivityHRManagement(BankAccount account, string Template, string userId, string ActivityName, bool isRejection)
        {
            bool retValue = false;
            try
            {
                PSF.SaveOrUpdate<BankAccount>(account);
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                //close the current activity
                Activity CurrentActivity = PSF.Get<Activity>("InstanceId", account.InstanceId, "ActivityName", ActivityName, "Completed", false);
                if (CurrentActivity != null)
                {
                    //if current activity doesnt need any rejection and there is no rejection then it will be completed
                    //otherwise once the rejection resolved then it will be completed
                    //this need to be obdated as waiting
                    if (CurrentActivity.IsRejApplicable == true && isRejection == true)
                    {
                        CurrentActivity.BranchCode = account.BranchCode;
                        //CurrentActivity.DeptCode = HRManagement.DeptCode;
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
                            CurrentActivity.BranchCode = account.BranchCode;
                            //CurrentActivity.DeptCode = CallManagement.DeptCode;
                            CurrentActivity.Waiting = false;
                            PSF.SaveOrUpdate(CurrentActivity);
                            WaitingAct.Completed = true;
                            WaitingAct.Assigned = false;
                            WaitingAct.Available = false;
                        }
                        CurrentActivity.BranchCode = account.BranchCode;
                        //CurrentActivity.DeptCode = CallManagement.DeptCode;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        CurrentActivity.Performer = userId;
                        if (CurrentActivity.ActivityName == "Complete")
                        {
                            account.IsIssueCompleted = true;
                            PSF.Update<BankAccount>(account);
                            ProcessInstance pi = PSF.Get<ProcessInstance>(account.InstanceId);
                            pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                            account.Status = "Completed";
                            account.ActivityFullName = "Completed";
                            PSF.SaveOrUpdate<BankAccount>(account);
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
                                    //ProcessInstance pi = PSF.Get<ProcessInstance>(CallManagement.InstanceId);
                                    //pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                                    account.Status = NextActivityRej.ActivityName;
                                    PSF.SaveOrUpdate<BankAccount>(account);
                                }
                                else NextActivityRej.Completed = false;
                                NextActivityRej.ActivityFullName = wfsRej.Description;
                                NextActivityRej.AppRole = wfsRej.Performer;

                                NextActivityRej.Performer = userId;
                                NextActivityRej.TemplateId = WorkFlowTemplate.Id;
                                NextActivityRej.InstanceId = account.InstanceId;
                                NextActivityRej.NextActOrder = wfsRej.NextActOrder;
                                NextActivityRej.ActivityOrder = wfsRej.ActivityOrder;
                                NextActivityRej.PreviousActOrder = wfsRej.PreviousActOrder;
                                NextActivityRej.ProcessRefId = account.Id;
                                NextActivityRej.RejectionFor = CurrentActivity.Id;
                                NextActivityRej.Completed = false;
                                NextActivityRej.Available = true;
                                NextActivityRej.Assigned = false;
                                NextActivityRej.BranchCode = account.BranchCode;
                                //NextActivityRej.DeptCode = CallManagement.DeptCode;
                                PSF.SaveOrUpdate<Activity>(NextActivityRej);
                                account.Status = NextActivityRej.ActivityName;
                                account.ActivityFullName = NextActivityRej.ActivityFullName;
                                PSF.SaveOrUpdate<BankAccount>(account);
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
                            NextActivity.InstanceId = account.InstanceId;
                            NextActivity.NextActOrder = wfs.NextActOrder;
                            NextActivity.ActivityOrder = wfs.ActivityOrder;
                            NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                            NextActivity.ProcessRefId = account.Id;
                            NextActivity.Available = true;
                            NextActivity.Assigned = false;
                            NextActivity.Completed = false;
                            NextActivity.BranchCode = account.BranchCode;
                            //NextActivity.DeptCode = CallManagement.DeptCode;
                            PSF.SaveOrUpdate<Activity>(NextActivity);
                            account.Status = NextActivity.ActivityName;
                            account.ActivityFullName = NextActivity.ActivityFullName;
                            PSF.SaveOrUpdate<BankAccount>(account);
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
        public Dictionary<long, IList<BankMgmntActivity>> GetBankHRActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<BankMgmntActivity>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public bool BankAccountBulkCompleteActivity(long[] ActivityId, string Template, string userId)
        {
            try
            {
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                Criteria.Add("Id", ActivityId);
                //add the criteria of activity name itself so that it will get only those avtivities
                Criteria.Add("ActivityName", "ApproveAccount");

                Dictionary<long, IList<Activity>> ActivityList = PSF.GetListWithSearchCriteriaCount<Activity>(0, 100, string.Empty, string.Empty, Criteria);
                if (ActivityList != null && ActivityList.First().Value != null && ActivityList.First().Value.Count > 0)
                {
                    foreach (Activity activity in ActivityList.First().Value)
                    {
                        BankAccount ba = PSF.Get<BankAccount>("InstanceId", activity.InstanceId);
                        Activity NextActivity = new Activity();
                        ba.ActivityFullName = "Complete";
                        ba.Status = "Complete";
                        ba.Performer = userId;
                        PSF.SaveOrUpdate<BankAccount>(ba);


                        activity.Completed = true;
                        activity.Available = false;
                        activity.Assigned = false;
                        activity.Performer = userId;
                        PSF.SaveOrUpdate<Activity>(activity);

                        IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", activity.TemplateId, "ActivityOrder", activity.NextActOrder);
                        foreach (WorkFlowStatus wfs in WorkFlowStatusList)
                        {
                            NextActivity.ActivityName = wfs.WFStatus;
                            NextActivity.ActivityFullName = wfs.WFStatus;
                            NextActivity.Available = true;
                            NextActivity.Assigned = false;
                            NextActivity.Completed = false;
                            NextActivity.TemplateId = wfs.TemplateId;
                            NextActivity.ProcessRefId = activity.ProcessRefId;
                            NextActivity.AppRole = wfs.Performer;
                            NextActivity.CreatedDate = activity.CreatedDate;
                            NextActivity.BranchCode = activity.BranchCode;
                            //  NextActivity.AppName = activity.AppName;
                            NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                            NextActivity.NextActOrder = wfs.NextActOrder;
                            NextActivity.InstanceId = activity.InstanceId;
                            NextActivity.ActivityOrder = wfs.ActivityOrder;

                            PSF.SaveOrUpdate<Activity>(NextActivity);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }


        // Certificate Request Details...

        public long StartCertificateHRManagement(CertificateRequest Crequest, string Template, string userId)
        {
            try
            {
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
                Crequest.InstanceId = pid;
                PSF.SaveOrUpdate<CertificateRequest>(Crequest);
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
                    NextActivity.ProcessRefId = Crequest.Id;
                    NextActivity.CreatedDate = DateTime.Now;
                    NextActivity.BranchCode = Crequest.BranchCode;
                    //NextActivity.DeptCode = lrequest.DeptCode;
                    //NextActivity.AppName = "HRM";
                    PSF.SaveOrUpdate<Activity>(NextActivity);
                }

                return Crequest.Id;
            }
            catch (Exception) { throw; }
        }
        public bool CompleteCertificateActivityHRManagement(CertificateRequest Crequest, string Template, string userId, string ActivityName, bool isRejection)
        {
            bool retValue = false;
            try
            {
                PSF.SaveOrUpdate<CertificateRequest>(Crequest);
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                //close the current activity
                Activity CurrentActivity = PSF.Get<Activity>("InstanceId", Crequest.InstanceId, "ActivityName", ActivityName, "Completed", false);
                if (CurrentActivity != null)
                {
                    //if current activity doesnt need any rejection and there is no rejection then it will be completed
                    //otherwise once the rejection resolved then it will be completed
                    //this need to be obdated as waiting
                    if (CurrentActivity.IsRejApplicable == true && isRejection == true)
                    {
                        CurrentActivity.BranchCode = Crequest.BranchCode;
                        //CurrentActivity.DeptCode = HRManagement.DeptCode;
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
                            CurrentActivity.BranchCode = Crequest.BranchCode;
                            //CurrentActivity.DeptCode = CallManagement.DeptCode;
                            CurrentActivity.Waiting = false;
                            PSF.SaveOrUpdate(CurrentActivity);
                            WaitingAct.Completed = true;
                            WaitingAct.Assigned = false;
                            WaitingAct.Available = false;
                        }
                        CurrentActivity.BranchCode = Crequest.BranchCode;
                        //CurrentActivity.DeptCode = CallManagement.DeptCode;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        CurrentActivity.Performer = userId;
                        if (CurrentActivity.ActivityName == "Complete")
                        {
                            Crequest.IsIssueCompleted = true;
                            PSF.Update<CertificateRequest>(Crequest);
                            ProcessInstance pi = PSF.Get<ProcessInstance>(Crequest.InstanceId);
                            pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                            Crequest.Status = "Completed";
                            Crequest.ActivityFullName = "Completed";
                            PSF.SaveOrUpdate<CertificateRequest>(Crequest);
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
                                    //ProcessInstance pi = PSF.Get<ProcessInstance>(CallManagement.InstanceId);
                                    //pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                                    Crequest.Status = NextActivityRej.ActivityName;
                                    PSF.SaveOrUpdate<CertificateRequest>(Crequest);
                                }
                                else NextActivityRej.Completed = false;
                                NextActivityRej.ActivityFullName = wfsRej.Description;
                                NextActivityRej.AppRole = wfsRej.Performer;

                                NextActivityRej.Performer = userId;
                                NextActivityRej.TemplateId = WorkFlowTemplate.Id;
                                NextActivityRej.InstanceId = Crequest.InstanceId;
                                NextActivityRej.NextActOrder = wfsRej.NextActOrder;
                                NextActivityRej.ActivityOrder = wfsRej.ActivityOrder;
                                NextActivityRej.PreviousActOrder = wfsRej.PreviousActOrder;
                                NextActivityRej.ProcessRefId = Crequest.Id;
                                NextActivityRej.RejectionFor = CurrentActivity.Id;
                                NextActivityRej.Completed = false;
                                NextActivityRej.Available = true;
                                NextActivityRej.Assigned = false;
                                NextActivityRej.BranchCode = Crequest.BranchCode;
                                //NextActivityRej.DeptCode = CallManagement.DeptCode;
                                PSF.SaveOrUpdate<Activity>(NextActivityRej);
                                Crequest.Status = NextActivityRej.ActivityName;
                                Crequest.ActivityFullName = NextActivityRej.ActivityFullName;
                                PSF.SaveOrUpdate<CertificateRequest>(Crequest);
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
                            NextActivity.InstanceId = Crequest.InstanceId;
                            NextActivity.NextActOrder = wfs.NextActOrder;
                            NextActivity.ActivityOrder = wfs.ActivityOrder;
                            NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                            NextActivity.ProcessRefId = Crequest.Id;
                            NextActivity.Available = true;
                            NextActivity.Assigned = false;
                            NextActivity.Completed = false;
                            NextActivity.BranchCode = Crequest.BranchCode;
                            //NextActivity.DeptCode = CallManagement.DeptCode;
                            PSF.SaveOrUpdate<Activity>(NextActivity);
                            Crequest.Status = NextActivity.ActivityName;
                            Crequest.ActivityFullName = NextActivity.ActivityFullName;
                            PSF.SaveOrUpdate<CertificateRequest>(Crequest);
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
        public Dictionary<long, IList<CertificateMgmntActivity>> GetCertificateHRActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<CertificateMgmntActivity>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public bool CertificateBulkCompleteActivityHRManagement(long[] ActivityId, string Template, string userId)
        {
            try
            {
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                Criteria.Add("Id", ActivityId);
                //add the criteria of activity name itself so that it will get only those avtivities
                Criteria.Add("ActivityName", "ApproveRequest");

                Dictionary<long, IList<Activity>> ActivityList = PSF.GetListWithSearchCriteriaCount<Activity>(0, 100, string.Empty, string.Empty, Criteria);
                if (ActivityList != null && ActivityList.First().Value != null && ActivityList.First().Value.Count > 0)
                {
                    foreach (Activity ac in ActivityList.First().Value)
                    {
                        CertificateRequest Cr = PSF.Get<CertificateRequest>("InstanceId", ac.InstanceId);
                        Activity NextActivity = new Activity();
                        Cr.ActivityFullName = "Complete";
                        Cr.Status = "Complete";
                        Cr.Performer = userId;
                        PSF.SaveOrUpdate<CertificateRequest>(Cr);


                        ac.Completed = true;
                        ac.Available = false;
                        ac.Assigned = false;
                        ac.Performer = userId;
                        PSF.SaveOrUpdate<Activity>(ac);

                        IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", ac.TemplateId, "ActivityOrder", ac.NextActOrder);
                        foreach (WorkFlowStatus wfs in WorkFlowStatusList)
                        {
                            NextActivity.ActivityName = wfs.WFStatus;
                            NextActivity.ActivityFullName = wfs.WFStatus;
                            NextActivity.Available = true;
                            NextActivity.Assigned = false;
                            NextActivity.Completed = false;
                            NextActivity.TemplateId = wfs.TemplateId;
                            NextActivity.ProcessRefId = ac.ProcessRefId;
                            NextActivity.AppRole = wfs.Performer;
                            NextActivity.CreatedDate = ac.CreatedDate;
                            NextActivity.BranchCode = ac.BranchCode;
                            //  NextActivity.AppName = ac.AppName;
                            NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                            NextActivity.NextActOrder = wfs.NextActOrder;
                            NextActivity.InstanceId = ac.InstanceId;
                            NextActivity.ActivityOrder = wfs.ActivityOrder;

                            PSF.SaveOrUpdate<Activity>(NextActivity);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        // Salary Advance Request Details...
        public long StartSalaryAdvanceHRManagement(SalaryAdvance Sadvance, string Template, string userId)
        {
            try
            {
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
                Sadvance.InstanceId = pid;
                PSF.SaveOrUpdate<SalaryAdvance>(Sadvance);
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
                    NextActivity.ProcessRefId = Sadvance.Id;
                    NextActivity.CreatedDate = DateTime.Now;
                    NextActivity.BranchCode = Sadvance.BranchCode;
                    //NextActivity.DeptCode = lrequest.DeptCode;
                    //NextActivity.AppName = "HRM";
                    PSF.SaveOrUpdate<Activity>(NextActivity);
                }

                return Sadvance.Id;
            }
            catch (Exception) { throw; }
        }
        public bool CompleteSalaryAdvanceActivityHRManagement(SalaryAdvance Sadvance, string Template, string userId, string ActivityName, bool isRejection)
        {
            bool retValue = false;
            try
            {
                PSF.SaveOrUpdate<SalaryAdvance>(Sadvance);
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                //close the current activity
                Activity CurrentActivity = PSF.Get<Activity>("InstanceId", Sadvance.InstanceId, "ActivityName", ActivityName, "Completed", false);
                if (CurrentActivity != null)
                {
                    //if current activity doesnt need any rejection and there is no rejection then it will be completed
                    //otherwise once the rejection resolved then it will be completed
                    //this need to be obdated as waiting
                    if (CurrentActivity.IsRejApplicable == true && isRejection == true)
                    {
                        CurrentActivity.BranchCode = Sadvance.BranchCode;
                        //CurrentActivity.DeptCode = HRManagement.DeptCode;
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
                            CurrentActivity.BranchCode = Sadvance.BranchCode;
                            //CurrentActivity.DeptCode = CallManagement.DeptCode;
                            CurrentActivity.Waiting = false;
                            PSF.SaveOrUpdate(CurrentActivity);
                            WaitingAct.Completed = true;
                            WaitingAct.Assigned = false;
                            WaitingAct.Available = false;
                        }
                        CurrentActivity.BranchCode = Sadvance.BranchCode;
                        //CurrentActivity.DeptCode = CallManagement.DeptCode;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        CurrentActivity.Performer = userId;
                        if (CurrentActivity.ActivityName == "Complete")
                        {
                            Sadvance.IsIssueCompleted = true;
                            PSF.Update<SalaryAdvance>(Sadvance);
                            ProcessInstance pi = PSF.Get<ProcessInstance>(Sadvance.InstanceId);
                            pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                            Sadvance.Status = "Completed";
                            Sadvance.ActivityFullName = "Completed";
                            PSF.SaveOrUpdate<SalaryAdvance>(Sadvance);
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
                                    //ProcessInstance pi = PSF.Get<ProcessInstance>(CallManagement.InstanceId);
                                    //pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                                    Sadvance.Status = NextActivityRej.ActivityName;
                                    PSF.SaveOrUpdate<SalaryAdvance>(Sadvance);
                                }
                                else NextActivityRej.Completed = false;
                                NextActivityRej.ActivityFullName = wfsRej.Description;
                                NextActivityRej.AppRole = wfsRej.Performer;

                                NextActivityRej.Performer = userId;
                                NextActivityRej.TemplateId = WorkFlowTemplate.Id;
                                NextActivityRej.InstanceId = Sadvance.InstanceId;
                                NextActivityRej.NextActOrder = wfsRej.NextActOrder;
                                NextActivityRej.ActivityOrder = wfsRej.ActivityOrder;
                                NextActivityRej.PreviousActOrder = wfsRej.PreviousActOrder;
                                NextActivityRej.ProcessRefId = Sadvance.Id;
                                NextActivityRej.RejectionFor = CurrentActivity.Id;
                                NextActivityRej.Completed = false;
                                NextActivityRej.Available = true;
                                NextActivityRej.Assigned = false;
                                NextActivityRej.BranchCode = Sadvance.BranchCode;
                                //NextActivityRej.DeptCode = CallManagement.DeptCode;
                                PSF.SaveOrUpdate<Activity>(NextActivityRej);
                                Sadvance.Status = NextActivityRej.ActivityName;
                                Sadvance.ActivityFullName = NextActivityRej.ActivityFullName;
                                PSF.SaveOrUpdate<SalaryAdvance>(Sadvance);
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
                            NextActivity.InstanceId = Sadvance.InstanceId;
                            NextActivity.NextActOrder = wfs.NextActOrder;
                            NextActivity.ActivityOrder = wfs.ActivityOrder;
                            NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                            NextActivity.ProcessRefId = Sadvance.Id;
                            NextActivity.Available = true;
                            NextActivity.Assigned = false;
                            NextActivity.Completed = false;
                            NextActivity.BranchCode = Sadvance.BranchCode;
                            //NextActivity.DeptCode = CallManagement.DeptCode;
                            PSF.SaveOrUpdate<Activity>(NextActivity);
                            Sadvance.Status = NextActivity.ActivityName;
                            Sadvance.ActivityFullName = NextActivity.ActivityFullName;
                            PSF.SaveOrUpdate<SalaryAdvance>(Sadvance);
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
        public Dictionary<long, IList<SalaryAdvanceMgmntActivity>> GetSalaryAdvanceHRActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<SalaryAdvanceMgmntActivity>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public bool SalaryAdvanceBulkCompleteActivityHRManagement(long[] ActivityId, string Template, string userId)
        {
            try
            {
                Dictionary<string, object> Criteria = new Dictionary<string, object>();
                Criteria.Add("Id", ActivityId);
                //add the criteria of activity name itself so that it will get only those avtivities
                Criteria.Add("ActivityName", "ApproveRequest");

                Dictionary<long, IList<Activity>> ActivityList = PSF.GetListWithSearchCriteriaCount<Activity>(0, 100, string.Empty, string.Empty, Criteria);
                if (ActivityList != null && ActivityList.First().Value != null && ActivityList.First().Value.Count > 0)
                {
                    foreach (Activity ac in ActivityList.First().Value)
                    {
                        SalaryAdvance Sa = PSF.Get<SalaryAdvance>("InstanceId", ac.InstanceId);
                        Activity NextActivity = new Activity();
                        Sa.ActivityFullName = "Complete";
                        Sa.Status = "Complete";
                        Sa.Performer = userId;
                        PSF.SaveOrUpdate<SalaryAdvance>(Sa);


                        ac.Completed = true;
                        ac.Available = false;
                        ac.Assigned = false;
                        ac.Performer = userId;
                        PSF.SaveOrUpdate<Activity>(ac);

                        IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", ac.TemplateId, "ActivityOrder", ac.NextActOrder);
                        foreach (WorkFlowStatus wfs in WorkFlowStatusList)
                        {
                            NextActivity.ActivityName = wfs.WFStatus;
                            NextActivity.ActivityFullName = wfs.WFStatus;
                            NextActivity.Available = true;
                            NextActivity.Assigned = false;
                            NextActivity.Completed = false;
                            NextActivity.TemplateId = wfs.TemplateId;
                            NextActivity.ProcessRefId = ac.ProcessRefId;
                            NextActivity.AppRole = wfs.Performer;
                            NextActivity.CreatedDate = ac.CreatedDate;
                            NextActivity.BranchCode = ac.BranchCode;
                            // NextActivity.AppName = ac.AppName;
                            NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                            NextActivity.NextActOrder = wfs.NextActOrder;
                            NextActivity.InstanceId = ac.InstanceId;
                            NextActivity.ActivityOrder = wfs.ActivityOrder;

                            PSF.SaveOrUpdate<Activity>(NextActivity);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public bool MoveBackToAvailable(long activityId)
        {
            if (activityId > 0)
            {
                Activity AssignActivity = PSF.Get<Activity>(activityId);
                AssignActivity.Available = true;
                AssignActivity.Assigned = false;
                AssignActivity.Performer = "";
                PSF.SaveOrUpdate<Activity>(AssignActivity);
                return true;
            }
            else return false;
        }
        public Dictionary<long, IList<CallMgmntActivity>> GetActivityListWithsearchCriteriaLikeSearch(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<CallMgmntActivity>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }

        public string StartETaskingSystem(TaskSystem TaskSystem, string Template, string userId)
        {
            try
            {
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
                TaskSystem.InstanceId = pid;
                PSF.SaveOrUpdate<TaskSystem>(TaskSystem);
                string TaskNo = "ETask-" + TaskSystem.Id;
                TaskSystem.TaskNo = TaskNo;
                PSF.SaveOrUpdate<TaskSystem>(TaskSystem);
                IList<WorkFlowStatus> WorkFlowStatusList = PSF.GetList<WorkFlowStatus>("TemplateId", WorkFlowTemplate.Id, "ActivityOrder", 1);
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
                    NextActivity.ProcessRefId = TaskSystem.Id;
                    NextActivity.CreatedDate = DateTime.Now;
                    NextActivity.BranchCode = TaskSystem.BranchCode;
                    NextActivity.DeptCode = TaskSystem.DeptCode;
                    if (wfs.IsRejectionRequired == true)
                    { NextActivity.IsRejApplicable = true; }
                    PSF.SaveOrUpdate<Activity>(NextActivity);
                }
                return TaskSystem.TaskNo;
            }
            catch (Exception) { throw; }
        }

        public bool CompleteActivityTaskSystem(TaskSystem TaskSystem, string Template, string userId, string ActivityName, bool isRejection)
        {
            bool retValue = false;
            try
            {
                PSF.SaveOrUpdate<TaskSystem>(TaskSystem);
                WorkFlowTemplate WorkFlowTemplate = PSF.Get<WorkFlowTemplate>("TemplateName", Template);
                //close the current activity
                Activity CurrentActivity = PSF.Get<Activity>("InstanceId", TaskSystem.InstanceId, "ActivityName", ActivityName, "Completed", false);
                if (CurrentActivity != null)
                {
                    //if current activity doesnt need any rejection and there is no rejection then it will be completed
                    //otherwise once the rejection resolved then it will be completed
                    //this need to be obdated as waiting
                    if (CurrentActivity.IsRejApplicable == true && isRejection == true)
                    {
                        CurrentActivity.BranchCode = TaskSystem.BranchCode;
                        CurrentActivity.DeptCode = TaskSystem.DeptCode;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        CurrentActivity.Performer = userId;
                        PSF.SaveOrUpdate(CurrentActivity);
                    }
                    else
                    {
                        Activity WaitingAct = PSF.Get<Activity>("WaitingFor", CurrentActivity.Id);
                        if (WaitingAct != null)
                        {
                            CurrentActivity.BranchCode = TaskSystem.BranchCode;
                            CurrentActivity.DeptCode = TaskSystem.DeptCode;
                            CurrentActivity.Waiting = false;
                            PSF.SaveOrUpdate(CurrentActivity);
                            WaitingAct.Completed = true;
                            WaitingAct.Assigned = false;
                            WaitingAct.Available = false;
                        }
                        CurrentActivity.BranchCode = TaskSystem.BranchCode;
                        CurrentActivity.DeptCode = TaskSystem.DeptCode;
                        CurrentActivity.Completed = true;
                        CurrentActivity.Available = false;
                        CurrentActivity.Assigned = false;
                        CurrentActivity.Performer = userId;
                        if (CurrentActivity.ActivityName == "CompleteETask")
                        {
                            TaskSystem.IsTaskCompleted = true;
                            PSF.Update<TaskSystem>(TaskSystem);
                            ProcessInstance pi = PSF.Get<ProcessInstance>(TaskSystem.InstanceId);
                            pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                            TaskSystem.Status = "Completed";
                            PSF.SaveOrUpdate<TaskSystem>(TaskSystem);
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
                                if (NextActivityRej.ActivityName == "CompleteETask")
                                {
                                    NextActivityRej.Completed = true;
                                    //ProcessInstance pi = PSF.Get<ProcessInstance>(CallManagement.InstanceId);
                                    //pi.Status = "Completed"; PSF.SaveOrUpdate<ProcessInstance>(pi);
                                    TaskSystem.Status = NextActivityRej.ActivityName;
                                    PSF.SaveOrUpdate<TaskSystem>(TaskSystem);
                                }
                                else NextActivityRej.Completed = false;
                                NextActivityRej.ActivityFullName = wfsRej.Description;
                                NextActivityRej.AppRole = wfsRej.Performer;
                                if (NextActivityRej.ActivityName == "CloseETaskRejection")
                                {
                                    NextActivityRej.Performer = TaskSystem.AssignedTo;
                                }
                                else
                                {
                                    NextActivityRej.Performer = TaskSystem.Reporter;
                                }
                                NextActivityRej.TemplateId = WorkFlowTemplate.Id;
                                NextActivityRej.InstanceId = TaskSystem.InstanceId;
                                NextActivityRej.NextActOrder = wfsRej.NextActOrder;
                                NextActivityRej.ActivityOrder = wfsRej.ActivityOrder;
                                NextActivityRej.PreviousActOrder = wfsRej.PreviousActOrder;
                                NextActivityRej.ProcessRefId = TaskSystem.Id;
                                NextActivityRej.RejectionFor = CurrentActivity.Id;
                                NextActivityRej.Completed = false;
                                NextActivityRej.Available = true;
                                NextActivityRej.Assigned = false;
                                NextActivityRej.BranchCode = TaskSystem.BranchCode;
                                NextActivityRej.DeptCode = TaskSystem.DeptCode;
                                PSF.SaveOrUpdate<Activity>(NextActivityRej);
                                TaskSystem.Status = NextActivityRej.ActivityName;
                                PSF.SaveOrUpdate<TaskSystem>(TaskSystem);
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
                            if (NextActivity.ActivityName == "ResolveETask")
                            {
                                NextActivity.Performer = TaskSystem.AssignedTo;
                            }
                            else
                            {
                                NextActivity.Performer = TaskSystem.Reporter;
                            }
                            NextActivity.TemplateId = WorkFlowTemplate.Id;
                            NextActivity.InstanceId = TaskSystem.InstanceId;
                            NextActivity.NextActOrder = wfs.NextActOrder;
                            NextActivity.ActivityOrder = wfs.ActivityOrder;
                            NextActivity.PreviousActOrder = wfs.PreviousActOrder;
                            NextActivity.ProcessRefId = TaskSystem.Id;
                            NextActivity.Available = true;
                            NextActivity.Assigned = false;
                            NextActivity.Completed = false;
                            NextActivity.BranchCode = TaskSystem.BranchCode;
                            NextActivity.DeptCode = TaskSystem.DeptCode;
                            PSF.SaveOrUpdate<Activity>(NextActivity);
                            TaskSystem.Status = NextActivity.ActivityName;
                            PSF.SaveOrUpdate<TaskSystem>(TaskSystem);
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

        public Dictionary<long, IList<TaskSystemActivity>> GetTaskSystemActivityListWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<TaskSystemActivity>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public Dictionary<long, IList<Activity>> GetActivityWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<Activity>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public Dictionary<long, IList<StaffActivities>> GetStaffActivityWithsearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, string ColumnName, string[] values, string[] alias)
        {
            try
            {
                return PSF.GetListWithSearchCriteriaCountArray<StaffActivities>(page, pageSize, sortBy, sortType, ColumnName, values, criteria, alias);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        #region Added By Prabakaran
        public Dictionary<long, IList<CallMgmntPIView>> GetProcessInstanceViewListWithExactandLikesearchCriteria(int? page, int? pageSize, string sortBy, string sortType, Dictionary<string, object> criteria, Dictionary<string, object> likecriteria)
        {
            try
            {
                return PSF.GetListWithExactAndLikeSearchCriteriaWithCount<CallMgmntPIView>(page, pageSize, sortBy, sortType, criteria,likecriteria);
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        #endregion
        #region Added By Prabakaran
        public StaffActivities GetStaffActivitiesById(long Id)
        {
            try
            {
                if (Id > 0)
                    return PSF.Get<StaffActivities>(Id);
                else throw new Exception("Id is required and it cannot be zero.");
            }
            catch (Exception) { throw; }
            finally { if (PSF != null) PSF.Dispose(); }
        }
        public StaffActivities GetStaffActivities(long InstanceId, long ProcessRefId)
        {
            try
            {
                StaffActivities staffactivities = null;
                if (InstanceId > 0 && ProcessRefId > 0)
                {
                    staffactivities = PSF.Get<StaffActivities>("InstanceId", InstanceId, "ProcessRefId", ProcessRefId);
                }
                else { throw new Exception("Id is Required"); }
                return staffactivities;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long CreateOrUpdateStaffActivities(StaffActivities staffactivities)
        {
            try
            {
                if (staffactivities != null)
                {
                    PSF.SaveOrUpdate<StaffActivities>(staffactivities);
                }
                else { throw new Exception("StaffActivities Details Required"); }
                return staffactivities.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //public StaffActivities GetStaffActivitiesByActivityId(long ActivityId)
        //{
        //    try
        //    {
        //        StaffActivities staffactivities = null;
        //        if (ActivityId > 0)
        //        {
        //            staffactivities = PSF.Get<StaffActivities>(ActivityId);
        //        }
        //        else { throw new Exception("Id is Required"); }
        //        return staffactivities;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //public StaffIssues GetStaffIssuesById(long Id)
        //{
        //    try
        //    {
        //        StaffIssues staffissues = null;
        //        if (Id > 0)
        //        {
        //            staffissues = PSF.Get<StaffIssues>(Id);
        //        }
        //        else { throw new Exception("Id is Required"); }
        //        return staffissues;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public StaffActivities GetStaffActivitiesByActivityId(long ActivityId)
        {
            try
            {
                StaffActivities staffactivities = null;
                if (ActivityId > 0)
                {
                    staffactivities = PSF.Get<StaffActivities>(ActivityId);
                }
                else { throw new Exception("Id is Required"); }
                return staffactivities;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public StaffIssues GetStaffIssuesById(long Id)
        {
            try
            {
                StaffIssues staffissues = null;
                if (Id > 0)
                {
                    staffissues = PSF.Get<StaffIssues>(Id);
                }
                else { throw new Exception("Id is Required"); }
                return staffissues;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
