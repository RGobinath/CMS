jQuery(function ($) {
    var grid_selector = "#FuelRefilListDetails";
    var pager_selector = "#FuelRefilListDetailsJqGridPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#FuelRefilListDetails").jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var fuelparent_column = $("#FuelRefilListDetails").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#FuelRefilListDetails").jqGrid('setGridWidth', fuelparent_column.width());
            }, 0);
        }
    })
    var VehicleId = $("#hdnVehicleId").val();
    jQuery(grid_selector).jqGrid({
        url: '/Transport/FuelRefilListDetailsJqGrid/?VehicleId=' + VehicleId,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Vehicle Id', 'Campus', 'Vehicle No', 'Fuel Type', 'Fuel Quantity', 'Litre Price', 'Total Price', 'Filled By', 'Filled Date', 'Bunk Name', 'Fuel Fill Type '
            , 'Is KM Reseted', 'KM Reset Value', 'Last Milometer Reading', 'Current Milometer Reading', 'Distance', 'Mileage', 'Created Date', 'Created By', 'Status'],
        colModel: [
        //if any column added need to check formateadorLink
             { name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'VehicleId', index: 'VehicleId', hidden: true },
             { name: 'Campus', index: 'Campus', width: 70 },
             { name: 'VehicleNo', index: 'VehicleNo', width: 100 },
             { name: 'FuelType', index: 'FuelType', width: 60 },
             { name: 'FuelQuantity', index: 'FuelQuantity', width: 50 },
             { name: 'LitrePrice', index: 'LitrePrice', width: 50 },
             { name: 'TotalPrice', index: 'TotalPrice', width: 60 },
             { name: 'FilledBy', index: 'FilledBy', width: 80 },
             { name: 'FilledDate', index: 'FilledDate', width: 90 },
             { name: 'BunkName', index: 'BunkName', width: 60 },
             { name: 'FuelFillType', index: 'FuelFillType', width: 70 },
             { name: 'IsKMReseted', index: 'IsKMReseted', width: 30 },
             { name: 'KMResetValue', index: 'KMResetValue', width: 70 },
             { name: 'LastMilometerReading', index: 'LastMilometerReading', width: 80 },             
             { name: 'CurrentMilometerReading', index: 'CurrentMilometerReading', width: 80 },
             { name: 'Distance', index: 'Distance', width: 70 },
             { name: 'Mileage', index: 'Mileage', width: 50 },
             { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
             { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
             { name: 'Status', index: 'Status', hidden: true },
        ],
        pager: '#FuelRefilListDetailsJqGridPager',
        rowNum: '20',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: 440,
        autowidth:true,
        shrinktofit: true,
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
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: 'Fuel Refill List'
    });
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
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
            }, {}, {}, { url: '/Transport/DeleteFuelRefillId' }, {})

    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
        onClickButton: function () {
            var VehicleId = $("#hdnVehicleId").val();
            var VehicleNo = $("#VehicleNo").val();
            var FuelType = $("#FuelType").val();
            var FuelQuantity = $("#FuelQuantity").val();
            var FilledBy = $("#FilledBy").val();
            var FilledDate = $("#FilledDate").val();
            var BunkName = $("#BunkName").val();
            var FuelFillType = $("#FuelFillType").val();
            var LastMilometerReading = $("#LastMilometerReading").val();
            var CurrentMilometerReading = $("#CurrentMilometerReading").val();
            var Mileage = $("#Mileage").val();
            var CreatedDate = $("#CreatedDate").val();
            var CreatedBy = $("#CreatedBy").val();
            window.open("/Transport/FuelRefilListDetailsJqGrid" + '?ExportType=Excel'
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
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    $("#ddlIsReset").val('False');
    $("#KMResetValue").hide();
    $("#ddlIsReset").change(function () {
        var IsReset = $("#ddlIsReset").val();
        if (IsReset == "True") {
            $("#KMResetValue").show();
            $("#CurrentMiloMeterReading").val('');
            //$("#KMIn").val('');
            //$("#KMOut").val('');
            $("#Distance").text('');
            $("#Mileage").text('');
        }
        if (IsReset == "False" || IsReset == "" || IsReset == "null" || IsReset == "undefined") {
            $("#KMResetValue").hide();
            $("#CurrentMiloMeterReading").val('');
            //$("#KMIn").val('');
            //$("#KMOut").val('');
            $("#Distance").text('');
            $("#Mileage").text('');
            $("#txtKMResetValue").val('');
        }
    });
    $("#VehicleNo").autocomplete({
        source: function (request, response) {
            var Campus = $("#ddlcampus").val();
            $.getJSON('/Transport/GetVehicleNo?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        select: function (event, ui) {
            FillVehicleType(ui.item.value);
        },
        minLength: 1,
        delay: 100
    });

    //VehicleFuelReportChart(VehicleId);

    $("#btnNew").click(function () {
        window.location.href = '/Transport/FuelRefilDetails';
    });
    $("#CurrentMiloMeterReading, #FuelQuantity").keyup(function () {

        CalculateMileage();
    });
    $("#txtKMResetValue").keyup(function () {
        CalculateMileage();
    });
    $("#FuelFillType").change(function () {

        CalculateMileage();
    });
    $("#FuelQuantity").keyup(function () {
        NumbersOnly($(this).val());
    });
    $("#txtKMResetValue").keyup(function () {
        NumbersOnly($(this).val());
    });
    $("#ddlcampus").change(function () {
        $("#VehicleNo").val('');
        $("input[type=text], textarea").val("");
        $("#FuelFillType").val('');        
        $("#Distance").text('');
        $("#Mileage").text('');        
        $("#txtKMResetValue").val('');
    });
    $("#VehicleNo").focus(function () {
        if ($("#ddlcampus").val() == "") {

            ErrMsg("Please Fill Campus");
            return $("#VehicleNo").val('');
        }
    })
    function NumbersOnly(value) {
        if (isNaN(value)) {
            ErrMsg("Numbers only allowed.");
            return false;
        }
    }
    $("#FilledBy").autocomplete({
        source: function (request, response) {
            var Campus = $("#ddlcampus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });
    $('#FuelRefilListDetails').jqGrid('filterToolbar', { searchOnEnter: true });

    $("#FuelQuantity, #LitrePrice").keyup(function () {
        CalculateTotalPrice();
    });


    //function formateadorLink(cellvalue, options, rowObject) {

    //    return "<a href=/Transport/ShowFuelRefill?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
    //}
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
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $("#ddlIsReset").val('False');
        $("#KMResetValue").hide();
    });

});

function FuelRefillCreate() {
    debugger;
    var VehicleId = $('#hdnVehicleId').val();
    var VehicleNo = $('#VehicleNo').val();
    var Campus = $('#ddlcampus').val();
    var FuelType = $('#FuelType').val();
    var FuelQuantity = $('#FuelQuantity').val();
    var FilledDate = $('#FilledDate').val();
    var FilledBy = $('#FilledBy').val();
    var BunkName = $('#BunkName').val();
    //var Type = $("#Type").val();
    var FuelFillType = $("#FuelFillType").val();
    var LastMilometerReading = $("#LastMiloMeterReading").val();
    var CurrentMilometerReading = $("#CurrentMiloMeterReading").val();
    var LitrePrice = $("#LitrePrice").val();
    var TotalPrice = $("#TotalPrice").val();
    var KMReseted = $("#ddlIsReset").val();
    var KMResetVal = $("#txtKMResetValue").val();
    var Mileage = $("#Mileage").text();
    var Distance = $("#Distance").text();
    if (Campus == '' || VehicleNo == '' || FuelType == '' || FuelQuantity == '' || FilledDate == '' || FilledBy == '' || BunkName == '' || FuelFillType == '' || LitrePrice == '' || TotalPrice == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    if (isNaN(FuelQuantity)) {
        ErrMsg('Numbers only allowed.');
        $('#FuelQuantity').focus();
        return false;
    }
    //if (FuelFillType == "Full Tank") {
    //    var Mileage = (parseFloat(CurrentMilometerReading) - parseFloat(LastMilometerReading)) / parseFloat(FuelQuantity);
    //}
    if (CurrentMilometerReading != '') {
        //var IsKMReseted = $("#ddlIsReset").val();
        //var KMResetValue = $("#txtKMResetValue").val();
        if (KMReseted == "False" || KMReseted == "") {
            if (parseFloat(CurrentMilometerReading) <= parseFloat(LastMilometerReading)) {
                ErrMsg("Current MiloMeter Reading should be greater than Last MiloMeter Reading");
                $('#CurrentMiloMeterReading').val('');
                $("#Distance").text('');
                $("#Mileage").text('');
                return false;
            }
        }
        if (KMReseted == "True") {
            if (parseFloat(LastMilometerReading) > parseFloat(KMResetVal)) {
                ErrMsg("KM Reset Value Must be Greater than Last MiloMeter Reading");
                $("#txtKMResetValue").val('');
                return false;
            }
            //if (parseFloat(LastMilometerReading) < parseFloat(KMResetVal)) {
            //    if (parseFloat(CurrentMilometerReading) > parseFloat(LastMilometerReading)) {
            //        ErrMsg(" Last MiloMeter Reading should be greater than Current Milo Meter Reading when Reset");
            //        $('#CurrentMiloMeterReading').val('');
            //        $("#Distance").text('');
            //        $("#Mileage").text('');
            //        return false;
            //    }
            //}
        }
    }
    $.ajax({
        type: 'POST',
        url: "/Transport/AddFuelRefill",
        data: {
            VehicleId: VehicleId, Campus: Campus, VehicleNo: VehicleNo, FuelType: FuelType, FuelQuantity: FuelQuantity, FilledDate: FilledDate, FilledBy: FilledBy, BunkName: BunkName, FuelFillType: FuelFillType, LastMilometerReading: LastMilometerReading, CurrentMilometerReading: CurrentMilometerReading, Mileage: Mileage, LitrePrice: LitrePrice, TotalPrice: TotalPrice, KMResetValue: KMResetVal, IsKMReseted: KMReseted,Distance:Distance
        },
        success: function (data) {            
            $("#FuelRefilListDetails").trigger('reloadGrid');
            $("input[type=text], textarea").val("");
            $('#hdnVehicleId').val('');
            $('#ddlcampus').val('');
            //$("#VehicleNo").val('');
            //$('#FuelType').val('');
            //$('#FuelQuantity').val('');
            //$('#FilledDate').val('');
            //$('#FilledBy').val('');
            //$('#BunkName').val('');
            $("#FuelFillType").val('');
            //$("#CurrentMiloMeterReading").val('');
            $("#Distance").text('');
            $("#Mileage").text('');
            //$("#LitrePrice").val('');
            //$("#TotalPrice").val('');
            $("#txtKMResetValue").val('');
            $("#KMResetValue").hide();
            $("#ddlIsReset").val('False');
        }

    });

    //VehicleFuelReportChart(VehicleId)
}
function CalculateMileage() {
    var FuelFillType = $("#FuelFillType").val();
    var IsReset = $("#ddlIsReset").val();
    if (FuelFillType == "Full Tank") {
        if (IsReset == "False" || IsReset == "") {
            var FuelQuantity = $('#FuelQuantity').val();
            var LastMilometerReading = $("#LastMiloMeterReading").val();
            var CurrentMilometerReading = $("#CurrentMiloMeterReading").val();
            if (CurrentMilometerReading != "") {
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
        if (IsReset == "True") {
            var FuelQuantity = $('#FuelQuantity').val();
            var LastMilometerReading = $("#LastMiloMeterReading").val();
            var CurrentMilometerReading = $("#CurrentMiloMeterReading").val();
            if (CurrentMilometerReading != "" && $("#txtKMResetValue").val() != "") {
                var Distance = (parseFloat($("#txtKMResetValue").val()) - parseFloat(LastMilometerReading) + parseFloat(CurrentMilometerReading));
                var Mileage = Math.round(parseFloat(Distance) / parseFloat(FuelQuantity));
                $("#Distance").text(Distance);
                $("#Mileage").text(Mileage);
            }
            else {
                $("#Distance").text('');
                $("#Mileage").text('');
            }
        }
    }
    else {
        $("#Distance").text('');
        $("#Mileage").text('');
    }
}

function CalculateTotalPrice() {
    var FuelQuantity = $("#FuelQuantity").val();
    var LitrePrice = $("#LitrePrice").val();
    if (FuelQuantity != '' && LitrePrice != '')
        $("#TotalPrice").val(parseFloat(FuelQuantity) * parseFloat(LitrePrice));
    else
        $("#TotalPrice").val('');
}

function FillVehicleType(VehicleNo) {
    $.ajax({
        type: 'POST',
        url: "/Transport/GetVehicleDetails",
        data: { VehicleNo: VehicleNo },
        success: function (data) {
            var VehicleId = $("#hdnVehicleId").val();
            var CurrentMilometerReading = $("#CurrentMiloMeterReading").val();
            debugger;
            $("#FuelType").val(data.FuelType);
            $("#LastMiloMeterReading").val(data.CurrentMilometerReading);
            $("#hdnVehicleId").val(data.VehicleId);
        }
    });
}


