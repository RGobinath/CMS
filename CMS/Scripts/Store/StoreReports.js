$(function () {
    $("#ddlReportType").change(function () {

        var modId = $(this).val();
        
        if (modId == "MIN") {
            $.get("MaterialInwardReport", "Store", function (result) {
                $("#divReport").html(result);
            });
        }

        if (modId == "MRF") {
            $.get("MaterialRequestReport", "Store", function (result) {
                $("#divReport").html(result);
            });
        }
        if (modId == "ISN") {
            $.get("MaterialIssueNoteReport", "Store", function (result) {
                $("#divReport").html(result);
            });
        }
    });
});