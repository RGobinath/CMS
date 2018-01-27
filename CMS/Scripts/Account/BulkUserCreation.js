jQuery(function ($) {
    $("#btnUpload").click(function () {
        ajaxUploadDocs()
    });
    function ajaxUploadDocs() {
        //var DocTypeText = $('#doctype option:selected').text();
        $.ajaxFileUpload({
            url: 'BulkUserCreation',
            secureuri: false,
            fileElementId: 'uploadedFile',
            dataType: 'json',
            success: function (data, status) {
                //$('#OrdersList').trigger("reloadGrid");
                $('#uploadedFile').val('');
                if (typeof data.result != 'undefined' && data.result != '') {
                    if (typeof data.success != 'undefined' && data.success == true) {
                        InfoMsg(data.result);
                    } else {
                        ErrMsg(data.result);
                    }
                }
            },
            error: function (data, status, e) {
            }
        });
    }
});