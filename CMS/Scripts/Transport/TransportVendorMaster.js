jQuery(function ($) {
    var grid_selector = "#TransportVendorList";
    var pager_selector = "#TransportVendorListPager";

    //resize to fit page size
    //    $(window).on('resize.jqGrid', function () {
    //        $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    //    })
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".tab-content").width());
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

    debugger;
    jQuery(grid_selector).jqGrid({
        url: '/Transport/TransportVendorMasterListJqGrid',
        datatype: 'json',
        height: 180,
        colNames: ['Name', 'Dealer Type', 'Vendor Type', 'Vendor For', 'PAN', 'TIN', 'FAX', 'Contact Name', 'Contact No', 'Email', 'Website', 'Reason For Selecting', 'Credit Days', 'Applicable For TDS', 'Bank Name', 'Bank Branch', 'Account Name', 'Account Type', 'Account No', 'PIN', 'Add1', 'Add2', 'Id'],
        colModel: [
            { name: 'Name', index: 'Name', editable: true, formoptions: { elmsuffix: ' *', rowpos: 1, colpos: 1} },
            { name: 'DealerType', index: 'DealerType', editable: true, formoptions: { elmsuffix: ' *', rowpos: 1, colpos: 2} },
            { name: 'VendorType', index: 'VendorType', editable: true, edittype: 'select', editoptions: { value: ":Select;Sale:Sale;Service:Service" }, formoptions: { elmsuffix: ' *', rowpos: 1, colpos: 3} },
            { name: 'VendorFor', index: 'VendorFor', editable: true, formoptions: { elmsuffix: ' *', rowpos: 2, colpos: 1} },
            { name: 'PAN', index: 'PAN', editable: true, formoptions: { elmsuffix: ' *', rowpos: 2, colpos: 2} },
            { name: 'TIN', index: 'TIN', editable: true, formoptions: { elmsuffix: ' *', rowpos: 2, colpos: 3} },
            { name: 'FAX', index: 'FAX', editable: true, formoptions: { elmsuffix: ' *', rowpos: 3, colpos: 1} },
            { name: 'ContactName', index: 'ContactName', editable: true, formoptions: { elmsuffix: ' *', rowpos: 3, colpos: 2} },
            { name: 'ContactNo', index: 'ContactNo', editable: true, formoptions: { elmsuffix: ' *', rowpos: 3, colpos: 3} },
            { name: 'Email', index: 'Email', editable: true, formoptions: { elmsuffix: ' *', rowpos: 4, colpos: 1} },
            { name: 'Website', index: 'Website', editable: true, formoptions: { elmsuffix: ' *', rowpos: 4, colpos: 2} },
            { name: 'ReasonForSelecting', index: 'ReasonForSelecting', editable: true, formoptions: { elmsuffix: ' *', rowpos: 4, colpos: 3} },
            { name: 'CreditDays', index: 'CreditDays', editable: true, formoptions: { elmsuffix: ' *', rowpos: 5, colpos: 1} },
            { name: 'ApplicableForTDS', index: 'ApplicableForTDS', editable: true, edittype: 'select', editoptions: { value: ":Select;True:Yes;False:No" }, formoptions: { elmsuffix: ' *', rowpos: 5, colpos: 2} },
            { name: 'BankName', index: 'BankName', editable: true, formoptions: { elmsuffix: ' *', rowpos: 5, colpos: 3} },
            { name: 'BankBranch', index: 'BankBranch', editable: true, formoptions: { elmsuffix: ' *', rowpos: 6, colpos: 1} },
            { name: 'AccountName', index: 'AccountName', editable: true, formoptions: { elmsuffix: ' *', rowpos: 6, colpos: 2} },
            { name: 'AccountType', index: 'AccountType', editable: true, formoptions: { elmsuffix: ' *', rowpos: 6, colpos: 3} },
            { name: 'AccountNo', index: 'AccountNo', editable: true, formoptions: { elmsuffix: ' *', rowpos: 7, colpos: 1} },
            { name: 'PIN', index: 'PIN', editable: true, formoptions: { elmsuffix: ' *', rowpos: 7, colpos: 2} },
            { name: 'Add1', index: 'Add1', editable: true, edittype: 'textarea', editoptions: { rows: "4", cols: "20", maxlength: 4000 }, formoptions: { elmsuffix: ' *', rowpos: 8, colpos: 1} },
            { name: 'Add2', index: 'Add2', editable: true, edittype: 'textarea', editoptions: { rows: "4", cols: "20", maxlength: 4000 }, formoptions: { elmsuffix: ' *', rowpos: 8, colpos: 2} },
            { name: 'Id', index: 'Id', editable: true, hidden: true, key: true },
            ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
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
        caption: "<i class='ace-icon fa fa-user'></i>&nbsp;Transport Vendor List"
    });
    //    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    //$(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });

    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: true,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            { height: 440, width: 1100, url: '/Transport/AddTransportVendor?test=edit' },
             { height: 440, width: 1100, url: '/Transport/AddTransportVendor' }, {}, {}, {})

    function styleCheckbox(table) {
    }
    //unlike navButtons icons, action icons in rows seem to be hard-coded
    //you can change them like this in here if you want
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

    $("#btnVndrSearch").click(function () {
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/TransportVendorMasterListJqGrid',
                    postData: { Name: $("#txtVendrName").val(), DealerType: $("#txtVndrDealerType").val(), VendorType: $("#txtVendorType").val(), VendorFor: $("#txtVendorFor").val(), PAN: $("#txtVndrPAN").val(), TIN: $("#txtVndrTIN").val(), FAX: $("#txtVndrFAX").val(), ContactName: $("#txtVndrContactName").val(), ContactNo: $("#txtVndrContactNo").val(), BankName: $("#txtVndrBankName").val(), AccountName: $("#txtVndrAccountName").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnVndrReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/TransportVendorMasterListJqGrid',
                    postData: { Name: $("#txtVendrName").val(), DealerType: $("#txtVndrDealerType").val(), VendorType: $("#txtVendorType").val(), VendorFor: $("#txtVendorFor").val(), PAN: $("#txtVndrPAN").val(), TIN: $("#txtVndrTIN").val(), FAX: $("#txtVndrFAX").val(), ContactName: $("#txtVndrContactName").val(), ContactNo: $("#txtVndrContactNo").val(), BankName: $("#txtVndrBankName").val(), AccountName: $("#txtVndrAccountName").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
});
