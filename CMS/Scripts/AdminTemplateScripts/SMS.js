jQuery(function ($) {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/CampusWiseSMSCountChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Line.swf", "CampusWiseSMSCountChart", "450", "200");
            chart.setDataXML(data);
            chart.render("SMSCampusChart");
        }
    });
});