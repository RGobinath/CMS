﻿function FillBrandDll() {
    $.getJSON("/Asset/GetBrandName",
      function (fillbc) {
          var ddlbc = $("#ddlBrandName");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
jQuery(function ($) {
    FillBrandDll();
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
        url: '/Asset/AssetModelMasterJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['BrnadMasterId', 'Brand', 'Model', 'Is Active', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'ModelMasterId', index: 'ModelMasterId', hidden: true, editable: true, key: true },
                      //{
                      //    name: 'Campus', index: 'Campus', editable: true, edittype: 'select', editoptions: { dataUrl: '/Assess360/GetCampusddl', style: "width: 149px; height: 20px; font-size: 0.9em" }, editrules: { required: true }
                      //},
                      { name: 'Brand', index: 'Brand', editable: true, editrules: { required: true }, edittype: 'select', editoptions: { dataUrl: '/Asset/GetBrandNameddl', style: "width: 149px; height: 20px; font-size: 0.9em" } },
                      { name: 'Model', index: 'Model', editable: true, editrules: { required: true } },
                      { name: 'IsActive', index: 'IsActive', edittype: 'select', editable: false, editoptions: { sopt: ['eq'], value: { '': 'Select', 'true': 'Yes', 'false': 'No' }, style: "width: 149px; height: 20px; font-size: 0.9em" }, editrules: { required: true } },
                      { name: 'CreatedBy', index: 'CreatedBy' },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 20, hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedDate', width: 20, hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 20, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        altRows: true,
        sortname: 'ModelMasterId',
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
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Asset Model Master",
    })
    //$(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size    
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: true,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {
                width: 'auto', url: '/Asset/EditAssetModelMaster', modal: false, closeAfterEdit: true
                //url: '/Common/AddAcademicMaster/?test=edit', closeAfterEdit: true, closeOnEscape: true, beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Edit
            {
                width: 'auto', url: '/Asset/AddAssetModelMaster', modal: false, closeAfterAdd: true
                //url: '/Common/AddAcademicMaster', closeOnEscape: true, beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Add
              {
                  width: 'auto', url: '/Asset/DeleteAssetModelMaster', beforeShowForm: function (params) {
                      selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                      return { Id: selectedrows }
                  }
              },
               {},
                {})
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        //FillBlockByCampus();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Asset/AssetModelMasterJqGrid',
           postData: { Brand: "", Model: "", IsActive: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        //var Campus = $("#ddlCampus").val();
        var Brand = $("#ddlBrandName").val();
        var IsActive = $("#ddlIsActive").val();
        var Model = $("#txtModel").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Asset/AssetModelMasterJqGrid',
           postData: { Brand: Brand, Model: Model, IsActive: IsActive },
           page: 1
       }).trigger("reloadGrid");
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


    //$(document).on('ajaxloadstart', function (e) {
    //    $(grid_selector).jqGrid('GridUnload');
    //    $('.ui-jqdialog').remove();
    //});
});