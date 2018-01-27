jQuery(function ($) {
    GetCampus();
    var grid_selector = "#GPSTrackingDeviceMasterJqGrid";
    var pager_selector = "#GPSTrackingDeviceMasterJqGridPager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    $('#txtPurchaseFromDate').datepicker({
        format: 'dd/mm/yyyy',
        maxDate: 0,
        numberOfMonths: 1,
        autoclose: true,
        endDate: new Date(),
    });
    $('#txtPurchaseToDate').datepicker({
        format: 'dd/mm/yyyy',
        maxDate: 0,
        numberOfMonths: 1,
        autoclose: true,
    });
    jQuery(grid_selector).jqGrid({
        url: '/Transport/GPSTrackingDeviceMasterJqGrid',
        datatype: 'json',
        height: 170,
        colNames: ['GPS_TrackingDeviceMaster_Id', 'Campus', 'Brand Name', 'Model Name', 'IMEI Nmber', 'Purchase Date', 'Created Date', 'Created By', 'Modified Date', 'Modified By'],
        colModel: [

            { name: 'GPS_TrackingDeviceMaster_Id', index: 'GPS_TrackingDeviceMaster_Id', editable: true, hidden: true },
            { name: 'Campus', index: 'Campus', editable: true, stype: 'select', searchoptions: { dataUrl: '/Assess360/GetCampusddl' }, edittype: 'select', editoptions: { dataUrl: '/Assess360/GetCampusddl', style: "width: 140px; height: 20px; font-size: 0.9em" }, editrules: { required: true, custom: true, custom_func: checkvalid }, sortable: true },
            { name: 'BrandName', index: 'BrandName', editable: true },
            { name: 'ModelName', index: 'ModelName', editable: true },
            { name: 'IMEINmber', index: 'IMEINmber', editable: true },
            { name: 'PurchaseDate', index: 'PurchaseDate', editable: true, editrules: { required: true }, editoptions: {
                         dataInit: function (elem) {
                             $(elem).datepicker({
                                 format: "dd/mm/yyyy",
                                 autoclose: true,
                                 buttonImage: "../../Images/date.gif",
                                 buttonImageOnly: true,
                                 changeMonth: true,
                                 timeFormat: 'hh:mm:ss',
                                 autowidth: true,
                                 changeYear: true,
                                 endDate: new Date(),
                                 showButtonPanel: false
                                 // onSelect: function (selected) { $("#ToDate").datepicker("option", "minDate", selected) } 
                             })
                         }
                     }, search: false
                 },
            { name: 'CreatedDate', index: 'CreatedDate', editable: false, hidden: true },
            { name: 'CreatedBy', index: 'CreatedBy', editable: false, hidden: true },
            { name: 'ModifiedDate', index: 'ModifiedDate', editable: false, search: false, hidden: true },
            { name: 'ModifiedBy', index: 'ModifiedBy', editable: false, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        sortname: 'GPS_TrackingDeviceMaster_Id',
        sortorder: 'Asc',
        altRows: true,
        autowidth: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'> </i>&nbsp;GPS Tracking Device Master"
    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
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
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {
                url: '/Transport/SaveOrUpdateGPSTrackingDeviceMaster', closeAfterEdit: true, closeOnEscape: true, beforeShowForm: function (frm)
                { $('#FormCode').attr('readonly', 'readonly'); }
            }, //Edit
            {
                url: '/Transport/SaveOrUpdateGPSTrackingDeviceMaster', closeOnEscape: true, beforeShowForm: function (frm)
                { $('#FormCode').attr('readonly', 'readonly'); }
            }, //Add

		    { url: '/Transport/DeleteGPSTrackingDeviceMaster' },
    {},
     {},
      {})
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
    function styleCheckbox(table) {
    }
    function updateActionIcons(table) {
    }
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

    $("#btnSearch").click(function () {
        debugger;
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/GPSTrackingDeviceMasterJqGrid',
                    postData: { Campus: $("#Campus").val(), BrandName: $("#txtBrandName").val(), ModelName: $("#txtModelName").val(), IMEINmber: $("#txtIMEINo").val(), PurchaseFromDate: $("#txtPurchaseFromDate").val(), PurchaseToDate: $("#txtPurchaseToDate").val(), },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/GPSTrackingDeviceMasterJqGrid',
                    postData: { Campus: $("#Campus").val(), BrandName: $("#txtBrandName").val(), ModelName: $("#txtModelName").val(), IMEINmber: $("#txtIMEINo").val(), PurchaseFromDate: $("#txtPurchaseFromDate").val(), PurchaseToDate: $("#txtPurchaseToDate").val(), },
                    page: 1
                }).trigger("reloadGrid");
    });


});
function checkvalid(value, column) {
    if (value == 'nil') {
        return [false, column + ": Field is Required"];
    }
    else {
        return [true];
    }
}
function GetCampus() {
    var ddlcam = $("#Campus");
    $.ajax({
        type: 'POST',
        async: true,
        dataType: "json",
        url: "/Base/FillBranchCode",
        success: function (data) {
            ddlcam.empty();
            ddlcam.append("<option value=''> Select </option>");
            for (var i = 0; i < data.length; i++) {
                ddlcam.append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
            }
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}