$(function ($) {
    var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    $("#StartKmrs").val($("#hdnstartkmrs").val());
    if ($("#hdnstartkmrs").val() > 0) {
        $("#StartKmrs").attr("disabled", true);
    }
    else {
        $("#StartKmrs").attr("disabled", false);
    }
    $("#ddlIsKMReseted").val('false');
    $("#divisreset").hide();
    $("#divresetvalue").hide();
    $("#ddlIsKMReseted").change(function () {
        if ($("#ddlIsKMReseted").val() == "true") {
            $("#divresetvalue").show();
            $("#txtKMResetValue").attr("disabled", false);
        }
        else {
            $("#txtKMResetValue").attr("disabled", true);
            $("#divresetvalue").hide();
        }
    });
    $.getJSON("/Transport/Purposeddl",
  function (fillig) {
      //debugger;
      var ddlcam = $("#ddlPurposeName");
      ddlcam.empty();
      ddlcam.append($('<option/>', { value: "", text: "Select" }));
      $.each(fillig, function (index, itemdata) {

          ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
      });
  });
    var grid_selector = "#VehicleCostDetailsJqGrid";
    var pager_selector = "#VehicleCostDetailsJqGridPager";

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
    //$("#txtMonthYear").datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    showButtonPanel: true,
    //    dateFormat: 'MM yy',
    //    onClose: function (dateText, inst) {
    //        $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
    //    }
    //});
    $('#txtVehicleTravelDate').datepicker({
        format: 'dd/mm/yyyy',
        maxDate: 0,
        numberOfMonths: 1,
        autoclose: true,
        endDate: new Date(),
    });
    $('#txtVehicleTravelDate').datepicker();

    var oldAddRowData = $.fn.jqGrid.addRowData;

    var VehicleId = $("#hdnVehicleId").val();

    jQuery(grid_selector).jqGrid({

        url: '/Transport/VehicleCostDetails_UpdatedJqGrid?VehicleId=' + VehicleId,
        datatype: 'json',
        height: 200,
        colNames: ['Vehicle Cost Id', 'Vehicle Id', 'Date', 'Entry Type', 'Campus', 'Trip Purpose', 'Vehicle No', 'Driver Name', 'Helper Name', 'Vehicle Route', 'Start Kms', 'End Kms', 'Is Kms Reseted', 'Kms Reseted Value', 'Trip Kms', 'Driver OT(Rs.)', 'Helper OT(Rs.)', 'Fuel(Rs.)', 'Maintenance(Rs.)', 'Service(Rs.)', 'FC(Rs.)', 'Other(Toll,fine and accident)', 'Created Date', 'Created By', 'Modified Date', 'Modified By', ],
        colModel: [
            { name: 'VehicleCostId', index: 'VehicleCostId', editable: true, hidden: true },
            { name: 'VehicleId', index: 'VehicleId', editable: true, hidden: true },
            {
                name: 'VehicleTravelDate', index: 'VehicleTravelDate', editable: true, width: 83,

                formoptions: { rowpos: 3, colpos: 3 }, editoptions: {
                    dataInit: function (el) {
                        $(el).keypress(function (e) {
                            if (e.which != 8 && e.which != 0 && e.which != 47 && (e.which < 48 || e.which > 57)) {
                                return false;
                            }
                        });
                        var elid = el.id;
                        elid = elid.slice(0, -11);
                        $(el).datepicker({
                            format: 'mm/dd/yyyy',
                            autoclose: true
                        });

                    }
                }
            },
              { name: 'EntryType', index: 'EntryType', width: 90, editable: true, sortable: false },
             {
                 name: 'Campus', index: 'Campus', editable: true, edittype: 'select',
                 editoptions: { dataUrl: '/Assess360/GetCampusddl' },
                 editrules: { required: true, custom: true, custom_func: checkvalid },
                 sortable: true, hidden: true
             },
             { name: 'TypeOfTrip', index: 'TypeOfTrip', width: 90, editable: true, sortable: false },
             { name: 'VehicleNo', index: 'VehicleNo', width: 90, editable: true, sortable: false, hidden: true },
             { name: 'DriverMaster.Id', index: 'DriverMaster.Id', width: 90, editable: true, sortable: false },
             { name: 'StaffDetails.Id', index: 'StaffDetails.Id', width: 90, editable: true, sortable: false },
             { name: 'VehicleRoute', index: 'VehicleRoute', width: 90, editable: true, sortable: false },
             { name: 'StartKmrs', index: 'StartKmrs', width: 100, editable: true, sortable: false },
             { name: 'EndKmrs', index: 'EndKmrs', width: 150, editable: true, sortable: false },
             { name: 'IsKMReseted', index: 'IsKMReseted', width: 80, editable: true, sortable: false },
             { name: 'KMResetValue', index: 'KMResetValue', width: 80, editable: true, sortable: false },
             { name: 'Distance', index: 'Distance', width: 60, editable: true, sortable: false },
             { name: 'DriverOt', index: 'DriverOt', width: 60, editable: true, sortable: false },
             { name: 'HelperOt', index: 'HelperOt', width: 60, editable: true, sortable: false },
             { name: 'Diesel', index: 'Diesel', width: 90, editable: true, sortable: false, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' }, formatter: formateadorLink },
             { name: 'Maintenance', index: 'Maintenance', width: 90, editable: true, sortable: false, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' }, formatter: formateadorLink1 },
             { name: 'Service', index: 'Service', width: 90, editable: true, sortable: false, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' }, formatter: formateadorLink2 },
             { name: 'FC', index: 'FC', width: 90, editable: true, sortable: false, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' }, formatter: formateadorLink3 },
             { name: 'Others', index: 'Others', width: 90, editable: true, sortable: false, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' }, formatter: formateadorLink4 },
             { name: 'CreatedBy', index: 'CreatedBy', editable: true, hidden: true },
             { name: 'CreatedDate', index: 'CreatedDate', editable: true, search: false, hidden: true },
             { name: 'ModifiedBy', index: 'ModifiedBy', editable: true, hidden: true },
             { name: 'ModifiedDate', index: 'ModifiedDate', editable: true, search: false, hidden: true },
        ],

        rowNum: 10000,
        rowList: [500, 1000, 2000],
        sortname: 'CreatedDate',
        sortorder: 'Desc',
        pager: pager_selector,
        altRows: true,
        //autowidth: true,
        shrinkToFit: false,
        footerrow: true,
        loadComplete: function () {
            var table = this;
            //var $footRow = $("#VehicleCostDetailsJqGrid").closest(".ui-jqgrid-bdiv").next(".ui-jqgrid-sdiv").find(".footrow");
            //var $name = $footRow.find('>td[aria-describedby="VehicleCostDetailsJqGrid_VehicleRoute"]'),
            //    $invdate = $footRow.find('>td[aria-describedby="VehicleCostDetailsJqGrid_StartKmrs"]'),
            //    $trip = $footRow.find('>td[aria-describedby="VehicleCostDetailsJqGrid_EndKmrs"]'),
            //    width2 = $name.width() + $invdate.outerWidth() + $trip.outerWidth();
            //$invdate.css("display", "none");
            //$trip.css("display", "none");
            //$name.attr("colspan", "3").width(width2);
            $(grid_selector).footerData('set', { VehicleTravelDate: 'No. Of Trips:' });
            var ids = jQuery(grid_selector).jqGrid('getDataIDs');
            var total = 0;
            for (var i = 0; i < ids.length; i++) {
                rowData = jQuery(grid_selector).jqGrid('getRowData', ids[i]);
                if (rowData.EntryType == "Trip") {
                    total = total + 1;
                }
            }
            //$('#txtTotal').val(total);

            $(grid_selector).footerData('set', { TypeOfTrip: total });
            var table = this;
            $(grid_selector).footerData('set', { EndKmrs: 'Total No.Of Trip Kms :' });

            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            var myGrid = $("#VehicleCostDetailsJqGrid"); // your grid
            var userData = myGrid.jqGrid("getGridParam", "userData");
            $("#VehicleCostDetailsJqGrid").jqGrid("footerData", "set", userData, false);
        },

        viewrecords: true,
        sortorder: "Asc",

        editurl: '/Transport/SaveOrUpdateVehicleCostDetails_Updated',

        caption: "<i class='ace-icon fa fa-truck'> </i>&nbsp;Vehicle Daily Usage"
    });


    //$footRow.find('>td[aria-describedby="VehicleCostDetailsJqGrid_StartKmrs"]')
    //            .css("border-right-color", "transparent")

    $(window).triggerHandler('resize.jqGrid');
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
          {}, {}, {});
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            debugger;
            VehicleId = $('#hdnVehicleId').val();
            window.open("ExportToExcelVCD" + '?VehicleId=' + VehicleId + '&rows=9999');
        }
    });
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-pdf-o'></i> Export To PDF",
        onClickButton: function () {
            VehicleId = $('#hdnVehicleId').val();
            window.open("VehicleCostDetailsReportPDF" + '?VehicleId=' + VehicleId + '&rows=9999');
        }
    });

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
    function showFormattedStatus(cellvalue, options, rowObject) {
        if (cellvalue == 'True') {
            return 'Active';
        }
        else {
            return 'Inactive';
        }
    }
    function beforeDeleteCallback(e) {
        var form = $(e[0]);
        if (form.data('styled')) return false;

        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
        style_delete_form(form);

        form.data('styled', true);
    }

    function beforeEditCallback(e) {
        var form = $(e[0]);
        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
        style_edit_form(form);
    }

    function beforeAddCallback(e) {
        var form = $(e[0]);
        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
        style_add_form(form);
    }

    $("#bedata").click(function () {
        jQuery("#VehicleCostDetailsJqGrid").jqGrid('editGridRow', "new", { height: 280, reloadAfterSubmit: false });
    });

    $(grid_selector).trigger("reloadGrid");

    $("#EndKmrs").blur(function () {
        var startKms = $("#StartKmrs").val();
        var endKms = $("#EndKmrs").val();

        if (parseFloat(startKms) <= parseFloat(endKms) && $("#ddlIsKMReseted").val() == "true") {
            ErrMsg("End Kms should be less than Start Kms when Reseted Kms");
            $("#EndKmrs").val("");
            return false;
        }
        if (parseFloat(startKms) >= parseFloat(endKms) && $("#ddlIsKMReseted").val() != "true") {
            ErrMsg("End Kms should be greater than Start Kms");
            $("#EndKmrs").val("");
            return false;
        }
    });
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    var today = dd + '/' + mm + '/' + yyyy;
    $("#txtVehicleTravelDate").val(today);

    $("#btnsearch").click(function () {
        $(grid_selector).jqGrid("clearGridData", true);
        var VehicleTravelDate = $("#txtMonthYear").val();
        var TravelDate = $("#txtMonthYear").val();
        if (VehicleTravelDate != '') {
            VehicleTravelDate = VehicleTravelDate.split(' ');
            TravelDate = monthNames.indexOf(VehicleTravelDate[0]) + 1;
            if (TravelDate <= 9) {
                TravelDate = "0" + TravelDate;
            }
            TravelDate = TravelDate + "-" + VehicleTravelDate[1];
        }
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/VehicleCostDetails_UpdatedJqGrid?VehicleId=' + VehicleId,
                    postData: { EntryType: $("#DdlEntryType").val(), VehicleTravelDate: TravelDate },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnreset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).jqGrid("clearGridData", true);
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/VehicleCostDetails_UpdatedJqGrid?VehicleId=' + VehicleId,
                    postData: { EntryType: $("#ddlEntryType").val(), VehicleTravelDate: $("#txtMonthYear").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#txtMonthYear").datepicker({
        format: "MM yyyy",
        viewMode: "months",
        minViewMode: "months",
        autoclose: true
    });

});
//$(document).keydown(function(e){ 
//    var elid = $(document.activeElement).is('#txtMonthYear');
//    if(e.keyCode === 8 && elid ){ 
//        return true; //allow the backspace character
//    }
//    else {
//        return false; //disallow all other characters
//    }
//});
$("#txtMonthYear").keypress(function (e) {
    e.preventDefault();
});
$("#StartKmrs").on("keyup", function () {
    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
        val = this.value;

    if (!valid) {
        console.log("Invalid input!");
        this.value = val.substring(0, val.length - 1);
    }
});


