$(function () {


    $('#btnAssessBack2Inbox').click(function () {
        var url = $("#BackUrl").val();
        window.location.href = url;
    });
    $.getJSON("/Base/FillBranchCode",
     function (fillig) {
         var ddlcam = $("#Campus");
         ddlcam.empty();
         ddlcam.append($('<option/>', { value: "", text: "Select One" }));
         $.each(fillig, function (index, itemdata) {
             ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
         });
     });
    /*student & Staff details popup code*/
    $("#btnStdnPopup").button({ icons: { primary: "ui-icon-search" },
        text: false
    });
    if ($('#Id').val() > 0) {
        $('#Campus').attr('disabled', true);
        GetAssess360MarksForAStudent($('#Id').val());
    }
    //    $("#btnAssessBack2Inbox").click(function () {
    //        window.location.href = '@Url.Action("Assess360Inbox", "Assess360")';
    //    });
    $("#btnStdnPopup").click(function () {
        var c_name = $('#Campus option:selected').val();
        if (isEmptyorNull(c_name)) {
            ErrMsg('Please select Campus', function () { $("#Campus").focus(); });
            return false;
        } else {
            ModifiedLoadPopupDynamicaly(
            "/Assess360/loadPartialView?PartialViewName=StudentPopup", $('#DivStudentSearch'),
            function () {
                $('#StdntCampus').val(c_name);
                LoadSetGridParam($('#StudentList'), "/Assess360/GetStudentDetails?Campus=" + c_name)
            },
            function (rdata) {

                $('#Name').val(rdata.Name);
                $('#IdNo').val(rdata.IdNo);
                $('#Grade').val(rdata.Grade);
                $('#Section').val(rdata.Section);
                $('#StudentId').val(rdata.Id);
                $('#AcademicYear').val(rdata.AcademicYear);
            }, 1300, 370, "Student List");
        }
    });
    /*student & Staff details popup code*/

    /* tab related code */
    function loadTabDynamically(PartialViewName, CallBackfun) {
        $('#partialPlaceHolder').empty();
        /* Request the partial view with .get request. */
        $.ajax({
            url: '/Assess360/loadPartialView?PartialViewName=' + PartialViewName,
            type: 'GET',
            async: false,
            dataType: 'html', // <-- to expect an html response
            success: function (data) {
                /* data is the pure html returned from action method, load it to your page */
                $('#partialPlaceHolder').html(data);
                /* do the call function */
                if (CallBackfun != undefined && CallBackfun) {
                    CallBackFunction(CallBackfun);
                }
            }
        });
    }

    $('#CharacterBehaviour').click(function () {
        if ($('#Id').val() > 0) {
            $('#partialPlaceHolder').empty();
            loadTabDynamically('CharacterBehaviour');
        }
    });
    $('#HomeWorkRelated').click(function () {
        if ($('#Id').val() > 0) {
            $('#partialPlaceHolder').empty();
            loadTabDynamically('HomeWorkRelated');
            GetIssueType('HWRAssmntType', '2');
            GetSubjectsByGrade('HWRSubject');
            /*
            This method added by Lee on 22-Jun-2013, 
            Earlier this is a free text, 
            now it has been turned to Master data.
            */
            // GetAssignmentName('HWRAssmntName', null);
        }
    });
    $('#StudentAchieveTab').click(function () {
        if ($('#Id').val() > 0) {
            $('#partialPlaceHolder').empty();
            loadTabDynamically('StudentAchievement');
            /*This method added by micheal on 02-Dec-2014, 
            Earlier this is a free text, 
            now it has been turned to Master data.*/
        }
    });
    $('#TestsAssessments').click(function () {
        if ($('#Id').val() > 0) {
            $('#partialPlaceHolder').empty();
            loadTabDynamically('TestsAssessments');
            GetIssueType('TAAssmntType', '3');
            GetSubjectsByGrade('TASubject');
            /*
            This method added by Lee on 22-Jun-2013, 
            Earlier this is a free text, 
            now it has been turned to Master data.
            */
            //  GetAssignmentName('TAAssmntName', null);
        }
    });
    $('#StudentMarksTab').click(function () {
        if ($('#Id').val() > 0) {
            $('#partialPlaceHolder').empty();
            loadTabDynamically('StudentMarks');
            GetConsolidatedMarksForAStudent($('#Id').val());
        }
    });

    function GetConsolidatedMarksForAStudent(Assess360Id) {

        $.ajax({
            url: '/Assess360/GetConsolidatedMarksForAStudent?Assess360Id=' + Assess360Id,
            type: 'GET',
            dataType: 'json',
            traditional: true,
            success: function (data) {

                var compArry = new Array();
                compArry = data.split(',');

                $('#MarksAwarded').html(compArry[0]);
                $('#StudentTotalMarksObtn').val(compArry[0].substring(0, compArry[0].lastIndexOf("/")));
                $('#StudentTotalMarks').val(compArry[0].substring(compArry[0].lastIndexOf("/") + 1, compArry[0].length));
                var stntTtlMrksObtn = $('#StudentTotalMarksObtn').val();
                var stntTtlMrks = $('#StudentTotalMarks').val();
                var CharactercylinderVal = compArry[1] == null ? 20 : compArry[1]; // (compArry[1] > 20 ? 20 : compArry[1]);
                var APcylinderVal = compArry[2] == null ? 10 : compArry[2]; // (compArry[2] > 10 ? 10 : compArry[2]);
                var HCcylinderVal = compArry[3] == null ? 5 : compArry[3]; //(compArry[3] > 5 ? 5 : compArry[3]);
                var HAcylinderVal = compArry[4] == null ? 15 : compArry[4]; // (compArry[4] > 15 ? 15 : compArry[4]);
                var WCTcylinderVal = compArry[5] == null ? 20 : compArry[5]; //: (compArry[5] > 20 ? 20 : compArry[5]);
                var SPAcylinderVal = compArry[6] == null ? 5 : compArry[6]; // (compArry[6] > 5 ? 5 : compArry[6]);
                var TAcylinderVal = compArry[7] == null ? 25 : compArry[7]; // (compArry[7] > 25 ? 25 : compArry[7]);

                $("#progressbar").slider({
                    range: "min",
                    value: 0,
                    min: 1,
                    max: 100
                });
                // 
                var prgsClr = "yellow";
                if (stntTtlMrksObtn < 75) {
                    prgsClr = "red";
                } else if (stntTtlMrksObtn < 100) {
                    prgsClr = "yellow";
                } else {
                    prgsClr = "green";
                }


                $("#progressbar").slider("disable");
                $("#progressbar").slider({ value: stntTtlMrksObtn, max: stntTtlMrks });
                //HLinearGauge.swf

                /* Character Chart */
                var Charactercylinder = "<chart manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='20' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#01DF01' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                Charactercylinder += "<value>" + CharactercylinderVal + "</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                //Charactercylinder += "<value> <a id='CompId1'> " + CharactercylinderVal + "</a></value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                Charactercylinder += "</annotationGroup></annotations></chart>";


                var Characterchart = new FusionCharts("../../Charts/Cylinder.swf", "Assess 360", "100", "200");
                Characterchart.setDataXML(Charactercylinder);
                Characterchart.render("CharacterChart");

                /* Attendnce Punctuality Chart */
                var APcylinder = "<chart manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='10' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#FA58D0' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                APcylinder += "<value>" + APcylinderVal + "</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                APcylinder += "</annotationGroup></annotations></chart>";

                var APchart = new FusionCharts("../../Charts/Cylinder.swf", "Assess 360", "100", "200");
                APchart.setDataXML(APcylinder);
                APchart.render("AttendncePunctualityChart");

                /* Homework Completion */
                var HCcylinder = "<chart manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='5' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#FACC2E' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                HCcylinder += "<value>" + HCcylinderVal + "</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                HCcylinder += "</annotationGroup></annotations></chart>";

                var HCchart = new FusionCharts("../../Charts/Cylinder.swf", "Assess 360", "100", "200");
                HCchart.setDataXML(HCcylinder);
                HCchart.render("HomeworkCompletionChart");

                /* Homework Accuracy */
                var HAcylinder = "<chart manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='15' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#81F7F3' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                HAcylinder += "<value>" + HAcylinderVal + "</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                HAcylinder += "</annotationGroup></annotations></chart>";

                var HAchart = new FusionCharts("../../Charts/Cylinder.swf", "Assess 360", "100", "200");
                HAchart.setDataXML(HAcylinder);
                HAchart.render("HomeworkAccuracyChart");

                /* Weekly / Chapter Tests */
                var WCTcylinder = "<chart manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='20' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#8181F7' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                WCTcylinder += "<value>" + WCTcylinderVal + "</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                WCTcylinder += "</annotationGroup></annotations></chart>";

                var WCTchart = new FusionCharts("../../Charts/Cylinder.swf", "Assess 360", "100", "200");
                WCTchart.setDataXML(WCTcylinder);
                WCTchart.render("WeeklyChapterTestsChart");

                /*SLC Parent Assessment */
                var SPAcylinder = "<chart manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='5' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#FA5858' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                SPAcylinder += "<value>" + SPAcylinderVal + "</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                SPAcylinder += "</annotationGroup></annotations></chart>";

                var SPAchart = new FusionCharts("../../Charts/Cylinder.swf", "Assess 360", "100", "200");
                SPAchart.setDataXML(SPAcylinder);
                SPAchart.render("SLCParentAssessmentChart");

                /*Terms Assessment */
                var TAcylinder = "<chart manageResize='1' bgColor='FFFFFF' bgAlpha='0' showBorder='0' lowerLimit='0' upperLimit='25' showTickMarks='1' showTickValues='1' showLimits='1' decmials='0' cylFillColor='#A4A4A4' baseFontColor='CC0000' chartLeftMargin='10' chartRightMargin='10' chartTopMargin='10'>";
                TAcylinder += "<value>" + TAcylinderVal + "</value><annotations><annotationGroup showBelow='1'><annotation type='rectangle' x='60' y='60' toX='60' toY='60' color='FFFFFF' alpha='20' showBorder='1' borderColor='CC0000' borderThickness='2' radius='10'/>";
                TAcylinder += "</annotationGroup></annotations></chart>";

                var TAchart = new FusionCharts("../../Charts/Cylinder.swf", "Assess 360", "100", "200");
                TAchart.setDataXML(TAcylinder);
                TAchart.render("TermsAssessmentChart");

            },
            error: function (xhr, status, error) {
                ErrMsg($.parseJSON(xhr.responseText).Message);
            }
        });
    }

    $("#Access360Tab").tabs();
    var tablst = null;
    $("#Access360Tab").find('a').click(function () {
        if (tablst) {
            tablst.tabs("enable");
            $("#Access360Tab ul li a").removeClass("selected-link"); //Remove any "selected-link" class
            $(this).find("a").addClass("selected-link");
        }
        $(this).tabs("disable");
        tablst = $(this);
    });
    setTimeout(function () { tablst = $("#CharacterBehaviour").click().tabs('disable'); }, 200);
    /* tab related code */

    /* Form submission code */
    $('#btnAssessSave').click(function () {
        // 
        if ($('#StudentId').val() > 0) {
            var objAssess360 = {
                //RequestNo: $('#RequestNo').val(),
                DateCreated: $('#DateCreated').val(),
                CreatedBy: $('#CreatedBy').val(),
                UserRole: $('#UserRole').val(),
                Status: $('#Status').val(),
                StudentId: $('#StudentId').val(),
                Name: $('#Name').val(),
                Campus: $('#Campus').val(),
                Section: $('#Section').val(),
                Grade: $('#Grade').val(),
                AcademicYear: $('#AcademicYear').val(),
                IdNo: $('#IdNo').val()
            };
            $.ajax({
                url: '/Assess360/SaveAssess360',
                type: 'POST',
                dataType: 'json',
                data: objAssess360,
                traditional: true,
                success: function (data) {
                    $('#RequestNo').val(data);
                    $('#Id').val(data.substring(data.lastIndexOf("-") + 1, data.length));
                    $('#btnStdnPopup').hide();
                    $('#btnAssessSave').hide();
                    $('#CharacterBehaviour').click();
                    InfoMsg("Assess 360 request created successful, Request No : " + data, function () { });
                },
                error: function (xhr, status, error) {
                    ErrMsg($.parseJSON(xhr.responseText).Message);
                }
            });
        } else {
            ErrMsg('Please select Student from popup.', function () { $("#Campus").focus(); });
            return false;
        }
    });
    /* Form submission code */




});



