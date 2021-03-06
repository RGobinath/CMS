﻿jQuery(function ($) {
    var grid_selector = "#ReferenceDetailsList";
    var pager_selector = "#ReferenceDetailsPager";

    //resize to fit page size
    //    $(window).on('resize.jqGrid', function () {
    //        $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    //    })
    $(window).on('resize.jqGrid', function () {
        var page_width = $(".page-content").width();
        page_width = page_width - 25;
        $(grid_selector).jqGrid('setGridWidth', page_width);
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

    jQuery(grid_selector).jqGrid({
        url: '/StaffManagement/ReferenceDetailsJqgrid',
        datatype: 'json',
        height: 100,
        colNames: ['Name', 'Contact No', 'How you Know', 'How Long you Know', '', ''],
        colModel: [
                          { name: 'RefName', index: 'RefName', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'RefContactNo', index: 'RefContactNo', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'RefHowKnow', index: 'RefHowKnow', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'RefHowLongKnow', index: 'RefHowLongKnow', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'Id', index: 'Id', width: '30%', align: 'left', sortable: false, hidden: true, key: true, editable: true, formatter: nildata },
                          { name: 'PreRegNum', index: 'PreRegNum', width: 50, align: 'left', sortable: false, hidden: true, editable: true, formatter: nildata }
                          ],
        viewrecords: true,
        rowNum: 7,
        rowList: [7, 10, 30],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Asc',
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-paw'></i>&nbsp;Reference Details List"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    // $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: true,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, { url: '/StaffManagement/EditStaffReferenceDetails/', left: '10%', top: '10%', height: '50%', width: 400, labelswidth: 60, closeAfterEdit: true, closeOnEscape: true, reloadAfterSubmit: true },
            {}, { url: '/StaffManagement/DeleteStaffReferenceDetails' }, {})

    //For pager Icons
    function styleCheckbox(table) {
    }
    function updateActionIcons(table) {
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
    function nildata(cellvalue, options, rowObject) {
        if ((cellvalue == '') || (cellvalue == null) || (cellvalue == 0)) {
            return ''
        }
        else {
            cellvalue = cellvalue.replace('&', 'and');
            //str = str.replace(/\&lt;/g, '<');
            return cellvalue
        }
    }
    $("#AddReferenceData").click(function () {
        if (document.getElementById("RefName").value == "") {
            ErrMsg("Please Enter Name!");
            return false;
        }
        else {
            var RefName = $("#RefName").val();
            var RefContactNo = $("#RefContactNo").val();
            var RefHowKnow = $("#RefHowKnow").val();
            var RefHowLongKnow = $("#RefHowLongKnow").val();

            $.ajax({
                url: '/StaffManagement/AddReferenceDetails',
                type: 'POST',
                dataType: 'json',
                data: { RefName: RefName, RefContactNo: RefContactNo, RefHowKnow: RefHowKnow, RefHowLongKnow: RefHowLongKnow },
                traditional: true,
                success: function (data) {
                    $(grid_selector).setGridParam({ url: '/StaffManagement/ReferenceDetailsJqgrid' }).trigger("reloadGrid");
                    document.getElementById("RefName").value = '';
                    document.getElementById("RefContactNo").value = '';
                    document.getElementById("RefHowKnow").value = '';
                    document.getElementById("RefHowLongKnow").value = '';
                },
                loadError: function (xhr, status, error) {
                    msgError = $.parseJSON(xhr.responseText).Message;
                    ErrMsg(msgError, function () { });
                }
            });
        }
    });
});