$("#EndKmrs").on("keyup", function () {
    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
        val = this.value;

    if (!valid) {
        console.log("Invalid input!");
        this.value = val.substring(0, val.length - 1);
    }
});

$("#DriverOt").on("keyup", function () {
    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
        val = this.value;

    if (!valid) {
        console.log("Invalid input!");
        this.value = val.substring(0, val.length - 1);
    }
});

$("#HelperOt").on("keyup", function () {
    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
        val = this.value;

    if (!valid) {
        console.log("Invalid input!");
        this.value = val.substring(0, val.length - 1);
    }
});

$("#Diesel").on("keyup", function () {
    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
        val = this.value;

    if (!valid) {
        console.log("Invalid input!");
        this.value = val.substring(0, val.length - 1);
    }
});

$("#Maintenance").on("keyup", function () {
    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
        val = this.value;

    if (!valid) {
        console.log("Invalid input!");
        this.value = val.substring(0, val.length - 1);
    }
});

$("#Service").on("keyup", function () {
    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
        val = this.value;

    if (!valid) {
        console.log("Invalid input!");
        this.value = val.substring(0, val.length - 1);
    }
});

$("#FC").on("keyup", function () {
    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
        val = this.value;

    if (!valid) {
        console.log("Invalid input!");
        this.value = val.substring(0, val.length - 1);
    }
});
$("#Others").on("keyup", function () {
    var valid = /^\d{0,10}(\.\d{0,10})?$/.test(this.value),
        val = this.value;

    if (!valid) {
        console.log("Invalid input!");
        this.value = val.substring(0, val.length - 1);
    }
});

