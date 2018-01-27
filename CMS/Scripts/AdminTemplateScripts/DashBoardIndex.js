jQuery(function ($) {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/CampusWiseCountsChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_MSColumn2DLineDY.swf", "DashboaardIndex", "700", "350");
            chart.setDataXML(data);
            chart.render("DashBoardIndexCampusChart");
        }
    });
});