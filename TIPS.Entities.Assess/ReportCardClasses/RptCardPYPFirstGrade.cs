using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIPS.Entities.Assess.ReportCardClasses
{
    public class RptCardPYPFirstGrade
    {
        /// <summary>
        /// ACCOMPLISHED == Accom, PROGRESSING == prog, NEEDS ENCOURAGEMENT == Encour, IdNo == NewId,
        /// </summary>
        public virtual long Id { get; set; }
        public virtual string RequestNo { get; set; }
        public virtual string IdNo { get; set; }
        public virtual long PreRegNum { get; set; }
        public virtual string Name { get; set; }
        public virtual string Campus { get; set; }
        public virtual string Section { get; set; }
        public virtual string Grade { get; set; }
        public virtual string AcademicYear { get; set; }
        public virtual long Semester { get; set; }
        public virtual string TeacherName { get; set; }
        public virtual string RptCardStatus { get; set; }
        public virtual DateTime? RptDate { get; set; }

        /// <summary>
        /// English Reading == ER, Confidence = Conf, Fluency == Flu, Comprehension == Com 
        /// </summary>

        public virtual bool? ER_Conf_Accom { get; set; }
        public virtual bool? ER_Conf_Prog { get; set; }
        public virtual bool? ER_Conf_Encour { get; set; }
        public virtual bool? ER_Flu_Accom { get; set; }
        public virtual bool? ER_Flu_Prog { get; set; }
        public virtual bool? ER_Flu_Encour { get; set; }
        public virtual bool? ER_Com_Accom { get; set; }
        public virtual bool? ER_Com_Prog { get; set; }
        public virtual bool? ER_Com_Encour { get; set; }

        /// <summary>
        /// English LISTENING & SPEAKING FICTION == ELSF, Beginning == Beg, Setting  == Sett, Character == Char, Problem  == Pro,Sequence == Seq, Resolution== Res
        /// English LISTENING & SPEAKING NON-FICTION == ELSNF, Topic == Top , Main idea(s) & details = Idea , Organization == Org ,Command of vocabulary == voc ,Accuracy == Acc
        /// </summary>

        public virtual bool? ELSF_Beg_Accom { get; set; }
        public virtual bool? ELSF_Beg_Prog { get; set; }
        public virtual bool? ELSF_Beg_Encour { get; set; }
        public virtual bool? ELSF_Sett_Accom { get; set; }
        public virtual bool? ELSF_Sett_Prog { get; set; }
        public virtual bool? ELSF_Sett_Encour { get; set; }
        public virtual bool? ELSF_Char_Accom { get; set; }
        public virtual bool? ELSF_Char_Prog { get; set; }
        public virtual bool? ELSF_Char_Encour { get; set; }
        public virtual bool? ELSF_Pro_Accom { get; set; }
        public virtual bool? ELSF_Pro_Prog { get; set; }
        public virtual bool? ELSF_Pro_Encour { get; set; }
        public virtual bool? ELSF_Seq_Accom { get; set; }
        public virtual bool? ELSF_Seq_Prog { get; set; }
        public virtual bool? ELSF_Seq_Encour { get; set; }
        public virtual bool? ELSF_Res_Accom { get; set; }
        public virtual bool? ELSF_Res_Prog { get; set; }
        public virtual bool? ELSF_Res_Encour { get; set; }


        public virtual bool? ELSNF_Top_Accom { get; set; }
        public virtual bool? ELSNF_Top_Prog { get; set; }
        public virtual bool? ELSNF_Top_Encour { get; set; }
        public virtual bool? ELSNF_Idea_Accom { get; set; }
        public virtual bool? ELSNF_Idea_Prog { get; set; }
        public virtual bool? ELSNF_Idea_Encour { get; set; }
        public virtual bool? ELSNF_Org_Accom { get; set; }
        public virtual bool? ELSNF_Org_Prog { get; set; }
        public virtual bool? ELSNF_Org_Encour { get; set; }
        public virtual bool? ELSNF_voc_Accom { get; set; }
        public virtual bool? ELSNF_voc_Prog { get; set; }
        public virtual bool? ELSNF_voc_Encour { get; set; }
        public virtual bool? ELSNF_Acc_Accom { get; set; }
        public virtual bool? ELSNF_Acc_Prog { get; set; }
        public virtual bool? ELSNF_Acc_Encour { get; set; }

        /// <summary>
        /// Language Skills == LS, Capitalization == Cap, Capital letter == Capl, Plurals == Plu, Regular plural nouns == regp, Irregular plural nouns== Irrp, Alphabetical order == Alpo
        /// Homophones == Homp, Antonyms == Ant, Nouns == N, Common nouns == Comn, Proper nouns == Propn, Pronouns == Pron
        /// Verbs == V, Action verbs == Acv, Tenses of action verbs == Tav, Linking verbs == Linv,
        /// Adjectives == Adj, Describing words == Dw, Comparative adjectives == Coma,
        /// Writing sentences == Ws, Capital letters and Period == Clp, Complete thoughts == comt



        /// </summary>

        public virtual bool? LS_Cap_Capl_Accom { get; set; }
        public virtual bool? LS_Cap_Capl_Prog { get; set; }
        public virtual bool? LS_Cap_Capl_Encour { get; set; }

        public virtual bool? LS_Plu_regp_Accom { get; set; }
        public virtual bool? LS_Plu_regp_Prog { get; set; }
        public virtual bool? LS_Plu_regp_Encour { get; set; }

        public virtual bool? LS_Plu_Irrp_Accom { get; set; }
        public virtual bool? LS_Plu_Irrp_Prog { get; set; }
        public virtual bool? LS_Plu_Irrp_Encour { get; set; }

        public virtual bool? LS_Alpo_Accom { get; set; }
        public virtual bool? LS_Alpo_Prog { get; set; }
        public virtual bool? LS_Alpo_Encour { get; set; }

        public virtual bool? LS_Homp_Accom { get; set; }
        public virtual bool? LS_Homp_Prog { get; set; }
        public virtual bool? LS_Homp_Encour { get; set; }

        public virtual bool? LS_Ant_Accom { get; set; }
        public virtual bool? LS_Ant_Prog { get; set; }
        public virtual bool? LS_Ant_Encour { get; set; }

        public virtual bool? LS_NN_Accom { get; set; }
        public virtual bool? LS_NN_Prog { get; set; }
        public virtual bool? LS_NN_Encour { get; set; }

        public virtual bool? LS_N_Comon_Accom { get; set; }
        public virtual bool? LS_N_Comon_Prog { get; set; }
        public virtual bool? LS_N_Comon_Encour { get; set; }

        public virtual bool? LS_N_Propn_Accom { get; set; }
        public virtual bool? LS_N_Propn_Prog { get; set; }
        public virtual bool? LS_N_Propn_Encour { get; set; }

        public virtual bool? LS_N_Pron_Accom { get; set; }
        public virtual bool? LS_N_Pron_Prog { get; set; }
        public virtual bool? LS_N_Pron_Encour { get; set; }

        public virtual bool? LS_V_Acv_Accom { get; set; }
        public virtual bool? LS_V_Acv_Prog { get; set; }
        public virtual bool? LS_V_Acv_Encour { get; set; }

        public virtual bool? LS_V_Tav_Accom { get; set; }
        public virtual bool? LS_V_Tav_Prog { get; set; }
        public virtual bool? LS_V_Tav_Encour { get; set; }

        public virtual bool? LS_V_Linv_Accom { get; set; }
        public virtual bool? LS_V_Linv_Prog { get; set; }
        public virtual bool? LS_V_Linv_Encour { get; set; }

        public virtual bool? LS_Adj_Dw_Accom { get; set; }
        public virtual bool? LS_Adj_Dw_Prog { get; set; }
        public virtual bool? LS_Adj_Dw_Encour { get; set; }

        public virtual bool? LS_Adj_Coma_Accom { get; set; }
        public virtual bool? LS_Adj_Coma_Prog { get; set; }
        public virtual bool? LS_Adj_Coma_Encour { get; set; }

        public virtual bool? LS_Ws_Clp_Accom { get; set; }
        public virtual bool? LS_Ws_Clp_Prog { get; set; }
        public virtual bool? LS_Ws_Clp_Encour { get; set; }

        public virtual bool? LS_Ws_comt_Accom { get; set; }
        public virtual bool? LS_Ws_comt_Prog { get; set; }
        public virtual bool? LS_Ws_comt_Encour { get; set; }

        /// <summary>
        /// Tamil/Hindi == TH, READING == Red, Confidence == Conf, Fluency == Flu, Tone and intonation == Toi, Pronunciation == Pron, Understanding == Und
        /// WRITING == Wri, Spelling == Spel, Vocabulary == Voc, Punctuation == Punc , Grammar == Gra, Ideas and content == Idec, Neatness and handwriting == Neh,
        /// LISTENING & SPEAKING == Ls, 
        /// </summary>

        public virtual bool? TH_Red_Conf_Accom { get; set; }
        public virtual bool? TH_Red_Conf_Prog { get; set; }
        public virtual bool? TH_Red_Conf_Encour { get; set; }
        public virtual bool? TH_Red_Flu_Accom { get; set; }
        public virtual bool? TH_Red_Flu_Prog { get; set; }
        public virtual bool? TH_Red_Flu_Encour { get; set; }
        public virtual bool? TH_Red_Toi_Accom { get; set; }
        public virtual bool? TH_Red_Toi_Prog { get; set; }
        public virtual bool? TH_Red_Toi_Encour { get; set; }
        public virtual bool? TH_Red_Pron_Accom { get; set; }
        public virtual bool? TH_Red_Pron_Prog { get; set; }
        public virtual bool? TH_Red_Pron_Encour { get; set; }
        public virtual bool? TH_Red_Und_Accom { get; set; }
        public virtual bool? TH_Red_Und_Prog { get; set; }
        public virtual bool? TH_Red_Und_Encour { get; set; }


        public virtual bool? TH_Wri_Spel_Accom { get; set; }
        public virtual bool? TH_Wri_Spel_Prog { get; set; }
        public virtual bool? TH_Wri_Spel_Encour { get; set; }
        public virtual bool? TH_Wri_Voc_Accom { get; set; }
        public virtual bool? TH_Wri_Voc_Prog { get; set; }
        public virtual bool? TH_Wri_Voc_Encour { get; set; }
        public virtual bool? TH_Wri_Punc_Accom { get; set; }
        public virtual bool? TH_Wri_Punc_Prog { get; set; }
        public virtual bool? TH_Wri_Punc_Encour { get; set; }
        public virtual bool? TH_Wri_Gra_Accom { get; set; }
        public virtual bool? TH_Wri_Gra_Prog { get; set; }
        public virtual bool? TH_Wri_Gra_Encour { get; set; }
        public virtual bool? TH_Wri_Idec_Accom { get; set; }
        public virtual bool? TH_Wri_Idec_Prog { get; set; }
        public virtual bool? TH_Wri_Idec_Encour { get; set; }
        public virtual bool? TH_Wri_Neh_Accom { get; set; }
        public virtual bool? TH_Wri_Neh_Prog { get; set; }
        public virtual bool? TH_Wri_Neh_Encour { get; set; }


        public virtual bool? TH_Ls_Conf_Accom { get; set; }
        public virtual bool? TH_Ls_Conf_Prog { get; set; }
        public virtual bool? TH_Ls_Conf_Encour { get; set; }
        public virtual bool? TH_Ls_Flu_Accom { get; set; }
        public virtual bool? TH_Ls_Flu_Prog { get; set; }
        public virtual bool? TH_Ls_Flu_Encour { get; set; }
        public virtual bool? TH_Ls_Pron_Accom { get; set; }
        public virtual bool? TH_Ls_Pron_Prog { get; set; }
        public virtual bool? TH_Ls_Pron_Encour { get; set; }
        public virtual bool? TH_Ls_Und_Accom { get; set; }
        public virtual bool? TH_Ls_Und_Prog { get; set; }
        public virtual bool? TH_Ls_Und_Encour { get; set; }
        public virtual bool? TH_Ls_Voc_Accom { get; set; }
        public virtual bool? TH_Ls_Voc_Prog { get; set; }
        public virtual bool? TH_Ls_Voc_Encour { get; set; }

        /// <summary>
        /// Mathematics == M, Numbers to 10 == N, Counting to 10 == Coun,Able to count from 0 to 10 objects == obj, Able to read and write 0-10 in numbers and words == wor,
        /// Comparing numbers == compn, Able to compare two sets of objects using one-to-one correspondence == corr, Able to identify the number that is greater than or less than another number == idnum,
        /// Making number patterns == Mnp, Able to make number patterns == amnp,
        /// Number bonds == Nb, Able to find different number bonds for numbers to 10 == nbn,
        /// Addition Facts to 10 == Af, Ways to add == wa, Able to count on to add == aca, Able to use number bonds to add in any order == aar,
        /// Making addition stories == Mas, Real world problems: Addition == Radd,
        /// Subtraction facts to 10 == Sf, Ways to subtract == Ws,Able to take away to subtract == aas, Able to count back to subtract == cbs, Able to use number bonds to subtract == nbs, Able to write and solve subtraction sentences == sss,
        /// Real world problems: Subtraction == Rs,Able to solve real- world word problems == wwp,
        /// Making fact families == Mff, Able to recognize related addition and subtraction sentences == arss, Able to use fact families to solve real-world problems == asrwp,
        /// </summary>

        public virtual bool? M_N_Coun_obj_Accom { get; set; }
        public virtual bool? M_N_Coun_obj_Prog { get; set; }
        public virtual bool? M_N_Coun_obj_Encour { get; set; }
        public virtual bool? M_N_Coun_wor_Accom { get; set; }
        public virtual bool? M_N_Coun_wor_Prog { get; set; }
        public virtual bool? M_N_Coun_wor_Encour { get; set; }

        public virtual bool? M_N_compn_corr_Accom { get; set; }
        public virtual bool? M_N_compn_corr_Prog { get; set; }
        public virtual bool? M_N_compn_corr_Encour { get; set; }
        public virtual bool? M_N_compn_idnum_Accom { get; set; }
        public virtual bool? M_N_compn_idnum_Prog { get; set; }
        public virtual bool? M_N_compn_idnum_Encour { get; set; }

        public virtual bool? M_N_Mnp_amnp_Accom { get; set; }
        public virtual bool? M_N_Mnp_amnp_Prog { get; set; }
        public virtual bool? M_N_Mnp_amnp_Encour { get; set; }

        public virtual bool? M_Nb_Nb_Accom { get; set; }
        public virtual bool? M_Nb_Nb_Prog { get; set; }
        public virtual bool? M_Nb_Nb_Encour { get; set; }


        public virtual bool? M_Af_wa_aca_Accom { get; set; }
        public virtual bool? M_Af_wa_aca_Prog { get; set; }
        public virtual bool? M_Af_wa_aca_Encour { get; set; }
        public virtual bool? M_Af_wa_aar_Accom { get; set; }
        public virtual bool? M_Af_wa_aar_Prog { get; set; }
        public virtual bool? M_Af_wa_aar_Encour { get; set; }

        public virtual bool? M_Af_Mas_Accom { get; set; }
        public virtual bool? M_Af_Mas_Prog { get; set; }
        public virtual bool? M_Af_Mas_Encour { get; set; }

        public virtual bool? M_Af_Radd_Accom { get; set; }
        public virtual bool? M_Af_Radd_Prog { get; set; }
        public virtual bool? M_Af_Radd_Encour { get; set; }


        public virtual bool? M_Sf_Ws_aas_Accom { get; set; }
        public virtual bool? M_Sf_Ws_aas_Prog { get; set; }
        public virtual bool? M_Sf_Ws_aas_Encour { get; set; }

        public virtual bool? M_Sf_Ws_cbs_Accom { get; set; }
        public virtual bool? M_Sf_Ws_cbs_Prog { get; set; }
        public virtual bool? M_Sf_Ws_cbs_Encour { get; set; }

        public virtual bool? M_Sf_Ws_nbs_Accom { get; set; }
        public virtual bool? M_Sf_Ws_nbs_Prog { get; set; }
        public virtual bool? M_Sf_Ws_nbs_Encour { get; set; }

        public virtual bool? M_Sf_Ws_sss_Accom { get; set; }
        public virtual bool? M_Sf_Ws_sss_Prog { get; set; }
        public virtual bool? M_Sf_Ws_sss_Encour { get; set; }

        public virtual bool? M_Sf_Rs_wwp_Accom { get; set; }
        public virtual bool? M_Sf_Rs_wwp_Prog { get; set; }
        public virtual bool? M_Sf_Rs_wwp_Encour { get; set; }

        public virtual bool? M_Sf_Mff_arss_Accom { get; set; }
        public virtual bool? M_Sf_Mff_arss_Prog { get; set; }
        public virtual bool? M_Sf_Mff_arss_Encour { get; set; }

        public virtual bool? M_Sf_Mff_asrwp_Accom { get; set; }
        public virtual bool? M_Sf_Mff_asrwp_Prog { get; set; }
        public virtual bool? M_Sf_Mff_asrwp_Encour { get; set; }

        /// <summary>
        /// Mathematics Shapes and Patterns == Msp,
        /// Shapes and Patterns == Sp,
        /// Exploring plane shapes == Eps, Able to identify, classify and describe plane shapes == dps, Able to describe the whole as the sum of its parts == sip,
        /// Exploring solid shapes == Ess, Making pictures and models with shapes == mws, Seeing shapes around us == sau, Making patterns with plane shapes == wps, Making patterns with solid shapes == wss,
        /// Ordinal numbers and position == Onp, Ordinal numbers == on, Position words == pw,
        /// </summary>


        public virtual bool? Msp_Sp_Eps_dps_Accom { get; set; }
        public virtual bool? Msp_Sp_Eps_dps_Prog { get; set; }
        public virtual bool? Msp_Sp_Eps_dps_Encour { get; set; }
        public virtual bool? Msp_Sp_Eps_sip_Accom { get; set; }
        public virtual bool? Msp_Sp_Eps_sip_Prog { get; set; }
        public virtual bool? Msp_Sp_Eps_sip_Encour { get; set; }

        public virtual bool? Msp_Sp_Ess_Accom { get; set; }
        public virtual bool? Msp_Sp_Ess_Prog { get; set; }
        public virtual bool? Msp_Sp_Ess_Encour { get; set; }

        public virtual bool? Msp_Sp_mws_Accom { get; set; }
        public virtual bool? Msp_Sp_mws_Prog { get; set; }
        public virtual bool? Msp_Sp_mws_Encour { get; set; }

        public virtual bool? Msp_Sp_sau_Accom { get; set; }
        public virtual bool? Msp_Sp_sau_Prog { get; set; }
        public virtual bool? Msp_Sp_sau_Encour { get; set; }

        public virtual bool? Msp_Sp_wps_Accom { get; set; }
        public virtual bool? Msp_Sp_wps_Prog { get; set; }
        public virtual bool? Msp_Sp_wps_Encour { get; set; }

        public virtual bool? Msp_Sp_wss_Accom { get; set; }
        public virtual bool? Msp_Sp_wss_Prog { get; set; }
        public virtual bool? Msp_Sp_wss_Encour { get; set; }

        public virtual bool? Msp_Onp_on_Accom { get; set; }
        public virtual bool? Msp_Onp_on_Prog { get; set; }
        public virtual bool? Msp_Onp_on_Encour { get; set; }

        public virtual bool? Msp_Onp_pw_Accom { get; set; }
        public virtual bool? Msp_Onp_pw_Prog { get; set; }
        public virtual bool? Msp_Onp_pw_Encour { get; set; }

        /// <summary>
        /// Programme Of Inquiry Theme – How we organize ourselves == Prog, Purpose of skills,and attitudes == psa, Qualities of responsible learners == qrl, Making contributions as a learner == mcl, Comments == Com,

        /// </summary>

        public virtual bool? Prog_psa_Accom { get; set; }
        public virtual bool? Prog_psa_Prog { get; set; }
        public virtual bool? Prog_psa_Encour { get; set; }

        public virtual bool? Prog_qrl_Accom { get; set; }
        public virtual bool? Prog_qrl_Prog { get; set; }
        public virtual bool? Prog_qrl_Encour { get; set; }

        public virtual bool? Prog_mcl_Accom { get; set; }
        public virtual bool? Prog_mcl_Prog { get; set; }
        public virtual bool? Prog_mcl_Encour { get; set; }

        public virtual string Prog_Com { get; set; }

        /// <summary>
        /// Development of Skills == Dos, Cooperating == Coo, Group decision making == Gdm,  Organization == Org, Time management == Tm, 
        /// Attitudes == Att, Cooperation == Co, Integrity == Int,Empathy == Emp,
        /// Development of Learner Profile == Dlp,Principled == pri, Caring == Car,
        /// </summary>

        public virtual bool? Prog_Dos_Coo_Accom { get; set; }
        public virtual bool? Prog_Dos_Coo_Prog { get; set; }
        public virtual bool? Prog_Dos_Coo_Encour { get; set; }
        public virtual bool? Prog_Dos_Gdm_Accom { get; set; }
        public virtual bool? Prog_Dos_Gdm_Prog { get; set; }
        public virtual bool? Prog_Dos_Gdm_Encour { get; set; }

        public virtual bool? Prog_Dos_Org_Accom { get; set; }
        public virtual bool? Prog_Dos_Org_Prog { get; set; }
        public virtual bool? Prog_Dos_Org_Encour { get; set; }
        public virtual bool? Prog_Dos_Tm_Accom { get; set; }
        public virtual bool? Prog_Dos_Tm_Prog { get; set; }
        public virtual bool? Prog_Dos_Tm_Encour { get; set; }

        public virtual bool? Prog_Att_Co_Accom { get; set; }
        public virtual bool? Prog_Att_Co_Prog { get; set; }
        public virtual bool? Prog_Att_Co_Encour { get; set; }
        public virtual bool? Prog_Att_Int_Accom { get; set; }
        public virtual bool? Prog_Att_Int_Prog { get; set; }
        public virtual bool? Prog_Att_Int_Encour { get; set; }
        public virtual bool? Prog_Att_Emp_Accom { get; set; }
        public virtual bool? Prog_Att_Emp_Prog { get; set; }
        public virtual bool? Prog_Att_Emp_Encour { get; set; }

        public virtual bool? Prog_Dlp_pri_Accom { get; set; }
        public virtual bool? Prog_Dlp_pri_Prog { get; set; }
        public virtual bool? Prog_Dlp_pri_Encour { get; set; }
        public virtual bool? Prog_Dlp_Car_Accom { get; set; }
        public virtual bool? Prog_Dlp_Car_Prog { get; set; }
        public virtual bool? Prog_Dlp_Car_Encour { get; set; }


        /// <summary>
        /// Theme who we are
        /// Whoweare == Wwa, Concept of home == ch, Homes in different culture == hdc, Circumstances that determine where people live == dwpl, Comments: == Com,
        /// Development of Skills == Ds, Respecting others == res, Collecting data == col, Organizing data  == ord, 
        /// Attitudes == Att, Respect == resp, Tolerance  == Tol, 
        /// Development of Learner Profile == Dlp, Open-minded == Om, Reflective == Ref,
        /// </summary>
        public virtual bool? Wwa_ch_Accom { get; set; }
        public virtual bool? Wwa_ch_Prog { get; set; }
        public virtual bool? Wwa_ch_Encour { get; set; }
        public virtual bool? Wwa_hdc_Accom { get; set; }
        public virtual bool? Wwa_hdc_Prog { get; set; }
        public virtual bool? Wwa_hdc_Encour { get; set; }
        public virtual bool? Wwa_dwpl_Accom { get; set; }
        public virtual bool? Wwa_dwpl_Prog { get; set; }
        public virtual bool? Wwa_dwpl_Encour { get; set; }
        public virtual string Wwa_Com { get; set; }

        public virtual bool? Wwa_Ds_res_Accom { get; set; }
        public virtual bool? Wwa_Ds_res_Prog { get; set; }
        public virtual bool? Wwa_Ds_res_Encour { get; set; }
        public virtual bool? Wwa_Ds_col_Accom { get; set; }
        public virtual bool? Wwa_Ds_col_Prog { get; set; }
        public virtual bool? Wwa_Ds_col_Encour { get; set; }
        public virtual bool? Wwa_Ds_ord_Accom { get; set; }
        public virtual bool? Wwa_Ds_ord_Prog { get; set; }
        public virtual bool? Wwa_Ds_ord_Encour { get; set; }

        public virtual bool? Wwa_Att_resp_Accom { get; set; }
        public virtual bool? Wwa_Att_resp_Prog { get; set; }
        public virtual bool? Wwa_Att_resp_Encour { get; set; }
        public virtual bool? Wwa_Att_Tol_Accom { get; set; }
        public virtual bool? Wwa_Att_Tol_Prog { get; set; }
        public virtual bool? Wwa_Att_Tol_Encour { get; set; }

        public virtual bool? Wwa_Dlp_Om_Accom { get; set; }
        public virtual bool? Wwa_Dlp_Om_Prog { get; set; }
        public virtual bool? Wwa_Dlp_Om_Encour { get; set; }
        public virtual bool? Wwa_Dlp_Ref_Accom { get; set; }
        public virtual bool? Wwa_Dlp_Ref_Prog { get; set; }
        public virtual bool? Wwa_Dlp_Ref_Encour { get; set; }

        /// <summary>
        /// How we express
        /// how we express == Hwe, Ideas through facial expressions == ife, Combining music and dance movements to tell a story == mts, Different music and dance forms == mdf, Comments: == Com,
        /// Development of Skills == Ds, Speaking == Sp, Non-verbal communication == Nvc, Gross motor skills == Gms, Organization  == Org, 
        /// Attitudes == Att, Creativity == Cre, Appreciation == App, Confidence == Conf,
        /// Development of Learner Profile == Dlp, Risk-taker == Rt, Communicator == Com,
        /// </summary>

        public virtual bool? Hwe_ife_Accom { get; set; }
        public virtual bool? Hwe_ife_Prog { get; set; }
        public virtual bool? Hwe_ife_Encour { get; set; }
        public virtual bool? Hwe_mts_Accom { get; set; }
        public virtual bool? Hwe_mts_Prog { get; set; }
        public virtual bool? Hwe_mts_Encour { get; set; }
        public virtual bool? Hwe_mdf_Accom { get; set; }
        public virtual bool? Hwe_mdf_Prog { get; set; }
        public virtual bool? Hwe_mdf_Encour { get; set; }
        public virtual string Hwe_Com { get; set; }


        public virtual bool? Hwe_Ds_Sp_Accom { get; set; }
        public virtual bool? Hwe_Ds_Sp_Prog { get; set; }
        public virtual bool? Hwe_Ds_Sp_Encour { get; set; }
        public virtual bool? Hwe_Ds_Nvc_Accom { get; set; }
        public virtual bool? Hwe_Ds_Nvc_Prog { get; set; }
        public virtual bool? Hwe_Ds_Nvc_Encour { get; set; }

        public virtual bool? Hwe_Ds_Gms_Accom { get; set; }
        public virtual bool? Hwe_Ds_Gms_Prog { get; set; }
        public virtual bool? Hwe_Ds_Gms_Encour { get; set; }
        public virtual bool? Hwe_Ds_Org_Accom { get; set; }
        public virtual bool? Hwe_Ds_Org_Prog { get; set; }
        public virtual bool? Hwe_Ds_Org_Encour { get; set; }

        public virtual bool? Hwe_Att_Cre_Accom { get; set; }
        public virtual bool? Hwe_Att_Cre_Prog { get; set; }
        public virtual bool? Hwe_Att_Cre_Encour { get; set; }
        public virtual bool? Hwe_Att_App_Accom { get; set; }
        public virtual bool? Hwe_Att_App_Prog { get; set; }
        public virtual bool? Hwe_Att_App_Encour { get; set; }
        public virtual bool? Hwe_Att_Conf_Accom { get; set; }
        public virtual bool? Hwe_Att_Conf_Prog { get; set; }
        public virtual bool? Hwe_Att_Conf_Encour { get; set; }

        public virtual bool? Hwe_Dlp_Rt_Accom { get; set; }
        public virtual bool? Hwe_Dlp_Rt_Prog { get; set; }
        public virtual bool? Hwe_Dlp_Rt_Encour { get; set; }
        public virtual bool? Hwe_Dlp_Com_Accom { get; set; }
        public virtual bool? Hwe_Dlp_Com_Prog { get; set; }
        public virtual bool? Hwe_Dlp_Com_Encour { get; set; }
        /// <summary>
        /// Applied Computers == Ac, Software Application  == Sa, Technical skills == Ts, 
        /// </summary>

        public virtual bool? Ac_Sa_Accom { get; set; }
        public virtual bool? Ac_Sa_Prog { get; set; }
        public virtual bool? Ac_Sa_Encour { get; set; }
        public virtual bool? Ac_Ts_Accom { get; set; }
        public virtual bool? Ac_Ts_Prog { get; set; }
        public virtual bool? Ac_Ts_Encour { get; set; }

        /// <summary>
        /// Physical Education Cycling == Pec, Starting == Sta, Stopping == Stop, Riding == Rid, Looking == Loo, Riding One Handed == Roh, 
        /// </summary>
        public virtual bool? Pec_Sta_Accom { get; set; }
        public virtual bool? Pec_Sta_Prog { get; set; }
        public virtual bool? Pec_Sta_Encour { get; set; }
        public virtual bool? Pec_Stop_Accom { get; set; }
        public virtual bool? Pec_Stop_Prog { get; set; }
        public virtual bool? Pec_Stop_Encour { get; set; }
        public virtual bool? Pec_Rid_Accom { get; set; }
        public virtual bool? Pec_Rid_Prog { get; set; }
        public virtual bool? Pec_Rid_Encour { get; set; }
        public virtual bool? Pec_Loo_Accom { get; set; }
        public virtual bool? Pec_Loo_Prog { get; set; }
        public virtual bool? Pec_Loo_Encour { get; set; }
        public virtual bool? Pec_Roh_Accom { get; set; }
        public virtual bool? Pec_Roh_Prog { get; set; }
        public virtual bool? Pec_Roh_Encour { get; set; }

        /// <summary>
        /// Swimming == Sw, Bobbing == bob, Leg beat holding the wall == lbhw, Leg beat without holding the wall == lbwoh,Floating == flo,
        /// </summary>
        public virtual bool? Sw_bob_Accom { get; set; }
        public virtual bool? Sw_bob_Prog { get; set; }
        public virtual bool? Sw_bob_Encour { get; set; }
        public virtual bool? Sw_lbhw_Accom { get; set; }
        public virtual bool? Sw_lbhw_Prog { get; set; }
        public virtual bool? Sw_lbhw_Encour { get; set; }
        public virtual bool? Sw_lbwoh_Accom { get; set; }
        public virtual bool? Sw_lbwoh_Prog { get; set; }
        public virtual bool? Sw_lbwoh_Encour { get; set; }
        public virtual bool? Sw_flo_Accom { get; set; }
        public virtual bool? Sw_flo_Prog { get; set; }
        public virtual bool? Sw_flo_Encour { get; set; }


        /// <summary>
        /// Track and Field
        /// Track and Field == Tf,Sprint == Sp, Hurdle == Hur, Shot put == Shp, Relay == Rel,
        /// </summary>
        public virtual bool? Tf_Sp_Accom { get; set; }
        public virtual bool? Tf_Sp_Prog { get; set; }
        public virtual bool? Tf_Sp_Encour { get; set; }
        public virtual bool? Tf_Hur_Accom { get; set; }
        public virtual bool? Tf_Hur_Prog { get; set; }
        public virtual bool? Tf_Hur_Encour { get; set; }
        public virtual bool? Tf_Shp_Accom { get; set; }
        public virtual bool? Tf_Shp_Prog { get; set; }
        public virtual bool? Tf_Shp_Encour { get; set; }
        public virtual bool? Tf_Rel_Accom { get; set; }
        public virtual bool? Tf_Rel_Prog { get; set; }
        public virtual bool? Tf_Rel_Encour { get; set; }

        /// <summary>
        /// Performing  Arts Music == Pam, Adhara sruthi == As, Saptha swaras  == Ss, Practicing integrated songs == pis, Practicing theme songs == Pts, 
        /// </summary>
        public virtual bool? Pam_As_Accom { get; set; }
        public virtual bool? Pam_As_Prog { get; set; }
        public virtual bool? Pam_As_Encour { get; set; }
        public virtual bool? Pam_Ss_Accom { get; set; }
        public virtual bool? Pam_Ss_Prog { get; set; }
        public virtual bool? Pam_Ss_Encour { get; set; }
        public virtual bool? Pam_Pis_Accom { get; set; }
        public virtual bool? Pam_Pis_Prog { get; set; }
        public virtual bool? Pam_Pis_Encour { get; set; }
        public virtual bool? Pam_Pts_Accom { get; set; }
        public virtual bool? Pam_Pts_Prog { get; set; }
        public virtual bool? Pam_Pts_Encour { get; set; }

        ///
        /// Western Dance
        /// Western Dance == Wd, Body Movements == Bm,Combination of moves == Cm, 8 and 16 count movements == acm, Dance for music and song == Dms,
        ///
        public virtual bool? Wd_Bm_Accom { get; set; }
        public virtual bool? Wd_Bm_Prog { get; set; }
        public virtual bool? Wd_Bm_Encour { get; set; }
        public virtual bool? Wd_Cm_Accom { get; set; }
        public virtual bool? Wd_Cm_Prog { get; set; }
        public virtual bool? Wd_Cm_Encour { get; set; }
        public virtual bool? Wd_acm_Accom { get; set; }
        public virtual bool? Wd_acm_Prog { get; set; }
        public virtual bool? Wd_acm_Encour { get; set; }
        public virtual bool? Wd_Dms_Accom { get; set; }
        public virtual bool? Wd_Dms_Prog { get; set; }
        public virtual bool? Wd_Dms_Encour { get; set; }

        ///
        /// Classical Dance == Cd, Body movements == Bm, Asamyuktha Hastham & meaning == Ahm, Able to move the body according to the talam  == Amba, Able to show 21 mudras  == Asm, Able to explain 5 mudras == Aem,
        /// Basic steps Poses == BSP, Body Movements == BM, Hastham = H,
        /// Basic steps (lesson 1&2) == Bs, Poses  == Po, Able to do half sitting with proper hand mudras  and facial expressions == Afe, Able to show different poses of Lord Siva == Pls, 
        /// Folk steps == Fs,
        ///

        public virtual bool? Cd_Bm_Ahm_Amba_Accom { get; set; }
        public virtual bool? Cd_Bm_Ahm_Amba_Prog { get; set; }
        public virtual bool? Cd_Bm_Ahm_Amba_Encour { get; set; }
        public virtual bool? Cd_Bm_Asm_Accom { get; set; }
        public virtual bool? Cd_Bm_Asm_Prog { get; set; }
        public virtual bool? Cd_Bm_Asm_Encour { get; set; }
        public virtual bool? Cd_Bm_Aem_Accom { get; set; }
        public virtual bool? Cd_Bm_Aem_Prog { get; set; }
        public virtual bool? Cd_Bm_Aem_Encour { get; set; }

        public virtual bool? Cd_Bm_Po_Afe_Accom { get; set; }
        public virtual bool? Cd_Bm_Po_Afe_Prog { get; set; }
        public virtual bool? Cd_Bm_Po_Afe_Encour { get; set; }
        public virtual bool? Cd_Bm_Po_Pls_Accom { get; set; }
        public virtual bool? Cd_Bm_Po_Pls_Prog { get; set; }
        public virtual bool? Cd_Bm_Po_Pls_Encour { get; set; }

        public virtual bool? Cd_Fs_Accom { get; set; }
        public virtual bool? Cd_Fs_Prog { get; set; }
        public virtual bool? Cd_Fs_Encour { get; set; }

        public virtual string RptCardCreatedBy { get; set; }
        public virtual DateTime? RptCardCreatedDate { get; set; }
        public virtual string RptCardModifiedBy { get; set; }
        public virtual DateTime? RptCardModifiedDate { get; set; }

    }
}