$("#ddlPurposeName").change(function () {
    if ($("#ddlPurposeName").val() == "Student Pickup and Drop") {
        $("#SubVehicleRouteId").show();
    }
    else {
        $("#SubVehicleRouteId").hide();
    }
});
//$("#ddlEntryType").change(function () {
//    if ($("#ddlEntryType").val() == "Trip") {
//        $("#SubTripPurposeId").show();
//        $("#SubDriverId").show();
//        $("#SubEndKmsId").show();
//    }
//    else {
//        $("#SubTripPurposeId").hide();
//        $('#SubTravelDateId').show();
//        $('#SubDriverId').show();
//        $("#SubVehicleRouteId").hide();
//        $("#SubEndKmsId").hide();
//    }

//});
$("#ddlEntryType").change(function () {
    debugger;
    var EntryType = $("#ddlEntryType").val();
    if ($("#ddlEntryType").val() == "Trip") {
        $("#ddlPurposeName").attr("disabled", false);
        $('#Name').attr("disabled", false);
        $('#HName').attr("disabled", false);
        $('#RouteNo').attr("disabled", false);
        $("#StartKmrs").val($("#hdnstartkmrs").val());
        if (parseInt($("#hdnstartkmrs").val()) > 0) {
            $("#StartKmrs").attr("disabled", true);
        }
        else {
            $("#StartKmrs").attr("disabled", false);
        }
        $('#txtVehicleTravelDate').attr("disabled", false);
        $("#EndKmrs").attr("disabled", false);
        $("#DriverOt").attr("disabled", true);
        $("#HelperOt").attr("disabled", true);
        $("#Diesel").attr("disabled", true);
        $("#Maintenance").attr("disabled", true);
        $("#Service").attr("disabled", true);
        $("#FC").attr("disabled", true);
        $("#Others").attr("disabled", true);
        $("#SubTripPurposeId").show();
        $("#SubDriverId").show();
        $("#SubEndKmsId").show();
        if ($("#showreset").val() == "True") {
            $("#ddlIsKMReseted").attr("disabled", false);
            $("#divisreset").show();
            $("#divresetvalue").show();
        }
        else {
            $("#divisreset").hide();
            $("#divresetvalue").hide();
        }
    }
    else {
        $("#ddlPurposeName").attr("disabled", true);
        $('#txtVehicleTravelDate').attr("disabled", false);
        $('#Name').attr("disabled", false);
        $('#HName').attr("disabled", false);
        $('#RouteNo').attr("disabled", false);
        $('#StartKmrs').attr("disabled", true);
        $("#EndKmrs").attr("disabled", true);
        $("#DriverOt").attr("disabled", false);
        $("#HelperOt").attr("disabled", false);
        $("#Diesel").attr("disabled", false);
        $("#Maintenance").attr("disabled", false);
        $("#Service").attr("disabled", false);
        $("#FC").attr("disabled", false);
        $("#Others").attr("disabled", false);
        $('#txtVehicleTravelDate').attr("disabled", false);
        $('#ddlPurposeName').val("");
        $("#StartKmrs").val("");
        $("#SubTripPurposeId").hide();
        $('#SubTravelDateId').show();
        $('#SubDriverId').show();
        $("#SubVehicleRouteId").hide();
        $("#SubEndKmsId").hide();

    }
});
$("#txtKMResetValue").keydown(function (e) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        // Allow: Ctrl+A, Command+A
        (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
});
function checkvalid(value, column) {
    if (value == 'nil') {
        return [false, column + ": Field is Required"];
    }
    else {
        return [true];
    }
}
function VehicleCostDetails() {
    debugger;

    var EntryType = $("#ddlEntryType").val();
    var VehicleId = $('#hdnVehicleId').val();
    var VehicleNo = $('#VehicleNo').val();
    var Campus = $('#hdnCampus').val();
    var TypeOfTrip = $('#ddlPurposeName').val();
    var VehicleTravelDate = $('#txtVehicleTravelDate').val();
    var DriverName = $('#DriverName').val();
    var HelperName = $('#HelperName').val();
    var VehicleRoute = $('#RouteNo').val();
    var StartKmrs = $('#StartKmrs').val();
    var EndKmrs = $("#EndKmrs").val();
    var DriverOt = $("#DriverOt").val();
    var HelperOt = $("#HelperOt").val();
    var Diesel = $("#Diesel").val();
    var Maintenance = $("#Maintenance").val();
    var Service = $("#Service").val();
    var FC = $("#FC").val();
    var Others = $("#Others").val();
    var IsKMReseted = $("#ddlIsKMReseted").val();
    var KMResetValue = $("#txtKMResetValue").val();
    //alert(DriverOt);
    //alert(HelperOt);
    //if (VehicleTravelDate == '' || DriverName == '' || StartKmrs == '' || EndKmrs == '') {
    //    ErrMsg("Please fill all the mandatory fields.");
    //    return false;
    //}
    if (EntryType == '') {
        ErrMsg("Please Select Entry Type");
        return false;
    }
    else if ($('#ddlPurposeName').val() == "Student Pickup and Drop" && VehicleRoute == '') {
        ErrMsg("Please fill Vehicle Route.");
        return false;
    }
    else if (EntryType == "Trip" && (TypeOfTrip == '' || VehicleTravelDate == '' || DriverName == '' || StartKmrs == '' || EndKmrs == '')) {
        ErrMsg("Please fill all the mandatory fields for Trip");
        return false;
    }
    else if (EntryType == "Trip" && $("#ddlIsKMReseted").val() == "true" && $("#txtKMResetValue").val() == "") {
        ErrMsg("Please fill KM Reset Value");
        return false;
    }
    else if (EntryType == "Trip" && $("#showreset").val() == "True" && $("#ddlIsKMReseted").val() == "") {
        ErrMsg("Please select Is Kms Reseted");
        return false;
    }
    else if (EntryType == "Trip" && $("#ddlIsKMReseted").val() == "true" && $("#txtKMResetValue").val() != "" && (parseInt(StartKmrs) > parseInt(KMResetValue))) {
        ErrMsg("Kms Reseted Value must be greater than Start Kms.");
        return false;
    }
    else if (EntryType == "Expenses" && (DriverName == '' || VehicleTravelDate == '')) {
        ErrMsg("Please fill all the mandatory fields for Expenses");
        return false;
    }
    else if (EntryType == "Expenses" && DriverName != '' && VehicleTravelDate != '' && (DriverOt == '' && Others == '' && HelperOt == '' && FC == '' && Maintenance == '' && Service == '' && Diesel == '')) {
        ErrMsg("Please Enter Any one expense");
        return false;
    }
    else {
        $.ajax({
            type: 'POST',
            url: "/Transport/SaveOrUpdateVehicleCostDetails_Updated",
            async: false,
            dataType: 'json',
            data: {
                EntryType: EntryType, VehicleId: VehicleId, VehicleNo: VehicleNo, Campus: Campus, TypeOfTrip: TypeOfTrip, VehicleTravelDate: VehicleTravelDate, DriverMasterId: DriverName, HelperId: HelperName, VehicleRoute: VehicleRoute, StartKmrs: StartKmrs, EndKmrs: EndKmrs, DriverOt: DriverOt, HelperOt: HelperOt, Diesel: Diesel, Maintenance: Maintenance, Service: Service, FC: FC, Others: Others, IsKMReseted: IsKMReseted, KMResetValue: KMResetValue
            },
            success: function (data) {
                if (data.statusval == "failed") {
                    var yes = confirm("There Is No Entry For Previous Day.Do You Want to Proceed?");
                    if (yes) {
                        $.ajax({
                            type: 'POST',
                            url: "/Transport/SaveVehicleCostDetails_Updated",
                            async: false,
                            dataType: 'json',
                            data: {
                                EntryType: EntryType, VehicleId: VehicleId, VehicleNo: VehicleNo, Campus: Campus, TypeOfTrip: TypeOfTrip, VehicleTravelDate: VehicleTravelDate, DriverMasterId: DriverName, HelperId: HelperName, VehicleRoute: VehicleRoute, StartKmrs: StartKmrs, EndKmrs: EndKmrs, DriverOt: DriverOt, HelperOt: HelperOt, Diesel: Diesel, Maintenance: Maintenance, Service: Service, FC: FC, Others: Others, IsKMReseted: IsKMReseted, KMResetValue: KMResetValue
                            },
                            success: function (data) {
                                if (data.statusval == "success") {
                                    $("#StartKmrs").attr("disabled", true);
                                    location.reload();
                                    $("#VehicleCostDetailsJqGrid").trigger("reloadGrid");
                                    $("#StartKmrs").val(data.EndKmrs);
                                    $('#txtVehicleTravelDate').val("");
                                    $('#ddlPurposeName').val("");
                                    $('#ddlEntryType').val("");
                                    $('#Name').val("");
                                    $('#HName').val("");
                                    $('#RouteNo').val("");
                                    $("#EndKmrs").val("");
                                    $("#DriverOt").val("");
                                    $("#HelperOt").val("");
                                    $("#Diesel").val("");
                                    $("#Maintenance").val("");
                                    $("#Service").val("");
                                    $("#FC").val("");
                                    $("#Others").val("");
                                    $("#DriverName").val("");
                                    $("#HelperName").val("");
                                    SucessMsg("Added Sucessfully");
                                }
                                if (data.statusval == "exist") {
                                    // $("#StartKmrs").val(data.EndKmrs);
                                    location.reload();
                                    $("#VehicleCostDetailsJqGrid").trigger("reloadGrid");
                                    $('#txtVehicleTravelDate').val("");
                                    $('#ddlPurposeName').val("");
                                    $('#ddlEntryType').val("");
                                    $('#Name').val("");
                                    $('#HName').val("");
                                    $('#RouteNo').val("");
                                    $("#EndKmrs").val("");
                                    $("#DriverOt").val("");
                                    $("#HelperOt").val("");
                                    $("#Diesel").val("");
                                    $("#Maintenance").val("");
                                    $("#Service").val("");
                                    $("#FC").val("");
                                    $("#Others").val("");
                                    $("#DriverName").val("");
                                    $("#HelperName").val("");
                                    ErrMsg("Already Exist!!");
                                    return false;
                                }

                            }
                        });
                        return true;
                    }
                    if (data.statusval == "exist") {
                        location.reload();
                        $("#VehicleCostDetailsJqGrid").trigger("reloadGrid");
                        $('#txtVehicleTravelDate').val("");
                        $('#ddlPurposeName').val("");
                        $('#ddlEntryType').val("");
                        $('#Name').val("");
                        $('#HName').val("");
                        $('#RouteNo').val("");
                        $("#EndKmrs").val("");
                        $("#DriverOt").val("");
                        $("#HelperOt").val("");
                        $("#Diesel").val("");
                        $("#Maintenance").val("");
                        $("#Service").val("");
                        $("#FC").val("");
                        $("#Others").val("");
                        $("#DriverName").val("");
                        $("#HelperName").val("");
                        ErrMsg("Already Exist!!");
                        return false;
                    }
                    else {
                        confirm("The transcation has been canceled:");
                        //$("#StartKmrs").attr("disabled", true);
                        location.reload();
                        $("#VehicleCostDetailsJqGrid").trigger("reloadGrid");
                        //$("#StartKmrs").val(data.EndKmrs);
                        $('#txtVehicleTravelDate').val("");
                        $('#ddlPurposeName').val("");
                        $('#ddlEntryType').val("");
                        $('#Name').val("");
                        $('#HName').val("");
                        $('#RouteNo').val("");
                        $("#EndKmrs").val("");
                        $("#DriverOt").val("");
                        $("#HelperOt").val("");
                        $("#Diesel").val("");
                        $("#Maintenance").val("");
                        $("#Service").val("");
                        $("#FC").val("");
                        $("#Others").val("");
                        $("#DriverName").val("");
                        $("#HelperName").val("");
                        return false;
                    }
                }
                if (data.statusval == "success") {
                    $("#StartKmrs").attr("disabled", true);
                    location.reload();
                    $("#VehicleCostDetailsJqGrid").trigger("reloadGrid");
                    $("#StartKmrs").val(data.EndKmrs);
                    $('#txtVehicleTravelDate').val("");
                    $('#ddlPurposeName').val("");
                    $('#ddlEntryType').val("");
                    $('#Name').val("");
                    $('#HName').val("");
                    $('#RouteNo').val("");
                    $("#EndKmrs").val("");
                    $("#DriverOt").val("");
                    $("#HelperOt").val("");
                    $("#Diesel").val("");
                    $("#Maintenance").val("");
                    $("#Service").val("");
                    $("#FC").val("");
                    $("#Others").val("");
                    $("#DriverName").val("");
                    $("#HelperName").val("");
                    SucessMsg("Added Sucessfully");
                    return true;
                }
                //else {
                //    $("#StartKmrs").attr("disabled", true);
                //    $("#VehicleCostDetailsJqGrid").trigger("reloadGrid");
                //   // $("#StartKmrs").val(data.EndKmrs);
                //    $('#txtVehicleTravelDate').val("");
                //    $('#TripName').val("");
                //    $('#Name').val("");
                //    $('#HName').val("");
                //    $('#RouteNo').val("");
                //    $("#EndKmrs").val("");
                //    $("#DriverOt").val("");
                //    $("#HelperOt").val("");
                //    $("#Diesel").val("");
                //    $("#Maintenance").val("");
                //    $("#Service").val("");
                //    $("#FC").val("");
                //    $("#Others").val("");
                //    ErrMsg("Already Exist!!");
                //    return false;
                //}
                if (data.statusval == "DateFaild") {
                    ErrMsg("The entry date must be greater than old date or same date !!!!");
                    $('#txtVehicleTravelDate').val("");
                    $("#EndKmrs").val("");
                    $('#Name').val("");
                    return false;
                }
            }
        });
    }
}

