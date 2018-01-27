function FillCampusDll() {
    $.getJSON("/Base/FillAllBranchCode",
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
    var Campus = $("#ddlCampus").val();
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
    var Campus = $("#ddlCampus").val();
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
    //FillAcademicYearDll();
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
        url: '/Asset/AssetLocationMasterJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['LocationId', 'Location Code', 'Campus', 'Block Name', 'Location', 'Is Active', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'LocationId', index: 'LocationId', hidden: true, key: true, editable: true },
                      { name: 'LocationCode', index: 'LocationCode', hidden: true },
                      {
                          name: 'Campus', index: 'Campus', editable: true, edittype: 'select', editoptions: {
                              dataUrl: '/Assess360/GetCampusddl', style: "width: 165px; height: 20px; font-size: 0.9em",
                              dataInit: function (e) {
                                  $(e).change(function () {
                                      debugger;
                                      var Campus = $(this).val();
                                      if ($("#Campus").val() != "") {
                                          $.getJSON("/Asset/GetBlockByCampus?Campus=" + Campus,
                                          function (recipes) {

                                              var selectHtml = "<select>";
                                              selectHtml += '<option value=" ">--select--</option>';
                                              //$.each(recipes, function (jdIndex, jdData) {
                                              //    selectHtml = selectHtml + "<option value='" + jdData.Value + "'>" + jdData.Text + "</option>";
                                              //});
                                              if (recipes != null) {
                                                  for (i = 0; i < recipes.length; i++) {

                                                      selectHtml = selectHtml + "<option value='" + recipes[i].Value + "'>" + recipes[i].Text + "</option></select>";
                                                  }
                                              }
                                              else {
                                                  selectHtml += "</select>";
                                              }
                                              $("#BlockName").html(selectHtml);
                                          });

                                      }
                                  });
                              }
                          },
                          editrules: { required: true }
                      },
                      {
                          name: 'BlockName', index: 'BlockName', editable: true, edittype: 'select', editoptions: {
                              dataUrl: '/Asset/GetBlockByCampus?Campus=',
                              buildSelect: function (data) {
                                  debugger;
                                  Block = jQuery.parseJSON(data);
                                  var html = '<select>'
                                  html += '<option value=" ">--Select--</option>';
                                  for (var i = 0; i < Block.length; i++) {
                                      html += '<option value=' + Block[i].Text + '>' + Block[i].Text + '</option>';
                                  }
                                  html += '</select>';
                                  return html;
                              },
                              dataInit: function (e) {
                                  $(e).focus(function () {
                                      if ($("#Campus").val() == "") {
                                          return ErrMsg("Please Select Campus");
                                      }
                                  });
                              }, style: "width: 165px; height: 20px; font-size: 0.9em"
                          },
                      },
                      { name: 'Location', index: 'Location', editable: true, editrules: { required: true } },
                      { name: 'IsActive', index: 'IsActive' },
                      { name: 'CreatedBy', index: 'CreatedBy' },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 20, hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedDate', width: 20, hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 20, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        sortname: 'LocationId',
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
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Asset Location Master",
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
                width: 'auto', url: '/Asset/EditAssetLocationMaster', closeAfterEdit: true
                //url: '/Common/AddAcademicMaster/?test=edit',  closeOnEscape: true, beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Edit
            {
                url: '/Asset/AddAssetLocationMaster',
                closeAfterAdd: true, closeOnEscape: true,
            },
              {
                  width: 'auto', url: '/Asset/DeleteAssetLocationMaster', beforeShowForm: function (params) {
                      selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                      return { Id: selectedrows }
                  }
              },
                {});

    //$(grid_selector).jqGrid('filterToolbar', {
    //    searchOnEnter: true, enableClear: false,
    //    afterSearch: function () { $(grid_selector).clearGridData(); }
    //});
    $("#ddlCampus").change(function () {
        FillBlockByCampus();
    });
    $("#ddlCampus,#ddlBlock").change(function () {
        FillLocationByCampusWithBlock();
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        FillBlockByCampus();
        FillLocationByCampusWithBlock();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Asset/AssetLocationMasterJqGrid',
           postData: { Campus: "", Location: "", Block: "", IsActive: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        var Campus = $("#ddlCampus").val();
        var Location = $("#ddlLocation").val();
        var Block = $("#ddlBlock").val();
        var IsActive = $("#ddlIsActive").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Asset/AssetLocationMasterJqGrid',
           postData: { Campus: Campus, Location: Location, Block: Block, IsActive: IsActive },
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