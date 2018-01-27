$(function () {

    if ($('#Pagename').val() == "Single") {
        $('#OverAllReport').hide();
        $('#SingleCampusReport').show();
    }
    else if ($('#Pagename').val() == "OverAll") {
        $('#SingleCampusReport').hide();
        $('#OverAllReport').show();
    }
    $.getJSON("/StudentsReport/getCampusList",

function (getCampusList) {
    var ddlCampus = $("#ddlCampus");
    ddlCampus.empty();
    ddlCampus.append($('<option/>', { value: "", text: "--Select Campus--" }));
    ddlCampus.append($('<option/>', { value: "All", text: "All Campus" }));
    $.each(getCampusList, function (index, itemdata) {
        ddlCampus.append($('<option/>',
            {
                value: itemdata.Value,
                text: itemdata.Text
            }));
    });
});
    //$('#ddlCampus').val(ddlCampusValue);
    //$('#SingleCampusReport').hide();
    //$('#ddlCampus').change(function () {
    //    var ddlCampus = $('#ddlCampus').val();
    //    var month = $("#ddlMonth").val();
    //    if (ddlCampus == "") {
    //        ddlCampus = "All";
    //        window.location.href = "/StudentsReport/MISReport?campus=" + ddlCampus +month;
    //        $('#SingleCampusReport').hide();
    //        $('#OverAllReport').show();
    //    }
    //    else {
    //        window.location.href = "/StudentsReport/MISReport?campus=" + ddlCampus + month;
    //        $('#OverAllReport').hide();
    //        $('#SingleCampusReport').show();
    //        $('#OverAllReport').hide();
    //    }
    //});

    $("#GetMISReport").click(function () {
        debugger;
        var ddlCampus = $("#ddlCampus").val();
        var month = $("#ddlMonth").val();
        if (ddlCampus == null) ddlCampus = "";
        window.open('/StudentsReport/MISMonthlyReportWithExcel?Campus=' + $('#ddlCampus').val() + "&month=" + $('#ddlMonth').val(), '_blank');

    });


    if ('Session["MISCampus"]' != null) {

        $('#ddlCampus').val('Session["MISCampus"].ToString()');
    }
    var Month = ($('#CurMonth')).val();
    $.getJSON("/Base/FillMonth",
     function (fillig) {
         debugger;
         var mior = $("#ddlMonth");
         mior.empty();
         mior.append("<option value=' '> Select </option>");
         $.each(fillig, function (index, itemdata) {
             if (itemdata.Value != Month) {
                 mior.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
             }
             else {
                 mior.append($('<option/>',
             {
                 value: itemdata.Value,
                 text: itemdata.Text,
                 selected: "Selected"

             }));
             }
         });
     });



});