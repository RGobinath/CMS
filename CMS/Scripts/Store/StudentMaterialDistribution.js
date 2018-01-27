$(function () {

    var grid_selector = '#jqGridStudentMaterialDistribution_vwList';
    var Pager_selector = '#jqGridStudentMaterialDistribution_vwListPager';

    var Materialgridddl = '/Store/FillMaterialSubGroup';
    var lastsel;
    $(grid_selector).jqGrid({
        url: '/Store/GetJqGridStudentMaterialDistribution_vwList',
        datatype: 'Json',
        mtype: 'GET',
        colNames: ['MaterialviewId', 'Academic Year', 'Campus', 'Grade', 'Section', 'StudId', 'New Id', 'Name', 'Gender', 'Is Hosteller', 'Material', 'Actual Quantity', 'MaterialSubGroupId', 'Size', 'IssueId', 'StudentId', 'MaterialId', 'Issued Qty', 'Received Qty', 'Pending Items', 'Extra Issued Qty', 'Total Qty', 'MaterialDistributionId'],
        colModel: [
                     { name: 'MaterialviewId', index: 'MaterialviewId', key: true, hidden: true, editable: true },
                     { name: 'AcademicYear', index: 'AcademicYear', sortable: false, editable: false, hidden: true },
                     { name: 'Campus', index: 'Campus', sortable: false, editable: false, hidden: true },
                    { name: 'Grade', index: 'Grade', sortable: false, editable: false, hidden: true },
                    { name: 'Section', index: 'Section', sortable: false, editable: false, hidden: true },


                    { name: 'StudId', index: 'StudId', hidden: true, editable: true ,sortable:true},
                    { name: 'NewId', index: 'NewId', editable: false },
                    { name: 'Name', index: 'Name', sortable: false, editable: false },
                    { name: 'Gender', index: 'Gender', sortable: false, editable: false, hidden: true },
                    { name: 'IsHosteller', index: 'IsHosteller', sortable: false, editable: false },
                    { name: 'MaterialSubGroup', index: 'MaterialSubGroup', sortable: false, editable: false },
                    {
                        name: 'Quantity', index: 'Quantity', sortable: false, editable: true, editrules: { number: true },
                        editoptions: {
                            disabled: true,
                            dataInit: function (element) {
                                $(element).keyup(function () {

                                    var rowId = parseInt($(this).attr("id"));
                                    CalculateTotalPrice(rowId);
                                })
                            }
                        }, editrules: { required: true, custom: true, custom_func: checkvalid }
                    },

                    { name: 'MaterialSubGroupId', index: 'MaterialSubGroupId', hidden: true, editable: true },
                    {
                        name: 'Material', index: 'Material', sortable: false, editable: true, edittype: 'select', editrules: { required: true},editoptions: {
                            dataUrl: '/Store/GetMaterialSubGroupddl',
                            postData: function (rowid, value, matSubId) {
                                return { MaterialSubGroupId: materialSubId }
                            },
                            buildSelect: function (data) {

                                var materialName = jQuery.parseJSON(data);
                                var s = '<select>';
                                s += '<option value="">-Select-</option>';
                                if (materialName && materialName.length) {
                                    for (var i = 0, l = materialName.length; i < l; i++) {

                                        s += '<option value="' + materialName[i].Value + '">' + materialName[i].Text + '</option>';

                                    }
                                }
                                return s + "</select>";
                            }
                        }
                    },


                    { name: 'IssueId', index: 'IssueId', key: true, hidden: true, editable: true },
                     {
                         name: 'StudentId', index: 'StudentId', sortable: false, editable: true, hidden: true

                     },
                     {
                         name: 'MaterialId', index: 'MaterialId', sortable: false, editable: true, hidden: true
                     },
                    {
                        name: 'IssuedQty', index: 'IssuedQty', sortable: false, editable: true, editrules: { required: true, number: true },
                        editoptions: {

                            dataInit: function (element) {
                                $(element).keyup(function () {

                                    var rowId = parseInt($(this).attr("id"));
                                    CalculateTotalPrice(rowId);
                                    CalculateTotalQty(rowId);
                                    //Printvalue(rowId);
                                })
                            }
                        }, editrules: { required: true, custom: true, custom_func: checkvalid }
                    },
                        {
                            name: 'ReceivedQty', index: 'ReceivedQty', sortable: false, editable: false, hidden: true, editrules: { number: true },
                            editoptions: {
                                disabled: true,
                                dataInit: function (element) {
                                    $(element).keyup(function () {

                                        var rowId = parseInt($(this).attr("id"));

                                        CalculateTotalQty(rowId);

                                    })
                                }
                            },
                        },



                    { name: 'PendingItems', index: 'PendingItems', sortable: false, editable: true, editoptions: { disabled: true, class: 'NoBorder' } },

                    {
                        name: 'ExtraQty', index: 'ExtraQty', sortable: false, editable: true, editrules: {number: true },
                        editoptions: {
                            disabled: true, class: 'NoBorder',
                            dataInit: function (element) {
                                $(element).keyup(function () {
                                    var rowId = parseInt($(this).attr("id"));
                                    CalculateTotalPrice(rowId);
                                })
                            }
                        }, 
                    },
                    {
                        name: 'TotalQty', index: 'TotalQty', sortable: false, editable: true,
                        editoptions: {
                            disabled: true,
                            dataInit: function (element) {
                                $(element).keyup(function () {

                                    var rowId = parseInt($(this).attr("id"));                                 

                                })
                            }
                        }, 
                    },
                     {
                         name: 'MaterialDistributionId', index: 'MaterialDistributionId', sortable: false, editable: true, hidden: true
                     },
        ],
        viewrecords: true,
        altRows: true,
        autowidth: true,
        multiselect: true,
         multiboxonly: true,
        //loadonce:true,
        height: 180,
        rowNum: 1000,
        rowList: [5, 10, 20],
        sortName: 'Id',
        sortOrder: 'Asc',
        pager: Pager_selector,
        footerrow: true,
        editurl: '/Store/AddOrEditMaterialIssueDetails',
        onSelectRow: function (id) {            
            materialSubId = jQuery(grid_selector).jqGrid('getCell', id, 'MaterialSubGroupId');
            if (id && id !== lastsel) {
                jQuery(grid_selector).jqGrid('restoreRow', lastsel);
                jQuery(grid_selector).jqGrid('editRow', id, true, '', '', '', '', reload);
                lastsel = id;
            }
            debugger;
            selRowId = jQuery(grid_selector).jqGrid('getGridParam', 'selrow'),
            celValue = $("#" + selRowId + "_IssuedQty").val();
            if (celValue > 0) {
                jQuery(grid_selector).jqGrid('restoreRow', lastsel);
                jQuery(grid_selector).jqGrid('resetSelection');
                return false;
            }
        },
        loadComplete: function () {
            var table = this;
            $(grid_selector).footerData('set', { Material: 'Total Qtys' });
            var colTrip = $(grid_selector).jqGrid('getCol', 'IssuedQty', false, 'sum');
            $(grid_selector).footerData('set', { IssuedQty: colTrip });
            var PendingItems = $(grid_selector).jqGrid('getCol', 'PendingItems', false, 'sum');
            $(grid_selector).footerData('set', { PendingItems: PendingItems });
            var ExtraQty = $(grid_selector).jqGrid('getCol', 'ExtraQty', false, 'sum');
            $(grid_selector).footerData('set', { ExtraQty: ExtraQty });
            var TotalQty = $(grid_selector).jqGrid('getCol', 'TotalQty', false, 'sum');
            $(grid_selector).footerData('set', { TotalQty: TotalQty });
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        reloadAfterEdit: true,
        reloadAfterSubmit: true,
        onAfterSaveCell: reload,
        afterSubmit: function () {
            $(grid_selector).jqGrid("setGridParam", true).trigger("reloadGrid");
            //$("#jqGridProductListEmpty").jqGrid("clearGridData", true).trigger("reloadGrid");
            return [true];
        },
        caption: '<i class="fa fa-th-list"></i>&nbsp; Student Material Details',

        serializeRowData: function (postdata) {
            
            if (postdata.issuedQty != 0) {
                return postdata;
            } else {
                return null;
            }
        }
        
    });
    
   // jQuery("#jqGridStudentMaterialDistribution_vwList").reload();
    jQuery(grid_selector).jqGrid('navGrid', Pager_selector,
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
            {}, //Edit
            {}, //Add
            {},
            {},
            {})
    jQuery(grid_selector).jqGrid('navButtonAdd', Pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            window.open("ExportToExcelStudentMaterialDistribution" + '?rows=9999');
            //window.open("ExportToExcelStudentMaterialDistribution");
            //window.open("ExportToExcel");
        }
    });
    //jQuery(grid_selector).jqGrid('editRow', rowid, keys, oneditfunc, successfunc, url, extraparam, aftersavefunc, errorfunc, afterrestorefunc);
    $("#btnSearch").click(function () {

        $(grid_selector).clearGridData();
        var Name = $("#txtName").val();

        var AcademicYear = $("#ddlAcademicYear").val();
        var Campus = $("#ddlCampus").val();
        var Grade = $("#ddlgrade").val();
        var Section = $("#ddlSection").val();
        var Gender = $("#ddlGender option:selected").val();
        var IsHostler = $("#ddlIshosteller option:selected").val();
        var Material = $('#ddlMaterialSubGroup option:selected').text();

        Material = Material == "Select One" ? "" : Material;

        if (AcademicYear == '' || Campus == '' || Grade == '' || Gender == '' || IsHostler == '') {
            ErrMsg("Please fill all the mandatory fields.");
            return false;
        }

        $(grid_selector).setGridParam(
                {

                    datatype: "json",
                    url: '/Store/GetJqGridStudentMaterialDistribution_vwList/',

                    postData: { Name: Name, AcademicYear: AcademicYear, Campus: Campus, Grade: Grade, Gender: Gender, Section: Section, IsHosteller: IsHostler, MaterialSubGroup: Material },
                    page: 1
                }).trigger("reloadGrid");
    });

    $('#ddlCampus').change(function () {
        if ($('#ddlCampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        gradeddl();
    });
    $('#ddlAcademicYear,#ddlCampus,#ddlgrade,#ddlGender,#ddlIshosteller').change(function () {

        if ($('#ddlIshosteller').val() == "") {
            ErrMsg("Please fill the Is Hosteller");
            return false;
        }
        FillMaterialSubGroup();
    });

    $("#btnReset").click(function () {

        $("input[type=text], textarea, select").val("");


        $(grid_selector).setGridParam(
            {

                datatype: "json",
                url: '/Store/GetJqGridStudentMaterialDistribution_vwList/',
                postData: { Name: "", AcademicYear: "", Campus: "", Grade: "", Gender: "", IsHosteller: "", Quantity: "" },
                page: 1
            }).trigger("reloadGrid");
    });
});
function reload(result) {
    $("#jqGridStudentMaterialDistribution_vwList").trigger("reloadGrid");
}
function checkvalid(value, column) {
    //obtndmrks = value;
    if (value == '') {
        return [false, column + ": Field is Required"];
    }
    else if (value == 0)
    {
        return [false, column + ": Field is must be greater then zero"];
            }
    else if (!$.isNumeric(value)) {
        return [false, column + 'Should be numeric'];
    }
    else {
        return [true];
    }
}
function gradeddl() {

    var e = document.getElementById('ddlCampus');
    var campus = e.options[e.selectedIndex].value;
    //alert(campus);
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
                function (modelData) {
                    var select = $("#ddlgrade");
                    select.empty();
                    select.append($('<option/>', { value: "", text: "Select Grade" }));
                    $.each(modelData, function (index, itemData) {
                        select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                    });
                });
}
function nildata(cellvalue, options, rowObject) {
    if ((cellvalue == '') || (cellvalue == null)) {
        return ''
    }
    else {
        cellvalue = cellvalue.replace('&', 'and');
        //str = str.replace(/\&lt;/g, '<');
        return cellvalue
    }
}
function CalculateTotalPrice(rowId) {

    $("#" + rowId + "_IssuedQty").keydown(function (e) {

        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    var Quantity = $("#" + rowId + "_Quantity").val();
    var issuedQty = $("#" + rowId + "_IssuedQty").val();

    if (issuedQty != "") {
        if (parseInt(Quantity) >= parseInt(issuedQty)) {
            $("#" + rowId + "_PendingItems").val(parseInt($("#" + rowId + "_Quantity").val()) - parseInt($("#" + rowId + "_IssuedQty").val()));
            $("#" + rowId + "_ExtraQty").val(0);
        }
        else {
            $("#" + rowId + "_ExtraQty").val(parseInt($("#" + rowId + "_IssuedQty").val()) - parseInt($("#" + rowId + "_Quantity").val()));
            $("#" + rowId + "_PendingItems").val(0);
        }
    }
    else {
        //$("#" + rowId + "_IssuedQty").val('');
        $("#" + rowId + "_PendingItems").val('');
        $("#" + rowId + "_ExtraQty").val('');
        $("#" + rowId + "_TotalQty").val('');
    }
}
function CalculateTotalQty(rowId) {
    if ($("#" + rowId + "_IssuedQty").val() != "") {
        $("#" + rowId + "_TotalQty").val(parseInt($("#" + rowId + "_IssuedQty").val()));
    }
    else {
        $("#" + rowId + "_TotalQty").val('');
    }

    //$("#" + rowId + "_TotalQty").val(parseInt($("#" + rowId + "_ExtraQty").val()) + parseInt($("#" + rowId + "_ReceivedQty").val()));
}
//function Printvalue(rowId) {

//    $("#" + rowId + "_ReceivedQty").val(parseInt($("#" + rowId + "_IssuedQty").val()));
//}
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
function FillMaterialSubGroup() {
    var AcademicYear = $("#ddlAcademicYear").val();
    var Campus = $("#ddlCampus").val();
    var Grade = $("#ddlgrade").val();
    var Gender = $("#ddlGender").val();
    var IsHostler = $("#ddlIshosteller").val();
    var ddlbc = $('#ddlMaterialSubGroup');
    if (AcademicYear != '' && Campus != '' && Grade != '' && Gender != '' && IsHostler != '') {
        $.getJSON("/Store/GetStudentMaterialSearch?AcademicYear=" + AcademicYear + '&Campus=' + Campus + '&Grade=' + Grade + '&Gender=' + Gender + '&IsHosteller=' + IsHostler,
            function (fillbc) {
                ddlbc.empty();
                ddlbc.append($('<option/>', { value: "", text: "Select One" }));
                $.each(fillbc, function (index, itemdata) {
                    ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                });
            });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
$('#btnAdd').click(function () {
   // alert();

    ModifiedLoadPopupDynamicaly("/Store/StudentMaterialOverView", $('#StudentMateriaOverViewReport'),
            function () { LoadSetGridParam($('#StudentMateriaOverViewReport')) }, function () { }, 1200, 600, "Student Material Overview Report");
    //}
});
function StudentMaterialListPopup(StudId) {
    //LoadPopupDynamicaly('/Store/MaterialIssueDetails?StudId=' + StudId, $('#StudentMaterialList'),
    //                    function () {
    //                    },function(){}, '', 900, 500, "Material Details");

    LoadPopupDynamicaly("/Store/MaterialIssueDetails?StudId=" + StudId, $('#StudentMaterialList'),
        function () {
            LoadSetGridParam($('#StudentMaterialList'))
        },
    function () { }, 900, 500, "Student Material List");

    //ModifiedLoadPopupDynamicaly("/Asset/AddNewITAsset?AssetId=" + $('#ddlAssetType').val(), $('#newITAsset'), function () {
    //    LoadSetGridParam($('#newITAsset'))
    //}, function () { }, 400, 500, "Add Asset");
}



