var grid_selector = "#jqGridStudentWiseReportList";
var pager_selector = "#jqGridStudentWiseReportListPager";

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

    $('#ddlCampus').change(function () {
        if ($('#ddlCampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        gradeddl();
    });

    $("#btnsearch").click(function () {
        $(grid_selector).clearGridData();
        var AcademicYear = $("#ddlAcademicYear").val();
        var Campus = $("#ddlCampus").val();
        var Grade = $("#ddlgrade").val();        
        var Gender = $("#ddlGender").val();
        var Name=$("#txtName").val();    
         
        $(grid_selector).setGridParam(
           {
               datatype: "json",
               url: '/Store/GetJqGridStudentWiseReportList/',

               postData: { AcademicYear: AcademicYear, Campus: Campus, Grade: Grade, Gender: Gender, Name: Name },
               page: 1
           }).trigger("reloadGrid");
    });
    
    $("#btnreset").click(function () {

        $("input[type=text], textarea, select").val("");

      
        $(grid_selector).setGridParam(
            {

                datatype: "json",
                url: '/Store/GetJqGridStudentWiseReportList/',
                postData: { AcademicYear: "", Campus: "", Grade: "", Gender: "", Name:""},
                page: 1
            }).trigger("reloadGrid");
    });
    jQuery(grid_selector).jqGrid({
        url: '/Store/GetJqGridStudentWiseReportList?StudId=' + $("#hdnstudid").val(),
        datatype: 'json',
        mtype: 'GET',
        colNames: ['StudReportId', 'Academic Year', 'Campus', 'Grade', 'StudId', 'New Id', 'Name', 'Gender','Tshirts', 'Shirts', 'Pant', 'Total Qty'],
        colModel: [
              { name: 'StudReportId', index: 'StudReportId', hidden: true },
              { name: 'AcademicYear', index: 'AcademicYear', width: 90 },
              { name: 'Campus', index: 'Campus', width: 90 },
              { name: 'Grade', index: 'Grade', width: 90 },
              { name: 'StudId', index: 'StudId', width: 90, hidden: true },
              { name: 'NewId', index: 'NewId', width: 90 },
              { name: 'Name', index: 'Name', width: 90 },
              { name: 'Gender', index: 'Gender', width: 90 },              
              { name: 'Tshirts', index: 'Tshirts', width: 90 },
              { name: 'LaptopBag', index: 'LaptopBag', width: 90 },
              { name: 'Shirt', index: 'Shirt', width: 90 },
              { name: 'TotalQty', index: 'TotalQty', width: 90 },


        ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50],
        sortname: 'StudReportId',
        sortorder: 'asc',
        height: '220',
        //width: 1225,
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="fa fa-exchange"></i> Student Wise Report',
        forceFit: true
    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    //$(grid_selector).jqGrid('filterToolbar', { searchOnEnter: false, enableClear: true })
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
        caption: "Export To Excel",
        onClickButton: function () {

            var ExptType = 'Excel';
            window.open("/Store/GetJqGridStudentWiseReportList" + '?rows=9999' + '&ExptType=' + ExptType);
            }
        });


});
    
function gradeddl() {

    var e = document.getElementById('ddlCampus');
    var campus = e.options[e.selectedIndex].value;
    //alert(campus);
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