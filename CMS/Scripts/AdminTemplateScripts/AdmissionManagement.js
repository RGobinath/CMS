jQuery(function ($) {
    var Campus = $('#AdmsnddlCampusList').val();
    GetAcademicYearWiseReportChart();
    GetAdmissionStatusWiseReport();
    GetBoardingTypeCount();
    //GetAdmissionBoardingTypeReport();


    $('#AdmsnddlCampusList').change(function () {
        GetAcademicYearWiseReportChart();
        GetAdmissionStatusWiseReport();
        GetBoardingTypeCount();

    });


    $.getJSON("/AdminTemplate/getCampusList",
function (getCampus) {
    var ddlCampusList = $("#AdmsnddlCampusList");
    ddlCampusList.empty();
    ddlCampusList.append($('<option/>',
{
    value: "All", text: "All"

}));
    $.each(getCampus, function (index, itemdata) {
        ddlCampusList.append($('<option/>',
            {
                value: itemdata.Value,
                text: itemdata.Text
            }));
    });
    });
    function GetAcademicYearWiseReportChart() {
        var Campus = $('#AdmsnddlCampusList').val();
        $.ajax({
            type: 'Get',
            async: false,
            url: '/AdminTemplate/GetAcademicYearWiseReportChart?Campus=' + Campus,
            success: function (data) {
                var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_MSColumn2D.swf", "GetAcademicYearWiseReportChart", "470", "250");
                chart.setDataXML(data);
                chart.render("AdmsnMultiSeriesChart");
            }
        });
    }

    function GetAdmissionStatusWiseReport() {
        var Campus = $('#AdmsnddlCampusList').val();
        $.ajax({
            type: 'Get',
            async: false,
            url: '/AdminTemplate/GetAdmissionStatusWiseReport?Campus=' + Campus,
            success: function (data) {
                var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Line.swf", "GetAdmissionStatusWiseReport", "470", "250");
                chart.setDataXML(data);
                chart.render("AdmsnCampusChart");
            }
        });
    }

    function GetAdmissionBoardingTypeReport() {
        var Campus = $('#AdmsnddlCampusList').val();
        $.ajax({
            type: 'Get',
            async: false,
            url: '/AdminTemplate/GetAdmissionBoardingTypeReport?Campus=' + Campus,
            success: function (data) {
                var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Area2D.swf", "GetAdmissionBoardingTypeReport", "470", "250");
                chart.setDataXML(data);
                chart.render("AdmsnBoardingTypeChart");
            }
        });
    }
    function GetBoardingTypeCount() {
        var Campus = $('#AdmsnddlCampusList').val();
        $.ajax({
            type: 'Get',
            async: false,
            url: '/AdminTemplate/GetBoardingTypeCount?Campus=' + Campus,
            success: function (data) {
                $('#ResidentialCount').text(data.ResidentialCount);
                $('#DayScholorCount').text(data.DayScholorCount);
                $('#WeekBoarderCount').text(data.WeekBoarderCount);
            }
        });
    }
});