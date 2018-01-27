$(function () {

    var grid_selector = "#VehicleSubTypeMasterList";
    var pager_selector = "#VehicleSubTypeMasterListPager";
    var grid_selector1 = "#VehicleSubTypeMasterList1";
    var pager_selector1 = "#VehicleSubTypeMasterListPager1";

    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(grid_selector).jqGrid('setGridWidth', parent_column.width());
                $(grid_selector1).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    var Id = $("#Id").val();
    var idsOfSelectedRows = [];
    //var SaveUrl = '@ViewBag.SaveUrl';
    //var reloadGridUrl = '@ViewBag.reloadGridUrl';
    //var GridId = '@ViewBag.GridId';
    //var BulkEntryType = '@ViewBag.BulkEntryType';
    var SaveUrl = $("#SaveUrl").val();
    var reloadGridUrl = $("#reloadGridUrl").val();
    var GridId = $("#GridId").val();
    var BulkEntryType = $("#BulkEntryType").val();

    jQuery(grid_selector).jqGrid({
        url: "/Transport/VehicleSubTypeMasterListJqGrid",
        datatype: 'json',
        mtype: 'GET',
        height: '170',
        //shrinkToFit: true,
        colNames: ['Id', 'Vehicle Type', 'Type', 'Vehicle No', 'Fuel Type', 'Campus', 'Purpose'],
        colModel: [
                { name: 'Id', index: 'Id', align: 'left', editable: true, hidden: true, edittype: 'text', key: true, sortable: false },
                { name: 'VehicleType', index: 'VehicleType', align: 'left', edittype: 'text', editable: true, hidden: false, editrules: { required: true }, editoptions: { maxlength: 50} },
                { name: 'Type', index: 'Type', align: 'left', edittype: 'text', editable: true, editrules: { required: true }, editoptions: { maxlength: 50} },
                { name: 'VehicleNo', index: 'VehicleNo', editable: true, edittype: 'text' },
                { name: 'FuelType', index: 'FuelType', editable: true, edittype: 'text' },
                    { name: 'Campus', index: 'Campus', align: 'left', edittype: 'select', editable: true, editrules: { required: true },
                        editoptions: {
                            dataUrl: '/Base/FillAllBranchCode',
                            buildSelect: function (data) {
                                Campus = jQuery.parseJSON(data);
                                var s = '<select>';
                                s += '<option value=" ">------Select------</option>';
                                if (Campus && Campus.length) {
                                    for (var i = 0, l = Campus.length; i < l; i++) {
                                        s += '<option value="' + Campus[i].Value + '">' + Campus[i].Text + '</option>';
                                    }
                                }
                                return s + "</select>";
                            }
                        }
                    },
                { name: 'Purpose', index: 'Purpose', align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "4", cols: "18", maxlength: 400} }
                ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: '',
        sortorder: "",
        viewrecords: true,
        multiselect: true,
        closeAfterEdit: true,
        closeAfterAdd: true,
        onSelectRow: function (id, status) {

            var index = $.inArray(id, idsOfSelectedRows);
            if (!status && index >= 0) {
                idsOfSelectedRows.splice(index, 1); // remove id from the list
            } else if (index < 0) {
                idsOfSelectedRows.push(id);
            };
            var RowList1;
            var selectedData1;
            RowList1 = $(grid_selector).getGridParam('selarrrow');
            Id = $("#Id").val();
            selectedData1 = $(grid_selector).jqGrid('getRowData', id);
            if (status == true) {
                $(grid_selector1).jqGrid('addRowData', id, selectedData1);
            }
            else {
                $(grid_selector1).jqGrid('delRowData', id, selectedData1);
            }
        },
        loadComplete: function () {
            var $this = $(this), i, count;
            for (i = 0, count = idsOfSelectedRows.length; i < count; i++) {
                $this.jqGrid('setSelection', idsOfSelectedRows[i], false);
            }
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }, caption: 'Vehicle Sub Type Master'
    });

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    //navButtons Add, edit, delete
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
    $(grid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () {
        $(grid_selector).clearGridData();
        return false;
    }
    });


    $("#gs_VehicleNo").autocomplete({
        source: function (request, response) {
            $.getJSON('/Transport/GetVehicleNo?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 0
    });

    jQuery(grid_selector1).jqGrid({
        //  url: '/Transport/VehicleSubTypeMasterJqGrid',
        datatype: 'json',
        mtype: 'GET',
        height: '130',
        colNames: ['Id', 'Vehicle Type', 'Vehicle Sub Type', 'Vehicle No', 'Campus', 'Purpose', 'Fuel Type'],
        colModel: [
                { name: 'Id', index: 'Id', hidden: true, key: true },
                { name: 'VehicleType', index: 'VehicleType' },
                { name: 'Type', index: 'Type' },
                { name: 'VehicleNo', index: 'VehicleNo' },
                { name: 'Campus', index: 'Campus' },
                { name: 'Purpose', index: 'Purpose' },
                { name: 'FuelType', index: 'FuelType' }
                ],
        pager: pager_selector1,
        rowNum: '100',
        rowList: [5, 10, 20, 50, 100, 200],
        sortname: '',
        sortorder: "",
        viewrecords: true,
        multiselect: true,
        closeAfterEdit: true,
        closeAfterAdd: true,
        caption: 'Vehicle Sub Type Master',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(grid_selector1).jqGrid('setGridWidth');
        }
    });
    //jQuery("#VehicleSubTypeMasterList1").navGrid('#VehicleSubTypeMasterListPager1', { add: false, edit: false, del: false, search: false, refresh: false });

    $("#btnSave").click(function () {
        var RowList;
        var selectedData;
        var DLst = '';
        RowList = $(grid_selector1).getDataIDs();
        Id = $("#Id").val();

        for (var i = 0, list = RowList.length; i < list; i++) {
            var selectedId = RowList[i];
            selectedData = $(grid_selector1).jqGrid('getRowData', selectedId);
            if (BulkEntryType == "FuelRefill") {
                DLst += "&[" + i + "].VehicleId=" + selectedData.Id
                + "&[" + i + "].VehicleNo=" + encodeURIComponent(selectedData.VehicleNo)
                + "&[" + i + "].Type=" + selectedData.Type
                + "&[" + i + "].RefId=" + Id
                + "&[" + i + "].FuelType=" + selectedData.FuelType
            }
            else {
                DLst += "&[" + i + "].VehicleId=" + selectedData.Id
                + "&[" + i + "].VehicleNo=" + encodeURIComponent(selectedData.VehicleNo)
                + "&[" + i + "].Type=" + selectedData.Type
                + "&[" + i + "].RefId=" + Id
            }
        }
        $.ajax({
            url: SaveUrl,
            type: 'POST',
            dataType: 'json',
            data: DLst,
            success: function (data) {
                Id = $("#Id").val();
                idsOfSelectedRows = [''];
                $("#" + GridId).setGridParam({ url: reloadGridUrl + Id }).trigger("reloadGrid");
            }
        });

    });
    $("#btnSubmitAndClose").click(function () {
        $("#btnSave").click();
        $('#DivVehicleSearch').dialog('close');
    });
});