jQuery(function ($) {
    var grid_selector = "#TyreDetailsList";
    var pager_selector = "#TyreDetailsListPager";

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

    $('#PurchasedDate').datepicker().datepicker('setDate', $("#PurchasedDate").val());
    $('#IdNum').val('@Model.Id');

    jQuery(grid_selector).jqGrid({
        url: '/Transport/TyreDetailsListJqGrid?InvoiceId=' + $("#Id").val(),
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'Invoice Id', 'Tyre No', 'Make', 'Model', 'Size', 'Type', 'Tube Cost', 'Tyre Cost', 'Total Cost'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true, key: true, editable: true },
              { name: 'InvoiceId', index: 'InvoiceId', hidden: true, editable: true },
              { name: 'TyreNo', index: 'TyreNo', editable: true },
              { name: 'Make', index: 'Make', editable: true },
              { name: 'Model', index: 'Model', editable: true, edittype: 'select', editrules: { required: true },
                  editoptions: { value: ":Select;Tube:Tube;Tubeless:Tubeless",
                      dataEvents: [
                                            { type: 'change',
                                                fn: function (e) {
                                                    debugger;
                                                    CalculateTotalCost();
                                                }
                                            }
                                                ]
                  }
              },
              { name: 'Size', index: 'Size', editable: true },
              { name: 'Type', index: 'Type', editable: true, stype: 'select', edittype: 'select', editrules: { required: true }, editoptions: { value: ":Select;New:New;Retreaded:Retreaded"} },
              { name: 'TubeCost', index: 'TubeCost', editable: true, editoptions: {
                  dataEvents: [
                                            { type: 'keyup',
                                                fn: function (e) {
                                                    debugger;
                                                    CalculateTotalCost();
                                                }
                                            }
                                                ]
              }
              },
              { name: 'TyreCost', index: 'TyreCost', editable: true, editoptions: {
                  dataEvents: [
                                            { type: 'keyup',
                                                fn: function (e) {
                                                    debugger;
                                                    CalculateTotalCost();
                                                }
                                            }
                                                ]
              }
              },
              { name: 'TotalCost', index: 'TotalCost', editable: true, editrules: { required: true }, editoptions: { readonly: true, style: "background-color:#f5f5f5"} },
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
            jQuery(grid_selector).trigger("reloadGrid");
            var $self = $(this),
                TubeCost = parseFloat($self.jqGrid("getCol", "TubeCost", false, "sum")).toFixed(2);
            TyreCost = parseFloat($self.jqGrid("getCol", "TyreCost", false, "sum")).toFixed(2);
            TotalCost = parseFloat($self.jqGrid("getCol", "TotalCost", false, "sum")).toFixed(2);
            $self.jqGrid("footerData", "set", { '': '', '': '', '': '', '': '', '': '', '': '', '': '', TubeCost: TubeCost, TyreCost: TyreCost, TotalCost: TotalCost });
            $("#TotalCost1").val(TotalCost);
            $("#TaxPercentage").keyup();
            $("#OtherExpenses").keyup();
        },
        caption: "<i class='ace-icon fa fa-cubes'></i>&nbsp;Tyre Details"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            { width: 'auto', url: '/Transport/AddTyreDetails?test=edit' },
            { width: 'auto', url: '/Transport/AddTyreDetails', beforeSubmit: function (postdata, formid) {
                debugger;
                postdata.InvoiceId = $("#IdNum").val();
                return [true, ''];
            }
            },
            {}, {})
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
    $("#btnSave").click(function () {
        var Campus = $("#Campus").val();
        var PurchasedDate = $("#PurchasedDate").val();
        var PurchasedFrom = $("#PurchasedFrom").val();
        var PurchasedBy = $("#PurchasedBy").val();
        var InvoiceNo = $("#InvoiceNo").val();
        if (Campus == '' || PurchasedDate == '' || PurchasedFrom == '' || PurchasedBy == '' || InvoiceNo == '') {
            ErrMsg("Please fill all Mandatory fields.");
            return false;
        }
    });

    $("#btnBack").click(function () {
        window.location.href = '/Transport/TyreManagement';
    });
    debugger;
    if (parseInt($("#Id").val()) == 0) {
        $("#add_TyreDetailsList").hide();
        $("#edit_TyreDetailsList").hide();
    }
    $("#btnComplete").click(function () {
        var RecordCount = jQuery("#TyreDetailsList").jqGrid('getGridParam', 'records');
        if (RecordCount == 0) {
            ErrMsg("Empty items cannot be completed. Please add tyre details.");
            return false;
        }
        if (confirm("Are you sure to Complete?") == false)
            return false;
    });
    $("#TotalCost").keyup(function (TotalCost) {
        debugger;
        $("#TotalCost").val(TotalCost);
    });
    $("#TaxPercentage").keyup(function () {
        debugger;
        if ($(this).val() != '') {
            $("#TaxAmount").val(((parseFloat($(this).val()) * parseFloat($("#TotalCost1").val()))) / 100);
            $("#RoundedOffCost").val((parseFloat($("#TotalCost1").val()) + parseFloat($("#TaxAmount").val()) + ($("#OtherExpenses").val() != '' ? parseFloat($("#OtherExpenses").val()) : 0)).toFixed(2));
        }
        else {
            $("#TaxAmount").val(0);
            $("#RoundedOffCost").val((parseFloat($("#TotalCost1").val()) + ($("#OtherExpenses").val() != '' ? parseFloat($("#OtherExpenses").val()) : 0)).toFixed(2));
        }
    });
    $("#OtherExpenses").keyup(function () {
        debugger;
        if ($(this).val() != '')
            $("#RoundedOffCost").val((parseFloat($("#TotalCost1").val()) + parseFloat($("#TaxAmount").val()) + parseFloat($(this).val())).toFixed(2));
        else
            $("#RoundedOffCost").val((parseFloat($("#TotalCost1").val()) + parseFloat($("#TaxAmount").val())).toFixed(2));
    });
    $("#VendorSearch").click(function () {
        $("#TransportVendorList").jqGrid('resetSelection');
        LoadPopupDynamicaly("/Transport/ChooseTransportVendor", $('#DivVendorSearch'),
            function () {
                LoadSetGridParam($('#TransportVendorList'), "/Transport/TransportVendorMasterListJqGrid")
            }, "", 1000);
    });
    function ValidateAndSave() {
        var objInvoiceDetails = {
            Campus: $("#Campus").val(),
            PurchasedDate: $("#PurchasedDate").val(),
            PurchasedFrom: $("#PurchasedFrom").val(),
            PurchasedBy: $("#PurchasedBy").val(),
            InvoiceNo: $("#InvoiceNo").val()
        }
        $.ajax({
            url: '/Transport/SaveTyreInvoiceDetails',
            type: 'POST',
            dataType: 'json',
            data: objInvoiceDetails,
            traditional: true,
            success: function (data) {
                $("#Id").val(data);
                //                $('#Id').val(data.substring(data.lastIndexOf("-") + 1, data.length));
            },
            error: function (xhr, status, error) {
                ErrMsg($.parseJSON(xhr.responseText).Message);
            }
        });
    }

    function CalculateTotalCost() {
        debugger;
        if ($("#Model").val() != '' && $("#Model").val() == "Tube")
            $("#TotalCost").val(($("#TubeCost").val() != '' ? parseFloat($("#TubeCost").val()) : 0) + ($("#TyreCost").val() != '' ? parseFloat($("#TyreCost").val()) : 0));
        else {
            $("#TubeCost").val('');
            $("#TotalCost").val($("#TyreCost").val() != '' ? parseFloat($("#TyreCost").val()) : 0);
        }
    }
});
