
    var grid_selector = "#MaterialRequestList";
    var pager_selector = "#MaterialRequestListPager";

    $(function () {
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
        $("#RequestorDescription").attr("readonly", "readonly").css("background-color", "#F5F5F5");

        if ($("#RequiredFromStore").val() != '') {
            $("#Store").empty();
            $("#Store").append("<option value='" + $("#RequiredFromStore").val() + "'>" + $("#RequiredFromStore").val() + "</option>");
        }
        else {
            var cam = $("#RequiredForCampus").val();
            $.ajax({
                type: 'POST',
                async: false,
                dataType: "json",
                url: '/Store/FillStore?Campus=' + cam,
                success: function (data) {
                    debugger;
                    $("#Store").empty();
                    $("#Store").append("<option value='" + data.rows[0].Value + "'>" + data.rows[0].Text + "</option>");
                }
            });
        }
        var Id = $("#RequestId").val();
        var Store = $("#Store").val();
        $("#Store").change(function () {
            $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/ItemsTobeIssued',
                    postData: { Id: Id, Store: $("#Store").val() },
                    page: 1
                }).trigger("reloadGrid");
        });

        jQuery(grid_selector).jqGrid({
            url: '/Store/ItemsTobeIssued?Id=' + Id + "&Store=" + Store,
            datatype: 'json',
            mtype: 'GET',
            colNames: ['Id', 'Req.Type', 'Grade', 'Section', 'Req.For', 'Material', 'Material Group', 'Material Sub Group', 'Units', 'Req.Date', 'Status', 'Req.Qty', 'App.Qty', 'Prev.Iss.Qty', 'Inward Id', 'Available Qty','Unit Price','Tax','Discount', 'Iss.Qty', 'Total Price'],
            colModel: [
              { name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'RequestType', index: 'RequestType', width: 90, sortable: true },
              { name: 'RequiredForGrade', index: 'RequiredForGrade', width: 90, sortable: true },
              { name: 'Section', index: 'Section', width: 60, sortable: true },
              { name: 'RequiredFor', index: 'RequiredFor', width: 90, sortable: true },
              { name: 'Material', index: 'Material', width: 90, sortable: true, cellattr: function (rowId, val, rawObject) { return 'title="' + 'Material Group:' + rawObject[6] + ', Material Sub Group:' + rawObject[7] + '"' } },
              { name: 'MaterialGroup', index: 'MaterialGroup', width: 90, sortable: true, hidden: true },
              { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 90, sortable: true, hidden: true },
              { name: 'Units', index: 'Units', width: 90, sortable: true },
              { name: 'RequiredDate', index: 'RequiredDate', width: 90, sortable: false },
              { name: 'Status', index: 'Status', width: 90, sortable: true },
              { name: 'Quantity', index: 'Quantity', width: 90, sortable: true },
              { name: 'ApprovedQty', index: 'ApprovedQty', width: 90, sortable: true },
              { name: 'IssueQty', index: 'IssueQty', width: 90, sortable: true, editrules: { required: true, integer: true} },
              { name: 'InwardIds', index: 'InwardIds' },
              { name: 'ClosingBalance', index: 'ClosingBalance', cellattr: function (rowId, cellValue, rawObject, cm, rdata) {
                  if (cellValue == 0) {
                      return 'class="ui-state-error ui-state-error-text"';
                  }
              }},
              { name: 'UnitPrices', index: 'UnitPrices' },
              { name: 'Taxes', index: 'Taxes', width: 90 },
              { name: 'Discounts', index: 'Discounts',width: 90 },
              { name: 'IssQty', index: 'IssQty', width: 90, sortable: true, editable: true, editrules: { custom: true, custom_func: fnNumberOnly },
                  editoptions: {
                      dataInit: function (element) {
                          $(element).keyup(function () {
                              var rowId = parseInt($(this).attr("id"));
                              CalculateTotalPrice(rowId);
                          })
                      }
                  }
              },

              { name: 'TotalPrice', index: 'TotalPrice', width: 90, sortable: true, editable: true, editrules: { custom: true, custom_func: fnNumberOnly} }
              ],
            pager: pager_selector,
            rowNum: '100',
            rowList: [100, 150, 200],
            sortname: 'Id',
            sortorder: 'Desc',
            height: '150',
            autowidth: true,
            shrinkToFit: true,
            viewrecords: true,
            caption: '<i class="fa fa-th-list">&nbsp;</i>Material Request List',
            forceFit: true,
            editurl: '/Store/UpdateIssueNoteList',
            loadError: function (xhr, status, error) {
                $(grid_selector).clearGridData();
                ErrMsg($.parseJSON(xhr.responseText).Message);
            },
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                var $this = $(this), rows = this.rows, l = rows.length, i, row;
                $(this).hide();
                for (i = 1; i < l; i++) {
                    row = rows[i];
                    if ($(row).hasClass("jqgrow")) {

                        var selectedData = $(grid_selector).jqGrid('getRowData', row.id);
                        if (selectedData.Status != "Completely Issued" && selectedData.ClosingBalance != 0 && selectedData.UnitPrice != 0) {
                            $this.jqGrid('editRow', row.id);
                        }
                    }
                }
                $(this).show();
            },
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
            subGridRowExpanded: function (MatIssueList, Id) {
                var MatIssueListTable, MatIssueListPager;
                MatIssueListTable = MatIssueList + "_t";
                MatIssueListPager = "p_" + MatIssueListTable;
                $("#" + MatIssueList).html("<table id='" + MatIssueListTable + "' ></table><div id='" + MatIssueListPager + "' ></div>");
                jQuery("#" + MatIssueListTable).jqGrid({
                    url: '/Store/MaterialIssueListJqGrid?Id=' + Id,
                    datatype: 'json',
                    mtype: 'GET',
                    colNames: ['Id', 'MRL Id', 'Iss Note Number', 'Issue Date', 'Issued Qty', 'Issued By', 'Status'],
                    colModel: [
                       { name: 'Id', index: 'Id', hidden: true },
                       { name: 'MRLId', index: 'MRLId', width: 90, sortable: true, hidden: true },
                       { name: 'IssNoteNumber', index: 'IssNoteNumber', width: 90, sortable: true },
                       { name: 'IssueDate', index: 'IssueDate', width: 90, sortable: true },
                       { name: 'IssueQty', index: 'IssueQty', width: 90, sortable: true },
                       { name: 'IssuedBy', index: 'IssuedBy', width: 90, sortable: true },
                       { name: 'Status', index: 'Status', width: 90, sortable: true },
                       ],
                    pager: MatIssueListPager,
                    rowNum: '5',
                    rowList: [5, 10, 20, 50, 100, 150, 200],
                    sortname: 'Id',
                    sortorder: "Asc",
                    height: '130',
                    width: 1225,
                    autowidth: true,
                    shrinkToFit: true,
                    viewrecords: true,
                    forceFit: true
                });
                jQuery("#" + MatIssueListTable).jqGrid('navGrid', "#" + MatIssueListPager, { edit: false, add: false, del: false })
            }
        });

        $("#btnBack").click(function () {
            window.location.href = $('#BackUrl').val();
            // '@Url.Action("MaterialRequestList", "Store")';
        });
        $("#btnGotoIssueNoteList").click(function () {
            window.location.href = $('#GotoIssue').val();
            //'@Url.Action("IssueNoteList", "Store")';
        });

        $("#btnIdIssue").click(function () {
            debugger;
            var MarIssList = '';
            var dataIds = $('#MaterialRequestList').jqGrid('getDataIDs');
            if (dataIds == "") {
                ErrMsg("Empty items cannot be issued.");
                return false;
            }
            var IssNoteId = '';

            var DeliveredThrough = $("#DeliveredThrough").val();
            var DeliveryDetails = $("#DeliveryDetails").val();
            var DeliveryDate = $("#DeliveryDate").val();
            var Store = $("#Store").val();
            if (DeliveredThrough == "") {
                ErrMsg("Please select delivered through");
                return false;
            }
            if (DeliveryDetails == "") {
                ErrMsg("Please type Delivery Details");
                $("#DeliveryDetails").focus();
                return false;
            }
            if (DeliveryDate == "") {
                ErrMsg("Please select Delivery Date");
                $("#DeliveryDate").focus();
                return false;
            }
            if (Store == "") {
                ErrMsg("Please select Store");
                $("#Store").focus();
                return false;
            }
            var MaterialIssueNote = {
                IssNoteId: $("#IssNoteId").val(),
                RequestId: $("#RequestId").val(),
                RequestNumber: $("#RequestNumber").val(),
                RequestedDate: $("#RequestedDate").val(),
                ProcessedBy: $("#ProcessedBy").val(),
                UserRole: $("#UserRole").val(),
                RequestStatus: $("#RequestStatus").val(),
                Campus: $("#Campus").val(),
                RequiredForCampus: $("#RequiredForCampus").val(),
                DCNumber: $("#DCNumber").val(),
                DeliveredThrough: $("#DeliveredThrough").val(),
                DeliveryDetails: $("#DeliveryDetails").val(),
                DeliveryDate: $("#DeliveryDate").val(),
                IssueDate: $("#IssueDate").val(),
                IssuedBy: $("#IssuedBy").val(),
                CreatedDate: $("#CreatedDate").val(),
                Store: $("#Store").val(),
                Department: $("#Department").val(),
                RequiredForStore: $("#RequiredForStore").val(),
                RequiredFromStore: $("#RequiredFromStore").val(),
                RequestorDescription: $("#RequestorDescription").val()
            };

            for (var i = 0, list = dataIds.length; i < list; i++) {

                selectedData = $(grid_selector).jqGrid('getRowData', dataIds[i]);
                var SeparatedClosingBalance = selectedData.ClosingBalance.split(', ');
                if (selectedData.Status != "Completely Issued" && parseInt(selectedData.ClosingBalance) != 0) {

                    var IssQty = $("#" + dataIds[i] + "_IssQty").val();

                    //                    if (parseInt(selectedData.ClosingBalance) == 0) {
                    //                        ErrMsg("Material cannot be issued since Available Qty is 0");
                    //                        return false;
                    //                        break;
                    //                    }

                    if (IssQty == "" || IssQty == null) {
                        ErrMsg("Please type Issue Qty");
                        $("#" + dataIds[i] + "_IssQty").focus();
                        return false;
                        break;
                    }
                    else if (isNaN(IssQty)) {
                        ErrMsg("Numbers only allowed");
                        $("#" + dataIds[i] + "_IssQty").focus();
                        return false;
                        break;
                    }
                    else if (parseInt(IssQty) < 0) {
                        ErrMsg("Issue Qty should be zero or greater than zero.");
                        $("#" + dataIds[i] + "_IssQty").focus();
                        return false;
                        break;
                    }
                    else if (parseInt(IssQty) + parseInt(selectedData.IssueQty) > parseInt(selectedData.ApprovedQty)) {
                        ErrMsg("Previous Issued Qty and Current Issue Qty should not exceed the Approved Quantity");
                        $("#" + dataIds[i] + "_IssQty").focus();
                        return false;
                        break;
                    }
                    else if (parseInt(IssQty) > (parseInt(SeparatedClosingBalance[0]) + parseInt(SeparatedClosingBalance[1]))) {
                        ErrMsg("Issue Quantity should not greater than Available Qty");
                        $("#" + dataIds[i] + "_IssQty").focus();
                        return false;
                        break;
                    }
                }
            }
            $.ajax({
                url: '/Store/SaveIssueNote',
                type: 'GET',
                dataType: 'json',
                data: MaterialIssueNote,
                traditional: true,
                async: false,
                success: function (data) {
                    IssNoteId = data;
                    $("#IssNoteId").val(data);
                    for (var i = 0, list = dataIds.length; i < list; i++) {
                        selectedData = $(grid_selector).jqGrid('getRowData', dataIds[i]);
                        // if (selectedData.Status != "Completely Issued" && parseInt(selectedData.ClosingBalance) != 0 && selectedData.UnitPrice) {
                        MarIssList = {
                            IssNoteId: data,
                            MRLId: selectedData.Id,
                            RequestQty: selectedData.Quantity,
                            ApprovedQty: selectedData.ApprovedQty,
                            IssueQty: $("#" + dataIds[i] + "_IssQty").val(),
                            Status: selectedData.Status,
                            UnitPrices: selectedData.UnitPrices,
                            TotalPrice: $("#" + dataIds[i] + "_TotalPrice").val(),
                            InwardIds: selectedData.InwardIds,
                            Material: selectedData.Material
                        }
                        upIssueLst(MarIssList);
                        //  }
                    }

                    $("#btnIdIssue").hide();
                    $("#DeliveredThrough").attr("disabled", true);
                    $("#DeliveryDetails").attr("disabled", true);
                    $("#DeliveryDate").attr("disabled", true);
                    $(grid_selector).setGridParam({ url: '/Store/ItemsTobeIssued?Id=' + Id }).trigger("reloadGrid");
                    InfoMsg("Material Issued successfully. Issue Note Id is INN-" + IssNoteId, function () { window.location.href = '/Store/ShowMaterialIssueNote?Id=' + IssNoteId; });
                }
            });

        });
        $("#CommentList").jqGrid({
            url: '/Store/DescriptionForSelectedIdJqGrid?Id=' + $('#RequestId').val(),
            datatype: 'json',
            mtype: 'GET',
            colNames: ['Commented By', 'Commented On', 'Rejection Comments', 'Resolution Comments'],
            colModel: [
            // { name: 'Issue Number', index: 'EntityRefId', sortable: false },
              {name: 'CommentedBy', index: 'CommentedBy', width: 300, sortable: false },
              { name: 'CommentedOn', index: 'CommentedOn', width: 300, sortable: false },
              { name: 'RejectionComments', index: 'RejectionComments', width: 300, sortable: false },
              { name: 'ResolutionComments', index: 'ResolutionComments', width: 300, sortable: false }
             ],
            rowNum: -1,
            width: 1160,
            autowidth: true,
            height: 100,
            sortname: 'CommentedOn',
            sortorder: "asc",
            viewrecords: true,
            caption: 'Discussion Forum'
        });

        $(".HeaderButton", $('#CommentList')[0].grid.cDiv).trigger("click");

        $("#CheckStock").click(function () {
            LoadPopupDynamicaly("/Store/Stock", $('#DivCheckStock'),
            function () {
                LoadSetGridParam($('#StockList'), "/Store/StockListJqGrid")
            });
        });

        function upIssueLst(MarIssList) {
            $.ajax({
                url: '/Store/UpdateIssueList',
                dataType: 'json',
                data: MarIssList,
                async: false,
                success: function (data) {

                    //alert("Nested Ajax call invoked, Wow, please put ur logic here....Bye Lee");
                }
            });
        }

        $("#btnbkToAvailable").click(function () {
            $.ajax({
                url: '/Store/MoveBackToAvailable?ActivityId=' + $('#ActivityId').val(),
                type: 'GET',
                dataType: 'json',
                traditional: true,
                success: function (data) {
                    if (data & data == true) {
                        SucessMsg($("#RequestNumber").val() + " is moved back to available.", function () { window.location.href = $('#BackUrl').val(); });
                    } else {
                    }
                },
                error: function (xhr, status, error) {
                    ErrMsg($.parseJSON(xhr.responseText).Message);
                }
            });
        });
        $('.date-picker').datepicker({
            format: "dd/mm/yyyy",
            numberOfMonths: 1,
            autoclose: true,
            endDate: '+0d'
        });
    });
    function fnNumberOnly(value, column) {
        if (value == '') {
            return [false, column + ": Field is Required"];
        }
        else if (!$.isNumeric(value)) {
            return [false, column + 'Should be numeric'];
        }
        else if (parseInt(value) < 0) {
            return [false, column + 'Should be 0 or greater.'];
        }
        else {
            return [true];
        }
    }
    function CalculateTotalPrice(rowId) {
    debugger;
        var IssQty = $("#" + rowId + "_IssQty").val();
        var selectedData = $('#MaterialRequestList').jqGrid('getRowData', rowId);
        if (parseInt(IssQty) + parseInt(selectedData.IssueQty) > parseInt(selectedData.ApprovedQty)) {
            ErrMsg("Previous Issued Qty and Current Issue Qty should not exceed the Approved Quantity");
            $("#" + rowId + "_TotalPrice").val('');
            $("#" + rowId + "_IssQty").val('');
            $("#" + rowId + "_IssQty").focus();
            return false;
        }
        var SeparatedUnitPrice = selectedData.UnitPrices.split(', ');
        var SeparatedClosingBalance = selectedData.ClosingBalance.split(', ');
        var TobeIssuedQty = selectedData.ApprovedQty - selectedData.IssueQty;
        var Tax=selectedData.Taxes.split(', ');
        var Discount=selectedData.Discounts.split(', ');

        var TotalPrice;
        var IssuedQty = 0;
        if (IssQty <= parseInt(SeparatedClosingBalance[0])) {
            TotalPrice = IssQty * ((SeparatedUnitPrice[0]-(SeparatedUnitPrice[0] * (Discount[0] / 100)))+ (SeparatedUnitPrice[0] * (Tax[0] / 100)));
        }
        if (IssQty > parseInt(SeparatedClosingBalance[0]) && parseInt(SeparatedClosingBalance[1]) != 0) {
            TotalPrice = parseInt(SeparatedClosingBalance[0]) * ((SeparatedUnitPrice[0]-(SeparatedUnitPrice[0] * (Discount[0] / 100)))+ (SeparatedUnitPrice[0] * (Tax[0] / 100)));
            IssuedQty = parseInt(IssuedQty) + parseInt(SeparatedClosingBalance[0]);
            TobeIssuedQty = IssQty - parseInt(SeparatedClosingBalance[0]);
            if (TobeIssuedQty <= parseInt(SeparatedClosingBalance[1])) {
                TotalPrice = TotalPrice + (TobeIssuedQty * ((SeparatedUnitPrice[1]-(SeparatedUnitPrice[1] * (Discount[1] / 100)))+ (SeparatedUnitPrice[1] * (Tax[1] / 100))));
                IssuedQty = parseInt(IssuedQty) + TobeIssuedQty;
            }
            else if (TobeIssuedQty > parseInt(SeparatedClosingBalance[1])) {
                TotalPrice = TotalPrice + (parseInt(SeparatedClosingBalance[1]) * ((SeparatedUnitPrice[1]-(SeparatedUnitPrice[1] * (Discount[1] / 100)))+ (SeparatedUnitPrice[1] * (Tax[1] / 100))));
                TobeIssuedQty = TobeIssuedQty - SeparatedClosingBalance[1];
                IssuedQty = parseInt(IssuedQty) + parseInt(SeparatedClosingBalance[1]);
                TobeIssuedQty = IssQty - IssuedQty;
                if (parseInt(IssuedQty) != IssQty && parseInt(IssuedQty) < parseInt(IssQty)) {
                }
            }
        }

        $("#" + rowId + "_TotalPrice").val(TotalPrice);

        if ($("#" + rowId + "_IssQty").val() == '')
            $("#" + rowId + "_TotalPrice").val('');
    }
    function updatePagerIcons(table) {
        var replacement = {
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

