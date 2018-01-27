$(function () {
    var grid_selector = "#MaterialPriceList";
    var pager_selector = "#MaterialPriceListPager";

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
        url: '/Store/MaterialPriceListJqGrid',
        datatype: 'json',
        mtype: 'POST',
        colNames: ['SKU Id', 'Material Group', 'Material Sub Group', 'Materials', 'Units', 'Old Prices'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true, key: true, editable: true },
              { name: 'MaterialGroup', index: 'MaterialGroup' },
              { name: 'MaterialSubGroup', index: 'MaterialSubGroup' },
              { name: 'Material', index: 'Material' },
              { name: 'UnitCode', index: 'UnitCode' },
              { name: 'OldPrices', index: 'OldPrices' }
              ],
        pager: pager_selector,
        rowNum: '50',
        rowList: [50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'asc',
        height: '230',
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true, loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-th-list'></i>Material Price List"
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
    jQuery(grid_selector).jqGrid('navButtonAdd', '#MaterialPriceListPager', {
        caption: "Export To Excel",
        onClickButton: function () {
            var MaterialGroup = $("#gs_MaterialGroup").val();
            var MaterialSubGroup = $("#gs_MaterialSubGroup").val();
            var Material = $("#Material").val();
            var UnitCode = $("#gs_UnitCode").val();
            var ExptType = 'Excel';
            window.open("/Store/MaterialPriceListJqGrid" + '?MaterialGroup=' + MaterialGroup + '&MaterialSubGroup=' + MaterialSubGroup + '&Material=' + Material + '&UnitCode=' + UnitCode + '&rows=9999' + '&ExptType=' + ExptType);
        }
    });
    $(grid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () { $(grid_selector).clearGridData(); return false; } });

    $("#Material").autocomplete({
        source: function (request, response) {
            $.getJSON('/Store/GetMaterials?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });


    $("#gs_Material").autocomplete({
        source: function (request, response) {
            $.getJSON('/Store/GetMaterials?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });

    $("#btnSearch").click(function () {
    debugger;
        $(grid_selector).clearGridData();
        var MatGrp = $("#ddlMaterialGroup").val();
        var MatSubGrp = $("#ddlMaterialSubGroup").val();
        var Mat = $("#ddlMaterial").val();
        $(grid_selector).setGridParam(
                        {
                            datatype: "json",
                            url: '/Store/MaterialPriceListJqGrid',
                            postData: { MatGrp: MatGrp, MatSubGrp: MatSubGrp, Mat: Mat },
                            page: 1
                        }).trigger("reloadGrid");
    });

    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("")
        $(grid_selector).setGridParam(
                        {
                            datatype: "json",
                            url: '/Store/MaterialPriceListJqGrid',
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
                        $("#ddlMaterial").append("<option value='" + data[i].Text + "'>" + data[i].Text + "</option>");
                    }
                },
                dataType: "json"
            });
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