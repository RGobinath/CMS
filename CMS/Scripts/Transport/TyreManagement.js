jQuery(function ($) {
    var grid_selector = "#TyreInvoiceList";
    var pager_selector = "#TyreInvoiceListPager";

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
    function formateadorLink(cellvalue, options, rowObject) {
        if (rowObject[14] == "Open")
            return "<a href=/Transport/TyreInvoiceDetails?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
        else
            return "<a href=/Transport/ShowTyreInvoiceDetails?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
    }

    jQuery(grid_selector).jqGrid({
        url: '/Transport/TyreInvoiceListJqGrid?Status=' + $("#Status").val(),
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'RefNo', 'Campus', 'Purchase Date', 'Purchased From', 'Invoice No', 'Payment Type', 'Total Cost', 'Tax %', 'Tax Amount', 'Other Expenses', 'Rounded Off Cost', 'Created Date', 'Created By', 'Status'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true, key: true, editable: true },
            { name: 'RefNo', index: 'RefNo', formatter: formateadorLink },
            { name: 'Campus', index: 'Campus' },
            { name: 'Purchase Date', index: 'Purchase Date', search: false },
            { name: 'Purchased From', index: 'Purchased From' },
            { name: 'Invoice No', index: 'Invoice No' },
            { name: 'Payment Type', index: 'Payment Type' },
            { name: 'TotalCost', index: 'TotalCost' },
            { name: 'TaxPercentage', index: 'TaxPercentage' },
            { name: 'TaxAmount', index: 'TaxAmount' },
            { name: 'OtherExpenses', index: 'OtherExpenses' },
            { name: 'RoundedOffCost', index: 'RoundedOffCost' },
            { name: 'CreatedDate', index: 'CreatedDate' },
            { name: 'CreatedBy', index: 'CreatedBy' },
            { name: 'Status', index: 'Status' },
            ],
        viewrecords: true,
        rowNum: 10,
        rowList: [25, 50, 100, 500],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Asc',
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-car'></i>&nbsp;Tyre Invoice Details",
        subGrid: true,
        subGridOptions: {
            plusicon: "ace-icon fa fa-plus center bigger-110 blue",
            minusicon: "ace-icon fa fa-minus center bigger-110 blue",
            openicon: "ace-icon fa fa-chevron-right center orange",
            // load the subgrid data only once // and the just show/hide 
            "reloadOnExpand": false,
            // select the row when the expand column is clicked 
            "selectOnExpand": true
        },
        subGridRowExpanded: function (subgrid_id, ParentId) {
            debugger;
            var subgrid_table_id, pager_id;
            subgrid_table_id = subgrid_id + "_t";
            pager_id = "p_" + subgrid_table_id;
            $("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");
            jQuery("#" + subgrid_table_id).jqGrid({
                url: '/Transport/TyreDetailsListJqGrid?InvoiceId=' + ParentId,
                datatype: "json",
                colNames: ['Id', 'Invoice Id', 'Tyre No', 'Make', 'Model', 'Size', 'Type', 'Tube Cost', 'Tyre Cost', 'Total Cost'],
                colModel: [
              { name: 'Id', index: 'Id', hidden: true, key: true, editable: true },
              { name: 'InvoiceId', index: 'InvoiceId', hidden: true, editable: true },
              { name: 'TyreNo', index: 'TyreNo' },
              { name: 'Make', index: 'Make' },
              { name: 'Model', index: 'Model' },
              { name: 'Size', index: 'Size' },
              { name: 'Type', index: 'Type' },
              { name: 'TubeCost', index: 'TubeCost' },
              { name: 'TyreCost', index: 'TyreCost' },
              { name: 'TotalCost', index: 'TotalCost' },
               ],
                autoWidth: true,
                altRows: true,
                viewrecords: true,
                shrinkToFit: true,
                rowNum: 5,
                rowList: [5, 10, 20, 30],
                pager: pager_id,
                sortname: 'Id',
                sortorder: "asc",
                height: 130,
                loadComplete: function () {
                    var $self = $(this),
                        TubeCost = parseFloat($self.jqGrid("getCol", "TubeCost", false, "sum")).toFixed(2);
                    TyreCost = parseFloat($self.jqGrid("getCol", "TyreCost", false, "sum")).toFixed(2);
                    TotalCost = parseFloat($self.jqGrid("getCol", "TotalCost", false, "sum")).toFixed(2);
                    // TotalQtySum = parseFloat($self.jqGrid("getCol", "IssuedQty", false, "sum")).toFixed(2);
                    //                amtrcv = parseFloat($self.jqGrid("getCol", "AmountReceived", false, "sum")).toFixed(2);
                    //                balpay = parseFloat($self.jqGrid("getCol", "BalancePayable", false, "sum")).toFixed(2);
                    $self.jqGrid("footerData", "set", { '': '', '': '', '': '', '': '', '': '', '': '', '': '', TubeCost: TubeCost, TyreCost: TyreCost, TotalCost: TotalCost });

                    var table = this;
                    setTimeout(function () {
                        styleCheckbox(table);
                        updateActionIcons(table);
                        updatePagerIcons(table);
                        enableTooltips(table);
                    }, 0);
                },
                multiselect: true
            });
            jQuery("#" + SKUListTable).jqGrid('navGrid', "#" + SKUListPager, { edit: false, add: false, del: false })
        }
    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, {}, {}, {}, {})


    //For pager Icons
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
    $("#btnNew").click(function () {
        window.location.href = '/Transport/TyreInvoiceDetails';
    });

    $("#Status").change(function () {
        $("#btnSearch").click();
    });

    $("#btnSearch").click(function (Status) {
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/TyreInvoiceListJqGrid',
                    postData: { Campus: $("#branchcodeddl").val(), PurchasedFrom: $("#PurchasedFrom").val(), InvoiceNo: $("#InvoiceNo").val(), PaymentType: $("#PaymentType").val(), TotalCost: $("#TotalCost").val(), TaxPercentage: $("#TaxPercentage").val(), RoundedOffCost: $("#RoundedOffCost").val(), TaxAmount: $("#TaxAmount").val(), Status: $("#Status").val() },
                    page: 1
                }).trigger("reloadGrid");
    });

    $("#btnReset").click(function (Status) {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/TyreInvoiceListJqGrid',
                    postData: { Campus: $("#branchcodeddl").val(), PurchasedFrom: $("#PurchasedFrom").val(), InvoiceNo: $("#InvoiceNo").val(), PaymentType: $("#PaymentType").val(), TotalCost: $("#TotalCost").val(), TaxPercentage: $("#TaxPercentage").val(), RoundedOffCost: $("#RoundedOffCost").val(), TaxAmount: $("#TaxAmount").val(), Status: $("#Status").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
});

$.getJSON("/Base/FillAllBranchCode",
     function (fillcampus) {
         var ddlcam = $("#branchcodeddl");
         ddlcam.empty();
         ddlcam.append($('<option/>',
        {
            value: "",
            text: "Select One"

        }));

         $.each(fillcampus, function (index, itemdata) {
             ddlcam.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
         });
     });