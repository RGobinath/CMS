jQuery(function ($) {
    TransportCharts();
    $('#btnSearch').click(function () {
        TransportCharts();
    });
    $('#btnReset').click(function () {
        $("input[type=text], textarea, select").val("");
        TransportCharts();
    });
    function TransportCharts() {
        var Campus = $('#ddlcampus').val();
        var Grade = $('#ddlgrade').val();
        var Section = $('#ddlsection').val();

        $.ajax({
            type: 'Get',
            async: false,
            url: '/Transport/StudentTransportRequiredCommonCountPieChart?Campus=' + Campus + '&Grade=' + Grade + '&Section=' + Section,
            success: function (data) {
                var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Pie2D.swf", "StudentTransportRequiredCommonCountChart", "400", "250");
                chart.setDataXML(data);
                chart.render("TransportRequiredPieChart");
            }
        });
        $.ajax({
            type: 'Get',
            async: false,
            url: '/Transport/StudentTransportRequiredGenderCount?Campus=' + Campus + '&Grade=' + Grade + '&Section=' + Section,
            success: function (data) {
                var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_StackedColumn2D.swf", "StudentTransportRequiredGenderCountChart", "200", "300");
                chart.setDataXML(data);
                chart.render("TransportRequiredGenderChart");
            }
        });
        $.ajax({
            type: 'Get',
            async: false,
            url: '/Transport/StudentTransportAllocationCommonCountChart?Campus=' + Campus + '&Grade=' + Grade + '&Section=' + Section,
            success: function (data) {
                var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Doughnut2D.swf", "StudentRouteAllocationCountChart", "400", "250");
                chart.setDataXML(data);
                chart.render("TransportAllocationCommonCountChart");
            }
        });
        $.ajax({
            type: 'Get',
            async: false,
            url: '/Transport/StudentTransportAllocationGenderCountChart?Campus=' + Campus + '&Grade=' + Grade + '&Section=' + Section,
            success: function (data) {
                var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_MSColumn2DLineDY.swf", "StudentRouteAllocationGenderCountChart", "200", "250");
                chart.setDataXML(data);
                chart.render("TransportAllocationGenderCountChart");
            }
        });
        $.ajax({
            type: 'Get',
            async: false,
            url: '/Transport/RouteStudentReportCounts?Campus=' + Campus,
            success: function (result) {
                var resultLenght = Object.keys(result).length;
                var TransportRequired = 0;
                var TransportNotRequired = 0;
                var RouteAllocated = 0;
                var RouteNotAllocated = 0;
                var TotalStudents = 0;

                if (resultLenght > 0) {
                    debugger;
                    for (var i = 0; i < resultLenght; i++) {
                        TransportRequired = TransportRequired + result[i].TransportRequired;
                        TransportNotRequired = TransportNotRequired + result[i].TransportNotRequired;
                        RouteAllocated = RouteAllocated + result[i].RouteAllocated;
                        RouteNotAllocated = RouteNotAllocated + result[i].RouteNotAllocated;
                    }
                }
                TotalStudents = TransportRequired + TransportNotRequired;
                $('#TransportRequired').text(TransportRequired);
                $('#TransportNotRequired').text(TransportNotRequired);
                $('#RouteAllocated').text(RouteAllocated);
                $('#RouteNotAllocated').text(RouteNotAllocated);
                $('#TotalStudents').text(TotalStudents);

            }
        });
    }
});