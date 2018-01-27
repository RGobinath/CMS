$(function () {
    //$('#btnReset').click(function () {
    //    var url = $("#BackUrl").val();
    //    window.location.href = url;
    //});
    var deviceId = ""; var FromDate = ""; var ToDate = ""; grade = "";
    var grid_selector = "#Grid";
    var pager_selector = "#Pager";
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
    //$(document).on('ajaxloadstart', function (e) {
    //    $(grid_selector).jqGrid('GridUnload');
    //    $('.ui-jqdialog').remove();
    //});

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
       // sortname: 'Total',
        sortroder: 'Desc',
        viewrecords: true,
        caption: '<i class="fa fa-list">&nbsp;</i> Biometric Report'
    });

    $("#btnSearch").click(function () {
        deviceId = $('#deviceddl').val();
        grade = $('#Grade').val();
         FromDate = $("#txtFromDate").val();
         ToDate = $("#txtToDate").val();
         if (grade!="" && deviceId!="" &&txtFromDate!=""&&txtToDate!="") {
            $(grid_selector).GridUnload();
            window.onload = loadgrid();
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
    function loadgrid() {
        if (grade == "IX") {
            colnames = "['Id','DeviceFName', 'LogDate', 'P1', 'P2', 'P3', 'P4', 'P5','P6', 'P7','P8','P9']";
            colmodel = "[{ name: 'Id', index: 'Id',hidden:true},{ name: 'DeviceFName', index: 'DeviceFName'},{ name: 'LogDate', index:'LogDate' },{ name: 'P1In', index: 'P1In' },{ name: 'P2In', index: 'P2In' },{ name: 'P3In', index: 'P3In'},{ name: 'P4In', index: 'P4In' },{ name: 'P5In', index: 'P5In'},{ name: 'P6In', index: 'P6In'},{ name: 'P7In', index: 'P7In' },{ name: 'P8In', index: 'P8In' },{ name: 'P9In', index: 'P9In' }]";
        }
        else {
            colnames = "['Id','DeviceFName', 'LogDate', 'P1', 'P2', 'P3', 'P4', 'P5','P6', 'P7']";
            colmodel = "[{ name: 'Id', index: 'Id',hidden:true},{ name: 'DeviceFName', index: 'DeviceFName'},{ name: 'LogDate', index:'LogDate' },{ name:'P1In', index:'P1In' },{ name:'P2In', index:'P2In' },{ name:'P3In', index:'P3In'},{ name:'P4In', index:'P4In' },{ name:'P5In', index:'P5In'},{ name:'P6In', index:'P6In'},{ name:'P7In', index:'P7In' }]";
        }

        columnName = eval(colnames);
        columnModel = eval(colmodel);

        $(grid_selector).jqGrid({
            url: "/BioMetricAttendance/DailyReportGradeWiseJqGrid",
            postData: { Grade:grade, deviceId: deviceId, frmDate: FromDate, toDate: ToDate },
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
            //sortname: 'Total',
            sortorder: 'Desc',
            pager: 'Pager',
            caption: '<i class="fa fa-th-list">&nbsp;</i>Biometric Report',
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
                deviceId = $('#deviceddl').val();
                grade = $('#Grade').val();
                FromDate = $("#txtFromDate").val();
                ToDate = $("#txtToDate").val();
                window.open("DailyReportGradeWiseJqGrid?Grade=" + grade + '&deviceId=' + deviceId + '&frmDate=' + FromDate + '&toDate=' + ToDate + '&rows=99999&sidx=&sord=&ExportType=Excel');
            }
        });
    }
    //replace icons with FontAwesome icons like above
    
});

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

