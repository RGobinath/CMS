$(function () {

    $("#ddlReportType").change(function () {
        debugger;

        var modId = $(this).val();

        if (modId == "OVBC") {
            $.get("OverViewByCampus", "Store", function (result) {
                $("#divReport").html(result);
            });
        }

        if (modId == "OVBM") {
            $.get("OverViewByMaterial", "Store", function (result) {
                $("#divReport").html(result);
            });
        }
        if (modId == "OVBD") {
            $.get("OverviewMaterialDistributionReportByDate", "Store", function (result) {
                $("#divReport").html(result);
            });
        }

    });
});