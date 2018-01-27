function FillAssetTypeDll() {
    $.getJSON("/Asset/FillITAssetName",
      function (fillbc) {
          var ddlbc = $("#ddlAssetType");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillModelByBrand() {
    var Brand = $("#ddlMake").val();
    var ddlbc = $("#ddlModel");
    if (Brand != "" && Brand != null) {
        $.getJSON("/Asset/GetModelByBrand?Brand=" + Brand,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillCampusDll() {
    $.getJSON("/Base/FillCampus",
      function (fillbc) {
          var ddlbc = $("#ddlCampus");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillBlockByCampus() {
    debugger;
    var Campus = $("#ddlCampus option:selected").text();
    var ddlbc = $("#ddlBlock");
    if (Campus != "" && Campus != null) {
        debugger;
        $.getJSON("/Asset/GetBlockByCampus?Campus=" + Campus,
          function (fillbc) {
              debugger;
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillLocationByCampusWithBlock() {
    debugger;
    var Campus = $("#ddlCampus option:selected").text();
    var Block = $("#ddlBlock").val();
    var ddlbc = $("#ddlLocation");
    if (Campus != "" && Campus != null && Block != "" && Block != null) {
        debugger;
        $.getJSON("/Asset/GetLocationByCampusWithBlock?Campus=" + Campus + '&Block=' + Block,
        function (fillbc) {
            debugger;
            ddlbc.empty();
            ddlbc.append($('<option/>', { value: "", text: "Select" }));
            $.each(fillbc, function (index, itemdata) {
                ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
            });
        });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
jQuery(function ($) {
    FillCampusDll();
    FillAssetTypeDll();
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";

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
        url: '/Asset/ITAssetServicejqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['AssetDet_Id', 'Asset Code', 'Asset Id', 'Asset Type', 'Brand', 'Model', 'Serial Number', 'Campus', 'Block', 'Location', 'User Type', 'Name', 'Created By', 'Created Date', 'Modified By', 'Modified Date', '', ],
        colModel: [
                      { name: 'AssetDet_Id', index: 'AssetDet_Id', editable: true, hidden: true, key: true },
                      { name: 'AssetCode', index: 'AssetCode' },
                      { name: 'Asset_Id', index: 'Asset_Id', editable: true, hidden: true, search: false },
                      { name: 'AssetType', index: 'AssetType' },
                      { name: 'Make', index: 'Make' },
                      { name: 'Model', index: 'Model' },
                      { name: 'SerialNo', index: 'SerialNo' },
                      { name: 'CurrentCampus', index: 'CurrentCampus' },
                      { name: 'CurrentBlock', index: 'CurrentBlock' },
                      { name: 'CurrentLocation', index: 'CurrentLocation' },
                      { name: 'UserType', index: 'UserType' },
                      { name: 'IdNum', index: 'IdNum',sortable:true },
                      //{ name: 'FormId', index: 'FormId', search: false },
                      //{ name: 'Location', index: 'Location' },
                      { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedDate', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true },
                      {
                          name: 'Complete', index: 'Complete', formatter: TaskComplete, sortable: false
                      }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        sortname: 'AssetDet_Id',
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
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;IT Sub Asset Service",
    })
    //$(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size


    //switch element when editing inline
    function aceSwitch(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=checkbox]')
                    .addClass('ace ace-switch ace-switch-5')
                    .after('<span class="lbl"></span>');
        }, 0);
    }
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
            },
            {},
            {})
    $("#btnSearch").click(function () {
        var AssetCode = $("#txtAssetCode").val();
        var AssetType = $("#ddlAssetType").val();
        var Make = $("#txtMake").val();
        var Model = $("#txtModel").val();
        var SerialNumber = $("#txtSerialNo").val();
        var Location = $("#ddlLocation").val();
        var Block = $("#ddlBlock").val();
        var FormId = $("#ddlCampus").val();
        var UserType = $("#SrchddlUserType").val();
        $(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Asset/ITAssetServicejqGrid',
           postData: { AssetCode: AssetCode, AssetType: AssetType, Make: Make, Model: Model, SerialNo: SerialNumber, CurrentLocation: Location, FormId: FormId, CurrentBlock: Block, UserType: UserType },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        FillBlockByCampus();
        FillLocationByCampusWithBlock();
        $(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Asset/ITAssetServicejqGrid',
           postData: { AssetCode: "", AssetType: "", Make: "", Model: "", SerialNo: "", FormId: "", UserType: "", CurrentLocation: "", CurrentBlock: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $('#btnAdd').click(function () {
        if ($('#ddlAssetType').val() == null || $('#ddlAssetType').val() == 'Undefined' || $('#ddlAssetType').val() == "" || $('#ddlAssetType').val() == " ") {
            ErrMsg('Select Asset Type!!');
            return false;
        } else {
            ModifiedLoadPopupDynamicaly("/Asset/AddNewITAsset?AssetId=" + $('#ddlAssetType').val(), $('#newITAsset'), function () {
                LoadSetGridParam($('#newITAsset'))
            }, function () { }, 100, 345, "Add Asset");
        }

    });
    $("#ddlCampus").change(function () {
        FillBlockByCampus();
    });
    $("#ddlCampus,#ddlBlock").change(function () {
        FillLocationByCampusWithBlock();
    });
    $.getJSON("/Asset/GetBrandName",
          function (fillig) {
              var ddlcam = $("#ddlMake");
              ddlcam.empty();
              ddlcam.append("<option value=''> Select </option>");
              $.each(fillig, function (index, itemdata) {
                  ddlcam.append($('<option/>',
                      {
                          value: itemdata.Value,
                          text: itemdata.Text
                      }));
              });
          });
    $("#ddlMake").change(function () {
        FillModelByBrand();
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


    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});
function TaskComplete(cellvalue, options, rowObject) {
    if (rowObject[7] == "Service") {
        return "<input type='button' value='Move Back' id='ServiceReturn' style='background-color:#8B0000; border: none;color:white;padding: 5px 25px;text-align:center;font-size: 10px;border-radius:5px;' onclick=MoveToBack('" + rowObject[0] + "');\>";
    }
    var empty = "";
    return empty;
}
function MoveToBack(AssetId) {

    ModifiedLoadPopupDynamicaly("/Asset/ITAssetServiceReturn?AssetId=" + AssetId, $('#MoveToBack'), function () {
    }, function () { }, 450, 430, "Move Back");
    //$.ajax({
    //    url: "/TaskManagement/CompleteorReopenTask?StaffTaskManagement_Id=" + NewTaskId + '&StaffDetails_Id=' + NewStaffId + '&Note=' + Note + '&Type=ReOpen',
    //    type: "POST",
    //    dataType: "json",
    //    success: function (data) {
    //        $("#ViewStaffTaskManagementList").trigger("reloadGrid");
    //    }
    //})

}