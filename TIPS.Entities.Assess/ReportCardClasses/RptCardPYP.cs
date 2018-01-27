using System.Collections.Generic;
using System.Text;
using System;
namespace TIPS.Entities.Assess.ReportCardClasses
{
    public class RptCardPYP
    {
        public virtual long Id { get; set; }
        public virtual string IdNo { get; set; }
        public virtual string RequestNo { get; set; }
        public virtual string Name { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Section { get; set; }
        public virtual string Grade { get; set; }
        public virtual long StudentId { get; set; }
        public virtual long PreRegNum { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual long Semester { get; set; }
        public virtual string TeacherName { get; set; }
        public virtual string RptCardStatus { get; set; }
        public virtual string Student2ndLanguge { get; set; }
        public virtual DateTime? RptDate { get; set; }
        public virtual bool? Eng_Rdg_Cnfdnc_Acmpl { get; set; }
        public virtual bool? Eng_Rdg_Cnfdnc_Procs { get; set; }
        public virtual bool? Eng_Rdg_Cnfdnc_NdsEncgmnt { get; set; }
        public virtual bool? Eng_Rdg_Flncy_Acmpl { get; set; }
        public virtual bool? Eng_Rdg_Flncy_Procs { get; set; }
        public virtual bool? Eng_Rdg_Flncy_NdsEncgmnt { get; set; }
        public virtual bool? Eng_Rdg_Cmpsn_Acmpl { get; set; }
        public virtual bool? Eng_Rdg_Cmpsn_Procs { get; set; }
        public virtual bool? Eng_Rdg_Cmpsn_NdsEncgmnt { get; set; }
        public virtual bool? Eng_Wrtng_Intrcn_Acmpl { get; set; }
        public virtual bool? Eng_Wrtng_Intrcn_Procs { get; set; }
        public virtual bool? Eng_Wrtng_Intrcn_NEngmt { get; set; }
        public virtual bool? Eng_Wrtng_OrgIds_Acmpl { get; set; }
        public virtual bool? Eng_Wrtng_OrgIds_Procs { get; set; }
        public virtual bool? Eng_Wrtng_OrgIds_NEngmt { get; set; }
        public virtual bool? Eng_Wrtng_InSuprtDtls_Acmpl { get; set; }
        public virtual bool? Eng_Wrtng_InSuprtDtls_Procs { get; set; }
        public virtual bool? Eng_Wrtng_InSuprtDtls_NEngmt { get; set; }
        public virtual bool? Eng_Wrtng_Cnlsn_Acmpl { get; set; }
        public virtual bool? Eng_Wrtng_Cnlsn_Procs { get; set; }
        public virtual bool? Eng_Wrtng_Cnlsn_NEngmt { get; set; }
        public virtual bool? Eng_LSNFcn_Tpcs_Acmpl { get; set; }
        public virtual bool? Eng_LSNFcn_Tpcs_Procs { get; set; }
        public virtual bool? Eng_LSNFcn_Tpcs_NEngmt { get; set; }
        public virtual bool? Eng_LSNFcn_MIdsDtls_Acmpl { get; set; }
        public virtual bool? Eng_LSNFcn_MIdsDtls_Procs { get; set; }
        public virtual bool? Eng_LSNFcn_MIdsDtls_NEngmt { get; set; }
        public virtual bool? Eng_LSNFcn_Orgn_Acmpl { get; set; }
        public virtual bool? Eng_LSNFcn_Orgn_Procs { get; set; }
        public virtual bool? Eng_LSNFcn_Orgn_NEngmt { get; set; }
        public virtual bool? Eng_LSNFcn_CVocblry_Acmpl { get; set; }
        public virtual bool? Eng_LSNFcn_CVocblry_Procs { get; set; }
        public virtual bool? Eng_LSNFcn_CVocblry_NEngmt { get; set; }
        public virtual bool? Eng_LSNFcn_Acrcy_Acmpl { get; set; }
        public virtual bool? Eng_LSNFcn_Acrcy_Procs { get; set; }
        public virtual bool? Eng_LSNFcn_Acrcy_NEngmt { get; set; }
        public virtual bool? Eng_LSFcn_Bgn_Acmpl { get; set; }
        public virtual bool? Eng_LSFcn_Bgn_Procs { get; set; }
        public virtual bool? Eng_LSFcn_Bgn_NdsEncgmnt { get; set; }
        public virtual bool? Eng_LSFcn_Stg_Acmpl { get; set; }
        public virtual bool? Eng_LSFcn_Stg_Procs { get; set; }
        public virtual bool? Eng_LSFcn_Stg_NdsEncgmnt { get; set; }
        public virtual bool? Eng_LSFcn_Crtr_Acmpl { get; set; }
        public virtual bool? Eng_LSFcn_Crtr_Procs { get; set; }
        public virtual bool? Eng_LSFcn_Crtr_NdsEncgmnt { get; set; }
        public virtual bool? Eng_LSFcn_Prblm_Acmpl { get; set; }
        public virtual bool? Eng_LSFcn_Prblm_Procs { get; set; }
        public virtual bool? Eng_LSFcn_Prblm_NEngmt { get; set; }
        public virtual bool? Eng_LSFcn_Seq_Acmpl { get; set; }
        public virtual bool? Eng_LSFcn_Seq_Procs { get; set; }
        public virtual bool? Eng_LSFcn_Seq_NdsEncgmnt { get; set; }
        public virtual bool? Eng_LSFcn_Rsln_Acmpl { get; set; }
        public virtual bool? Eng_LSFcn_Rsln_Procs { get; set; }
        public virtual bool? Eng_LSFcn_Rsln_NdsEncgmnt { get; set; }
        public virtual bool? Hnd_Rdg_Cnfdnc_Acmpl { get; set; }
        public virtual bool? Hnd_Rdg_Cnfdnc_Procs { get; set; }
        public virtual bool? Hnd_Rdg_Cnfdnc_NdsEncgmnt { get; set; }
        public virtual bool? Hnd_Rdg_Flncy_Acmpl { get; set; }
        public virtual bool? Hnd_Rdg_Flncy_Procs { get; set; }
        public virtual bool? Hnd_Rdg_Flncy_NdsEncgmnt { get; set; }
        public virtual bool? Hnd_Rdg_TonInTon_Acmpl { get; set; }
        public virtual bool? Hnd_Rdg_TonInTon_Procs { get; set; }
        public virtual bool? Hnd_Rdg_TonInTon_NEngmt { get; set; }
        public virtual bool? Hnd_Rdg_Pronctn_Acmpl { get; set; }
        public virtual bool? Hnd_Rdg_Pronctn_Procs { get; set; }
        public virtual bool? Hnd_Rdg_Pronctn_NEngmt { get; set; }
        public virtual bool? Hnd_Rdg_udstnd_Acmpl { get; set; }
        public virtual bool? Hnd_Rdg_udstnd_Procs { get; set; }
        public virtual bool? Hnd_Rdg_udstnd_NdsEncgmnt { get; set; }
        public virtual bool? Hnd_Wrtng_Splng_Acmpl { get; set; }
        public virtual bool? Hnd_Wrtng_Splng_Procs { get; set; }
        public virtual bool? Hnd_Wrtng_Splng_NEngmt { get; set; }
        public virtual bool? Hnd_Wrtng_Vocblry_Acmpl { get; set; }
        public virtual bool? Hnd_Wrtng_Vocblry_Procs { get; set; }
        public virtual bool? Hnd_Wrtng_Vocblry_NEngmt { get; set; }
        public virtual bool? Hnd_Wrtng_Punctn_Acmpl { get; set; }
        public virtual bool? Hnd_Wrtng_Punctn_Procs { get; set; }
        public virtual bool? Hnd_Wrtng_Punctn_NEngmt { get; set; }
        public virtual bool? Hnd_Wrtng_Grmr_Acmpl { get; set; }
        public virtual bool? Hnd_Wrtng_Grmr_Procs { get; set; }
        public virtual bool? Hnd_Wrtng_Grmr_NdsEncgmnt { get; set; }
        public virtual bool? Hnd_Wrtng_IdsCntnt_Acmpl { get; set; }
        public virtual bool? Hnd_Wrtng_IdsCntnt_Procs { get; set; }
        public virtual bool? Hnd_Wrtng_IdsCntnt_NEngmt { get; set; }
        public virtual bool? Hnd_Wrtng_NetHndwrt_Acmpl { get; set; }
        public virtual bool? Hnd_Wrtng_NetHndwrt_Procs { get; set; }
        public virtual bool? Hnd_Wrtng_NetHndwrt_NEngmt { get; set; }
        public virtual bool? Hnd_LS_Confdnc_Acmpl { get; set; }
        public virtual bool? Hnd_LS_Confdnc_Procs { get; set; }
        public virtual bool? Hnd_LS_Confdnc_NdsEncgmnt { get; set; }
        public virtual bool? Hnd_LS_Flncy_Acmpl { get; set; }
        public virtual bool? Hnd_LS_Flncy_Procs { get; set; }
        public virtual bool? Hnd_LS_Flncy_NdsEncgmnt { get; set; }
        public virtual bool? Hnd_LS_Pronctn_Acmpl { get; set; }
        public virtual bool? Hnd_LS_Pronctn_Procs { get; set; }
        public virtual bool? Hnd_LS_Pronctn_NdsEncgmnt { get; set; }
        public virtual bool? Hnd_LS_udstnd_Acmpl { get; set; }
        public virtual bool? Hnd_LS_udstnd_Procs { get; set; }
        public virtual bool? Hnd_LS_udstnd_NdsEncgmnt { get; set; }
        public virtual bool? Hnd_LS_Vocblry_Acmpl { get; set; }
        public virtual bool? Hnd_LS_Vocblry_Procs { get; set; }
        public virtual bool? Hnd_LS_Vocblry_NdsEncgmnt { get; set; }
        public virtual bool? Maths_Nmrs_B10VSys_Acmpl { get; set; }
        public virtual bool? Maths_Nmrs_B10VSys_Procs { get; set; }
        public virtual bool? Maths_Nmrs_B10VSys_NEngmt { get; set; }
        public virtual bool? Maths_Nmrs_SPIAAS_Acmpl { get; set; }
        public virtual bool? Maths_Nmrs_SPIAAS_Procs { get; set; }
        public virtual bool? Maths_Nmrs_SPIAAS_NEngmt { get; set; }
        public virtual bool? Maths_Nmrs_UESIAAS_Acmpl { get; set; }
        public virtual bool? Maths_Nmrs_UESIAAS_Procs { get; set; }
        public virtual bool? Maths_Nmrs_UESIAAS_NdsEncgmnt { get; set; }
        public virtual bool? Maths_SAS_IAD2A3S_Acmpl { get; set; }
        public virtual bool? Maths_SAS_IAD2A3S_Procs { get; set; }
        public virtual bool? Maths_SAS_IAD2A3S_NEngmt { get; set; }
        public virtual bool? Maths_SAS_UAILOSRARS_Acmpl { get; set; }
        public virtual bool? Maths_SAS_UAILOSRARS_Procs { get; set; }
        public virtual bool? Maths_SAS_UAILOSRARS_NEngmt { get; set; }
        public virtual bool? Maths_SAS_UTCASI2S_Acmpl { get; set; }
        public virtual bool? Maths_SAS_UTCASI2S_Procs { get; set; }
        public virtual bool? Maths_SAS_UTCASI2S_NEngmt { get; set; }
        public virtual bool? Maths_Msrmnt_EAMLPAA_Acmpl { get; set; }
        public virtual bool? Maths_Msrmnt_EAMLPAA_Procs { get; set; }
        public virtual bool? Maths_Msrmnt_EAMLPAA_NEngmt { get; set; }
        public virtual bool? Maths_Msrmnt_CTUOLMAC_Acmpl { get; set; }
        public virtual bool? Maths_Msrmnt_CTUOLMAC_Procs { get; set; }
        public virtual bool? Maths_Msrmnt_CTUOLMAC_NEngmt { get; set; }
        public virtual bool? Maths_Msrmnt_RAMAC_Acmpl { get; set; }
        public virtual bool? Maths_Msrmnt_RAMAC_Procs { get; set; }
        public virtual bool? Maths_Msrmnt_RAMAC_NEngmt { get; set; }
        public virtual bool? Maths_CmpHmAssgn_Acmpl { get; set; }
        public virtual bool? Maths_CmpHmAssgn_Procs { get; set; }
        public virtual bool? Maths_CmpHmAssgn_NEngmt { get; set; }
        public virtual bool? POI_THWEOS_LSAS_A { get; set; }
        public virtual bool? POI_THWEOS_LSAS_B { get; set; }
        public virtual bool? POI_THWEOS_LSAS_C { get; set; }
        public virtual bool? POI_THWEOS_LSAS_D { get; set; }
        public virtual bool? POI_THWEOS_RFTDOCS_A { get; set; }
        public virtual bool? POI_THWEOS_RFTDOCS_B { get; set; }
        public virtual bool? POI_THWEOS_RFTDOCS_C { get; set; }
        public virtual bool? POI_THWEOS_RFTDOCS_D { get; set; }
        public virtual bool? POI_THWEOS_SSOCATW_A { get; set; }
        public virtual bool? POI_THWEOS_SSOCATW_B { get; set; }
        public virtual bool? POI_THWEOS_SSOCATW_C { get; set; }
        public virtual bool? POI_THWEOS_SSOCATW_D { get; set; }
        public virtual string POI_THWEOS_Cmnts { get; set; }
        public virtual bool? POI_DOSkls_CSklPrsnt_Acmpl { get; set; }
        public virtual bool? POI_DOSkls_CSklPrsnt_Procs { get; set; }
        public virtual bool? POI_DOSkls_CSklPrsnt_NEngmt { get; set; }
        public virtual bool? POI_DOSkls_CSklNVComm_Acmpl { get; set; }
        public virtual bool? POI_DOSkls_CSklNVComm_Procs { get; set; }
        public virtual bool? POI_DOSkls_CSklNVComm_NEngmt { get; set; }
        public virtual bool? POI_DOSkls_TSklAppn_Acmpl { get; set; }
        public virtual bool? POI_DOSkls_TSklAppn_Procs { get; set; }
        public virtual bool? POI_DOSkls_TSklAppn_NEngmt { get; set; }
        public virtual bool? POI_DOSkls_TSSynth_Acmpl { get; set; }
        public virtual bool? POI_DOSkls_TSSynth_Procs { get; set; }
        public virtual bool? POI_DOSkls_TSSynth_NEngmt { get; set; }
        public virtual bool? POI_DOSkls_ICTSkl_Acmpl { get; set; }
        public virtual bool? POI_DOSkls_ICTSkl_Procs { get; set; }
        public virtual bool? POI_DOSkls_ICTSkl_NEngmt { get; set; }
        public virtual bool? POI_Attds_Cnfdnc_Acmpl { get; set; }
        public virtual bool? POI_Attds_Cnfdnc_Procs { get; set; }
        public virtual bool? POI_Attds_Cnfdnc_NEngmt { get; set; }
        public virtual bool? POI_Attds_Crtvty_Acmpl { get; set; }
        public virtual bool? POI_Attds_Crtvty_Procs { get; set; }
        public virtual bool? POI_Attds_Crtvty_NEngmt { get; set; }
        public virtual bool? POI_Attds_Indpndc_Acmpl { get; set; }
        public virtual bool? POI_Attds_Indpndc_Procs { get; set; }
        public virtual bool? POI_Attds_Indpndc_NEngmt { get; set; }
        public virtual bool? POI_DLP_Comm_Acmpl { get; set; }
        public virtual bool? POI_DLP_Comm_Procs { get; set; }
        public virtual bool? POI_DLP_Comm_NdsEncgmnt { get; set; }
        public virtual bool? POI_DLP_Thnkr_Acmpl { get; set; }
        public virtual bool? POI_DLP_Thnkr_Procs { get; set; }
        public virtual bool? POI_DLP_Thnkr_NdsEncgmnt { get; set; }
        public virtual bool? POI_THTWW_IOTDCOTE_A { get; set; }
        public virtual bool? POI_THTWW_IOTDCOTE_B { get; set; }
        public virtual bool? POI_THTWW_IOTDCOTE_C { get; set; }
        public virtual bool? POI_THTWW_IOTDCOTE_D { get; set; }
        public virtual bool? POI_THTWW_HTEHCAICTC_A { get; set; }
        public virtual bool? POI_THTWW_HTEHCAICTC_B { get; set; }
        public virtual bool? POI_THTWW_HTEHCAICTC_C { get; set; }
        public virtual bool? POI_THTWW_HTEHCAICTC_D { get; set; }
        public virtual bool? POI_THTWW_RFTCOTE_A { get; set; }
        public virtual bool? POI_THTWW_RFTCOTE_B { get; set; }
        public virtual bool? POI_THTWW_RFTCOTE_C { get; set; }
        public virtual bool? POI_THTWW_RFTCOTE_D { get; set; }
        public virtual bool? POI_THTWW_HRTTEC_A { get; set; }
        public virtual bool? POI_THTWW_HRTTEC_B { get; set; }
        public virtual bool? POI_THTWW_HRTTEC_C { get; set; }
        public virtual bool? POI_THTWW_HRTTEC_D { get; set; }
        public virtual string POI_THTWW_Cmnts { get; set; }
        public virtual bool? POI_THTWW_SMSklGMSkl_Acmpl { get; set; }
        public virtual bool? POI_THTWW_SMSklGMSkl_Procs { get; set; }
        public virtual bool? POI_THTWW_SMSklGMSkl_NEngmt { get; set; }
        public virtual bool? POI_THTWW_SMSklInfCho_Acmpl { get; set; }
        public virtual bool? POI_THTWW_SMSklInfCho_Procs { get; set; }
        public virtual bool? POI_THTWW_SMSklInfCho_NEngmt { get; set; }
        public virtual bool? POI_THTWW_RSklObsr_Acmpl { get; set; }
        public virtual bool? POI_THTWW_RSklObsr_Procs { get; set; }
        public virtual bool? POI_THTWW_RSklObsr_NEngmt { get; set; }
        public virtual bool? POI_THTWW_RSklRcd_Acmpl { get; set; }
        public virtual bool? POI_THTWW_RSklRcd_Procs { get; set; }
        public virtual bool? POI_THTWW_RSklRcd_NEngmt { get; set; }
        public virtual bool? POI_THTWW_RSklPrsnt_Acmpl { get; set; }
        public virtual bool? POI_THTWW_RSklPrsnt_Procs { get; set; }
        public virtual bool? POI_THTWW_RSklPrsnt_NEngmt { get; set; }
        public virtual bool? POI_THTWW_ICTSkl_Acmpl { get; set; }
        public virtual bool? POI_THTWW_ICTSkl_Procs { get; set; }
        public virtual bool? POI_THTWW_ICTSkl_NEngmt { get; set; }
        public virtual bool? POI_THTWW_AttCrtsy_Acmpl { get; set; }
        public virtual bool? POI_THTWW_AttCrtsy_Procs { get; set; }
        public virtual bool? POI_THTWW_AttCrtsy_NEngmt { get; set; }
        public virtual bool? POI_THTWW_AttTolnc_Acmpl { get; set; }
        public virtual bool? POI_THTWW_AttTolnc_Procs { get; set; }
        public virtual bool? POI_THTWW_AttTolnc_NEngmt { get; set; }
        public virtual bool? POI_THTWW_DLPRskTkr_Acmpl { get; set; }
        public virtual bool? POI_THTWW_DLPRskTkr_Procs { get; set; }
        public virtual bool? POI_THTWW_DLPRskTkr_NEngmt { get; set; }
        public virtual bool? POI_THTWW_DLPKnwdg_Acmpl { get; set; }
        public virtual bool? POI_THTWW_DLPKnwdg_Procs { get; set; }
        public virtual bool? POI_THTWW_DLPKnwdg_NEngmt { get; set; }
        public virtual bool? PhysEd_Bhvr_Acmpl { get; set; }
        public virtual bool? PhysEd_Bhvr_Procs { get; set; }
        public virtual bool? PhysEd_Bhvr_NdsEncgmnt { get; set; }
        public virtual bool? PhysEd_TmWrk_Acmpl { get; set; }
        public virtual bool? PhysEd_TmWrk_Procs { get; set; }
        public virtual bool? PhysEd_TmWrk_NdsEncgmnt { get; set; }
        public virtual bool? PhysEd_BscSkl_Acmpl { get; set; }
        public virtual bool? PhysEd_BscSkl_Procs { get; set; }
        public virtual bool? PhysEd_BscSkl_NdsEncgmnt { get; set; }
        public virtual bool? PhysEd_Ftns_Acmpl { get; set; }
        public virtual bool? PhysEd_Ftns_Procs { get; set; }
        public virtual bool? PhysEd_Ftns_NdsEncgmnt { get; set; }
        public virtual bool? PhysEd_Cmtmnt_Acmpl { get; set; }
        public virtual bool? PhysEd_Cmtmnt_Procs { get; set; }
        public virtual bool? PhysEd_Cmtmnt_NdsEncgmnt { get; set; }
        public virtual bool? PrfArt_Musc_PfrmSng_Acmpl { get; set; }
        public virtual bool? PrfArt_Musc_PfrmSng_Procs { get; set; }
        public virtual bool? PrfArt_Musc_PfrmSng_NEngmt { get; set; }
        public virtual bool? PrfArt_Musc_CrtCmp_Acmpl { get; set; }
        public virtual bool? PrfArt_Musc_CrtCmp_Procs { get; set; }
        public virtual bool? PrfArt_Musc_CrtCmp_NEngmt { get; set; }
        public virtual bool? PrfArt_Musc_LstnAprn_Acmpl { get; set; }
        public virtual bool? PrfArt_Musc_LstnAprn_Procs { get; set; }
        public virtual bool? PrfArt_Musc_LstnAprn_NEngmt { get; set; }
        public virtual bool? PrfArt_WDnc_SklTech_Acmpl { get; set; }
        public virtual bool? PrfArt_WDnc_SklTech_Procs { get; set; }
        public virtual bool? PrfArt_WDnc_SklTech_NEngmt { get; set; }
        public virtual bool? PrfArt_WDnc_PrbSlvng_Acmpl { get; set; }
        public virtual bool? PrfArt_WDnc_PrbSlvng_Procs { get; set; }
        public virtual bool? PrfArt_WDnc_PrbSlvng_NEngmt { get; set; }
        public virtual bool? PrfArt_WDnc_Clbrtn_Acmpl { get; set; }
        public virtual bool? PrfArt_WDnc_Clbrtn_Procs { get; set; }
        public virtual bool? PrfArt_WDnc_Clbrtn_NEngmt { get; set; }
        public virtual bool? PrfArt_ClsDnc_SklTech_Acmpl { get; set; }
        public virtual bool? PrfArt_ClsDnc_SklTech_Procs { get; set; }
        public virtual bool? PrfArt_ClsDnc_SklTech_NEngmt { get; set; }
        public virtual bool? PrfArt_ClsDnc_BscStps_Acmpl { get; set; }
        public virtual bool? PrfArt_ClsDnc_BscStps_Procs { get; set; }
        public virtual bool? PrfArt_ClsDnc_BscStps_NEngmt { get; set; }
        public virtual bool? PrfArt_ClsDnc_Cmtmnt_Acmpl { get; set; }
        public virtual bool? PrfArt_ClsDnc_Cmtmnt_Procs { get; set; }
        public virtual bool? PrfArt_ClsDnc_Cmtmnt_NEngmt { get; set; }
    }
}