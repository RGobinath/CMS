jQuery(function ($) {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetBrowserWiseChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Area2D.swf", "GetBrowserWiseChart", "400", "300");
            chart.setDataXML(data);
            chart.render("LoginBrowserChart");
        }
    });
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetUserTypeWiseChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Doughnut2D.swf", "GetUserTypeWiseChart", "400", "300");
            chart.setDataXML(data);
            chart.render("LoginUsertTypeChart");
        }
    });
});
