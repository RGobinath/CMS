
var srchgrid_selector = "#StoreMaterialsList";
var srchpager_selector = "#StoreMaterialsListPager";

var Listoutgrid_selector = "#StoreMaterialsList1";
var Listoutpager_selector = "#StoreMaterialsList1Pager";


$(function () {
    $(window).on('resize.jqGrid', function () {
        $(srchgrid_selector).jqGrid('setGridWidth', $("#DivMaterialSearch").width());
        $(Listoutgrid_selector).jqGrid('setGridWidth', $("#DivMaterialSearch").width());
    })

    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(srchgrid_selector).jqGrid('setGridWidth', parent_column.width());
                $(Listoutgrid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    var idsOfSelectedRows = [];
    jQuery(srchgrid_selector).jqGrid({
        url: '/Store/StoreSKUListJqGridForMaterialInward',
        datatype: 'json',
        mtype: 'GET',
        height: '150',
        //width: '900',
        //shrinkToFit: true,
        colNames: ['Id', 'Material Group', 'Material Sub Group', 'Material', 'Units'],
        colModel: [
                { name: 'Id', index: 'Id', width: 50, align: 'left', editable: true, hidden: true, edittype: 'text', key: true },
                { name: 'MaterialGroup', index: 'MaterialGroup', width: 100, align: "left" },
                { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 100, align: 'left' },
                { name: 'Material', index: 'Material', width: 100, align: 'left' },
                { name: 'Units', index: 'Units', width: 100, align: 'left' },
        // { name: 'ClosingBalance', index: 'ClosingBalance', width: 100, align: 'left' },
                ],
        pager: srchpager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100],
        sortname: 'Material',
        sortorder: "",
        viewrecords: true,
        multiselect: true,
        autowidth: true,
        reloadAfterSubmit: true,
        caption: '<i class="fa fa-th-list"></i> Storer Materials List',
        onSelectRow: function (id, status) {
            var index = $.inArray(id, idsOfSelectedRows);
            if (!status && index >= 0) {
                idsOfSelectedRows.splice(index, 1); // remove id from the list
            } else if (index < 0) {
                idsOfSelectedRows.push(id);
            };
            var RowList1;
            var selectedData;
            RowList1 = $(srchgrid_selector).getGridParam('selarrrow');
            var Id = $("#Id").val();
            selectedData = $(srchgrid_selector).jqGrid('getRowData', id);
            if (status == true) {
                $(Listoutgrid_selector).jqGrid('addRowData', id, selectedData);
            }
            else {
                $(Listoutgrid_selector).jqGrid('delRowData', id, selectedData);
            }
        },
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            var $this = $(this), i, count;
            for (i = 0, count = idsOfSelectedRows.length; i < count; i++) {
                $this.jqGrid('setSelection', idsOfSelectedRows[i], false);
            }
        }
    });
    jQuery(srchgrid_selector).jqGrid('navGrid', srchpager_selector,
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
    $(srchgrid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () {
        $(srchgrid_selector).clearGridData();
        return false;
    }
    });

    $.getJSON("/Store/FillMaterialGroup",
                 function (fillig) {
                     var ddlmatgrp = $("#ddlMaterialGroup");
                     ddlmatgrp.empty();
                     ddlmatgrp.append($('<option/>',
                    {
                        value: "",
                        text: "Select One"
                    }));
                     $.each(fillig, function (index, itemdata) {
                         ddlmatgrp.append($('<option/>',
                             {
                                 value: itemdata.Value,
                                 text: itemdata.Text
                             }));
                     });
                 });
    $("#ddlMaterialGroup").change(function () {
        // $("#ddlMaterialSubGroup").change();
        var matgrp = $("#ddlMaterialGroup").val();
        if (matgrp != "") {
            $.ajax({
                type: 'POST',
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

    $("#ddlMaterialSubGroup").change(function () {
        var matgrp = $("#ddlMaterialGroup").val();
        var matsubgrp = $(this).val();
        if (matgrp != "") {
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

    $("#gs_Material").autocomplete({
        source: function (request, response) {
            $.getJSON('/Store/GetMaterials?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });

    $("#btnSubmit").click(function () {
        var RowList;
        var selectedData;
        var skuLst = '';
        RowList = $(Listoutgrid_selector).getDataIDs();
        var Id = $("#Id").val();
        for (var i = 0, list = RowList.length; i < list; i++) {
            var selectedId = RowList[i];
            selectedData = $(Listoutgrid_selector).jqGrid('getRowData', selectedId);
            skuLst += "&[" + i + "].MaterialGroup=" + encodeURIComponent(selectedData.MaterialGroup)
                + "&[" + i + "].MaterialSubGroup=" + encodeURIComponent(selectedData.MaterialSubGroup)
                + "&[" + i + "].Material=" + encodeURIComponent(selectedData.Material)
                + "&[" + i + "].OrderedUnits=" + selectedData.Units
                + "&[" + i + "].ReceivedUnits=" + selectedData.Units
                + "&[" + i + "].MaterialRefId=" + Id
                + "&[" + i + "].DamagedQty=" + 0
        }
        $.ajax({
            url: '/Store/AddSKUList/',
            type: 'POST',
            dataType: 'json',
            data: skuLst,
            success: function (data) {
                var Id = $("#Id").val();
                idsOfSelectedRows = [''];
                $("#MaterialSkuList").setGridParam({ url: '/Store/MaterialSkuListJqGrid?Id=' + Id +'&Store='+$("#ddlStore").val()}).trigger("reloadGrid");
            }
        });
        $(Listoutgrid_selector).jqGrid("clearGridData", true).trigger("reloadGrid");

    });
    $("#btnSubmitAndClose").click(function () {
        $("#btnSubmit").click();
        $('#DivMaterialSearch').dialog('close');
    });

    jQuery(Listoutgrid_selector).jqGrid({
        // url: '/Store/StoreSKUListJqGrid',
        datatype: 'json',
        mtype: 'GET',
        height: '150',
        //shrinkToFit: true,
        colNames: ['Id', 'Material Group', 'Material Sub Group', 'Material', 'Units'],
        colModel: [
                                { name: 'Id', index: 'Id', width: 50, align: 'left', editable: true, hidden: true, edittype: 'text', key: true, sortable: false },
                                { name: "MaterialGroup", index: "MaterialGroup", width: 100, align: "left" },
                                { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 100, align: 'left', sortable: false },
                                { name: 'Material', index: 'Material', width: 100, align: 'left', sortable: false },
                                { name: 'Units', index: 'Units', width: 100, align: 'left', sortable: false },
                                ],
        pager: Listoutpager_selector,
        sopt: ['bw', 'eq', 'ne', 'lt', 'le', 'gt', 'ge', 'ew', 'cn'],
        rowNum: '50',
        rowList: [5, 10, 20, 50, 100],
        sortname: '',
        sortorder: "",
        viewrecords: true,
        // multiselect: true,
        closeAfterEdit: true,
        closeAfterAdd: true,
        autowidth: true,
        //  shrinkToFit: true,
        reloadAfterSubmit: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="fa fa-th-list"></i> Selected Materials List'
    });
    $("#btnSearch").click(function () {
        debugger;
        $(srchgrid_selector).clearGridData();
        var MaterialGroupId = $("#ddlMaterialGroup").val();
        var MaterialSubGroupId = $("#ddlMaterialSubGroup").val();
        var MaterialId = $("#ddlMaterial").val();
        $(srchgrid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/StoreSKUListJqGridForMaterialInward/',
                    postData: { MaterialGroupId: MaterialGroupId, MaterialSubGroupId: MaterialSubGroupId, MaterialId: MaterialId, Units: '' },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("#ddlMaterialGroup").val('');
        $("#ddlMaterialSubGroup").val('');
        $("#ddlMaterial").val('');
        $(srchgrid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/StoreSKUListJqGridForMaterialInward/',
                    postData: { MaterialGroupId: 0, MaterialSubGroupId: 0, MaterialId: 0, Units: '' },
                    page: 1
                }).trigger("reloadGrid");
    });

});

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