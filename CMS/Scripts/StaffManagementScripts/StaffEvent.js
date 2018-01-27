$(function () {
    //var dialog;
    //dialog = $("#dialog-form").dialog({
    //    autoOpen: false,
    //    height: 200,
    //    width: 500,
    //    modal: false
    //});
    
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
        select: function (start, end, allDay) {
            debugger;
            ModifiedLoadPopupDynamicaly("/StaffManagement/CreateEvent", $('#eventId'), function () { }, "", 450, 250, "Create Event");
            //dialog.dialog("open");
        },
        events: {
            //url: '/Asset/GetEventList',
            ////cache: true,
            //type: 'POST',
            //data: { Campus: Campus, Hall: Hall }
        },
        eventClick: function (date, jsEvent, view) {
            if (view.name == 'month' || view.name == 'basicWeek') {
                $('#calendar').fullCalendar('changeView', 'agendaDay');
                $('#calendar').fullCalendar('gotoDate', date.start);
            }
        },
        //dayClick: function(date, jsEvent, view) {},
        //dayClick: function (date, jsEvent, view) {
        //    if (view.name == 'month' || view.name == 'basicWeek') {
        //        $('#calendar').fullCalendar('changeView', 'agendaDay');
        //        $('#calendar').fullCalendar('gotoDate', date);
        //    }
        //},

    });
    $('#btntitledelete').hide();
    $('#btnAdd').click(function () {
        $.ajax({
            url: '/StaffManagement/AddEvent?subject=' + subject,
            type: "POST",
            success: function (result) {
                if (result == true) {

                }
            }
        });
    });
});