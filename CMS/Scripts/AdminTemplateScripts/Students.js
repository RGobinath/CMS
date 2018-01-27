jQuery(function ($) {
    $('#CampusGradeSectionChart').hide();
    $('#CampusForAllGradeChart').hide();
    $('#AllCampusAndGradeChart').show();

    GetAllCampusSudentsChart();
    GetAllGradeSudentsChart();
    StudGradeDdl();

    $('#StudDdlCampusList').change(function () {
        StudGradeDdl();
    });

    $.getJSON("/AdminTemplate/getCampusList",
function (getCampus) {
    var ddlCampusList = $("#StudDdlCampusList");
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
function StudGradeDdl() {
    var Campus = $('#StudDdlCampusList').val();
    $.getJSON("/Admission/CampusGradeddl/", { Campus: Campus },
        function (modelData) {
            var ddlGradeList = $("#StudddlGradeList");
            ddlGradeList.empty();
            ddlGradeList.append($('<option/>',
        {
            value: "All", text: "All Grade"

        }));
            $.each(modelData, function (index, itemdata) {
                ddlGradeList.append($('<option/>',
                    {
                        value: itemdata.gradcod,
                        text: itemdata.gradcod
                    }));
            });
        });
}

function GetAllCampusSudentsChart() {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetAllCampusSudentsChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Line.swf", "GetAllCampusSudentsCharts", "420", "330");
            chart.setDataXML(data);
            chart.render("StudCampusChart");
        }
    });
}
function GetAllGradeSudentsChart() {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetAllGradeSudentsChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Line.swf", "GetAllGradeSudentsCharts", "420", "330");
            chart.setDataXML(data);
            chart.render("StudGradeChart");
        }
    });
}
function GetAcademicYearWiseStudentsCountChart() {
    var Campus = $('#StudDdlCampusList').val();
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetAcademicYearWiseStudentsCount?Campus=' + Campus,
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Column2D.swf", "GetAcademicYearWiseStudentsCount", "340", "230");
            chart.setDataXML(data);
            chart.render("AcademicYearChart");
        }
    });
}
function GetStudentsChartByCampusForAllGrade() {
    var Campus = $('#StudDdlCampusList').val();
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetStudentsChartByCampusForAllGrade?Campus=' + Campus,
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_MSColumn2DLineDY.swf", "GetStudentsChartByCampusForAllGrade", "800", "330");
            chart.setDataXML(data);
            chart.render("StudCampusChartForAllGrade");
        }
    });
}
function GetStudentsChartByCampusAndGrade() {
    var Campus = $('#StudDdlCampusList').val();
    var Grade = $('#StudddlGradeList').val();
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetStudentsChartByCampusAndGrade?Campus=' + Campus + '&Grade=' + Grade,
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_MSColumn2DLineDY.swf", "GetStudentsChartByCampusAndGrade", "800", "330");
            chart.setDataXML(data);
            chart.render("StudCampusAndGradeAndSectionChart");
        }
    });
}
function GetChart() {
    var Campus = $('#StudDdlCampusList').val();
    var Grade = $('#StudddlGradeList').val();
    if (Campus != "All" && Grade == "All") {
        $('#AllCampusAndGradeChart').hide();
        $('#CampusGradeSectionChart').hide();
        $('#CampusForAllGradeChart').show();
        GetStudentsChartByCampusForAllGrade();
    }
    else if (Campus != "All" && Grade != "All") {
        GetStudentsChartByCampusAndGrade();
        $('#AllCampusAndGradeChart').hide();
        $('#CampusForAllGradeChart').hide();
        $('#CampusGradeSectionChart').show();
    }
    else {
        $('#CampusForAllGradeChart').hide();
        $('#CampusGradeSectionChart').hide();
        $('#AllCampusAndGradeChart').show();
    }
}