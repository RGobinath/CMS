jQuery(function ($) {
    $.getJSON("http://jsonip.appspot.com?callback=?",
   function (data) {
       alert("Your ip: " + data.ip);
   });
    $("#txtStaffIdNumber").val('TIPS-');
    $("#btncheckin").hide();
    $("#btncheckout").hide();
    //$("#ddlcampus").change(function () {
    //StaffIdNumberAutoComplete();
    //});
    //$("#txtStaffIdNumber").focus(function () {
    //    if ($("#ddlcampus").val() == "") {
    //        ErrMsg("Please Select Campus");
    //        return false;
    //    }
    //});
    $("#btncheckin").click(function () {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            async: false,
            url: '/StaffManagement/SaveStaffAttendanceDetails?PreRegNum=' + $("#StaffPreRegNum").val(),
            success: function (data) {
                if (data == "success") {
                    $("#lbl_Name").text('');
                    $("#lbl_Campus").text('');
                    $("#lbl_Department").text('');
                    $("#lbl_Designation").text('');
                    $("#lbl_IdNumber").text('');
                    $("#StaffPreRegNum").val('');
                    $("#txtStaffIdNumber").val('TIPS-');
                    document.getElementById("myImage").src = "/Images/no_image.jpg";
                    $("#btncheckin").hide();
                    $("#btncheckout").hide();
                    SucessMsg("Check In is Done.");
                    return true;
                }
                else if (data == "failed") {
                    ErrMsg("Please Try Again.");
                    return false;
                }
            }
        });
    });
    $("#btncheckout").click(function () {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            async: false,
            url: '/StaffManagement/EditStaffAttendanceDetails?PreRegNum=' + $("#StaffPreRegNum").val(),
            success: function (data) {
                if (data == "success") {
                    $("#lbl_Name").text('');
                    $("#lbl_Campus").text('');
                    $("#lbl_Department").text('');
                    $("#lbl_Designation").text('');
                    $("#lbl_IdNumber").text('');
                    $("#StaffPreRegNum").val('');
                    $("#txtStaffIdNumber").val('TIPS-');
                    document.getElementById("myImage").src = "/Images/no_image.jpg";
                    $("#btncheckin").hide();
                    $("#btncheckout").hide();
                    SucessMsg("Check Out is Done.");
                    return true;
                }
                else if (data == "failed") {
                    ErrMsg("Please Try Again");
                    return false;
                }
            }
        });
    });
    $("#reset").click(function () {
        $("#btncheckin").hide();
        $("#btncheckout").hide();
        $("#lbl_Name").text('');
        $("#lbl_Campus").text('');
        $("#lbl_Department").text('');
        $("#lbl_Designation").text('');
        $("#lbl_IdNumber").text('');
        $("#StaffPreRegNum").val('');
        $("#ddlcampus").val('');
        $("#txtStaffIdNumber").val('TIPS-');
        document.getElementById("myImage").src = "/Images/no_image.jpg";
        return false;
    });
    var cache = {};
    $("#txtStaffIdNumber").autocomplete({        
        source: function (request, response) {
            debugger;
            var term = request.term;            
            if (term in cache) {
                debugger;
                response($.map(cache[term], function (item) {
                    debugger;
                    return { label: item.IdNumber, value: item.PreRegNum }
                }))
                return;
            }
            //else {
                $.getJSON("/StaffManagement/StaffIdNumberAutoComplete", request, function (data, status, xhr) {
                    cache[term] = data;
                    //response(data);
                    response($.map(data, function (item) {
                        return { label: item.IdNumber, value: item.PreRegNum }
                    }))
                });
            //}
        },
        minLength: 1,
        delay: 100,
        select: function (event, ui) {
            debugger;
            event.preventDefault();
            $("#StaffPreRegNum").val(ui.item.value);
            $("#txtStaffIdNumber").val(ui.item.label);
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#txtStaffIdNumber").val(ui.item.label);
        },
        messages: {
            noResults: "", results: ""
        }
    });
});
//function StaffIdNumberAutoComplete() {
//    //var Campus = $("#ddlcampus").val();
//    //if (Campus != "") {
//        $("#txtStaffIdNumber").autocomplete({
//            source: function (request, response) {
//                if (request.term.length <= 10) {
//                    return false;
//                }
//                if (request.term.length > 10) {
//                    $.ajax({
//                        url: "/StaffManagement/StaffIdNumberAutoComplete",
//                        type: "POST",
//                        dataType: "json",
//                        data: { term: request.term, Campus: "" },
//                        success: function (data) {
//                            response($.map(data, function (item) {
//                                return { label: item.IdNumber, value: item.PreRegNum }
//                            }))
//                        }
//                    })
//                }
//            },
//            minLength: 1,
//            delay: 100,
//            select: function (event, ui) {
//                debugger;
//                event.preventDefault();
//                $("#StaffPreRegNum").val(ui.item.value);
//                $("#txtStaffIdNumber").val(ui.item.label);
//            },
//            focus: function (event, ui) {
//                event.preventDefault();
//                $("#txtStaffIdNumber").val(ui.item.label);
//            },
//            messages: {
//                noResults: "", results: ""
//            }
//        });
//        return false;
//    //}
//    //else {
//    //    return false;
//    //}
//}
function startTime() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    m = checkTime(m);
    s = checkTime(s);
    document.getElementById('txt').innerHTML =
    h + ":" + m + ":" + s;
    var t = setTimeout(startTime, 500);
}
function checkTime(i) {
    if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
    return i;
}
var canvas = document.getElementById("canvas");
var ctx = canvas.getContext("2d");
var radius = canvas.height / 2;
ctx.translate(radius, radius);
radius = radius * 0.90
setInterval(drawClock, 1000);
function drawClock() {
    drawFace(ctx, radius);
    drawNumbers(ctx, radius);
    drawTime(ctx, radius);
}

