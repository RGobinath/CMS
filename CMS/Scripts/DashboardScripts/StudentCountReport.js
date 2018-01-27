$(window).on('resize.jqGrid', function () {
    $("#TableData").jqGrid('setGridWidth', $(".col-sm-6").width());
})
var parent_column = $("#TableData").closest('[class*="col-"]');
$(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
    if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
        //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
        setTimeout(function () {
            $("#TableData").jqGrid('setGridWidth', parent_column.width());
        }, 0);
    }
})
$(function () {
    ;
    var column = "";
    var column1 = "";
    var value = "";
    var url = "";
    var url1 = "";
    var yeartype = "";

    var year = "";
    var campus = "";
    var grade = "";

    year = $("#YearType").val();
    campus = $("#CampusType").val();
    grade = $("#GradeType").val();

    column = "['Campus', 'Grade', 'Section Count', 'Student Count', 'Occupancy', 'Available Capacity']";
    column1 = "[{ name: 'campus', index: 'campus', align: 'left'},";
    column1 = column1 + "{ name: 'Grade', index: 'Grade', align: 'left'},";
    column1 = column1 + "{ name: 'sectioncount', index: 'sectioncount', align: 'left'},";
    column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
    column1 = column1 + "{ name: 'occupancy', index: 'occupancy', align: 'left'},";
    column1 = column1 + "{ name: 'availablecapacity', index: 'availablecapacity', align: 'left'},";
    column1 = column1 + "]";

    url = "/DashBoard/GetTotalStudentCount?year=" + year + "&campus=" + campus;
    ///url = "/DashBoard/GetTotalStudentCount?year=" + year + "&campus=" + campus + "&grade=" + grade;
    ///DashBoard/GetCampusStudentCountByGrade?year=' + yeartype

    $("#btnsubmit").click(function () {
        //                //                var e = document.getElementById("ReportType");
        //                //                var reporttype = e.options[e.selectedIndex].text;

        $("#TableData").GridUnload();

        window.onload = loadgrid();

        function loadgrid() {


            var e = document.getElementById("YearType");
            var year = $("#YearType").val();

            var e = document.getElementById("CampusType");
            var campus = $("#CampusType").val();

            //                    var e = document.getElementById("GradeType");
            //                    var grade = $("#GradeType").val();

            url = "/DashBoard/GetTotalStudentCount?year=" + year + "&campus=" + campus;

            $.ajax({
                type: 'Get',
                url: '/DashBoard/TotalStudentCount?year=' + year + '&campus=' + campus,
                //url: '@Url.Content("~/DashBoard/ManagementEnquiryReport/")',
                success: function (data) {
                    var chart = new FusionCharts("../../Charts/FCF_MSLine.swf", "ChartId", "570", "400");
                    chart.setDataXML(data);
                    chart.render("EnquiryManagement");
                    if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                        processBusy.dialog('close');
                    }
                },

                async: false,
                dataType: "text"
            });



            column = "['Campus', 'Grade', 'Section Count', 'Student Count', 'Occupancy', 'Available Capacity']";
            column1 = "[{ name: 'campus', index: 'campus', align: 'left'},";
            column1 = column1 + "{ name: 'Grade', index: 'Grade', align: 'left'},";
            column1 = column1 + "{ name: 'sectioncount', index: 'sectioncount', align: 'left'},";
            column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
            column1 = column1 + "{ name: 'occupancy', index: 'occupancy', align: 'left'},";
            column1 = column1 + "{ name: 'availablecapacity', index: 'availablecapacity', align: 'left'},";
            column1 = column1 + "]";

        }

        column = eval(column);
        column1 = eval(column1);

        //                // alert(column);
        //                // alert(column1);
        //                // alert(value);
        $("#TableData").jqGrid({
            url: url,
            //postData: { SelIds: SelIds, Departments: Departments },
            datatype: 'json',
            type: 'GET',
            //shrinkToFit: false,
            colNames: column,
            colModel: column1,
            pager: '#TableDataPager',
            rowNum: '150',
            rowList: [50, 100, 150, 200],
            sortname: '',
            sortorder: '',
            //width: 120,
            height: 200,
            autowidth: true,
            multiboxonly: true,
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    styleCheckbox(table);
                    updateActionIcons(table);
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
            },
            viewrecords: true,

            // multiselect: true,
            caption: value + '<i class="menu-icon fa fa-pencil-square-o"></i> List'
        });
        $(window).triggerHandler('resize.jqGrid');

        $("#TableData").navGrid('#TableDataPager',
                 { 	//navbar options

                     edit: false,
                     editicon: 'ace-icon fa fa-pencil blue',
                     add: false,
                     addicon: 'ace-icon fa fa-plus-circle purple',
                     del: false,
                     delicon: 'ace-icon fa fa-trash-o red',
                     search: false,
                     searchicon: 'ace-icon fa fa-search orange',
                     refresh: true,
                     refreshicon: 'ace-icon fa fa-refresh green',
                     view: false,
                     viewicon: 'ace-icon fa fa-search-plus grey'
                 },
            {},
            {}, {}, {});

        $("#TableData").navGrid('#TableDataPager', { add: false, edit: false, del: false, search: false, refresh: false });
        jQuery("#TableData").jqGrid('navButtonAdd', '#TableDataPager', {
            caption: "<i class='fa fa-file-excel-o'></i>Export To Excel",
            onClickButton: function () {
                ;
                var CampusType = $("#CampusType").val();
                //                        var GradeType = $("#GradeType").val();
                var YearType = $("#YearType").val();

                window.open("GetTotalStudentCount" + '?year=' + YearType + '&campus=' + CampusType + '&ExportType=Excel');
            }
        });

    });


    //----------------------------------------------- For Initial Load Start---------------------------------
    column = eval(column);
    column1 = eval(column1);

    // alert(column);
    // alert(column1);
    // alert(value);
    $("#TableData").jqGrid({
        url: url,
        //postData: { SelIds: SelIds, Departments: Departments },
        datatype: 'json',
        type: 'GET',
        //shrinkToFit: true,
        colNames: column,
        colModel: column1,
        pager: '#TableDataPager',
        rowNum: '150',
        rowList: [50, 100, 150, 200],
        sortname: '',
        sortorder: '',
       // width: 120,
        height: 200,
        autowidth: true,
        viewrecords: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        // multiselect: true,
        caption: value + '<i class="menu-icon fa fa-pencil-square-o"></i> List'
    });

    $(window).triggerHandler('resize.jqGrid');

    $("#TableData").navGrid('#TableDataPager',
                 { 	//navbar options

                     edit: false,
                     editicon: 'ace-icon fa fa-pencil blue',
                     add: false,
                     addicon: 'ace-icon fa fa-plus-circle purple',
                     del: false,
                     delicon: 'ace-icon fa fa-trash-o red',
                     search: false,
                     searchicon: 'ace-icon fa fa-search orange',
                     refresh: true,
                     refreshicon: 'ace-icon fa fa-refresh green',
                     view: false,
                     viewicon: 'ace-icon fa fa-search-plus grey'
                 },
            {},
            {}, {}, {});
    jQuery("#TableData").jqGrid('navButtonAdd', '#TableDataPager', {
        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
        onClickButton: function () {
            ;
            var CampusType = $("#CampusType").val();
            var GradeType = $("#GradeType").val();
            var YearType = $("#YearType").val();

            window.open("GetTotalStudentCount" + '?year=' + YearType + '&campus=' + CampusType + '&ExportType=Excel');

        }
    });
    $('#reset').click(function () {
        var url = $('#BackUrl').val();
        window.location.href = url;
    });



    $.ajax({
        type: 'Get',
        //url: '/DashBoard/TotalStudentCount',
        url: '/DashBoard/TotalStudentCount?year=' + year + '&campus=' + campus,
        //url: '@Url.Content("~/DashBoard/ManagementEnquiryReport/")',
        success: function (data) {
            var chart = new FusionCharts("../../Charts/FCF_MSLine.swf", "ChartId", "550", "330");
            chart.setDataXML(data);
            chart.render("EnquiryManagement");
            if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                processBusy.dialog('close');
            }
        },

        async: false,
        dataType: "text"
    });


    //---------------------------- For initial load End --------------------------------------------

});

//Pager icons
function styleCheckbox(table) {
}
function updateActionIcons(table) {
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