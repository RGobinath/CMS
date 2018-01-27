var grid_selector = "#MaterialReturnList";
var pager_selector = "#MaterialReturnListPager";

$(function () {
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
    if ($("#MatRetId").val() != 0) {
        $("#btnSave").hide();
    }
    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialReturnListJqGrid?MatRetId=' + $("#MatRetId").val(),
        datatype: 'json',
        mtype: 'POST',
        height: '220',
        //width: '1000',
        colNames: ['Id', 'Material Group', 'Material Sub Group', 'Material', 'Units', 'Return Qty'],
        colModel: [{ name: 'Id', index: 'Id', width: 50, align: 'left', editable: true, hidden: true, edittype: 'text', key: true },
                   { name: "MaterialGroup", index: "MaterialGroup", width: 100, align: "left" },
                   { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 100 },
                   { name: 'Material', index: 'Material', width: 100 },
                   { name: 'Units', index: 'Units', width: 100 },
                   { name: 'ReturnQty', index: 'ReturnQty', width: 50}],
        pager: pager_selector,
        rowNum: '50',
        rowList: [5, 10, 20, 50, 100],
        sortname: 'Id',
        sortorder: "asc",
        viewrecords: true,
        autowidth: true,
        caption: '<i class="fa fa-th-list">&nbsp;</i>Materials Return List',
        forceFit: true,
        gridview: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }
    });
    jQuery(grid_selector).jqGrid('navGrid', pager_selector, {
        //navbar options
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
    }, {}, {}, {}, {})


    $("#MaterialSearch").click(function () {
        debugger;
        var Campus = $("#Campus").val();
        ModifiedLoadPopupDynamicaly("/Store/MaterialReturnMaterialSearch?Campus=" + Campus, $('#DivMaterialSearch'),
            function () {
                LoadSetGridParam($('#StoreMaterialsList'), "/Store/StoreSKUListJqGridForMaterialReturn")
            }, function () { }, 1100, 690, "Material Details");
    });

    $("#Complete").click(function () {
        if (confirm("Are you sure to Complete?")) {
            var MaterialReturn = {
                MatRetId: $("#MatRetId").val()
            };
            $.ajax({
                url: '/Store/CompleteMaterialReturn',
                type: 'POST',
                dataType: 'json',
                data: MaterialReturn,
                traditional: true,
                success: function (data) {
                    if (data != "") {
                        InfoMsg(data + " is completed successfully", function () { window.location.href = '/Store/MaterialReturnList'; });
                    }
                },
                error: function (xhr, status, error) {
                    ErrMsg($.parseJSON(xhr.responseText).Message);
                }
            });
        }
    });

    $("#btnBack").click(function () {
        var url = $('#BackUrl').val();
        window.location.href = url;
    });
});
function SaveValidation() {
    if ($("#DCNumber").val() == '') {
        ErrMsg("Please type DC Number");
        $(this).focus();
        return false;
    }
    if ($("#Description").val() == '') {
        ErrMsg("Please type Description");
        $(this).focus();
        return false;
    }
    var objMatInward = {
        MatRetId: $("#MatRetId").val(),
        CreatedBy: $("#CreatedBy").val(),
        Campus: $("#Campus").val(),
        FromStore: $("#FromStore").val(),
        ToStore: $("#ToStore").val(),
        DCNumber: $("#DCNumber").val(),
        ReturnStatus: $("#ReturnStatus").val(),
        Description: $("#Description").val()
    };
    $.ajax({
        url: '/Store/SaveMaterialReturn',
        type: 'POST',
        dataType: 'json',
        data: objMatInward,
        traditional: true,
        success: function (mr) {
            $("#btnSave").hide();
            $("#MatRetId").val(mr.MatRetId);
            $("#ReturnRefNum").val(mr.ReturnRefNum);
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}
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

