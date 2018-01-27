$(function () {
    var grid_selector = "#StockList";
    var pager_selector = "#StockListPager";

    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    jQuery(grid_selector).jqGrid({
        url: '/Store/StockListJqGrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'ItemId', 'ItemCode', 'Store', 'Material Group', 'Material Sub Group', 'Material', 'Units', 'Stock'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'ItemId', index: 'ItemId', width: 90, sortable: true },
              { name: 'ItemCode', index: 'ItemCode', width: 90, sortable: true },
              { name: 'Store', index: 'Store', width: 90, sortable: true },
              { name: 'MaterialGroup', index: 'MaterialGroup', width: 90, sortable: true },
              { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 90, sortable: true },
              { name: 'Material', index: 'Material', width: 90, sortable: true },
              { name: 'Units', index: 'Units', width: 90, sortable: true },
              { name: 'ClosingBalance', index: 'ClosingBalance', width: 90, cellattr: function (rowId, cellValue, rawObject, cm, rdata) {
                  if (cellValue == 0) {
                      return 'class="ui-state-error ui-state-error-text"';
                  }
              }
              },
              ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '230',
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-th-list'></i>&nbsp;Stock List",
        multiselect: true
    });
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
            {}, {}, {})
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "Export To Excel",
        onClickButton: function () {
            var MatGrp = $("#ddlMaterialGroup").val();
            var MatSubGrp = $("#ddlMaterialSubGroup").val();
            var Mat = $("#ddlMaterial").val();
            var ItemId = $("#gs_ItemId").val();
            var ItemCode = $("#gs_ItemCode").val();
            var Store = $("#gs_Store").val();
            var MaterialGroup = $("#gs_MaterialGroup").val();
            var MaterialSubGroup = $("#gs_MaterialSubGroup").val();
            var Material = $("#gs_Material").val();
            var Units = $("#gs_Units").val();
            var ClosingBalance = $("#gs_ClosingBalance").val();
            var ExptType = 'Excel';
            window.open("/Store/StockListJqGrid" + '?MatGrp=' + MatGrp + '&MatSubGrp=' + MatSubGrp + '&Mat=' + Mat
                + '&ItemId=' + ItemId + '&ItemCode=' + ItemCode + '&Store=' + Store + '&MaterialGroup=' + MaterialGroup + '&MaterialSubGroup=' + MaterialSubGroup + '&Material=' + Material + '&Units=' + Units + '&ClosingBalance=' + ClosingBalance
                + '&rows=9999&ExptType=' + ExptType);
        }
    });
    $(grid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () { $(grid_selector).clearGridData(); return false; } });

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    $("#gs_Material").autocomplete({
        source: function (request, response) {
            $.getJSON('/Store/GetMaterials?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1
    });

    $("#btnSearch").click(function () {
        $(grid_selector).clearGridData();
        var MatGrp = $("#ddlMaterialGroup").val();
        var MatSubGrp = $("#ddlMaterialSubGroup").val();
        var Mat = $("#ddlMaterial").val();
        $(grid_selector).setGridParam(
                        {
                            datatype: "json",
                            url: '/Store/StockListJqGrid',
                            postData: { MatGrp: MatGrp, MatSubGrp: MatSubGrp, Mat: Mat },
                            page: 1
                        }).trigger("reloadGrid");
    });

    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("")
        $(grid_selector).setGridParam(
                        {
                            datatype: "json",
                            url: '/Store/StockListJqGrid',
                            postData: { MatGrp: "", MatSubGrp: "", Mat: "" },
                            page: 1
                        }).trigger("reloadGrid");
    });

    $("#ddlMaterialSubGroup").change(function () {
        var matgrp = $("#ddlMaterialGroup").val();
        var matsubgrp = $(this).val();
        if (matgrp != "" && matsubgrp != "") {
            $.ajax({
                type: 'POST',
                async: false,
                url: '/Store/FillMaterial?MaterialGroupId=' + matgrp + "&MaterialSubGroupId=" + matsubgrp,
                success: function (data) {
                    $("#ddlMaterial").empty();
                    $("#ddlMaterial").append("<option value=''> Select One </option>");
                    for (var i = 0; i < data.length; i++) {
                        $("#ddlMaterial").append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                    }
                },
                dataType: "json"
            });
        }
        else {
            $("#ddlMaterial").empty();
            $("#ddlMaterial").append("<option value=''> Select One </option>");
        }
    });

    $("#ddlMaterialGroup").change(function () {
        var matgrp = $("#ddlMaterialGroup").val();
        if (matgrp != "") {
            $.ajax({
                type: 'GET',
                async: false,
                url: '/Store/FillMaterialSubGroup?MaterialGroupId=' + matgrp,
                success: function (data) {
                    $("#ddlMaterialSubGroup").empty();
                    $("#ddlMaterialSubGroup").append("<option value=''> Select One </option>");
                    for (var i = 0; i < data.length; i++) {
                        $("#ddlMaterialSubGroup").append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                    }
                },
                dataType: "json"
            });
        }
        else {
            $("#ddlMaterialSubGroup").empty();
            $("#ddlMaterialSubGroup").append("<option value=''> Select One </option>");
        }

    });

    $.getJSON("/Store/FillMaterialGroup",
                         function (fillig) {
                             var ddlmatgrp = $("#ddlMaterialGroup");
                             ddlmatgrp.empty();
                             ddlmatgrp.append($('<option/>', { value: "", text: "Select One" }));
                             $.each(fillig, function (index, itemdata) {
                                 ddlmatgrp.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                             });
                         });
});

function updatePagerIcons(table) {
    var replacement = {
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