jQuery(function ($) {
    var grid_selector = "#DriverOTAllowanceReport";
    var pager_selector = "#DriverOTAllowanceReportPager";

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
    var DriverName;
    var Campus;
    var DriverIdNo;
    var FromDate;
    var ToDate;

    jQuery(grid_selector).jqGrid({
        url: '/Transport/DriverOTAllowanceJqGridNew',
        datatype: 'json',
        height: 250,
        colNames: ['Id', 'Campus', 'Driver Name', 'DriverId No', 'From Date', 'To Date', 'Evening', 'Evening Allowance', 'Night', 'Night Allowance', 'OutStation', 'OutStation Allowance', 'Holiday', 'Holiday Allowance', 'Remedial', 'Remedial Allowance', 'Total OT Count', 'Total Allowance'],
        colModel: [
                { name: 'Id', index: 'Id', hidden: true },
                { name: 'Campus', index: 'Campus' },
                { name: 'DriverName', index: 'DriverName', width: '250px' },
                { name: 'DriverIdNo', index: 'DriverIdNo', width: '250px' },
                { name: 'FromDate', index: 'FromDate' },
                { name: 'ToDate', index: 'ToDate' },
                { name: 'Evening', index: 'Evening' },
                { name: 'EveningAllowance', index: 'EveningAllowance' },
                { name: 'Night', index: 'Night' },
                { name: 'NightAllowance', index: 'NightAllowance' },
                { name: 'OutStation', index: 'OutStation' },
                { name: 'OutStationAllowance', index: 'OutStationAllowance' },
                { name: 'Holiday', index: 'Evening' },
                { name: 'HolidayAllowance', index: 'HolidayAllowance' },
                { name: 'Remedial', index: 'Remedial' },
                { name: 'RemedialAllowance', index: 'RemedialAllowance' },
                { name: 'TotalOTCount', index: 'TotalOTCount' },
                { name: 'TotalAllowance', index: 'TotalAllowance' }
              ],
        viewrecords: true,
        rowNum: 10,
        rowList: [25, 50, 100, 500],
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
        caption: "<i class='ace-icon fa fa-user'></i>&nbsp;Driver OT Allowance Report"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    // $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
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

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
        onClickButton: function () {
            debugger;
            DriverName = $("#txtDriverName").val();
            DriverIdNo = $("#txtDriverNo").val();
            Campus = $("#Campus").val();
            FromDate = $("#txtFromDate").val();
            ToDate = $("#txtToDate").val();
            window.open("/Transport/DriverOTAllowanceJqGridNew" + '?DriverName=' + DriverName + '&DriverIdNo=' + DriverIdNo + '&Campus=' + Campus + '&FromDate=' + FromDate + '&ToDate=' + ToDate + ' &rows=9999 ' + '&ExptXl=1');
        }
    });

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
    $("#btnSearch").click(function () {
        DriverName = $("#txtDriverName").val();
        DriverIdNo = $("#txtDriverNo").val();
        Campus = $("#Campus").val();
        FromDate = $("#txtFromDate").val();
        ToDate = $("#txtToDate").val();
        if (Campus == '') {
            ErrMsg("Please select Campus");
            return false;
        };
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/DriverOTAllowanceJqGridNew',
                    postData: { DriverName: DriverName, DriverIdNo: DriverIdNo, Campus: Campus, FromDate: FromDate, ToDate: ToDate },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#Reset").click(function () {
        $("input[type=text], textarea, select").val("");
        DriverName = $("#txtDriverName").val();
        DriverIdNo = $("#txtDriverNo").val();
        Campus = $("#Campus").val();
        FromDate = $("#txtFromDate").val();
        ToDate = $("#txtToDate").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/DriverOTAllowanceJqGridNew',
                    postData: { DriverName: DriverName, DriverIdNo: DriverIdNo, Campus: Campus, FromDate: FromDate, ToDate: ToDate },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#txtDriverName").autocomplete({
        source: function (request, response) {
            var Campus = $("#Campus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        select: function (event, ui) {
            debugger;
            FillDriverIdNo(ui.item.value);
        },
        minLength: 1,
        delay: 100
    });
    $("#Campus").change(function () {
        $("#DriverIdNo").val('');
        $.getJSON("/Transport/GetDriverByCampus?Campus=" + $(this).val(),
             function (fillig) {
                 var Dri = $("#txtDriverName");
                 Dri.empty();
                 Dri.append($('<option/>',
                {
                    value: "",
                    text: "Select One"

                }));
                 $.each(fillig, function (index, itemdata) {
                     Dri.append($('<option/>',
                         {
                             value: itemdata.Value,
                             text: itemdata.Text
                         }));
                 });

             });
    });
    function FillDriverIdNo(DriverName) {
        debugger;
        var Campus = $("#Campus").val();
        $.ajax({
            type: 'POST',
            url: "/Transport/GetDriverDetailsByNameAndCampus",
            data: { Campus: Campus, DriverName: DriverName },
            success: function (data) {
                $("#txtDriverNo").val(data.DriverIdNo);
            }
        });
    }

});