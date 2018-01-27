$(function () {



    $('#txtDateOfJoining').datepicker({
        format: "dd/mm/yyyy",
        beforeShowDay: function (Date) {
        },
        autoclose: true
    });




    $.getJSON("/Base/FillBranchCode",
function (fillig) {
    var ddlcam = $("#Campus");
    ddlcam.empty();
    ddlcam.append($('<option/>',
{
    value: "",
    text: "Select One"

}));

    $.each(fillig, function (index, itemdata) {
        ddlcam.append($('<option/>',
{
    value: itemdata.Value,
    text: itemdata.Text
}));
    });
});



    $('#Campus').change(function () {

        $.getJSON("/HostelManagement/GetHostelName/", { campus: $('#Campus').val() },
        function (fillig) {
            var ddlcam = $("#hostelddl");
            ddlcam.empty();
            ddlcam.append($('<option/>',
        {
            value: "",
            text: "Select One"

        }));

            $.each(fillig, function (index, itemdata) {
                ddlcam.append($('<option/>',
        {
            value: itemdata.Value,
            text: itemdata.Text
        }));
            });
        });
    });




    $("#hostelddl").change(function () {
        GetTypeddl();
    });

    $('#ddlType').change(function () {
        if ($('#hostelddl').val() == "") { ErrMsg("Please fill the Hostel"); return false; }
        if ($('#ddlType').val() == "") { ErrMsg("Please fill the Type"); return false; }
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        $.getJSON("/HostelManagement/GetFloorDetails/", { campus: $('#Campus').val(), hstName: $('#hostelddl').val(), hstType: $('#ddlType').val() },
       function (fillig) {
           var ddlcam = $("#ddlFloor");
           ddlcam.empty();
           ddlcam.append($('<option/>',
       {
           value: "",
           text: "Select One"

       }));

           $.each(fillig, function (index, itemdata) {
               ddlcam.append($('<option/>',
       {
           value: itemdata.Value,
           text: itemdata.Text
       }));
           });
       });
        //$("#landingPage").setGridParam(
        //        {
        //            datatype: "json",
        //            url: '/HostelManagement/JqgridLandingPage',
        //            postData: { location: $('#lction').val(), type: $('#ddlType').val() },
        //            page: 1
        //        }).trigger("reloadGrid");
    });


    $('#ddlFloor').change(function () {
        if ($('#hostelddl').val() == "") { ErrMsg("Please fill the Hostel"); return false; }
        if ($('#ddlType').val() == "") { ErrMsg("Please fill the Type"); return false; }
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        $.getJSON("/HostelManagement/GetRoomListDetails/", { campus: $('#Campus').val(), hstName: $('#hostelddl').val(), hstType: $('#ddlType').val(), floor: $("#ddlFloor").val() },
       function (fillig) {
           var ddlcam = $("#ddlRoom");
           ddlcam.empty();
           ddlcam.append($('<option/>',
       {
           value: "",
           text: "Select One"

       }));

           $.each(fillig, function (index, itemdata) {
               ddlcam.append($('<option/>',
       {
           value: itemdata.Value,
           text: itemdata.Text
       }));
           });
       });
        //$("#landingPage").setGridParam(
        //        {
        //            datatype: "json",
        //            url: '/HostelManagement/JqgridLandingPage',
        //            postData: { location: $('#lction').val(), type: $('#ddlType').val() },
        //            page: 1
        //        }).trigger("reloadGrid");
    });

    $("#ddlRoom").change(function () {
        if ($('#ddlRoom').val() == "") {
            ErrMsg('Please select Available Rooms');
            return false;
        } else {

            $.ajax({
                url: "/HostelManagement/GetRoomsDtls?rmNum=" + $('#ddlRoom').val(),
                success: function (data) {
                    $('#BedDisplay').html(data);

                }
            });

        }


    });



});

