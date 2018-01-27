$(function () {
    $('#btnReset').click(function () {
        var url = $("#BackUrl").val();
        window.location.href = url;
    });
    var campus = ""; var grade = ""; var section = ""; var month = ""; var name = "";
    var grid_selector = "#JqGrid";
    var pager_selector = "#JqGridPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand 
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    //pager icon
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    $(grid_selector).jqGrid({
        //url: "/Assess360/SummativeAssessmentJqGridNew",
        datatype: 'json',
        type: 'GET',
        autowidth: true,
        height: 270,
        colNames: [],
        colModel: [],
        rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Total',
        sortroder: 'Desc',
        viewrecords: true,
        caption: '<i class="fa fa-list">&nbsp;</i>Summative Test Report'
    });

    $("#btnSearch").click(function () {
        name = $('#Name').val();
        campus = $('#Campus').val();
        grade = $('#Grade').val();
        section = $('#Section').val();
        if (!isEmptyorNull(campus) && !isEmptyorNull(grade) && !isEmptyorNull(section)) {
            $.ajax({
                type: 'POST',
                async: false,
                url: '/Assess360/CheckComparitiveAssessmetReport?Campus=' + campus + '&Grade=' + grade + '&Section=' + section,
                success: function (data) {
                    if (data == "Success") {
                        $('#IsCombinedSci').val(true);
                        $(grid_selector).GridUnload();
                        window.onload = loadgrid(campus, grade, section, true);
                    }
                    else {
                        $('#IsCombinedSci').val(false);
                        $(grid_selector).GridUnload();
                        window.onload = loadgrid(campus, grade, section, false);
                    }
                }
            });
        }
        else {
            ErrMsg('Please enter all the values.');
            return false;
        }
    });

    $("#btnGenerate").click(function () {
        name = $('#Name').val();
        campus = $('#Campus').val();
        grade = $('#Grade').val();
        section = $('#Section').val();
        if (!isEmptyorNull(campus) && !isEmptyorNull(grade) && !isEmptyorNull(section)) {
            window.open("ComparitiveSAReportJqGrid" + '?Campus=' + campus + '&Grade=' + grade + '&Section=' + section + '&IsCombinedSci=' + $('#IsCombinedSci').val()  + '&rows=9999&sidx=&sord=Desc&ExptXl=1');
        }
        else {
            ErrMsg('Please enter all the values.');
            return false;
        }
    });


    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    function loadgrid(campus, grade, section, isCombinedSci) {
        if (grade == "X" &&isCombinedSci == true) {
            colnames = "['RptId', 'Student Id', 'Name', 'Campus', 'Grade', 'Section', 'AcademicYear', 'EngSI','EngMI','EngMII','EngMIII','EngMIV','LangSI','LangMI','LangMII','LangMIII','LangMIV','ComSciSI','ComSciMI','ComSciMII','ComSciMIII','ComSciMIV','MathSI','MAthMI','MathMII','MathMIII','MathMIV','EcoSI','EcoMI','EcoMII','EcoMIII','EcoMIV','PhyEduSI','PhyEduMI','PhyEduMII','PhyEduMIII','PhyEduMIV','ICTSI','ICTMI','ICTMII','ICTMIII','ICTMIV','TotSI','TotMI','TotMII','TotMIII','TotMIV','AvgSI','AvgMI','AvgMII','AvgMIII','AvgMIV','GrdSI','GrdMI','GrdMII','GrdMIII','GrdMIV']";
            colmodel = "[{ name: 'RptId', index: 'RptId', editable: true, hidden: true, key: true },{ name: 'NewId', index: 'NewId', align: 'center', sortable: false },{ name: 'Name', index: 'Name' },{ name: 'Campus', index: 'Campus', align: 'center', sortable: false },{ name: 'Grade', index: 'Grade', align: 'center', sortable: false },{ name: 'Section', index: 'Section', align: 'center', sortable: false },{ name: 'AcademicYear', index: 'AcademicYear', align: 'center', sortable: false },{ name: 'SIEng', index: 'SIEng', align: 'center', sortable: false },{ name: 'MIEng', index: 'MIEng', align: 'center', sortable: false },{ name: 'MIIEng', index: 'MIIEng', align: 'center', sortable: false},{ name: 'MIIIEng', index: 'MIIIEng', align: 'center', sortable: false },{ name: 'MIVEng', index: 'MIVEng', align: 'center',sortable: false },";
            colmodel = colmodel + "{ name: 'SILang', index: 'SILang', align: 'center', sortable: false },{ name: 'MILang', index: 'MILang', align: 'center', sortable: false },{ name: 'MIILang', index: 'MIILang', align: 'center', sortable: false },{ name: 'MIIILang', index: 'MIIILang', align: 'center', sortable: false },{ name: 'MIVLang', index: 'MIVLang', align: 'center', sortable: false },{ name: 'SIComSci', index: 'SIComSci', align: 'center', sortable: false },{ name: 'MIComSci', index: 'MIComSci', align: 'center', sortable: false },{ name: 'MIICombSci', index: 'MIIComSci', align: 'center', sortable: false },{ name: 'MIIIComSci', index: 'MIIIComSci', align: 'center', sortable: false },{ name: 'MIVCombSci', index: 'MIVCombSci', align: 'center', sortable: false },{ name: 'SIMath', index: 'SIMath', align: 'center', sortable: false },{ name: 'MIMath', index: 'MIMath', align: 'center', sortable: false },";
            colmodel = colmodel + "{ name: 'MIIMath', index: 'MIIMath', align: 'center', sortable: false },{ name: 'MIIIMath', index: 'MIIIMath', align: 'center', sortable: false },{ name: 'MIVMath', index: 'MIVMath', align: 'center', sortable: false },{ name: 'SIEco', index: 'SIEco', align: 'center', sortable: false },{ name: 'MIEco', index: 'MIEco', align: 'center', sortable: false },{ name: 'MIIEco', index: 'MIIEco', align: 'center', sortable: false },{ name: 'MIIIEco', index: 'MIIIEco', align: 'center', sortable: false },{ name: 'MIVEco', index: 'MIVEco', align: 'center', sortable: false },{ name: 'SIPhyEdu', index: 'SIPhyEdu', align: 'center', sortable: false },{ name: 'MIPhyEdu', index: 'MIPhyEdu', align: 'center', sortable: false },{ name: 'MIIPhyEdu', index: 'MIIPhyEdu', align: 'center', sortable: false },{ name: 'MIIIPhyEdu', index: 'MIIIPhyEdu', align: 'center', sortable: false },";
            colmodel = colmodel + "{ name: 'MIVPhyEdu', index: 'MIVPhyEdu', align: 'center', sortable: false },{ name: 'SIICT', index: 'SIICT', align: 'center', sortable: false },{ name: 'MIICT', index: 'MIICT', align: 'center', sortable: false },{ name: 'MIIICT', index: 'MIIICT', align: 'center', sortable: false },{ name: 'MIIIICT', index: 'MIIIICT', align: 'center', sortable: false },{ name: 'MIVICT', index: 'MIVICT', align: 'center', sortable: false },{ name: 'SITotal', index: 'SITotal', align: 'center', sortable: false },{ name: 'MITotal', index: 'MITotal', align: 'center', sortable: false },{ name: 'MIITotal', index: 'MIITotal', align: 'center', sortable: false },{ name: 'MIIITotal', index: 'MIIITotal', align: 'center', sortable: false },{ name: 'MIVTotal', index: 'MIVTotal', align: 'center', sortable: false },{ name: 'SIAvg', index: 'SIAvg', align: 'center', sortable: false },";
            colmodel = colmodel + "{ name: 'MIAvg', index: 'MIAvg', align: 'center' },{ name: 'MIIAvg', index: 'MIIAvg', align: 'center' },{ name: 'MIIIAvg', index: 'MIIIAvg', align: 'center' },{ name: 'MIVAvg', index: 'MIVAvg', align: 'center' },{ name: 'SIGrd', index: 'SIGrd', align: 'center' },{ name: 'MIGrd', index: 'MIGrd', align: 'center' },{ name: 'MIIGrd', index: 'MIIGrd', align: 'center' },{ name: 'MIIIGrd', index: 'MIIIGrd', align: 'center' },{ name: 'MIVGrd', index: 'MIVGrd', align: 'center' }]";
        }
        else {
            colnames = "['RptId', 'Student Id', 'Name', 'Campus', 'Grade', 'Section', 'AcademicYear', 'EngSI','EngMI','EngMII','EngMIII','EngMIV','LangSI','LangMI','LangMII','LangMIII','LangMIV','MathSI','MAthMI','MathMII','MathMIII','MathMIV','ChemSI','ChemMI','ChemMII','ChemMIII','ChemMIV','PhySI','PhyMI','PhyMII','PhyMIII','PhyMIV','BioSI','BioMI','BioMII','BioMIII','BioMIV','EcoSI','EcoMI','EcoMII','EcoMIII','EcoMIV','ICTSI','ICTMI','ICTMII','ICTMIII','ICTMIV','TotSI','TotMI','TotMII','TotMIII','TotMIV','AvgSI','AvgMI','AvgMII','AvgMIII','AvgMIV','GrdSI','GrdMI','GrdMII','GrdMIII','GrdMIV']";
            colmodel = "[{ name: 'RptId', index: 'RptId', editable: true, hidden: true, key: true },{ name: 'NewId', index: 'NewId', align: 'center', sortable: false },{ name: 'Name', index: 'Name' },{ name: 'Campus', index: 'Campus', align: 'center', sortable: false },{ name: 'Grade', index: 'Grade', align: 'center', sortable: false },{ name: 'Section', index: 'Section', align: 'center', sortable: false },{ name: 'AcademicYear', index: 'AcademicYear', align: 'center', sortable: false },{ name: 'SIEng', index: 'SIEng', align: 'center', sortable: false},{ name: 'MIEng', index: 'MIEng', align: 'center', sortable: false },{ name: 'MIIEng', index: 'MIIEng', align: 'center', sortable: false },{ name: 'MIIIEng', index: 'MIIIEng', align: 'center', sortable: false },{ name: 'MIVEng', index: 'MIVEng', align: 'center',sortable: false },";
            colmodel = colmodel + "{ name: 'SILang', index: 'SILang', align: 'center', sortable: false },{ name: 'MILang', index: 'MILang', align: 'center', sortable: false },{ name: 'MIILang', index: 'MIILang', align: 'center', sortable: false },{ name: 'MIIILang', index: 'MIIILang', align: 'center', sortable: false },{ name: 'MIVLang', index: 'MIVLang', align: 'center', sortable: false },{ name: 'SIMath', index: 'SIMath', align: 'center', sortable: false },{ name: 'MIMath', index: 'MIMath', align: 'center', sortable: false },{ name: 'MIIMath', index: 'MIIMath', align: 'center', sortable: false },{ name: 'MIIIMath', index: 'MIIIMath', align: 'center', sortable: false },{ name: 'MIVMath', index: 'MIVMath', align: 'center', sortable: false },{ name: 'SIChem', index: 'SIChem', align: 'center', sortable: false },{ name: 'MIChem', index: 'MIChem', align: 'center', sortable: false },";
            colmodel = colmodel + "{ name: 'MIIChem', index: 'MIIChem', align: 'center', sortable: false },{ name: 'MIIIChem', index: 'MIIIChem', align: 'center', sortable: false },{ name: 'MIVChem', index: 'MIVChem', align: 'center', sortable: false },{ name: 'SIPhy', index: 'SIPhy', align: 'center', sortable: false },{ name: 'MIPhy', index: 'MIPhy', align: 'center', sortable: false },{ name: 'MIIPhy', index: 'MIIPhy', align: 'center', sortable: false },{ name: 'MIIIPhy', index: 'MIIIPhy', align: 'center', sortable: false },{ name: 'MIVPhy', index: 'MIVPhy', align: 'center', sortable: false },{ name: 'SIBio', index: 'SIBio', align: 'center', sortable: false },{ name: 'MIBio', index: 'MIBio', align: 'center', sortable: false },{ name: 'MIIBio', index: 'MIIBio', align: 'center', sortable: false },{ name: 'MIIIBio', index: 'MIIIBio', align: 'center', sortable: false },";
            colmodel = colmodel + "{ name: 'MIBio', index: 'MIVBio', align: 'center', sortable: false },{ name: 'SIEco', index: 'SIEco', align: 'center', sortable: false },{ name: 'MIEco', index: 'MIEco', align: 'center', sortable: false },{ name: 'MIIEco', index: 'MIIEco', align: 'center', sortable: false },{ name: 'MIIIEco', index: 'MIIIEco', align: 'center', sortable: false },{ name: 'MIVEco', index: 'MIVEco', align: 'center', sortable: false },{ name: 'SIICT', index: 'SIICT', align: 'center', sortable: false },{ name: 'MIICT', index: 'MIICT', align: 'center', sortable: false },{ name: 'MIIICT', index: 'MIIICT', align: 'center', sortable: false },{ name: 'MIIIICT', index: 'MIIIICT', align: 'center', sortable: false },{ name: 'MIVICT', index: 'MIVICT', align: 'center', sortable: false },{ name: 'SITotal', index: 'SITotal', align: 'center', sortable: false },";
            colmodel=colmodel+"{ name: 'MITotal', index: 'MITotal', align: 'center', sortable: false },{ name: 'MIITotal', index: 'MIITotal', align: 'center', sortable: false },{ name: 'MIIITotal', index: 'MIIITotal', align: 'center', sortable: false },{ name: 'MIVTotal', index: 'MIVTotal', align: 'center', sortable: false },{ name: 'SIAvg', index: 'SIAvg', align: 'center', sortable: false },{ name: 'MIAvg', index: 'MIAvg', align: 'center' },{ name: 'MIIAvg', index: 'MIIAvg', align: 'center' },{ name: 'MIIIAvg', index: 'MIIIAvg', align: 'center' },{ name: 'MIVAvg', index: 'MIVAvg', align: 'center' },{ name: 'SIGrd', index: 'SIGrd', align: 'center' },{ name: 'MIGrd', index: 'MIGrd', align: 'center' },{ name: 'MIIGrd', index: 'MIIGrd', align: 'center' },{ name: 'MIIIGrd', index: 'MIIIGrd', align: 'center' },{ name: 'MIVGrd', index: 'MIVGrd', align: 'center' }]";
        }
        columnName = eval(colnames);
        columnModel = eval(colmodel);
        $(grid_selector).jqGrid({
            url: "/Assess360/ComparitiveSAReportJqGrid",
            postData: { Name: name, Campus: campus, Grade: grade, Section: section, IsCombinedSci: isCombinedSci },
            datatype: 'json',
            type: 'GET',
            autowidth: true,
            height: 250,
            colNames: columnName,
            colModel: columnModel,
            rowNum: 50,
            shrinkToFit: false,
            rowList: [50, 100, 150, 200],
            viewrecords: true,
            sortname: 'Name',
            sortorder: 'Desc',
            pager: pager_selector,
            caption: '<i class="fa fa-th-list">&nbsp;</i>Summative Test Report',
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(window).triggerHandler('resize.jqGrid');
            }
        });
        jQuery(grid_selector).navGrid(pager_selector, { add: false, edit: false, del: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green', search: false });
        $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: "&nbsp;&nbsp;<i class='fa fa-file-excel-o'>&nbsp;</i><u>Export To Excel</u>",
            onClickButton: function () {
                window.open("ComparitiveSAReportJqGrid" + '?Campus=' + $('#Campus').val() + '&Grade=' + $('#Grade').val() + '&Section=' + $('#Section').val() + '&IsCombinedSci=' + $('#IsCombinedSci').val()  +'&rows=9999&sidx=&sord=Desc' + '&ExptXl=1');
            }
        });
        if (grade == "X" && isCombinedSci == true) {
            jQuery(grid_selector).jqGrid('setGroupHeaders', {
                useColSpanStyle: true,
                groupHeaders: [
                 { startColumnName: 'SIEng', numberOfColumns: 5, titleText: '<em>English<em>' },
                 { startColumnName: 'SILang', numberOfColumns: 5, titleText: '<em>Language</em>' },
                 { startColumnName: 'SIComSci', numberOfColumns: 5, titleText: '<em>Combined Science</em>' },
                 { startColumnName: 'SIMath', numberOfColumns: 5, titleText: '<em>Math</em>' },
                 { startColumnName: 'SIEco', numberOfColumns: 5, titleText: '<em>Economics</em>' },
                 { startColumnName: 'SIPhyEdu', numberOfColumns: 5, titleText: '<em>Physical Education</em>' },
                 { startColumnName: 'SIICT', numberOfColumns: 5, titleText: '<em>ICT</em>' },
                 { startColumnName: 'SITotal', numberOfColumns: 5, titleText: '<em>Total</em>' },
                 { startColumnName: 'SIAvg', numberOfColumns: 5, titleText: '<em>Average</em>' },
                 { startColumnName: 'SIGrd', numberOfColumns: 5, titleText: '<em>Grade</em>' },
                ]
            });
        }
        else {
            jQuery(grid_selector).jqGrid('setGroupHeaders', {
                useColSpanStyle: true,
                groupHeaders: [
                 { startColumnName: 'SIEng', numberOfColumns: 5, titleText: '<em>English<em>' },
                 { startColumnName: 'SILang', numberOfColumns: 5, titleText: '<em>Language</em>' },
                 { startColumnName: 'SIMath', numberOfColumns: 5, titleText: '<em>Math</em>' },
                 { startColumnName: 'SIChem', numberOfColumns: 5, titleText: '<em>Chemistry</em>' },
                 { startColumnName: 'SIPhy', numberOfColumns: 5, titleText: '<em>Physics</em>' },
                 { startColumnName: 'SIBio', numberOfColumns: 5, titleText: '<em>Biology</em>' },
                 { startColumnName: 'SIEco', numberOfColumns: 5, titleText: '<em>Economics</em>' },
                 { startColumnName: 'SIICT', numberOfColumns: 5, titleText: '<em>ICT</em>' },
                 { startColumnName: 'SITotal', numberOfColumns: 5, titleText: '<em>Total</em>' },
                 { startColumnName: 'SIAvg', numberOfColumns: 5, titleText: '<em>Average</em>' },
                 { startColumnName: 'SIGrd', numberOfColumns: 5, titleText: '<em>Grade</em>' },
                ]
            });
        }
    }
    //replace icons with FontAwesome icons like above
    function updatePagerIcons(table) {
        var replacement =
            {
                'ui-icon-seek-first': 'ace-icon fa fa-angle-double-left bigger-140',
                'ui-icon-seek-prev': 'ace-icon fa fa-angle-left bigger-140',
                'ui-icon-seek-next': 'ace-icon fa fa-angle-right bigger-140',
                'ui-icon-seek-end': 'ace-icon fa fa-angle-double-right bigger-140'
            };
        $('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
            var icon = $(this);
            var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

            if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
        })
    }
});


function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}



/*enter key press event*/
function defaultFunc(e) {
    if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
        $('#btnSearch').click();
        return false;
    }
    return true;
};