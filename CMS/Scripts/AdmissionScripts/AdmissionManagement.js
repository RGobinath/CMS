var grid_selector = "#grid-table";
var pager_selector = "#grid-pager";
var id = "", grade = "", acadyr = "", appname = "", section = "", admstat = "", appno = "", preregno = "", flag = "", reset = "", Gender = "", appliedfrmdate = "", appliedtodate = "";
jQuery(function ($) {
    if ($('#registered').val() == "yes") {
        InfoMsg("Student Registered Successfully \n Student Id is " + $('#regId').val());
    }
    if ($('#AdmissionSearched').val() != null) {
        var srchitems = $('#AdmissionSearched').val();
        var srchitemsArr = srchitems.split(',');
        id = srchitemsArr[0]; grade = srchitemsArr[1]; Gender = srchitemsArr[2]; acadyr = srchitemsArr[3]; appname = srchitemsArr[4]; appno = srchitemsArr[5];
        admstat = srchitemsArr[6]; preregno = srchitemsArr[7]; appliedfrmdate = srchitemsArr[8]; appliedtodate = srchitemsArr[9];
        $('#ddlcampus').val(id);
        $('#ddlgrade').val(grade);
        $('#ddlGender').val(Gender);
        $("#ddlacademicyear").val(acadyr);
        $("#appname").val(appname);
        $("#txtAppNum").val(appno);
        $("#AdmissionStatus").val(admstat);
        $("#txtPreRegNum").val(preregno);
        $("#txtAppliedFrmDate").val(appliedfrmdate);
        $("#txtAppliedToDate").val(appliedtodate);
    }
    window.onload = loadgrid();
    $("#ddlcampus").change(function () {
        gradeddl();
    });

    $("#New").click(function () {
        debugger;
        window.location.href = $('#NewRegUrl').val();
    });
    $("#Search").click(function () {
        $(grid_selector).GridUnload();
        var e = document.getElementById("ddlcampus");
        id = e.options[e.selectedIndex].value;
        var f = document.getElementById("ddlgrade");
        grade = f.options[f.selectedIndex].value;
        var g = document.getElementById("ddlacademicyear");
        acadyr = g.options[g.selectedIndex].value;
        appname = document.getElementById('appname').value;
        var h = document.getElementById("AdmissionStatus");
        admstat = h.options[h.selectedIndex].value;
        var Gen = document.getElementById("ddlGender");
        Gender = Gen.options[Gen.selectedIndex].value;
        appno = document.getElementById('txtAppNum').value;
        preregno = document.getElementById('txtPreRegNum').value;
        appliedfrmdate = document.getElementById("txtAppliedFrmDate").value;
        appliedtodate = document.getElementById("txtAppliedToDate").value;
        reset = "no";
        loadgrid();
    });
    $("#reset").click(function () {
        $(grid_selector).GridUnload();
        id = "";
        grade = "";
        acadyr = "";
        appname = "";
        admstat = "";
        appno = "";
        preregno = "";
        appliedfrmdate = "";
        appliedtodate = "";
        reset = "yes";
        flag = "";
        var e = document.getElementById('ddlcampus');
        e.options[0].selected = true; // "Select";
        // alert(e.options[e.selectedIndex].value);
        var f = document.getElementById('ddlgrade');
        f.options[0].selected = true; // "Select";

        var g = document.getElementById('ddlacademicyear');
        g.options[0].selected = true; // "Select";

        var h = document.getElementById('AdmissionStatus');
        h.options[0].selected = true; // "Select";

        document.getElementById('appname').value = "";
        document.getElementById('txtAppNum').value = "";
        document.getElementById('txtPreRegNum').value = "";
        document.getElementById('txtAppliedFrmDate').value = "";
        document.getElementById('txtAppliedToDate').value = "";
        gradeddl();
        //   LoadSetGridParam($("#AdmissionManagementList"), '/Admission/AdmissionManagementListJqGrid/');
        loadgrid();
    });
    $("#BulkRegister").click(function () {
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = "";
        if (GridIdList.length > 0) {
            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                if (rowData1 == "") {
                    rowData1 = rowData[i].PreRegNo;
                }
                else { rowData1 = rowData1 + "," + rowData[i].PreRegNo; }
            }
            ModifiedLoadPopupDynamicaly("/Admission/BulkRegister?PreRegNo=" + rowData1, $('#BulkRegisterdiv'), function () { }, "", 450, 200, "Bulk Register");
        }
    });
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
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
});
function loadgrid() {
    jQuery(grid_selector).jqGrid({
        mtype: 'GET',
        url: '/Admission/AdmissionManagementListJqGrid',
        postData: { Id: id, Gender: Gender, grade: grade, section: section, acadyr: acadyr, feestructyr: '', appname: appname, admstat: admstat, appno: appno, idnum: '', preregnumber: preregno, ishosteller: '', flag: flag, flag1: '', reset: reset, stdntmgmt: '', AppDateFrm: appliedfrmdate, AppDateTo: appliedtodate },
        datatype: 'json',
        colNames: ['Id', 'Application No', 'Pre-Reg No', 'Name of Applicant', 'Gender', 'Grade', 'Section', 'Campus', 'Fee Structure Year', 'Admission Status', 'Student Id', 'Academic Year', 'Applied Date', 'Source'],
        colModel: [{ name: 'Id', index: 'Id', sortable: false, hidden: true },
                   { name: 'ApplicationNo', index: 'ApplicationNo', width: 60, align: 'left', sortable: true },
                   { name: 'PreRegNo', index: 'PreRegNum', width: 30, align: 'left', sortable: true },
                   { name: 'Name', index: 'Name', width: 100, align: 'left', sortable: true },
                   { name: 'Gender', index: 'Gender', width: 30, align: 'left', sortable: true },
                   { name: 'Grade', index: 'Grade', width: 30, align: 'left', sortable: true },
                   { name: 'Section', index: 'Section', width: 30, align: 'left', sortable: true },
                   { name: 'Campus', index: 'Campus', width: 60, align: 'left', sortable: true },
                   { name: 'FeeStructYear', index: 'FeeStructYear', width: 30, align: 'left', sortable: true },
                   { name: 'AdmissionStatus', index: 'AdmissionStatus', width: 60, align: 'left', sortable: true },
                   { name: 'NewId', index: 'NewId', width: 60, align: 'left', sortable: true },
                   { name: 'AcademicYear', index: 'AcademicYear', width: 60, align: 'left', sortable: true },
                   { name: 'AppliedDate', index: 'CreatedDate', width: 60, align: 'left', sortable: true },
                   { name: 'EntryFrom', index: 'EntryFrom', width: 60, align: 'left', sortable: true, formatter: Source }
                   ],
        pager: pager_selector,
        rowNum: 7,
        rowList: [7, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'desc',
        multiselect: true,
        viewrecords: true,
        height: '200',
        autowidth: true,
        altRows: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                //styleCheckbox(table);
                //updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-inbox'></i>&nbsp;Inbox"
    });
    //navButtons
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
                    refresh: false,
                    refreshicon: 'ace-icon fa fa-refresh green',
                    view: true,
                    viewicon: 'ace-icon fa fa-search-plus grey'
                }, {}, {}, {}, {//search form
                    recreateForm: true,
                    afterShowSearch: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                        style_search_form(form);
                    }, afterRedraw: function () {
                        style_search_filters($(this));
                    }, multipleSearch: true
                    /**
                    multipleGroup:true,
                    showQuery: true
                    */
                }, { //view record form
                    recreateForm: true,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                    }
                })
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            window.open("ExportToExcel" + '?rows=9999' + '&Id=' + id + '&grade=' + grade + '&section=' + section + '&acadyr=' + acadyr + '&feestructyr=&appname=' + appname + '&admstat=' + admstat + '&appno=' + appno + '&idnum=&preregnumber=' + preregno + '&ishosteller=&flag=' + flag + '&flag1=&reset=' + reset + '&stdntmgmt=&AppDateFrm=' + appliedfrmdate + '&AppDateTo=' + appliedtodate);
        }
    })
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
function Source(cellvalue, options, rowObject) {
    if (cellvalue == 'Parent') { return 'Parent' }
    else { return 'School' }
}

function gradeddl() {
    var e = document.getElementById('ddlcampus');
    var campus = e.options[e.selectedIndex].value;
    //     alert(campus);
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
function getdata(id1) {
    window.location.href = "/Admission/GetFormData?Id=" + id1;
    //'@ViewBag.editid' == id1;
}
function Source(cellvalue, options, rowObject) {
    if (cellvalue == 'Parent') { return 'Parent' }
    else { return 'School' }
}