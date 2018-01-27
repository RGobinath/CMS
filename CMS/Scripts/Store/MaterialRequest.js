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
    $.getJSON("/Store/FillMaterialGroup",
                 function (fillig) {
                     var ddlmatgrp = $("#ddlMaterialGroup");
                     ddlmatgrp.empty();
                     ddlmatgrp.append($('<option/>', { value: "", text: "Select One" }));

                     $.each(fillig, function (index, itemdata) {
                         ddlmatgrp.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                     });
                 });
    $("#ddlRequiredForStore").change(function () {
        
        var subStore = $("#ddlRequiredForStore").val();
        alert(subStore);
        if (subStore != "") {
            $.ajax({
                type: 'POST',
                async: false,
                dataType: "json",
                url: '/Store/FillMainStoreBySubStore?SubStore=' + subStore,
                success: function (data) {
                    $("#ddlRequiredFromStore").empty();
                    $("#ddlRequiredFromStore").append($('<option/>', { value: '', text: 'Select One' }));
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].Text == $('#RequiredFromStore').val()) {
                            $("#ddlRequiredFromStore").append("<option value='" + data[i].Value + "' selected='selected'>" + data[i].Text + "</option>");
                        }
                        else {
                            $("#ddlRequiredFromStore").append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                        }
                    }

                }
            });
        }
    });



    $.getJSON("/Store/FillDepartment",
     function (fillig) {
         var ddldep = $("#ddlDepartment");
         ddldep.empty();
         ddldep.append($('<option/>', { value: "", text: "Select One" }));
         if ($('#Department').val() == "Academics") { ddldep.append("<option value='Academics' selected='selected'>Academics</option>"); }
         else { ddldep.append("<option value='Academics'>Academics</option>"); }
         $.each(fillig, function (index, itemdata) {
             if (itemdata.Text == $('#Department').val()) {
                 ddldep.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
             }
             else {
                 if ($('#Flag').val() == "Show") {
                     ddldep.append("<option value='Store' selected='selected'>Store</option>");
                     return false;
                 }
                 else {
                     ddldep.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                 }
             }
         });
     });

    //$("#MaterialSearch1").button({ icons: { primary: "ui-icon-search"} });
    //$.getJSON("/Base/FillAllBranchCode",
    // function (fillbc) {
    //     var ddlbc = $("#ddlReqForCamp");
    //     ddlbc.empty();
    //     ddlbc.append($('<option/>', { value: "", text: "Select One" }));

    //     $.each(fillbc, function (index, itemdata) {
    //         if (itemdata.Text == $('#RequiredForCampus').val()) {
    //             ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
    //         }
    //         else {
    //             ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
    //         }
    //     });
    // });

    var Id = ($("#Id").val());
    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialRequestJqGrid?Id=' + Id,
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Req.Type', 'Grade', 'Section', 'Req.For', 'Material', 'Material Group', 'Material Sub Group', 'Units', 'Req.Date', 'Status', 'Req.Qty', 'App.Qty', 'Issued.Qty', ''],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true, editable: false, key: true },
              { name: 'RequestType', index: 'RequestType', width: 50, sortable: true },
              { name: 'RequiredForGrade', index: 'RequiredForGrade', width: 50, sortable: true },
              { name: 'Section', index: 'Section', width: 50, sortable: true },
              { name: 'RequiredFor', index: 'RequiredFor', width: 100, sortable: true },
              { name: 'Material', index: 'Material', width: 90, sortable: true, editoptions: { class: 'NoBorder' }, cellattr: function (rowId, val, rawObject) { return 'title="' + 'Material Group:' + rawObject[6] + ', Material Sub Group:' + rawObject[7] + '"' } },
              { name: 'MaterialGroup', index: 'MaterialGroup', width: 90, sortable: true, editoptions: { class: 'NoBorder' }, hidden: true },
              { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 90, sortable: true, editoptions: { class: 'NoBorder' }, hidden: true },
              { name: 'Units', index: 'Units', width: 40 },
              { name: 'RequiredDate', index: 'RequiredDate', width: 90, sortable: true },
              { name: 'Status', index: 'Status', width: 90, sortable: true },
              { name: 'Quantity', index: 'Quantity', width: 50, sortable: true },
              { name: 'ApprovedQty', index: 'ApprovedQty', width: 50, sortable: true },
              { name: 'IssuedQty', index: 'IssuedQty', width: 50, sortable: true, editrules: { required: true, integer: true } },
              { name: 'Delete', index: 'Delete', width: 30, align: "center", sortable: false, formatter: frmtrDel }
        ],
        pager: pager_selector,
        rowNum: '50',
        rowList: [5, 10, 20, 50, 100],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '180',
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="fa fa-th-list">&nbsp;</i>Material Request List',
        forceFit: true,
        editurl: '/Store/UpdateMaterialRequestList',
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
                    $this.jqGrid('editRow', row.id);
                }
            }
            $(this).show();
        },
        gridComplete: function () {
            var rdata = $(grid_selector).getRowData();
            // alert(rdata.length);
            if (rdata.length > 0) {
                $('.T1CompDel').click(function () { DeleteComponentDtls($(this).attr('rowid')); });
            }
        }
    });
    jQuery(grid_selector).jqGrid('navGrid', pager_selector, {
        //navbar options
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
    //////////////  
    $(".Store").hide();
    if ($('#Flag').val() == "Show") {
        $(".Store").show();
        $("#RequiredForStore").val($('#Campus').val());
        $("ddlRequiredForStore").val($('#Campus').val());
        $("#ddlReqForCamp").val($('#Campus').val());
    }
    else
        $(".Store").hide();
    ////////////////////////

    if ($('#General').attr('checked'))
        $("#StudentSearch").hide();
    else $("#StudentSearch").show();

    $("#btnSave").click(function () {

        if (SaveValidation() == false) {
            return false;
        }
        else {
            var RequiredForCampus = $("#ddlReqForCamp").val();
            var RequestorDescription = $("#txtDescription").val();
            var Department = $("#ddlDepartment").val();
            var RequiredForStore = $("#ddlRequiredForStore").val();
            var RequiredFromStore = $("#ddlRequiredFromStore").val();
            var objMatReq = {
                Id: $("#Id").val(),
                RequestNumber: $("#RequestNumber").val(),
                RequestedDate: $("#RequestedDate").val(),
                ProcessedBy: $("#ProcessedBy").val(),
                RequestStatus: $("#RequestStatus").val(),
                UserRole: $("#UserRole").val(),
                Campus: $("#Campus").val(),
                RequestorDescription: RequestorDescription,
                RequiredForCampus: RequiredForCampus,
                Department: Department,
                RequiredForStore: RequiredForStore,
                RequiredFromStore: RequiredFromStore
            };
            $.ajax({
                url: '/Store/StartMaterialRequest',
                type: 'POST',
                dataType: 'json',
                data: objMatReq,
                traditional: true,
                success: function (data) {
                    var aa = data.toString();
                    var result = [2];
                    result = aa.split(',');
                    $("#RequestNumber").val(result[0]);
                    $("#InstanceId").val(result[1]);
                    $("#btnSave").hide();
                    $('#Id').val(result[0].substring(result[0].lastIndexOf("-") + 1, result[0].length));
                },
                error: function (xhr, status, error) {
                    ErrMsg($.parseJSON(xhr.responseText).Message);
                }
            });
        }
    });
    $("#btnSubmit").click(function () {
        if (SaveValidation() == false) {
            return false;
        }
        else {
            if ($('#Id').val() == 0) { ErrMsg("Please Save the request"); return false; }
            else {
                RowList = $('#MaterialRequestList').getDataIDs();
                if (RowList == "") {
                    ErrMsg("Please add Material. Empty request cannot be submitted");
                    return false;
                }
            }
        }
    });
    $("#ddlMaterialGroup").change(function () {
        $("#ddlMaterialSubGroup").change();
        var matgrp = $("#ddlMaterialGroup").val();
        if (matgrp != "") {
            $.ajax({
                type: 'POST',
                async: false,
                url: '/Store/FillMaterialSubGroup?MaterialGroup=' + matgrp,
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
                url: '/Store/FillMaterial?MaterialGroup=' + matgrp + "&MaterialSubGroup=" + matsubgrp,
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

    $("#SelectType").change(function () {
        if (($(this).val() == "Multiple")) {
            var BranchCode = $("#ddlReqForCamp").val();
            ModifiedLoadPopupDynamicaly("/Store/MutipleStudentSearch?Campus=" + BranchCode, $('#DivMultipleStudentSearch'),
            function () {
                LoadSetGridParam($('#StudentList'), "/Store/MultipleStudentDetailsListJqGrid?Campus=" + BranchCode)
            }, function () { }, 1000, 700, "Student Details");
        }
    });
    $("#MaterialSearch1").click(function () {
        var Id = $("#Id").val();
        var BranchCode = $("#ddlReqForCamp").val();
        if (BranchCode == "") {
            ErrMsg("Please select Required for Campus");
            return false;
        }
        if (Id == 0) {
            ErrMsg("Please Save");
            return false;
        }
        var RequiredFromStore = $("#ddlRequiredFromStore").val();
        ModifiedLoadPopupDynamicaly("/Store/MaterialRequestMaterialSearch?Campus=" + BranchCode, $('#DivMaterialSearch'),
            function () { LoadSetGridParam($('#StoreMaterialsList'), "/Store/StoreSKUListJqGrid") }, function () { }, 1100, 700, "Material Details");
    });
    if ($("#Id").val() != 0) {
        $("#btnSave").hide();
    }
    $("#txtQuantity").keyup(function () {
        var qty = $("#txtQuantity").val();
        if (isNaN(qty)) {
            ErrMsg("Numbers only allowed for Quantity");
            return false;
        }
        else if (qty > 100) {
            ErrMsg("Quantity should not exceed 100.");
            return false;
        }
    });
    $("#btnBack").click(function () {
        window.location.href = $('#BackUrl').val(); // '@Url.Action("MaterialRequestList", "Store")';
    });
});
function frmtrDel(cellvalue, options, rowdata) {
    var delBtn = "";
    delBtn = "<i id='T1btnDel_" + options.rowId + "'class='ace-icon fa fa-trash-o red T1CompDel' rowid='" + options.rowId + "' title='Delete'></i>";
    return delBtn;
}
function SaveValidation() {
    if ($("#ddlReqForCamp").val() == "") {
        ErrMsg("Please Select Required For Campus");
        return false;
    }
    else if ($("#ddlDepartment").val() == "") {
        ErrMsg("Please Select Department");
        return false;
    }
    else if ($("#txtDescription").val() == "") {
        ErrMsg("Please type Requester remarks");
        $("#txtDescription").focus();
        return false;
    }
    else if ($('#Flag').val() == "Show") {
        if ($("#ddlRequiredForStore").val() == "") {
            ErrMsg("Please Select Required for store");
            return false;
        }
        else if ($("#ddlRequiredFromStore").val() == "") {
            ErrMsg("Please Select Required from store");
            return false;
        }
        else return true;
    }
    else
        return true;
}
function DeleteComponentDtls(id) {

    if (confirm("Are you sure you want to delete this item?")) {
        DeleteComponentIds(
                '/Store/DeleteMaterialRequestList?id=' + id, //delURL, 
                '/Store/MaterialRequestJqGrid?Id=' + $('#Id').val(), //reloadURL, 
                $("#MaterialRequestList") //GridId, 
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