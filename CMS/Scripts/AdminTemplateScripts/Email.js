jQuery(function ($) {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/CampusWiseEmailCountChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Line.swf", "EmailCampusWiseEmailCountChart", "450", "200");
            chart.setDataXML(data);
            chart.render("EmailCampusChart");
        }
    });
});