function GetTypeddl() {
    $.getJSON("/HostelManagement/GetType/", { hostelNm: $('#hostelddl').val(), campus: $('#Campus').val() },
function (modelData) {
    var select = $("#ddlType");
    select.empty();
    select.append($('<option/>'
, {
    value: "",
    text: "Select Type"
}));
    $.each(modelData, function (index, itemData) {
        debugger;
        select.append($('<option/>',
{
    value: itemData.Value,
    text: itemData.Text
}));
    });
});
}

function GetSelectedBedLst(BedMst_Id, BedNumber, HstlMst_Id) {
    ErrMsg("Sorry, it's reserved");
    return false;
}

function GetBedLst(BedMst_Id, BedNumber, HstlMst_Id) {

    var date = new Date();
    var day = date.getDay();        // yields day
    var month = date.getMonth();    // yields month
    var year = date.getFullYear();  // yields year
    var hour = date.getHours();     // yields hours 
    var minute = date.getMinutes(); // yields minutes
    var second = date.getSeconds(); // yields seconds

    // After this construct a string with the above results as below
    // var time = day + "/" + month + "/" + year + " " + hour + ':' + minute + ':' + second;
    var time = day + "/" + month + "/" + year;

    if ($('#hostelddl').val() == "") {
        ErrMsg("Please fill the Hostel");
        return false;
    }
    else if ($('#ddlType').val() == "") {
        ErrMsg("Please fill the Type");
        return false;
    }
    else if ($('#Campus').val() == "") {
        ErrMsg("Please fill the Campus");
        return false;
    }
    else if ($('#ddlFloor').val() == "") {
        ErrMsg("Please fill the Floor");
        return false;
    }
    else if ($('#AvbRoomsId').val() == "") {
        ErrMsg("Please fill the Room");
        return false;
    } else if ($('#txtDateOfJoining').val() == "") {
        ErrMsg("Please fill the Date Of Joining");
        return false;
    }
    else if ($('#txtSIPNo').val() == "") {
        ErrMsg("Please fill the SIP Number");
        return false;
    }
    else {

        if ($('#hdnFlag').val() == "ChangeRoomAllotment") {
            $.ajax({
                type: 'GET',
                async: false,
                dataType: "json",
                url: '/HostelManagement/RoomAllocation?hstName=' + $('#hostelddl').val() + '&hstType=' + $('#ddlType').val() + '&hstCampus=' + $('#Campus').val() + '&hstFloor=' + $("#ddlFloor").val() + '&hstId=' + HstlMst_Id + '&AvbRoomsId=' + $('#ddlRoom').val() + '&roomNum=' + $("#ddlRoom option:selected").text() + '&bedId=' + BedMst_Id + '&bedNum=' + BedNumber + '&studID=' + $('#hdnStudID').val() + '&dateofjoining=""&sipno=""&Flag=ChangeRoomAllocation',
                success: function (data) {
                    $.ajax({
                        url: "/HostelManagement/GetRoomsDtls?rmNum=" + $('#ddlRoom').val(),
                        success: function (data) {
                            $('#BedDisplay').html(data);
                        }
                    });
                }
            });
        } else {
            $.ajax({
                type: 'GET',
                async: false,
                dataType: "json",
                url: '/HostelManagement/RoomAllocation?hstName=' + $('#hostelddl').val() + '&hstType=' + $('#ddlType').val() + '&hstCampus=' + $('#Campus').val() + '&hstFloor=' + $("#ddlFloor").val() + '&hstId=' + HstlMst_Id + '&AvbRoomsId=' + $('#ddlRoom').val() + '&roomNum=' + $("#ddlRoom option:selected").text() + '&bedId=' + BedMst_Id + '&bedNum=' + BedNumber + '&studID=' + $('#hdnStudID').val() + '&dateofjoining=' + $('#txtDateOfJoining').val() + '&sipno=' + $('#txtSIPNo').val() + '&Flag=RoomAllocation',
                success: function (data) {
                    if (data == "Already exists") {
                        ErrMsg(data);
                    }
                    $.ajax({
                        url: "/HostelManagement/GetRoomsDtls?rmNum=" + $('#ddlRoom').val(),
                        success: function (data) {
                            $('#BedDisplay').html(data);
                        }
                    });
                }
            });
        }


    }

}