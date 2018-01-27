jQuery(function ($) {
    StaffDeptWiseCharts();
    $('#DeptWiseReport').hide();
    $('#CampusWiseReport').show();


    $('#ddlCampusList').change(function () {
        StaffDeptWiseCharts();
    });

    $.getJSON("/AdminTemplate/getCampusList",
 function (getCampus) {
     var ddlCampusList = $("#ddlCampusList");
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
function StaffManagementCampusWiseChart() {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/StaffManagementCampusWiseChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Area2D.swf", "StaffManagementCampusWiseChart", "360", "250");
            chart.setDataXML(data);
            chart.render("StaffCampusChart");
        }
    });
}

function StaffManagementStatusWiseChart() {
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/StaffManagementStatusWiseChart',
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Line.swf", "StaffManagementStatusWiseChart", "400", "250");
            chart.setDataXML(data);
            chart.render("StaffStatusChart");
        }
    });
}

function StaffDeptWiseCharts() {
    var Campus = $('#ddlCampusList').val();
    $.ajax({
        type: 'Get',
        async: false,
        url: '/AdminTemplate/StaffDeptWiseChart?Campus=' + Campus,
        success: function (data) {
            var chart = new FusionCharts("../../Content/AdminTemplate/Content/FusionCharts/Charts/FCF_Line.swf", "StaffsDeptWiseChart", "900", "350");
            chart.setDataXML(data);
            chart.render("StaffDeptWiseChart");
        }
    });
}

function GetDeptWiseReport() {
    $('#CampusWiseReport').hide();
    $('#DeptWiseReport').show();
}
function GetCampusWiseReport() {
    $('#CampusWiseReport').show();
    $('#DeptWiseReport').hide();
}