function FillModelByBrand() {
    var BrandId = $("#SrchddlBrand").val();
    var ddlbc = $("#SrchddlModel");
    if (BrandId != "" && BrandId != null) {
        $.getJSON("/Asset/GetITAccessoriesModelByBrand?BrandId=" + BrandId,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillProductTypeByName() {
    var ProductNameId = $("#SrchddlProductName").val();
    var ddlbc = $("#SrchddlProductType");
    if (ProductNameId != "" && ProductNameId != null) {
        $.getJSON("/Asset/GetAssetProductTypeByAssetProductName?ProductNameId=" + ProductNameId,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
jQuery(function ($) {
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
        url: '/Asset/ITAccessoriesJqgrid',
        datatype: 'json',
        height: 190,
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Product Name', 'Product Type', 'Brand', 'Model', 'Quantity', 'Amount', 'Warranty', 'Vendor Name','Document', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'Id', index: 'Id', hidden: true, editable: true, key: true },
                      {
                          name: 'CampusMaster.FormId', index: 'CampusMaster.FormId', editable: true, edittype: 'select', editoptions: { dataUrl: '/Assess360/GetCampusddl', style: "width: 149px; height: 20px; font-size: 0.9em" }, editrules: { required: true }
                      },
                      {
                          name: 'AssetProductMaster.AssetProductMasterId', index: 'AssetProductMaster.AssetProductMasterId', editable: true, editrules: { required: true }

                      },
                      {
                          name: 'AssetProductTypeMaster.AssetProductTypeMasterId', index: 'AssetProductTypeMaster.AssetProductTypeMasterId', editable: true, editrules: { required: true }

                      },
                      {
                          name: 'ITAccessoriesBrandMaster.Id', index: 'ITAccessoriesBrandMaster.Id', editable: true, editrules: { required: true }

                      },
                      {
                          name: 'ITAccessoriesModelMaster.Id', index: 'ITAccessoriesModelMaster.Id', editable: true, editrules: { required: true }

                      },
                      {
                          name: 'Quantity', index: 'Quantity', editable: true, editrules: { required: true }

                      },
                      {
                          name: 'Amount', index: 'Amount', editable: true, editrules: { required: true }, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' }

                      },
                      {
                          name: 'Warranty', index: 'Warranty', editable: true, editrules: { required: true }
                      },

                      {
                          name: 'AssetInvoiceDetails.VendorMaster.VendorId', index: 'AssetInvoiceDetails.VendorMaster.VendorId', sortable:false

                      },
                      {
                          name: 'Document', index: 'Document',sortable:false
                      },
                      { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 20, hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedDate', width: 20, hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 20, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Asc',
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;IT Accessories",
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
                //width: 'auto', url: '', modal: true, closeAfterEdit: true
                //url: '/Common/AddAcademicMaster/?test=edit', closeAfterEdit: true, closeOnEscape: true, beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Edit
            {
                //width: 'auto', url: '', modal: true, closeAfterAdd: true
                //url: '/Common/AddAcademicMaster', closeOnEscape: true, beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Add
              {
                  //width: 'auto', url: '', beforeShowForm: function (params) {
                  //    selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                  //    return { Id: selectedrows }
                  //}
              },
               {},
                {})
    $("#srchbtnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Asset/ITAccessoriesJqgrid',
           postData: { CampusId: "", ProductNameId: "", ProductTypeId: "", BrandId: "", ModelId: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        var ProductNameId = $("#SrchddlProductName").val();
        var ProductTypeId = $("#SrchddlProductType").val();
        var BrandId = $("#SrchddlBrand").val();
        var ModelId = $("#SrchddlModel").val();
        var CampusId = $("#SrchddlCampus").val()
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Asset/ITAccessoriesJqgrid',
           postData: { CampusId: CampusId, ProductNameId: ProductNameId, ProductTypeId: ProductTypeId, BrandId: BrandId, ModelId: ModelId },
           page: 1
       }).trigger("reloadGrid");
    });
    $('#btnAdd').click(function () {
        ModifiedLoadPopupDynamicaly("/Asset/AddNewITAccessories", $('#newITAccessories'), function () {
            LoadSetGridParam($('#newITAccessories'))
        }, function () { }, 400, 500, "Add Accessories");
    });
    $("#SrchddlBrand").change(function () {
        FillModelByBrand();
    });
    $("#SrchddlProductName").change(function () {
        FillProductTypeByName();
    });
    FillProductName();
    $.getJSON("/Base/FillCampus",
          function (fillig) {
              var ddlcam = $("#SrchddlCampus");
              ddlcam.empty();
              ddlcam.append("<option value=' '> Select </option>");
              $.each(fillig, function (index, itemdata) {
                  ddlcam.append($('<option/>',
                      {
                          value: itemdata.Value,
                          text: itemdata.Text
                      }));
              });
          });
    $.getJSON("/Asset/GetITAccessoriesBrandName",
          function (fillig) {
              var ddlcam = $("#SrchddlBrand");
              ddlcam.empty();
              ddlcam.append("<option value=''>Select</option>");
              $.each(fillig, function (index, itemdata) {
                  ddlcam.append($('<option/>',
                      {
                          value: itemdata.Value,
                          text: itemdata.Text
                      }));
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

    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }
});
function FillProductName() {
    $.getJSON("/Asset/GetAssetProductName",
           function (fillig) {
               var ddlcam = $("#SrchddlProductName");
               ddlcam.empty();
               ddlcam.append("<option value=''>Select</option>");
               $.each(fillig, function (index, itemdata) {
                   ddlcam.append($('<option/>',
                       {
                           value: itemdata.Value,
                           text: itemdata.Text
                       }));
               });
           });
}
function uploaddat(id1) {
    //window.location.href = "/Asset/uploaddisplay?Id=" + id1 + '&DocumentFor=ITAsset';
    window.location.href = "/Asset/uploaddisplay?Id=" + id1;
    //processBusy.dialog('close');
}