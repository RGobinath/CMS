$(function () {

    $('#RequestNo').css('ui-corner-all');
    GetCampus();
    if ($('#SrchSessionvalue').val() != null) {
        var srchitems = $('#SrchSessionvalue').val();
        var srchitemsArr = srchitems.split(',');
        $("#RequestNo").val(srchitemsArr[0]);
        $("#Campus").val(srchitemsArr[1]);
        $("#Name").val(srchitemsArr[2]);
        $("#Section").val(srchitemsArr[3]);
        $("#Grade").val(srchitemsArr[4]);
        $("#RptStatus").val(srchitemsArr[5]);
        $("#Semester").val(srchitemsArr[6]);
        $("#academicyear").val(srchitemsArr[7]);
    }
    var grid_selector = "#ReportCardList";
    var pager_selector = "#ReportCardPage";
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




    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    function formateadorLink(cellvalue, options, rowObject) {
        if (rowObject[6] == 'III' || rowObject[6] == 'IV' || rowObject[6] == 'V') {
            return "<a href=/AchievementReport/ReportCardPYP?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
        } else if (rowObject[6] == 'VI' || rowObject[6] == 'VII' || rowObject[6] == 'VIII') {
            return "<a href=/AchievementReport/ReportCardMYP?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
        }
        return;
    }

    function formateadPDFLink(cellvalue, options, rowObject) {
        return "<span id='T2btnViewPdf_" + rowObject[0] + "'class='fa fa-file-pdf-o red T2ViewPDF'  vId='" + rowObject[0] + "' vGrade='" + rowObject[6] + "' vSemester='" + rowObject[9] + "'title='View PDF'></span>";
    }

    function ViewPDF(pGrade, pSemester, pId) {
        //return window.location.href = '/AchievementReport/GenerateReportCardPDF?Grade=' + pGrade + '&Semester=' + pSemester + '&Id=' + pId;
        return window.location.href = '/AchievementReport/MYPReportCardPDFGen?&Id=' + pId;
    }



    $(grid_selector).jqGrid({
        mtype: 'GET',
        url: "/AchievementReport/GetReportCardInbox",
        datatype: 'json',
        height: '150',
        colNames: ['Id', 'RequestNo', 'Campus Name', 'Id No', 'Name', 'Section', 'Grade', 'StudentId', 'Teacher Name', 'Semester', 'Academic Year', ''],
        colModel: [
                          { name: 'Id', index: 'Id', hidden: true, key: true }, //0
                          {name: 'RequestNo', index: 'RequestNo', formatter: formateadorLink }, //1
                          {name: 'Campus', index: 'Campus', sortable: false }, //2
                          {name: 'IdNo', index: 'IdNo', sortable: false }, //3
                          {name: 'Name', index: 'Name', sortable: false }, //4
                          {name: 'Section', index: 'Section', sortable: false }, //5
                          {name: 'Grade', index: 'Grade', sortable: false }, //6
                          {name: 'StudentId', index: 'StudentId', hidden: true }, //7
                          {name: 'TeacherName', index: 'TeacherName', hidden: true }, //8
                          {name: 'Semester', index: 'Semester' }, //9
                          {name: 'AcademicYear', index: 'AcademicYear' }, //10  added by felix kinoniya
                          {name: 'pdfView', index: 'pdfView', width: '30%',align:'center', formatter: formateadPDFLink}//11

                          ],
        pager: pager_selector,
        shrinkToFit: true,
        rowNum: '10',
        sortname: 'RequestNo',
        sortorder: 'asc',
        autowidth: true,
        caption: '<i class="fa fa-list">&nbsp;</i>ReportCard List',
        height: 220,
        rowList: [10, 20, 50, 100, 150, 200],
        //multiselect: true,
        viewrecords: true,
        loadComplete: function () {
            var table = this;

            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        //        ondblClickRow: function (rowid, iRow, iCol, e) {
        //            $(grid_selector).editGridRow(rowid, prmGridDialog);
        //        },
        gridComplete: function () {
            var rdata = $(grid_selector).getRowData();
            if (rdata.length > 0) {
                $('.T2ViewPDF').click(function () { ViewPDF($(this).attr('vGrade'), $(this).attr('vSemester'), $(this).attr('vId')); });
            }
        }
    });
    $(grid_selector).navGrid(pager_selector, { add: false, edit: false, del: false, search: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green' });

    $('#btnSearch').click(function () {
        if ($('#Grade').val() == 'Select') { ErrMsg('Please fill the Grade'); return false; }
        if (!isEmptyorNull($('#RequestNo').val()) || !isEmptyorNull($('#Name').val()) ||
                $('#Campus option:selected').val() != 'Select' || $('#Section option:selected').val() != 'Select' ||
                $('#Grade option:selected').val() != 'Select' || $('#RptStatus option:selected').val() != 'Select') {
            $('#ReportCardList').clearGridData();
            LoadSetGridParam($(grid_selector), "/AchievementReport/GetReportCardInbox?RequestNo=" + $('#RequestNo').val() +
            '&Name=' + $('#Name').val() +
            '&Campus=' + $('#Campus option:selected').val() +
            '&Section=' + $('#Section option:selected').val() +
            '&Grade=' + $('#Grade option:selected').val() +
            '&RptStatus=' + $('#RptStatus option:selected').val() +
             '&Semester=' + $('#Semester').val() +
              '&AcademicYear=' + $('#academicyear').val()
            );
        } else {
            ErrMsg('Please enter or select any values', function () { $("#RequestNo").focus(); });
            return false;
        }

    });

    $('#btnReset').click(function () {
        $('#ReportCardList').clearGridData();
        $('#RequestNo').val('');
        $('#Name').val('');
        $('#Campus').val('');
        $('#Section').val('Select');
        $('#Grade').val('Select');
        $('#RptStatus').val('Select');
        $('#academicyear').val('Select');
        $('#Semester').val('Select');
        LoadSetGridParam($(grid_selector), "/AchievementReport/GetReportCardInbox?page=1");
    });

    $('#btnNewReport').click(function () {
        return window.location.href = '/AchievementReport/ReportCardMYP/';
    });

    $('#btnRptFocus').click(function () {
        return window.location.href = '/AchievementReport/ReportCardFocus?Id=0';
    });



    $('#RequestNo').keypress(function (event) { if (isEmptyorNull($('#RequestNo').val())) { return true; } else { return defaultFunc(event); } });
    $('#Name').keypress(function (event) { if (isEmptyorNull($('#Name').val())) { return true; } else { return defaultFunc(event); } });
    $('#Campus').keypress(function (event) { if (isEmptyorNull($('#Campus').val())) { return true; } else { return defaultFunc(event); } });
    /*enter key press event*/

});

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
/*enter key press event*/
function defaultFunc(e) {
    if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
        $('#btnSearch').click();
        return false;
    }
    return true;
};

function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}


function GetCampus() {
    var ddlcam = $("#Campus");
    $.ajax({
        type: 'POST',
        async: true,
        dataType: "json",
        url: "/Base/FillBranchCode",
        success: function (data) {
            ddlcam.empty();
            ddlcam.append("<option value=''> Select </option>");
            for (var i = 0; i < data.length; i++) {
                ddlcam.append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
            }
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}