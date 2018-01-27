$(function () {

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
$(window).on('resize.jqGrid', function () {
    $("#TableData1").jqGrid('setGridWidth', $(".col-sm-6").width());
})
var parent_column = $("#TableData1").closest('[class*="col-"]');
$(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
    if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
        //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
        setTimeout(function () {
            $("#TableData1").jqGrid('setGridWidth', parent_column.width());
        }, 0);
    }
})


    $("#CampusType").change(function () {
        gradeddl();
    });

    
    var column = "";
    var column1 = "";
    var value = "";
    var url = "";
    var url1 = "";
    var yeartype = "";

    column = "['Campus Name', 'Student Count', 'Occupancy', 'Occupancy %']";
    column1 = "[{ name: 'Campus', index: 'Campus', align: 'left'},";
    column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
    column1 = column1 + "{ name: 'occupancy', index: 'occupancy', align: 'left'},";
    column1 = column1 + "{ name: 'percentage', index: 'percentage', align: 'left'},";
    column1 = column1 + "]";
    value = "Campus";
    yeartype = $("#YearList").val();
    url1 = "/DashBoard/GetCampusStudentCountByGrade?year=" + yeartype;
    ///DashBoard/GetCampusStudentCountByGrade?year=' + yeartype

    $("#btnsubmit").click(function () {
        ;
        //                var e = document.getElementById("ReportType");
        //                var reporttype = e.options[e.selectedIndex].text;

        $("#TableData").GridUnload();
        $("#TableData1").GridUnload();
        window.onload = loadgrid();

        function loadgrid() {

            var e = document.getElementById("CampusType");
            var campustype = e.options[e.selectedIndex].text;

            var e = document.getElementById("GradeType");
            var gradetype = e.options[e.selectedIndex].text;

            var e = document.getElementById("YearList");
            yeartype = e.options[e.selectedIndex].text;



            if (campustype != "All Campus" && gradetype != "All Grade") {
                column = "['Section', 'Student Count', 'Occupancy', 'Occupancy %']";
                column1 = "[{ name: 'section', index: 'section', align: 'left'},";
                column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
                column1 = column1 + "{ name: 'occupancy', index: 'occupancy', align: 'left'},";
                column1 = column1 + "{ name: 'percentage', index: 'percentage', align: 'left'},";
                column1 = column1 + "]";
                value = "Section";
                url = "/DashBoard/GetSectionStudentCount?campus=" + campustype + "&grade=" + gradetype + "&year=" + yeartype;

                $.ajax({
                    type: 'Get',
                    url: '/DashBoard/StudentBySection?campus=' + campustype + '&grade=' + gradetype + '&year=' + yeartype,
                    //url: '@Url.Content("~/DashBoard/ManagementEnquiryReport/")',
                    success: function (data) {
                        var chart = new FusionCharts("../../Charts/FCF_MSLine.swf", "ChartId", "550", "350");
                        chart.setDataXML(data);
                        chart.render("EnquiryManagement");
                        if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                            processBusy.dialog('close');
                        }
                    },

                    async: false,
                    dataType: "text"
                });
                $("#GradeStudentCount").hide();
                $("#box").hide();
                $("#TableDataPager1").hide();
            }
            else if (campustype != "All Campus") {

                column = "['Grade', 'SpainGrade', 'Student Count', 'Occupancy', 'Occupancy %']";
                column1 = "[{ name: 'grade', index: 'grade', align: 'left'},";
                column1 = column1 + "{ name: 'spaingrade', index: 'spaingrade', align: 'left'},";
                column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
                column1 = column1 + "{ name: 'occupancy', index: 'occupancy', align: 'left'},";
                column1 = column1 + "{ name: 'percentage', index: 'percentage', align: 'left'},";
                column1 = column1 + "]";
                value = "Grade";
                url = "/DashBoard/GetGradeStudentCount?campus=" + campustype + "&year=" + yeartype;
                $.ajax({
                    type: 'Get',
                    url: '/DashBoard/StudentByGrade?campus=' + campustype + '&year=' + yeartype,
                    //url: '@Url.Content("~/DashBoard/ManagementEnquiryReport/")',
                    success: function (data) {
                        var chart = new FusionCharts("../../Charts/FCF_MSLine.swf", "ChartId", "550", "350");
                        chart.setDataXML(data);
                        chart.render("EnquiryManagement");
                        if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                            processBusy.dialog('close');
                        }
                    },

                    async: false,
                    dataType: "text"
                });

                $.ajax({
                    type: 'Get',
                    url: '/DashBoard/StudentByCampusAllSection?campus=' + campustype + '&year=' + yeartype,
                    //url: '@Url.Content("~/DashBoard/ManagementEnquiryReport/")',
                    success: function (data) {
                        var chart = new FusionCharts("../../Charts/FCF_MSLine.swf", "ChartId", "550", "350");
                        chart.setDataXML(data);
                        chart.render("GradeStudentCount");
                        if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                            processBusy.dialog('close');
                        }
                    },

                    async: false,
                    dataType: "text"
                });
                $("#GradeStudentCount").hide();
                $("#box").hide();
                $("#TableDataPager1").hide();
            }
            else {
                ;

                column = "['Campus Name', 'Student Count', 'Occupancy', 'Occupancy %']";
                column1 = "[{ name: 'Campus', index: 'Campus', align: 'left'},";
                column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
                column1 = column1 + "{ name: 'occupancy', index: 'occupancy', align: 'left'},";
                column1 = column1 + "{ name: 'percentage', index: 'percentage', align: 'left'},";
                column1 = column1 + "]";
                value = "Campus";
                url = "/DashBoard/GetCampusStudentCount?year=" + yeartype;
                url1 = "/DashBoard/GetCampusStudentCountByGrade?year=" + yeartype;
                $.ajax({
                    type: 'Get',
                    url: '/DashBoard/StudentByCampus?year=' + yeartype,
                    //url: '@Url.Content("~/DashBoard/ManagementEnquiryReport/")',
                    success: function (data) {
                        var chart = new FusionCharts("../../Charts/FCF_MSLine.swf", "ChartId", "550", "350");
                        chart.setDataXML(data);
                        chart.render("EnquiryManagement");
                        if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                            processBusy.dialog('close');
                        }
                    },

                    async: false,
                    dataType: "text"
                });

                $.ajax({
                    type: 'Get',
                    url: '/DashBoard/StudentByCampusAllgrade?year=' + yeartype,
                    //url: '@Url.Content("~/DashBoard/ManagementEnquiryReport/")',
                    success: function (data) {
                        var chart = new FusionCharts("../../Charts/FCF_MSLine.swf", "ChartId", "550", "350");
                        chart.setDataXML(data);
                        chart.render("GradeStudentCount");
                        if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                            processBusy.dialog('close');
                        }
                    },

                    async: false,
                    dataType: "text"
                });

                $("#TableData1").jqGrid({
                    // url: '/DashBoard/GetCampusStudentCountByGrade?year=' + yeartype,
                    url: url1,
                    //postData: { SelIds: SelIds, Departments: Departments },
                    datatype: 'json',
                    type: 'GET',
                    shrinkToFit: true,
                    colNames: ['Grade', 'Student Count', 'Occupancy', 'Occupancy %'],
                    colModel: [{ name: 'grade', index: 'grade', align: 'left' },
                            { name: 'studentcount', index: 'studentcount', align: 'left' },
                            { name: 'occupancy', index: 'occupancy', align: 'left' },
                            { name: 'percentage', index: 'percentage', align: 'left' },
                            ],
                    pager: '#TableDataPager1',
                    rowNum: '50',
                    rowList: [5, 10, 20, 50],
                    sortname: '',
                    sortorder: '',
                    //width: 120,
                    height: 180,
                    autowidth: true,
                    viewrecords: true,
                    multiboxonly: true,
                    loadComplete: function () {
                        var table = this;
                        setTimeout(function () {
                            updatePagerIcons(table);
                        }, 0);
                    },
                    // multiselect: true,
                    caption: '<i class="fa fa-list-alt"></i>' + value + ' List',
                });
                $("#TableData1").navGrid('#TableDataPager1',
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

                jQuery("#TableData1").jqGrid('navButtonAdd', '#TableDataPager1', {
                    caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
                    onClickButton: function () {
                        ;
                        var CampusType = $("#CampusType").val();
                        var GradeType = $("#GradeType").val();
                        var YearType = $("#YearList").val();
                        window.open("GetCampusStudentCountByGrade" + '?ExportType=Excel&year=' + YearType);
                    }
                });


                $("#GradeStudentCount").show();
                $("#box").show();
                $("#TableDataPager1").show();
            }

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
                shrinkToFit: true,
                colNames: column,
                colModel: column1,
                pager: '#TableDataPager',
                rowNum: '50',
                rowList: [5, 10, 20, 50],
                sortname: '',
                sortorder: '',
                //width: 120,
                height: 180,
                autowidth: true,
                viewrecords: true,
                multiboxonly: true,
                loadComplete: function () {
                    var table = this;
                    setTimeout(function () {
                        updatePagerIcons(table);
                    }, 0);
                },
                // multiselect: true,
                caption:'<i class="fa fa-list-alt"></i>' +  value + ' List'
            });
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
                caption: "<i class='fa fa-file-excel-o'> </i>Export To Excel",
                onClickButton: function () {
                    ;
                    var CampusType = $("#CampusType").val();
                    var GradeType = $("#GradeType").val();
                    var YearType = $("#YearList").val();
                    if (CampusType != "All" && GradeType != "All") {
                        window.open("GetSectionStudentCount" + '?campus=' + CampusType + '&grade=' + GradeType + '&ExportType=Excel&year=' + YearType);
                    }
                    else if (CampusType != "All") {
                        window.open("GetGradeStudentCount" + '?campus=' + CampusType + '&ExportType=Excel&year=' + YearType);
                    }
                    else {
                        window.open("GetCampusStudentCount" + '?ExportType=Excel&year=' + YearType);
                    }
                }
            });

        }

    });



    //----------------------------------------------- For Initial Load Start---------------------------------
    column = eval(column);
    column1 = eval(column1);

    // alert(column);
    // alert(column1);
    // alert(value);
    $("#TableData").jqGrid({
        url: '/DashBoard/Get' + value + 'StudentCount?year=' + yeartype,
        //postData: { SelIds: SelIds, Departments: Departments },
        datatype: 'json',
        type: 'GET',
        shrinkToFit: true,
        colNames: column,
        colModel: column1,
        pager: '#TableDataPager',
        rowNum: '50',
        rowList: [5, 10, 20, 50],
        sortname: '',
        sortorder: '',
        //width: 120,
        height: 180,
        //autowidth: true,
        viewrecords: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
            }, 0);
        },
        // multiselect: true,
        caption:'<i class="fa fa-list-alt"></i>' +  value + ' List'
    });
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
            var YearType = $("#YearList").val();
            if (CampusType != "All" && GradeType != "All") {
                window.open("GetSectionStudentCount" + '?CampusType=' + CampusType + '&GradeType=' + GradeType + '&ExportType=Excel&year=' + YearType);
            }
            else if (CampusType != "All") {
                window.open("GetGradeStudentCount" + '?CampusType=' + CampusType + '&ExportType=Excel&year=' + YearType);
            }
            else {
                window.open("GetCampusStudentCount" + '?ExportType=Excel&year=' + YearType);
            }
        }
    });
    $('#reset').click(function () {
        var url = $('#BackUrl').val();
        window.location.href = url;
    });


    $.ajax({
        type: 'Get',
        url: '/DashBoard/StudentByCampus?year=' + yeartype,
        //url: '@Url.Content("~/DashBoard/ManagementEnquiryReport/")',
        success: function (data) {
            var chart = new FusionCharts("../../Charts/FCF_MSLine.swf", "ChartId", "550", "350");
            chart.setDataXML(data);
            chart.render("EnquiryManagement");
            if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                processBusy.dialog('close');
            }
        },

        async: false,
        dataType: "text"
    });

    $("#TableData1").jqGrid({
        // url: '/DashBoard/GetCampusStudentCountByGrade?year=' + yeartype,
        url: url1,
        //postData: { SelIds: SelIds, Departments: Departments },
        datatype: 'json',
        type: 'GET',
        shrinkToFit: true,
        colNames: ['Grade', 'SpainGrade', 'Student Count', 'Occupancy', 'Occupancy %'],
        colModel: [{ name: 'grade', index: 'grade', align: 'left' },
                            { name: 'spaingrade', index: 'spaingrade', align: 'left' },
                            { name: 'studentcount', index: 'studentcount', align: 'left' },
                            { name: 'occupancy', index: 'occupancy', align: 'left' },
                            { name: 'percentage', index: 'percentage', align: 'left' },
                            ],
        pager: '#TableDataPager1',
        rowNum: '50',
        rowList: [5, 10, 20, 50],
        sortname: '',
        sortorder: '',
        // width: 120,
        height: 180,
        autowidth: true,
        viewrecords: true,
        // multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
            }, 0);
        },
        caption:'<i class="fa fa-list-alt"></i>' +   value + ' List'
    });
    $("#TableData1").navGrid('#TableDataPager1',
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

    jQuery("#TableData1").jqGrid('navButtonAdd', '#TableDataPager1', {
        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
        onClickButton: function () {
            ;
            var CampusType = $("#CampusType").val();
            var GradeType = $("#GradeType").val();
            var YearType = $("#YearList").val();
            window.open("GetCampusStudentCountByGrade" + '?ExportType=Excel&year=' + YearType);
        }
    });



    $.ajax({
        type: 'Get',
        url: '/DashBoard/StudentByCampusAllgrade?year=' + yeartype,
        //url: '@Url.Content("~/DashBoard/ManagementEnquiryReport/")',
        success: function (data) {
            var chart = new FusionCharts("../../Charts/FCF_MSLine.swf", "ChartId", "575", "350");
            chart.setDataXML(data);
            chart.render("GradeStudentCount");
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
function gradeddl() {
    //var e = document.getElementById('CampusType');
    var campus = $('#CampusType').val(); // e.options[e.selectedIndex].value;
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
            function (modelData) {
                var select = $("#GradeType");
                select.empty();
                select.append($('<option/>', {value: "All",text: "All Grade"}));
                $.each(modelData, function (index, itemData) {
                    select.append($('<option/>',{value: itemData.gradcod,text: itemData.gradcod}));
                });
            });
}    