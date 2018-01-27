$(function () {

    $('#btntitledelete').hide();
    $('#evntDiscription').hide();
    $('#btneventListAdd').hide();

    $('#btntitleAdd').click(function () {
        $.ajax({
            url: '/StaffManagement/AddEvent?title=' + $('#title').val() + '&userId=' + $('#userId').val() + '&eventFor=Staff',
            type: "POST",
            async: false,
            datatype: 'json',
            success: function (result) {
                if (result != 0) {
                    $('#Id').val(result);
                    $('#title').attr("disabled", true);
                    $('#btntitleAdd').hide();
                    $('#btntitledelete').show();
                    $('#evntDiscription').show();
                    $('#btneventListAdd').show();
                }
            }
        });
    });

    $('#btneventListAdd').click(function () {
        $.ajax({
            url: '/StaffManagement/AddEventList?title=' + $('#title').val() + '&userId=' + $('#userId').val() + '&eventFor=Staff',
            type: "POST",
            async: false,
            datatype: 'json',
            success: function (result) {
                if (result != 0) {
                    $('#Id').val(result);
                    $('#title').attr("disabled", true);
                    $('#btntitleAdd').hide();
                    $('#btntitledelete').show();
                    $('#evntDiscription').show();
                    $('#btneventListAdd').show();
                }
            }
        });
    });
});

function GeteventList() {
    $.ajax({
        type: 'Get',
        url: '/StaffManagement/GetEventList?EventId=' + $('#Id').val(),
        success: function (data) {
            $('#checkList').prepend(data);
        },
        async: false,
        dataType: "text"
    });
}