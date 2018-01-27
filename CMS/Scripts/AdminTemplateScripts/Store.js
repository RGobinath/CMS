jQuery(function ($) {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/StoreCampusWiseRequestChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Column2D.swf", "StoreCampusWiseRequestCharts", "300", "250");
            chart.setDataXML(data);
            chart.render("StoreCampusWiseRequestChart");
        }
    });
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/StoreInwardChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Line.swf", "StoreInwardCharts", "300", "250");
            chart.setDataXML(data);
            chart.render("StoreInwardChart");
        }
    });
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/StoreMaterialsMasterChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Pie2D.swf", "StoreMaterialsMasterChart", "400", "250");
            chart.setDataXML(data);
            chart.render("MaterialsMasterChart");
        }
    });
});