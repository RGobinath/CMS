jQuery(function ($) {
    $("#sidno1").hide();
    $("#sidicon").hide();
    $("#Back").click(function () {
        window.location.href = '/StaffManagement/NewStaffDisplay';
    });

    $("#Back1").click(function () {
        window.location.href = '/StaffManagement/StaffDisplay';
    });
    $("#BackToRpt").click(function () {
        window.location.href = '/Common/DocumentReport';
    });
    $("#BackToProfile").click(function () {
        window.location.href = '/StaffManagement/StaffProfile';
    });
    //$("#sidicon").click(function () {
    //    $("#sidno1").hide();
    //    //$("#sidicon").hide();
    //});
    $("#Status").change(function () {
        if ($("#Status").val() == "Registered" && $("#viewbagidno").val() == "" && $("#SessionStatus").val() == "Sent For Approval") {
            $("#sidno1").show();
            //$("#sidicon").show();
        }
        else {
            $("#sidno1").hide();
            //$("#sidicon").hide();
        }
        //if ($("#sidno").val() == "" && ($("#SessionStatus").val() == "New Registration" || $("#SessionStatus").val() == "Sent For Approval")) {
        //    $("#sidno").removeProp('readonly');
        //}
        //else {
        //    $("#sidno").prop('readonly',true);
        //}
    });
});

function validate1() {
    debugger;
    var email = document.getElementById("emailid").value;
    var docrecs = parseInt($("#DocumentList").getGridParam("records"), 10);
    var qualrecs = parseInt($("#QualificationList").getGridParam("records"), 10);
    document.getElementById("doccheck").value = "";
    document.getElementById("qualcheck").value = "";
    if (document.getElementById("sname").value == "") {
        ErrMsg("Please Enter Name!");
        return false;
    }
    else if (document.getElementById("ddlcampus").value == "") {
        ErrMsg("Please Select Campus!");
        return false;
    }
    else if ($.trim(email).length == 0) {
        ErrMsg("Please Enter Personal Email Id!");
        return false;
    }
    else if ($.trim(email).length > 1) {
        validateCaseSensitiveEmail(email);
        if (validateCaseSensitiveEmail(email) == false) {
            return false;
        }
    }
    else if (isNaN(docrecs) || docrecs == 0) {
        //ErrMsg("Please Upload Documents!");
        $("#tabs").tabs({ selected: 0 });
        document.getElementById("doccheck").value = "yes";
        return true;
    }
    else if (isNaN(qualrecs) || qualrecs == 0) {
        //  ErrMsg("Please Enter Qualification Details!");
        document.getElementById("qualcheck").value = "yes";
        return true;
        $("#tabs").tabs({ selected: 1 });
    } else { return true; }
}

function validate2() {
    var staffid = document.getElementById("sidno1").value;
    var staffstatus = document.getElementById("Status").value;
    if (document.getElementById("Status").value == "") {
        ErrMsg("Please Select Status!");
        return false;
    }
    else if ($.trim(staffid).length == 0) {
        ErrMsg("Please Enter Id Number!");
        return false;
    }
    else if ($.trim(staffid).length > 0 && staffstatus == "Registered") {
        //checkidnumber(staffid);
        if (checkidnumber(staffid) == false)
        {
            ErrMsg("Id is Exist");
            return false;
        }
    }
    else {
        return true;
    }
}
function checkidnumber(staffid) {
    var retdata;
    $.ajax({
        type: 'Get',
        async: false,
        url: '/StaffManagement/checkIdNumber?IdNumber=' + staffid,
        success: function (data) {
            if (data == "failed") {
                retdata = false;
            }
            if (data == "success") {
                retdata = true;
            }
        }
    });
    return retdata;
}
function SendMailClick() {
    if (document.getElementById("PKId").value == "") {
        ErrMsg("Please Save the details before send mail!");
        return false;
    }
    else {
        var PKId = document.getElementById("PKId").value;
        var Message = "";
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            async: false,
            url: '/StaffManagement/SendStaffLoginDetailsByEmail?id=' + PKId,
            data: { id: PKId },
            success: function (data) {
                debugger;
                if (data != null) {
                    data = data.split(",");
                    if (data[0] == "Email") {
                        Message = "Please Save the Email Id.";
                        SucessMsg(Message);
                        return false;
                    }
                    else if (data[0] == "user" && data[1] == "success") {
                        Message = "User is Created Successfully and Login Details has been sent to Staff Personal Email Id";
                        SucessMsg(Message);
                        return false;
                    }
                    else if (data[0] == "user" && data[1] == "failed") {
                        Message = "User is Created Successfully and Mail has been not sent.";
                        SucessMsg(Message);
                        return false;
                    }
                    else if (data[1] == "success") {
                        Message = "Mail has been sent.";
                        SucessMsg(Message);
                        return false;
                    }
                    else if (data[1] == "failed") {
                        Message = "Mail has been not sent.";
                        SucessMsg(Message);
                        return false;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    Message = "Failed";
                    ErrMsg(Message);
                    return false;
                }
            }
        });
    }
}
function validateCaseSensitiveEmail(email) {
    var reg = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
    if (reg.test(email)) {
        return true;
    }
    else {
        ErrMsg("Invalid Email Id");
        return false;
    }
}