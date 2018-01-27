jQuery(function ($) {
    
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#PermitJqGrid").jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $("#PermitJqGrid").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#PermitJqGrid").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    var VehicleId = $("#hdnVehicleId").val();
    var Type = $("#Type").val();
    $("#PermitJqGrid").jqGrid({
        url: '/Transport/PermitJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'Permit No', 'Valid In', 'Valid From', 'Valid To', 'Created Date', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true, width: 190 },
            { name: 'VehicleId', index: 'VehicleId', hidden: true, width: 190 },
            { name: 'VehicleNo', index: 'VehicleNo' },
            { name: 'PermitNo', index: 'PermitNo' },
            { name: 'ValidIn', index: 'ValidIn' },
            { name: 'ValidFrom', index: 'ValidFrom' },
            { name: 'ValidTo', index: 'ValidTo' },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true, width: 190 },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true, width: 190 }
            ],
        pager: '#PermitPager',
        rowNum: '10',
        rowList: [10, 15, 20],
        sortname: 'Id',
        sortorder: 'Desc',
        height: 105,
        //shrinkToFit: true,
        viewrecords: true,
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            //$(window).triggerHandler('resize.jqGrid');
        },
        caption: 'Permit'
    });
    $(window).triggerHandler('resize.jqGrid');
    $("#PermitJqGrid").jqGrid('filterToolbar', { searchOnEnter: true });
    //navButtons
    jQuery("#PermitJqGrid").jqGrid('navGrid', '#PermitPager',
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
            }, {}, {},
            { url: '/Transport/DeletePermitDetailsById' }, {})

    $("#PermitJqGrid").jqGrid('navButtonAdd', '#PermitPager', {
        caption: '<i class="fa fa-file-excel-o"></i>&nbsp Export Excel',
        onClickButton: function () {
            window.open('/Transport/PermitJqGrid?ExportType=Excel'
                    + '&VehicleId=' + $("#gs_VehicleId").val()
                    + '&VehicleNo=' + $("#gs_VehicleNo").val()
                    + '&PermitNo=' + $("#gs_PermitNo").val()
                    + '&ValidIn=' + $("#gs_ValidIn").val()
                    + '&ValidFrom=' + $("#gs_ValidFrom").val()
                    + '&ValidTo=' + $("#gs_ValidTo").val()
                    + '&CreatedDate=' + $("#gs_CreatedDate").val()
                    + '&CreatedBy=' + $("#gs_CreatedBy").val()
                    + '&rows=9999');
        }
    });
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
        $("#PermitJqGrid").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

});

function ValidatePermitDetails() {
    var VehicleId = $('#hdnVehicleId').val();
    var VehicleNo = $('#VehicleNo').val();
    var PermitNo = $('#PermitNo').val();
    var ValidIn = $('#ValidIn').val();
    var ValidFrom = $('#ValidFrom').val();
    var ValidTo = $('#ValidTo').val();

    if (VehicleId == '' || VehicleNo == '' || PermitNo == '' || ValidIn == '' || ValidFrom == '' || ValidTo == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }

    $.ajax({
        type: 'POST',
        url: "/Transport/AddPermitDetails",
        data: { VehicleId: VehicleId, VehicleNo: VehicleNo, PermitNo: PermitNo, ValidIn: ValidIn, ValidFrom: ValidFrom, ValidTo: ValidTo },
        success: function (data) {
            $("#PermitJqGrid").trigger('reloadGrid');
            $("input[type=text], textarea, select").val("");
        }
    });

    $('#PermitNo').val("");
    $('#ValidIn').val("");
    $('#ValidFrom').val("");
    $('#ValidTo').val("");
}