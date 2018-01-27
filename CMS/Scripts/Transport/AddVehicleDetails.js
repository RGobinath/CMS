

$(document).ready(function () {

    $("#btnReset").click(function () {
        $("input[type=text], textarea").val("");
        // $('#AdVehicleType').val('');
        $('#ddlCampus').val('');
        $('#FType').val('');
        $('#VecType').val('');
    });

    $("#btnBack").click(function () {
        window.location.href = '/Transport/TransportDetailsManagement'
    });
});



function ChangeStatus() {
    debugger;
    var Status = $('#VecStatus').val();
    var Id = $('#hdnVehicleId').val();

    $.ajax({
        type: 'POST',
        url: "/Transport/AddVehicleTypeDetails",
        data: {
            Status: Status, Id: Id
        },
        success: function (data) {
            if (data != null) {
                InfoMsg("Vehicle Status is successfully changed");
            }
        }
    });

    //VehicleFuelReportChart(VehicleId)
}

function AddVehicleType() {
    debugger;
    var Campus = $('#ddlCampus').val();
    var VehicleNo = $('#VehicleNo').val();
    var FuelType = $('#FType').val();
    var EngineType = $('#EngineType').val();
    var EngineNo = $('#EngineNo').val();
    var firstRegDate = $('#firstRegDate').val();
    var Chassisno = $('#Chassisno').val();
    var CC = $('#CC').val();
    var BHP = $('#BHP').val();
    var WheelBase = $('#WheelBase').val();
    var UnladenWeight = $('#UnlandenWeight').val();
    var Color = $('#Color').val();
    var GVW = $('#GVW').val();
    var Address = $('#Address').val();
    var Model = $('#Modal').val();
    var VehicleType = $('#VecType').val();
    var Status = $('#VecStatus').val();
    if (VehicleType == '' || Campus == '' || FuelType == '' || VehicleNo == '' || Status == '' ) {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
   
    $.ajax({
        type: 'POST',
        url: "/Transport/AddVehicleSubType",
        data: {
        Campus: Campus, VehicleNo: VehicleNo, FuelType: FuelType, EngineType: EngineType, EngineNumber: EngineNo, FirstRegisteredDate: firstRegDate, ChassisNo: Chassisno,
            VehicleTypeId: VehicleType, CC: CC, BHP: BHP, WheelBase: WheelBase, UnladenWeight: UnladenWeight, Color: Color, GVW: GVW, Address: Address, Model: Model,Status:Status
        },
        success: function (data) {
            debugger;
            if (data != null)
                InfoMsg("Vehicle Is Successfully Registered");
            $("#VehicleTypeMasterJqgrid").trigger('reloadGrid');
            $("input[type=text], textarea").val("");
            // $('#AdVehicleType').val('');
            $('#ddlCampus').val('');
            $('#FType').val('');
            $('#VecType').val('');
        }
    });

    //VehicleFuelReportChart(VehicleId)
}