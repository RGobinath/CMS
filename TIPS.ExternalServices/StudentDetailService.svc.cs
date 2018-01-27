using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TIPS.ExternalServiceContract;
using TIPS.Entities;
using TIPS.Component;
using System.Web.Security;

namespace TIPS.ExternalServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class StudentDetailService : IStudentDetailService
    {
        public bool ValidateWebServiceUser(string uid, string pwd, string roleCode)
        {
            try
            {
                if (uid == "WSU" && pwd == "p@ssw0rd")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StudentDetailsVw>> GetStudentDetailsListWithPagingAndCriteria(int? page, int? pageSize, string sortby, string sortType, string uid, string pwd, string roleCode, Dictionary<string, object> criteria)
        {
            try
            {
                if (uid == "WSU" && pwd == "p@ssw0rd")
                {
                    MasterDataBC MasterDataBC = new MasterDataBC();
                    return MasterDataBC.GetStudentDetailsVwListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
                }
                else
                {
                    throw new FaultException("The user name or password provided is incorrect.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<StudentDetailsVw> GetStudentDetailsListWithPagingWithoutCount(int? page, int? pageSize, string sortby, string sortType, string uid, string pwd, string roleCode, Dictionary<string, object> criteria)
        {
            try
            {
                if (uid == "WSU" && pwd == "p@ssw0rd")
                {
                    MasterDataBC MasterDataBC = new MasterDataBC();
                    Dictionary<long, IList<StudentDetailsVw>> retValue = MasterDataBC.GetStudentDetailsVwListWithPagingAndCriteria(page, pageSize, sortType, sortby, criteria);
                    if (retValue != null && retValue.First().Value != null)
                        return retValue.FirstOrDefault().Value;
                    else throw new Exception("No records found!");
                }
                else
                {
                    throw new FaultException("The user name or password provided is incorrect.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StudentDetailsVw>> GetStudentDetailsListWithPagingCriteriaEnum(int? page, int? pageSize, string sortby, string sortType, string uid, string pwd, string roleCode, Dictionary<StudentDetails_Enum, object> criteria)
        {
            try
            {
                if (uid == "WSU" && pwd == "p@ssw0rd")
                {
                    MasterDataBC MasterDataBC = new MasterDataBC();
                    Dictionary<string, object> FinalCriteria = new Dictionary<string, object>();
                    foreach (KeyValuePair<StudentDetails_Enum, object> key in criteria)
                    {
                        FinalCriteria.Add(key.Key.ToString(), key.Value);
                    }
                    return MasterDataBC.GetStudentDetailsVwListWithPagingAndCriteria(page, pageSize, sortType, sortby, FinalCriteria);
                }
                else
                {
                    throw new FaultException("The user name or password provided is incorrect.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<StudFamilyDetailsEmail> GetStudFamilyDetailsEmailListForAStudent(string AppNo, string uid, string pwd, string roleCode)
        {
            try
            {
                if (uid == "WSU" && pwd == "p@ssw0rd")
                {
                    MasterDataBC MasterDataBC = new MasterDataBC();
                    Dictionary<string, object> FinalCriteria = new Dictionary<string, object>();
                    FinalCriteria.Add("ApplicationNo", AppNo);
                    Dictionary<long, IList<StudFamilyDetailsEmail>> retValue = MasterDataBC.GetStudFamilyDetailsEmailListForAStudent(0, 10000, "ApplicationNo", "Asc", FinalCriteria);
                    if (retValue != null && retValue.Values != null && retValue.Values.FirstOrDefault() != null)
                    {
                        return retValue.Values.FirstOrDefault();
                    }
                    else return null;
                }
                else
                {
                    throw new FaultException("The user name or password provided is incorrect.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StudFamilyDetailsEmail>> GetStudFamilyDetailsEmailListForAppnos(string[] AppNo, string uid, string pwd, string roleCode)
        {
            try
            {
                if (uid == "WSU" && pwd == "p@ssw0rd")
                {
                    MasterDataBC MasterDataBC = new MasterDataBC();
                    string colName = "ApplicationNo";
                    string[] Values = AppNo;
                    return MasterDataBC.GetStudFamilyDetailsEmailListForAppnos(0, 10000, "ApplicationNo", "Asc", null, colName, Values);
                }
                else
                {
                    throw new FaultException("The user name or password provided is incorrect.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<long, IList<StudFamilyDetailsEmail>> GetStudFamilyDetailsEmailListWithFamilyType(string[] AppNo, FamilyTypeEnum FamilyTypeEnum, string uid, string pwd, string roleCode)
        {
            try
            {
                if (uid == "WSU" && pwd == "p@ssw0rd")
                {
                    MasterDataBC MasterDataBC = new MasterDataBC();
                    string colName = "ApplicationNo";
                    string[] Values = AppNo;
                    string FTypeColName = "FamilyDetailType";string[] FTvalue=new string[1];
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    switch (FamilyTypeEnum)
                    {
                        case FamilyTypeEnum.Father:
                            FTvalue[0] = "Father";
                            break;
                        case FamilyTypeEnum.Mother:
                            FTvalue[0] = "Mother";
                            break;
                        case FamilyTypeEnum.GrandFather:
                            FTvalue[0] = "GrandFather";
                            break;
                        case FamilyTypeEnum.GrandMother:
                            FTvalue[0] = "GrandMother";
                            break;
                        case FamilyTypeEnum.Others:
                            FTvalue[0] = "Others";
                            break;
                        case FamilyTypeEnum.Uncle:
                            FTvalue[0] = "Uncle";
                            break;
                        case FamilyTypeEnum.Sister:
                            FTvalue[0] = "Sister";
                            break;
                        case FamilyTypeEnum.Aunt:
                            FTvalue[0] = "Aunt";
                            break;
                        case FamilyTypeEnum.Brother:
                            FTvalue[0] = "Brother";
                            break;
                        default:
                            break;
                    }
                    criteria.Add(FTypeColName, FTvalue);
                    return MasterDataBC.GetStudFamilyDetailsEmailListForAppnos(0, 10000, "ApplicationNo", "Asc", criteria, colName, Values);
                }
                else
                {
                    throw new FaultException("The user name or password provided is incorrect.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<long, IList<StudFamilyDetailsEmail>> GetStudFamilyDtlsEmailLstWithFamilyTypeForIdNo(string[] IdNo, FamilyTypeEnum FamilyTypeEnum, string uid, string pwd, string roleCode)
        {
            try
            {
                if (uid == "WSU" && pwd == "p@ssw0rd")
                {
                    MasterDataBC MasterDataBC = new MasterDataBC();
                    string colName = "NewId";
                    string[] Values = IdNo;
                    string FTypeColName = "FamilyDetailType"; string[] FTvalue = new string[1];
                    Dictionary<string, object> criteria = new Dictionary<string, object>();
                    switch (FamilyTypeEnum)
                    {
                        case FamilyTypeEnum.Father:
                            FTvalue[0] = "Father";
                            break;
                        case FamilyTypeEnum.Mother:
                            FTvalue[0] = "Mother";
                            break;
                        case FamilyTypeEnum.GrandFather:
                            FTvalue[0] = "GrandFather";
                            break;
                        case FamilyTypeEnum.GrandMother:
                            FTvalue[0] = "GrandMother";
                            break;
                        case FamilyTypeEnum.Others:
                            FTvalue[0] = "Others";
                            break;
                        case FamilyTypeEnum.Uncle:
                            FTvalue[0] = "Uncle";
                            break;
                        case FamilyTypeEnum.Sister:
                            FTvalue[0] = "Sister";
                            break;
                        case FamilyTypeEnum.Aunt:
                            FTvalue[0] = "Aunt";
                            break;
                        case FamilyTypeEnum.Brother:
                            FTvalue[0] = "Brother";
                            break;
                        default:
                            break;
                    }
                    criteria.Add(FTypeColName, FTvalue);
                    return MasterDataBC.GetStudFamilyDetailsEmailListForAppnos(0, 10000, "ApplicationNo", "Asc", criteria, colName, Values);
                }
                else
                {
                    throw new FaultException("The user name or password provided is incorrect.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
