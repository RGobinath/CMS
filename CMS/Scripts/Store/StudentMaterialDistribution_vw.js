$(function () {
    
    var grid_selector = '#jqGridStudentMaterialDistribution_vwList';
    var Pager_selector = '#jqGridStudentMaterialDistribution_vwListPager';

    var Materialgridddl = '/Store/FillMaterialSubGroup';
    var lastsel;
    $(grid_selector).jqGrid({
        url: '/Store/GetJqGridStudentMaterialDistribution_vwList',
        datatype: 'Json',
        mtype: 'GET',
        colNames: ['MaterialviewId', 'AcademicYear', 'Campus', 'Grade', 'Section', 'StudId', 'NewId', 'Name', 'Gender', 'IsHosteller', 'Material', 'Quantity', 'MaterialSubGroupId', 'Size', 'IssueId', 'StudentId', 'MaterialId', 'IssuedQty', 'ReceivedQty', 'PendingItems', 'ExtraQty', 'TotalQty', 'MaterialDistributionId'],
        colModel: [
                     { name: 'MaterialviewId', index: 'MaterialviewId', key: true, hidden: true, editable: true },
                     { name: 'AcademicYear', index: 'AcademicYear', sortable: false, editable: false, hidden: true },
                     { name: 'Campus', index: 'Campus', sortable: false, editable: false, hidden: true },
                    { name: 'Grade', index: 'Grade', sortable: false, editable: false, hidden: true },
                    { name: 'Section', index: 'Section', sortable: false, editable: false, hidden: true },
                    
                    
                    { name: 'StudId', index: 'StudId', hidden: true, editable: true },
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
                    {name: 'Material', index: 'Material', sortable: false,editable: true, edittype: 'select', editoptions: {
                                  dataUrl: '/Store/GetMaterialSubGroupddl',
                                  postData: function (rowid, value, matSubId) {
                                      return {MaterialSubGroupId:materialSubId}
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
                        name: 'IssuedQty', index: 'IssuedQty', sortable: false,editable: true,editrules: { required: true, number: true},
                        editoptions: {

                            dataInit: function (element) {
                                $(element).keyup(function () {

                                    var rowId = parseInt($(this).attr("id"));
                                    CalculateTotalPrice(rowId);
                                    Printvalue(rowId);
                                })
                            }
                        }, editrules: { required: true, custom: true, custom_func: checkvalid }
                    },
                        {
                            name: 'ReceivedQty', index: 'ReceivedQty', sortable: false, editable: true, editrules: { number: true },
                            editoptions: {
                                disabled: true,
                                dataInit: function (element) {
                                    $(element).keyup(function () {

                                        var rowId = parseInt($(this).attr("id"));

                                        CalculateTotalQty(rowId);

                                    })
                                }
                            }, editrules: { required: true, custom: true, custom_func: checkvalid }
                        },
                   


                    { name: 'PendingItems', index: 'PendingItems', sortable: false, editable: true, editoptions: { disabled: true, class: 'NoBorder' } },

                    {
                        name: 'ExtraQty', index: 'ExtraQty', sortable: false, editable: true,editrules: { required: true, number: true},
                        editoptions: {
                            dataInit: function (element) {
                                $(element).keyup(function () {
                                    var rowId = parseInt($(this).attr("id"));
                                    CalculateTotalQty(rowId);
                                })
                            }
                        }, editrules: { required: true, custom: true, custom_func: checkvalid }
                    },
                    { name: 'TotalQty', index: 'TotalQty', sortable: false, editable: true, editoptions: { disabled: true, class: 'NoBorder' } },
                     {
                         name: 'MaterialDistributionId', index: 'MaterialDistributionId', sortable: false, editable: true, hidden: true
                     },



        ],

        viewrecords: true,
        altRows: true,
        autowidth: true,
        multiselect: true,
        // multiboxonly: true,
        height: '220',
        rowNum: 1000,
        rowList: [5, 10, 20],
        sortName: 'Id',
        sortOrder: 'Asc',
        pager: Pager_selector,
        editurl: '/Store/AddOrEditMaterialIssueDetails',
        onSelectRow: function (id) {
           
            materialSubId = jQuery(grid_selector).jqGrid('getCell', id, 'MaterialSubGroupId');
            if (id && id !== lastsel) {
                jQuery(grid_selector).jqGrid('restoreRow', lastsel);
                jQuery(grid_selector).jqGrid('editRow', id, true);
                lastsel = id;
               
            }
        },
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        
        caption: '<i class="fa fa-th-list"></i>&nbsp; Student Meterial Details'
    });
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
                refresh: false,
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
function checkvalid(value, column) {
    obtndmrks = value;
    if (value == '') {
        return [false, column + ": Field is Required"];
    }
    else if (!$.isNumeric(value)) {
        return [false, column + 'Should be numeric'];
    }
    else if (parseInt(obtndmrks) > parseInt($("#totalmks").val())) {
        return [false, column + ' Should be lesser than Obtained marks'];
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
   
    if (parseInt(Quantity) >= parseInt(issuedQty)) {
        $("#" + rowId + "_PendingItems").val(parseInt($("#" + rowId + "_Quantity").val()) - parseInt($("#" + rowId + "_IssuedQty").val()));
    }
    else
        {
        $("#" + rowId + "_IssuedQty").val("");           
    }
    }
function CalculateTotalQty(rowId) {

    $("#" + rowId + "_TotalQty").val(parseInt($("#" + rowId + "_ExtraQty").val()) + parseInt($("#" + rowId + "_ReceivedQty").val()));
        }
function Printvalue(rowId) {
    
    $("#" + rowId + "_ReceivedQty").val(parseInt($("#" + rowId + "_IssuedQty").val()));
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


function FillMaterialSubGroup() {   
  
    var AcademicYear = $("#ddlAcademicYear").val();
    var Campus = $("#ddlCampus").val();
    var Grade = $("#ddlgrade").val();
    var Gender = $("#ddlGender").val();
    var IsHostler = $("#ddlIshosteller").val();    
    var ddlbc = $('#ddlMaterialSubGroup');
    if (AcademicYear != '' && Campus != '' && Grade != '' && Gender != '' && IsHostler!='') {
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
   


