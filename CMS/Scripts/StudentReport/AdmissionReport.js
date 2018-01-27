var grid_selector = "#AddmissionStatusReport";
var pager_selector = "#AddmissionStatusReportPager";

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

    var Year = $('#CurrYear').val();
    
    var Campus = "";
    $("#ddlyear").val(Year);
    
    AddmissionStatusCountChart(Campus);
    $(grid_selector).jqGrid({
        url: '/StudentsReport/AddmissionStatusReportJqGrid/?CurrAcadamicYear=' + Year,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Academic Year', 'New Enquiry', 'New Registration', 'Registered', 'Discontinued', 'Not Interested'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'Campus', index: 'Campus' },
              { name: 'AcademicYear', index: 'AcademicYear' },
              { name: 'NewEnquiry', index: 'NewEnquiry' },
              { name: 'NewRegistration', index: 'NewRegistration' },
              { name: 'Registered', index: 'Registered' },
              { name: 'Discontinued', index: 'Discontinued' },
              { name: 'NotInterested', index: 'NotInterested' },
              ],
        pager: pager_selector,
        //rowNum: '',
        rowList: [50, 100],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '200',

        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="fa fa-star"></i> Addmission Status wise Report',
        forceFit: true,
        gridview: true,
        footerrow: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            jQuery(grid_selector).trigger("reloadGrid");
            var $self = $(this),

                NewEnquiry = parseFloat($self.jqGrid("getCol", "NewEnquiry", false, "sum")).toFixed(2);
            NewRegistration = parseFloat($self.jqGrid("getCol", "NewRegistration", false, "sum")).toFixed(2);
            Registered = parseFloat($self.jqGrid("getCol", "Registered", false, "sum")).toFixed(2);
            Discontinued = parseFloat($self.jqGrid("getCol", "Discontinued", false, "sum")).toFixed(2);
            NotInterested = parseFloat($self.jqGrid("getCol", "NotInterested", false, "sum")).toFixed(2);
            $self.jqGrid("footerData", "set", { '': '', '': '', AcademicYear: "Total :", NewEnquiry: NewEnquiry, NewRegistration: NewRegistration, Registered: Registered, Discontinued: Discontinued, NotInterested: NotInterested });
        }
    });
    //  $("#AddmissionStatusReport").navGrid('#AddmissionStatusReportPager', { add: false, edit: false, del: false, search: false, refresh: false });
    $(grid_selector).navGrid(pager_selector,
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
        caption: "<i class='fa fa-file-excel-o'> </i> Export To Excel",
        onClickButton: function () {
            Campus = $("#Campus").val();
            Year = $("#ddlyear").val();
            window.open("AddmissionStatusReportJqGrid" + '?CurrAcadamicYear=' + Year + '&Campus=' + Campus + '&rows=9999 ' + '&ExptXl=1');
        }
    });

    $.getJSON("/Base/FillBranchCode",
    function (fillig) {
        var ddlcam = $("#Campus");
        ddlcam.empty();
        ddlcam.append($('<option/>', { value: "", text: "Select One" }));
        $.each(fillig, function (index, itemdata) { ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text })); });
    });
    //search 
    $("#btnSearch").click(function () {
        Campus = $("#Campus").val();
        Year = $("#ddlyear").val();
        if (Campus != "") {
            $(grid_selector).clearGridData();
            $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/StudentsReport/AddmissionStatusReportJqGrid/',
                    postData: { Campus: Campus, CurrAcadamicYear: Year },
                    page: 1
                }).trigger("reloadGrid");
            AddmissionStatusCountChart(Campus);
        }
        else {
            ErrMsg("Please select Campus.");
            return false;
        }
    });

//    $('#btnReset').click(function () {
//        $('#Campus').val("");
//        $('#ddlyear').val("");
//        //$('#Section').val("");
//        jQuery(grid_selector).jqGrid('clearGridData')
//.jqGrid('setGridParam', { data: data, page: 1 })
//.trigger('reloadGrid');
    //    });
    $('#btnReset').click(function () {
        var url = $('#BackUrl').val();
        window.location.href = url;
   });
//    $("#btnReset").click(function () {
//        window.location.href = '"AdmissionReport", "StudentsReport"';
//    });
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

function AddmissionStatusCountChart(campus) {
    //var Campus = ""; //$('#ddlCampusList').val();
    $.ajax({
        type: 'Get',
        async: false,
        url: '/StudentsReport/GetAcademicYearWiseReportChart/?Campus=' + campus,
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_MSColumn2D.swf", "Example", "490", "320");
            chart.setDataXML(data);
            chart.render("MultiSeriesChart");
        }
    });
}
    