$(function () {

    debugger;
    //$('#file2').ace_file_input();
    new nicEditor({ maxHeight: 150, fullPanel: true }).panelInstance('txtMessage');
    //Validation Date Picker
    var startDate = new Date();
    var FromEndDate = new Date();
    var ToEndDate = new Date();

    ToEndDate.setDate(ToEndDate.getDate() + 365);
    $('.txtPublishDate').datepicker({
        format: "dd-mm-yyyy",
        weekStart: 1,
        startDate: startDate,
        autoclose: true
    }).on('changeDate', function (selected) {
        startDate = new Date(selected.date.valueOf());
        startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
        $('.txtExpireDate').datepicker('setStartDate', startDate);
    });
    $('.txtExpireDate').datepicker({
        format: "dd-mm-yyyy",
        weekStart: 1,
        startDate: startDate,
        endDate: ToEndDate,
        autoclose: true
    }).on('changeDate', function (selected) {
        FromEndDate = new Date(selected.date.valueOf());
        FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
        $('.txtPublishDate').datepicker('setEndDate', FromEndDate);
    });

});

var splitstr = "";

$("#file2").click(function () {
    //var ddlBranchCode = $("#ddlBranchCode").val();

    debugger;
    var txtTopic = $("#txtTopic").val();
    var txtPublishDate = $("#txtPublishDate").val();
    var txtExpireDate = $("#txtExpireDate").val();
    var ddlNoteType = $("#ddlNoteType").val();
    var ddlPublishTo = $("#ddlPublishTo").val();

    if (txtTopic == "" || txtTopic == null || txtPublishDate == "" || txtPublishDate == null || txtExpireDate == "" || txtExpireDate == null || ddlNoteType == "" || ddlNoteType == null || ddlPublishTo == "" || ddlPublishTo == null) {
        ErrMsg('Please Fill Required Notification Details.');
        return false;
    }
});

$("#Submit").click(function () {

    var txtTopic = $("#txtTopic").val();
    var txtPublishDate = $("#txtPublishDate").val();
    var txtExpireDate = $("#txtExpireDate").val();
    var ddlNoteType = $("#ddlNoteType").val();
    var ddlPublishTo = $("#ddlPublishTo").val();
    if (txtTopic == "" || txtTopic == null || txtPublishDate == "" || txtPublishDate == null || txtExpireDate == "" || txtExpireDate == null || ddlNoteType == "" || ddlNoteType == null || ddlPublishTo == "" || ddlPublishTo == null) {
        ErrMsg("Please Fill Required Notification Details.");
        return false;
    }
});
var e = $.Event("keydown", {
    keyCode: 27
});

$('#cancel').click(function () {
    $(".ui-dialog").trigger(e);
});

function resethtml2() {
    // $('#file2').val() = "";

    //alert('here');
    $('#clear2').html($('#clear2').html());
    var div = document.getElementById('Attachfiles2');
    div.innerHTML = 'Attached Files &nbsp;&nbsp;&nbsp;  ';
    $.ajax({
        url: '/Notify/DeleteAttachment/',
        type: 'POST',
        dataType: 'json',
        traditional: true
    });
}

function uploaddoc2() {
    debugger;
    if (document.getElementById("file2").value == "") {
        ErrMsg("Please Browse a Document");
    }
    else {
        splitstr = splitstr + $('#file2').val().split('\\');


        $.ajaxFileUpload({

            url: '/Notify/MailAttachments2?NotePreId=' + $('#NotePreId').val(),
            secureuri: false,
            fileElementId: 'file2',
            dataType: 'json',
            success: function (data, success) {
                var div = document.getElementById('Attachfiles2');

                if ((div.innerHTML == 'Attached Files &nbsp;&nbsp;&nbsp;  ')) {
                    div.innerHTML = div.innerHTML + data.result;
                }
                else {
                    div.innerHTML = div.innerHTML + ', ' + data.result;
                }
            }
        });
        $('#clear2').html($('#clear2').html());

    }
}

function textLimit(field, maxlen) {
    if (field.value.length > maxlen + 1)
        alert('your input has been truncated!');
    if (field.value.length > maxlen)
        field.value = field.value.substring(0, maxlen);
} 