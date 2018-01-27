function FillCampusDll() {
    $.getJSON("/Base/FillAllBranchCode",
      function (fillbc) {
          var ddlbc = $("#SrchCampus");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
jQuery(function ($) {
    FillCampusDll();
    var grid_selector = "#DistanceCoveredList";
    var pager_selector = "#DistanceCoveredListPager";

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
    $('.datetimepicker').datetimepicker({
        //format: 'dd/mm/yyyy hh:ii',
        //endDate: new Date(),
        //todayHighLight:true
        format: 'DD/MM/YYYY HH:mm',
        maxDate: new Date(),
        todayHighLight: true,
        useCurrent: false
    });
    $("#SrchVehicleNo").autocomplete({
        source: function (request, response) {
            $.getJSON('/Transport/GetVehicleNo?term=' + request.term,
                         function (data) {
                             response(data);
                         });
        },
        minLength: 1,
        delay: 100,
        select: function (event, ui) {
            $.getJSON('/Transport/GetVehicleDetailsByVehicleNo?VehicleNo=' + ui.item.value,
                                 function (data) {
                                     if (data[0].Campus != '') {
                                         $("#SrchCampus").val(data[0].Campus);
                                         if (data[0].Type != '') {
                                             $("#txtSrchVehicleType").val(data[0].Type);
                                         }
                                     }
                                     else {
                                         $("#SrchCampus").val('');
                                         $("#txtSrchVehicleType").val('');
                                     }
                                     $("#SrchVehicleId").val(data[0].Id);
                                 }
                             )
        }
    });
    $("#VehicleNo").autocomplete({
        source: function (request, response) {
            $.getJSON('/Transport/GetVehicleNoByStatus?term=' + request.term,
                         function (data) {
                             response(data);
                         });
        },
        minLength: 1,
        delay: 100,
        select: function (event, ui) {
            $.getJSON('/Transport/GetVehicleReadingByVehcileNo?VehicleNo=' + ui.item.value,
                                 function (data) {
                                     debugger;
                                     if (data.Campus != '') {
                                         $("#Campus").val(data.Campus);
                                         $("#KMOut").val(data.KMIn);
                                         if (data.Type != '') {
                                             $("#txtVehicleType").val(data.Type);
                                         }
                                     }
                                     else {
                                         $("#Campus").val('');
                                         $("#txtVehicleType").val('');
                                     }
                                     $("#VehicleId").val(data.VehicleId);
                                 }
                             )
        }
    });
    $("#SrchDriverName").autocomplete({
        source: function (request, response) {
            var Campus = $("#SrchCampus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });
    $("#DriverName").autocomplete({
        source: function (request, response) {
            var Campus = $("#Campus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });

    $("#Purpose").autocomplete({
        source: function (request, response) {
            $.getJSON('/Transport/GetTripPurposeMaster?term=' + request.term,
                 function (data) {
                     response(data);
                 });
        },
        minLength: 1,
        delay: 100
    });
    $("#SrchPurpose").autocomplete({
        source: function (request, response) {
            $.getJSON('/Transport/GetTripPurposeMaster?term=' + request.term,
                 function (data) {
                     response(data);
                 });
        },
        minLength: 1,
        delay: 100
    });
    $("#SrchSource, #SrchDestination").autocomplete({
        source: function (request, response) {
            debugger;
            var Campus = $("#SrchCampus").val();
            $.getJSON('/Transport/GetAutoCompleteLocationByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });
    $("#Source, #Destination").autocomplete({
        source: function (request, response) {
            debugger;
            var Campus = $("#Campus").val();
            $.getJSON('/Transport/GetAutoCompleteLocationByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });
    $("#ddlSrchStatus").val('Open');
    //$("#ddlSrchStatus").val($("#VehicleStatus").val());
    //}
    var status = $("#ddlSrchStatus").val();
    jQuery(grid_selector).jqGrid({
        url: '/Transport/DistanceCoveredListJqGrid/?Status='+status,
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'Vehicle Id', 'Vehicle No', 'Vehicle Type', 'Campus', 'Driver Name', 'OutDate Time', 'KM Out', 'Purpose Type', 'Purpose', 'Source', 'Destination', 'InDate Time', 'KM In', 'Distance Covered', 'Service Center Name', 'Is KM Reseted', 'KM Reset Value', 'Created Date', 'Created By', 'Status', 'SLA Status'],
        //'Is Any Other Service',
        colModel: [
        //if any column added need to check formateadorLink
             { name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'VehicleId', width: 30, index: 'VehicleId', hidden: true },
             { name: 'VehicleNo', index: 'VehicleNo', width: 250 },
             { name: 'VehicleType', index: 'VehicleType' },
             { name: 'Campus', index: 'Campus' },
             { name: 'DriverName', index: 'DriverName' },
             { name: 'OutDateTime', index: 'OutDateTime', search: false },
             { name: 'KMOut', index: 'KMOut', editable: true, editoptions: { dataInit: function (e) { $(e).attr('readonly', 'readonly') } }, search: false },
             { name: 'PurposeType', index: 'PurposeType' },
             { name: 'Purpose', index: 'Purpose' },
             { name: 'Source', index: 'Source' },
             { name: 'Destination', index: 'Destination' },
             {
                 name: 'InDateTime', index: 'InDateTime', search: false, editable: true, editoptions: {
                     dataInit: function (e) {
                         $(e).datetimepicker({
                             format: 'DD/MM/YYYY HH:mm',
                             maxDate: new Date(),
                             todayHighLight: true,
                             useCurrent: false
                             //format: 'dd/mm/yyyy hh:ii',
                             //endDate: new Date(),
                             //todayHighLight: true

                         });
                         $(e).attr('readonly', 'readonly');
                     }
                 }, editrules: { required: true }
             },
             {
                 name: 'KMIn', index: 'KMIn', width: 120, editable: true, editable: true, editoptions: {
                     dataInit: function (e) {
                         $(e).keyup(function () {
                             var IsReset = $("#IsKMReseted").val();
                             if (IsReset == "False" || IsReset == "") {
                                 if ($("#KMIn").val() != '' && $("#KMOut").val() != '' && !isNaN($("#KMIn").val()))
                                     $("#DistanceCovered").val(parseFloat($("#KMIn").val()) - parseFloat($("#KMOut").val()));
                                 else
                                     $("#DistanceCovered").val('');
                             }
                             if (IsReset == "True") {
                                 if ($("#KMIn").val() != '' && $("#KMOut").val() != '' && !isNaN($("#KMIn").val()) && $("#KMResetValue").val() != '' && !isNaN($("#KMResetValue").val())) {
                                     $("#DistanceCovered").val(parseFloat($("#KMResetValue").val()) - parseFloat($("#KMOut").val()) + parseFloat($("#KMIn").val()));
                                 }
                                 else
                                     $("#DistanceCovered").val('');
                             }

                         });
                     }
                 }, search: false, editrules: { required: true, number: true }
             },
             { name: 'DistanceCovered', index: 'DistanceCovered', editable: true, editoptions: { dataInit: function (e) { $(e).attr('readonly', 'readonly') } }, search: false, editrules: { required: true } },
             //{ name: 'IsAnyService', index: 'IsAnyService' },
             { name: 'ServiceCenterName', index: 'ServiceCenterName' },
             {
                 name: 'IsKMReseted', index: 'IsKMReseted', editable: true, edittype: 'select', editoptions: {
                     sopt: ['eq'], value: { '': '----Select One----', 'True': 'Yes', 'False': 'No' }, dataInit: function (e) {
                         $(e).change(function () {
                             $("#KMIn").val('');
                             $("#DistanceCovered").val('');
                             $("#KMResetValue").val('');
                         })
                     }
                 }, search: false, editrules: { required: true }
             },
             {
                 name: 'KMResetValue', index: 'KMResetValue', editable: true, search: false, editoptions: {
                     dataInit: function (e) {
                         $(e).keyup(function () {
                             if ($("#KMIn").val() != '' && $("#KMOut").val() != '' && $("#KMResetValue").val() != '' && !isNaN($("#KMResetValue").val())) {
                                 var IsReset = $("#IsKMReseted").val();
                                 if (IsReset == "False" || IsReset == "") {
                                     if ($("#KMIn").val() != '' && $("#KMOut").val() != '' && !isNaN($("#KMIn").val()))
                                         $("#DistanceCovered").val(parseFloat($("#KMIn").val()) - parseFloat($("#KMOut").val()));
                                     else
                                         $("#DistanceCovered").val('');
                                 }
                                 if (IsReset == "True") {
                                     if ($("#KMIn").val() != '' && $("#KMOut").val() != '' && !isNaN($("#KMIn").val()) && $("#KMResetValue").val() != '' && !isNaN($("#KMResetValue").val())) {
                                         $("#DistanceCovered").val(parseFloat($("#KMResetValue").val()) - parseFloat($("#KMOut").val()) + parseFloat($("#KMIn").val()));
                                     }
                                     else
                                         $("#DistanceCovered").val('');
                                 }
                             }
                         });
                     }
                 }, search: false, editruless: { number: true, required: true }
             },
             { name: 'CreatedDate', index: 'CreatedDate', width: 170, search: false },
             { name: 'CreatedBy', index: 'CreatedBy', width: 170, search: false },
             { name: 'Status', index: 'Status' },
             { name: 'SLAStatus', index: 'SLAStatus', formatter: statusimage, search: false, width: 50 }
             //{ name: 'Update', index: 'Update', width: 30, align: 'center', sortable: false },
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 25, 50, 100, 500],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Asc',
        multiselect: true,
        gridComplete: function () {
            var rdata = $(grid_selector).getRowData();
            if (rdata.length > 0) {
                $('.T1CompUpdate').click(function () { UpdateComponentDtls($(this).attr('rowid')); });
                //  $('.T1CompDel').click(function () { DeleteComponentDtls($(this).attr('rowid')); });
            }
        },
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        onSelectRow: function (id) {
            var myGrid = $(grid_selector),
            selRowId = myGrid.jqGrid('getGridParam', 'selrow'),
            celValue = myGrid.jqGrid('getCell', selRowId, 'Status');
            if (celValue == "Completed") {
                debugger;
                $('#jqg_DistanceCoveredList_' + id).attr('checked', false)
                myGrid.jqGrid('resetSelection');
                ErrMsg("You can't Update if the Status is Completed")
                return false;
            }
            if (celValue == "Open") {
                return true;
            }
        },
        caption: "<i class='ace-icon fa fa-truck'></i>&nbsp;Distance Covered List"
    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
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
            },
            {
                url: '/Transport/EditVehicleDistanceCovered',
                closeAfterEdit: true, closeOnEscape: true,
                beforeShowForm: function ($form) {
                    if ($form.find("#tr_KMResetValue").val() == "True") {
                        $form.find("#tr_KMResetValue").show();
                    }
                    else {
                        $form.find("#tr_KMResetValue").hide();
                    }
                },
                afterShowForm: function ($form) {
                    $form.find("#IsKMReseted").change(function () {
                        if ($form.find("#IsKMReseted").val() == "True") {
                            $form.find("#tr_KMResetValue").show();
                        }
                        else { $form.find("#tr_KMResetValue").hide(); }
                    });
                    $form.find("#tr_KMResetValue").hide();
                },
                beforeSubmit: function (postdata, formid) {
                    debugger;
                    if (postdata.KMIn != '') {
                        //var IsKMReseted = $("#ddlIsReset").val();
                        // var KMResetValue = $("#txtKMResetValue").val();
                        if (postdata.IsKMReseted == "False" || postdata.IsKMReseted == "") {
                            if (parseFloat(postdata.KMIn) <= parseFloat(postdata.KMOut)) {
                                ErrMsg("KMIn should be greater than KMOut");
                                $('#KMIn').val('');
                                $('#DistanceCovered').val('');
                                return [false];
                            }
                        }
                        if (postdata.IsKMReseted == "True") {
                            if (postdata.KMResetValue == "") {
                                ErrMsg("Please Fill KM Reset Value");
                                return [false];
                            }
                            if (postdata.KMResetValue != "") {
                                if (isNaN(postdata.KMResetValue)) {
                                    ErrMsg("Numbers only allowed.");
                                    return [false];
                                }
                            }
                            if (parseFloat(postdata.KMOut) > parseFloat(postdata.KMResetValue)) {
                                ErrMsg("KM Reset Value Must be Greater than KMOut");
                                $("#KMResetValue").val('');
                                return [false];
                            }
                            if (parseFloat(postdata.KMOut) < parseFloat(postdata.KMResetValue)) {
                                if (parseFloat(postdata.KMIn) > parseFloat(postdata.KMOut)) {
                                    ErrMsg(" KMOut should be greater than KMIn when Reset");
                                    $('#KMIn').val('');
                                    $('#DistanceCovered').val('');
                                    return [false];
                                }
                            }
                        }
                    }
                    return [true];
                }
            }, {}, { url: '/Transport/DeleteDistanceCoveredId' }, {})

    $(grid_selector).jqGrid('navButtonAdd', '#DistanceCoveredListPager', {
        caption: "Export Excel", buttonicon: "ui-icon-print",
        onClickButton: function () {
            var ExportType = "Excel";
            window.open("/Transport/DistanceCoveredListJqGrid" + '?ExportType=' + ExportType +

            '&VehicleNo=' + $("#SrchVehicleNo").val() +
             '&Campus=' + $("#SrchCampus").val() +
            '&VehicleType=' + $("#txtSrchVehicleType").val() +
            '&DriverName=' + $("#SrchDriverName").val() +
             '&PurposeType=' + $("#ddlSrchPurposeType").val() +
            '&Purpose=' + $("#SrchPurpose").val() +
             '&Source =' + $("#SrchSource").val() +
             '&Destination =' + $("#SrchDestination").val() +
             '&ServiceCenterName =' + $("#txtSrchServiceCenterName").val() +
             '&Status =' + $("#ddlSrchStatus").val() +
             '&rows=9999');
            //'&VehicleNo=' + $("#gs_VehicleNo").val() +
            //'&Campus=' + $("#gs_Campus").val() +
            //'&DriverName=' + $("#gs_DriverName").val() +
            //'&KMOut=' + $("#gs_KMOut").val() +
            //'&Purpose=' + $("#gs_Purpose").val() +
            //'&Source=' + $("#gs_Source").val() +
            //'&Destination=' + $("#gs_Destination").val() +
            //'&KMIn=' + $("#gs_KMIn").val() +
            //'&DistanceCovered=' + $("#gs_DistanceCovered").val() +
            //'&CreatedBy=' + $("#gs_CreatedBy").val() +
            //'&Status=' + $("#gs_Status").val() +


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
    $("#gs_VehicleNo").autocomplete({
        source: function (request, response) {
            $.getJSON('/Transport/GetVehicleNo?term=' + request.term, function (data) {
                // alert(data);
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });
    //$("#KMIn").keyup(function () {
    //    CalculateDistance();
    //});
    //$("#txtKMResetValue").keyup(function () {
    //    if ($("#KMIn").val() != '' && $("#KMOut").val() != '' && $("#txtKMResetValue").val() != '' && !isNaN($("#txtKMResetValue").val())) {
    //        CalculateDistance();
    //    }
    //});
    //$("#KMIn, #KMOut").keyup(function () {
    //    NumbersOnly($(this).val());
    //});
    //$("#txtKMResetValue").keyup(function () {
    //    NumbersOnly($(this).val());
    //});
    function NumbersOnly(value) {
        if (isNaN(value)) {
            ErrMsg("Numbers only allowed.");
            return false;
        }
    }

    function CalculateDistance() {
        debugger;
        var IsReset = $("#ddlIsReset").val();
        if (IsReset == "False" || IsReset == "") {
            if ($("#KMIn").val() != '' && $("#KMOut").val() != '' && !isNaN($("#KMIn").val()))
                $("#DistanceCovered").val(parseFloat($("#KMIn").val()) - parseFloat($("#KMOut").val()));
            else
                $("#DistanceCovered").val('');
        }
        if (IsReset == "True") {
            if ($("#KMIn").val() != '' && $("#KMOut").val() != '' && !isNaN($("#KMIn").val()) && $("#txtKMResetValue").val() != '' && !isNaN($("#txtKMResetValue").val())) {
                $("#DistanceCovered").val(parseFloat($("#txtKMResetValue").val()) - parseFloat($("#KMOut").val()) + parseFloat($("#KMIn").val()));
            }
            else
                $("#DistanceCovered").val('');
        }
    }
    function frmtrUpdate(cellvalue, options, rowdata) {
        var saveBtn = "";
        if (rowdata[15] == "Open") {
            saveBtn = "<span class='ui-icon ui-icon-pencil T1CompUpdate' id='T1btnUpdate_" + options.rowId + "' rowid='" + options.rowId + "' title='Update'></span>";
            return saveBtn;
        }
    }
    function UpdateComponentDtls(id) {
        var rowData = $(grid_selector).getRowData(id);
        debugger;

        $('#Id').val(rowData.Id);
        $('#VehicleId').val(rowData.VehicleId);
        $('#VehicleNo').val(rowData.VehicleNo);
        $('#Campus').val(rowData.Campus);
        $('#DriverName').val(rowData.DriverName);
        $('#OutDateTime').val(rowData.OutDateTime);
        $('#KMOut').val(rowData.KMOut);
        $('#Purpose').val(rowData.Purpose);
        $('#Source').val(rowData.Source);
        $('#Destination').val(rowData.Destination);
        $('#CreatedDate').val(rowData.CreatedDate);
        $('#CreatedBy').val(rowData.CreatedBy);
    }
    $("#ddlIsAnyService").val('False');
    $("#ServiceCenterName").hide();
    $("#KMResetValue").hide();
    $("#ddlIsReset").val('False');
    $("#ddlSrchIsAnyService").val('False');
    $("#ddlSrchIsReset").val('False');
    $("#ddlPurposeType").change(function () {
        var PurposeType = $("#ddlPurposeType").val();
        if (PurposeType == "FC" || PurposeType == "Maintenance" || PurposeType == "Accidental" || PurposeType == "OtherServices") {
            $("#ServiceCenterName").show();
        }
        else {
            $("#ServiceCenterName").hide();
        }
        if (PurposeType == "Others") {
            $("#txtPurpose").show();
        }
        else {
            $("#txtPurpose").hide();
        }
    });
    //$("#ddlIsAnyService").change(function () {
    //    var IsAnyService = $("#ddlIsAnyService").val();
    //    if (IsAnyService == "True") {
    //        $("#ServiceCenterName").show();                       
    //    }
    //    if (IsAnyService == "False" || IsAnyService == "" || IsAnyService == "null" || IsAnyService == "undefined") {
    //        $("#ServiceCenterName").hide();
    //    }
    //});
    //$("#ddlIsAnyService").change(function(){
    //    $(this).find("option:selected").each(function(){            
    //        if ($(this).attr("value") == "True") {
    //            $("#ddlPurposeType option[value=FC]").show();
    //            $("#ddlPurposeType option[value=Maintenance]").show();
    //            $("#ddlPurposeType option[value=Accidental]").show();
    //            $("#ddlPurposeType option[value=Others]").show();
    //            $("#ddlPurposeType option[value=SchoolTrip]").hide();
    //            $("#ddlPurposeType option[value=RemedialTrip]").hide();
    //            $("#ServiceCenterName").show();                
    //        }
    //        else if ($(this).attr("value") == "False") {
    //            $("#ddlPurposeType option[value=FC]").hide();
    //            $("#ddlPurposeType option[value=Maintenance]").hide();
    //            $("#ddlPurposeType option[value=Accidental]").hide();                
    //            $("#ddlPurposeType option[value=SchoolTrip]").show();
    //            $("#ddlPurposeType option[value=RemedialTrip]").show();
    //            $("#ddlPurposeType option[value=Others]").show();
    //            $("#ServiceCenterName").hide();
    //        }
    //        else if ($(this).attr("value") == "") {
    //            $("#ddlPurposeType option[value=FC]").hide();
    //            $("#ddlPurposeType option[value=Maintenance]").hide();
    //            $("#ddlPurposeType option[value=Accidental]").hide();
    //            $("#ddlPurposeType option[value=SchoolTrip]").hide();
    //            $("#ddlPurposeType option[value=RemedialTrip]").hide();
    //            $("#ddlPurposeType option[value=Others]").hide();
    //            $("#ServiceCenterName").hide();
    //        }
    //    });
    //}).change();
    //$("#ddlSrchIsAnyService").change(function (){        
    //    $(this).find("option:selected").each(function () {            
    //        if ($(this).attr("value") == "True") {               
    //            $("#ddlSrchPurposeType option[value=FC]").show();
    //            $("#ddlSrchPurposeType option[value=Maintenance]").show();
    //            $("#ddlSrchPurposeType option[value=Accidental]").show();
    //            $("#ddlSrchPurposeType option[value=Others]").show();
    //            $("#ddlSrchPurposeType option[value=SchoolTrip]").hide();
    //            $("#ddlSrchPurposeType option[value=RemedialTrip]").hide();                
    //        }
    //        else if ($(this).attr("value") == "False") {                
    //            $("#ddlSrchPurposeType option[value=FC]").hide();
    //            $("#ddlSrchPurposeType option[value=Maintenance]").hide();
    //            $("#ddlSrchPurposeType option[value=Accidental]").hide();
    //            $("#ddlSrchPurposeType option[value=SchoolTrip]").show();
    //            $("#ddlSrchPurposeType option[value=RemedialTrip]").show();
    //            $("#ddlSrchPurposeType option[value=Others]").show();                
    //        }
    //        else if ($(this).attr("value") == "") {                
    //            $("#ddlSrchPurposeType option[value=FC]").hide();
    //            $("#ddlSrchPurposeType option[value=Maintenance]").hide();
    //            $("#ddlSrchPurposeType option[value=Accidental]").hide();
    //            $("#ddlSrchPurposeType option[value=SchoolTrip]").hide();
    //            $("#ddlSrchPurposeType option[value=RemedialTrip]").hide();
    //            $("#ddlSrchPurposeType option[value=Others]").hide();                
    //        }
    //    });
    //}).change();
    //$("#ddlIsReset").change(function () {
    //    var IsReset = $("#ddlIsReset").val();
    //    if (IsReset == "True") {
    //        $("#KMResetValue").show();
    //        $("#KMIn").val('');
    //        $("#KMOut").val('');
    //        $("#DistanceCovered").val('');
    //    }
    //    if (IsReset == "False" || IsReset == "" || IsReset == "null" || IsReset == "undefined") {
    //        $("#KMResetValue").hide();
    //        $("#KMIn").val('');
    //        $("#KMOut").val('');
    //        $("#DistanceCovered").val('');
    //        $("#txtKMResetValue").val('');
    //    }
    //});
    $("#txtPurpose").hide();    
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        //$("#ddlSrchIsAnyService").val('False');        
        //$("#ddlSrchIsReset").val('False');
        $("#ddlSrchStatus").val('Open');
        //$("#ddlIsAnyService").val('False');        
        //$("#ddlIsReset").val('False');
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Transport/DistanceCoveredListJqGrid',
           postData: {
               Campus: "", VehicleType: "", VehicleNo: "", DriverName: "", PurposeType: "",
               Purpose: "", Source: "", Destination: "", ServiceCenterName: "", Status: $("#ddlSrchStatus").val()
               //IsAnyService:  $("#ddlSrchIsAnyService").val(),IsReset: $("#ddlSrchIsReset").val(),KMIn: "", DistanceCovered: "", KMOut: "", 
           },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        var Campus = $("#SrchCampus").val();
        var VehicleNo = $("#SrchVehicleNo").val();
        var VehicleType = $("#txtSrchVehicleType").val();
        var DriverName = $("#SrchDriverName").val();
        //var KMOut = $("#SrchKMOut").val();
        //var IsAnyService = $("#ddlSrchIsAnyService").val();
        var PurposeType = $("#ddlSrchPurposeType").val();
        var Purpose = $("#SrchPurpose").val();
        var Source = $("#SrchSource").val();
        var Destination = $("#SrchDestination").val();
        //var IsReset = $("#ddlSrchIsReset").val();
        //var KMIn = $("#SrchKMIn").val();
        //var DistanceCovered = $("#SrchDistanceCovered").val();
        var ServiceCenterName = $("#txtSrchServiceCenterName").val();
        var Status = $("#ddlSrchStatus").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Transport/DistanceCoveredListJqGrid',
           postData: {
               Campus: Campus, VehicleType: VehicleType, VehicleNo: VehicleNo, DriverName: DriverName, PurposeType: PurposeType,
               Purpose: Purpose, Source: Source, Destination: Destination, ServiceCenterName: ServiceCenterName, Status: Status
               //KMOut:KMOut, IsAnyService: IsAnyService,IsReset: IsReset, KMIn: KMIn, DistanceCovered: DistanceCovered,
           },
           page: 1
       }).trigger("reloadGrid");
    });
    //$("#btnSave").click(function () {
    //});   
});
function VehicleDistanceCoverdCreate() {
    debugger;
    var VehicleId = $("#VehicleId").val();
    var VehicleNo = $('#VehicleNo').val();
    var Campus = $("#Campus").val();
    var DriverName = $('#DriverName').val();
    var OutDateTime = $('#OutDateTime').val();
    var KMOut = $("#KMOut").val();
    var Purpose = $("#Purpose").val();
    var Source = $('#Source').val();
    var Destination = $('#Destination').val();
    var InDateTime = $('#InDateTime').val();
    var KMIn = $('#KMIn').val();
    var IsAnyService = $("#ddlIsAnyService").val();
    var KMReseted = $("#ddlIsReset").val();
    var ServiceCenterName = $("#txtServiceCenterName").val();
    var DistanceCovered = $('#DistanceCovered').val();
    var KMResetVal = $("#txtKMResetValue").val();
    var PurposeType = $("#ddlPurposeType").val();
    var VehicleType = $("#txtVehicleType").val();
    if (VehicleNo == '' || Source == '' || Destination == '' || DriverName == '' || OutDateTime == '' || PurposeType == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    //if (IsAnyService == "True") {
    //    if (ServiceCenterName == "" || ServiceCenterName == null || ServiceCenterName == "undefined") {
    //        return ErrMsg("Please fill all the mandatory fields.");
    //    }
    //}
    //if (KMReseted == "True") {
    //    if (KMResetVal == "" || KMResetVal == null || KMResetVal == "undefined") {
    //        return ErrMsg("Please fill all the mandatory fields.");
    //    }
    //}
    //if (OutDateTime != "" && InDateTime != "") {
    //    if (Date.parse(OutDateTime) >= Date.parse(InDateTime)) {
    //        return ErrMsg("In Time Must be Greater than Out Time");
    //    }
    //}
    //if (isNaN(DistanceCovered)) {
    //    ErrMsg('Numbers only allowed.');
    //    $('#DistanceCovered').focus();
    //    return false;
    //}

    if (KMIn != '') {
        //var IsKMReseted = $("#ddlIsReset").val();
        //var KMResetValue = $("#txtKMResetValue").val();
        if (KMReseted == "False" || KMReseted == "") {
            if (parseFloat(KMIn) <= parseFloat(KMOut)) {
                ErrMsg("KMIn should be greater than KMOut");
                $('#KMIn').val('');
                $('#DistanceCovered').val('');
                return false;
            }
        }
        if (KMReseted == "True") {
            if (parseFloat(KMOut) > parseFloat(KMResetValue)) {
                ErrMsg("KM Reset Value Must be Greater than KMOut");
                $("#txtKMResetValue").val('');
                return false;
            }
            if (parseFloat(KMOut) < parseFloat(KMResetVal)) {
                if (parseFloat(KMIn) > parseFloat(KMOut)) {
                    ErrMsg(" KMOut should be greater than KMIn when Reset");
                    $('#KMIn').val('');
                    $('#DistanceCovered').val('');
                    return false;
                }
            }
        }
    }    
    $.ajax({
        type: 'POST',
        url: "/Transport/AddVehicleDistanceCovered",
        data: { Id: parseInt($("#Id").val()), VehicleId: parseInt($("#VehicleId").val()), VehicleNo: VehicleNo, Campus: Campus, DriverName: DriverName, OutDateTime: OutDateTime, KMOut: KMOut, Purpose: Purpose, Source: Source, Destination: Destination, ServiceCenterName: ServiceCenterName, PurposeType: PurposeType, VehicleType: VehicleType, Purpose: Purpose },
        //IsKMReseted: KMReseted, KMResetValue: KMResetVal,InDateTime: InDateTime, KMIn: KMIn, DistanceCovered: DistanceCovered, IsAnyService: IsAnyService, 
        complete: function (data) {
            debugger;
            if (data.responseJSON == "success") {
                $('input[type=text], textarea, select').val('');
                //$('#ddlIsAnyService').val('False');
                $('#ServiceCenterName').hide();
                //$('#ddlIsReset').val('False');
                //$('#KMResetValue').hide();
                //$('#ddlSrchIsAnyService').val('False');
                //$('#ddlSrchIsReset').val('False');
                //alert($("#VehicleStatus").val());                
                //window.location.href = '/Transport/VehicleDistanceCoveredNew';
                $("#ddlSrchStatus").val('Open');
                jQuery("#DistanceCoveredList").setGridParam(
      {
          datatype: "json",
          url: '/Transport/DistanceCoveredListJqGrid?Status=Open',                  
      }).trigger("reloadGrid");                
            }
        }
    });

}
function statusimage(cellvalue, options, rowObject) {
    var i;
    var cellValueInt = parseFloat(cellvalue);
    var cml = $("#DistanceCoveredList").jqGrid();
    for (i = 0; i < cml.length; i++) {
        if (cellValueInt <= 24) {
            return '<img src="../../Images/yellow.jpg" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
        }
        else if (cellValueInt > 24 && cellValueInt <= 48) {
            return '<img src="../../Images/orange.jpg" height="10px" width="10px"  alt=' + cellvalue + ' title=' + cellvalue + ' />'
        }
        else if (cellValueInt > 48) {
            return '<img src="../../Images/redblink3.gif" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
        }
        else if (cellvalue == 'Completed') {
            return '<img src="../../Images/green.jpg" height="12px" width="12px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
        }
    }
}