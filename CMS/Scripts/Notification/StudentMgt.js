$(function () {

    $('#ddlgrade').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '300',
        numberDisplayed: 4,
        includeSelectAllDivider: true
    });
    $('#ddlsection').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '300',
        numberDisplayed: 3,
        includeSelectAllDivider: true
    });
   
    ////$("#ddlgrade").dropdownchecklist({ icon: { placement: 'right' }, maxDropHeight: 15 });
    var id = "";
    var grade = "";
    var acadyr = ""; var appname = ""; var section = "";
    var ishosteller = ""; var idnum = ""; var preregno = ""; var flag = ""; var reset = "";
    var admstat = ""; var feestructyr = "";
    var appno = ""; var selNewIds = ""; var MainrowData = [];
    var MainrowData1 = ""; var MainrowData2 = ""; var MainrowDataPRN = []; var j = 0;
    var grid_selector = "#StudentManagementList";
    var pager_selector = "#StudentManagementListPager";
    function generate(layout, data1) {

        var n = noty({
            text: "Following Pre-Reg Nos are selected: " + data1,
            type: 'information',
            dismissQueue: true,
            layout: layout,
            theme: 'defaultTheme',
            timeout: 1500,
            header: false
        });
        console.log('html: ' + n.options.id);
    }
    function generateAll() {
        generate('top');
    }
    function runEffect() {
        // get effect type from
        var selectedEffect = $("#effectTypes").val();
        // most effect types need no options passed by default
        var options = {};
        // some effects have required parameters
        if (selectedEffect === "scale") {
            options = { percent: 100 };
        } else if (selectedEffect === "size") {
            options = { to: { width: 280, height: 185} };
        }
        // run the effect
        $("#effect").show(selectedEffect, options, 500, callback);
    };
    //callback function to bring a hidden box back
    function callback() {
        setTimeout(function () {
            $("#effect:visible").removeAttr("style").fadeOut();
        }, 1000);
    };

    gradeddl();
    sectionddl();
    $.getJSON("/Notify/FillCampusList",
     function (fillbc) {
         var ddlbc = $("#ddlcampus");
         ddlbc.empty();
         ddlbc.append($('<option/>', { value: "", text: "Select One" }));
         $.each(fillbc, function (index, itemdata) {
             if (itemdata.Text == $('#Campus').val()) {
                 ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
             }
             else {
                 ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
             }
         });
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

    //pager icon


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



    function loadgrid() {


        jQuery(grid_selector).jqGrid({
            mtype: 'GET',
            url: "/Notify/AdmissionManagementListJqGrid",

            postData: { flag1: 'Register', Id: id, grade: grade, section: section, acadyr: acadyr, appname: appname, idnum: idnum, admstat: admstat, appno: '', preregnumber: preregno, ishosteller: ishosteller, flag: flag, reset: reset, stdntmgmt: 'yes' },
            datatype: 'json',
            colNames: ['Application No', 'Pre-Reg No', 'Name of Applicant', 'Grade', 'Section', 'Campus', 'Fee Structure Year', 'Admission Status', 'Student Id', 'Academic Year', 'Applied Date'],
            colModel: [

              { name: 'ApplicationNo', index: 'ApplicationNo', width: 60, align: 'left', sortable: false },
              { name: 'PreRegNo', index: 'PreRegNo', width: 30, align: 'left', sortable: false },
              { name: 'Name', index: 'Name', width: 100, align: 'left', sortable: false },
              { name: 'Grade', index: 'Grade', width: 30, align: 'left', sortable: false },
              { name: 'Section', index: 'Section', width: 30, align: 'left', sortable: false },
              { name: 'Campus', index: 'Campus', width: 60, align: 'left', sortable: false },
              { name: 'FeeStructYear', index: 'FeeStructYear', width: 30, align: 'left', sortable: false },
              { name: 'AdmissionStatus', index: 'AdmissionStatus', width: 60, align: 'left', sortable: false },
              { name: 'NewId', index: 'NewId', width: 60, align: 'left', sortable: false },
              { name: 'AcademicYear', index: 'AcademicYear', width: 60, align: 'left', sortable: false },
              { name: 'AppliedDate', index: 'AppliedDate', width: 60, align: 'left', sortable: false}],
            pager: pager_selector,
            rowNum: '10',
            rowList: [5, 10, 20, 50, 100, 150, 200],
            sortname: 'Id',
            sortorder: 'desc',
            viewrecords: true,
            multiselect: true,
            height: '175',
            autowidth: true,
            caption: '<i class="fa fa-list-alt">&nbsp;</i>Students List',
            forceFit: true,
            multiselect: true,
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
            },
            ondblClickRow: function (rowid, iRow, iCol, e) {
                $(grid_selector).editGridRow(rowid, prmGridDialog);
            },
            loadError: function (xhr, status, error) {
                $(grid_selector).clearGridData();
                ErrMsg($.parseJSON(xhr.responseText).Message);
            },

            onPaging: function (pgButton) {
                var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');

                if (GridIdList.length > 0) {
                    for (i = 0; i < GridIdList.length; i++) {
                        MainrowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                        if (MainrowData1 == "") {
                            MainrowData1 = MainrowData[i].NewId;
                            MainrowData2 = MainrowData[i].PreRegNo;
                        }
                        else {
                            MainrowData1 = MainrowData1 + "," + MainrowData[i].NewId;
                            MainrowData2 = MainrowData2 + "," + MainrowData[i].PreRegNo;
                        }
                    }
                }
                if (MainrowData1 != "") {
                    generate('bottomRight', MainrowData1);
                }
            }
        });
        jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: true,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {},
            {});
    }


    $("#ddlcampus").change(function () {
        gradeddl();
    });


    window.onload = loadgrid();
    $("#Search").click(function () {

        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];
        if (GridIdList.length > 0) {
            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                rowData1[i] = rowData[i].PreRegNo;
                if (MainrowData1 != "") {
                    MainrowData1 = MainrowData1 + ',' + rowData1[i];
                    //MainrowData2 = MainrowData2 + ',' + rowData1[i].PreRegNo;
                }
                else {
                    MainrowData1 = rowData1[i];
                }
            }
        }

        $(grid_selector).GridUnload();
        var e = document.getElementById("ddlcampus");
        id = e.options[e.selectedIndex].value;
        grade = $("#ddlgrade").val();
        section = $("#ddlsection").val();
        if (grade != "" & grade != null) {
            grade = $('#ddlgrade').val().toString();
        }
        if (section != "" & section != null) {
            section = $('#ddlsection').val().toString();
        }
        var i = document.getElementById("ishosteller");
        ishosteller = i.options[i.selectedIndex].value;
        var o = document.getElementById("academicyear");
        acadyr = o.options[o.selectedIndex].value;
        appname = document.getElementById('appname').value;
        idnum = document.getElementById('idnumber').value;
        reset = "no";
        loadgrid();
    });

    $("#reset").click(function () {

        $(grid_selector).GridUnload();
        loadgrid();
        LoadSetGridParam($(grid_selector), '/Admission/AdmissionManagementListJqGrid/');
        id = "";
        grade = "";
        acadyr = "";
        appname = "";
        section = "";
        reset = "yes";
        appno = "";
        preregno = "";
        flag = "";
        var e = document.getElementById('ddlcampus');
        e.options[0].selected = true;
        grade = "";
        section = "";
        var i = document.getElementById("ishosteller");
        i.options[0].selected = true; // "Select";
        gradeddl();
        sectionddl();
        loadgrid();
        document.getElementById('appname').value = "";

        document.getElementById('idnumber').value = "";


    });

    $("#BulkEmail").click(function () {

        var e = document.getElementById("ddlcampus");
        var campus = e.options[e.selectedIndex].value;
        LoadPopupDynamicaly1("/Admission/CampusEmail", $('#EmailDiv1'),
                            function () {
                                $.ajax({
                                    url: "/Admission/CampusEmail",
                                    type: 'GET',
                                    dataType: 'json',
                                    data: { campusname: campus },
                                    traditional: true
                                });
                            });
    });

    function LoadPopupDynamicaly1(dynURL, ModalId, loadCalBack, onSelcalbck, width, height) {

        if (width == undefined) { width = 1150; }
        if (height == undefined) { height = 800; }
        if (ModalId.html().length == 0) {
            $.ajax({
                url: dynURL,
                type: 'GET',
                // type: 'POST',
                async: false,
                dataType: 'html', // <-- to expect an html response
                //    data: { ActivityId: rowData1 },
                success: function (data) {
                    ModalId.dialog({
                        height: height,
                        width: width,
                        modal: true,

                        buttons: {}
                    });
                    ModalId.html(data);
                }
            });
        }
        clbPupGrdSel = onSelcalbck;
        ModalId.dialog('open');
        CallBackFunction(loadCalBack);
    }




    function LoadSetGridParam(GridId, brUrl) {

        GridId.setGridParam({
            url: brUrl,
            datatype: 'json',
            mtype: 'GET'
        }).trigger("reloadGrid");
    }

    $("#Parent").click(function () {

        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = "";
        var AcademicYear = "";
        var rowDataPRN = [];
        var rowData2 = "";
        if (GridIdList.length == 0) {
            ErrMsg("Please Select requied Students");
        }
        if (GridIdList.length > 0) {
            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                if (rowData[0] != "") { AcademicYear = rowData[0].AcademicYear; }
                if (rowData1 == "") {
                    rowData1 = rowData[i].NewId;
                    rowData2 = rowData[i].PreRegNo;
                }
                else {
                    rowData1 = rowData1 + "," + rowData[i].NewId;
                    rowData2 = rowData2 + "," + rowData[i].PreRegNo;
                }
            }
            selNewIds = MainrowData1 + "," + rowData1;
            selPRNs = MainrowData2 + "," + rowData2;
            var e = document.getElementById("ddlcampus");
            var campus = e.options[e.selectedIndex].value;            
            if (campus == "") {
                ErrMsg("Please Select Campus");
            } else {

                //                LoadPopupDynamicaly1("/notify/SpecificNotification?PublishTo=Parent&PreRegNo=" + selPRNs + "&NewId=" + selNewIds + "&campus=" + campus + "&AcademicYear=" + AcademicYear, $('#ParentDiv'),
                //                    function () {
                //                        document.getElementById("preregnos").value = selPRNs;
                //                        document.getElementById("NewIds").value = selNewIds;
                //                        document.getElementById("campus").value = campus;
                //                    });
                //                ModifiedLoadPopupDynamicaly("/Admission/CampusEmail?campusname=" + campus, $('#AnswersPopup'), function () { }, function () { }, 900, 900, "Student Details");                                                                
                ModifiedLoadPopupDynamicaly("/notify/SpecificNotification?PreRegNo=" + selPRNs + "&NewId=" + selNewIds + "&campus=" + campus + "&AcademicYear=" + AcademicYear, $('#ParentDiv'),
                    function () {
                        document.getElementById("preregnos").value = selPRNs;
                        document.getElementById("NewIds").value = selNewIds;
                        document.getElementById("campus").value = campus;
                    }, function () { }, 900, 286, "Individual Notification");


                //                   

            }
        }
    });

    $("#Student").click(function () {
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = "";
        if (GridIdList.length == 0) {
            ErrMsg("Please Select requied Students");
        }
        if (GridIdList.length > 0) {
            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                if (rowData1 == "") {
                    rowData1 = rowData[i].NewId;
                    rowData2 = rowData[i].PreRegNo;
                }
                else {
                    rowData1 = rowData1 + "," + rowData[i].NewId;
                    rowData2 = rowData2 + "," + rowData[i].PreRegNo;
                }
            }
            selNewIds = MainrowData1 + "," + rowData1;
            selPRNs = MainrowData2 + "," + rowData2;
            var e = document.getElementById("ddlcampus");
            var campus = e.options[e.selectedIndex].value;
            if (campus == "") {
                ErrMsg("Please Select Campus");
            } else {               
                ModifiedLoadPopupDynamicaly("/notify/SpecificNotification1?PublishTo=Student&PreRegNo=" + selPRNs + "&NewId=" + selNewIds + "&campus=" + campus, $('#StudentDiv'),
            function () {
                document.getElementById("preregnos").value = selPRNs;
                document.getElementById("NewIds").value = selNewIds;
                document.getElementById("campus").value = campus;
            }, function () { }, 818, 286, "Notification to Student");
            }
        }

    });
});

