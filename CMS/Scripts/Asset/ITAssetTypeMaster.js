jQuery(function ($) {
    var grid_selector = "#ITAssetMasterjqGrid";
    var pager_selector = "#ITAssetMasterjqGridPager";
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

    function getSpecification() {
        var spec;
        $.ajax(
            {
                url: '/Asset/FillITAssetSpecification',
                async: false,
                success: function (data, result) {
                    debugger;
                    if (data != "" && data != null) {
                        var tempData = '';
                        for (var index = 0; index < data.length; index++) {
                            tempData += data[index].Value + ':' + data[index].Text;
                            if (index != data.length - 1)
                                tempData += ';';
                        }
                        spec = tempData;
                    }
                    else {
                        spec = '';
                    }
                }
            });
        return spec;
    }
    jQuery(grid_selector).jqGrid({
        url: '/Asset/ITAssetTypeMasterjqGrid',
        datatype: 'json',
        height: 200,
        width: 1270,
        colNames: ['Id', 'Asset Name', 'Asset Code','Is Sub Asset', 'Specifications', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'Asset_Id', index: 'Asset_Id', key: true, hidden: true, editable: true },
                     //{
                     //    name: 'FormId', index: 'FormId', editable: true, width: 60, edittype: 'select', editoptions: {
                     //        dataUrl: '/Base/FillCampus',
                     //        buildSelect: function (data) {
                     //            var CampusName = jQuery.parseJSON(data);
                     //            var s = '<select>';
                     //            s += '<option value="">----Select----</option>';
                     //            if (CampusName && CampusName.length) {
                     //                for (var i = 0, l = CampusName.length; i < l; i++) {
                     //                    s += '<option value="' + CampusName[i].Value + '">' + CampusName[i].Text + '</option>';
                     //                }
                     //            }
                     //            return s + "</select>";
                     //        },
                     //        style: "width: 167px; height: 20px; font-size: 0.9em"
                     //    }, editrules: { custom: true, custom_func: checkvalid }, sortable: true
                     //},
                      { name: 'AssetType', index: 'AssetType', editable: true, search: true, width: 80, editoptions: { style: "width: 167px; height: 20px; font-size: 0.9em" }, editrules: { custom: true, custom_func: checkvalid } },
                       {
                           name: 'AssetCode', index: 'AssetCode', editable: true, search: true, width: 80, editoptions: {
                               style: "width: 167px; height: 20px; font-size: 0.9em"
                           }, editrules: { custom: true, custom_func: checkvalid }
                       },
                       { name: 'IsSubAsset', index: 'IsSubAsset', editable: true, width: 60, edittype: "select", editoptions: { sopt: ['eq'], value: { '': 'Select', 'true': 'Yes', 'false': 'No' }, style: "width: 165px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, sortable: true, stype: 'select' },
                      {
                          name: 'SpecificationsDetails', index: 'SpecificationsDetails', editable: true, hidden: false, width: 160, edittype: 'select', editoptions: {
                              multiple: true,
                              value: getSpecification(),
                              dataInit: function (elem) {
                                  setTimeout(function () {
                                      $(elem).multiselect({
                                          includeSelectAllOption: true,
                                          selectAllText: ' Select All',
                                          enableCaseInsensitiveFiltering: true,
                                          enableFiltering: true,
                                          maxHeight: '410',
                                          buttonWidth: '210px',
                                          numberDisplayed: 2,
                                          includeSelectAllDivider: true
                                      });
                                      $('#tr_SpecificationsDetails').find('ul').css('position', 'relative');
                                  });
                              },
                              //dataEvents: [{
                              //    type: 'change', fn: function (e) {
                              //        debugger;
                              //        alert($(this).text());
                              //    }
                              //}],
                              style: "width: 167px; height: 20px; font-size: 0.9em"
                          }, editrules: { required: true }, sortable: true
                      },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 60, hidden: false },
                      { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true }
        ],
        sortname: 'Asset_Id',
        sortorder: 'Desc',
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
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
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Asset Master"
    });
    $(window).triggerHandler('resize.jqGrid');//trigger window resize to make the grid get the correct size
    //navButtons

    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
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
                url: '/Asset/EditITAssetTypeMaster', closeAfterEdit: true, closeOnEscape: true, dataheight: 500, width: 900, height: 600, top: 10, left: 10, beforeShowForm: function (frm) { }
                //,beforeSubmit: function (frm) {
                //    debugger;
                //    var campusMaster = { FormId: frm.FormId }
                //    frm.CampusMaster = campusMaster;
                //    return [true, ""];
                //}
            }, //Edit
            {
                url: '/Asset/AddITAssetTypeMaster', closeOnEscape: true, width: 500, height: 619, top: 0, left: 480, beforeShowForm: function (frm) { }
                //, beforeSubmit: function (frm) {
                //    debugger;
                //    myArray = frm.SpecificationsDetails.split(',');
                //    return [false, ""];
                //}
            }, //Add
              { url: '/Asset/DeleteITAssetTypeMaster', beforeShowForm: function (params) { var gsr = $(grid_selector).getGridParam('selrow'); var sdata = $(grid_selector).getRowData(gsr); return { Id: sdata.Id } } },
              {}, {})

    //$.getJSON("/Base/FillCampus",
    //        function (fillig) {
    //            var ddlcam = $("#ddlCampus");
    //            ddlcam.empty();
    //            ddlcam.append("<option value=' '> ----Select---- </option>");
    //            $.each(fillig, function (index, itemdata) {
    //                ddlcam.append($('<option/>',
    //                    {
    //                        value: itemdata.Value,
    //                        text: itemdata.Text
    //                    }));
    //            });
    //        });
    //$("#ddlCampus").change(function () {
    //    FillAssetName();
    //});

    FillAssetName();
    $("#btnSearch").click(function () {
        jQuery(grid_selector).clearGridData();
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Asset/ITAssetTypeMasterjqGrid',
                        postData: { Asset_Id: $('#ddlAsset').val(), AssetCode: $('#txtAssetCode').val() },
                        page: 1
                    }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $(grid_selector).clearGridData();
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: '/Asset/ITAssetTypeMasterjqGrid',
                        postData: { Asset_Id: "", AssetCode: "" },
                        page: 1
                    }).trigger("reloadGrid");

    });
    function checkvalid(value, column) {
        debugger;
        if (value == 'nil' || value == "") {
            return [false, column + ": Field is Required"];
        }
        else if (column == "Asset Code") {
            if (value.length > 4) {
                return [false, column + ":Maximum Four Characters"];
            }
            else {
                return [true];
            }
        }
        else {
            return [true];
        }
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
});
function FillAssetName() {
    // FormId = $('#ddlCampus').val();
    //$.getJSON("/Asset/FillITAssetName", { FormId: FormId },
    $.getJSON("/Asset/FillITAssetName",
           function (fillig) {
               var ddlcam = $("#ddlAsset");
               ddlcam.empty();
               ddlcam.append("<option value=' '>Select</option>");
               if (fillig != null) {
                   $.each(fillig, function (index, itemdata) {
                       ddlcam.append($('<option/>',
                       {
                           value: itemdata.Value,
                           text: itemdata.Text
                       }));
                   });
               }
           });
}