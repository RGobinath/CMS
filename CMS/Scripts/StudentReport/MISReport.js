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
    $('#ddlCampus').change(function () {
        var ddlCampus = $('#ddlCampus').val();
        if (ddlCampus == "") {
            ddlCampus = "All";
            window.location.href = "/StudentsReport/MISReport?campus=" + ddlCampus;
            $('#SingleCampusReport').hide();
            $('#OverAllReport').show();
        }
        else {
            window.location.href = "/StudentsReport/MISReport?campus=" + ddlCampus;
            $('#OverAllReport').hide();
            $('#SingleCampusReport').show();
            $('#OverAllReport').hide();
        }
    });

    $("#GetMISReport").click(function () {
        var ddlCampus = $("#ddlCampus").val();
        if (ddlCampus == null) ddlCampus = "";
        window.open('/StudentsReport/MISReportWithExcel?Campus=' + 'Session["MISCampus"].ToString()', '_blank');

    });

    if ('Session["MISCampus"]' != null) {

        $('#ddlCampus').val('Session["MISCampus"].ToString()');
    }




});