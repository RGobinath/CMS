jQuery(function ($) {
    $('#btnNewRouteStudList').click(function () {
        if ($('#Campus').val() == "") {
            ErrMsg("Please select Campus!");
            return false;
        }
        if ($('#ddlroute').val() == "") {
            ErrMsg("Please select Route!");
            return false;
        }
        var tempOldRouteStudCode = "";
        var temp = 0;
        //window.location.href = '/Transport/RouteMasterConfiguration?RouteNo=' + $('#ddlroute').val() + '&Flag=New';
        window.location.href = '/Transport/RouteMasterConfiguration?RouteId=' + $('#ddlroute').val() + '&Flag=New' + '&OldRouteStudCode=' + tempOldRouteStudCode + '&longRouteId=' + temp;
    });

    $('#ddlcampus').change(function () {
        var Campus = $('#ddlcampus').val();
        $.getJSON("/Transport/Routeddl/", { Campus: Campus },
            function (modelData) {
                var select = $("#ddlroute");
                select.empty();
                select.append($('<option/>', { value: "", text: "-----Select-----" }));
                $.each(modelData, function (index, itemData) {
                    select.append($('<option/>', { value: itemData.Value, text: itemData.Text }));
                });
            });
    });


    var grid_selector = "#jqGridRouteStudConfig";
    var pager_selector = "#jqGridRouteStudConfigPager";

    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".tab-content").width());
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
        url: '/Transport/RouteStudJqGrid',
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'RouteId', 'RouteStudCode', 'RouteNo', 'Campus', 'Source', 'Destination', 'Via', 'No Of Students', 'Print PDF', 'Show List'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            { name: 'RouteId', index: 'RouteId', hidden: true },
            { name: 'RouteStudCode', index: 'RouteStudCode', editable: true, width: 70, editable: true },
            { name: 'RouteNo', index: 'RouteNo', editable: true, width: 70, align: 'center' },
            { name: 'Campus', index: 'Campus', editable: true, width: 90, editable: false, search: true },
            { name: 'Source', index: 'Source', editable: true },
            { name: 'Destination', index: 'Destination', editable: true },
            { name: 'Via', index: 'Via', editable: false, search: true },
            { name: 'NoOfStudents', index: 'NoOfStudents', width: 70, align: 'center', editable: false, search: true },
            { name: 'PrintPDF', index: 'PrintPDF', width: 50, align: 'center', search: false },
            { name: 'ShowList', index: 'ShowList', width: 50, align: 'center', search: false }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        sortname: 'Id',
        sortorder: 'Asc',
        autowidth: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var ids = jQuery(grid_selector).jqGrid('getDataIDs');
            $("tr.jqgrow:odd").addClass('RowBackGroundColor');
            for (var i = 0; i < ids.length; i++) {
                rowData = jQuery(grid_selector).jqGrid('getRowData', ids[i]);
                if (rowData.NoOfStudents == "0") {
                    //                    $(grid_selector).setCell(ids[i], "PrintPDF", "", { "background-color": "#66CCFF" });
                    $(grid_selector).jqGrid('setCell', ids[i], 'PrintPDF', '-');
                }
            }
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-th-list'></i>&nbsp;Route Student List"
    });

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
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
            {}, //Edit
            {}, //Add
              {}, {}, {})
    // For Pager Icons
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

    $("#btnSearch").click(function () {
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/RouteStudJqGrid',
                    postData: { Campus: $("#ddlcampus").val(), RouteId: $("#ddlroute").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/RouteStudJqGrid',
                    postData: { Campus: $("#ddlcampus").val(), RouteId: $("#ddlroute").val() },
                    page: 1
                }).trigger("reloadGrid");
    });


});
function OpenOldRouteStudList(RouteId, RouteStudCode) {
    var RouteIds = RouteId.toString();
    var Flag = "Old";
    window.location.href = '/Transport/RouteMasterConfiguration?longRouteId=' + RouteId + '&Flag=' + Flag + '&OldRouteStudCode=' + RouteStudCode;
}
function OpenStudentList(RouteId, RouteStudCode) {
    debugger;
    var RouteId = RouteId;
    var RouteStudCode = RouteStudCode;
    ModifiedLoadPopupDynamicaly("/Transport/RouteStudentConfigurationForm?RouteStudCode=" + RouteStudCode + '&RouteId=' + RouteId, $('#StudentConfig'), function () { }, "", 1000, 450, "Configured Student List");
}