using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistenceFactory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using System.Data;
using Castle.Components.DictionaryAdapter;
using System.ComponentModel;
using TIPS.Entities.Assess;
namespace TIPS.Component
{
    public class PSFHelperClassBC
    {
        PersistenceServiceFactory PSF = null;
        static ISessionFactory sessionFactory;
        public PSFHelperClassBC()
        {
            List<String> Assembly = new List<String>();
            Assembly.Add("TIPS.Entities");
            Assembly.Add("TIPS.Entities.Assess");
            Assembly.Add("TIPS.Entities.TicketingSystem");
            Assembly.Add("TIPS.Entities.TaskManagement");
            PSF = new PersistenceServiceFactory(Assembly);

            sessionFactory = PersistenceServiceFactory.sessionFactory;
            //Session = sessionFactory.OpenSession();
        }
        public Dictionary<long, IList<Assess360BulkInsert>> GetAssess360BulkInsertListWithHQLQuery(string[] columns, object[] values)
        {
            try
            {
                Dictionary<long, IList<Assess360BulkInsert>> retValue = new System.Collections.Generic.Dictionary<long, System.Collections.Generic.IList<Assess360BulkInsert>>();
                //arrayColumns[0] = "AssessCompGroup"; 
                //arrayColumns[1] = "Subject"; 
                //arrayColumns[2] = "AssignmentName";
                //arrayColumns[3] = "Assess360Id"; 
                //arrayColumns[4] = "AcademicYear"; 
                //arrayColumns[5] = "Grade";
                //arrayColumns[6] = "Campus"; 
                //arrayColumns[7] = "Section";
                using (ISession session = sessionFactory.OpenSession())
                {
                    IList<Assess360BulkInsert> Assess360BulkInsertList = session
                    .CreateQuery("from Assess360BulkInsert p where "+
                    "p.AssessCompGroup = :AssessCompGroup AND "+
                    "p.Subject in(:Subject) and " +
                    "p.AssignmentName in( :AssignmentName) and p.Assess360Id in(:Assess360Id) and AcademicYear=:AcademicYear and " +
                    "p.Grade=:Grade and p.Campus=:Campus and p.Section=:Section")
                    .SetParameter(columns[0], values[0]).SetParameter(columns[1], values[1])
                    .SetParameter(columns[2], values[2])
                    .SetParameterList(columns[3], values[3] as IEnumerable<long>).SetParameter(columns[4], values[4]).SetParameter(columns[5], values[5])
                    .SetParameter(columns[6], values[6]).SetParameter(columns[7], values[7])
                    .List<Assess360BulkInsert>();
                    if (Assess360BulkInsertList != null && Assess360BulkInsertList.Count > 0)
                    {
                        retValue.Add(Assess360BulkInsertList.Count, Assess360BulkInsertList);
                    }
                }
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Added for IB Main Report card to do take care of semester wise in Assess 360 bulk entry 
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public Dictionary<long, IList<Assess360BulkInsert>> GetAssess360BulkInsertListWithHQLQueryforIBMain(string[] columns, object[] values)
        {
            try
            {
                Dictionary<long, IList<Assess360BulkInsert>> retValue = new System.Collections.Generic.Dictionary<long, System.Collections.Generic.IList<Assess360BulkInsert>>();
                using (ISession session = sessionFactory.OpenSession())
                {
                    IList<Assess360BulkInsert> Assess360BulkInsertList = session
                    .CreateQuery("from Assess360BulkInsert p where " +
                    "p.AssessCompGroup = :AssessCompGroup AND " +
                    "p.Subject in(:Subject) and " +
                    "p.AssignmentName in( :AssignmentName) and p.Assess360Id in(:Assess360Id) and AcademicYear=:AcademicYear and " +
                    "p.Grade=:Grade and p.Campus=:Campus and p.Section=:Section and p.Semester=:Semester")
                    .SetParameter(columns[0], values[0]).SetParameter(columns[1], values[1]).SetParameter(columns[2], values[2])
                    .SetParameterList(columns[3], values[3] as IEnumerable<long>).SetParameter(columns[4], values[4]).SetParameter(columns[5], values[5])
                    .SetParameter(columns[6], values[6]).SetParameter(columns[7], values[7]).SetParameter(columns[8], values[8])
                    .List<Assess360BulkInsert>();
                    if (Assess360BulkInsertList != null && Assess360BulkInsertList.Count > 0)
                    {
                        retValue.Add(Assess360BulkInsertList.Count, Assess360BulkInsertList);
                    }
                }
                return retValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