var cache = {};
$("#Name").autocomplete({
    source: function (request, response) {
        var term = request.term;
        if (term in cache) {
            response($.map(cache[term], function (item) {
                return { label: item.Name, value: item.Id }
            }))
            return;
        }
        $.getJSON('/Transport/GetDriverName', request, function (data, status, xhr) {
            cache[term] = data;
            response($.map(data, function (item) {
                return { label: item.Name, value: item.Id }

            }))
        });
    },
    minLength: 1,
    delay: 100,
    select: function (event, ui) {
        event.preventDefault();
        $("#DriverName").val(ui.item.value);
        $("#Name").val(ui.item.label);
    },
    open: function () {
        $(this).autocomplete('widget').zIndex(10);
    },
    focus: function (event, ui) {

        event.preventDefault();
        $("#Name").val(ui.item.label);

    },
    messages: {
        noResults: "", results: ""
    },
    change: function (event, ui) {
        debugger;
        if (ui.item == null) {
            this.value = '';
            ErrMsg('Please select a value from the driver name list'); // or a custom message
        }
    }
});

$('#RouteNo').autocomplete({
    source: function (request, response) {
        var trem = request.trem;
        $.ajax({


            url: '/Transport/GetVehicleRoute',
            type: 'POST',
            dataType: "json",
            data: { RouteNo: request.term },
            success: function (data) {
                response(data);

            }
        });

    },
    open: function () {
        $(this).autocomplete('widget').zIndex(10);
    },
    change: function (event, ui) {
        debugger;
        if (ui.item == null) {
            this.value = '';
            ErrMsg('Please select a value from the route list'); // or a custom message
        }
    }
});

