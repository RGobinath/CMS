jQuery(function ($) {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetUserTypeChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Column2D.swf", "GetUserTypeChart", "400", "300");
            chart.setDataXML(data);
            chart.render("UserTypeChart");
        }
    }); 

    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetUsersCampusWiseChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Line.swf", "GetUsersCampusWiseChart", "400", "300");
            chart.setDataXML(data);
            chart.render("UsersCampusWiseChart");
        }
    });
});
