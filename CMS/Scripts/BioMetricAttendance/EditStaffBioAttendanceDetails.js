$(function ($) {
    var NoOfworkingDays = parseInt($('#NoOfworkingDays').val());
    var TotalNoOfDaysWorked = $('#TotalNoOfDaysWorked').val();
    var TotalNoOfLeaves = $('#NoOfDaysLeaveTaken').val();
    var PreRegNum = $('#PreRegNum').val();
    var AttendanceMonth = $('#AttendanceMonth').val();
    var AttendanceYear = $('#AttendanceYear').val();
    var OpeningBalance = $('#OpeningBalance').val();
    var AllotedCL = $('#AllotedCL').val();
    var NoOfHolidays = $('#NoOfHolidays').val();
    var LeaveTaken = $('#LeavesTaken').val();
    var TotalNoOfWorkedDaysByHolidays = $('#TotalNoOfWorkedDaysByHolidays').val();
    var TotalNoOfLeaveTakenByHolidays = $('#TotalNoOfLeaveTakenByHolidays').val();
    var LeaveToBeCalculated = $('#LeaveToBeCalculated').val();
    $('#txtOnDuty').keyup(function () {
        var NoOfworkingDays = parseInt($('#NoOfworkingDays').val());
        var TotalNoOfLeaveTakenByHolidays = $('#TotalNoOfLeaveTakenByHolidays').val();
        var leaves = NoOfworkingDays - TotalNoOfLeaveTakenByHolidays;
        var LeaveToBeCalculated = $('#LeaveToBeCalculated').val();
        var LeaveTaken = $('#LeavesTaken').val();
        //alert(leaves);
        //if ($('#txtOnDuty').val() < leaves) {
            if ($('#txtOnDuty').val() == "" || $('#txtTotalNoOfPermissionLeaves').val() == "") {
                $('#txtTotalNoOfDaysWorked').val($('#TotalNoOfWorkedDaysByHolidays').val());
                $('#txtNoOfDaysLeaveTaken').val($('#TotalNoOfLeaveTakenByHolidays').val());
                $('#txtLeavesToBeCalculated').val($('#LeaveToBeCalculated').val());
                $('#txtClosingBalance').val($('#ClosingBalance').val());
            }
            else {
                WorkedAndLeavesDaysCalculation();
            }
        //}
        //else {
        //    ErrMsg("Please fill less then the worked days!!");
        //    $('#txtOnDuty').val($('#txtOnDuty').val());
        //    return false;
        //}
    });
    $('#txtTotalNoOfPermissionLeaves').keyup(function () {
        if ($('#txtTotalNoOfPermissionLeaves').val() == "") {
            $('#txtTotalNoOfDaysWorked').val($('#TotalNoOfWorkedDaysByHolidays').val());
                $('#txtNoOfDaysLeaveTaken').val($('#TotalNoOfLeaveTakenByHolidays').val());
                $('#txtLeavesToBeCalculated').val($('#LeaveToBeCalculated').val());
                $('#txtClosingBalance').val($('#ClosingBalance').val());
        }
        else if ($('#txtTotalNoOfPermissionLeaves').val() == 0) {
            $('#txtTotalNoOfDaysWorked').val($('#TotalNoOfWorkedDaysByHolidays').val());
            $('#txtNoOfDaysLeaveTaken').val($('#TotalNoOfLeaveTakenByHolidays').val());
            $('#txtLeavesToBeCalculated').val($('#LeaveToBeCalculated').val());
            $('#txtClosingBalance').val($('#ClosingBalance').val());
        }
        else {
            WorkedAndLeavesDaysCalculation();
        }
    });
    $("#btnSave").click(function () {
        debugger;
        var TotalNoOfDaysWorked = parseInt($('#TotalNoOfDaysWorked').val());
        var TotalNoOfLeaves = parseInt($('#NoOfDaysLeaveTaken').val());
        var PreRegNum = $('#PreRegNum').val();
        var AttendanceMonth = $('#AttendanceMonth').val();
        var AttendanceYear = $('#AttendanceYear').val();
        var OpeningBalance = $('#OpeningBalance').val();
        var AllotedCL = parseFloat($('#AllotedCL').val());
        var NoOfHolidays = $('#NoOfHolidays').val();
        var LeaveTaken = parseFloat($('#txtCLTaken').val());
        var Onduty = parseFloat($('#txtOnDuty').val());
        var ChangeTotalNoOfDaysWorked = parseFloat($('#txtTotalNoOfDaysWorked').val());
        var ChangeNoOfLeavesCalculated = parseFloat($('#txtNoOfDaysLeaveTaken').val());
        var TotalNoOfPermissionLeaves = parseFloat($('#txtTotalNoOfPermissionLeaves').val());
        var LeaveToBeCalculated = parseFloat($('#txtLeavesToBeCalculated').val());
        var ClosingBalance = parseFloat($('#txtClosingBalance').val());
        var PermissionsInHours = parseFloat($('#txtNoOfPermission').val());
        var Remarks = $('#txtRemarks').val();
        //if (Remarks == "") {
        //    ErrMsg("Please Enter the Remarks");
        //    return false;
        //}
        $.ajax({
            type: 'POST',
            dataType: 'json',
            async: false,
            url: '/StaffManagement/SaveStaffAttendanceChangeDetails',
            data: {
                TotalNoOfDaysWorkedByChange: ChangeTotalNoOfDaysWorked, TotalNoOfLeavesTakenByChange: ChangeNoOfLeavesCalculated,
                PreRegNum: PreRegNum, TotalNoOfLeavesTakenByLogs: TotalNoOfLeaves, TotalNoOfDaysWorkedByLogs: TotalNoOfDaysWorked, AttendanceMonth: AttendanceMonth, AttendanceYear: AttendanceYear,
                NoOfLeavesCalculatedByPermissions: TotalNoOfPermissionLeaves, OnDuty: Onduty, NoOfPermissionsTaken: PermissionsInHours,
                NoOfHolidays: NoOfHolidays, OpeningBalance: OpeningBalance, AllotedCl: AllotedCL, ClosingBalance: ClosingBalance, LeaveToBeCalculated: LeaveToBeCalculated,
                TotalAvailableBalance: LeaveTaken, Remarks: Remarks
            },
            success: function (data) {
                if (data != null) {
                    $('#divEditStaffAttendanceDetails').dialog('close');
                    SucessMsg('Added Sucessfully!!');
                    reloadGrid();
                    return true;
                }
            }
        });
    });

});
$('#btnExit').click(function () {
    $('#divEditStaffAttendanceDetails').dialog('close');
});
$(function () {
    //called when key is pressed in textbox
    $("#txtCasualLeaves").keypress(function (e) {
        //if the letter is not digit then display error and don't type anything
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //display error message
            $("#errmsg").html("Digits Only").show().fadeOut("slow");
            return false;
        }
    });
    //$("#txtOnDuty").keypress(function (e) {
    //    //if the letter is not digit then display error and don't type anything
    //    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
    //        //display error message
    //        $("#errmsg1").html("Digits Only").show().fadeOut("slow");
    //        return false;
    //    }
    //});
    $("#txtNoOfPermission").keypress(function (e) {
        //if the letter is not digit then display error and don't type anything
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //display error message
            $("#errmsg2").html("Digits Only").show().fadeOut("slow");
            return false;
        }
    });
    $("#txtOnDuty").keydown(function (event) {
        if (event.shiftKey == true) {
            event.preventDefault();
        }
        if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)
            || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 ||
            event.keyCode == 46 || event.keyCode == 190) {
        }
        else {
            event.preventDefault();
        }
        if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
            event.preventDefault();
    });
    $("#txtTotalNoOfPermissionLeaves").keydown(function (event) {
        if (event.shiftKey == true) {
            event.preventDefault();
        }
        if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)
            || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 ||
            event.keyCode == 46 || event.keyCode == 190) {
        }
        else {
            event.preventDefault();
        }
        if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
            event.preventDefault();
    });
});
//function WorkedAndLeaveDaysCalculation() {
//    var TotalNoOfLeaveCal = parseFloat($('#txtTotalNoOfLeaveCal').val());
//    var CashualLeaves = parseInt($('#txtCasualLeaves').val());
//    var TotalNoOfDaysWorked = parseFloat($('#TotalNoOfDaysWorked').val());
//    var OnDuty = parseFloat($('#txtOnDuty').val());
//    var NoOfDaysLeaveTaken = parseFloat($('#NoOfDaysLeaveTaken').val());
//    parseFloat($('#txtNoOfDaysLeaveTaken').val((NoOfDaysLeaveTaken - OnDuty) + TotalNoOfLeaveCal));
//    parseFloat($('#txtTotalNoOfDaysWorked').val((TotalNoOfDaysWorked + OnDuty) - TotalNoOfLeaveCal));
//    var NoOfLeavesCalculatedByPermissions = $('#txtNoOfDaysLeaveTaken').val();
//}
//function WorkedAndLeaveDaysCalculationUsingOD() {
//    var TotalNoOfLeaveCal = parseFloat($('#txtTotalNoOfLeaveCal').val());
//    var Onduty = parseFloat($('#txtOnDuty').val());
//    var TotalNoOfDaysWorked = parseFloat($('#TotalNoOfDaysWorked').val());
//    var ChangeTotalNoOfDaysWorked = parseFloat($('#txtTotalNoOfDaysWorked').val());
//    var NoOfDaysLeaveTaken = parseFloat($('#NoOfDaysLeaveTaken').val());
//    parseFloat($('#txtTotalNoOfDaysWorked').val((TotalNoOfDaysWorked + Onduty)));
//    parseFloat($('#txtNoOfDaysLeaveTaken').val((NoOfDaysLeaveTaken - Onduty)));
//    var ChangeTotalNoOfDaysWorked = parseFloat($('#txtTotalNoOfDaysWorked').val());
//}