var cache = {};
$("#HName").autocomplete({
    source: function (request, response) {
        debugger;
        var term = request.term;
        if (term in cache) {
            response($.map(cache[term], function (item) {
                return { label: item.Name, value: item.Id }
            }))
            return;
        }
        $.getJSON('/StaffManagement/GetStaffNameAutoComplete', request, function (data, status, xhr) {
            cache[term] = data;
            response($.map(data, function (item) {
                return { label: item.Name, value: item.Id }

            }))
        });
    },
    minLength: 1,
    delay: 100,
    select: function (event, ui) {
        event.preventDefault();
        $("#HelperName").val(ui.item.value);
        $("#HName").val(ui.item.label);
    },
    open: function () {
        $(this).autocomplete('widget').zIndex(10);
    },
    focus: function (event, ui) {

        event.preventDefault();
        $("#HName").val(ui.item.label);

    },
    messages: {
        noResults: "", results: ""
    },
    change: function (event, ui) {
        debugger;
        if (ui.item == null) {
            this.value = '';
            ErrMsg('Please select a value from the helper name list'); // or a custom message
        }
    }
});
function formateadorLink(cellvalue, options, rowObject) {
    var cv = cellvalue.split(" ");
    if (parseFloat(cv[1]) > 0) {
        return "<a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick='FuelPopUp(" + rowObject[0] + ")'>" + cellvalue + "</a>";
        //"<a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick=\"FuelPopUp('" + items.VehicleCostId + "','" + items.Diesel +  "');\">"+items.Diesel+"</a>":items.Diesel.ToString(),
    }
    else {
        return cellvalue;
    }

}
function formateadorLink1(cellvalue, options, rowObject) {
    var cv = cellvalue.split(" ");
    if (parseFloat(cv[1]) > 0) {
        return "<a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick='MaintenancePopUp(" + rowObject[0] + ")'>" + cellvalue + "</a>";
        //"<a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick=\"FuelPopUp('" + items.VehicleCostId + "','" + items.Diesel +  "');\">"+items.Diesel+"</a>":items.Diesel.ToString(),
    }
    else {
        return cellvalue;
    }

}
function formateadorLink2(cellvalue, options, rowObject) {

    var cv = cellvalue.split(" ");
    if (parseFloat(cv[1]) > 0) {
        return "<a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick='ServicePopUp(" + rowObject[0] + ")'>" + cellvalue + "</a>";
        //"<a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick=\"FuelPopUp('" + items.VehicleCostId + "','" + items.Diesel +  "');\">"+items.Diesel+"</a>":items.Diesel.ToString(),
    }
    else {
        return cellvalue;
    }

}
function formateadorLink3(cellvalue, options, rowObject) {

    var cv = cellvalue.split(" ");
    if (parseFloat(cv[1]) > 0) {
        return "<a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick='FCPopUp(" + rowObject[0] + ")'>" + cellvalue + "</a>";
        //"<a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick=\"FuelPopUp('" + items.VehicleCostId + "','" + items.Diesel +  "');\">"+items.Diesel+"</a>":items.Diesel.ToString(),
    }
    else {
        return cellvalue;
    }

}
function formateadorLink4(cellvalue, options, rowObject) {

    var cv = cellvalue.split(" ");
    if (parseFloat(cv[1]) > 0) {
        return "<a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick='OthersPopUp(" + rowObject[0] + ")'>" + cellvalue + "</a>";
        //"<a style='color:#034af3;text-decoration:underline;cursor:pointer;' onclick=\"FuelPopUp('" + items.VehicleCostId + "','" + items.Diesel +  "');\">"+items.Diesel+"</a>":items.Diesel.ToString(),
    }
    else {
        return cellvalue;
    }

}
function FuelPopUp(VehicleCostId) {
    ModifiedLoadPopupDynamicaly("/Transport/VehicleFuelEntryForm?VehicleCostId=" + VehicleCostId, $('#divVehicleFuelEntryDetails'),
            function () { }, function () { }, 860, 300, "Vehicle Fuel Entry Form");
}
function MaintenancePopUp(VehicleCostId) {
    ModifiedLoadPopupDynamicaly("/Transport/VehicleMaintenanceEntryForm?VehicleCostId=" + VehicleCostId, $('#divVehicleMaintenanceEntryDetails'),
            function () { }, function () { }, 860, 320, "Vehicle Maintenance Entry Form");
}
function ServicePopUp(VehicleCostId, Service) {
    ModifiedLoadPopupDynamicaly("/Transport/VehicleServiceEntryForm?VehicleCostId=" + VehicleCostId + '&Service=' + Service, $('#divVehicleServiceEntryDetails'),
            function () { }, function () { }, 860, 300, "Vehicle Service Entry Form");
}
function FCPopUp(VehicleCostId, FC) {
    ModifiedLoadPopupDynamicaly("/Transport/VehicleFCEntryForm?VehicleCostId=" + VehicleCostId + '&FC=' + FC, $('#divVehicleFCEntryDetails'),
            function () { }, function () { }, 860, 300, "Vehicle FC Entry Form");
}
function OthersPopUp(VehicleCostId, Others) {
    ModifiedLoadPopupDynamicaly("/Transport/VehicleOthersEntryForm?VehicleCostId=" + VehicleCostId + '&Others=' + Others, $('#divVehicleOthersEntryDetails'),
            function () { }, function () { }, 860, 300, "Vehicle Others Entry Form");
}
