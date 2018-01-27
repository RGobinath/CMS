using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using TIPS.Entities.ParentPortalEntities;
using TIPS.ServiceContract;
using TIPS.Entities.LMSEntities;
using System.Text;
using Newtonsoft.Json.Linq;
using TIPS.Entities.StaffManagementEntities;
using System.Configuration;
using TIPS.Entities.AdmissionEntities;
using TIPS.Entities;

namespace CMS.Controllers
{
    public class MobileAPIController : ApiController
    {

        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}

        public HttpResponseMessage GetAttachmentsFileStream(int Id)
        {
            ParentPortalService pps = new ParentPortalService();
            Dictionary<string, object> Doccriteria = new Dictionary<string, object>();
            Doccriteria.Add("Id", Convert.ToInt64(Id));
            Dictionary<long, IList<NoteAttachment>> docList = pps.GetDocumentsListWithPaging(0, 9999, string.Empty, string.Empty, Doccriteria);
            IList<NoteAttachment> list = docList.FirstOrDefault().Value;
            string extObj = Path.GetExtension(list[0].AttachmentName);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            File.WriteAllBytes(@"D:\ICMS APPS\API PDF\GetAttachmentsFileStream\" + list[0].AttachmentName + "", list.FirstOrDefault().Attachment);
            response.Content = new StreamContent(new FileStream(@"D:\ICMS APPS\API PDF\GetAttachmentsFileStream\" + list[0].AttachmentName + "", FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = list[0].AttachmentName;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return response;
        }

        public HttpResponseMessage GetAttachmentsMemoryStream(int Id)
        {

            ParentPortalService pps = new ParentPortalService();
            Dictionary<string, object> Doccriteria = new Dictionary<string, object>();
            Doccriteria.Add("Id", Convert.ToInt64(Id));
            Dictionary<long, IList<NoteAttachment>> docList = pps.GetDocumentsListWithPaging(0, 9999, string.Empty, string.Empty, Doccriteria);
            IList<NoteAttachment> list = docList.FirstOrDefault().Value;
            string extObj = Path.GetExtension(list[0].AttachmentName);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            File.WriteAllBytes(@"D:\ICMS APPS\API PDF\GetAttachmentsMemoryStream\" + list[0].AttachmentName + "", list.FirstOrDefault().Attachment);
            response.Content = new StreamContent(new FileStream(@"D:\ICMS APPS\API PDF\GetAttachmentsMemoryStream\" + list[0].AttachmentName + "", FileMode.Open, FileAccess.Read));
            MemoryStream ms = new MemoryStream(File.ReadAllBytes(@"D:\ICMS APPS\API PDF\GetAttachmentsMemoryStream\" + list[0].AttachmentName + ""));
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = list[0].AttachmentName
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            // application/octet-stream
            return response;
        }

        //public IList<DateTime> GetCurrentAbsentList(string userId)
        //{
        //    AdmissionManagementService AMS = new AdmissionManagementService();
        //    AttendanceService attServObj = new AttendanceService();
        //    //string stNewId = userId.Substring(1, userI;
        //    string stNewId = userId.Substring(1);
        //    if (!string.IsNullOrEmpty(stNewId))
        //    {
        //        StudentTemplate st = AMS.GetStudentDetailsByNewId(stNewId.Trim());
        //        if (st != null)
        //        {
        //            Dictionary<string, object> Criteria = new Dictionary<string, object>();
        //            Criteria.Add("PreRegNum", st.PreRegNum);
        //            if (st.Grade == "IX" || st.Grade == "X" || st.Grade == "XI" || st.Grade == "XII")
        //            {
        //                Criteria.Add("AcademicYear", DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString());
        //            }
        //            else
        //            {
        //                if (DateTime.Now.Month >= 5)
        //                {
        //                    Criteria.Add("AcademicYear", DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString());
        //                }
        //                else
        //                {
        //                    Criteria.Add("AcademicYear", (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString());
        //                }
        //            }
        //            Dictionary<long, IList<StudentAttendance>> AbsentList = attServObj.GetStudentAbsentListForAnAttendanceListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, Criteria);
        //            IList<DateTime> absentListObj = new List<DateTime>();
        //            foreach (var item in AbsentList.FirstOrDefault().Value)
        //            {
        //                if (item.AbsentDate != null)
        //                {
        //                    absentListObj.Add(item.AbsentDate);
        //                }
        //            }
        //            //absentListObj = (from u in AbsentList.First().Value
        //            //               select new { u.AbsentDate }).ToList();
        //            return absentListObj;
        //        }
        //    }
        //    return null;
        //}

        //public HttpResponseMessage GetStudentAbsentDates(string userId)
        //{
        //    List<StudentAttendance> attenLstObj = new List<StudentAttendance>();
        //    AdmissionManagementService AMS = new AdmissionManagementService();
        //    AttendanceService attServObj = new AttendanceService();
        //    if (!string.IsNullOrEmpty(userId.Substring(1).Trim()))
        //    {
        //        StudentTemplate st = AMS.GetStudentDetailsByNewId(userId.Substring(1).Trim());
        //        if (st != null)
        //        {
        //            Dictionary<string, object> Criteria = new Dictionary<string, object>();
        //            Criteria.Add("PreRegNum", st.PreRegNum);
        //            switch (st.Grade)
        //            {
        //                case "IX":
        //                case "X":
        //                case "XI":
        //                case "XII":
        //                    {
        //                        Criteria.Add("AcademicYear", DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString());
        //                        break;
        //                    }
        //                default:
        //                    {
        //                        if (DateTime.Now.Month >= 5)
        //                        {
        //                            Criteria.Add("AcademicYear", DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Year + 1).ToString());
        //                        }
        //                        else
        //                        {
        //                            Criteria.Add("AcademicYear", (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Year.ToString());
        //                        }
        //                        break;
        //                    }
        //            }
        //            Dictionary<long, IList<StudentAttendance>> AbsentList = attServObj.GetStudentAbsentListForAnAttendanceListWithPagingAndCriteria(0, 9999, string.Empty, string.Empty, Criteria);
        //            attenLstObj = AbsentList.FirstOrDefault().Value.ToList();
        //        }
        //    }
        //    return new HttpResponseMessage()
        //    {
        //        Content = new StringContent(JArray.FromObject(attenLstObj).ToString(), Encoding.UTF8, "application/json")
        //    };
        //}

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpPost]
        public HttpResponseMessage GetStaffandStudentDetailsList(long PreRegNum, string UserType)
        {
            LMSService lms = new LMSService();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(UserType))
                criteria.Add("UserType", UserType);
            if (PreRegNum > 0)
                criteria.Add("PreRegNumber", PreRegNum);
            try
            {
                Dictionary<long, IList<LMS_StaffStudentDetails_Vw>> staffstudentdetails = lms.GetStaffandStudentDetailsListwithCriteria(0, 10000, string.Empty, string.Empty, criteria);
                return new HttpResponseMessage()
                {
                    Content = new StringContent(JArray.FromObject(staffstudentdetails.FirstOrDefault().Value).ToString(), Encoding.UTF8, "application/json")
                };
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Error");
            }
        }
        
        public HttpResponseMessage GetStaffDetailsByIdNumberval(string IdNumber)
        {
            StaffManagementService smsObj = new StaffManagementService();
            StaffDetails sdetails = smsObj.GetStaffDeatailsByIdNumber(IdNumber);
            if (sdetails != null && sdetails.PreRegNum > 0)
            {
                StaffAttendance sattendance = smsObj.GetStaffAttendanceByAttendanceDatewithPreRegNum(sdetails.PreRegNum);
                if (sdetails != null)
                {
                    var checkin = string.Empty;
                    if (sattendance == null)
                    {
                        checkin = "true";
                    }
                    else if (sattendance != null && sattendance.LogOut == null)
                    {
                        checkin = "false";
                    }
                    else if (sattendance != null && sattendance.LogOut != null)
                    {
                        checkin = "completed";
                    }
                    var jsondata = new
                    {
                        Name = sdetails.Name,
                        Designation = sdetails.Designation,
                        Campus = sdetails.Campus,
                        IdNumber = sdetails.IdNumber,
                        Department = sdetails.Department,
                        checkinval = checkin
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, jsondata);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, "failed");
        }
        [HttpGet]
        public HttpResponseMessage SaveStaffAttendanceDetails(string IdNumber)
        {
            try
            {
                StaffManagementService smsObj = new StaffManagementService();
                StaffDetails sdetails = smsObj.GetStaffDeatailsByIdNumber(IdNumber);
                if (sdetails != null && sdetails.PreRegNum > 0)
                {
                    StaffAttendance sa = new StaffAttendance();
                    sa.PreRegNum = sdetails.PreRegNum;
                    sa.AttendanceDate = DateTime.Now.Date;
                    sa.LogIn = DateTime.Now;
                    sa.CreatedDate = DateTime.Now;
                    sa.CreatedBy = "";
                    smsObj.CreateOrUpdateStaffAttendance(sa);
                    return Request.CreateResponse(HttpStatusCode.OK, "success");
                }
                return Request.CreateResponse(HttpStatusCode.OK, "failed");
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Error");
            }
        }
        [HttpGet]
        public HttpResponseMessage EditStaffAttendanceDetails(string IdNumber)
        {
            try
            {
                StaffManagementService smsObj = new StaffManagementService();
                StaffDetails sdetails = smsObj.GetStaffDeatailsByIdNumber(IdNumber);
                if (sdetails != null && sdetails.PreRegNum > 0)
                {
                    StaffAttendance sattendance = smsObj.GetStaffAttendanceByAttendanceDatewithPreRegNum(sdetails.PreRegNum);
                    if (sattendance != null)
                    {
                        sattendance.LogOut = DateTime.Now;
                        sattendance.ModifiedBy = "";
                        sattendance.ModifiedDate = DateTime.Now;
                        smsObj.CreateOrUpdateStaffAttendance(sattendance);
                        return Request.CreateResponse(HttpStatusCode.OK, "success");
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, "failed");

            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Error");
            }
        }
        [HttpGet]
        public HttpResponseMessage StaffIdNumberAutoComplete()
        {
            try
            {
                StaffManagementService sms = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                //if (!string.IsNullOrEmpty(Campus))
                //{ criteria.Add("Campus", Campus); }
                //if (!string.IsNullOrEmpty(term))
                //{ criteria.Add("IdNumber", term); }
                criteria.Add("Status", "Registered");
                Dictionary<long, IList<StaffDetailsView>> StaffDetails = sms.GetStaffDetailsViewListWithPaging(0, 9999, "IdNumber", string.Empty, criteria);
                if (StaffDetails.Count > 0 && StaffDetails.FirstOrDefault().Key > 0)
                {
                    var StaffIdNumber = (from u in StaffDetails.FirstOrDefault().Value where u.IdNumber != null && u.PreRegNum > 0 select new { IdNumber = u.IdNumber }).ToList();
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(JArray.FromObject(StaffIdNumber).ToString(), Encoding.UTF8, "application/json")
                    };
                }
                return Request.CreateResponse(HttpStatusCode.OK, "failed");
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Error");
            }
        }
        [HttpGet]
        public HttpResponseMessage uploaddisplay1(string IdNumber)
        {
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                StaffManagementService smsObj = new StaffManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                StaffDetails sdetails = smsObj.GetStaffDeatailsByIdNumber(IdNumber);
                if (sdetails != null && sdetails.PreRegNum > 0)
                {
                    criteria.Add("PreRegNum", Convert.ToInt64(sdetails.PreRegNum));
                    criteria.Add("DocumentFor", "Staff");
                    criteria.Add("DocumentType", "Staff Photo");
                    Dictionary<long, IList<UploadedFiles>> UploadedFiles = ads.GetUploadedFilesListWithPagingAndCriteria(0, 1000, string.Empty, string.Empty, criteria);
                    if (UploadedFiles != null && UploadedFiles.FirstOrDefault().Value != null && UploadedFiles.FirstOrDefault().Value.Count != 0)
                    {
                        if (UploadedFiles.First().Value[0].OldFiles == 1)
                        {
                            string ImagePath = UploadedFiles.First().Value[0].FileDirectory + "\\" + UploadedFiles.First().Value[0].DocumentName;// "green.jpg"; ConfigurationManager.AppSettings["ImageFilePath1"] 

                            if (!System.IO.File.Exists(ImagePath))
                            {
                                ImagePath = ConfigurationManager.AppSettings["ImageFilePath1"] + UploadedFiles.First().Value[0].DocumentName;

                            }
                            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                            MemoryStream ms = new MemoryStream(File.ReadAllBytes(ImagePath));
                            response.Content = new StreamContent(ms);
                            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = UploadedFiles.First().Value[0].DocumentName
                            };
                            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            // application/octet-stream
                            return response;

                        }
                        else
                        {
                            IList<UploadedFiles> list = UploadedFiles.FirstOrDefault().Value;
                            //    IList<UploadedFiles> list = UploadedFiles;
                            UploadedFiles doc = list.FirstOrDefault();
                            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                            if (doc.DocumentData != null)
                            {
                                MemoryStream ms = new MemoryStream(doc.DocumentData);
                                response.Content = new StreamContent(ms);
                                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                                {
                                    FileName = doc.DocumentName
                                };
                                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                                // application/octet-stream
                                return response;
                            }
                            else
                            {
                                string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                                MemoryStream ms = new MemoryStream(File.ReadAllBytes(ImagePath));
                                response.Content = new StreamContent(ms);
                                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                                {
                                    FileName = "no_image.jpg"
                                };
                                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                                // application/octet-stream
                                return response;
                            }
                        }
                    }
                    else
                    {
                        string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                        MemoryStream ms = new MemoryStream(File.ReadAllBytes(ImagePath));
                        response.Content = new StreamContent(ms);
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = "no_image.jpg"
                        };
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        // application/octet-stream
                        return response;
                    }
                }
                else
                {
                    string ImagePath = ConfigurationManager.AppSettings["ImageFilePath"];
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    MemoryStream ms = new MemoryStream(File.ReadAllBytes(ImagePath));
                    response.Content = new StreamContent(ms);
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "no_image.jpg"
                    };
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    // application/octet-stream
                    return response;
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Error");
            }
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpGet]
        public HttpResponseMessage GetFilepathList(string IdNumber)
        {
            HttpResponseMessage objResponseMessage = new HttpResponseMessage();
            ResponseResult objResponse = new ResponseResult();
            try
            {
                AdmissionManagementService ads = new AdmissionManagementService();
                Dictionary<string, object> criteria = new Dictionary<string, object>();
                StudentTemplate studetails = ads.GetStudentDetailsByNewId(IdNumber);
                if (studetails != null && studetails.PreRegNum > 0)
                {
                    if (studetails.AdmissionStatus == "Registered")
                    {
                        IList<FileContentSharing> objFilepath = new List<FileContentSharing>();
                        string folderPath = ConfigurationManager.AppSettings["AppFilePath"].ToString();
                        folderPath = folderPath + studetails.Grade;
                        foreach (var path in Directory.GetFiles(@folderPath))
                        {
                            FileContentSharing obj = new FileContentSharing();
                            obj.FileName = System.IO.Path.GetFileName(path); //file name
                            obj.FilePath = path; // full path
                            FileInfo fi = new FileInfo(path);
                            obj.FileSize = fi.Length;
                            objFilepath.Add(obj);
                        }
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(JArray.FromObject(objFilepath).ToString(), Encoding.UTF8, "application/json")
                        };

                    }
                    else
                    {
                        objResponse.isValid = false;
                        objResponse.Result = "Yet to register given student ID Number";
                        return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                    }

                }
                else
                {
                    objResponse.isValid = false;
                    objResponse.Result = "In Valid Student Id Number";
                    return Request.CreateResponse(HttpStatusCode.OK, objResponse);
                }
            }
            catch (Exception)
            {
                objResponse.isValid = false;
                objResponse.Result = "Error";
                return Request.CreateResponse(HttpStatusCode.OK, objResponse);
            }
        }
    }
}