function WorkedAndLeavesDaysCalculation() {
    debugger;
    var NoOfHolidays = parseInt($('#NoOfHolidays').val());
    var NoOfworkingDays = parseInt($('#NoOfworkingDays').val());
    var TotalNoOfDaysWorked = parseFloat($('#TotalNoOfDaysWorked').val());
    var Onduty = parseFloat($('#txtOnDuty').val());
    var NoOfDaysLeaveTaken = parseFloat($('#NoOfDaysLeaveTaken').val());
    var TotalNoOfPermissionLeaves = parseFloat($('#txtTotalNoOfPermissionLeaves').val());
    var AllowedCasualLeaves = $('#AllowedCasualLeave').val();
    var LeaveTaken = parseFloat($('#txtCLTaken').val());
    if ($("#Exist").val() == 1)
    {
        if ($('#txtOnDuty').val() == 0 || $('#txtTotalNoOfPermissionLeaves').val() == "") {
            parseFloat($('#txtTotalNoOfDaysWorked').val((((TotalNoOfDaysWorked + Onduty) - TotalNoOfPermissionLeaves))));
            parseFloat($('#txtNoOfDaysLeaveTaken').val((((NoOfworkingDays - TotalNoOfDaysWorked - Onduty) + TotalNoOfPermissionLeaves))));
            var ChangeTotalNoOfDaysWorked = parseFloat($('#txtTotalNoOfDaysWorked').val());
            var ChangeNoOfLeavesCalculated = parseFloat($('#txtNoOfDaysLeaveTaken').val());
            parseFloat($('#txtLeavesToBeCalculated').val(LeaveTaken - ChangeNoOfLeavesCalculated));
            var LeaveToBeCalculated = parseFloat($('#txtLeavesToBeCalculated').val());
        }
        else {
            parseFloat($('#txtTotalNoOfDaysWorked').val((((TotalNoOfDaysWorked + Onduty) - TotalNoOfPermissionLeaves))));
            parseFloat($('#txtNoOfDaysLeaveTaken').val(((NoOfworkingDays - TotalNoOfDaysWorked - Onduty) + TotalNoOfPermissionLeaves)));
            var ChangeTotalNoOfDaysWorked = parseFloat($('#txtTotalNoOfDaysWorked').val());
            var ChangeNoOfLeavesCalculated = parseFloat($('#txtNoOfDaysLeaveTaken').val());
            parseFloat($('#txtLeavesToBeCalculated').val(LeaveTaken - ChangeNoOfLeavesCalculated));
            var LeaveToBeCalculated = parseFloat($('#txtLeavesToBeCalculated').val());


        }
        debugger;
        var ChangeTotalNoOfDaysWorked = parseFloat($('#txtTotalNoOfDaysWorked').val());
        if (ChangeTotalNoOfDaysWorked <= 26) {
            if (LeaveTaken > ChangeNoOfLeavesCalculated) {
                var LeaveToBeCalculateds = parseFloat($('#txtLeavesToBeCalculated').val("0"));
                var ClosingBalance = parseFloat($('#txtClosingBalance').val(LeaveToBeCalculated));
            }
            else {
                var LeaveToBeCalculateds = parseFloat($('#txtLeavesToBeCalculated').val());
                var ClosingBalance = parseFloat($('#txtClosingBalance').val("0"));
            }
        }
        else {
            var ChangeNoOfLeavesCalculated = parseFloat($('#txtNoOfDaysLeaveTaken').val("0"));
            var LeaveToBeCalculateds = parseFloat($('#txtLeavesToBeCalculated').val("0"));
            $('#txtClosingBalance').val($('#LeavesTaken').val());
            var ClosingBalance = $('#txtClosingBalance').val();
        }
    }
    else
    {
        if ($('#txtOnDuty').val() == 0 || $('#txtTotalNoOfPermissionLeaves').val() == "") {
            parseFloat($('#txtTotalNoOfDaysWorked').val((((TotalNoOfDaysWorked + Onduty) - TotalNoOfPermissionLeaves) + NoOfHolidays)));
            parseFloat($('#txtNoOfDaysLeaveTaken').val((((NoOfworkingDays - TotalNoOfDaysWorked - Onduty) + TotalNoOfPermissionLeaves) - NoOfHolidays)));
            var ChangeTotalNoOfDaysWorked = parseFloat($('#txtTotalNoOfDaysWorked').val());
            var ChangeNoOfLeavesCalculated = parseFloat($('#txtNoOfDaysLeaveTaken').val());
            parseFloat($('#txtLeavesToBeCalculated').val(LeaveTaken - ChangeNoOfLeavesCalculated));
            var LeaveToBeCalculated = parseFloat($('#txtLeavesToBeCalculated').val());
        }
        else {
            parseFloat($('#txtTotalNoOfDaysWorked').val((((TotalNoOfDaysWorked + Onduty) - TotalNoOfPermissionLeaves) + NoOfHolidays)));
            parseFloat($('#txtNoOfDaysLeaveTaken').val(((NoOfworkingDays - TotalNoOfDaysWorked - Onduty) + TotalNoOfPermissionLeaves) - NoOfHolidays));
            var ChangeTotalNoOfDaysWorked = parseFloat($('#txtTotalNoOfDaysWorked').val());
            var ChangeNoOfLeavesCalculated = parseFloat($('#txtNoOfDaysLeaveTaken').val());
            parseFloat($('#txtLeavesToBeCalculated').val(LeaveTaken - ChangeNoOfLeavesCalculated));
            var LeaveToBeCalculated = parseFloat($('#txtLeavesToBeCalculated').val());


        }
        debugger;
        var ChangeTotalNoOfDaysWorked = parseFloat($('#txtTotalNoOfDaysWorked').val());
        if (ChangeTotalNoOfDaysWorked <= 26) {
            if (LeaveTaken > ChangeNoOfLeavesCalculated) {
                var LeaveToBeCalculateds = parseFloat($('#txtLeavesToBeCalculated').val("0"));
                var ClosingBalance = parseFloat($('#txtClosingBalance').val(LeaveToBeCalculated));
            }
            else {
                var LeaveToBeCalculateds = parseFloat($('#txtLeavesToBeCalculated').val());
                var ClosingBalance = parseFloat($('#txtClosingBalance').val("0"));
            }
        }
        else {
            var ChangeNoOfLeavesCalculated = parseFloat($('#txtNoOfDaysLeaveTaken').val("0"));
            var LeaveToBeCalculateds = parseFloat($('#txtLeavesToBeCalculated').val("0"));
            $('#txtClosingBalance').val($('#LeavesTaken').val());
            var ClosingBalance = $('#txtClosingBalance').val();
        }
    }

}
function reloadGrid() {
    debugger;
    $('#StaffConsolidateSummaryNewGridList').setGridParam(
         {
             datatype: "json",
             url: '/BioMetricAttendance/StaffConsolidateSummaryNewJqGrid?AttendanceFromDate=' + $('#AttendanceFromDate').val() + '&AttendanceToDate=' + $('#AttendanceToDate').val(),
             postData: {
                 Campus: $("#ddlCampus").val(), StaffType: $("#ddlStaffType").val(), IdNumber: $("#StaffId").val(), StaffName: $("#StaffName").val(), MonthYear: $("#txtMonthYear").val()
             },
         }).trigger("reloadGrid");

}