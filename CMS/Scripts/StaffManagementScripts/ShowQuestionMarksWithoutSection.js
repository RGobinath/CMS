
$(function () {

    var subgrid_selector = "#SurveyMarkswsecListJqgrid";
    var subpager_selector = "#SurveyMarkswsecListPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(subgrid_selector).jqGrid('setGridWidth', $('#grid').width());
    })
    //resize on sidebar collapse/expand 
    var parent_column = $(subgrid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(subgrid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    //pager icon
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
    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }
    $(document).on('ajaxloadstart', function (e) {
        $(subgrid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    $(subgrid_selector).jqGrid({
    datatype: 'local',
    type: 'GET',
    colNames: ['Id', 'Academic Year', 'Campus', 'Grade', 'Survey Number', 'Survey Date', 'Staff Name', 'No. of Students Attended', 'Question Asked in Survey', 'Score in Survey', 'Average Score by Question'],
    colModel: [{ name: 'Id', index: 'Id', key: true, hidden: true },
               { name: 'AcademicYear', index: 'AcademicYear', hidden: true },
               { name: 'Campus', index: 'Campus', hidden: true },
               { name: 'Grade', index: 'Grade', hidden: true },
               { name: 'CategoryName', index: 'CategoryName', hidden: true },
               { name: 'EvaluationDate', index: 'EvaluationDate', hidden: true },
               { name: 'StaffName', index: 'StaffName', align: 'center', width: 300, hidden: true },
               { name: 'StudentCount', index: 'StudentCount', align: 'center',hidden:true },
               { name: 'StudentSurveyQuestion', index: 'StudentSurveyQuestion', width: 500 },
               { name: 'Score', index: 'Score', align: 'center', width: 90, formatoptions: { prefix: "", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' } },
               { name: 'Average', index: 'Average', align: 'center', width: 90, formatoptions: { prefix: "", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' } }

    ],
    pager: subpager_selector,
    rowNum: '50',
    rowList: [5, 10, 20, 50, 100, 150, 200],
    sortname: 'Id',
    sortorder: 'Asc',
    autowidth: true,
    shrinkToFit: true,
    height: 230,
    viewrecords: true,
    caption: '<i class="fa fa-th-list">&nbsp;</i>Staff wise Detailed Report',
    footerrow: true,
    userDataOnFooter: true,
    loadComplete: function () {
        var table = this;
        $(subgrid_selector).footerData('set', { StudentSurveyQuestion: 'Total:' });
        //var colScore = $(subgrid_selector).jqGrid('getCol', 'Score', false, 'sum');
        //$(subgrid_selector).footerData('set', { Score: colScore });
        //var colAvg = $(subgrid_selector).jqGrid('getCol', 'Average', false, 'sum');
        //$(subgrid_selector).footerData('set', { Average: colAvg });

        setTimeout(function () {
            updatePagerIcons(table);
            enableTooltips(table);
        }, 0);
        $(window).triggerHandler('resize.jqGrid');
    }
});
    $(subgrid_selector).navGrid(subpager_selector, { add: false, edit: false, del: false, search: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green' });

    $(subgrid_selector).jqGrid('navButtonAdd', subpager_selector, {
    caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
    onClickButton: function () {
        Campus = $("#Campus").val();
        Grade = $("#Grade").val();
        Section = $("#Section").val();
        preRegnum = $("#StaffPreRegNumber").val();
        AcademicYear = $("#AcademicYear").val();
        Surveyno = $("#StaffEvaluationCategoryId").val();
        window.open("/StaffManagement/ShowQuestionMarksListJqGrid?ExptXl=1&cam=" + Campus + '&gra=' + Grade + '&sect=' + Section + '&acayear=' + AcademicYear + '&preRegNum=' + preRegnum + '&SurveyId=' + Surveyno + '&rows=999999');
    }
});

});