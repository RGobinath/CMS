using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.Assess
{
    public class Assess360MarksCaluctionForAStudent_Vw
    {
        public virtual long UniqId { get; set; }
        public virtual long Id { get; set; }
        public virtual long StudentId { get; set; }
        public virtual string IdNo { get; set; }
        public virtual string Name { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Section { get; set; }
        public virtual string Grade { get; set; }
        public virtual string GroupName { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Semester { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual decimal SumOfMarks { get; set; }
        public virtual long CountOfMarks { get; set; }
        public virtual decimal OverAllMrks { get; set; }

        /// <summary>
        /// For English As Second Language
        /// </summary>
       
        //public virtual Int32 TermI_EngAsSeocndLAng { get; set; }
        //public virtual Int32 TermI_Star_EngAsSeocndLAng { get; set; }

        //public virtual Int32 TermI_HW_EngAsSeocndLAng { get; set; }
        //public virtual Int32 TermI_FA_EngAsSeocndLAng { get; set; }
        //public virtual Int32 TermI_AS_EngAsSeocndLAng { get; set; }
        //public virtual Int32 TermI_OSP_EngAsSeocndLAng { get; set; }

        //public virtual string TermI_HW_Grade_EngAsSeocndLAng { get; set; }
        //public virtual string TermI_FA_Grade_EngAsSeocndLAng { get; set; }
        //public virtual string TermI_AS_Grade_EngAsSeocndLAng { get; set; }
        //public virtual string TermI_OSP_Grade_EngAsSeocndLAng { get; set; }

        //public virtual Int32 TermII_EngAsSeocndLAng { get; set; }
        //public virtual Int32 TermII_Star_EngAsSeocndLAng { get; set; }

        //public virtual Int32 TermII_HW_EngAsSeocndLAng { get; set; }
        //public virtual Int32 TermII_FA_EngAsSeocndLAng { get; set; }
        //public virtual Int32 TermII_AS_EngAsSeocndLAng { get; set; }
        //public virtual Int32 TermII_OSP_EngAsSeocndLAng { get; set; }

        //public virtual string TermII_HW_Grade_EngAsSeocndLAng { get; set; }
        //public virtual string TermII_FA_Grade_EngAsSeocndLAng { get; set; }
        //public virtual string TermII_AS_Grade_EngAsSeocndLAng { get; set; }
        //public virtual string TermII_OSP_Grade_EngAsSeocndLAng { get; set; }

        //public virtual Int32 TermIII_EngAsSeocndLAng { get; set; }
        //public virtual Int32 TermIII_Star_EngAsSeocndLAng { get; set; }

        //public virtual Int32 TermIII_HW_EngAsSeocndLAng { get; set; }
        //public virtual Int32 TermIII_FA_EngAsSeocndLAng { get; set; }
        //public virtual Int32 TermIII_AS_EngAsSeocndLAng { get; set; }
        //public virtual Int32 TermIII_OSP_EngAsSeocndLAng { get; set; }

        //public virtual string TermIII_HW_Grade_EngAsSeocndLAng { get; set; }
        //public virtual string TermIII_FA_Grade_EngAsSeocndLAng { get; set; }
        //public virtual string TermIII_AS_Grade_EngAsSeocndLAng { get; set; }
        //public virtual string TermIII_OSP_Grade_EngAsSeocndLAng { get; set; }

        ///// <summary>
        ///// For French Or Hindi Language
        ///// </summary>
        //public virtual Int32 TermI_FrenchOrHindi { get; set; }
        //public virtual Int32 TermI_Star_FrenchOrHindi { get; set; }

        //public virtual Int32 TermI_HW_FrenchOrHindi { get; set; }
        //public virtual Int32 TermI_FA_FrenchOrHindi { get; set; }
        //public virtual Int32 TermI_AS_FrenchOrHindi { get; set; }
        //public virtual Int32 TermI_OSP_FrenchOrHindi { get; set; }

        //public virtual string TermI_HW_Grade_FrenchOrHindi { get; set; }
        //public virtual string TermI_FA_Grade_FrenchOrHindi { get; set; }
        //public virtual string TermI_AS_Grade_FrenchOrHindi { get; set; }
        //public virtual string TermI_OSP_Grade_FrenchOrHindi { get; set; }


        //public virtual Int32 TermII_FrenchOrHindi { get; set; }
        //public virtual Int32 TermII_Star_FrenchOrHindi { get; set; }

        //public virtual Int32 TermII_HW_FrenchOrHindi { get; set; }
        //public virtual Int32 TermII_FA_FrenchOrHindi { get; set; }
        //public virtual Int32 TermII_AS_FrenchOrHindi { get; set; }
        //public virtual Int32 TermII_OSP_FrenchOrHindi { get; set; }

        //public virtual string TermII_HW_Grade_FrenchOrHindi { get; set; }
        //public virtual string TermII_FA_Grade_FrenchOrHindi { get; set; }
        //public virtual string TermII_AS_Grade_FrenchOrHindi { get; set; }
        //public virtual string TermII_OSP_Grade_FrenchOrHindi { get; set; }


        //public virtual Int32 TermIII_FrenchOrHindi { get; set; }
        //public virtual Int32 TermIII_Star_FrenchOrHindi { get; set; }

        //public virtual Int32 TermIII_HW_FrenchOrHindi { get; set; }
        //public virtual Int32 TermIII_FA_FrenchOrHindi { get; set; }
        //public virtual Int32 TermIII_AS_FrenchOrHindi { get; set; }
        //public virtual Int32 TermIII_OSP_FrenchOrHindi { get; set; }

        //public virtual string TermIII_Grade_HW_FrenchOrHindi { get; set; }
        //public virtual string TermIII_Grade_FA_FrenchOrHindi { get; set; }
        //public virtual string TermIII_Grade_AS_FrenchOrHindi { get; set; }
        //public virtual string TermIII_OSP_Grade_FrenchOrHindi { get; set; }

        ///// <summary>
        ///// For Mathematics Language
        ///// </summary>
        //public virtual Int32 TermI_Math { get; set; }
        //public virtual Int32 TermI_Star_Math { get; set; }

        //public virtual Int32 TermI_HW_Math { get; set; }
        //public virtual Int32 TermI_FA_Math { get; set; }
        //public virtual Int32 TermI_AS_Math { get; set; }
        //public virtual Int32 TermI_OSP_Math { get; set; }

        //public virtual string TermI_HW_Grade_Math { get; set; }
        //public virtual string TermI_FA_Grade_Math { get; set; }
        //public virtual string TermI_AS_Grade_Math { get; set; }
        //public virtual string TermI_OSP_Grade_Math { get; set; }

        //public virtual Int32 TermII_Math { get; set; }
        //public virtual Int32 TermII_Star_Math { get; set; }

        //public virtual Int32 TermII_HW_Math { get; set; }
        //public virtual Int32 TermII_FA_Math { get; set; }
        //public virtual Int32 TermII_AS_Math { get; set; }
        //public virtual Int32 TermII_OSP_Math { get; set; }

        //public virtual string TermII_HW_Grade_Math { get; set; }
        //public virtual string TermII_FA_Grade_Math { get; set; }
        //public virtual string TermII_AS_Grade_Math { get; set; }
        //public virtual string TermII_OSP_Grade_Math { get; set; }

        //public virtual Int32 TermIII_Math { get; set; }
        //public virtual Int32 TermIII_Star_Math { get; set; }

        //public virtual Int32 TermIII_HW_Math { get; set; }
        //public virtual Int32 TermIII_FA_Math { get; set; }
        //public virtual Int32 TermIII_AS_Math { get; set; }
        //public virtual Int32 TermIII_OSP_Math { get; set; }

        //public virtual string TermIII_HW_Grade_Math { get; set; }
        //public virtual string TermIII_FA_Grade_Math { get; set; }
        //public virtual string TermIII_AS_Grade_Math { get; set; }
        //public virtual string TermIII_OSP_Grade_Math { get; set; }

        ///// <summary>
        ///// For Physics Language
        ///// </summary>
        //public virtual Int32 TermI_Physics { get; set; }
        //public virtual Int32 TermI_Star_Physics { get; set; }

        //public virtual Int32 TermI_HW_Physics { get; set; }
        //public virtual Int32 TermI_FA_Physics { get; set; }
        //public virtual Int32 TermI_AS_Physics { get; set; }
        //public virtual Int32 TermI_OSP_Physics { get; set; }

        //public virtual string TermI_HW_Grade_Physics { get; set; }
        //public virtual string TermI_FA_Grade_Physics { get; set; }
        //public virtual string TermI_AS_Grade_Physics { get; set; }
        //public virtual string TermI_OSP_Grade_Physics { get; set; }


        //public virtual Int32 TermII_Physics { get; set; }
        //public virtual Int32 TermII_Star_Physics { get; set; }

        //public virtual Int32 TermII_HW_Physics { get; set; }
        //public virtual Int32 TermII_FA_Physics { get; set; }
        //public virtual Int32 TermII_AS_Physics { get; set; }
        //public virtual Int32 TermII_OSP_Physics { get; set; }

        //public virtual string TermII_HW_Grade_Physics { get; set; }
        //public virtual string TermII_FA_Grade_Physics { get; set; }
        //public virtual string TermII_AS_Grade_Physics { get; set; }
        //public virtual string TermII_OSP_Grade_Physics { get; set; }

        //public virtual Int32 TermIII_Physics { get; set; }
        //public virtual Int32 TermIII_Star_Physics { get; set; }

        //public virtual Int32 TermIII_HW_Physics { get; set; }
        //public virtual Int32 TermIII_FA_Physics { get; set; }
        //public virtual Int32 TermIII_AS_Physics { get; set; }
        //public virtual Int32 TermIII_OSP_Physics { get; set; }

        //public virtual string TermIII_HW_Grade_Physics { get; set; }
        //public virtual string TermIII_FA_Grade_Physics { get; set; }
        //public virtual string TermIII_AS_Grade_Physics { get; set; }
        //public virtual string TermIII_OSP_Grade_Physics { get; set; }


        ///// <summary>
        ///// For Chemistry Language
        ///// </summary>
        //public virtual Int32 TermI_Chemistry { get; set; }
        //public virtual Int32 TermI_Star_Chemistry { get; set; }

        //public virtual Int32 TermI_HW_Chemistry { get; set; }
        //public virtual Int32 TermI_FA_Chemistry { get; set; }
        //public virtual Int32 TermI_AS_Chemistry { get; set; }
        //public virtual Int32 TermI_OSP_Chemistry { get; set; }

        //public virtual string TermI_HW_Grade_Chemistry { get; set; }
        //public virtual string TermI_FA_Grade_Chemistry { get; set; }
        //public virtual string TermI_AS_Grade_Chemistry { get; set; }
        //public virtual string TermI_OSP_Grade_Chemistry { get; set; }

        //public virtual Int32 TermII_Chemistry { get; set; }
        //public virtual Int32 TermII_Star_Chemistry { get; set; }

        //public virtual Int32 TermII_HW_Chemistry { get; set; }
        //public virtual Int32 TermII_FA_Chemistry { get; set; }
        //public virtual Int32 TermII_AS_Chemistry { get; set; }
        //public virtual Int32 TermII_OSP_Chemistry { get; set; }

        //public virtual string TermII_HW_Grade_Chemistry { get; set; }
        //public virtual string TermII_FA_Grade_Chemistry { get; set; }
        //public virtual string TermII_AS_Grade_Chemistry { get; set; }
        //public virtual string TermII_OSP_Grade_Chemistry { get; set; }

        //public virtual Int32 TermIII_Chemistry { get; set; }
        //public virtual Int32 TermIII_Star_Chemistry { get; set; }

        //public virtual Int32 TermIII_HW_Chemistry { get; set; }
        //public virtual Int32 TermIII_FA_Chemistry { get; set; }
        //public virtual Int32 TermIII_AS_Chemistry { get; set; }
        //public virtual Int32 TermIII_OSP_Chemistry { get; set; }

        //public virtual string TermIII_HW_Grade_Chemistry { get; set; }
        //public virtual string TermIII_FA_Grade_Chemistry { get; set; }
        //public virtual string TermIII_AS_Grade_Chemistry { get; set; }
        //public virtual string TermIII_OSP_Grade_Chemistry { get; set; }


        ///// <summary>
        ///// For Biology Language
        ///// </summary>
        //public virtual Int32 TermI_Biology { get; set; }
        //public virtual Int32 TermI_Star_Biology { get; set; }

        //public virtual Int32 TermI_HW_Biology { get; set; }
        //public virtual Int32 TermI_FA_Biology { get; set; }
        //public virtual Int32 TermI_AS_Biology { get; set; }
        //public virtual Int32 TermI_OSP_Biology { get; set; }

        //public virtual string TermI_HW_Grade_Biology { get; set; }
        //public virtual string TermI_FA_Grade_Biology { get; set; }
        //public virtual string TermI_AS_Grade_Biology { get; set; }
        //public virtual string TermI_OSP_Grade_Biology { get; set; }

        //public virtual Int32 TermII_Biology { get; set; }
        //public virtual Int32 TermII_Star_Biology { get; set; }

        //public virtual Int32 TermII_HW_Biology { get; set; }
        //public virtual Int32 TermII_FA_Biology { get; set; }
        //public virtual Int32 TermII_AS_Biology { get; set; }
        //public virtual Int32 TermII_OSP_Biology { get; set; }

        //public virtual string TermII_HW_Grade_Biology { get; set; }
        //public virtual string TermII_FA_Grade_Biology { get; set; }
        //public virtual string TermII_AS_Grade_Biology { get; set; }
        //public virtual string TermII_OSP_Grade_Biology { get; set; }

        //public virtual Int32 TermIII_Biology { get; set; }
        //public virtual Int32 TermIII_Star_Biology { get; set; }

        //public virtual Int32 TermIII_HW_Biology { get; set; }
        //public virtual Int32 TermIII_FA_Biology { get; set; }
        //public virtual Int32 TermIII_AS_Biology { get; set; }
        //public virtual Int32 TermIII_OSP_Biology { get; set; }

        //public virtual string TermIII_HW_Grade_Biology { get; set; }
        //public virtual string TermIII_FA_Grade_Biology { get; set; }
        //public virtual string TermIII_AS_Grade_Biology { get; set; }
        //public virtual string TermIII_OSP_Grade_Biology { get; set; }

        ///// <summary>
        ///// For Economics Language
        ///// </summary>
        //public virtual Int32 TermI_Economics { get; set; }
        //public virtual Int32 TermI_Star_Economics { get; set; }

        //public virtual Int32 TermI_HW_Economics { get; set; }
        //public virtual Int32 TermI_FA_Economics { get; set; }
        //public virtual Int32 TermI_AS_Economics { get; set; }
        //public virtual Int32 TermI_OSP_Economics { get; set; }

        //public virtual string TermI_HW_Grade_Economics { get; set; }
        //public virtual string TermI_FA_Grade_Economics { get; set; }
        //public virtual string TermI_AS_Grade_Economics { get; set; }
        //public virtual string TermI_OSP_Grade_Economics { get; set; }

        //public virtual Int32 TermII_Economics { get; set; }
        //public virtual Int32 TermII_Star_Economics { get; set; }

        //public virtual Int32 TermII_HW_Economics { get; set; }
        //public virtual Int32 TermII_FA_Economics { get; set; }
        //public virtual Int32 TermII_AS_Economics { get; set; }
        //public virtual Int32 TermII_OSP_Economics { get; set; }

        //public virtual string TermII_HW_Grade_Economics { get; set; }
        //public virtual string TermII_FA_Grade_Economics { get; set; }
        //public virtual string TermII_AS_Grade_Economics { get; set; }
        //public virtual string TermII_OSP_Grade_Economics { get; set; }

        //public virtual Int32 TermIII_Economics { get; set; }
        //public virtual Int32 TermIII_Star_Economics { get; set; }

        //public virtual Int32 TermIII_HW_Economics { get; set; }
        //public virtual Int32 TermIII_FA_Economics { get; set; }
        //public virtual Int32 TermIII_AS_Economics { get; set; }
        //public virtual Int32 TermIII_OSP_Economics { get; set; }

        //public virtual string TermIII_HW_Grade_Economics { get; set; }
        //public virtual string TermIII_FA_Grade_Economics { get; set; }
        //public virtual string TermIII_AS_Grade_Economics { get; set; }
        //public virtual string TermIII_OSP_Grade_Economics { get; set; }

        ///// <summary>
        ///// For Physical Education Language
        ///// </summary>
        //public virtual Int32 TermI_PhysicalEducation { get; set; }
        //public virtual Int32 TermI_Star_PhysicalEducation { get; set; }

        //public virtual Int32 TermI_HW_PhysicalEducation { get; set; }
        //public virtual Int32 TermI_FA_PhysicalEducation { get; set; }
        //public virtual Int32 TermI_AS_PhysicalEducation { get; set; }
        //public virtual Int32 TermI_OSP_PhysicalEducation { get; set; }

        //public virtual string TermI_HW_Grade_PhysicalEducation { get; set; }
        //public virtual string TermI_FA_Grade_PhysicalEducation { get; set; }
        //public virtual string TermI_AS_Grade_PhysicalEducation { get; set; }
        //public virtual string TermI_OSP_Grade_PhysicalEducation { get; set; }

        //public virtual Int32 TermII_PhysicalEducation { get; set; }
        //public virtual Int32 TermII_Star_PhysicalEducation { get; set; }

        //public virtual Int32 TermII_HW_PhysicalEducation { get; set; }
        //public virtual Int32 TermII_FA_PhysicalEducation { get; set; }
        //public virtual Int32 TermII_AS_PhysicalEducation { get; set; }
        //public virtual Int32 TermII_OSP_PhysicalEducation { get; set; }

        //public virtual string TermII_HW_Grade_PhysicalEducation { get; set; }
        //public virtual string TermII_FA_Grade_PhysicalEducation { get; set; }
        //public virtual string TermII_AS_Grade_PhysicalEducation { get; set; }
        //public virtual string TermII_OSP_Grade_PhysicalEducation { get; set; }


        //public virtual Int32 TermIII_PhysicalEducation { get; set; }
        //public virtual Int32 TermIII_Star_PhysicalEducation { get; set; }

        //public virtual Int32 TermIII_HW_PhysicalEducation { get; set; }
        //public virtual Int32 TermIII_FA_PhysicalEducation { get; set; }
        //public virtual Int32 TermIII_AS_PhysicalEducation { get; set; }
        //public virtual Int32 TermIII_OSP_PhysicalEducation { get; set; }

        //public virtual string TermIII_HW_Grade_PhysicalEducation { get; set; }
        //public virtual string TermIII_FA_Grade_PhysicalEducation { get; set; }
        //public virtual string TermIII_AS_Grade_PhysicalEducation { get; set; }
        //public virtual string TermIII_OSP_Grade_PhysicalEducation { get; set; }
        ///// <summary>
        ///// For Information and communication technology Language
        ///// </summary>
        //public virtual Int32 TermI_InfoAndComTech { get; set; }
        //public virtual Int32 TermI_Star_InfoAndComTech { get; set; }

        //public virtual Int32 TermI_HW_InfoAndComTech { get; set; }
        //public virtual Int32 TermI_FA_InfoAndComTech { get; set; }
        //public virtual Int32 TermI_AS_InfoAndComTech { get; set; }
        //public virtual Int32 TermI_OSP_InfoAndComTech { get; set; }

        //public virtual string TermI_HW_Grade_InfoAndComTech { get; set; }
        //public virtual string TermI_FA_Grade_InfoAndComTech { get; set; }
        //public virtual string TermI_AS_Grade_InfoAndComTech { get; set; }
        //public virtual string TermI_OSP_Grade_InfoAndComTech { get; set; }

        //public virtual Int32 TermII_InfoAndComTech { get; set; }
        //public virtual Int32 TermII_Star_InfoAndComTech { get; set; }

        //public virtual Int32 TermII_HW_InfoAndComTech { get; set; }
        //public virtual Int32 TermII_FA_InfoAndComTech { get; set; }
        //public virtual Int32 TermII_AS_InfoAndComTech { get; set; }
        //public virtual Int32 TermII_OSP_InfoAndComTech { get; set; }

        //public virtual string TermII_HW_Grade_InfoAndComTech { get; set; }
        //public virtual string TermII_FA_Grade_InfoAndComTech { get; set; }
        //public virtual string TermII_AS_Grade_InfoAndComTech { get; set; }
        //public virtual string TermII_OSP_Grade_InfoAndComTech { get; set; }


        //public virtual Int32 TermIII_InfoAndComTech { get; set; }
        //public virtual Int32 TermIII_Star_InfoAndComTech { get; set; }

        //public virtual Int32 TermIII_HW_InfoAndComTech { get; set; }
        //public virtual Int32 TermIII_FA_InfoAndComTech { get; set; }
        //public virtual Int32 TermIII_AS_InfoAndComTech { get; set; }
        //public virtual Int32 TermIII_OSP_InfoAndComTech { get; set; }

        //public virtual string TermIII_HW_Grade_InfoAndComTech { get; set; }
        //public virtual string TermIII_FA_Grade_InfoAndComTech { get; set; }
        //public virtual string TermIII_AS_Grade_InfoAndComTech { get; set; }
        //public virtual string TermIII_OSP_Grade_InfoAndComTech { get; set; }

        ///// <summary>
        ///// For Stem Lab Language
        ///// </summary>
        //public virtual Int32 TermI_StemLab { get; set; }
        //public virtual Int32 TermI_Star_StemLab { get; set; }

        //public virtual Int32 TermI_HW_StemLab { get; set; }
        //public virtual Int32 TermI_FA_StemLab { get; set; }
        //public virtual Int32 TermI_AS_StemLab { get; set; }
        //public virtual Int32 TermI_OSP_StemLab { get; set; }

        //public virtual string TermI_Grade_HW_StemLab { get; set; }
        //public virtual string TermI_Grade_FA_StemLab { get; set; }
        //public virtual string TermI_Grade_AS_StemLab { get; set; }
        //public virtual string TermI_OSP_Grade_StemLab { get; set; }


        //public virtual Int32 TermII_StemLab { get; set; }
        //public virtual Int32 TermII_Star_StemLab { get; set; }

        //public virtual Int32 TermII_HW_StemLab { get; set; }
        //public virtual Int32 TermII_FA_StemLab { get; set; }
        //public virtual Int32 TermII_AS_StemLab { get; set; }
        //public virtual Int32 TermII_OSP_StemLab { get; set; }

        //public virtual string TermII_HW_Grade_StemLab { get; set; }
        //public virtual string TermII_FA_Grade_StemLab { get; set; }
        //public virtual string TermII_AS_Grade_StemLab { get; set; }
        //public virtual string TermII_OSP_Grade_StemLab { get; set; }


        //public virtual Int32 TermIII_StemLab { get; set; }
        //public virtual Int32 TermIII_Star_StemLab { get; set; }

        //public virtual Int32 TermIII_HW_StemLab { get; set; }
        //public virtual Int32 TermIII_FA_StemLab { get; set; }
        //public virtual Int32 TermIII_AS_StemLab { get; set; }
        //public virtual Int32 TermIII_OSP_StemLab { get; set; }

        //public virtual string TermIII_HW_Grade_StemLab { get; set; }
        //public virtual string TermIII_FA_Grade_StemLab { get; set; }
        //public virtual string TermIII_AS_Grade_StemLab { get; set; }
        //public virtual string TermIII_OSP_Grade_StemLab { get; set; }

        ///// <summary>
        ///// For Spark Lab Language
        ///// </summary>
        //public virtual Int32 TermI_SparkLab { get; set; }
        //public virtual Int32 TermI_Star_SparkLab { get; set; }

        //public virtual Int32 TermI_HW_SparkLab { get; set; }
        //public virtual Int32 TermI_FA_SparkLab { get; set; }
        //public virtual Int32 TermI_AS_SparkLab { get; set; }
        //public virtual Int32 TermI_OSP_SparkLab { get; set; }

        //public virtual string TermI_HW_Grade_SparkLab { get; set; }
        //public virtual string TermI_FA_Grade_SparkLab { get; set; }
        //public virtual string TermI_AS_Grade_SparkLab { get; set; }
        //public virtual string TermI_OSP_Grade_SparkLab { get; set; }

        //public virtual Int32 TermII_SparkLab { get; set; }
        //public virtual Int32 TermII_Star_SparkLab { get; set; }

        //public virtual Int32 TermII_HW_SparkLab { get; set; }
        //public virtual Int32 TermII_FA_SparkLab { get; set; }
        //public virtual Int32 TermII_AS_SparkLab { get; set; }
        //public virtual Int32 TermII_OSP_SparkLab { get; set; }

        //public virtual string TermII_HW_Grade_SparkLab { get; set; }
        //public virtual string TermII_FA_Grade_SparkLab { get; set; }
        //public virtual string TermII_AS_Grade_SparkLab { get; set; }
        //public virtual string TermII_OSP_Grade_SparkLab { get; set; }

        //public virtual Int32 TermIII_SparkLab { get; set; }
        //public virtual Int32 TermIII_Star_SparkLab { get; set; }

        //public virtual Int32 TermIII_HW_SparkLab { get; set; }
        //public virtual Int32 TermIII_FA_SparkLab { get; set; }
        //public virtual Int32 TermIII_AS_SparkLab { get; set; }
        //public virtual Int32 TermIII_OSP_SparkLab { get; set; }

        //public virtual string TermIII_HW_Grade_SparkLab { get; set; }
        //public virtual string TermIII_FA_Grade_SparkLab { get; set; }
        //public virtual string TermIII_AS_Grade_SparkLab { get; set; }
        //public virtual string TermIII_OSP_Grade_SparkLab { get; set; }

        ///// <summary>
        ///// For Fine Arts Language
        ///// </summary>
        //public virtual Int32 TermI_FineArts { get; set; }
        //public virtual Int32 TermI_Star_FineArts { get; set; }

        //public virtual Int32 TermI_HW_FineArts { get; set; }
        //public virtual Int32 TermI_FA_FineArts { get; set; }
        //public virtual Int32 TermI_AS_FineArts { get; set; }
        //public virtual Int32 TermI_OSP_FineArts { get; set; }

        //public virtual string TermI_HW_Grade_FineArts { get; set; }
        //public virtual string TermI_FA_Grade_FineArts { get; set; }
        //public virtual string TermI_AS_Grade_FineArts { get; set; }
        //public virtual string TermI_OSP_Grade_FineArts { get; set; }

        //public virtual Int32 TermII_FineArts { get; set; }
        //public virtual Int32 TermII_Star_FineArts { get; set; }

        //public virtual Int32 TermII_HW_FineArts { get; set; }
        //public virtual Int32 TermII_FA_FineArts { get; set; }
        //public virtual Int32 TermII_AS_FineArts { get; set; }
        //public virtual Int32 TermII_OSP_FineArts { get; set; }

        //public virtual string TermII_HW_Grade_FineArts { get; set; }
        //public virtual string TermII_FA_Grade_FineArts { get; set; }
        //public virtual string TermII_AS_Grade_FineArts { get; set; }
        //public virtual string TermII_OSP_Grade_FineArts { get; set; }

        //public virtual Int32 TermIII_FineArts { get; set; }
        //public virtual Int32 TermIII_Star_FineArts { get; set; }

        //public virtual Int32 TermIII_HW_FineArts { get; set; }
        //public virtual Int32 TermIII_FA_FineArts { get; set; }
        //public virtual Int32 TermIII_AS_FineArts { get; set; }
        //public virtual Int32 TermIII_OSP_FineArts { get; set; }

        //public virtual string TermIII_HW_Grade_FineArts { get; set; }
        //public virtual string TermIII_FA_Grade_FineArts { get; set; }
        //public virtual string TermIII_AS_Grade_FineArts { get; set; }
        //public virtual string TermIII_OSP_Grade_FineArts { get; set; }

        ///// <summary>
        ///// For Sports And Games Language
        ///// </summary>
        //public virtual Int32 TermI_SprotsAndGames { get; set; }
        //public virtual Int32 TermI_Star_SprotsAndGames { get; set; }

        //public virtual Int32 TermI_HW_SprotsAndGames { get; set; }
        //public virtual Int32 TermI_FA_SprotsAndGames { get; set; }
        //public virtual Int32 TermI_AS_SprotsAndGames { get; set; }
        //public virtual Int32 TermI_OSP_SprotsAndGames { get; set; }

        //public virtual string TermI_HW_Grade_SprotsAndGames { get; set; }
        //public virtual string TermI_FA_Grade_SprotsAndGames { get; set; }
        //public virtual string TermI_AS_Grade_SprotsAndGames { get; set; }
        //public virtual string TermI_OSP_Grade_SprotsAndGames { get; set; }

        //public virtual Int32 TermII_SprotsAndGames { get; set; }
        //public virtual Int32 TermII_Star_SprotsAndGames { get; set; }

        //public virtual Int32 TermII_HW_SprotsAndGames { get; set; }
        //public virtual Int32 TermII_FA_SprotsAndGames { get; set; }
        //public virtual Int32 TermII_AS_SprotsAndGames { get; set; }
        //public virtual Int32 TermII_OSP_SprotsAndGames { get; set; }

        //public virtual string TermII_HW_Grade_SprotsAndGames { get; set; }
        //public virtual string TermII_FA_Grade_SprotsAndGames { get; set; }
        //public virtual string TermII_AS_Grade_SprotsAndGames { get; set; }
        //public virtual string TermII_OSP_Grade_SprotsAndGames { get; set; }

        //public virtual Int32 TermIII_SprotsAndGames { get; set; }
        //public virtual Int32 TermIII_Star_SprotsAndGames { get; set; }

        //public virtual Int32 TermIII_HW_SprotsAndGames { get; set; }
        //public virtual Int32 TermIII_FA_SprotsAndGames { get; set; }
        //public virtual Int32 TermIII_AS_SprotsAndGames { get; set; }
        //public virtual Int32 TermIII_OSP_SprotsAndGames { get; set; }

        //public virtual string TermIII_HW_Grade_SprotsAndGames { get; set; }
        //public virtual string TermIII_FA_Grade_SprotsAndGames { get; set; }
        //public virtual string TermIII_AS_Grade_SprotsAndGames { get; set; }
        //public virtual string TermIII_OSP_Grade_SprotsAndGames { get; set; }
        
    }
}
