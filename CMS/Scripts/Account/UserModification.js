jQuery(function ($) {

    $("#UserSearch").click(function () {
        LoadPopupDynamicaly("/Account/UserList", $('#DivUserSearch'),
            function () {
                LoadSetGridParam($('#UserList'), "/Account/UserListJqGrid")
            });
    });
    var clbPupGrdSel = null;
    function LoadPopupDynamicaly(dynURL, ModalId, loadCalBack, onSelcalbck, width) {

        if (width == undefined) { width = 850; }
        if (ModalId.html().length == 0) {
            $.ajax({
                url: dynURL,
                type: 'GET',
                async: false,
                dataType: 'html', // <-- to expect an html response
                success: function (data) {
                    ModalId.dialog({
                        height: 500,
                        width: width,
                        modal: true,
                        title: 'User Details',
                        buttons: {}
                    });
                    ModalId.html(data);
                }
            });
        }
        clbPupGrdSel = onSelcalbck;
        ModalId.dialog('open');
        CallBackFunction(loadCalBack);
    }
    function LoadSetGridParam(GridId, brUrl) {
        GridId.setGridParam({
            url: brUrl,
            datatype: 'json',
            mtype: 'GET'
        }).trigger("reloadGrid");
    }
    $.getJSON("/Account/FillUserType",
                 function (fillusertype) {
                     var ddlusrtyp = $("#ddlUserType");
                     ddlusrtyp.empty();
                     ddlusrtyp.append($('<option/>',
                    {
                        value: "",
                        text: "Select One"

                    }));

                     $.each(fillusertype, function (index, itemdata) {

                         ddlusrtyp.append($('<option/>',
                             {
                                 value: itemdata.Value,
                                 text: itemdata.Text
                             }));

                     });
                 });
    $('#btnSubmit').click(function () {
        var userid = $("#UserId").val();
        var usertyp = $("#ddlUserType").val();
        var userName = "";
        var sEmail = $('#EmailId').val();


        if ($.trim(userid).length == 0) {
            ErrMsg('UserId is Mandatory.');
            return false;
        }
        if ($.trim(usertyp).length == 0) {
            ErrMsg('User Type is Mandatory.');
            return false;
        }
    });
    $("#btnReset").click(function () {
        window.location.href = '/Account/UserModification';
    });
    //switch element when editing inline
    function aceSwitch(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=checkbox]')
                    .addClass('ace ace-switch ace-switch-5')
                    .after('<span class="lbl"></span>');
        }, 0);
    }
    //enable datepicker
    function pickDate(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=text]')
                        .datepicker({ format: 'yyyy-mm-dd', autoclose: true });
        }, 0);
    }

    function style_edit_form(form) {
        //enable datepicker on "sdate" field and switches for "stock" field
        form.find('input[name=sdate]').datepicker({ format: 'yyyy-mm-dd', autoclose: true })
                .end().find('input[name=stock]')
                    .addClass('ace ace-switch ace-switch-5').after('<span class="lbl"></span>');
        //don't wrap inside a label element, the checkbox value won't be submitted (POST'ed)
        //.addClass('ace ace-switch ace-switch-5').wrap('<label class="inline" />').after('<span class="lbl"></span>');

        //update buttons classes
        var buttons = form.next().find('.EditButton .fm-button');
        buttons.addClass('btn btn-sm').find('[class*="-icon"]').hide(); //ui-icon, s-icon
        buttons.eq(0).addClass('btn-primary').prepend('<i class="ace-icon fa fa-check"></i>');
        buttons.eq(1).prepend('<i class="ace-icon fa fa-times"></i>')

        buttons = form.next().find('.navButton a');
        buttons.find('.ui-icon').hide();
        buttons.eq(0).append('<i class="ace-icon fa fa-chevron-left"></i>');
        buttons.eq(1).append('<i class="ace-icon fa fa-chevron-right"></i>');
    }

    function style_delete_form(form) {
        var buttons = form.next().find('.EditButton .fm-button');
        buttons.addClass('btn btn-sm btn-white btn-round').find('[class*="-icon"]').hide(); //ui-icon, s-icon
        buttons.eq(0).addClass('btn-danger').prepend('<i class="ace-icon fa fa-trash-o"></i>');
        buttons.eq(1).addClass('btn-default').prepend('<i class="ace-icon fa fa-times"></i>')
    }

    function style_search_filters(form) {
        form.find('.delete-rule').val('X');
        form.find('.add-rule').addClass('btn btn-xs btn-primary');
        form.find('.add-group').addClass('btn btn-xs btn-success');
        form.find('.delete-group').addClass('btn btn-xs btn-danger');
    }
    function style_search_form(form) {
        var dialog = form.closest('.ui-jqdialog');
        var buttons = dialog.find('.EditTable')
        buttons.find('.EditButton a[id*="_reset"]').addClass('btn btn-sm btn-info').find('.ui-icon').attr('class', 'ace-icon fa fa-retweet');
        buttons.find('.EditButton a[id*="_query"]').addClass('btn btn-sm btn-inverse').find('.ui-icon').attr('class', 'ace-icon fa fa-comment-o');
        buttons.find('.EditButton a[id*="_search"]').addClass('btn btn-sm btn-purple').find('.ui-icon').attr('class', 'ace-icon fa fa-search');
    }

    function beforeDeleteCallback(e) {
        var form = $(e[0]);
        if (form.data('styled')) return false;

        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
        style_delete_form(form);

        form.data('styled', true);
    }

    function beforeEditCallback(e) {
        var form = $(e[0]);
        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
        style_edit_form(form);
    }
    //it causes some flicker when reloading or navigating grid
    //it may be possible to have some custom formatter to do this as the grid is being created to prevent this
    //or go back to default browser checkbox styles for the grid
    function styleCheckbox(table) {
    }
    function updateActionIcons(table) {
    }

    //replace icons with FontAwesome icons like above
    function updatePagerIcons(table) {
        var replacement =
            {
                'ui-icon-seek-first': 'ace-icon fa fa-angle-double-left bigger-140',
                'ui-icon-seek-prev': 'ace-icon fa fa-angle-left bigger-140',
                'ui-icon-seek-next': 'ace-icon fa fa-angle-right bigger-140',
                'ui-icon-seek-end': 'ace-icon fa fa-angle-double-right bigger-140'
            };
        $('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
            var icon = $(this);
            var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

            if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
        })
    }
    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});