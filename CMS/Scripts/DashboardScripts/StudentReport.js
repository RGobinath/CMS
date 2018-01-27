var grid_selector = "#TableData";
var pager_selector = "#TableDataPager";

$(window).on('resize.jqGrid', function () {
    $(grid_selector).jqGrid('setGridWidth', $(".col-sm-6").width());
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
$(function () {

    $("#CampusType").change(function () {
        gradeddl();
    });

    function gradeddl() {
        var e = document.getElementById('CampusType');
        var campus = e.options[e.selectedIndex].value;
        $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
            function (modelData) {
                var select = $("#GradeType");
                select.empty();
                select.append($('<option/>', {value: "",text: "All Grade"}));
                $.each(modelData, function (index, itemData) {
                    select.append($('<option/>',{value: itemData.gradcod,text: itemData.gradcod}));
                });
            });
    }
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

    column = "['Year', 'Student Count']";
    column1 = "[{ name: 'Year', index: 'Year', align: 'left'},";
    column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
    column1 = column1 + "]";

    url = "/DashBoard/GetStudentCount?year=" + year + "&campus=" + campus + "&grade=" + grade;
    ///DashBoard/GetCampusStudentCountByGrade?year=' + yeartype

    $("#btnsubmit").click(function () {
        //                var e = document.getElementById("ReportType");
        //                var reporttype = e.options[e.selectedIndex].text;

        $(grid_selector).GridUnload();

        window.onload = loadgrid();

        function loadgrid() {
            var e = document.getElementById("YearType");
            var year = $("#YearType").val();
            var e = document.getElementById("CampusType");
            var campus = $("#CampusType").val();
            var e = document.getElementById("GradeType");
            var grade = $("#GradeType").val();
            url = "/DashBoard/GetStudentCount?year=" + year + "&campus=" + campus + "&grade=" + grade;
            $.ajax({
                type: 'Get',
                url: '/DashBoard/StudentCount?year=' + year + '&campus=' + campus + '&grade=' + grade,
                //url: '@Url.Content("~/DashBoard/ManagementEnquiryReport/")',
                success: function (data) {
                    var chart = new FusionCharts("../../Charts/FCF_Line.swf", "ChartId", "550", "350");
                    chart.setDataXML(data);
                    chart.render("EnquiryManagement");
                    if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                        processBusy.dialog('close');
                    }
                },
                async: false,
                dataType: "text"
            });
            if (year != 'All' && campus != '' && grade != '') {
                column = "['Year', 'Month', 'Campus' , 'Grade' , 'Student Count']";
                column1 = "[{ name: 'Year', index: 'Year', align: 'left'},";
                column1 = column1 + "{ name: 'period', index: 'period', align: 'left'},";
                column1 = column1 + "{ name: 'campus', index: 'campus', align: 'left'},";
                column1 = column1 + "{ name: 'grade', index: 'grade', align: 'left'},";
                column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
                column1 = column1 + "]";
            }
            else if (year != 'All' && campus != '' && grade == '') {
                column = "['Year', 'Month', 'Campus' , 'Student Count']";
                column1 = "[{ name: 'Year', index: 'Year', align: 'left'},";
                column1 = column1 + "{ name: 'period', index: 'period', align: 'left'},";
                column1 = column1 + "{ name: 'campus', index: 'campus', align: 'left'},";
                column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
                column1 = column1 + "]";
            }
            else if (year != 'All' && campus == '' && grade != '') {
                column = "['Year', 'Month', 'Grade' , 'Student Count']";
                column1 = "[{ name: 'Year', index: 'Year', align: 'left'},";
                column1 = column1 + "{ name: 'period', index: 'period', align: 'left'},";
                column1 = column1 + "{ name: 'grade', index: 'grade', align: 'left'},";
                column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
                column1 = column1 + "]";
            }
            else if (year != 'All') {
                column = "['Year', 'Month', 'Student Count']";
                column1 = "[{ name: 'Year', index: 'Year', align: 'left'},";
                column1 = column1 + "{ name: 'period', index: 'period', align: 'left'},";
                column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
                column1 = column1 + "]";
            }
            else {
                if (year == 'All' && campus == '' && grade == '') {
                    column = "['Year', 'Student Count']";
                    column1 = "[{ name: 'Year', index: 'Year', align: 'left'},";
                    column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
                    column1 = column1 + "]";
                }
                else if (year == 'All' && campus != '' && grade == '') {
                    column = "['Year', 'Campus', 'Student Count']";
                    column1 = "[{ name: 'Year', index: 'Year', align: 'left'},";
                    column1 = column1 + "{ name: 'campus', index: 'campus', align: 'left'},";
                    column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
                    column1 = column1 + "]";
                }
                else if (year == 'All' && campus == '' && grade != '') {
                    column = "['Year', 'Grade', 'Student Count']";
                    column1 = "[{ name: 'Year', index: 'Year', align: 'left'},";
                    column1 = column1 + "{ name: 'grade', index: 'grade', align: 'left'},";
                    column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
                    column1 = column1 + "]";
                }
                else if (year == 'All' && campus != '' && grade != '') {
                    column = "['Year', 'Campus', 'Grade', 'Student Count']";
                    column1 = "[{ name: 'Year', index: 'Year', align: 'left'},";
                    column1 = column1 + "{ name: 'campus', index: 'campus', align: 'left'},";
                    column1 = column1 + "{ name: 'grade', index: 'grade', align: 'left'},";
                    column1 = column1 + "{ name: 'studentcount', index: 'studentcount', align: 'left'},";
                    column1 = column1 + "]";
                }
            }
        }
        column = eval(column);
        column1 = eval(column1);
        $(grid_selector).jqGrid({
            url: url,
            //postData: { SelIds: SelIds, Departments: Departments },
            datatype: 'json',
            type: 'GET',
            shrinkToFit: false,
            colNames: column,
            colModel: column1,
            pager: pager_selector,
            rowNum: '50',
            rowList: [5, 10, 20, 50],
            sortname: '',
            sortorder: '',
            height: 230,
            autowidth: true,
            viewrecords: true,
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    styleCheckbox(table);
                    updateActionIcons(table);
                    updatePagerIcons(table);
                    //enableTooltips(table);
                }, 0);
            },
            // multiselect: true,
            caption: '<i class="fa fa-list"></i>' + value + ' List'
        });

        $(window).triggerHandler('resize.jqGrid');
        jQuery(grid_selector).jqGrid('navGrid', pager_selector,
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

        jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
            onClickButton: function () {
                var CampusType = $("#CampusType").val();
                var GradeType = $("#GradeType").val();
                var YearType = $("#YearType").val();
                window.open("GetStudentCount" + '?year=' + YearType + '&campus=' + CampusType + '&grade=' + GradeType + '&ExportType=Excel');
            }
        });

    });


    //----------------------------------------------- For Initial Load Start---------------------------------
    column = eval(column);
    column1 = eval(column1);

    // alert(column);
    // alert(column1);
    // alert(value);
    $(grid_selector).jqGrid({
        url: url,
        //postData: { SelIds: SelIds, Departments: Departments },
        datatype: 'json',
        type: 'GET',
        shrinkToFit: false,
        colNames: column,
        colModel: column1,
        pager: pager_selector,
        rowNum: '50',
        rowList: [5, 10, 20, 50],
        sortname: '',
        sortorder: '',
        height: 230,
        autowidth: true,
        viewrecords: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                //enableTooltips(table);
            }, 0);
        },
        // multiselect: true,
        caption: '<i class="fa fa-list"></i>' + value + ' List'
    });


    $(window).triggerHandler('resize.jqGrid');
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
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
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            ;
            var CampusType = $("#CampusType").val();
            var GradeType = $("#GradeType").val();
            var YearType = $("#YearType").val();

            window.open("GetStudentCount" + '?year=' + YearType + '&campus=' + CampusType + '&grade=' + GradeType + '&ExportType=Excel');

        }
    });



    $.ajax({
        type: 'Get',
        url: '/DashBoard/StudentCount?year=' + year + '&campus=' + campus + '&grade=' + grade,
        //url: '@Url.Content("~/DashBoard/ManagementEnquiryReport/")',
        success: function (data) {
            var chart = new FusionCharts("../../Charts/FCF_Line.swf", "ChartId", "550", "350");
            chart.setDataXML(data);
            chart.render("EnquiryManagement");
            if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                processBusy.dialog('close');
            }
        },

        async: false,
        dataType: "text"
    });

    $('#reset').click(function () {
        var url = $('#BackUrl').val();
        window.location.href = url;
    });
    //---------------------------- For initial load End --------------------------------------------

});


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