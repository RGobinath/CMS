
$(document).ready(function () {
    RenderCalendar("", "");
    $('#btnSearch').click(function () {
        var Campus = $("#ddlcampus").val();
        var Hall = $("#ddlAsset").val();
        $('#calendar').fullCalendar('destroy');
        RenderCalendar(Campus, Hall);
    });
    $('#btnReset').click(function () { $("input[type=text], textarea, select").val(""); $('#calendar').fullCalendar('refetchEvents'); });
    $("#ddlcampus").change(function () {
        getAssetdll($("#ddlcampus").val(), '#ddlAsset', "");
    });
    $("#campusddl").change(function () {
        getAssetdll($("#campusddl").val(), '#Assetddl', "");
    });
    $("#srhCampusddl").change(function () {
        getAssetdll($("#srhCampusddl").val(), '#srhAssetddl', "");
    });
    
});
function RenderCalendar(Campus, Hall) {
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        height: 350,
        defaultView: 'month',
        editable: false,
        allDaySlot: true,
        selectable: true,
        slotMinutes: 15,
        events: {
            url: '/Asset/GetEventList',
            //cache: true,
            type: 'POST',
            data: { Campus: Campus, Hall: Hall }
        },
        eventClick: function (date, jsEvent, view) {
            debugger;
            if (view.name == 'month' || view.name == 'basicWeek') {
                $('#calendar').fullCalendar('changeView', 'agendaDay');
                $('#calendar').fullCalendar('gotoDate',  date.start);
            }
        },
        //dayClick: function(date, jsEvent, view) {},
        dayClick: function (date, jsEvent, view) {
            if (view.name == 'month' || view.name == 'basicWeek') {
                $('#calendar').fullCalendar('changeView', 'agendaDay');
                $('#calendar').fullCalendar('gotoDate', date);
            }
        },

    });
}
function getAssetdll(campus, AssetdllId, selhall) {
    $.getJSON("/Asset/GetAssetByCampus?Campus=" + campus,
          function (fillig) {
              $(AssetdllId).empty();
              $(AssetdllId).append($('<option/>', {
                  value: "", text: "Select One"
              }));
              $.each(fillig, function (index, itemdata) {
                  if (itemdata.Value == selhall) {
                      $(AssetdllId).append($('<option/>', { value: itemdata.Value, text: itemdata.Text, selected: "Selected" }));
                  }
                  else {
                      $(AssetdllId).append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                  }
              });
          });
}
