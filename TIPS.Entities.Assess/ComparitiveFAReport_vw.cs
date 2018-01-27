using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TIPS.Entities.Assess
{
    [DataContract]
    public class ComparitiveFAReport_vw
    {
        [DataMember]
        public virtual long Id { get; set; }
        [DataMember]
        public virtual long AssessId { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string IdNo { get; set; }
        [DataMember]
        public virtual string Campus { get; set; }
        [DataMember]
        public virtual string Section { get; set; }
        [DataMember]
        public virtual string Grade { get; set; }
        [DataMember]
        public virtual string AcademicYear { get; set; }

        public virtual string BioJan { get; set; }
        public virtual string BioFeb { get; set; }
        public virtual string BioMar { get; set; }
        public virtual string BioApr { get; set; }
        public virtual string BioMay { get; set; }
        public virtual string BioJun { get; set; }
        public virtual string BioJly { get; set; }
        public virtual string BioAug { get; set; }
        public virtual string BioSep { get; set; }
        public virtual string BioOct { get; set; }
        public virtual string BioNov { get; set; }
        public virtual string BioDec { get; set; }
        public virtual string ChemJan { get; set; }
        public virtual string ChemFeb { get; set; }
        public virtual string ChemMar { get; set; }
        public virtual string ChemApr { get; set; }
        public virtual string ChemMay { get; set; }
        public virtual string ChemJun { get; set; }
        public virtual string ChemJly { get; set; }
        public virtual string ChemAug { get; set; }
        public virtual string ChemSep { get; set; }
        public virtual string ChemOct { get; set; }
        public virtual string ChemNov { get; set; }
        public virtual string ChemDec { get; set; }
        public virtual string ComSciJan { get; set; }
        public virtual string ComSciFeb { get; set; }
        public virtual string ComSciMar { get; set; }
        public virtual string ComSciApr { get; set; }
        public virtual string ComSciMay { get; set; }
        public virtual string ComSciJun { get; set; }
        public virtual string ComSciJly { get; set; }
        public virtual string ComSciAug { get; set; }
        public virtual string ComSciSep { get; set; }
        public virtual string ComSciOct { get; set; }
        public virtual string ComSciNov { get; set; }
        public virtual string ComSciDec { get; set; }
        public virtual string EcoJan { get; set; }
        public virtual string EcoFeb { get; set; }
        public virtual string EcoMar { get; set; }
        public virtual string EcoApr { get; set; }
        public virtual string EcoMay { get; set; }
        public virtual string EcoJun { get; set; }
        public virtual string EcoJly { get; set; }
        public virtual string EcoAug { get; set; }
        public virtual string EcoSep { get; set; }
        public virtual string EcoOct { get; set; }
        public virtual string EcoNov { get; set; }
        public virtual string EcoDec { get; set; }
        public virtual string EngJan { get; set; }
        public virtual string EngFeb { get; set; }
        public virtual string EngMar { get; set; }
        public virtual string EngApr { get; set; }
        public virtual string EngMay { get; set; }
        public virtual string EngJun { get; set; }
        public virtual string EngJly { get; set; }
        public virtual string EngAug { get; set; }
        public virtual string EngSep { get; set; }
        public virtual string EngOct { get; set; }
        public virtual string EngNov { get; set; }
        public virtual string EngDec { get; set; }
        public virtual string FreJan { get; set; }
        public virtual string FreFeb { get; set; }
        public virtual string FreMar { get; set; }
        public virtual string FreApr { get; set; }
        public virtual string FreMay { get; set; }
        public virtual string FreJun { get; set; }
        public virtual string FreJly { get; set; }
        public virtual string FreAug { get; set; }
        public virtual string FreSep { get; set; }
        public virtual string FreOct { get; set; }
        public virtual string FreNov { get; set; }
        public virtual string FreDec { get; set; }
        public virtual string HinJan { get; set; }
        public virtual string HinFeb { get; set; }
        public virtual string HinMar { get; set; }
        public virtual string HinApr { get; set; }
        public virtual string HinMay { get; set; }
        public virtual string HinJun { get; set; }
        public virtual string HinJly { get; set; }
        public virtual string HinAug { get; set; }
        public virtual string HinSep { get; set; }
        public virtual string HinOct { get; set; }
        public virtual string HinNov { get; set; }
        public virtual string HinDec { get; set; }
        public virtual string HisGeoJan { get; set; }
        public virtual string HisGeoFeb { get; set; }
        public virtual string HisGeoMar { get; set; }
        public virtual string HisGeoApr { get; set; }
        public virtual string HisGeoMay { get; set; }
        public virtual string HisGeoJun { get; set; }
        public virtual string HisGeoJly { get; set; }
        public virtual string HisGeoAug { get; set; }
        public virtual string HisGeoSep { get; set; }
        public virtual string HisGeoOct { get; set; }
        public virtual string HisGeoNov { get; set; }
        public virtual string HisGeoDec { get; set; }
        public virtual string IctJan { get; set; }
        public virtual string IctFeb { get; set; }
        public virtual string IctMar { get; set; }
        public virtual string IctApr { get; set; }
        public virtual string IctMay { get; set; }
        public virtual string IctJun { get; set; }
        public virtual string IctJly { get; set; }
        public virtual string IctAug { get; set; }
        public virtual string IctSep { get; set; }
        public virtual string IctOct { get; set; }
        public virtual string IctNov { get; set; }
        public virtual string IctDec { get; set; }
        public virtual string MatJan { get; set; }
        public virtual string MatFeb { get; set; }
        public virtual string MatMar { get; set; }
        public virtual string MatApr { get; set; }
        public virtual string MatMay { get; set; }
        public virtual string MatJun { get; set; }
        public virtual string MatJly { get; set; }
        public virtual string MatAug { get; set; }
        public virtual string MatSep { get; set; }
        public virtual string MatOct { get; set; }
        public virtual string MatNov { get; set; }
        public virtual string MatDec { get; set; }
        public virtual string PhyEduJan { get; set; }
        public virtual string PhyEduFeb { get; set; }
        public virtual string PhyEduMar { get; set; }
        public virtual string PhyEduApr { get; set; }
        public virtual string PhyEduMay { get; set; }
        public virtual string PhyEduJun { get; set; }
        public virtual string PhyEduJly { get; set; }
        public virtual string PhyEduAug { get; set; }
        public virtual string PhyEduSep { get; set; }
        public virtual string PhyEduOct { get; set; }
        public virtual string PhyEduNov { get; set; }
        public virtual string PhyEduDec { get; set; }
        public virtual string PhyJan { get; set; }
        public virtual string PhyFeb { get; set; }
        public virtual string PhyMar { get; set; }
        public virtual string PhyApr { get; set; }
        public virtual string PhyMay { get; set; }
        public virtual string PhyJun { get; set; }
        public virtual string PhyJly { get; set; }
        public virtual string PhyAug { get; set; }
        public virtual string PhySep { get; set; }
        public virtual string PhyOct { get; set; }
        public virtual string PhyNov { get; set; }
        public virtual string PhyDec { get; set; }
    }
}
