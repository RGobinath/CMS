function FillCampusDll() {
    $.getJSON("/Base/FillCampus",
      function (fillbc) {
          var ddlbc = $("#ddlCampus");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
          });
      });
}
jQuery(function ($) {
    FillCampusDll();
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";
    //FillYear();
    //FillMonth();
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        jQuery(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    jQuery(grid_selector).jqGrid({
        url: '/StaffManagement/StaffandDriverListJqgrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['Id', 'PreRegNum', 'Name', 'Id Number', 'Campus', 'DOB', 'Gender', 'Phone No.', 'Status'],
        colModel: [
                      { name: 'Id', index: 'Id', hidden: true, key: true },
                      { name: 'PreRegNum', index: 'PreRegNum', sortable: false },
                      { name: 'Name', index: 'Name' },
                      { name: 'IdNumber', index: 'IdNumber', sortable: false },
                      { name: 'Campus', index: 'Campus' },
                      { name: 'DOB', index: 'DOB' },
                      { name: 'Gender', index: 'Gender' },
                      { name: 'PhoneNo', index: 'PhoneNo' },
                      { name: 'Status', index: 'Status' },
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        altRows: true,
        sortname: 'Id',
        sortorder: 'Asc',
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Print Id Card Details",
    })
    //$(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size    
    //navButtons
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
            {
            }, //Edit
            {
            }, //Add
              {
              },//Delete
               {},
                {})
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffandDriverListJqgrid',
           postData: { Type: "", Campus: "", Name: "", IdNumber: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        var Type = $("#ddlType").val();
        var Campus = $("#ddlCampus").val();
        var Name = $("#txtName").val();
        var IdNumber = $("#txtIdNumber").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/StaffManagement/StaffandDriverListJqgrid',
           postData: { Type: Type, Campus: Campus, Name: Name, IdNumber: IdNumber },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#IdCard").click(function () {
        debugger;
        var Type = $("#ddlType").val();
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        if (Type == "Staff") {
            if (GridIdList == "") { ErrMsg("Please select Staff List "); return false; }
        }
        else if (Type == "Driver") {
            if (GridIdList == "") { ErrMsg("Please select Driver List "); return false; }
        }
        else {
            if (GridIdList == "") { ErrMsg("Please select List "); return false; }
        }
        var rowData = [];
        var rowData1 = [];
        var MainrowData1 = "";
        //  alert(GridIdList.length);
        if (GridIdList.length > 0) {

            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                rowData1[i] = rowData[i].PreRegNum;
                if (MainrowData1 != "") {
                    MainrowData1 = MainrowData1 + ',' + rowData1[i];
                }
                else {
                    MainrowData1 = rowData1[i];
                }
            }
            if (Type == "Staff") {
                window.location.href = "/StaffManagement/StaffIdCard?ReqNo=" + MainrowData1; // rowData1;
            }
            else {
                window.location.href = "/Transport/PrintIdCard?PreRegNo=" + MainrowData1;
            }
        }
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

    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }
});
