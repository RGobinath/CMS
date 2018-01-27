jQuery(function ($) {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/CallmanagementIssueCountbyIssueGroupChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Area2D.swf", "CallmanagementIssueCountbyIssueGroupChart", "550", "270");
            chart.setDataXML(data);
            chart.render("CallMgmntIssueCountChart");
        }
    });
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/CallmanagementIssueCountbyIssueStatusChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Funnel.swf", "CallmanagementIssueCountbyIssueStatusChart", "300", "250");
            chart.setDataXML(data);
            chart.render("CallMgntIssueStatusChart");
        }
    });
});