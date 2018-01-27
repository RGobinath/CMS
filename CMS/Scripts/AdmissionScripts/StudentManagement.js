
var MainrowData = [];
//    var Grid1 = $("#StudentManagementList").jqGrid('getGridParam', 'selarrrow');
var row1 = [];
var row2 = [];
var rcampus2 = [];
var MainrowData1 = "";
var j = 0;
var grid_selector = "#StudentManagementList";
var pager_selector = "#StudentManagementListPager";
var id = "", grade = "", acadyr = "", appname = "", section = "", ishosteller = "", idnum = "", preregno = "", flag = "", reset = "";
var admstat = "", feestructyr = "", appliedfrmdate = "", appliedtodate = "", Gender = "";
jQuery(function ($) {
    if ($('#transferpdf').val() == "yes") {
        if (confirm("Transfer Certificate Is Generated \n Are you sure you want to Discontinue this Student?")) {
            // window.location.href = "/Admission/DetainDiscontinue?PreRegNo=123&type=Discontinue"; // rowData1;
            window.location.href = "/Admission/DetainDiscontinue?type=Discontinue"; // rowData1;                         
        }
        else {
            // alert('Skipped');
        }
    }
    if ($('#transfered').val() == 'yes') {
        InfoMsg($('#transferedName').val() + ' is Transferred Successfully. NewId is' + $('#transferedId').val());
        $.ajax({
            url: '/Admission/ResetSession?type=transfer',
            type: 'GET',
            dataType: 'json',
            traditional: true
        });
    }
    if ($('#transfered').val() == 'no') {
        InfoMsg('Student Cannot be transferred to the same Campus');
        $.ajax({
            url: '/Admission/ResetSession?type=transfer',
            type: 'GET',
            dataType: 'json',
            traditional: true
        });
    }
    if ($('#discontinue').val() == 'yes') {
        InfoMsg($('#discontinueName').val() + ' has been Discontinued" Successfully.');
        $.ajax({
            url: '/Admission/ResetSession?type=discontinue',
            type: 'GET',
            dataType: 'json',
            traditional: true
        });
    }
    if ($('#promotion').val() == 'yes') {
        if ($('#notpromotedpreregno').val() != "") {
            if ($('#promotionId').val() == "") {
                InfoMsg('Students with preregnos ' + $('#notpromotedpreregno').val() + ' cannot be promoted ');
            }
            else {
                InfoMsg('Student Promoted Successfully to ' + $('#promotionId').val() + '.\n Students with preregnos ' + $('#notpromotedpreregno').val() + ' cannot be promoted');
            }
        }
        else {
            InfoMsg('Student Promoted Successfully to ' + $('#promotionId').val() + '.');
        }
        $.ajax({
            url: '/Admission/ResetSession?type=promotion',
            type: 'GET',
            dataType: 'json',
            traditional: true
        });
    }
    else if ($('#promotion').val() == 'no') {
        InfoMsg('Student cannot be promoted to the same class.');
        $.ajax({
            url: '/Admission/ResetSession?type=promotion',
            type: 'GET',
            dataType: 'json',
            traditional: true
        });
    }

    if ($('#bonafidepdf').val() == 'yes') {
        InfoMsg('Bonafide Certificate Has Been Issued Successfully ');
        $.ajax({
            url: '/Admission/ResetSession',
            type: 'GET',
            dataType: 'json',
            traditional: true
        });
    }
    if ($('#sportspdf').val() == 'yes') {
        InfoMsg('Sports Certificate Has Been Issued Successfully ');
        $.ajax({
            url: '/Admission/ResetSession?type=sports',
            type: 'GET',
            dataType: 'json',
            traditional: true
        });
    }
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
    if ($('#AdmissionSearched').val() != null && $('#AdmissionSearched').val() != "") {
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

    $("#ddlcampus").change(function () {
        gradeddl();
    });
    function loadgrid() {
        jQuery(grid_selector).jqGrid({
            mtype: 'GET',
            url: '/Admission/AdmissionManagementListJqGrid/',
            postData: { flag1: 'Register', Id: id, Gender: Gender, grade: grade, section: section, acadyr: acadyr, appname: appname, feestructyr: feestructyr, idnum: idnum, admstat: admstat, appno: '', preregnumber: preregno, ishosteller: ishosteller, flag: flag, reset: reset, stdntmgmt: 'yes', AppDateFrm: appliedfrmdate, AppDateTo: appliedtodate },
            datatype: 'json',
            colNames: ['Id', 'Application No', 'Pre-Reg', 'Name', 'Gender', 'Grade', 'Section', 'Campus', 'Fee Year', 'Admission Status', 'Stud Id', 'Academic Year', 'Applied Date'],
            colModel: [
                    { name: 'Id', index: 'Id', sortable: false, hidden: true },
                  { name: 'ApplicationNo', index: 'ApplicationNo', width: 50, align: 'left', sortable: true },
                  { name: 'PreRegNo', index: 'PreRegNum', width: 30, align: 'left', sortable: true },
                  { name: 'Name', index: 'Name', width: 100, align: 'left', sortable: true },
                  { name: 'Gender', index: 'Grade', width: 30, align: 'left', sortable: true },
                  { name: 'Grade', index: 'Gender', width: 30, align: 'left', sortable: true },
                  { name: 'Section', index: 'Section', width: 30, align: 'left', sortable: true },
                  { name: 'Campus', index: 'Campus', width: 60, align: 'left', sortable: true },
                  { name: 'FeeStructYear', index: 'FeeStructYear', width: 40, align: 'left', sortable: true },
                  { name: 'AdmissionStatus', index: 'AdmissionStatus', width: 60, align: 'left', sortable: true },
                  { name: 'NewId', index: 'NewId', width: 60, align: 'left', sortable: true },
                  { name: 'AcademicYear', index: 'AcademicYear', width: 60, align: 'left', sortable: true },
                  { name: 'AppliedDate', index: 'CreatedDate', width: 60, align: 'left', sortable: true }
                  ],
            pager: pager_selector,
            altRows: true,
            rowNum: 7,
            rowList: [7, 20, 50, 100, 150, 200],
            sortname: 'Id',
            sortorder: 'desc',
            height: '200',
            autowidth: true,
            viewrecords: true,
            multiselect: true,
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    // styleCheckbox(table);
                    // updateActionIcons(table);
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(window).triggerHandler('resize.jqGrid');
            },

            caption: "<i class='ace-icon fa fa-graduation-cap'></i>&nbsp;Edit Students",
            onPaging: function (pgButton) {
                var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
                if (GridIdList.length > 0) {
                    for (i = 0; i < GridIdList.length; i++) {
                        MainrowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                        if (MainrowData1 == "") {
                            MainrowData1 = MainrowData[i].PreRegNo;
                        }
                        else {
                            MainrowData1 = MainrowData1 + ", " + MainrowData[i].PreRegNo;
                        }
                    }
                }
                //                if (MainrowData1 != "") {
                //                    generate('bottomRight', MainrowData1);
                //                }
            }
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
                    refresh: true,
                    refreshicon: 'ace-icon fa fa-refresh green',
                    view: false,
                    viewicon: 'ace-icon fa fa-search-plus grey'
                }, {}, {}, {}, {}, {})
        jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
            onClickButton: function () {
                window.open("ExportToExcel" + '?rows=9999' + '&flag1=Register&Id=' + id + '&grade=' + grade + '&section=' + section + '&acadyr=' + acadyr + '&appname=' + appname + '&feestructyr=' + feestructyr + '&idnum=' + idnum + '&admstat=' + admstat + '&appno=&preregnumber=' + preregno + '&ishosteller=' + ishosteller + '&flag=' + flag + '&reset=' + reset + '&stdntmgmt=yes&AppDateFrm=' + appliedfrmdate + '&AppDateTo=' + appliedtodate);
            }
        });
    }

    window.onload = loadgrid();

    $("#Search").click(function () {
        debugger;
        GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];
        if (GridIdList.length > 0) {
            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                rowData1[i] = rowData[i].PreRegNo;
                if (MainrowData1 != "") {
                    MainrowData1 = MainrowData1 + ',' + rowData1[i];
                }
                else {
                    MainrowData1 = rowData1[i];
                }
            }
        }
        $(grid_selector).GridUnload();
        var e = document.getElementById("ddlcampus");
        id = e.options[e.selectedIndex].value;

        var f = document.getElementById("ddlgrade");
        grade = f.options[f.selectedIndex].value;

        var g = document.getElementById("admstats");
        admstat = g.options[g.selectedIndex].value;

        var h = document.getElementById("ddlsection");
        section = h.options[h.selectedIndex].value;

        var l = document.getElementById("feestructddl");
        feestructyr = l.options[l.selectedIndex].value;

        var n = document.getElementById("ishosteller");
        ishosteller = n.options[n.selectedIndex].value;

        var o = document.getElementById("academicyear");
        acadyr = o.options[o.selectedIndex].value;

        appname = document.getElementById('appname').value;

        var gndr = document.getElementById("ddlGender");
        Gender = gndr.options[gndr.selectedIndex].value;
        appliedfrmdate = document.getElementById("txtAppliedFrmDate").value;
        appliedtodate = document.getElementById("txtAppliedToDate").value;
        idnum = document.getElementById('idnumber').value;
        reset = "no";
        loadgrid();
    });
    $("#reset").click(function () {
        $(grid_selector).GridUnload();
        id = "";
        grade = "";
        admstat = "";
        appname = "";
        section = "";
        reset = "yes";
        idnum = "";
        preregno = "";
        flag = "";
        acadyr = "";
        $("input[type=text], textarea, select").val("");
        gradeddl();
        loadgrid();
    });
    $("#IdCard").click(function () {
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];

        //  alert(GridIdList.length);
        if (GridIdList.length > 0) {

            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                rowData1[i] = rowData[i].PreRegNo;
                if (MainrowData1 != "") {
                    MainrowData1 = MainrowData1 + ',' + rowData1[i];
                }
                else {
                    MainrowData1 = rowData1[i];
                }
            }
            window.location.href = "/Admission/PrintIdCard?PreRegNo=" + MainrowData1; // rowData1;
        }
        if (GridIdList.length == 0) {
            ErrMsg("Please Select Row");
            return false;
        }
    });


    $("#PickupCard").click(function () {
        debugger;
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];
        //  alert(GridIdList.length);
        if (GridIdList.length > 0) {
            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                rowData1[i] = rowData[i].PreRegNo;
                if (MainrowData1 != "") {
                    MainrowData1 = MainrowData1 + ',' + rowData1[i];
                }
                else {
                    MainrowData1 = rowData1[i];
                }
            }
            window.location.href = "/Admission/PrintPickupCard?PreRegNo=" + MainrowData1; // rowData1;
        }
        if (GridIdList.length == 0) {
            ErrMsg("Please Select Row");
            return false;
        }
    });

    $("#PrintSC").click(function () {
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];
        var sportsid = [];
        var sportsid1;
        var sportpreregnos = "";
        if (GridIdList.length > 0) {
            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                rowData1[i] = rowData[i].PreRegNo;
                sportsid[i] = rowData[i].Id;
                if (sportpreregnos != "") {
                    sportpreregnos = sportpreregnos + ',' + rowData1[i];
                    sportsid1 = sportsid1 + ',' + sportsid[i];
                }
                else {
                    sportpreregnos = rowData1[i];
                    sportsid1 = sportsid[i];
                }

                if (MainrowData1 != "") {  // For selecting students from multiple pages
                    MainrowData1 = MainrowData1 + ',' + rowData1[i];
                }
                else {
                    MainrowData1 = rowData1[i];
                }
            }
            ModifiedLoadPopupDynamicaly("/Admission/SportsPopup", $('#SportsPopupDiv'), function () { document.getElementById("sportspreregno1").value = MainrowData1; }, "", 300, 200, "Sports");
        }
        if (GridIdList.length == 0) {
            ErrMsg("Please Select Row");
            return false;
        }
    });

    $("#PrintPRF").click(function () {
        debugger;
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];
        var sportsid = [];
        var sportsid1;
        if (GridIdList.length == 1) {
            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                rowData1[i] = rowData[i].PreRegNo;
                if (MainrowData1 != "") {
                    MainrowData1 = MainrowData1 + ',' + rowData1[i];
                    sportsid1 = sportsid1 + ',' + sportsid[i];
                }
                else {
                    MainrowData1 = rowData1[i];
                    sportsid1 = sportsid[i];
                }
            }
            window.location.href = "/Admission/PrintPRF?PreRegNo=" + rowData1; // rowData1;
        }
        else if (GridIdList.length == 0) {
            ErrMsg("Please Select Row");
            return false;
        }
        else { ErrMsg('Only 1 student should be selected at a time'); return false;}
    });
    $("#btnpromotion").click(function () {
        debugger;
        $("#PromoteDiv").html("");
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];
        var rowcampus = [];
        var rowgrade = [];

        if (GridIdList.length > 0) {
            //  alert(GridIdList.length);
            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);

                if (rowData1 == "") {
                    rowData1 = rowData[i].PreRegNo;
                    rowgrade = rowData[i].Grade;
                    rowcampus = rowData[i].Campus;
                }
                else {
                    rowData1 = rowData1 + "," + rowData[i].PreRegNo;
                    rowgrade = rowgrade + "," + rowData[i].Grade;
                    rowcampus = rowcampus + "," + rowData[i].Campus;
                }
            }
            $.ajax({
                url: '/Admission/promotioncampus',
                type: 'GET',
                //  dataType: 'json',
                data: { PreRegNo: rowData1, campus: rowcampus, grade: rowgrade, check: 'yes' },
                traditional: true,
                success: function (data) {
                    if (data == "True") {
                        $.ajax({
                            url: '/Admission/promotioncampus',
                            type: 'GET',
                            //  dataType: 'json',
                            data: { PreRegNo: rowData1, campus: rowcampus },
                            traditional: true
                        });
                        ModifiedLoadPopupDynamicaly("/Admission/Promotion", $('#PromoteDiv'), function () { }, "", 500, 250, "Promotion");
                        var e = document.getElementById("ddlcampus");
                        var campus = e.options[e.selectedIndex].value;
                    }
                    else {
                        ErrMsg('Students to be promoted should belong to same grade and campus.');
                    }
                }
            });
        }
        if (GridIdList.length == 0) {
            ErrMsg("Please Select Row");
            return false;
        }
    });


    $("#Transfer").click(function () {
        debugger;
        $("#TransferDiv").html("");
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowPreregno = [];
        var rowcampus = [];
        var rowgrade = [];
        if (GridIdList.length > 0) {
            if (GridIdList.length > 1) {
                ErrMsg("Only 1 Student can be trafered at once");
                return false;
            }
            else {
                rowData[0] = $(grid_selector).jqGrid('getRowData', GridIdList[0]);
                rowgrade = rowData[0].Grade;
                rowcampus = rowData[0].Campus;
                rowPreregno = rowData[0].PreRegNo;
            }
            $.ajax({
                url: '/Admission/transfergrade',
                type: 'GET',
                data: { grd: rowgrade },
                traditional: true,
                success: function (data) {
                    ModifiedLoadPopupDynamicaly("/Admission/Transfer", $('#TransferDiv'), function () { document.getElementById("preregno2").value = rowPreregno; }, "", 500, 250, "Transfer");
                }
            });
        }
        if (GridIdList.length == 0) {
            ErrMsg("Please Select Row");
            return false;
        }
    });

    $("#RequestTC").click(function () {
        debugger;
       // $("#TransferDiv").html("");
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowPreregno = [];
        if (GridIdList.length > 0) {
            if (GridIdList.length > 1) {
                ErrMsg("Only 1 Student can be trafered at once");
                return false;
            }            
            else {                
                rowData[0] = $(grid_selector).jqGrid('getRowData', GridIdList[0]);                
                rowPreregno = rowData[0].PreRegNo;
                rowNewId = rowData[0].NewId;
                if (checkTCRequest(rowNewId, rowPreregno) == false) {
                    ErrMsg("Already Exist");
                    return false;                    
                }
                else {
                    window.location.href = "/Admission/TCRequest?PreRegNo=" + rowPreregno + '&Id=0';
                }
            }
        }
        if (GridIdList.length == 0)
        {
            ErrMsg("Please Select Row");
            return false;
        }
    });

    $("#Discontinue").click(function () {
        debugger;
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];
        var rowcampus = [];
        if (GridIdList.length > 0) {
            if (GridIdList.length > 1) { ErrMsg("Only 1 Student can be Discontinued at once"); }
            else {
                for (i = 0; i < GridIdList.length; i++) {
                    rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                    rowcampus[i] = rowData[i].Campus;
                    if (rowData1 == "") {
                        rowData1 = rowData[i].PreRegNo;
                    }
                    else {
                        rowData1 = rowData1 + "," + rowData[i].PreRegNo;
                    }
                }
                var stats;
                $.ajax({
                    url: '/Admission/Discontinuestats',
                    type: 'GET',
                    data: { PreRegNo: rowData1, campus: rowcampus },
                    traditional: true,
                    success: function (data) {
                        stats = data;
                        if (stats == "True") {
                            ModifiedLoadPopupDynamicaly("/Admission/Discontinue?PreRegNo=" + rowData1, $('#DiscontinueDiv'), function () { }, "", 700, 280, "Discontinue");
                        }
                        else {
                            if (confirm("Transfer Certificate Is Generated \n Are you sure you want to Discontinue this Student?")) {
                                rowData1 = "";
                                for (i = 0; i < GridIdList.length; i++) {
                                    if (rowData1 == "") {
                                        rowData1 = rowData[i].PreRegNo;
                                    }
                                    else {
                                        rowData1 = rowData1 + "," + rowData[i].PreRegNo;
                                    }
                                }
                                $.ajax({
                                    url: '/Admission/DetainDiscontinue',
                                    type: 'GET',
                                    data: { PreRegNo: rowData1, type: 'Discontinue' },
                                    traditional: true,
                                    success: function (data) {
                                        InfoMsg('Student Discontinued successfully.');
                                    }
                                });
                            }
                            else {
                            }
                        }
                    }
                });
            }
        }
        if (GridIdList.length == 0) {
            ErrMsg("Please Select Row");
            return false;
        }
    });
    $("#detain").click(function () {
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];
        var rowcampus = [];
        if (GridIdList.length > 1) {
            ErrMsg("Only 1 Student can be Detained at once");
            return false;
        }
        else if (GridIdList.length == 0) {
            ErrMsg("Please Select Row");
            return false;
        }
        else {
            if (confirm("Are you sure you want to Detain this Student?")) {
                for (i = 0; i < GridIdList.length; i++) {
                    rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                    rowcampus[i] = rowData[i].Campus;
                    if (rowData1 == "") {
                        rowData1 = rowData[i].PreRegNo;
                    }
                    else {
                        rowData1 = rowData1 + "," + rowData[i].PreRegNo;
                    }
                }
                $.ajax({
                    url: '/Admission/Detain',
                    type: 'GET',
                    dataType: 'json',
                    data: { PreRegNo: rowData1 },
                    traditional: true,
                    success: function (data) {
                        if (data.detained == "yes") {
                            InfoMsg(' ' + data.name + ' Detained successfully.');
                        }
                        else {
                            ErrMsg('' + data.name + ' cannot be detained.');
                        }
                        $(grid_selector).trigger('reloadGrid');
                    }
                });
            }
            else { }
        }

    });

    $("#bonafide").click(function () {
        debugger;
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];
        var rowcampus = [];
        if (GridIdList.length > 1) {
            ErrMsg("Bonafide Certificate Can Be Issued To Only 1 Student at once");
            return false;
        }
        else if (GridIdList.length == 0) {
            ErrMsg("Please Select Row");
            return false;
        }
        else {
            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                rowData1 = rowData[i].PreRegNo;
                rowcampus[i] = rowData[i].Campus;
            }
            var stats;
            $.ajax({
                url: '/Admission/Discontinuestats',
                type: 'GET',
                data: { PreRegNo: rowData1, campus: rowcampus, type: 'bonafide' },
                traditional: true,
                success: function (data) {
                    stats = data;
                    if (stats == "True") {
                        $.ajax({
                            url: '/Admission/Bonafide',
                            type: 'GET',
                            dataType: 'json',
                            data: { PreRegNo: rowData1 },
                            traditional: true,
                            success: function (data) {
                                InfoMsg('Bonafide Certificate Issued Successfully For ' + data.Name + ' Id No: ' + data.AfterId + '');
                            }
                        });
                    }
                    else {
                        ErrMsg("Bonafide Certificate Has Already Been Issued For This Student");
                    }
                }
            });
        }
    });

    $("#sports").click(function () {
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];
        var rowcampus = [];
        var rowname = [];
        if (GridIdList.length > 0) {
            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                if (rowData1 == "") {
                    rowData1 = rowData[i].PreRegNo;
                    rowcampus = rowData[i].Campus;
                    rowname = rowData[i].Name;
                }
                else {
                    rowData1 = rowData1 + "," + rowData[i].PreRegNo;
                    rowcampus = rowcampus + "," + rowData[i].Campus;
                    rowname = rowname + "," + rowData[i].Name;
                }
            }
            var stats;
            ModifiedLoadPopupDynamicaly("/Admission/Sports?preregno=" + rowData1, $('#SportsDiv'), function () { }, "", 600, 310, "Sports");
        }
    });
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});
//function generate(layout, data1) {
//    // alert(data1);
//    var n = noty({
//        text: "Following Pre-Reg Nos are selected: " + data1,
//        //     template: '<div class="noty_message1"  style="width:1000px" >Following Pre-Reg No has been selected:  <br/>' + data1 + '</div>',                  
//        type: 'information',
//        dismissQueue: true,
//        layout: layout,
//        theme: 'defaultTheme',
//        timeout: 1500,
//        header: false
//        //    pause: true,
//        //  pauseOnPagerHover: true
//        //                    height: 'auto',
//        //                   width: 'auto'
//    });
//    console.log('html: ' + n.options.id);
//}

//function generateAll() {
//    //generate('top');
//}

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

function getdata(id1) {
    window.location.href = "/Admission/GetFormData?Id=" + id1 + "&RegisteredForm=yes";
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

function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}

function gradeddl() {
    var e = document.getElementById('ddlcampus');
    var campus = e.options[e.selectedIndex].value;
    $.getJSON("/Admission/CampusGradeddl/", { Campus: campus },
            function (modelData) {
                var select = $("#ddlgrade");
                select.empty();
                select.append($('<option/>', { value: "", text: "Select Grade" }));
                $.each(modelData, function (index, itemData) {
                    select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                });
            });
}

function checkTCRequest(NewId,preregno)
{    
        var retdata;
        $.ajax({
            type: 'Get',
            async: false,
            url: '/Admission/CheckTCRequest?NewId=' + NewId + '&PreRegNo=' + preregno,
            success: function (data) {
                if (data == "failed") {
                    retdata = false;
                }
                if (data == "success") {
                    retdata = true;
                }
            }
        });
        return retdata;    
}