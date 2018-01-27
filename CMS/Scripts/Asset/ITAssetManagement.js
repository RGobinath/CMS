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
              ddlbc.append($('<option/>', { value: "Stock", text: "Stock" }));
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
            ddlbc.append($('<option/>', { value: "Stock", text: "Stock" }));
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
        url: '/Asset/ITAssetManagementjqGrid?IsSubAsset=false',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['AssetDet_Id', 'Asset Code', 'Asset Id', 'Asset Type', 'Brand', 'Model', 'Serial Number', 'Transaction Type', 'Campus', 'Block', 'Location', 'User Type', 'Name', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'AssetDet_Id', index: 'AssetDet_Id', hidden: true,key:true },
                      { name: 'AssetCode', index: 'AssetCode', formatter: formatterlink },
                      { name: 'Asset_Id', index: 'Asset_Id', hidden: true, search: false },
                      { name: 'AssetType', index: 'AssetType' },
                      { name: 'Make', index: 'Make' },
                      { name: 'Model', index: 'Model' },
                      { name: 'SerialNo', index: 'SerialNo' },
                      { name: 'TransactionType', index: 'TransactionType' },
                      { name: 'CurrentCampus', index: 'CurrentCampus' },
                      { name: 'CurrentBlock', index: 'CurrentBlock' },
                      { name: 'CurrentLocation', index: 'CurrentLocation' },
                      { name: 'UserType', index: 'UserType' },
                      { name: 'IdNum', index: 'IdNum', sortable: true },
                      //{ name: 'FormId', index: 'FormId', search: false },
                      //{ name: 'Location', index: 'Location' },
                      { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedDate', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true }
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
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;IT Asset Management",
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
        var TransactionType = $("#ddlTransactionType").val();
        var UserType = $("#SrchddlUserType").val();
        $(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Asset/ITAssetManagementjqGrid',
           postData: { AssetCode: AssetCode, AssetType: AssetType, Make: Make, Model: Model, SerialNo: SerialNumber, CurrentLocation: Location, FormId: FormId, CurrentBlock: Block, TransactionType: TransactionType, UserType: UserType, IsSubAsset: false },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#ddlCampus").change(function () {
        FillBlockByCampus();
    });
    $("#ddlMake").change(function () {
        FillModelByBrand();
    });
    $("#ddlCampus,#ddlBlock").change(function () {
        FillLocationByCampusWithBlock();
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        FillBlockByCampus();
        FillLocationByCampusWithBlock();
        $(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Asset/ITAssetManagementjqGrid',
           postData: { AssetCode: "", AssetType: "", Make: "", Model: "", SerialNo: "", CurrentLocation: "", FormId: "", TransactionType: "", CurrentBlock: "", UserType: "" },
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
            }, function () { }, 400, 500, "Add Asset");
        }

    });
    $("#btnprint").click(function () {        
        debugger;
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        if (GridIdList == "") { ErrMsg("Please select List "); return false; }
        var rowData = [];
        var rowData3 = [];
        var rowData1 = [];
        var MainrowData1 = "";
        
        if (GridIdList.length > 0) {
            var columnIndex = 2;
            var ObjBarCodeAx = document.getElementById('ObjBarCode');
            var strCommand = "O\nQ200,24\nq820\nD12\nZT\nJF";
            var flag = true;
            var j = GridIdList.length;
            for (i = 0; i < GridIdList.length; i++) {                
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                rowData1[i] = $("#" + rowData[i].AssetDet_Id).find('td').eq(columnIndex).text();
                rowData3[i] = rowData[i].SerialNo;                
                if (flag == true)
                {
                    j=j-1;
                    strCommand += "\nN\nA30,20,0,1,1,2,N,\"" + rowData3[i] + "\"\nB30,45,0,1,2,2,60,B,\"" + rowData1[i] + "\"\nA30,149,0,3,2,2,N,\"\"";
                    if (j == 0)
                    {
                        strCommand += "\nP1\nN\n";
                    }
                    flag = false;
                }
                else if (flag == false)
                {
                    j = j-1;
                    strCommand += "\nA430,20,0,1,1,2,N,\"" + rowData3[i] + "\"\nB430,45,0,1,2,2,60,B,\"" + rowData1[i] + "\"\nA430,149,0,3,2,2,N,\"\"";
                    flag = true;
                    if (j > 0 || j == 0)
                    {
                        strCommand += "\nP1\nN\n";
                    }
                }
                else
                {
                    return false;
                }                
            }
            sCommand = strCommand;
            //var sStaticPrinter = "\\\\inhydsateesh\\Zebra  TLP2844";
            var sStaticPrinter = "ZDesigner TLP 2844";
            sPrinter = ObjBarCodeAx.PrintBarCodeCommand(sStaticPrinter, sCommand);
            return false;
        }
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
    //$('#btnbulkAdd').click(function () {
    //    if ($('#ddlAssetType').val() == null || $('#ddlAssetType').val() == 'Undefined' || $('#ddlAssetType').val() == "" || $('#ddlAssetType').val() == " ") {
    //        ErrMsg('Select Asset Type!!');
    //        return false;
    //    } else {
    //        ModifiedLoadPopupDynamicaly("/Asset/AddNewBulkITAsset?AssetId=" + $('#ddlAssetType').val(), $('#newBulkITAsset'), function () {
    //            LoadSetGridParam($('#newBulkITAsset'))
    //        }, function () { }, 1100, 500, "Add Bulk Asset");
    //    }

    //});

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
function formatterlink(cellvalue, options, rowObject) {
    //return cellvalue;
    var delBtn = "";
    //delBtn = "<a href=ShowAssetDetails('" + rowObject[1] + "','" + options.colModel.index + "');>" + cellvalue + "</a>";
    delBtn = "<a href='/Asset/ITAssetTransaction?AssetDet_Id=" + rowObject[0] + "'>" + cellvalue + "</a>";
    return delBtn;
}