/* --------Grade Multidrop down -------*/






function gradeddl() {
    var e = document.getElementById('ddlcampus');
    var campus = e.options[e.selectedIndex].value;
    //var campus = $("#Campus").val();
    $.ajax({
        type: 'POST',
        async: false,
        dataType: "json",
        url: '/Communication/FillGrades?campus=' + campus,
        success: function (data) {
            $("#ddlgrade").empty();
//            $("#ddlgrade").dropdownchecklist('destroy');
            if (data != null) {
//                $("#ddlgrade").append("<option value=' '> All </option>");
                for (var k = 0; k < data.length; k++) {
                    $("#ddlgrade").append("<option value='" + data[k].Value + "'>" + data[k].Text + "</option>");
                }
            }
            $('#ddlgrade').multiselect('rebuild');
            //Refreshddl();
        }
    });

}
function Refreshddl() {
    $("#ddlgrade").dropdownchecklist({
        firstItemChecksAll: true,
        maxDropHeight: 150,
        Height: 80,
        width: 155
    });
}
/* -------- End Grade Multidrop down -------*/
/*-------------------------------------------section Multidrop down--------------------------------------------------*/
function sectionddl() {

    $("#ddlsection").empty();
//    $("#ddlsection").append("<option value=' '> All </option>");
//    $("#ddlsection").dropdownchecklist('destroy');
    $("#ddlsection").append("<option value='A'>A</option>");
    $("#ddlsection").append("<option value='B'>B</option>");
    $("#ddlsection").append("<option value='C'>C</option>");
    $("#ddlsection").append("<option value='D'>D</option>");
    $("#ddlsection").append("<option value='E'>E</option>");
    $("#ddlsection").append("<option value='F'>F</option>");
    $('#ddlsection').multiselect('rebuild');
//    $("#ddlsection").dropdownchecklist({
//        firstItemChecksAll: true,
//        maxDropHeight: 150,
//        Height: 1,
//        width: 155
//    });
}
/*------------------------------------------------End section Multidrop down-----------------------------------------------*/
function getdata(id1) {
    // 'Session["RegisteredForm"]'="yes";

    window.location.href = "/Admission/GetFormData?Id=" + id1 + "&RegisteredForm=yes";
}


try { ace.settings.check('main-container', 'fixed') } catch (e) { }