$(function () {
    var grid_selector = "#FuelRefillList";
    var pager_selector = "#FuelRefillListPager";
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
    var Id = $("#Id").val();
    $("#VehicleSearch").button({ icons: { primary: "ui-icon-search"} });
    $("#VehicleSearch").click(function () {
        Id = $("#Id").val();
        if (Id == 0) {
            ErrMsg("Please Save");
            return false;
        }
        var SaveUrl = "/Transport/AddFuelRefillList/";
        var reloadGridUrl ="/Transport/FuelRefillListBulkEntryJqGrid?RefId=";
        var GridId = "FuelRefillList";
        var BulkEntryType = "FuelRefill";
        ModifiedLoadPopupDynamicaly("/Transport/VehicleSubTypeMaster?SaveUrl=" + SaveUrl + '&reloadGridUrl=' + reloadGridUrl + '&GridId=' + GridId + '&BulkEntryType=' + BulkEntryType, $('#DivVehicleSearch'),
            function () {
                LoadSetGridParam($('#VehicleSubTypeMasterList'), "/Transport/VehicleSubTypeMasterListJqGrid")
            }, function () { }, 950, 635, "Vechile Sub Type Master");
    });

    jQuery(grid_selector).jqGrid({
        url: "/Transport/FuelRefillListBulkEntryJqGrid?RefId=" + Id,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'RefId', 'Vehicle Id', 'Type', 'Vehicle No', 'Fuel Type', 'Fuel Fill Type', 'Fuel Quantity', 'Litre Price', 'Total Price', 'Last MM Reading', 'Current MM Reading',
            'Mileage', 'Filled By', 'Filled Date', 'Bunk Name', 'Created By', 'CreatedDate', ''],
        colModel: [
        //if any column added need to check formateadorLink
             {name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'RefId', index: 'RefId', hidden: true },
             { name: 'VehicleId', index: 'VehicleId', hidden: true },
             { name: 'Type', index: 'Type' },
             { name: 'VehicleNo', index: 'VehicleNo' },
             { name: 'FuelType', index: 'FuelType' },
             { name: 'FuelFillType', index: 'FuelFillType', editable: true, edittype: 'select', editoptions: { value: ":Select;Full Tank:Full Tank;Partial:Partial",
                 dataInit: function (element) {
                     $(element).change(function () {

                         var rowId = parseInt($(this).attr("id"));
                         CalculateMileage(rowId);
                     })
                 }
             }
             },
             { name: 'FuelQuantity', index: 'FuelQuantity', editable: true, editoptions: {
                 dataInit: function (element) {
                     $(element).keyup(function () {
                         var rowId = parseInt($(this).attr("id"));
                         CalculateMileage(rowId);
                         CalculateTotalPrice(rowId);
                     })
                 }
             }
             },
             { name: 'LitrePrice', index: 'LitrePrice', editable: true, editoptions: {
                 dataInit: function (element) {
                     $(element).keyup(function () {
                         var rowId = parseInt($(this).attr("id"));
                         CalculateTotalPrice(rowId);
                     })
                 }
             }
             },
             { name: 'TotalPrice', index: 'TotalPrice', editable: true, editoptions: { readonly: 'readonly'} },
             { name: 'LastMilometerReading', index: 'LastMilometerReading', editable: true, editoptions: { readonly: 'readonly'} },
             { name: 'CurrentMilometerReading', index: 'CurrentMilometerReading', editable: true,
                 editoptions: {
                     dataInit: function (element) {
                         $(element).keyup(function () {
                             var rowId = parseInt($(this).attr("id"));
                             CalculateMileage(rowId);
                         })
                     }
                 }
             },
             { name: 'Mileage', index: 'Mileage', editable: true },
             { name: 'DriverName', index: 'DriverName', width: 270, editable: true, editoptions: {
                 dataUrl: '/Transport/Driverddl',
                 dataInit: function (el) {
                     $(el).autocomplete({ source: function (request, response) {
                         var Campus = $("#Campus").val();
                         $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                             response(data);
                         });
                     },
                         minLength: 1,
                         delay: 100
                     })
                 }
             }
             },
             { name: 'FilledDate', index: 'FilledDate', editable: true, editoptions: {
                 dataInit: function (el) {
                     $(el).datepicker({ dateFormat: "dd/mm/yy", buttonImage: "../../Images/date.gif", buttonImageOnly: true,
                         changeMonth: true,
                         // timeFormat: 'hh:mm:ss',
                         autowidth: true,
                         changeYear: true,
                         autoclose:true,
                         // minDate: '+0d'
                     }).attr('readonly', 'readonly');
                 }
             }
             },
             { name: 'BunkName', index: 'BunkName', editable: true },
             { name: 'CreatedBy', index: 'CreatedBy' },
             { name: 'CreatedDate', index: 'CreatedDate' },
             { name: 'Delete', index: 'Delete', align: "center", sortable: false, formatter: frmtrDel },
                ],
        pager: pager_selector,
        rowNum: '100',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Asc',
        height: '230',
        autowidth: true,
        viewrecords: true,
        caption:'<i class="fa fa-th-list"></i>&nbsp;Fuel Refill List',
        gridComplete: function () {
            var rdata = $(grid_selector).getRowData();
            if (rdata.length > 0) {
                $('.T1CompDel').click(function () { DeleteComponentDtls($(this).attr('rowid')); });
            }
        },
        loadComplete: function () {
            var $this = $(this), rows = this.rows, l = rows.length, i, row;
            $(this).hide();
            for (i = 1; i < l; i++) {
                row = rows[i];
                if ($(row).hasClass("jqgrow")) {
                    $this.jqGrid('editRow', row.id);
                }
            }
            $(this).show();
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(grid_selector).jqGrid('setGridWidth');
        }
    });

    //navButtons Add, edit, delete
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
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

            },
            {},
            {}, {}, {})
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

    $("#btnBack").click(function () {
        window.location.href = "~/Transport/FuelRefillBulkEntryList/";
    });
});
    function frmtrDel(cellvalue, options, rowdata) {
        var delBtn = "";
        delBtn = "<span id='T1btnDel_" + options.rowId + "'class='ace-icon fa fa-trash-o red T1CompDel' rowid='" + options.rowId + "' title='Delete'></span>";
        return delBtn;
    }

    function DeleteComponentDtls(id) {

        if (confirm("Are you sure you want to delete this item?")) {
            DeleteComponentIds(
                '/Transport/DeleteFuelRefillId?Id=' + id, //delURL, 
                '/Transport/FuelRefillListBulkEntryJqGrid?RefId=' + $('#Id').val(), //reloadURL, 
                $(grid_selector) //GridId, 
                );
        }
    }

    function DeleteComponentIds(delURL, reloadURL, GridId) {
        $.ajax({
            url: delURL,
            type: 'POST',
            dataType: 'json',
            traditional: true,
            success: function (data) {
                LoadSetGridParam(GridId, reloadURL);
            },
            loadError: function (xhr, status, error) {
                msgError = $.parseJSON(xhr.responseText).Message;
                ErrMsg(msgError, function () { });
            }
        });
    }

    function ValidateAndSave() {
        var RowList;
        var selectedData;
        var DLst = '';
        RowList = $('#FuelRefillList').getDataIDs();
        Id = $("#Id").val();

        for (var i = 0, list = RowList.length; i < list; i++) {
            var selectedId = RowList[i];
            selectedData = $('#FuelRefillList').jqGrid('getRowData', selectedId);

            var FuelQuantity = $("#" + selectedId + "_FuelQuantity").val();
            if (isNaN(FuelQuantity)) {
                ErrMsg('Numbers only allowed.');
                $("#" + selectedId + "_FuelQuantity").focus();
                return false;
            }

            var FilledDate = $("#" + selectedId + "_FilledDate").val();

            var value = FilledDate.split("/");
            var dd = value[0];
            var mm = value[1];
            var yy = value[2];

            var resultFilledDate = mm + "/" + dd + "/" + yy;

            DLst += "&[" + i + "].Id=" + selectedData.Id
                + "&[" + i + "].RefId=" + selectedData.RefId
                + "&[" + i + "].VehicleId=" + encodeURIComponent(selectedData.VehicleId)
                + "&[" + i + "].Type=" + selectedData.Type
                + "&[" + i + "].VehicleNo=" + encodeURIComponent(selectedData.VehicleNo)
                + "&[" + i + "].FuelType=" + selectedData.FuelType
                + "&[" + i + "].FuelFillType=" + $("#" + selectedId + "_FuelFillType").val()
                + "&[" + i + "].FuelQuantity=" + $("#" + selectedId + "_FuelQuantity").val()
                + "&[" + i + "].LitrePrice=" + $("#" + selectedId + "_LitrePrice").val()
                + "&[" + i + "].TotalPrice=" + $("#" + selectedId + "_TotalPrice").val()
                + "&[" + i + "].LastMilometerReading=" + $("#" + selectedId + "_LastMilometerReading").val()
                + "&[" + i + "].CurrentMilometerReading=" + $("#" + selectedId + "_CurrentMilometerReading").val()
                + "&[" + i + "].Mileage=" + $("#" + selectedId + "_Mileage").val()
                + "&[" + i + "].FilledBy=" + $("#" + selectedId + "_DriverName").val()
                + "&[" + i + "].FilledDate=" + resultFilledDate
                + "&[" + i + "].BunkName=" + $("#" + selectedId + "_BunkName").val()
                + "&[" + i + "].Campus=" + $("#Campus").val()
        }
        $.ajax({
            url: "/Transport/CompleteFuelRefillList/",
            type: 'POST',
            dataType: 'json',
            data: DLst,
            success: function (data) {
                Id = $("#Id").val();
                // idsOfSelectedRows = [''];
                $("#FuelRefillList").setGridParam({ url: "/Transport/FuelRefillListBulkEntryJqGrid?RefId=" + Id }).trigger("reloadGrid");
            }
        });
    }
    function Complete() {
        var Id = $("#Id").val();
        if (Id == 0) {
            ErrMsg("Please Save");
            return false;
        }
        if (ValidateAndSave() == false) {
            return false;
        }
        else {
            if (confirm("Are you sure to Complete?")) {
                var vdc = {
                    Id: $("#Id").val()
                };
                $.ajax({
                    url: '/Transport/CompleteFuelRefill',
                    type: 'POST',
                    dataType: 'json',
                    data: vdc,
                    traditional: true,
                    success: function (data) {
                        if (data != "") {
                            InfoMsg(data + " is completed successfully", function () { window.location.href = '/Transport/ShowFuelRefill?Id=' + Id; });
                        }
                    },
                    error: function (xhr, status, error) {
                        ErrMsg($.parseJSON(xhr.responseText).Message);
                    }
                });
            }
            else {
                return false;
            }
        }
    }

    function CalculateMileage(rowId) {
        var FuelFillType = $("#" + rowId + "_FuelFillType").val();
        if (FuelFillType == "Full Tank") {
            var FuelQuantity = $("#" + rowId + "_FuelQuantity").val();
            var LastMilometerReading = $("#" + rowId + "_LastMilometerReading").val();
            var CurrentMilometerReading = $("#" + rowId + "_CurrentMilometerReading").val();
            var Distance = (parseFloat(CurrentMilometerReading) - parseFloat(LastMilometerReading));
            var Mileage = parseFloat(Distance) / parseFloat(FuelQuantity);
            $("#Distance").text(Distance);
            $("#" + rowId + "_Mileage").val(Mileage);
        }
        else {
            $("#" + rowId + "_Mileage").val('');
        }

    }

    function CalculateTotalPrice(rowId) {
        var FuelQuantity = $("#" + rowId + "_FuelQuantity").val();
        var LitrePrice = $("#" + rowId + "_LitrePrice").val();
        if (FuelQuantity != '' && LitrePrice != '')
            $("#" + rowId + "_TotalPrice").val(parseFloat(FuelQuantity) * parseFloat(LitrePrice));
        else
            $("#" + rowId + "_TotalPrice").val('');
    }
