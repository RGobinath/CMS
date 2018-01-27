
var srchgrid_selector = "#StoreMaterialsList";
var srchpager_selector = "#StoreMaterialsListPager";

var Listoutgrid_selector = "#StoreMaterialsList1";
var Listoutpager_selector = "#StoreMaterialsList1Pager";

$(function () {
    $(window).on('resize.jqGrid', function () {
        $(srchgrid_selector).jqGrid('setGridWidth', $("#DivMaterialSearch").width());
        $(Listoutgrid_selector).jqGrid('setGridWidth', $("#DivMaterialSearch").width());
    })

    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(srchgrid_selector).jqGrid('setGridWidth', parent_column.width());
                $(Listoutgrid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    var campus = $('#Campus').val();
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
            function (modelData) {
                var select = $("#ddlRequiredForGrade");
                select.empty();
                select.append($('<option/>', { value: "", text: "Select Grade" }));
                $.each(modelData, function (index, itemData) {
                    select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                });
            });
    $.getJSON("/Store/FillMaterialGroup",
                 function (fillig) {
                     var ddlmatgrp = $("#ddlMaterialGroup");
                     ddlmatgrp.empty();
                     ddlmatgrp.append($('<option/>',
                     {
                         value: "",
                         text: "Select One"
                     }));
                     $.each(fillig, function (index, itemdata) {
                         ddlmatgrp.append($('<option/>',
                             {
                                 value: itemdata.Value,
                                 text: itemdata.Text
                             }));
                     });
                 });
    var idsOfSelectedRows = [];
    var FromStore;
    if ($("#RequiredFromStore").val() != '')
        FromStore = $("#RequiredFromStore").val();
    else {
        var cam = $("#ddlReqForCamp").val();
        $.ajax({
            type: 'POST',
            async: false,
            dataType: "json",
            url: '/Store/FillStore?Campus=' + cam,
            success: function (data) {
                FromStore = data.rows[0].Value;
            }
        });
    }
    jQuery(srchgrid_selector).jqGrid({
        url: '/Store/StoreSKUListJqGrid?Store=' + FromStore,
        //url: '/Store/StoreSKUListJqGrid',
        datatype: 'json',
        mtype: 'GET',
        height: '150',
        //shrinkToFit: true,
        colNames: ['Id', 'Material Group', 'Material Sub Group', 'Material', 'Units', 'Store', 'Available Qty'],
        colModel: [
                { name: 'Id', index: 'Id', width: 50, align: 'left', editable: true, hidden: true, edittype: 'text', key: true },
                { name: 'MaterialGroup', index: 'MaterialGroup', width: 100, align: "left" },
                { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 100, align: 'left' },
                { name: 'Material', index: 'Material', width: 100, align: 'left' },
                { name: 'Units', index: 'Units', width: 100, align: 'left' },
                { name: 'Store', index: 'Store', width: 100, align: 'left' },
                { name: 'ClosingBalance', index: 'ClosingBalance', width: 100, align: 'left', cellattr: function (rowId, cellValue, rawObject, cm, rdata) {
                    if (cellValue == 0) {
                        return 'class="ui-state-error ui-state-error-text"';
                    }
                }
                },
                ],
        pager: srchpager_selector,
        rowNum: '5',
        rowList: [5, 10, 20, 50],
        sortname: 'Material',
        sortorder: "",
        viewrecords: true,
        multiselect: true,
        autowidth: true,
        caption: '<i class="fa fa-th-list">&nbsp;</i>Storer Materials List',
        onSelectRow: function (id, status) {
            var index = $.inArray(id, idsOfSelectedRows);
            if (!status && index >= 0) {
                idsOfSelectedRows.splice(index, 1); // remove id from the list
            } else if (index < 0) {
                idsOfSelectedRows.push(id);
            };
            var RowList1;
            var selectedData1;
            RowList1 = $(srchgrid_selector).getGridParam('selarrrow');
            var Id = $("#Id").val();
            selectedData1 = $(srchgrid_selector).jqGrid('getRowData', id);
            if (status == true) {
                if (parseInt(selectedData1.ClosingBalance) == 0) {
                    ErrMsg("Available quantity is 0");
                    return false
                }
                else {
                    $('#StoreMaterialsList1').jqGrid('addRowData', id, selectedData1);
                    jQuery('#StoreMaterialsList1').editRow(id, true);
                }
            }
            else {
                $('#StoreMaterialsList1').jqGrid('delRowData', id, selectedData1);
            }
        },
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            var $this = $(this), i, count;
            for (i = 0, count = idsOfSelectedRows.length; i < count; i++) {
                $this.jqGrid('setSelection', idsOfSelectedRows[i], false);
            }
        }
    });
    jQuery(srchgrid_selector).jqGrid('navGrid', srchpager_selector,
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
            {}, {}, {});
    $(srchgrid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () {
        $(srchgrid_selector).clearGridData();
        return false;
    }
    });

    $("#gs_Store").val(FromStore).attr("readonly", true);
    $("#gs_Material").autocomplete({
        source: function (request, response) {
            $.getJSON('/Store/GetMaterials?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });

    jQuery(Listoutgrid_selector).jqGrid({
        url: '/Store/EmptyJsonUrl',
        datatype: 'json',
        mtype: 'POST',
        height: '150',
        colNames: ['Id', 'Material Group', 'Material Sub Group', 'Material', 'Units', 'Available Qty', 'Required Qty', 'Required Date'],
        colModel: [
                                { name: 'Id', index: 'Id', width: 50, align: 'left', editable: true, hidden: true, edittype: 'text', key: true, sortable: false },
                                { name: "MaterialGroup", index: "MaterialGroup", width: 100, align: "left", editable: true, editoptions: { disabled: true, class: 'NoBorder'} },
                                { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 100, align: 'left', sortable: false, editable: true, editoptions: { disabled: true, class: 'NoBorder'} },
                                { name: 'Material', index: 'Material', width: 100, align: 'left', sortable: false, editable: true, editoptions: { disabled: true, class: 'NoBorder'} },
                                { name: 'Units', index: 'Units', width: 70, align: 'left', sortable: false, editable: true, editoptions: { readonly: true, class: 'NoBorder'} },
                                { name: 'ClosingBalance', index: 'ClosingBalance', width: 70 },
                                { name: 'Quantity', index: 'Quantity', width: 50, editable: true, edittype: 'text', editrules: { integer: true} },
                                { name: 'RequiredDate', index: 'RequiredDate',width: 100, editable: true, editoptions: {
                                    dataInit: function (el) {
                                        $(el).datepicker({ format: "dd/mm/yyyy",
                                            changeMonth: true,
                                            autoclose: true,
                                            timeFormat: 'hh:mm:ss',
                                            autowidth: true,
                                            changeYear: true,
                                            startDate: '+0d'
                                        }).attr('readonly', 'readonly');
                                    }
                                }
                                },
                                ],
        pager: Listoutpager_selector,
        rowNum: '50',
        rowList: [10, 20, 50, 100],
        sortname: 'Id',
        sortorder: "asc",
        viewrecords: true,
        autowidth: true,
        caption: '<i class="fa fa-th-list">&nbsp;</i> Selected Materials List',
        forceFit: true,
        gridview: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }
    });
    $("#btnSearch").click(function () {
        $(srchgrid_selector).clearGridData();
        var MaterialGroupId = $("#ddlMaterialGroup").val();
        var MaterialSubGroupId = $("#ddlMaterialSubGroup").val();
        var MaterialId = $("#ddlMaterial").val();
        var Store = $("#gs_Store").val();
        $(srchgrid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/StoreSKUListJqGrid/',
                    postData: { MaterialGroupId: MaterialGroupId, MaterialSubGroupId: MaterialSubGroupId, MaterialId: MaterialId, Units: '', Store: Store },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("#ddlMaterialGroup").val('');
        $("#ddlMaterialSubGroup").val('');
        $("#ddlMaterial").val('');
        $("#ddlRequiredForGrade").val('');
        $("#txtstName").val('');
        var Store = $("#gs_Store").val();
        $(srchgrid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/StoreSKUListJqGrid/',
                    postData: { MaterialGroupId: 0, MaterialSubGroupId: 0, MaterialId: 0, Units: '', Store: Store },
                    page: 1
                }).trigger("reloadGrid");
    });
    $(srchgrid_selector).jqGrid("clearGridData", true).trigger("reloadGrid");
    $(Listoutgrid_selector).jqGrid("clearGridData", true).trigger("reloadGrid");

    $("#btnSubmitAndClose").click(function () {
        if (ValidateSave() == false) {
            return false;
        }
        else {
            idsOfSelectedRows = [''];
            $(srchgrid_selector).jqGrid("clearGridData", true).trigger("reloadGrid");
            $(Listoutgrid_selector).jqGrid("clearGridData", true).trigger("reloadGrid");
            $('#DivMaterialSearch').dialog('close');
        }
    });

    $("#ddlMaterialGroup").change(function () {
        var matgrp = $("#ddlMaterialGroup").val();
        if (matgrp != "") {
            $.ajax({
                type: 'POST',
                async: false,
                url: '/Store/FillMaterialSubGroup?MaterialGroupId=' + matgrp,
                success: function (data) {
                    $("#ddlMaterialSubGroup").empty();
                    $("#ddlMaterialSubGroup").append("<option value=''> Select One </option>");
                    for (var i = 0; i < data.length; i++) {
                        $("#ddlMaterialSubGroup").append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                    }
                },
                dataType: "json"
            });
        }
        else {
            $("#ddlMaterialSubGroup").empty();
            $("#ddlMaterialSubGroup").append("<option value=''> Select One </option>");
        }
    });

    $("#ddlMaterialSubGroup").change(function () {
        var matgrp = $("#ddlMaterialGroup").val();
        var matsubgrp = $(this).val();
        if (matgrp != "") {
            $.ajax({
                type: 'POST',
                async: false,
                url: '/Store/FillMaterial?MaterialGroupId=' + matgrp + "&MaterialSubGroupId=" + matsubgrp,
                success: function (data) {

                    $("#ddlMaterial").empty();
                    $("#ddlMaterial").append("<option value=''> Select One </option>");
                    for (var i = 0; i < data.length; i++) {
                        $("#ddlMaterial").append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                    }
                },
                dataType: "json"
            });
        }
        else {
            $("#ddlMaterial").empty();
            $("#ddlMaterial").append("<option value=''> Select One </option>");
        }
    });


    $("#StudentSearch").click(function () {
        var grade = $("#ddlRequiredForGrade").val();
        var sect = $("#ddlSection").val();
        var stu = $('#Student').attr('checked') ? true : false;
        if (stu == true) {
            if (grade == "") {
                ErrMsg("Please select Grade");
                return false;
            }
            if (sect == "") {
                ErrMsg("Please select Section");
                return false;
            }
        }
        else { ErrMsg("Please select General or Student"); return false; }
        $("#campus").val(campus);
        $("#grade").val(grade);
        $("#section").val(sect);
        var PopupUrl = '/Store/StudentDetails?Campus=' + campus + '&Grade=' + grade + "&Section=" + sect;
        ModifiedLoadPopupDynamicaly(PopupUrl, $('#DivStudentSearch'),
            function () {
                LoadSetGridParam($('#StudentList'), "/Store/StudentDetailsListJqGrid?cname=" + campus + "&grade=" + grade + "&sect=" + sect)
            }, function () { }, 700, 900, "Student Details");
    });

//    $("#General").change(function () {
//        var GeneralVal = $('#General').attr('checked') ? true : false;
//        if (GeneralVal == true) {
//            $("#StudentSearch").hide();
//            $("#txtstName").val('');
//            $("#hdnRequestType").val("General");
//        }
//    });

//    $("#Student").change(function () {
//        var StudentVal = $('#Student').attr('checked') ? true : false;
//        if (StudentVal == true) {
//            $("#StudentSearch").show();
//            $("#hdnRequestType").val("Student");
//        }
//    });
});
function ValidateSave() {
    debugger;
    var RowList;
    var selectedData;
    var MatReqLst = '';
    var gen = $('#General').attr('checked') ? true : false;
    var stu = $('#Student').attr('checked') ? true : false;
    var ReqForGrade = $("#ddlRequiredForGrade").val();
    var reqfor = $("#txtstName").val();
    if (gen == "" && stu == "") {
        ErrMsg("Please Select General or Student.");
        return false;
    }
    //        else if (ReqForGrade == "") {
    //            ErrMsg("Please Select Required For Grade.");
    //            return false;
    //        }
    else if (stu == true && reqfor == "") {
        ErrMsg("Please Select Student Name.");
        return false;
    }
    RowList = $(Listoutgrid_selector).getDataIDs();
    var Id = $("#Id").val();
    var qty = '';
    var reqdate = '';
    for (var i = 0, list = RowList.length; i < list; i++) {
        var selectedId = RowList[i];
        qty = $("#" + selectedId + "_Quantity").val();
        reqdate = $("#" + selectedId + "_RequiredDate").val();
        if (qty == "") {
            ErrMsg("Please type Quantity");
            $("#" + selectedId + "_Quantity").focus();
            return false;
            break;
        }
        if (isNaN(qty)) {
            ErrMsg("Quantity should be in number");
            $("#" + selectedId + "_Quantity").focus();
            return false;
            break;
        }
        if (reqdate == "") {
            ErrMsg("Please select Required Date");
            $("#" + selectedId + "_RequiredDate").focus();
            return false;
            break;
        }
        var RequiredDate = $("#" + selectedId + "_RequiredDate").val();

        var value = RequiredDate.split("/");
        var dd = value[0];
        var mm = value[1];
        var yy = value[2];

        var resultField = mm + "/" + dd + "/" + yy;

        selectedData = $(Listoutgrid_selector).jqGrid('getRowData', selectedId);
        if (parseInt(qty) > parseInt(selectedData.ClosingBalance)) {
            ErrMsg("Request Quantity should not be greater than Available Quantity.");
            $("#" + selectedId + "_Quantity").focus().css("color", "#CD0A0A");
            return false;
            break;
        }
        MatReqLst += "&[" + i + "].MaterialGroup=" + encodeURIComponent($("#" + selectedId + "_MaterialGroup").val())
                + "&[" + i + "].MaterialSubGroup=" + encodeURIComponent($("#" + selectedId + "_MaterialSubGroup").val())
                + "&[" + i + "].Material=" + encodeURIComponent($("#" + selectedId + "_Material").val())
                + "&[" + i + "].Units=" + $("#" + selectedId + "_Units").val()
                + "&[" + i + "].RequestType=" + $("#hdnRequestType").val()
                + "&[" + i + "].RequiredForCampus=" + $("#ddlReqForCamp").val()
                + "&[" + i + "].RequiredForGrade=" + $("#ddlRequiredForGrade").val()
                + "&[" + i + "].Section=" + $("#ddlSection").val()
                + "&[" + i + "].RequiredFor=" + $("#txtstName").val()
                + "&[" + i + "].Quantity=" + $("#" + selectedId + "_Quantity").val()
                + "&[" + i + "].RequiredDate=" + resultField
                + "&[" + i + "].Status=" + 'Requested'
                + "&[" + i + "].MatReqRefId=" + Id
    }
    $.ajax({
        url: '/Store/AddMaterialRequestList',
        type: 'POST',
        dataType: 'json',
        data: MatReqLst,
        success: function (data) {
            var Id = $("#Id").val();
            $("#MaterialRequestList").setGridParam({ url: '/Store/MaterialRequestJqGrid?Id=' + Id }).trigger("reloadGrid");
            idsOfSelectedRows = [''];
            $(srchgrid_selector).jqGrid('resetSelection');
            $(Listoutgrid_selector).clearGridData();
        }
    });
    return true;
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