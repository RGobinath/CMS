jQuery(function ($) {
    var grid_selector = "#AdmissionFollowupGrid";
    var pager_selector = "#AdmissionFollowupGridPager";
    var Campus = $("#Campus").val();
    $.getJSON("/StudentsReport/getCampusList",
    function (getCampusList) {
        var Campusdll = $("#campusddl");
        Campusdll.empty();
        Campusdll.append($('<option/>', { value: "", text: "--Select One--" }));
        $.each(getCampusList, function (index, itemdata) {
            if (itemdata.Value == Campus) {
                Campusdll.append($('<option/>', { value: itemdata.Value, text: itemdata.Text, selected: "Selected" }));
            }
            else {
                Campusdll.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
            }

        });
    });
    gradeddl();
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
    var flag = $("#flag").val();
    jQuery(grid_selector).jqGrid({
        url: '/Admission/AdmissionEnquiryFollowUpJqgrid/?Campus=' + Campus + '&flag=' + flag,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Admission Status', 'Email Id', 'MoblieNo', 'Followup Date'],
        colModel: [
             { name: 'Id', index: 'Id', editable: true, hidden: true, search: false },
             { name: 'PreRegNum', index: 'PreRegNum', hidden: true, },
             { name: 'Name', index: 'Name' },
             { name: 'Campus', index: 'Campus' },
             { name: 'Grade', index: 'Grade' },
             { name: 'AdmissionStatus', index: 'AdmissionStatus' },
             { name: 'EmailId', index: 'EmailId', width: 120 },
             { name: 'MoblieNo', index: 'MoblieNo', width: 120 },
             { name: 'FollowupDate', index: 'FollowupDate' },
        ],
        pager: pager_selector,
        rowNum: '20',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: 300,
        //autowidth:true,
        //shrinktofit: true,
        viewrecords: true,
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        caption: 'Admission Enquiry FollowUp List',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });

    $("#btnSearch").click(function () {
        jQuery(grid_selector).clearGridData();
        var flag = $("#flag").val();
        var campus = $("#campusddl").val();
        var name = $("#txtName").val();
        var grd = $("#ddlgrade").val();
        var adstatus = $("#AdmissionStatus").val();
        if (Campus == '') {
            ErrMsg('Please select Campus', function () { $("#ddlcampus").focus(); });
            return false;
        }
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Admission/AdmissionEnquiryFollowUpJqgrid',
                    postData: { Name: name, Campus: Campus, Grade: grd, AdmissionStatus: adstatus, flag: flag },
                    page: 1
                }).trigger("reloadGrid");

    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        var flag = $("#flag").val();
        var campus = $("#campusddl").val();
        var name = $("#txtName").val();
        var grd = $("#ddlgrade").val();
        var adstatus = $("#AdmissionStatus").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Admission/AdmissionEnquiryFollowUpJqgrid',
                    postData: { Name: name, Campus: Campus, Grade: grd, AdmissionStatus: adstatus, flag: flag },
                    page: 1
                }).trigger("reloadGrid");
    });
    jQuery(grid_selector).jqGrid('navGrid', pager_selector, {
        //navbar options
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
    }, {}, {}, {}, {})

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
})
function gradeddl() {
    var campus = $("#campusddl").val();
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
                function (modelData) {
                    var select = $("#ddlgrade");
                    select.empty();
                    select.append($('<option/>', { value: "", text: "Select Grade" }));
                    $.each(modelData, function (index, itemData) {
                        select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                    });
                });
}
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
function getdata(id1) {
    window.location.href = "/Admission/GetFormData?Id=" + id1;
    //'@ViewBag.editid' == id1;
}
function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}