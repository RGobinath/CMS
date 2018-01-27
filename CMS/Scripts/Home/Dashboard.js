$(function () {

    $.getJSON("/Base/FillBranchCode",
     function (fillcampus) {
         var ddlcam = $("#ddlCampus");
         ddlcam.empty();
         ddlcam.append($('<option/>',
        {
            value: "",
            text: "Select One"

        }));

         $.each(fillcampus, function (index, itemdata) {
             ddlcam.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
         });
     });


    var cam = $("#ddlCampus").val();
    call(cam);

    $("#ddlCampus").change(function () {
        var cam = $("#ddlCampus").val();
        call(cam);
    });
    function call(cam) {

        $.getJSON('/Home/NonCompletedSlaStatusChart?cam=' + cam,
            function (result1) {
                var below24 = 0;
                var bet24to48 = 0;
                var slaBreached = 0;
                var i = 0;

                $.each(result1, function (index, itemdata) {

                    if (itemdata.Hours <= 24) {
                        below24++;
                    }
                    else if (itemdata.Hours > 24 && itemdata.Hours <= 48) {
                        bet24to48++;
                    }
                    else if (itemdata.Hours > 48) {
                        slaBreached++;
                    }
                    else { }
                    i++;

                });
                if (below24 == 0) {
                    below24 = "";
                }
                if (bet24to48 == 0) {
                    bet24to48 = "";
                }
                if (slaBreached == 0) {
                    slaBreached = "";
                }
                var ColumnChart = "<graph caption='' xAxisName='Hours' yAxisName='Issue Count' decimalPrecision='0' formatNumberScale='0' rotateNames='1' >";
                ColumnChart = ColumnChart + " <set name='Below 24' value='" + below24 + "' color='AFD8F8' link='/Home/IssueList?NonCompletedSLA=below24&amp;cam=" + cam + "'/>";
                ColumnChart = ColumnChart + " <set name='24-48' value='" + bet24to48 + "' color='F6BD0F' link='/Home/IssueList?NonCompletedSLA=24-48&amp;cam=" + cam + "'/>";
                ColumnChart = ColumnChart + " <set name='SLA Breached' value='" + slaBreached + "' color='8BBA00' link='/Home/IssueList?NonCompletedSLA=breached&amp;cam=" + cam + "'/></graph>";
                var chart = new FusionCharts("../../Charts/FCF_Column3D.swf", "productSales", "250", "230");
                chart.setDataXML(ColumnChart);
                chart.render("ColumnChartDiv");
            });



        $.ajax({
            type: 'Get',
            url: '/Home/GetIssueCountByIssueGroupNew',
            data: { cam: cam },
            success: function (data) {
                var chart = new FusionCharts("../../Charts/FCF_Column3D.swf", "productSales", "350", "230");
                chart.setDataXML(data);
                chart.render("FunnelChartDiv");
                if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                    processBusy.dialog('close');
                }
            },

            async: false,
            dataType: "text"
        });



        $.ajax({
            type: 'Get',
            url: '/Home/WeeklyIssueStatus',
            data: { cam: cam },
            success: function (data) {
                var chart = new FusionCharts("../../Charts/FCF_MSLine.swf", "ChartId", "280", "230");
                chart.setDataXML(data);
                chart.render("WeeklyIssueStatus1Div");
                if ($("#processDiv").closest('.ui-dialog').is(':visible')) {
                    processBusy.dialog('close');
                }
            },

            async: false,
            dataType: "text"
        });

    }

});