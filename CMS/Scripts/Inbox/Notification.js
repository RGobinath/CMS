
    $(document).ready(function () {
        $.ajax({
            url: '@Url.Content("~/Account/Feedback")',
            mtype: 'GET',
            async: false,
            datatype: 'json',
            success: function (data) {

                $('#DescriptionofInbox').html(data);
            }
        });

    });