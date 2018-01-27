
var idno = "";
var name = "";
var cname = "";
var grade = "";
var section = "";
var ret1 = "";

function gridload() {
    $("#StudentList").jqGrid({
        //url: 'Url.Content("~/Home/StudentDetailsListJqGrid")',
        postData: { idno: idno, name: name, cname: cname, grade: grade, sect: section },
        datatype: 'local',
        type: 'GET',
        //shrinkToFit: true,
        colNames: ['Id', 'IdNo', 'Name', 'Section', 'Campus Name', 'Grade', 'Is Hosteller', 'Boarding Type', 'Email'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'id_no', index: 'id_no', width: 110, sortable: true },
              { name: 'name', index: 'name', sortable: true },
              { name: 'section', index: 'section', width: 50, sortable: true },
              { name: 'campus_name', index: 'campus_name', width: 110, sortable: true },
              { name: 'grade', index: 'grade', width: 50, sortable: true },
              { name: 'IsHosteller', index: 'IsHosteller', formatter: showYesOrNo, width: 50, sortable: true },
              { name: 'BoardingType', index: 'BoardingType', sortable: true },
              { name: 'EmailId', index: 'EmailId', sortable: true }
              ],
        pager: '#StudentPager',
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        //autowidth: true,
        height: 150,
        autowidth: true,
        viewrecords: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                //enableTooltips(table);
            }, 0);
        },
        caption: '<i class="fa fa-list"></i> Student List',
        onSelectRow: function (rowid) {
            if (rowid == null) {
                ids = 0;
                ret1 = jQuery("#StudentList").jqGrid('getRowData', rowid);
                if ($('#ddlIssueGroup option:selected').val() == "Academics-PYP") {
                    if (ret1.grade == 'I' || ret1.grade == 'II' || ret1.grade == 'III' || ret1.grade == 'IV' || ret1.grade == 'V') { updateStudentInfo(ret1); return true; }
                    else { ErrMsg("Please change Issue Group as Academics-PYP for selected Student"); $('#ddlIssueGroup').val(""); GetIssueType(); return false; }
                }
                else if ($('#ddlIssueGroup option:selected').val() == "Academics-MYP") {
                    if (ret1.grade == 'VI' || ret1.grade == 'VII' || ret1.grade == 'VIII' || ret1.grade == 'IX' || ret1.grade == 'X') { updateStudentInfo(ret1); return true; }
                    else { ErrMsg("Please change Issue Group as Academics-MYP for selected Student"); $('#ddlIssueGroup').val(""); GetIssueType(); return false; }
                }
                else if ($('#ddlIssueGroup option:selected').val() == "Academics-KG") {
                    if (ret1.grade == 'PlaySchool' || ret1.grade == 'PreKG' || ret1.grade == 'LKG' || ret1.grade == 'UKG') { updateStudentInfo(ret1); return true; }
                    else { ErrMsg("Please change Issue Group as Academics-KG for selected Student"); $('#ddlIssueGroup').val(""); GetIssueType(); return false; }
                }
                else if ($('#ddlIssueGroup option:selected').val() == "Academics-DP") {
                    if (ret1.grade == 'XI' || ret1.grade == 'XII') { updateStudentInfo(ret1); return true; }
                    else { ErrMsg("Please change Issue Group as Academics-DP for selected Student"); $('#ddlIssueGroup').val(""); GetIssueType(); return false; }
                }
                else { updateStudentInfo(ret1); return true; }
            } else {
                ret1 = jQuery("#StudentList").jqGrid('getRowData', rowid);
                if ($('#ddlIssueGroup option:selected').val() == "Academics-PYP") {
                    if (ret1.grade == 'I' || ret1.grade == 'II' || ret1.grade == 'III' || ret1.grade == 'IV' || ret1.grade == 'V') { updateStudentInfo(ret1); return true; }
                    else {
                        ErrMsg("Please change Issue Group as Academics-PYP for selected Student");$('#ddlIssueGroup').val("");GetIssueType();return false;}
                }
                else if ($('#ddlIssueGroup option:selected').val() == "Academics-MYP") {
                    if (ret1.grade == 'VI' || ret1.grade == 'VII' || ret1.grade == 'VIII' || ret1.grade == 'IX' || ret1.grade == 'X') { updateStudentInfo(ret1); return true; }
                    else { ErrMsg("Please change Issue Group as Academics-MYP for selected Student"); $('#ddlIssueGroup').val(""); GetIssueType(); return false; }
                }
                else if ($('#ddlIssueGroup option:selected').val() == "Academics-KG") {
                    if (ret1.grade == 'PlaySchool' || ret1.grade == 'PreKG' || ret1.grade == 'LKG' || ret1.grade == 'UKG') { updateStudentInfo(ret1); return true; }
                    else { ErrMsg("Please change Issue Group as Academics-KG for selected Student"); $('#ddlIssueGroup').val(""); GetIssueType(); return false; }
                }
                else if ($('#ddlIssueGroup option:selected').val() == "Academics-DP") {
                    if (ret1.grade == 'XI' || ret1.grade == 'XII') { updateStudentInfo(ret1); return true; }
                    else { ErrMsg("Please change Issue Group as Academics-DP for selected Student"); $('#ddlIssueGroup').val(""); GetIssueType(); return false; }
                }
                else { updateStudentInfo(ret1); return true; }
            }
        }
    });

}
function updateStudentInfo(retVAl) {
    $('#txtStuNum').val(retVAl.id_no);
    $('#txtstName').val(retVAl.name);
    $('#txtschool').val(retVAl.campus_name);
    $('#txtsection').val(retVAl.section);
    $('#txtgrade').val(retVAl.grade);
    if (retVAl.IsHosteller == 'Yes')
        $('#IsHosteller').attr("value", "True"); //$("#CheckOne").attr("value", "True");
    else
        $('#IsHosteller').attr("value", "False");
    $('#txtBoardingType').val(retVAl.BoardingType);
    $('#txtEmail').val(retVAl.EmailId);
    $('#DivStudentSearch').dialog('close');
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
function showYesOrNo(cellvalue, options, rowObject) {
    if (cellvalue == 'True') {
        return 'Yes';
    }
    else {
        return 'No';
    }
}
$(function () {
    $(window).on('resize.jqGrid', function () {
        $("#StudentList").jqGrid('setGridWidth', $(".page-content").width());
    })

    //resize on sidebar collapse/expand
    var parent_column = $("#StudentList").closest('[class*="col-"]');

    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#StudentList").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    $("#campus").attr("readonly", true).css("background-color", "#F5F5F5");
    $('#campus').val(cname);

    $('#DivStudentSearch').dialog({ modal: true });

    $("#Search").click(function () {
        idno = document.getElementById('idno').value;
        name = document.getElementById('name').value;
        cname = document.getElementById('campus').value;
        grade = document.getElementById('grade').value;
        section = document.getElementById('section').value;
        var brUrl1 = '/Home/StudentDetailsListJqGrid?idno=' + idno + '&name=' + name + '&cname=' + cname + '&grade=' + grade + '&sect=' + section;
        LoadSetGridParam($('#StudentList'), brUrl1);
    });
    $("#reset").click(function () {
        idno = "";
        name = "";
        campus = "";
        grade = "";
        section = "";
        var cname = document.getElementById('campus').value;
        document.getElementById('idno').value = "";
        document.getElementById('name').value = "";
        // document.getElementById('campus').value = "";
        document.getElementById('grade').value = "";
        document.getElementById('section').value = "";
        var brUrl1 = '/Home/StudentDetailsListJqGrid?idno=' + idno + '&name=' + name + '&cname=' + cname + '&grade=' + grade + '&sect=' + section;
        LoadSetGridParam($('#StudentList'), brUrl1);
        gridload();
    });
    window.onload = gridload();
});
