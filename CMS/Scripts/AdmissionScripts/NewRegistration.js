jQuery(function ($) {

    //window.onload = $("#myTab").tabs("option", "disabled", [1]);


    //$('.nav li.active').tabs

    var tabindex = "";
    if ($('userrole').val() == "ADM-APP") {
        $(function () {
            $("#myTab").tabs({ selected: 6 });
        });
    }
    else {
        if ($('Tabselected').val() == "Payment") {
            $(function () {
                $("#tabs").tabs({ selected: 5 });
            });
        }
        else {
            $(function () {
                $("#myTab").tabs({
                });
            });
        }
    }
    if ($('#errmsg').val() == "Not Paid") {
        ErrMsg('Applicant still not paid Registration Fee');
        $(function () {
            $("#myTab").tabs({ selected: 5 });
        });
    }

    else if ($('errmsg').val() == "Paid") {
        ErrMsg('Fee Type Already Added');
        $(function () {
            $("#myTab").tabs({ selected: 5 });
        });
    }
    else { }

    VanNumberddl($("#ddlcampus").val());

    window.onload = "PastSchoolDetails.cshtml";

    $("#Back").click(function () {
        // window.location.href = 'Url.Action("AdmissionManagement", "Admission")';
        window.location.href = "/Admission/AdmissionManagement?resetsession=no";
    });

    $("#Back1").click(function () {
        //window.location.href = "/Admission/NewRegistration?resetsession=no";   // rowData1
        window.location.href = "/Admission/StudentManagement?pagename=stmgmt";
    });


    var btnlst = null;
    $("#link1").find('a').click(function () {
        if (btnlst) {
            btnlst.removeClass("ui-state-hover ui-state-focus").addClass("ui-state-default").button('enable');
        }
        $(this).button('disable');
        btnlst = $(this)
    });
    setTimeout(function () { btnlst = $("#FamilyDetails").click().button('disable'); }, 200)

    //$('#myTab a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    //    if ($(e.target).attr('href') == "#StudentDetails") drawChartNow();
    //})

    //resize to fit page size 
    //$(window).on('resize.jqGrid', function () {
    //    var page_width = $(".page-content").width();
    //    page_width = page_width - 27;
    //    $(grid_selector).jqGrid('setGridWidth', page_width);
    //})

    ////resize on sidebar collapse/expand 
    //var parent_column = $(grid_selector).closest('[class*="col-"]');
    //$(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
    //    if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
    //        //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
    //        setTimeout(function () {
    //            $(grid_selector).jqGrid('setGridWidth', parent_column.width());
    //        }, 0);
    //    }
    //})

});
function VanNumberddl(campus) {
    if (campus != "") {
        $.getJSON("/Base/GetVanRouteNoByCampus", { campus: campus },
function (fillig) {
    var ddlvan = $("#vanno");
    ddlvan.empty();
    ddlvan.append($('<option/>', { value: "", text: "Select One" }));
    $.each(fillig, function (index, itemdata) {
        //alert(itemdata.Value);
        //alert('ViewBag.RouteNo');
        if (itemdata.Value != $('RouteNo').val()) {
            ddlvan.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
        }
        else {
            ddlvan.append($('<option/>', { value: itemdata.Value, text: itemdata.Text, selected: "Selected" }));
        }

    });
});
    }

}

function validate() {

    if (document.getElementById("name").value == "") {
        ErrMsg("Please Provide Applicant's Name!", function () { $('#name').focus(); });
        return false;
    }
    //        else if (document.getElementById("initial").value == "") {
    //            ErrMsg("Please Provide Applicant's Initial with Capital letter!");
    //            return false;
    //        }
    else if (document.getElementById("gender").value == "") {
        ErrMsg("Please Provide Applicant's Gender!", function () { $('#gender').focus(); });
        return false;
    }
    else if (document.getElementById("ddlcampus").value == "") {
        ErrMsg("Please Provide Applicant's Campus!", function () { $('#ddlcampus').focus(); });

        return false;
    }
    else if (document.getElementById("academicyear").value == "") {
        ErrMsg("Please Provide Applicant's Academic Year!", function () { $('#academicyear').focus(); });

        return false;
    }
    else if (document.getElementById("stdntgrade").value == "") {
        ErrMsg("Please Provide Applicant's Grade!", function () { $('#stdntgrade').focus(); });

        return false;
    }
    else if (document.getElementById("dob").value == "") {
        ErrMsg("Please Provide Applicant's DOB!", function () { $('#dob').focus(); });

        return false;
    }
    else if (document.getElementById("createddate").value == "") {
        ErrMsg("Please select Applied Date", function () { $('#createddate').focus(); });

        return false;
    }
    else if ($('#admissionstatus').val() != "New Enquiry") {
        if (document.getElementById("applicationno").value == "") {
            ErrMsg("Please Provide Applicant's ApplicationNo!");
            return false;
        }
        else if (document.getElementById("boardingtype").value == "") {
            ErrMsg("Please Select Applicants Boarding Type!");
            return false;
        }
        else if (document.getElementById("emailid").value == "") {
            ErrMsg("Please Enter Email Id");
            return false;
        }
        else return true;
    }
    else {
        return true;
    }
}

function VanNumberddl(campus) {
    if (campus != "") {
        $.getJSON("/Base/GetVanRouteNoByCampus", { campus: campus },
function (fillig) {
    var ddlvan = $("#vanno");
    ddlvan.empty();
    ddlvan.append($('<option/>', { value: "", text: "Select One" }));
    $.each(fillig, function (index, itemdata) {
        //alert(itemdata.Value);
        //alert('ViewBag.RouteNo');
        if (itemdata.Value != $('RouteNo').val()) {
            ddlvan.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
        }
        else {
            ddlvan.append($('<option/>', { value: itemdata.Value, text: itemdata.Text, selected: "Selected" }));
        }

    });
});
    }

}