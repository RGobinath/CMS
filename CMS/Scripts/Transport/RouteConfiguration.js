jQuery(function ($) {
    var Campus = $("#Campus").val();
//    $("#LocationName").autocomplete({
//        source: function (request, response) {
//            $.getJSON('/Transport/GetLocationNames?term=' + request.term + '&Campus=' + $("#Campus").val(), function (data) {
//                response(data);
//            });
//        },
//        minLength: 1,
//        delay: 0
//    });
//    $("#LocationName").on('change', function () {
//        $.ajax({
//            type: 'POST',
//            async: false,
//            url: '/Transport/GetStudentLocationMasterByLocation?Location=' + $('#LocationName').val(),
//            success: function (data) {
//                if (data == false) {
//                    InfoMsg("Please choose Location Name using Autofill!");
//                    $("#LocationName").val("");
//                    return false;
//                }
//            }
//        });
//    });

    $('#btnBack').click(function () {
        window.location.href = '/Transport/TransportMasters';
    });
    var grid_selector = "#jqGridRouteConfiguration";
    var pager_selector = "#jqGridRouteConfigurationPager";

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
    var RouteMasterId = $("#Id").val();
    jQuery(grid_selector).jqGrid({
        url: '/Transport/RouteConfiguraionJqGrid?RouteMasterId=' + RouteMasterId + '&LocationName=' + $('#LocationName').val() + '&Campus=' + $("#Campus").val(),
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'Route Master Id', 'LocationName','LocationDetails', 'StopOrderNumber', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            { name: 'RouteMasterId', index: 'RouteMasterId', editable: false, search: false, hidden: true },
            { name: 'LocationName', index: 'LocationName', editable: true },
            { name: 'LocationDetails', index: 'LocationDetails', editable: true },
            { name: 'StopOrderNumber', index: 'StopOrderNumber', editable: true },
            { name: 'CreatedBy', index: 'CreatedBy', editable: false, search: false },
             { name: 'CreatedDate', index: 'CreatedDate', editable: false, search: false, hidden: true },
            { name: 'ModifiedBy', index: 'ModifiedBy', editable: false, search: false, hidden: true },
        { name: 'ModifiedDate', index: 'ModifiedDate', editable: false, search: false, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        sortname: 'StopOrderNumber',
        sortorder: 'Asc',
        autowidth: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-th-list'></i>&nbsp;Route Configuration Details"
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
                del: true,
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
              {
              width: 'auto', url: '/Transport/DeleteRouteConfiguration/', left: '10%', top: '10%', height: '50%', width: 400,
              beforeShowForm: function (params) { selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow"); return { Id: selectedrows} }
          }, {}, {})
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
    $("#btnAddLocations").click(function () {
        if ($('#LocationName').val() == "") {
            ErrMsg("Please fill Location name!!");
            return false;
        }

//        if ($('#LocationName').val().search(/[^A-Za-z\s]/) != -1) {
//            ErrMsg("Location Name should be Alpha!!!");
//            return false;
//        }
        if ($('#StopOrderNumber').val() == "") {
            ErrMsg("Please fill Stop Order Number!!");
            return false;
        }
        var numericValidation = isNumeric($('#StopOrderNumber').val());
        if (numericValidation == false) {
            ErrMsg("Stop Order Number should be numeric!!!");
            return false;
        }
        $.ajax({
            type: 'POST',
            async: false,
            url: '/Transport/AddRouteConfigurationDetails?RouteMasterId=' + RouteMasterId + '&LocationName=' + $('#LocationName').val() + '&StopOrderNumber=' + $('#StopOrderNumber').val(),
            success: function (data) {
                $('#LocationName').val("");
                $('#StopOrderNumber').val("")
                if (data != null)
                    InfoMsg(data);
                else
                    InfoMsg("Location added Unsuccessfully!");
            }
        });
        $(grid_selector).trigger("reloadGrid");
    });
    function isNumeric(n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    }
});
