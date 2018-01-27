var grid_selector = "#DetailedAdmissionReportJqGrid";
var pager_selector = "#DetailedAdmissionReportJqGridPager";


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
$(function () {
    $(grid_selector).jqGrid({
        url: '/StudentsReport/DetailedAdmissionReportJqGrid',
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Campus', 'Declined', 'Deleted', 'Discontinued', 'In Active', 'New Enquiry', 'New Registration', 'Not Interested', 'Not Joined', 'Registered', 'Sent For Approval', 'Sent For Clearance'],
        colModel: [
                { name: 'Id', index: 'Id', hidden: true },
                { name: 'Campus', index: 'Campus' },
                { name: 'DeclinedCnt', index: 'DeclinedCnt' },
                { name: 'DeletedCnt', index: 'DeletedCnt' },
                { name: 'DiscontinuedCnt', index: 'DiscontinuedCnt' },
                { name: 'InactiveCnt', index: 'InactiveCnt' },
                { name: 'NewEnquiryCnt', index: 'NewEnquiryCnt' },
                { name: 'NewRegistrationCnt', index: 'NewRegistrationCnt' },
                { name: 'NotInterestedCnt', index: 'NotInterestedCnt' },
                { name: 'NotJoinedCnt', index: 'NotJoinedCnt' },
                { name: 'RegisteredCnt', index: 'RegisteredCnt' },
                { name: 'SentForApprovalCnt', index: 'SentForApprovalCnt' },
                { name: 'SentForClearanceCnt', index: 'SentForClearanceCnt' }
            ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [10, 50, 100, 500],
        sortname: 'Id',
        sortorder: 'Asc',
        reloadAfterSubmit: true,
        autowidth: true,
        height: 220,
        viewrecords: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="ace-icon fa fa-files-o bigger-130"></i> Detailed Admission Report'
    });
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

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export Excel", buttonicon: "ui-icon-print",
        onClickButton: function () {
            var Campus = $('#ddlCampus').val();
            var AcademicYear = $('#ddlAcademicYear').val();
            var Criteria = $('#ddlCriteria').val();
            var FromDate = $('#FromDate').val();
            var ToDate = $('#ToDate').val();
            window.open("/StudentsReport/DetailedAdmissionReportJqGrid?ExportType=Excel"
                    + "&Campus=" + Campus
                    + "&AcademicYear=" + AcademicYear
                    + '&Criteria=' + Criteria
                    + '&FromDate=' + FromDate
                    + '&ToDate=' + ToDate
                    + '&rows=9999');
        }
    });


    $('#btnSearch').click(function () {
        var Campus = $('#ddlCampus').val();
        var AcademicYear = $('#ddlAcademicYear').val();
        var Criteria = $('#ddlCriteria').val();
        var FromDate = $('#FromDate').val();
        var ToDate = $('#ToDate').val();
        var Excel = "";
        $(grid_selector).setGridParam({
            datatype: "json",
            url: '/StudentsReport/DetailedAdmissionReportJqGrid',
            postData: { Campus: Campus, AcademicYear: AcademicYear, Criteria: Criteria, ExportType: Excel, FromDate: FromDate, ToDate: ToDate }
        }).trigger("reloadGrid");
    });
    $('#btnReset').click(function () {
        var url = $('#BackUrl').val();
        window.location.href = url;
    });


    $.getJSON("/Base/GetJsonAcademicYear",
function (getAcademicYear) {
    var ddlAcademicYear = $("#ddlAcademicYear");
    ddlAcademicYear.empty();
    ddlAcademicYear.append($('<option/>',
            {
                value: "",
                text: "--Select One--"
            }));
    $.each(getAcademicYear, function (index, itemdata) {
        ddlAcademicYear.append($('<option/>',
            {
                value: itemdata.Value,
                text: itemdata.Text
            }));
    });
});

    $.getJSON("/StudentsReport/getCampus",
function (getCampus) {
    var ddlCampus = $("#ddlCampus");
    ddlCampus.empty();
    ddlCampus.append($('<option/>',
            {
                value: "",
                text: "--Select One--"
            }));
    $.each(getCampus, function (index, itemdata) {
        ddlCampus.append($('<option/>',
            {
                value: itemdata.Value,
                text: itemdata.Text
            }));
    });
});

});
    
    $(function () {
        $("#FromDate").datepicker({
            
        });
    });
    $(function () {
        $("#ToDate").datepicker({
            
        });
    });
    

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