jQuery(function ($) {
    //resize to fit page size
   $(window).on('resize.jqGrid', function () {
       $("#FuelRefillListGrid").jqGrid('setGridWidth', $(".tab-content").width());
   })
    //resize on sidebar collapse/expand
    var fuelparent_column = $("#FuelRefillListGrid").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#FuelRefillListGrid").jqGrid('setGridWidth', fuelparent_column.width());
            }, 0);
        }
    })

    var VehicleId = $("#hdnVehicleId").val();
    jQuery("#FuelRefillListGrid").jqGrid({
        url: '/Transport/FuelRefillListJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'Fuel Type', 'Fuel Quantity', 'Litre Price', 'Total Price', 'Filled By', 'Filled Date', 'Bunk Name', 'Fuel Fill Type '
            , 'Last Milometer Reading', 'Current Milometer Reading', 'Mileage', 'Created Date', 'Created By', 'Status'],
        colModel: [
        //if any column added need to check formateadorLink
             {name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'VehicleId', index: 'VehicleId', hidden: true },
             { name: 'VehicleNo', index: 'VehicleNo',width:100 },
             { name: 'FuelType', index: 'FuelType',width:60 },
             { name: 'FuelQuantity', index: 'FuelQuantity',width:50 },
             { name: 'LitrePrice', index: 'LitrePrice', width: 50 },
             { name: 'TotalPrice', index: 'TotalPrice', width: 60 },
             { name: 'FilledBy', index: 'FilledBy',width:100  },
             { name: 'FilledDate', index: 'FilledDate', width: 100 },
             { name: 'BunkName', index: 'BunkName', width: 80 },
             { name: 'FuelFillType', index: 'FuelFillType', width: 100 },
             { name: 'LastMilometerReading', index: 'LastMilometerReading', width: 100 },
             { name: 'CurrentMilometerReading', index: 'CurrentMilometerReading', width: 100 },
             { name: 'Mileage', index: 'Mileage',width: 100 },
             { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
             { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
             { name: 'Status', index: 'Status', hidden: true },
             ],
        pager: '#FuelRefillListGridPager',
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: 130,
        //autowidth:true,
        //shrinktofit: true,
        viewrecords: true,
        gridComplete: function () {
            RowList = $('#FuelRefillListGrid').getDataIDs();
            if (RowList != null) {
                var rowData = jQuery('#FuelRefillListGrid').jqGrid('getRowData', RowList[0]);
                if (rowData != null)
                    $("#LastMiloMeterReading").val(rowData.CurrentMilometerReading);
                else
                    $("#LastMiloMeterReading").val(0);
            }
        },
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: 'Fuel Refill List'
    });
    
    var Type = $("#Type").val();
    //VehicleFuelReportChart(VehicleId);
    $("#btnNew").click(function () {
        window.location.href = '/Transport/FuelRefill';
    });
    $("#CurrentMiloMeterReading, #FuelQuantity").keyup(function () {

        CalculateMileage();
    });
    $("#FuelFillType").change(function () {

        CalculateMileage();
    });

    $("#FilledBy").autocomplete({
        source: function (request, response) {
            var Campus = $("#Campus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });
    $('#FuelRefillListGrid').jqGrid('filterToolbar', { searchOnEnter: true });

    $("#FuelQuantity, #LitrePrice").keyup(function () {
        CalculateTotalPrice();
    });
    //navButtons
    jQuery("#FuelRefillListGrid").jqGrid('navGrid', '#FuelRefillListGridPager',
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
            { url: '/Transport/DeleteFuelRefillId' }, {})

    $("#FuelRefillListGrid").jqGrid('navButtonAdd', '#FuelRefillListGridPager', {
        caption: '<i class="fa fa-file-excel-o"></i>&nbsp; Export Excel',
        onClickButton: function () {
            var VehicleNo = $("#gs_VehicleNo").val();
            var FuelType = $("#gs_FuelType").val();
            var FuelQuantity = $("#gs_FuelQuantity").val();
            var FilledBy = $("#gs_FilledBy").val();
            var FilledDate = $("#gs_FilledDate").val();
            var BunkName = $("#gs_BunkName").val();
            var FuelFillType = $("#gs_FuelFillType").val();
            var LastMilometerReading = $("#gs_LastMilometerReading").val();
            var CurrentMilometerReading = $("#gs_CurrentMilometerReading").val();
            var Mileage = $("#gs_Mileage").val();
            var CreatedDate = $("#gs_CreatedDate").val();
            var CreatedBy = $("#gs_CreatedBy").val();
            window.open("/Transport/FuelRefillListJqGrid" + '?ExportType=Excel'
                    + '&VehicleId=' + VehicleId
                    + '&VehicleNo=' + VehicleNo
                    + '&FuelQuantity=' + FuelQuantity
                    + '&FilledBy=' + FilledBy
                    + '&FilledDate=' + FilledDate
                    + '&BunkName=' + BunkName
                    + '&FuelFillType=' + FuelFillType
                    + '&LastMilometerReading=' + LastMilometerReading
                    + '&CurrentMilometerReading=' + CurrentMilometerReading
                    + '&Mileage=' + Mileage
                    + '&CreatedDate=' + CreatedDate
                    + '&CurrentMilometerReading=' + CreatedBy
                    + '&rows=9999');
        }
    });
    function formateadorLink(cellvalue, options, rowObject) {

        return "<a href=/Transport/ShowFuelRefill?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
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


    $(document).on('ajaxloadstart', function (e) {
        $("#FuelRefillListGrid").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

});

function FuelRefillCreate() {

    var VehicleNo = $('#VehicleNo').val();
    var FuelType = $('#FuelType').val();
    var FuelQuantity = $('#FuelQuantity').val();
    var FilledDate = $('#FilledDate').val();
    var FilledBy = $('#FilledBy').val();
    var BunkName = $('#BunkName').val();
    var VehicleId = $("#hdnVehicleId").val();
    var Type = $("#Type").val();
    var FuelFillType = $("#FuelFillType").val();
    var LastMilometerReading = $("#LastMiloMeterReading").val();
    var CurrentMilometerReading = $("#CurrentMiloMeterReading").val();
    var LitrePrice = $("#LitrePrice").val();
    var TotalPrice = $("#TotalPrice").val();

    if (VehicleNo == '' || FuelType == '' || FuelQuantity == '' || FilledDate == '' || FilledBy == '' || BunkName == '' || VehicleId == '' || Type == '' || FuelFillType == '' || LitrePrice == '' || TotalPrice == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }

    if (isNaN(FuelQuantity)) {
        ErrMsg('Numbers only allowed.');
        $('#FuelQuantity').focus();
        return false;
    }

    if (FuelFillType == "Full Tank") {
        var Mileage = (parseFloat(CurrentMilometerReading) - parseFloat(LastMilometerReading)) / parseFloat(FuelQuantity);
    }

    $.ajax({
        type: 'POST',
        url: "/Transport/AddFuelRefill",
        data: { VehicleId: VehicleId, Type: Type, VehicleNo: VehicleNo, FuelType: FuelType, FuelQuantity: FuelQuantity, FilledDate: FilledDate, FilledBy: FilledBy
            , BunkName: BunkName, FuelFillType: FuelFillType, LastMilometerReading: LastMilometerReading, CurrentMilometerReading: CurrentMilometerReading, Mileage: Mileage
            , LitrePrice: LitrePrice, TotalPrice: TotalPrice
        },
        success: function (data) {
            $("#FuelRefillListGrid").trigger('reloadGrid');
            //  $("input[type=text], textarea").val("");
            $('#FuelQuantity').val('');
            $('#FilledDate').val('');
            $('#FilledBy').val('');
            $('#BunkName').val('');
            $("#FuelFillType").val('');
            $("#CurrentMiloMeterReading").val('');
            $("#Distance").text('');
            $("#Mileage").text('');
            $("#LitrePrice").val('');
            $("#TotalPrice").val('');
        }
    });

    //VehicleFuelReportChart(VehicleId)
}
function CalculateMileage() {
    var FuelFillType = $("#FuelFillType").val();
    if (FuelFillType == "Full Tank") {
        var FuelQuantity = $('#FuelQuantity').val();
        var LastMilometerReading = $("#LastMiloMeterReading").val();
        var CurrentMilometerReading = $("#CurrentMiloMeterReading").val();
        var Distance = (parseFloat(CurrentMilometerReading) - parseFloat(LastMilometerReading));
        var Mileage = Math.round(parseFloat(Distance) / parseFloat(FuelQuantity));
        $("#Distance").text(Distance);
        $("#Mileage").text(Mileage);
    }
    else {
        $("#Distance").text('');
        $("#Mileage").text('');
    }
}
function VehicleFuelReportChart(VehicleId) {
    $.ajax({
        type: 'Get',
        url: '/Transport/VehicleFuelReportChart/?VehicleId=' + VehicleId,
        success: function (data) {
            var chart = new FusionCharts("../../Charts/FCF_Column3D.swf", "productSales", "400", "220");
            chart.setDataXML(data);
            chart.render("VehicleFuelChart");
        },
        async: false,
        dataType: "text"
    });
}
function CalculateTotalPrice() {
    var FuelQuantity = $("#FuelQuantity").val();
    var LitrePrice = $("#LitrePrice").val();
    if (FuelQuantity != '' && LitrePrice != '')
        $("#TotalPrice").val(parseFloat(FuelQuantity) * parseFloat(LitrePrice));
    else
        $("#TotalPrice").val('');
}