function drawFace(ctx, radius) {
    var grad;
    ctx.beginPath();
    ctx.arc(0, 0, radius, 0, 2 * Math.PI);
    ctx.fillStyle = '#fff';
    ctx.fill();
    grad = ctx.createRadialGradient(0, 0, radius * 0.95, 0, 0, radius * 1.05);
    grad.addColorStop(0, '#333');
    grad.addColorStop(0.5, '#fff');
    grad.addColorStop(1, '#fff');
    ctx.strokeStyle = grad;
    ctx.lineWidth = radius * 0.1;
    ctx.stroke();
    ctx.beginPath();
    ctx.arc(0, 0, radius * 0.1, 0, 2 * Math.PI);
    ctx.fillStyle = '#333';
    ctx.fill();
}

function drawNumbers(ctx, radius) {
    var ang;
    var num;
    ctx.font = radius * 0.15 + "px arial";
    ctx.textBaseline = "middle";
    ctx.textAlign = "center";
    for (num = 1; num < 13; num++) {
        ang = num * Math.PI / 6;
        ctx.rotate(ang);
        ctx.translate(0, -radius * 0.85);
        ctx.rotate(-ang);
        ctx.fillText(num.toString(), 0, 0);
        ctx.rotate(ang);
        ctx.translate(0, radius * 0.85);
        ctx.rotate(-ang);
    }
}

function drawTime(ctx, radius) {
    var now = new Date();
    var hour = now.getHours();
    var minute = now.getMinutes();
    var second = now.getSeconds();
    //hour
    hour = hour % 12;
    hour = (hour * Math.PI / 6) +
    (minute * Math.PI / (6 * 60)) +
    (second * Math.PI / (360 * 60));
    drawHand(ctx, hour, radius * 0.5, radius * 0.07);
    //minute
    minute = (minute * Math.PI / 30) + (second * Math.PI / (30 * 60));
    drawHand(ctx, minute, radius * 0.8, radius * 0.07);
    // second
    second = (second * Math.PI / 30);
    drawHand(ctx, second, radius * 0.9, radius * 0.02);
}

function drawHand(ctx, pos, length, width) {
    ctx.beginPath();
    ctx.lineWidth = width;
    ctx.lineCap = "round";
    ctx.moveTo(0, 0);
    ctx.rotate(pos);
    ctx.lineTo(0, -length);
    ctx.stroke();
    ctx.rotate(-pos);
}
