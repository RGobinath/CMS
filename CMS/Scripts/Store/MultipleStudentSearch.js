
var studentgrid_selector = "#StudentList";
var studentpager_selector = "#StudentPager";

var StudentMatgrid_selector = "#StudentMaterialList";
var StudentMatpager_selector = "#StudentMaterialListPager";

$(function () {
    $(window).on('resize.jqGrid', function () {
        $(studentgrid_selector).jqGrid('setGridWidth', $("#DivMultipleStudentSearch").width());
        $(StudentMatgrid_selector).jqGrid('setGridWidth', $("#DivMultipleStudentSearch").width());
    })

    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(studentgrid_selector).jqGrid('setGridWidth', parent_column.width());
                $(StudentMatgrid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    var campus = $('#Campus').val();
    $.getJSON("/Store/FillMaterialGroup",
                 function (fillig) {
                     var ddlmatgrp = $("#ddlMaterialGroup");
                     ddlmatgrp.empty();
                     ddlmatgrp.append($('<option/>', { value: "", text: "Select One" }));
                     $.each(fillig, function (index, itemdata) {
                         ddlmatgrp.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                     });
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
    var SelectedRowIds = [];

    jQuery(studentgrid_selector).jqGrid({
        url: '/Store/MultipleStudentDetailsListJqGrid?Campus=' + $('#Campus').val(),
        //postData: { idno: idno, name: name, cname: cname, grade: grade, sect: section },
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'New Id', 'Name', 'Campus', 'Grade', 'Section', 'Is Hosteller'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true, key: true },
              { name: 'NewId', index: 'NewId', width: 110, sortable: true },
              { name: 'Name', index: 'Name', sortable: true },
              { name: 'Campus', index: 'Campus', sortable: true },
              { name: 'Grade', index: 'Grade', sortable: true },
              { name: 'Section', index: 'Section', sortable: true },
              { name: 'IsHosteller', index: 'IsHosteller', formatter: showYesOrNo, width: 50, sortable: true }
              ],
        pager: studentpager_selector,
        rowNum: '50',
        rowList: [50, 100, 150, 200],
        sortname: 'Name',
        sortorder: 'asc',
        //width: 1000,
        height: '150',
        autowidth: true,
        forceFit: true,
        viewrecords: true,
        caption: '<i class="fa fa-th-list">&nbsp;</i> Student List',
        multiselect: true,
        onSelectRow: function (id, status) {
            //debugger;
            if ($("#ddlMaterialGroup").val() == '' || $("#ddlMaterialSubGroup").val() == '') {
                ErrMsg("Please select Material Group and Sub Group");
                return false;
            }
            var index = $.inArray(id, SelectedRowIds);
            if (!status && index >= 0) {
                SelectedRowIds.splice(index, 1); // remove id from the list
            } else if (index < 0) {
                SelectedRowIds.push(id);
            };
            //debugger;
            var e = document.getElementById("ddlMaterialGroup");
            var MaterialGroup = e.options[e.selectedIndex].text;
            var f = document.getElementById("ddlMaterialSubGroup");
            var MaterialSubGroup = f.options[f.selectedIndex].text;
            selectedData1 = $(studentgrid_selector).jqGrid('getRowData', id);
            if (status == true) {
                $(StudentMatgrid_selector).jqGrid('addRowData', id, selectedData1);
                jQuery(StudentMatgrid_selector).jqGrid('setCell', id, "Name", selectedData1.Name);
                jQuery(StudentMatgrid_selector).jqGrid('setCell', id, "Campus", selectedData1.Campus);
                jQuery(StudentMatgrid_selector).jqGrid('setCell', id, "Grade", selectedData1.Grade);
                jQuery(StudentMatgrid_selector).jqGrid('setCell', id, "Section", selectedData1.Section);
                jQuery(StudentMatgrid_selector).jqGrid('setCell', id, "MaterialGroup", MaterialGroup);
                jQuery(StudentMatgrid_selector).jqGrid('setCell', id, "MaterialSubGroup", MaterialSubGroup);
                jQuery(StudentMatgrid_selector).editRow(id, true);
            }
            else {
                $(StudentMatgrid_selector).jqGrid('delRowData', id, selectedData1);
            }
        },
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            var $this = $(this), i, count;
            for (i = 0, count = SelectedRowIds.length; i < count; i++) {
                $this.jqGrid('setSelection', SelectedRowIds[i], false);
            }
        }
    });


    $(studentgrid_selector).jqGrid('filterToolbar', { searchOnEnter: true, beforeSearch: function () {
        $(studentgrid_selector).clearGridData();
        return false;
    }
    });
    $("#gs_Campus").val($('#Campus').val()).attr("readonly", true);
    $("#gs_Name").autocomplete({
        source: function (request, response) {
            $.getJSON('/Store/FillAutoCompleteStudentName?term=' + request.term + '&Campus=' + $('#Campus').val() + '&Grade=' + $("#gs_Grade").val(), function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 1
    });
    var s = '<select>';
    jQuery(StudentMatgrid_selector).jqGrid({
        url: '/Store/EmptyJsonUrl',
        datatype: 'local',
        mtype: 'GET',
        height: '150',
        // width: '1000',
        colNames: ['Id', 'Student Name', 'Campus', 'Grade', 'Section', 'Material Group', 'Material Sub Group', 'Material', 'Units', 'Required Qty', 'Required Date'],
        colModel: [{ name: 'Id', index: 'Id', width: 50, align: 'left', editable: true, hidden: true, edittype: 'text', key: true, sortable: false },
                   { name: 'Name', index: 'Name' },
                   { name: 'Campus', index: 'Campus' },
                   { name: 'Grade', index: 'Grade' },
                   { name: 'Section', index: 'Section' },
                   { name: "MaterialGroup", index: "MaterialGroup", width: 100, align: "left" },
                   { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 100, align: 'left' },
                                { name: 'Material', index: 'Material', width: 250, align: 'left', sortable: false, editable: true,
                                    editoptions: {
                                        dataUrl: '/Store/FillMaterialByMaterialGroupIdAndSubGroupId',
                                        dataInit: function (el) {
                                            $(el).autocomplete({ source: function (request, response) {
                                                $.getJSON('/Store/FillMaterialByMaterialGroupIdAndSubGroupId?term=' + request.term
                                                + '&MaterialGroupId=' + $("#ddlMaterialGroup").val()
                                                + '&MaterialSubGroupId=' + $("#ddlMaterialSubGroup").val(),
                                                function (data) {
                                                    response(data);
                                                });
                                            },
                                                minLength: 1,
                                                delay: 0
                                            })
                                        }
                                    }
                                },
                   { name: 'Units', index: 'Units', width: 90, editable: true, editrules: { required: true },
                       edittype: 'select',
                       editoptions: {
                           dataUrl: '/Store/Unitsddl',
                           buildSelect: function (data) {
                               jqGridMaterialGroup = jQuery.parseJSON(data);
                               var s = '<select>';
                               s += '<option value=" ">----Select----</option>';
                               if (jqGridMaterialGroup && jqGridMaterialGroup.length) {
                                   for (var i = 0, l = jqGridMaterialGroup.length; i < l; i++) {
                                       var mg = jqGridMaterialGroup[i];
                                       s += '<option value="' + mg + '">' + mg + '</option>';
                                   }
                               }
                               return s + "</select>";
                           }
                       }},
                   { name: 'Quantity', index: 'Quantity', width: 50, editable: true, edittype: 'text', editrules: { integer: true} },
                   { name: 'RequiredDate', index: 'RequiredDate', editable: true, editoptions: {
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
                   },],
        pager: StudentMatpager_selector,
        rowNum: '50',
        rowList: [50, 100, 200, 300],
        sortname: 'Id',
        sortorder: "asc",
        viewrecords: true,
        autowidth: true,
        caption: 'Selected Materials List',
        forceFit: true,
        gridview: true
    });
    $("#btnSubmitAndClose").click(function () {
        if (ValidateSave() == false) {
            return false;
        }
        else {
            SelectedRowIds = [''];
            $(studentgrid_selector).jqGrid("clearGridData", true).trigger("reloadGrid");
            $(StudentMatgrid_selector).jqGrid("clearGridData", true).trigger("reloadGrid");
            $('#DivMultipleStudentSearch').dialog('close');
        }
    });
});
function ValidateSave() {
    debugger;
    var RowList;
    var selectedData;
    var MatReqLst = '';

    RowList = $(StudentMatgrid_selector).getDataIDs();
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

//        var value = RequiredDate.split("/");
//        var dd = value[0];
//        var mm = value[1];
//        var yy = value[2];

//        var resultField = mm + "/" + dd + "/" + yy;

        selectedData = $(StudentMatgrid_selector).jqGrid('getRowData', selectedId);

        MatReqLst += "&[" + i + "].MaterialGroup=" + selectedData.MaterialGroup
                + "&[" + i + "].MaterialSubGroup=" + selectedData.MaterialSubGroup
                + "&[" + i + "].Material=" + encodeURIComponent($("#" + selectedId + "_Material").val())
                + "&[" + i + "].Units=" + $("#" + selectedId + "_Units").val()
                + "&[" + i + "].RequestType=" + "Student"
                + "&[" + i + "].RequiredForCampus=" + selectedData.Campus
                + "&[" + i + "].RequiredForGrade=" + selectedData.Grade
                + "&[" + i + "].Section=" + selectedData.Section
                + "&[" + i + "].RequiredFor=" + selectedData.Name
                + "&[" + i + "].Quantity=" + $("#" + selectedId + "_Quantity").val()
                + "&[" + i + "].RequiredDate=" + RequiredDate
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
            SelectedRowIds = [''];
            $(studentgrid_selector).jqGrid('resetSelection');
            $(StudentMatgrid_selector).clearGridData();
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