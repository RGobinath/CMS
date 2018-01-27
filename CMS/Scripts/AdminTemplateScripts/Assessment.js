jQuery(function ($) {
    GetAssessmentRankChart();
    GetAssessmentRankList();
    GetAssessmentRanksCount();

    $("#AssessDdlCampusList").change(function () {
        gradeddl();
    });

    $("#AssessDdlgrade").change(function () {
        GetAssessmentRankList();
        GetAssessmentRankChart();
        GetAssessmentRanksCount();
    });
    $.getJSON("/AdminTemplate/getCampusList",
function (getCampus) {
    var ddlCampusList = $("#AssessDdlCampusList");
    ddlCampusList.empty();
    ddlCampusList.append($('<option/>',
{
    value: "", text: "All Campus"

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
function gradeddl() {
    var e = document.getElementById('AssessDdlCampusList');
    var campus = e.options[e.selectedIndex].value;
    $.getJSON("/Admission/CampusGradeddl/", { Campus: campus },
        function (modelData) {
            var select = $("#AssessDdlgrade");
            select.empty();
            select.append($('<option/>'
                           , {
                               value: "",
                               text: "Select Grade"
                           }));
            $.each(modelData, function (index, itemData) {
                select.append($('<option/>',
                              {
                                  value: itemData.gradcod,
                                  text: itemData.gradcod
                              }));
            });
        });
}

function GetAssessmentRankChart() {
    var Campus = $('#AssessDdlCampusList').val();
    var Grade = $('#AssessDdlgrade').val();
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetAssessmentRankChart?Campus=' + Campus + '&Grade=' + Grade,
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Area2D.swf", "GetAssessmentRankCharts", "400", "200");
            chart.setDataXML(data);
            chart.render("AssessRankChart");
        }
    });
}
function GetAssessmentRankList() {
    var Campus = $('#AssessDdlCampusList').val();
    var Grade = $('#AssessDdlgrade').val();
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetAssessmentRankList?Campus=' + Campus + '&Grade=' + Grade,
        success: function (data) {
            if (data[0] != null) {
                var FirstRankCylinder = "<chart caption='First Rank' manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='20' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#8A2BE2' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                FirstRankCylinder += "<value>" + data[0].Mark + "</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                FirstRankCylinder += "</annotationGroup></annotations></chart>";

                var FirstRankChart = new FusionCharts("../../Charts/Cylinder.swf", "Assess360GetAssessmentRankList1", "100", "150");
                FirstRankChart.setDataXML(FirstRankCylinder);
                FirstRankChart.render("AssessFirstRank");
            }
            else {
                var FirstRankCylinder = "<chart caption='First Rank' manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='20' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#8A2BE2' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                FirstRankCylinder += "<value>0</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                FirstRankCylinder += "</annotationGroup></annotations></chart>";

                var FirstRankChart = new FusionCharts("../../Charts/Cylinder.swf", "Assess360GetAssessmentRankList2", "100", "150");
                FirstRankChart.setDataXML(FirstRankCylinder);
                FirstRankChart.render("AssessFirstRank");
            }

            if (data[1] != null) {
                var SecondRankCylinder = "<chart caption='First Rank' manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='20' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#6baa01' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                SecondRankCylinder += "<value>" + data[1].Mark + "</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                SecondRankCylinder += "</annotationGroup></annotations></chart>";

                var SecondRankChart = new FusionCharts("../../Charts/Cylinder.swf", "Assess360GetAssessmentRankList3", "100", "150");
                SecondRankChart.setDataXML(SecondRankCylinder);
                SecondRankChart.render("AssessSecondRank");
            }
            else {
                var SecondRankCylinder = "<chart caption='First Rank' manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='20' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#6baa01' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                SecondRankCylinder += "<value>0</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                SecondRankCylinder += "</annotationGroup></annotations></chart>";

                var SecondRankChart = new FusionCharts("../../Charts/Cylinder.swf", "Assess360GetAssessmentRankList4", "100", "150");
                SecondRankChart.setDataXML(SecondRankCylinder);
                SecondRankChart.render("AssessSecondRank");
            }

            if (data[2] != null) {
                var ThirdRankCylinder = "<chart caption='First Rank' manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='20' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#f8bd19' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                ThirdRankCylinder += "<value>" + data[2].Mark + "</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                ThirdRankCylinder += "</annotationGroup></annotations></chart>";

                var ThirdRankChart = new FusionCharts("../../Charts/Cylinder.swf", "Assess360GetAssessmentRankList5", "100", "150");
                ThirdRankChart.setDataXML(ThirdRankCylinder);
                ThirdRankChart.render("AssessThirdRank");
            }
            else {
                var ThirdRankCylinder = "<chart caption='First Rank' manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='20' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#f8bd19' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                ThirdRankCylinder += "<value>0</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                ThirdRankCylinder += "</annotationGroup></annotations></chart>";

                var ThirdRankChart = new FusionCharts("../../Charts/Cylinder.swf", "Assess360GetAssessmentRankList6", "100", "150");
                ThirdRankChart.setDataXML(ThirdRankCylinder);
                ThirdRankChart.render("AssessThirdRank");
            }
        }
    });
}
function GetAssessmentCountChart() {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetAssessmentCountChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Line.swf", "Example", "400", "200");
            chart.setDataXML(data);
            chart.render("AssessCountChart");
        }
    });
}

function GetAssessmentRanksCount() {
    var Campus = $('#AssessDdlCampusList').val();
    var Grade = $('#AssessDdlgrade').val();
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/GetAssessmentRanksCount?Campus=' + Campus + '&Grade=' + Grade,
        success: function (data) {
            $('#Below75Count').text(data.Below75Count);
            $('#MeritListCount').text(data.MeritListCount);
            $('#HightAcieversClubCount').text(data.HightAcieversClubCount);
            $('#ChairmanAwardCount').text(data.ChairmanAwardCount);
        }
    });
}