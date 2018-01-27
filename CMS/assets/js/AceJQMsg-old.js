(function ($) {
    //$.preventDefault();
    $.msgs = {
        okBtn: ' OK ',
        okCalBck: null,
        dlgAlrt: null,
        icnSpan: '<span class="ui-icon" style="float:left; margin:0 7px 50px 0em;"></span>',
        btnCls: null,
        errIcn: "",
        infoIcn: "",
        sucsIcn: "",
        info: function (message, calBck, title) {
            if (title == null) title = "<div class='widget-header' style='align:left; color:#8a6d3b;'><h4 class='smaller warning'><i class='ace-icon fa	fa-info-circle'></i> Information</h4></div>";
            this.show(message, calBck, title);
            this.dlgAlrt.find('div').removeClass('ui-state-highlight').addClass('ui-icon-check').addClass('alert-warning');
            this.dlgAlrt.find('span').addClass(this.infoIcn);
            this.btnCls = 'btn-warning';
        },
        error: function (message, calBck, title) {
            if (title == null) title = "<div class='widget-header' style='align:left; color:#a94442;'><h4 class='smaller Danger'><i class='ace-icon fa fa-exclamation-triangle'></i> Error</h4></div>";
            this.show(message, calBck, title);
            this.dlgAlrt.find('div').removeClass('ui-state-highlight').addClass('ui-state-error').addClass('alert-danger');
            this.dlgAlrt.find('span').addClass(this.errIcn);
            this.btnCls = 'btn-danger';
        },
        success: function (message, calBck, title) {
            if (title == null) title = "<div class='widget-header' style='align:left; color:green;'><h4 class='smaller Success'><i class='ace-icon fa fa-check-square-o'></i> Success</h4></div>";
            this.show(message, calBck, title);
            this.dlgAlrt.find('div').removeClass('ui-state-highlight').addClass('ui-icon-check').addClass('alert-success');
            this.dlgAlrt.find('span').addClass(this.sucsIcn);
            this.btnCls = 'btn-success';
        },
        _init: function (dtitle) {
            this.dlgAlrt = $("<div><div class='alert bigger-110'><p id='alrtMsg'></p></div></div>");
            $("BODY").append(this.dlgAlrt);

            this.dlgAlrt.dialog({
                modal: true,
                title: dtitle,
                widht: 'auto',
                resizable: false,
                close: function (event, ui) { $.msgs._clbf(); },
                buttons:
                    [{
                        html: "<i class='ace-icon fa fa-times bigger-110'></i>&nbsp; OK ",
                        //text: okBtn,
                        "class": "btn btn-primary btn-xs",
                        click: function () {
                            $(this).dialog("close"); $.msgs._clbf();
                        }
                    }],
                open: function (event, ui) {
                    $(".ui-icon-closethick").hide();

                    //$(this.buttons).removeClass('btn-primary').addClass('btn-success');
                }
            });

        },
        _clbf: function () {
            var clbf = $.msgs.okCalBck; if (clbf != undefined && clbf) { clbf(); }
        },
        show: function (message, calBck, title) {
            if (calBck) { this.okCalBck = calBck; }

            if (!this.dlgAlrt) {
                this._init(title);
            }
            else {
                this.dlgAlrt.dialog("option", "title", title);
                this.dlgAlrt.dialog("open");
            }
            $("#alrtMsg").html($.msgs.icnSpan + message);
        }
    };

    // Shortuct functions
    InfoMsg = function (message, callback, title) { $.msgs.info(message, callback, title); };

    ErrMsg = function (message, callback, title) { $.msgs.error(message, callback, title); };

    SucessMsg = function (message, callback, title) { $.msgs.success(message, callback, title); };

    MsgcalBack = function (clb) { $.msgs.okCalBck = clb; };



})(jQuery);
