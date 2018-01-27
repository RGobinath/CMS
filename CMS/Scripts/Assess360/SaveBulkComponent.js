$(function () {
    var grid_selector = "#blkCompSaveLst";
    var pager_selector = "#blkCompSavePage";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $("#jqgrid").width());
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
    var lastSel2;
    function SBCValidation() {
        if (!isEmptyorNull($("#SBCSection").val()) && !isEmptyorNull($("#SBCGrade").val()) && !isEmptyorNull($("#SBCCampus").val()) && $('#SBCAssmntType option:selected').val() != 'Select' &&
                    $('#SBCAssmntType option:selected').val() == 'SLC Assessment' && !isEmptyorNull($('#SBCDatofAssgn').val()) && !isEmptyorNull($('#SBCAssmntName').val())) {
            return true;
        } else if (!isEmptyorNull($("#SBCSection").val()) && !isEmptyorNull($("#SBCGrade").val()) && !isEmptyorNull($("#SBCCampus").val()) && $('#SBCAssmntType option:selected').val() != 'Select' &&
                    $('#SBCAssmntType option:selected').val() != 'SLC Assessment' && !isEmptyorNull($('#SBCDatofAssgn').val()) && $('#SBCSubject option:selected').val() != 'Select' && !isEmptyorNull($('#SBCAssmntName').val()) && !isEmptyorNull($('#SBCStaff').val())) {
            return true;
        }
        ErrMsg('Please enter all the values.');
        return false;
    }
    $(function () {
        semhideshow("hide");
        $("#SBCSubject").change(function () {
            GetAssignmentNameByCampusGradeSubject();
            GetTermtestMarkWeightings();
        });
        $("#SBCAssmntName").change(function () {
            GetTermtestMarkWeightings();
        });
        $.getJSON("/Base/FillBranchCode", function (fillbc) {
            var ddlbc = $("#SBCCampus");
            ddlbc.empty();
            ddlbc.append($('<option/>', { value: "", text: "Select One" }));
            $.each(fillbc, function (index, itemdata) {
                ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
            });
        });
        $("#btnSBCStaffPopup").button({ icons: { primary: "ui-icon-search" },
            text: false
        });

        if ($('#loggedInUserType').val() == "SBCStaff") {
            $('#SBCStaff').val($('#loggedInUserName').val());
            $('#btnSBCStaffPopup').hide();
        }

        $('#btnSBCStaffPopup').click(function () {
            GetStaffPopup();
        });
        $("#SBCDatofAssgn").datepicker({
            format: 'dd-M-yy',
            autoclose: true
            //            showOn: "button",
            //            buttonImage: "../../Images/date.gif",
            //            buttonImageOnly: true
        });

        var SBCGrd = $(grid_selector);

        $("#SBCCampus").change(function () {
            if ($("#SBCCampus").val() == "IB MAIN" || $("#SBCCampus").val() == "CHENNAI MAIN" || $("#SBCCampus").val() == "KARUR") {
                semhideshow("show");
                GetTermtestMarkWeightings();
            }
            else {
                semhideshow("hide");
            }
            GetSubjectsByGrade('SBCSubject');
            GetIssueType('SBCAssmntType', '23');
        });
        $('#SBCGrade').change(function () {
            if ($("#SBCGrade").val() != "Select") {
                //GetIssueType('SBCAssmntType', '23');
                GetSubjectsByGrade('SBCSubject');
                GetTermtestMarkWeightings();
            }
        });

        //        $("#btnGetStdntLst").click(function () {
        //            var isValid = false;
        //            if ($('#SBCAssmntType option:selected').val() != 'Select' && $('#SBCAssmntType option:selected').text() == 'SLC Assessment' && !isEmptyorNull($('#SBCDatofAssgn').val()) && !isEmptyorNull($('#SBCAssmntName').val()) && !isEmptyorNull($('#totalmks').val())) {
        //                isValid = true;
        //            }
        //            else if ($('#SBCAssmntType option:selected').val() != 'Select' && $('#SBCAssmntType option:selected').text() != 'SLC Assessment' && !isEmptyorNull($('#SBCDatofAssgn').val()) && $('#SBCSubject option:selected').val() != '' &&
        //                    !isEmptyorNull($('#SBCAssmntName').val()) && !isEmptyorNull($('#SBCStaff').val()) && !isEmptyorNull($('#totalmks').val())) {
        //                if ($("#SBCCampus").val() != "IB MAIN") {
        //                    isValid = true;
        //                }
        //                else { if (!isEmptyorNull($('#SBCSemester').val())) { isValid = true; } }
        //            }

        //            if (isValid == true) {
        //                debugger;
        //                SBCGrd.clearGridData();
        //                var term = encodeURIComponent($('#SBCSemester option:selected').text()); // $('#SBCSemester').val();
        //                var subj = encodeURIComponent($('#SBCSubject option:selected').text());
        //                LoadSetGridParam(SBCGrd, '/Assess360/GetBulkAssess360Inbox?campus=' + $("#SBCCampus").val() + '&grade=' + $("#SBCGrade").val() + '&section=' + $("#SBCSection").val() + '&IncidentDate=' + $("#SBCDatofAssgn").val() + '&AssessCompGroupId=' + $("#SBCAssmntType option:selected").val() + '&AssignmentName=' + $("#SBCAssmntName").val() + '&EnteredBy=' + $("#lgdUserId").val() + '&GroupName=' + $("#SBCAssmntType option:selected").text() + '&Staff=' + $("#SBCStaff").val() + '&Subject=' + subj + '&totalmks=' + $("#totalmks").val() + '&semterm=' + term);
        //            } else {
        //                ErrMsg('Please Enter all values.', function () { $("#SBCGrade").focus(); });
        //                return false;
        //            }
        //        });
        $("#btnGetStdntLst").click(function () {
            debugger;
            var isValid = false;
            if ($('#SBCAssmntType option:selected').val() != 'Select' && $('#SBCAssmntType option:selected').text() == 'SLC Assessment' && !isEmptyorNull($('#SBCDatofAssgn').val()) && !isEmptyorNull($('#SBCAssmntName').val()) && !isEmptyorNull($('#totalmks').val())) {
                isValid = true;
            }
            else if ($('#SBCAssmntType option:selected').val() != 'Select' && $('#SBCAssmntType option:selected').text() != 'SLC Assessment' && !isEmptyorNull($('#SBCDatofAssgn').val()) && $('#SBCSubject option:selected').val() != '' &&
                    !isEmptyorNull($('#SBCAssmntName').val()) && !isEmptyorNull($('#SBCStaff').val()) && !isEmptyorNull($('#totalmks').val())) {
                if ($("#SBCCampus").val() != "IB MAIN" || $("#SBCCampus").val() != "CHENNAI MAIN" || $("#SBCCampus").val() != "KARUR") {
                    isValid = true;
                }
                else { if (!isEmptyorNull($('#SBCSemester').val())) { isValid = true; } }
            }

            if (isValid == true) {
                SBCGrd.clearGridData();
                var term = encodeURIComponent($('#SBCSemester option:selected').val()); // $('#SBCSemester').val();
                var subj = encodeURIComponent($('#SBCSubject option:selected').text());
                var assName = encodeURIComponent($('#SBCAssmntName option:selected').text());
                //$("#SBCAssmntName").val()
                LoadSetGridParam(SBCGrd, '/Assess360/GetBulkAssess360Inbox?campus=' + $("#SBCCampus").val() + '&grade=' + $("#SBCGrade").val() + '&section=' + $("#SBCSection").val() + '&IncidentDate=' + $("#SBCDatofAssgn").val() + '&AssessCompGroupId=' + $("#SBCAssmntType option:selected").val() + '&AssignmentName=' + assName + '&EnteredBy=' + $("#lgdUserId").val() + '&GroupName=' + $("#SBCAssmntType option:selected").text() + '&Staff=' + $("#SBCStaff").val() + '&Subject=' + subj + '&totalmks=' + $("#totalmks").val() + '&semterm=' + term);
            } else {
                if ($("#SBCCampus").val() == "") { ErrMsg('Please Select Campus.', function () { $("#SBCCampus").focus(); }); }
                else if ($("#SBCGrade").val() == "Select") { ErrMsg('Please Select Grade.', function () { $("#SBCGrade").focus(); }); }
                else if ($("#SBCSection").val() == "Select") { ErrMsg('Please Select Section.', function () { $("#SBCSection").focus(); }); }
                else if ($("#SBCStaff").val() == "") { ErrMsg('Please Select Staff.', function () { $("#SBCStaff").focus(); }); }
                else if ($("#SBCAssmntType").val() == "") { ErrMsg('Please Select Assessment.', function () { $("#SBCAssmntType").focus(); }); }
                else if ($("#SBCSubject").val() == "") { ErrMsg('Please Select Subject.', function () { $("#SBCSubject").focus(); }); }
                else if ($("#SBCDatofAssgn").val() == "") { ErrMsg('Please Select Date.', function () { $("#SBCDatofAssgn").focus(); }); }
                else if ($("#SBCAssmntName").val() == "") { ErrMsg('Please Select Assessment Name.', function () { $("#SBCAssmntName").focus(); }); }
                else if ($("#totalmks").val() == "") { ErrMsg('Please Enter Total Marks.', function () { $("#totalmks").focus(); }); }
                else { $("#btnGetStdntLst").focus(); }
                return false;
            }
        });

        $("#btnSaveSST").click(function () {
            if (isEmptyorNull($('#NameSSTName').val())) {
                ErrMsg('Please enter search template name', function () { $("#NameSSTName").focus(); });
                return false;
            }
            var varDtCrd = $('#SSTDateCreated').val();
            var varSavedSearch = //"RequestNo->" + $('#RequestNo').val() +
                                                "Campus->" + $('#SBCCampus option:selected').val() +
                                                "##Staff->" + $('#SBCStaff').val() +
                                                "##Section->" + $('#SBCSection option:selected').val() +
                                                "##AssessmentType->" + $('#SBCAssmntType option:selected').text() +
                                                "##Grade->" + $('#SBCGrade option:selected').val() +
                                                "##Subject->" + $('#SBCSubject option:selected').val() +
                                                "##DateOfAssignment->" + $('#SBCDatofAssgn').val() +
                                                "##AssignmentName->" + $('#SBCAssmntName option:selected').val() +
                                                "##TotalMarks->" + $('#totalmks').val();
            var ObjSvSrchTmplt = {
                Id: $('#varSSTId').val(),
                UserId: $('#lgdUserId').val(),
                SearchName: $('#NameSSTName').val(),
                Application: 'Assess360BulkEntry',
                SavedSearch: varSavedSearch,
                IsDefault: $('#IsDefaultSST').val(),
                DateCreated: varDtCrd,
                CreatedBy: $('#lgdUserId').val()
            };
            $.ajax({
                url: '/Assess360/SaveorUpdateSearchTemplate',
                type: 'POST',
                dataType: 'json',
                data: ObjSvSrchTmplt,
                traditional: true,
                success: function (data) {
                    GetSearchTemplate();
                    InfoMsg("Search template saved successfully", function () { });
                },
                error: function (xhr, status, error) {
                    ErrMsg($.parseJSON(xhr.responseText).Message);
                }
            });
        });
        $('#SavedSearchTempl').change(function () {
            if ($('#SavedSearchTempl option:selected').text() == 'Select') {
                $('#btnReset').click();
            } else {
                var stempl = new Array();
                stempl = $('#SavedSearchTempl option:selected').attr('SavedSearch').split('##');

                if ($('#SavedSearchTempl option:selected').attr('IsDefault') == 'true') {
                    $('#IsDefaultSST').attr("checked", true);
                    $('#SSTIsDefault').val('true');
                } else {
                    $('#IsDefaultSST').attr("checked", false);
                    $('#SSTIsDefault').val('false');
                }

                GetIssueType('SBCAssmntType', '23');
                //GetSubjectsByGrade('SBCSubject');

                for (var i = 0; i < stempl.length; i++) {
                    var Search = stempl[i].split('->');
                    if ($.trim(Search[0]) == 'DateOfAssignment')
                    { $('#SBCDatofAssgn').val(Search[1]); }
                    else if ($.trim(Search[0]) == 'Staff') {
                        if (!isEmptyorNull(Search[1]) != '') {
                            $('#SBCStaff').val(Search[1]);
                        }
                    }
                    else if ($.trim(Search[0]) == 'TotalMarks')
                    { $('#totalmks').val(Search[1]); }
                    else if ($.trim(Search[0]) == 'Campus') {
                        var vCampus = document.getElementById("SBCCampus");
                        for (var j = 0; j < vCampus.options.length; j++) {
                            if (vCampus.options[j].text == Search[1]) {
                                vCampus.selectedIndex = j;
                                if (Search[1] == "IB MAIN" || Search[1] == "CHENNAI MAIN") { semhideshow("show"); }
                                else { semhideshow("hide"); }
                            }
                        }
                    }
                    else if ($.trim(Search[0]) == 'Section') {
                        var vSection = document.getElementById("SBCSection");
                        for (var k = 0; k < vSection.options.length; k++) {
                            if (vSection.options[k].text == Search[1]) {
                                vSection.selectedIndex = k;
                            }
                        }
                    }
                    else if ($.trim(Search[0]) == 'Grade') {
                        var vGrade = document.getElementById("SBCGrade");
                        for (var g = 0; g < vGrade.options.length; g++) {
                            if (vGrade.options[g].text == Search[1]) {
                                vGrade.selectedIndex = g;
                            }
                        }
                        GetSubjectsByGrade('SBCSubject');
                    }
                    else if ($.trim(Search[0]) == 'Subject') {
                        var vSubject = document.getElementById("SBCSubject");
                        for (var g = 0; g < vSubject.options.length; g++) {
                            if (vSubject.options[g].text == Search[1]) {
                                vSubject.selectedIndex = g;
                            }
                        }
                    }
                    else if ($.trim(Search[0]) == 'AssessmentType') {
                        var vAssesmentType = document.getElementById("SBCAssmntType");
                        for (var g = 0; g < vAssesmentType.options.length; g++) {
                            if (vAssesmentType.options[g].text == Search[1]) {
                                vAssesmentType.selectedIndex = g;
                            }
                        }
                    }
                    else if ($.trim(Search[0]) == 'AssignmentName') {
                        var vAsmntName = document.getElementById("SBCAssmntName");
                        for (var g = 0; g < vAsmntName.options.length; g++) {
                            if (vAsmntName.options[g].text == Search[1]) {
                                vAsmntName.selectedIndex = g;
                            }
                        }
                    }
                }
                //  $('#btnGetStdntLst').click();
                SBCGrd.clearGridData();
                var term = $('#SBCSemester option:selected').text();
                var subj = encodeURIComponent($('#SBCSubject option:selected').text());
                LoadSetGridParam(SBCGrd, '/Assess360/GetBulkAssess360Inbox?campus=' + $("#SBCCampus").val() + '&grade=' + $("#SBCGrade").val() + '&section=' + $("#SBCSection").val() + '&IncidentDate=' + $("#SBCDatofAssgn").val() + '&AssessCompGroupId=' + $("#SBCAssmntType option:selected").val() + '&AssignmentName=' + $("#SBCAssmntName").val() + '&EnteredBy=' + $("#lgdUserId").val() + '&GroupName=' + $("#SBCAssmntType option:selected").text() + '&Staff=' + $("#SBCStaff").val() + '&Subject=' + subj + '&totalmks=' + $("#totalmks").val(), '&Term=' + term);
            }
        });
        /*Save Search Template Code*/

        $('#btnReset').click(function () {
            var IssueCredits = $('#hdnIsCredit').val();
            $(grid_selector).clearGridData();
            //    $('#RequestNo').val('');
            $('#SBCDatofAssgn').val('');
            $('#SBCCampus').val('');
            $('#SBCSection').val('Select');
            $('#SBCGrade').val('Select');
            $('#SavedSearchTempl').val('Select');
            $('#IsDefaultSST').attr("checked", false);
            $('#SSTIsDefault').val('false');
            //$('#SBCStaff').val('');
            //$('#SBCStaffUserName').val('');
            $('#SBCSubject').val('Select');
            semhideshow("hide");
            $('#SBCAssmntName').val('Select');
            $('#totalmks').val('');
            $('#SBCAssmntType').val('Select');
            // LoadSetGridParam($('#blkCompSaveLst'), "/Assess360/GetAssess360CompMasterListByName?tab=&IssueCredits=" + IssueCredits);

            SBCGrd.clearGridData();
        });

        var AssignmentName = $('#SBCAssmntName').val();
        var obtndmrks = '';
        var totalmks = '';
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

        function checkvalid1(value, column) {
            if (value == '') {
                return [false, column + ": Field is Required"];
            }
            else if (!$.isNumeric(value)) {
                return [false, column + 'Should be numeric'];
            }
            else if (parseInt(value) < 1) {
                return [false, column + 'Should be Greater than 0'];
            }
            else if (parseInt(obtndmrks) > value) {
                return [false, column + ' Should be greater than Obtained marks'];
            }
            else {
                return [true];
            }
            $(grid_selector).jqGrid('getCell', row_id, 'column_name');
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

        function nildata1(cellvalue, options, rowObject) {
            document.getElementById('totalmks').value = cellvalue; // to assign values to total mks textbox
            if ((cellvalue == '') || (cellvalue == null)) {
                return ''
            }
            else {
                cellvalue = cellvalue.replace('&', 'and');
                //str = str.replace(/\&lt;/g, '<');
                return cellvalue
            }
        }
        GetSearchTemplate();
        $(grid_selector).jqGrid({
            mtype: 'POST',
            height: 180,
            autowidth: true,
            colNames: ['Id', 'RequestNo', 'Academic Year', 'Campus Name', 'Id No', 'Name', 'Section', 'Grade',
                            'Consolidated Marks', 'Date Created', 'A360CompId', 'Obtained Marks', 'Total Marks'],
            colModel: [
                          { name: 'Id', index: 'Id', hidden: true, key: true },
                          { name: 'RequestNo', index: 'RequestNo' },
                          { name: 'AcademicYear', index: 'AcademicYear', hidden: true },
                          { name: 'Campus', index: 'Campus' },
                          { name: 'IdNo', index: 'IdNo', sortable: false },
                          { name: 'Name', index: 'Name', editable: false, sortable: false },
                          { name: 'Section', index: 'Section' },
                          { name: 'Grade', index: 'Grade' },
                          { name: 'ConsolidatedMarks', index: 'ConsolidatedMarks', hidden: true },
                          { name: 'DateCreated', index: 'DateCreated', hidden: true },
                          { name: 'A360CompId', index: 'A360CompId', edithidden: true, hidden: true },
                          { name: 'ObtainedMarks', index: 'ObtainedMarks', editable: true, edittype: 'text', editrules: { custom: true, custom_func: checkvalid }, formatter: nildata },
                          { name: 'TotalMarks', index: 'TotalMarks', editrules: { custom: true, custom_func: checkvalid1 }, formatter: nildata1 }
                          ],
            pager: pager_selector,
            rowNum: '30',
            sortname: 'Name',
            sortorder: 'asc',
            rowList: [30, 50, 70, 100],
            viewrecords: true,
            editurl: "/Assess360/SaveBulkA360Components",
            caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;Bulk Component List',
            onSelectRow: function (id) {
                if (lastSel2) {
                    $(grid_selector).jqGrid('restoreRow', lastSel2);
                }
                if (id && id !== lastSel2) {
                    compId = $(grid_selector).getRowData(id).A360CompId == "" ? 0 : $(grid_selector).getRowData(id).A360CompId;
                    $(grid_selector).editRow(id, true, null, checksaveNew, null, { A360CompId: compId });
                    lastSel2 = id;
                }
            },
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(window).triggerHandler('resize.jqGrid');
            }
        });
        //navButtons Add, edit, delete
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
            },
            {},
            {}, {}, {})
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
        //$("#blkCompSaveLst").navGrid('#blkCompSavePage', { add: false, edit: false, del: false, search: false, refresh: false });
        jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: "Export To Excel",
            onClickButton: function () {
                debugger;
                window.open("GetBulkAssess360Inbox" + '?campus=' + $("#SBCCampus").val() + '&grade=' + $("#SBCGrade").val() + '&section=' + $("#SBCSection").val() + '&IncidentDate=' + $("#SBCDatofAssgn").val() + '&AssessCompGroupId=' + $("#SBCAssmntType option:selected").val() + '&AssignmentName=' + $("#SBCAssmntName").val() + '&EnteredBy=' + $("#lgdUserId").val() + '&GroupName=' + $("#SBCAssmntType option:selected").text() + '&Staff=' + $("#SBCStaff").val() + '&Subject=' + $("#SBCSubject").val() + '&totalmks=' + $("#totalmks").val() + '&semterm=' + $("#SBCSemester").val() + '&ExportType=Excel&rows=9999');
            }
        });
        jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: "Export To PDF",
            onClickButton: function () {
                window.open("GetBulkAssess360Inbox" + '?campus=' + $("#SBCCampus").val() + '&grade=' + $("#SBCGrade").val() + '&section=' + $("#SBCSection").val() + '&IncidentDate=' + $("#SBCDatofAssgn").val() + '&AssessCompGroupId=' + $("#SBCAssmntType option:selected").val() + '&AssignmentName=' + $("#SBCAssmntName").val() + '&EnteredBy=' + $("#lgdUserId").val() + '&GroupName=' + $("#SBCAssmntType option:selected").text() + '&Staff=' + $("#SBCStaff").val() + '&Subject=' + $("#SBCSubject").val() + '&totalmks=' + $("#totalmks").val() + '&semterm=' + $("#SBCSemester").val() + '&ExportType=PDF&rows=9999');
            }
        });

        function checksaveNew(data, status) {
            if (data.status == 200) {
                var vA360CompId = data.responseText;
                $(grid_selector).jqGrid("setCell", lastSel2, 'A360CompId', vA360CompId);
                $(grid_selector).trigger("reloadGrid");
                return true;
            } else {
                return false;
            }
        }
        $('#SBCAssmntType').change(function () {
            $('#hdnIsCredit').val($('#SBCAssmntType option:selected').attr('IsCredit'));
            if ($('#SBCAssmntType option:selected').text() == "SLC Assessment") {
                $('#SBCSubject').attr('disabled', 'disabled');
                $('#btnSBCStaffPopup').hide();
                $('#SBCStaff').val('');
                $('#SBCStaffUserName').val('');
                if ($("#SBCCampus").val() == "IB MAIN" || $("#SBCCampus").val() == "CHENNAI MAIN"|| $("#SBCCampus").val() == "KARUR") {
                    semhideshow("hide");
                }
                else {
                    semhideshow("show");
                }
                $("#SBCAssmntName").empty();
                $("#SBCAssmntName").append("<option value=''> Select One </option>");
                for (var i = 1; i < 6; i++) {
                    $("#SBCAssmntName").append("<option value='SLC Asessment" + i + "'>SLC Asessment" + i + "</option>");
                }
            } else {
                $('#SBCSubject').removeAttr('disabled');
                $('#btnSBCStaffPopup').show();
                if ($("#AdminRole").val() != "All") {
                    $("#SBCStaff").val();
                }
                if ($("#SBCCampus").val() == "IB MAIN" || $("#SBCCampus").val() == "CHENNAI MAIN" || $("#SBCCampus").val() == "KARUR") {
                    semhideshow("show");
                    GetTermtestMarkWeightings();
                }
                else {
                    semhideshow("hide");
                }
            }
        });
    });
    function semhideshow(value) {
        if (value == "show") {
            $('#divSBCSemester').show();
        } else {
            $('#divSBCSemester').hide();
        }
    }
    function GetIssueType(drpdwnId, AssessCompGrp, SelVal) {
        var IssueCredits = $('#hdnIsCredit').val();
        $.ajax({
            type: 'GET',
            async: false,
            dataType: "json",
            url: '/Assess360/GetAssess360CompMasterListByName?tab=' + AssessCompGrp + '&IssueCredits=' + IssueCredits,
            success: function (data) {
                $('#' + drpdwnId).empty();
                $('#' + drpdwnId).append("<option value='' IsCredit='' Marks=''> Select </option>");
                var varval = "";
                for (var i = 0; i < data.rows.length; i++) {
                    varval = data.rows[i].Value;
                    if (varval == "3" || varval == "4" || varval == "5" || varval == "6" || varval == "7" || (($("#SBCCampus").val() == "IB MAIN" ||$("#SBCCampus").val()=="CHENNAI MAIN"||$("#SBCCampus").val() == "KARUR")&& varval == "8" || varval == "9" || varval == "10")) {
                        $('#' + drpdwnId).append("<option value='" + varval + "' IsCredit='" + data.rows[i].IsCredit + "' Marks='" + data.rows[i].Marks + "'>" + data.rows[i].Text + "</option>");
                    }
                }
            },
            error: function (xhr, status, error) {
                ErrMsg($.parseJSON(xhr.responseText).Message);
            }
        });
    }

    function GetSubjectsByGrade(drpdwnId) {
        if (!isEmptyorNull($('#SBCCampus').val()) && !isEmptyorNull($('#SBCGrade').val())) {
            $.ajax({
                type: 'GET',
                async: false,
                dataType: "json",
                url: '/Assess360/GetSubjectsByGrade?Campus=' + $("#SBCCampus").val() + '&Grade=' + $('#SBCGrade').val(),
                success: function (data) {
                    $('#' + drpdwnId).empty();
                    $('#' + drpdwnId).append("<option value=''> Select </option>");
                    for (var i = 0; i < data.rows.length; i++) {
                        $('#' + drpdwnId).append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
                    }
                },
                error: function (xhr, status, error) {
                    ErrMsg($.parseJSON(xhr.responseText).Message);
                }
            });
        }
    }

    function GetStaffPopup() {
        var grade = ''; // $('#Grade').val();
        ModifiedLoadPopupDynamicaly("/Assess360/loadPartialView?PartialViewName=StaffMasterPopup", $('#DivStaffMasterSearch'),
              function () {
                  $('#StaffGrade').val(grade);
                  LoadSetGridParam($('#StaffMasterList'), "/Assess360/GetStaffMasterDetails?Grade=" + grade);
              }, function (rdata) { $('#SBCStaffUserName').val(rdata.UserName); $('#SBCStaff').val(rdata.UserId); }, 600, 345, "Staff Name")
    }

    function GetSearchTemplate() {
        var varlgdUserId = $('#lgdUserId').val();
        var drpdwnId = 'SavedSearchTempl';
        $.ajax({
            type: 'GET',
            async: true,
            dataType: "json",
            url: "/Assess360/GetSavedSearchTemplate?UserId=" + varlgdUserId,
            success: function (data) {
                $('#' + drpdwnId).empty();
                $('#' + drpdwnId).append("<option value='Select' DateCreated='' IsDefault='' SavedSearch='' > Select </option>");
                for (var i = 0; i < data.rows.length; i++) {
                    if (data.rows[i].IsDefault == true) {
                        $('#' + drpdwnId).append("<option value='" + data.rows[i].Value + "' DateCreated='" + data.rows[i].DateCreated + "' IsDefault='" + data.rows[i].IsDefault + "' SavedSearch='" + data.rows[i].SavedSearch + "' selected='selected'>" + data.rows[i].Text + "</option>");
                        $('#SavedSearchTempl').change();
                    } else {
                        $('#' + drpdwnId).append("<option value='" + data.rows[i].Value + "' DateCreated='" + data.rows[i].DateCreated + "' IsDefault='" + data.rows[i].IsDefault + "' SavedSearch='" + data.rows[i].SavedSearch + "'>" + data.rows[i].Text + "</option>");
                    }
                }
            },
            error: function (xhr, status, error) {
                ErrMsg($.parseJSON(xhr.responseText).Message);
            }
        });
    }
    function GetAssignmentNameByCampusGradeSubject() {
        var cam = $("#SBCCampus").val();
        var gra = $("#SBCGrade").val();
        var sub = $("#SBCSubject").val();
        $.ajax({
            type: 'POST',
            async: false,
            url: "/Assess360/GetAssignmentNameByCampusGradeSubject?cam=" + cam + '&gra=' + gra + '&sub=' + encodeURIComponent(sub),
            success: function (data) {
                $("#SBCAssmntName").empty();
                $("#SBCAssmntName").append("<option value=''> Select One </option>");
                for (var i = 0; i < data.rows.length; i++) {
                    $("#SBCAssmntName").append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
                }
            },
            dataType: "json"
        });
    }
    function GetTermtestMarkWeightings() {
        var cam = $("#SBCCampus").val();
        var gra = $("#SBCGrade").val();
        var sub = $("#SBCSubject").val();
        var assmntname = $("#SBCAssmntName").val();
        if (!isEmptyorNull(cam) && (gra == "IX" || gra == "X") && !isEmptyorNull(sub) && !isEmptyorNull(assmntname) && assmntname != 'select' && ($('#SBCAssmntType option:selected').text() == "Term Assessment" || $('#SBCAssmntType option:selected').text() == "SA")) {
            $.ajax({
                type: 'POST',
                async: false,
                dataType: "json",
                url: "/Assess360/GetMarkWeightingsbyCampusGrade?Campus=" + cam + '&Grade=' + gra + '&Subject=' + sub + '&AssignmntName=' + assmntname,
                success: function (data) {
                    if (data != null) {
                        $("#totalmks").val(data);
                        $('#totalmks').attr('disabled', 'disabled');
                    }
                    else {
                        $("#totalmks").val('');
                        $('#totalmks').attr('disabled', false);
                    }
                }
            });
        }
        else {
            $("#totalmks").val('');
            $('#totalmks').attr('disabled', false);
        }
    }
});