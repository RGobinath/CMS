jQuery(function ($) {
    TransportCampusWiseCountChart();
    TransportVehicleTypeCountChart();

    $("#TransDdlCampusList").change(function () {
        var Campus = $('#TransDdlCampusList').val();
        if (Campus == "All") {
            $("#SingleCampusChart").hide();
            $("#AllCampusChart").show();
        }
        else {
            TransportReportChartForSingleCampus();
            TransportVehicleTypeCountChartForSingleCampus();
            $("#AllCampusChart").hide();
            $("#SingleCampusChart").show();
        }
    });


    $.getJSON("/AdminTemplate/getCampusList",
function (getCampus) {
    var ddlCampusList = $("#TransDdlCampusList");
    ddlCampusList.empty();
    ddlCampusList.append($('<option/>',
{
    value: "All", text: "All Campus"

}));
    $.each(getCampus, function (index, itemdata) {
        ddlCampusList.append($('<option/>',
            {
                value: itemdata.Value,
                text: itemdata.Text
            }));
    });
    });
});
function TransportCampusWiseCountChart() {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/TransportCampusWiseCountChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_StackedArea2D.swf", "TransportCampusWiseCountCharts", "460", "350");
            chart.setDataXML(data);
            chart.render("CampusWiseTransportReport");
        }
    });
}

function TransportVehicleTypeCountChart() {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/TransportVehicleTypeCountChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_StackedColumn2D.swf", "TransportVehicleTypeCountCharts", "460", "350");
            chart.setDataXML(data);
            chart.render("TransportVehicleTypeCountChart");
        }
    });
}

function TransportReportChartForSingleCampus() {
    var Campus = $('#TransDdlCampusList').val();
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/TransportReportChartForSingleCampus?Campus=' + Campus,
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Pie2D.swf", "TransportReportChartForSingleCampusChart", "500", "350");
            chart.setDataXML(data);
            chart.render("TransportReportChartForSingleCampus");
        }
    });
}
function TransportVehicleTypeCountChartForSingleCampus() {
    var Campus = $('#TransDdlCampusList').val();
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/TransportVehicleTypeCountChartForSingleCampus?Campus=' + Campus,
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Pie2D.swf", "TransportVehicleTypeCountChartForSingleCampus1", "500", "350");
            chart.setDataXML(data);
            chart.render("TransportVehicleTypeCountChartForSingleCampus");
        }
    });
}