function GetAssess360MarksForAStudent(Assess360Id) {
    $.ajax({
        url: '/Assess360/GetAssess360MarksForAStudent?Assess360Id=' + Assess360Id,
        type: 'GET',
        dataType: 'json',
        traditional: true,
        success: function (data) {
            $('#Assess360MarksAwarded').html(data);
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}

function DeleteComponentIds(delURL, reloadURL, GridId) {
    $.ajax({
        url: delURL,
        type: 'POST',
        dataType: 'json',
        traditional: true,
        success: function (data) {

            LoadSetGridParam(GridId, reloadURL);
            InfoMsg(data, function () { });
        },
        loadError: function (xhr, status, error) {
            msgError = $.parseJSON(xhr.responseText).Message;
            ErrMsg(msgError, function () { });
        }
    });
}
/* saving of component detials */
function SaveOrUpdateAssess360Component(MstrComp) {
    // 
    var isSuccess = false, objAssess360Cmpnt = '', IsCredit = $('#hdnIsCredit').val(), AssessCompGroup = '', IncidentDateVal = '';
    var Subject = '', AssignmentName = '', MarksOutOff = '', GroupName = '', MarksVal = '', DescriptionVal = '', semester = '';
    var Staff = $('#Staff').val(), tab = 3;
    var IncidentDateValdt = new Date();
    if (MstrComp == "CharacterBehaviour") {
        tab = 1;
        IsCredit = $('#Assessment').val(),
            AssessCompGroup = $('#IssuesCredits option:selected').val();
        IncidentDateVal = $('#IncidentDate').val(), MarksVal = $('#Marks').val(), DescriptionVal = $('#Description').val();
        GroupName = $('#IssuesCredits option:selected').text();

        if (AssessCompGroup != "Select" && !isEmptyorNull(GroupName) && GroupName == "Others" && !isEmptyorNull(IsCredit) && !isEmptyorNull(IncidentDateVal) &&
                !isEmptyorNull($('#OtherMarks').val()) && !isEmptyorNull(DescriptionVal) && !isEmptyorNull(Staff)) {
            MarksVal = $('#OtherMarks').val();
            isSuccess = true;
        } else if (AssessCompGroup != "Select" && !isEmptyorNull(GroupName) && GroupName != "Others" && !isEmptyorNull(IsCredit) && !isEmptyorNull(IncidentDateVal) &&
                !isEmptyorNull(MarksVal) && !isEmptyorNull(DescriptionVal) && !isEmptyorNull(Staff)) {
            isSuccess = true;
        }
    } else if (MstrComp == "HomeWorkRelated") {
        tab = 2;
        AssessCompGroup = $('#HWRAssmntType option:selected').val(), Subject = $('#HWRSubject option:selected').text();
        AssignmentName = $('#HWRAssmntName option:selected').val(), MarksOutOff = $('#HWRTotalMarks').val(), MarksobtnVal = $('#HWRobtainedMarks').val();
        IncidentDateVal = $('#HWRDatofAssgn').val(), MarksVal = $('#HWRMarks option:selected').val();
        GroupName = $('#HWRAssmntType option:selected').text();
        if (AssessCompGroup != "Select" && GroupName == "Homework score" && Subject != 'Select' && !isEmptyorNull(IncidentDateVal)
            && !isEmptyorNull(MarksVal) && !isEmptyorNull(AssignmentName) && !isEmptyorNull(Staff) && !isEmptyorNull(MarksobtnVal) && !isEmptyorNull(MarksOutOff)) {
            if ($("#hdnCampus").val() != "IB MAIN" || $('#hdnCampus').val() != "CHENNAI MAIN") {
                isSuccess = true;
                MarksVal = MarksobtnVal;
            }
            else {
                semester = $('#HWSemester').val();
                if (!isEmptyorNull(semester)) {
                    isSuccess = true;
                    MarksVal = MarksobtnVal;
                }
            }
            if ((MarksOutOff - MarksVal) < 0) {
                ErrMsg("Please enter the obtained marks less then or equal to total marks.", function () { $('#TAobtainedMarks').focus(); });
                return false;
            }
        } else if (AssessCompGroup != "Select" && Subject != 'Select' && !isEmptyorNull(IncidentDateVal) &&
                MarksVal != "Select" && !isEmptyorNull(MarksVal) && !isEmptyorNull(GroupName) && !isEmptyorNull(AssignmentName) && !isEmptyorNull(Staff)) {
            isSuccess = true;
        }
    } else if (MstrComp == "TestsAssessments") {
        tab = 3;
        AssessCompGroup = $('#TAAssmntType option:selected').val(), Subject = $('#TASubject option:selected').text();
        AssignmentName = $('#TAAssmntName option:selected').val(), MarksOutOff = $('#TATotalMarks').val();
        IncidentDateVal = $('#TADatofAssgn').val(), MarksVal = $('#TAobtainedMarks').val();
        GroupName = $('#TAAssmntType option:selected').text();
        if (GroupName != "SLC Assessment" && AssessCompGroup != "Select" && Subject != 'Select' && !isEmptyorNull(IncidentDateVal) && !isEmptyorNull(Staff) &&
                !isEmptyorNull(MarksVal) && !isEmptyorNull(GroupName) && !isEmptyorNull(AssignmentName) && !isEmptyorNull(MarksOutOff)) {
            if ($("#hdnCampus").val() != "IB MAIN" || $('#hdnCampus').val() != "CHENNAI MAIN") {
                isSuccess = true;
            }
            else {
                semester = $('#TASemester').val();
                if (!isEmptyorNull(semester)) {
                    isSuccess = true;
                }
            }
        } else if (GroupName == "SLC Assessment" && AssessCompGroup != "Select" && !isEmptyorNull(IncidentDateVal) &&
                !isEmptyorNull(MarksVal) && !isEmptyorNull(GroupName) && !isEmptyorNull(AssignmentName) && !isEmptyorNull(MarksOutOff)) {
            Staff = "Parent";
            Subject = '';
            isSuccess = true;
        }
        if (isSuccess && ((MarksOutOff - MarksVal) < 0)) {
            ErrMsg("Please enter the obtained marks less then or equal to total marks.", function () { $('#TAobtainedMarks').focus(); });
            return false;
        }
    }

    if (isSuccess) {
        objAssess360Cmpnt = {
            Assess360Id: $('#Id').val(),
            Id: $('#Assess360ComponentId').val(),
            DateCreated: $('#A360CompCreatedDate').val(),
            AssessCompGroup: AssessCompGroup,
            IsCredit: IsCredit,
            IncidentDate: IncidentDateVal.toString(),
            Mark: MarksVal,
            Description: DescriptionVal,
            GroupName: GroupName,
            Subject: Subject,
            AssignmentName: AssignmentName,
            MarksOutOff: MarksOutOff,
            Staff: Staff,
            Semester: semester,
            EnteredBy: $('#loggedInUserId').val()
        };
    } else {
        ErrMsg('Please enter all the values.');
        return false;
    }
    var refreshGrid = '/Assess360/GetAssess360ComponentListWithPagingAndCriteria?Assess360Id=' + $('#Id').val() + "&tab=" + tab;
    $.ajax({
        url: '/Assess360/SaveOrUpdateAssess360Component?StudentName=' + $('#Name').val() + '&StudentId=' + $('#StudentId').val(),
        type: 'POST',
        dataType: 'json',
        data: objAssess360Cmpnt,
        traditional: true,
        success: function (data) {
            // 
            if (data && data > 0) {
                GetAssess360MarksForAStudent($('#Id').val());
                if (parseInt($('#Assess360ComponentId').val()) > 0) {
                    InfoMsg("Update sucessfully");
                } else {
                    InfoMsg("Saved sucessfully");
                }
                $('#Assess360ComponentId').val('');
                $('#A360CompCreatedDate').val('');
                if (MstrComp == "CharacterBehaviour") {
                    $('#CharacterBehaviour').click();
                } else if (MstrComp == "HomeWorkRelated") {
                    $('#HomeWorkRelated').click();
                } else if (MstrComp == "TestsAssessments") {
                    $('#TestsAssessments').click();
                }
            }
        },
        error: function (xhr, status, error) { ErrMsg($.parseJSON(xhr.responseText).Message); }
    });
};
/* saving of component detials */

function GetIssueType(drpdwnId, AssessCompGrp, SelVal) {
    // 
    var IssueCredits = $('#hdnIsCredit').val();
    var campus = $('#hdnCampus').val();
    $.ajax({
        type: 'GET',
        async: false,
        dataType: "json",
        url: '/Assess360/GetAssess360CompMasterListByName?tab=' + AssessCompGrp + '&IssueCredits=' + IssueCredits + '&Campus=' + campus,
        success: function (data) {
            $('#' + drpdwnId).empty();
            $('#' + drpdwnId).append("<option value='' IsCredit='' Marks=''> Select </option>");
            for (var i = 0; i < data.rows.length; i++) {
                if (SelVal) {
                    $('#' + drpdwnId).append("<option value='" + data.rows[i].Value + "' IsCredit='" + data.rows[i].IsCredit + "' Marks='" + data.rows[i].Marks + "' selected='selected'>" + data.rows[i].Text + "</option>");
                } else {
                    $('#' + drpdwnId).append("<option value='" + data.rows[i].Value + "' IsCredit='" + data.rows[i].IsCredit + "' Marks='" + data.rows[i].Marks + "'>" + data.rows[i].Text + "</option>");
                }
            }
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}

function GetSubjectsByGrade(drpdwnId) {
    $.ajax({
        type: 'GET',
        async: false,
        dataType: "json",
        url: '/Assess360/GetSubjectsByGrade?Campus=' + $('#hdnCampus').val() + '&Grade=' + $('#Grade').val(),
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
/*
This method added by Lee on 22-Jun-2013, 
Earlier this is a free text, 
now it has been turned to Master data.
*/
function GetAssignmentName(drpdwnId, SelVal) {
    //;
    $.ajax({
        type: 'GET',
        async: false,
        dataType: "json",
        url: '/Assess360/GetAssignmentName',
        success: function (data) {
            //;
            $('#' + drpdwnId).empty();
            $('#' + drpdwnId).append("<option value=''> Select </option>");
            for (var i = 0; i < data.rows.length; i++) {
                if (SelVal != null && data.rows[i].Value == SelVal) {
                    $('#' + drpdwnId).append("<option value='" + data.rows[i].Value + "' selected='selected'>" + data.rows[i].Text + "</option>");
                } else {
                    $('#' + drpdwnId).append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
                }
            }
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}
function GetAssignmentNameByCampusGradeSubject(drpdwnId, SelVal, cam, gra, sub) {

    $.ajax({
        type: 'GET',
        async: false,
        url: '/Assess360/GetAssignmentNameByCampusGradeSubject?cam=' + cam + '&gra=' + gra + '&sub=' + sub,
        success: function (data) {
            $('#' + drpdwnId).empty();
            $('#' + drpdwnId).append("<option value=''> Select </option>");
            for (var i = 0; i < data.rows.length; i++) {
                if (SelVal != null && data.rows[i].Value == SelVal) {
                    $('#' + drpdwnId).append("<option value='" + data.rows[i].Value + "' selected='selected'>" + data.rows[i].Text + "</option>");
                } else {
                    $('#' + drpdwnId).append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
                }
            }
        },
        dataType: "json"
    });
}

function GetMarksbyCompName(drpdwnId, strMarks, IsCredit) {
    // 
    var arrMarks = new Array();
    arrMarks = strMarks.split(',');
    $('#' + drpdwnId).empty();
    $('#' + drpdwnId).append("<option value=''> Select </option>");
    $('#hdnIsCredit').val(IsCredit);
    for (var i = 0; i < arrMarks.length; i++) {
        $('#' + drpdwnId).append("<option value='" + arrMarks[i] + "'>" + arrMarks[i] + "</option>");
    }
}

function GetStaffPopup() {
  
    var grade = ''; // $('#Grade').val();
    ModifiedLoadPopupDynamicaly('/Assess360/loadPartialView?PartialViewName=StaffMasterPopup',
            $('#DivStaffMasterSearch'),function () {
                $('#StaffGrade').val(grade);
                LoadSetGridParam($('#StaffMasterList'), "/Assess360/GetStaffMasterDetails?Grade=" + grade);
            },
            function (rdata) {
                
                $('#StaffId').val(rdata.Id);
                $('#Staff').val(rdata.UserId);
                $('#SBCStaffUserName').val(rdata.UserName);
            },
            400, 325, 'Staff Name')
}
function semhideshow(value) {
    if (value == "show") {
        $('#tdSemheading').show();
        $('#tdSemester').show();
    } else {
        $('#tdSemheading').hide();
        $('#tdSemester').hide();
    }
}

var grid_selector = "#StudentList";
var pager_selector = "#StudentPager";
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


function InitStudentGrid() {
    $(grid_selector).jqGrid({
        datatype: 'local',

        colNames: ['Id', 'Id No', 'Name', 'Section', 'Campus Name', 'Grade', 'Academic Year', 'Is Hosteller'],
        colModel: [
              { name: 'Id', index: 'Id', key: true, hidden: true },
              { name: 'IdNo', index: 'IdNo' },
              { name: 'Name', index: 'Name', width: 150 },
              { name: 'Section', index: 'Section' },
              { name: 'Campus', index: 'Campus' },
              { name: 'Grade', index: 'Grade' },
              { name: 'AcademicYear', index: 'AcademicYear', sortable: false },
              { name: 'IsHosteller', index: 'IsHosteller', formatter: showYesOrNo }
              ],
        pager: $(StudentPager),
        rowNum: 10,
        rowList: [10, 15, 20, 50],
        sortname: 'Name',
        sortorder: 'asc',
        viewrecords: true,
        height: 150,
        shrinkToFit: true,
        caption: 'Student List',
        width: $(".col-xs-12").parent().width(), //430,
        //autowidth:true,
        onSelectRow: function (rowid, status) {
            debugger;
            rowData = $(grid_selector).getRowData(rowid);
            $('#Name').val(rowData.Name);
            $('#IdNo').val(rowData.IdNo);
            $('#Grade').val(rowData.Grade);
            $('#Section').val(rowData.Section);
            $('#StudentId').val(rowData.Id);
            $('#AcademicYear').val(rowData.AcademicYear);
            if (clbPupGrdSel != undefined && clbPupGrdSel) { clbPupGrdSel(rowData); }
            clbPupGrdSel = null;
            $('#DivStudentSearch').dialog('close');
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
}


