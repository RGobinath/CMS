jQuery(function ($) {
    $('#singlecampus').hide();
    var grid_selector = "#TodayFollowupGrid";
    var pager_selector = "#TodayFollowupGridPager";
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".col-sm-8").width());
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
    var Campus = $('#Campus').val();
    campuswisechart(Campus);

    //$.ajax({
    //    type: 'Get',
    //    async: false,
    //    url: '/Admission/FollowUpCountChart?campus=' + Campus,
    //    success: function (data) {
    //        var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Pie2D.swf", "StoreMaterialsMasterChart", "400", "250");
    //        chart.setDataXML(data);
    //        chart.render("FollowUpcount");
    //    }
    //});

    $.getJSON("/StudentsReport/getCampusList",
    function (getCampusList) {
        var ddlCampus = $("#ddlCampus");
        ddlCampus.empty();
        ddlCampus.append($('<option/>', { value: "", text: "All Campus" }));
        $.each(getCampusList, function (index, itemdata) {
            if (itemdata.Value == Campus) {
                ddlCampus.append($('<option/>', { value: itemdata.Value, text: itemdata.Text, selected: "Selected" }));
            }
            else {
                ddlCampus.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
            }

        });
    });
    jQuery(grid_selector).jqGrid({
        url: '/Admission/AdmissionEnquiryFollowUpJqgrid/?Campus=' + Campus + '&flag=today',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Admission Status', 'Email Id', 'MoblieNo', 'Followup Date', 'Remarks'],
        colModel: [
             { name: 'Id', index: 'Id', editable: true, hidden: true, search: false },
             { name: 'PreRegNum', index: 'PreRegNum', hidden: true, },
             { name: 'Name', index: 'Name', width: 200 },
             { name: 'Campus', index: 'Campus', width: 80 },
             { name: 'Grade', index: 'Grade', width: 60 },
             { name: 'AdmissionStatus', index: 'AdmissionStatus', width: 120 },
             { name: 'EmailId', index: 'EmailId', width: 120 },
             { name: 'MoblieNo', index: 'MoblieNo', width: 100 },
             { name: 'FollowupDate', index: 'FollowupDate', width: 80 },
             { name: 'Remarks', index: 'Remarks', width: 60, hidden: true },
        ],
        pager: pager_selector,
        rowNum: '20',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: 200,
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

    $('#ddlCampus').change(function () {
        var ddlCampus = $('#ddlCampus').val();
        if (ddlCampus != "") {
            $('#allcampus').hide();
            $('#singlecampus').show();
            $.ajax({
                type: 'Get',
                async: false,
                url: '/Admission/FollowUpCampusCount?campus=' + ddlCampus,
                success: function (data) {
                    if (data != null) {
                        $("#Yday").text(data[0]);
                        $("#today").text(data[1]);
                        $("#tomorrow").text(data[2]);
                        //document.getElementById("Yday").textContent = data[0];
                        //document.getElementById("today").textContent = data[1];
                        //document.getElementById("tomorrow").textContent = data[2];
                    }
                }
            });
            campuswisechart(ddlCampus);
            $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Admission/AdmissionEnquiryFollowUpJqgrid',
                    postData: { Campus: ddlCampus, flag: "today" },
                    page: 1
                }).trigger("reloadGrid");
        }

        else
            window.location.href = "/Admission/AdmissionFollowUpDashBoard";
    });
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
function getdata(id1) {
    window.location.href = "/Admission/GetFormData?Id=" + id1;
    //'@ViewBag.editid' == id1;
}
function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}
function callFollowUpPage(flag, count, txt) {
    debugger;
    if (txt != "all")
        count = $('#' + flag).text();
    if (count != "" && count != "0") {
        debugger;
        var ddlCampus = $('#ddlCampus').val();
        if (flag == "tomorrow") { window.location.href = "/Admission/AdmissionFollowUp?campus=" + ddlCampus + '&flag=' + flag; }
        else if (flag == "Yday") { window.location.href = "/Admission/AdmissionFollowUp?campus=" + ddlCampus + '&flag=' + flag; }
        else { return false; }
    }
    else { ErrMsg("No Enquiries to FollowUp"); return false; }
}

function campuswisechart(campus) {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/Admission/FollowUpCampusWiseCountChart?campus=' + campus,
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_MSColumn2DLineDY.swf", "DashboaardIndex", "400", "350");
            chart.setDataXML(data);
            chart.render("FollowupCampuswiseChart");
        }
    });
}
