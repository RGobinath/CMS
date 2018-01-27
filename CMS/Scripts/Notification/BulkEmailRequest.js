$(function () {
    $("#txtExpiryDate").attr("disabled", true);

    $("#txtExpiryDate").datepicker({
        format: "dd-mm-yyyy",
        startDate: new Date(),
        autoclose: true
    });
    $('#txtExpiryDate').keydown(function (e) {
        e.preventDefault();
        return false;
    });
    $('#Gradeddl').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '300',
        numberDisplayed: 4,
        includeSelectAllDivider: true
    });
    $('#Sectionddl').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '300',
        numberDisplayed: 3,
        includeSelectAllDivider: true
    });
    $("#StudentPortal").click(function () {
        if ($("#ParentPortal").is(":checked") == true) {
            $('#txtExpiryDate').attr("disabled", false);
        }
        else {
            $('#txtExpiryDate').attr("disabled", $(this).is(":checked") == true ? false : true);
            if ($(this).is(":checked") == false) {
                $('#txtExpiryDate').val('');
            }
        }
    });
    $("#ParentPortal").click(function () {
        if ($("#StudentPortal").is(":checked") == true) {
            $('#txtExpiryDate').attr("disabled", false);
        }
        else {
            $('#txtExpiryDate').attr("disabled", $(this).is(":checked") == true ? false : true);
            if ($(this).is(":checked") == false) {
                $('#txtExpiryDate').val('');
            }
        }
    });
    var url;
    // for Reason Dialog box
    $("#dialog").dialog({
        autoOpen: false,
        show: {
            effect: "blind",
            duration: 250
        },
        hide: {
            effect: "explode",
            duration: 1000
        }
    });
    // Fill campus
    $.getJSON("/Base/FillBranchCode",
    function (fillig) {
        var ddlcam = $("#ddlCampus");
        ddlcam.empty();
        ddlcam.append($('<option/>', { value: "", text: "Select One" }));
        $.each(fillig, function (index, itemdata) { ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text })); });
    });
    if ($('#IsSaveList').val() != "True") {
        bkLib.onDomLoaded(function () {
            nicEditors.allTextAreas({ maxHeight: 80, buttonList: ['bold', 'italic', 'underline', 'left', 'center', 'fontSize', 'fontFamily', 'fontFormat', 'bgcolor', 'forecolor', 'removeformat'] });
        });

    }
    $(window).resize(function () {
        $('#Message').width('100%');
    });
    $("#ddlCampus").change(function () {
        var campus = $('#ddlCampus').val();
        GetGrade();
        VanNumberddl(campus);
    });

    var grid_selector = "#BulkGrid";
    var pager_selector = "#BulkGridPager";

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

    //Pager icons


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

    $(grid_selector).jqGrid({
        url: '/Communication/JqgridBulkEmailRequest?ComposeId=' + $('#Id').val(),
        type: 'GET',
        datatype: 'json',
        colNames: ['Id', 'IdKeyValue', 'PreRegNum', 'NewId', 'Name', 'Campus', 'Grade', 'Section', 'Fee StructYear', 'Admission Status', 'Academic Year', 'Is Hosteller', 'Route No', 'Status', 'Family Type', 'EmailId', 'Applied Date', 'Recipients Created Date', 'Recipients Modified Date'],
        colModel: [
            { name: 'Id', index: 'Id', editable: true, hidden: true, editoptions: { disabled: true, class: 'NoBorder' } },
             { name: 'IdKeyValue', index: 'IdKeyValue', hidden: true, key: true, editable: true, editoptions: { disabled: true, class: 'NoBorder' } },
            { name: 'PreRegNum', index: 'PreRegNum', hidden: true, editable: true, editoptions: { disabled: true, class: 'NoBorder' } },
            { name: 'NewId', index: 'NewId', width: '125' },
            { name: 'Name', index: 'Name', width: '250', editable: true, editoptions: { disabled: true, class: 'NoBorder' } },
            { name: 'Campus', index: 'Campus', width: '170' },
            { name: 'Grade', index: 'Grade', width: '100' },
            { name: 'Section', index: 'Section', width: '80' },
            { name: 'FeeStructYear', index: 'FeeStructYear', hidden: true },
            { name: 'AdmissionStatus', index: 'AdmissionStatus', width: '130' },
            { name: 'AcademicYear', index: 'AcademicYear', width: '120' },
            { name: 'IsHosteller', index: 'IsHosteller', width: '90' },
            { name: 'VanNo', index: 'VanNo', width: '60' },
            { name: 'Status', index: 'Status', width: '95' }, //,formatter: statusview,  },
             { name: 'FamilyDetailType', index: 'FamilyDetailType', width: '70' },
            { name: 'U_EmailId', index: 'U_EmailId', editable: true, width: '245' },
            { name: 'CreatedDateNew', index: 'CreatedDateNew', hidden: false },
             { name: 'RecipientsCreatedDate', index: 'RecipientsCreatedDate', hidden: true },
             { name: 'RecipientsModifiedDate', index: 'RecipientsModifiedDate', hidden: true },

        ],
        autowidth: true,
        height: '220',
        rowNum: 50,
        rowList: [5, 50, 150, 200, 500, 700, 1000],
        viewrecords: true,
        multiselect: true,
        pager: pager_selector,
        shrinkToFit: true,
        sortname: 'Id',
        sortorder: 'Desc',
        caption: '<i class="fa fa-th-list"></i>&nbsp;Bulk Email Request',
        loadComplete: function () {
            var table = this;

            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });
    jQuery(grid_selector).jqGrid('navGrid', pager_selector, {
        del: true, add: false, edit: true, search: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green',
        view: false
    },
        {
            url: '/Communication/ReciptAddingFunction?ComposeId=' + $('#Id').val(), width: 450, beforeSubmit: function (postdata, formid) {
                if (postdata.U_EmailId != "") {
                    if (!ValidateEmail(postdata.U_EmailId)) {
                        return [false, 'Invalid email address.'];
                    }
                }
                return [true, '']; // no error
            }, closeAfterEdit: true
        }, {}, {
            url: '/Communication/ReciptDelete?ComposeId=' + $('#Id').val(), left: '10%', top: 700, height: '50%', width: 400, beforeShowForm: function (params) {
                selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow"); return { Id: selectedrows }
            }
        });

    if ($('#Id').val() == 0) {
        $('#Recipients').hide();
        $('#divGrid').hide();
        $('#sendBulkEmail').hide();
        $('#sendFalseMail').hide();
        $('#btnSuspend').hide();
    } else {

        //$('#email').attr('disabled', true); $('#password').attr('disabled', true);
        if ($('#IsSaveList').val() == "True") {
            GetMailBody();
            $("#ddlCampus").val($('#Campus').val())
            $('#composeInfo :input').each(function () {
                $(this).attr('disabled', true);
            });
            $('#Recipients :input').each(function () {
                $(this).attr('disabled', true);
            });
            $('#cancel').attr('disabled', false); $('#clearList').attr('disabled', false);
            $('#btnSuspend').attr('disabled', false); $('#sendBulkEmail').attr('disabled', false);
            $('#selectsendmail').show(); $('#sendFalseMail').hide();
            $('#edit_BulkGrid').show();
            $('#del_BulkGrid').show();
        }
        else {
            $('#btnSuspend').hide();
            $('#sendBulkEmail').hide();
            $('#selectsendmail').hide();
            $('#sendFalseMail').hide();
            $('#edit_BulkGrid').hide();
            $('#del_BulkGrid').hide();
        }
        if ($('#Attachment').val() == "True") {
            GetAttachment();
        }
        if ($('#Suspend').val() == "Suspend" || $('#Status').val() == "SuccessfullyCompleted") {
            $('#composeInfo :input').each(function () { $(this).attr('disabled', true); });
            $('#Recipients :input').each(function () { $(this).attr('disabled', true); });
            $('#btnSuspend').hide();
            $('#selectsendmail').attr('disabled', true);
            $('#sendFalseMail').attr('disabled', true);
            $('#cancel').attr('disabled', false);
            $('#edit_BulkGrid').hide();
            $('#del_BulkGrid').hide();
            $("#StudentPortal,#ParentPortal").attr("disabled", true);
        }
        if ($('#BulkEmailAdded').val() == "True") {
            $('#composeInfo :input').each(function () { $(this).attr('disabled', true); });
            $('#Recipients :input').each(function () { $(this).attr('disabled', true); });
            $('#cancel').attr('disabled', false); $('#sendFalseMail').show();
            $("#StudentPortal,#ParentPortal").attr("disabled", true);
        }
        if ($('#AlternativeEmailId').val() == "" || $('#AlternatPassword').val() == "") {
            $('#alterEmail').attr('disabled', true);
        } else { $('#alterEmail').attr('checked', true); $('#alterEmail').attr('disabled', false); }
        GetGrade();
    }
    $('#testMail').click(function () {


        var txtMailId = $('#txtTestMail').val();
        if ($('#Id').val() == 0) {
            ErrMsg("Please compose the content to send mail");
            return false;
        }
        if (txtMailId != "") {
            $('#dvLoading').hide().ajaxStart(function () {
                $(this).show(); // show on any Ajax event.
            });
            $.ajax({
                type: 'POST',
                async: false,
                url: "/Communication/SendTestMail?MailId=" + txtMailId + '&MailComposeId=' + $('#Id').val(),
                success: function (data) {
                    $('#dvLoading').hide().ajaxStop(function () {
                        $(this).hide(); // hide it when it is done.
                    });
                    if (data != null)
                        InfoMsg(data);
                    else
                        ErrMsg("Sending Mail Error! Please check composed content");
                }
            });
        }
        else {
            ErrMsg("Please Enter Mail Address to Test");
            return false;
        }
    });

    $('#btnSuspend').click(function () {
        $("#dialog").dialog({
            autoOpen: true,
            modal: true,
            width: 315,
            buttons: {
                "Yes": function () {
                    if ($('#reason').val() == "") { { ErrMsg("Please fill the reason"); return false; } }
                    $.ajax({
                        type: 'GET',
                        async: false,
                        dataType: "json",
                        url: '/Communication/SaveSuspend?Id=' + $('#Id').val() + '&Reason=' + $('#reason').val(),
                        success: function (data) {
                        }
                    });
                    url = $('#ReturnBackUrl').val();
                    window.location.href = url;
                }
            }

        });
    });

    $('#sendBulkEmail').click(function () {
        if (($("#ParentPortal").is(":checked") == true || $("#StudentPortal").is(":checked") == true) && $("#txtExpiryDate").val() == "") {
            ErrMsg("Please Fill Expiry Date");
            return false;
        }
        else {
            alert($('#txtExpiryDate').val());
            $.ajax({
                type: 'GET',
                async: false,
                dataType: "json",
                url: '/Communication/SendBulkEmailRequest?ComposeId=' + $('#Id').val() +
                    '&Campus=' + $('#ddlCampus').val() + '&IsAlterNativeMail=' + $('#alterEmail').is(':checked') +
                    '&StudentPortal=' + $('#StudentPortal').val() + '&ParentPortal=' + $('#ParentPortal').val() + '&ExpiryDate=' + $('#txtExpiryDate').val(),
                success: function (data) {
                    if (data != null) {
                        InfoMsg("Bulk send email has been initiated and in progress", function () {
                            url = $('#ReturnBackUrl').val();
                            window.location.href = url;
                        });
                    }
                }
            });
        }
    });

    $('#save').click(function () {
        var retValue = "";
        if ($('#email').val() != "") { if ($('#password').val() == "") { ErrMsg("Please fill the password"); return false; } }
        if ($('#password').val() != "") { if ($('#email').val() == "") { ErrMsg("Please fill the Email Address"); return false; } }
        if (($('#Father').is(':checked') == false) && ($('#Mother').is(':checked') == false) && ($('#General').is(':checked') == false)) {
            ErrMsg("Please fill the Sent To Option");
            return false;
        }
        if ($('#Subject').val() == "") { ErrMsg("Please fill the Subject"); return false; }
        if ($('.nicEdit-main').text() == "") { ErrMsg("Please fill the Message"); return false; }
        if ($('#email').val() != "" && $('#password').val() != "") {
            $.ajax({
                type: 'GET',
                async: false,
                dataType: "json",
                url: '/Communication/GoogleAuthEmailValidation?EmailId=' + $('#email').val() + '&Password=' + $('#password').val(),
                success: function (data) {
                    if (data == "Invalid") {
                        retValue = "False";
                    }
                }
            });
            if (retValue == "False") {
                ErrMsg("Invalid Alternate Email or Password");
                return false;
            }

        }
        $('#Recipients').show();
    });


    $('#saveList').live('click', function () {
        if ($('#ddlCampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        else if ($('#admstats').val() == "") { ErrMsg("Please Select the Admission Status"); return false; }
        else if ($('#academicyear').val() == "") { ErrMsg("Please Select the Academic Year"); return false; }
        else {
            var Grad = $("#Gradeddl").val();
            var Sec = $("#Sectionddl").val();
            if (Grad != "" & Grad != null) {
                Grad = $('#Gradeddl').val().toString();
            }
            if (Sec != "" & Sec != null) {
                Sec = $('#Sectionddl').val().toString();
            }
            var AppliedFrmDate = $('#txtAppliedFrmDate').val();
            var AppliedToDate = $('#txtAppliedToDate').val();
            var AcademicYear = $('#academicyear').val();
            $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Communication/JqgridBulkEmailRequest',
                    postData: { Campus: $('#ddlCampus').val(), Grade: Grad, Section: Sec, saveOrClear: "Save", ComposeId: $('#Id').val(), AppliedFrmDate: AppliedFrmDate, AppliedToDate: AppliedToDate, AcYear: AcademicYear },
                    page: 1
                }).trigger("reloadGrid");
            $('.nicEdit-panel').hide();
            $('#clearList').show(); $('#sendBulkEmail').show();
            $('#btnSuspend').show(); $('#selectsendmail').show();
            $('#saveList').attr('disabled', true); $('#reset').attr('disabled', true);
            $('#ddlCampus').attr('disabled', true); $('#Gradeddl').attr('disabled', true);
            $('#Sectionddl').attr('disabled', true); $('#search').attr('disabled', true);
            $('#edit_BulkGrid').show(); $('#del_BulkGrid').show();
            $('#Subject').attr('disabled', true); $('#Message').attr('disabled', true);
            $('#Father').attr('disabled', true); $('#Mother').attr('disabled', true);
            $('#General').attr('disabled', true); $('#save').attr('disabled', true);
        }
    });

    $('#clearList').click(function () {
        $('#Recipients :input').each(function () {
            $(this).attr('disabled', false);
        });
        if ($('#ddlCampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        var Grad = $("#Gradeddl").val();
        var Sec = $("#Sectionddl").val();
        if (Grad != "" & Grad != null) {
            Grad = $('#Gradeddl').val().toString();
        }
        if (Sec != "" & Sec != null) {
            Sec = $('#Sectionddl').val().toString();
        }
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Communication/JqgridBulkEmailRequest',
                    postData: { Campus: $('#ddlCampus').val(), Grade: Grad, Section: Sec, saveOrClear: "Clear", ComposeId: $('#Id').val() },
                    page: 1
                }).trigger("reloadGrid");
        $('#btnSuspend').hide(); $('#sendBulkEmail').show();
        $('#selectsendmail').show();
        $('#Subject').attr('disabled', false); $('#Message').attr('disabled', false);
        $('#Father').attr('disabled', false); $('#Mother').attr('disabled', false);
        $('#General').attr('disabled', false); $('#save').attr('disabled', false);
    });

    $('#selectsendmail').click(function () {
        //alert($('#email').is(':checked'));
        var ids = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        if (ids.length === 0)
        {
            ErrMsg("Please, select row"); return false;
        }
        else if ($("#StudentPortal").is(':checked') == true || $("#ParentPortal").is(':checked') == true)
        {
            ErrMsg("Please Uncheck Notification To Options");
            return false;
        }
        else {
            $('#dvLoading').hide().ajaxStart(function () {
                $(this).show(); // show on any Ajax event.
            });
            $.ajax({
                type: 'POST',
                async: false,
                url: "/Communication/SendSelectEmailFunction?Ids=" + ids + '&ComposeId=' + $('#Id').val() + '&IsAlterNativeMail=' + $('#alterEmail').is(':checked'),
                success: function (data) {
                    $('#dvLoading').hide().ajaxStop(function () {
                        $(this).hide(); // hide it when it is done.
                    });
                    //InfoMsg("bulk send email has been initiated and in progress");
                    InfoMsg("Selected send email has been initiated and in progress", function () {
                        url = $('#ReturnBackUrl').val();
                        window.location.href = url;
                    });
                }
            });
            $(grid_selector).trigger("reloadGrid");
        }
    });

    $('#sendFalseMail').click(function () {
        $('#dvLoading').hide().ajaxStart(function () {
            $(this).show(); // show on any Ajax event.
        });
        $.ajax({
            type: 'POST',
            async: false,
            url: "/Communication/SendFalseEmailFunction?ComposeId=" + $('#Id').val() + '&IsAlterNativeMail=' + $('#alterEmail').is(':checked'),
            success: function (data) {
                $('#dvLoading').hide().ajaxStop(function () {
                    $(this).hide(); // hide it when it is done.
                });
                InfoMsg("Send False email has been initiated and in progress", function () {
                    url = $('#ReturnBackUrl').val();
                    window.location.href = url;
                });
            }
        });
        $(grid_selector).trigger("reloadGrid");
    });

    $('#search').click(function () {
        if ($('#ddlCampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        else if ($('#admstats').val() == "") { ErrMsg("Please Select the Admission Status"); return false; }
        else if ($('#academicyear').val() == "") { ErrMsg("Please Select the Academic Year"); return false; }
        else {
            var Grad = $("#Gradeddl").val();
            var Sec = $("#Sectionddl").val();
            if (Grad != "" & Grad != null) {
                Grad = $('#Gradeddl').val().toString();
            }
            if (Sec != "" & Sec != null) {
                Sec = $('#Sectionddl').val().toString();
            }
            var AppliedFrmDate = $('#txtAppliedFrmDate').val();
            var AppliedToDate = $('#txtAppliedToDate').val();
            $(grid_selector).setGridParam({
                datatype: "json",
                url: '/Communication/JqgridBulkEmailRequest',
                postData: {
                    Campus: $('#ddlCampus').val(),
                    Grade: Grad, Section: Sec,
                    AdStatus: $('#admstats').val(),
                    FeeStYear: $('#feestructddl').val(),
                    StName: $('#appname').val(),
                    StId: $('#idnumber').val(),
                    StIshostel: $('#ishosteller').val(),
                    AcYear: $('#academicyear').val(),
                    VanNo: $('#vanno').val(),
                    saveOrClear: "",
                    ComposeId: $('#Id').val(),
                    AppliedFrmDate: AppliedFrmDate,
                    AppliedToDate: AppliedToDate
                },
                page: 1
            }).trigger("reloadGrid");
        }
    });

    $('#reset').click(function () {
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Communication/JqgridBulkEmailRequest',
                    postData: { Id: 0, Campus: "", Grade: "", Section: "", saveOrClear: "", ComposeId: 0 },
                    page: 1
                }).trigger("reloadGrid");
        $('#txtAppliedFrmDate').val('');
        $('#txtAppliedToDate').val('');
        $('#ddlCampus').val('');
        $('#admstats').val('');
        $('#feestructddl').val('');
        $('#appname').val('');
        $('#idnumber').val('');
        $('#ishosteller').val('');
        $('#academicyear').val('');
        //attr('selected', 'selected').prop('checked', true);
        $('#Gradeddl').attr('selected', false)//.prop('checked', true);
        //$('#Section').val('');
        $('#Sectionddl').multiselect('destroy');
        $('#Sectionddl option').each(function () {
            $(this).attr('selected', false);
        });
        Refreshsectionddl();
        $('#Gradeddl').dropdownchecklist('destroy');
        $('#Gradeddl option').each(function () {
            $(this).attr('selected', false);
        });
        Refreshgradeddl();
    });
    /*-------------------------------------------section--------------------------------------------------*/
    $("#Sectionddl").empty();
    $("#Sectionddl").append("<option value=' '> All </option>");
    $("#Sectionddl").multiselect('destroy');
    $("#Sectionddl").append("<option value='A'>A</option>");
    $("#Sectionddl").append("<option value='B'>B</option>");
    $("#Sectionddl").append("<option value='C'>C</option>");
    $("#Sectionddl").append("<option value='D'>D</option>");
    $("#Sectionddl").append("<option value='E'>E</option>");
    $("#Sectionddl").append("<option value='F'>F</option>");
    if ($('#Section').val() != "" & $('#Section').val() != null) {
        var sec = $('#Section').val();
        var secArr = sec.split(',');
        if (secArr != "") {
            for (var j = 0; j < secArr.length; j++) {
                $('#Sectionddl option').filter(function () { return $(this).text() == '' + secArr[j] + '' }).attr('selected', 'selected').prop('checked', true);
            }
        }
    }
    Refreshsectionddl();
    /*------------------------------------------------End section-----------------------------------------------*/

    $('#alterEmail').click(function () {
        if ($('#alterEmail').is(':checked') == true) {
            if ($('#email').val() == "") { $('#alterEmail').attr('checked', false); ErrMsg("Please fill the Email Address"); return false; }
            if ($('#password').val() == "") { $('#alterEmail').attr('checked', false); ErrMsg("Please fill the Password"); return false; }
        }
    });


    $('#cancel').click(function () {
        url = $('#ReturnBackUrl').val();
        window.location.href = url;
    });

    //---------------------------------------counter----------------------------/
    //    $('#Message').maxlength({
    //        alwaysShow: true,
    //        placement: 'bottom-right'
    //    });
    $('#Subject').maxlength({
        alwaysShow: true,
        placement: 'bottom-right'
    });
    //---------------------------------------counter----------------------------/
});

function uploaddoc2() {
    var splitstr = "";
    if (document.getElementById("file2").value == "") {
        ErrMsg("Please Browse a Document");
    }
    else {
        if ($('#Id').val() == 0) { ErrMsg("Please Save the Request"); }
        else {
            splitstr = splitstr + $('#file2').val().split('\\');
            $.ajaxFileUpload({
                url: '/Communication/BulkMailAttachments?PreRegNum=' + $('#Id').val() + '&AppName=Student',
                secureuri: false,
                fileElementId: 'file2',
                dataType: 'json',
                success: function (data) {
                    if (data.Message == "NotUploaded") { ErrMsg("Maximum 2 MB memory size files can be allowed to upload.Please try again later"); }
                    else {
                        $('#file2').val('');
                        GetAttachment();
                    }
                }
            });
            $('#clear2').html($('#clear2').html());

        }
    }
}

function Refreshgradeddl() {
    $("#Gradeddl").multiselect({
        firstItemChecksAll: true,
        maxDropHeight: 150,
        Height: 80,
        width: 155
    });
    $('#Gradeddl').multiselect('rebuild');
}
function Refreshsectionddl() {
    $("#Sectionddl").multiselect({
        firstItemChecksAll: true,
        maxDropHeight: 150,
        Height: 80,
        width: 150
    });
    $('#Sectionddl').multiselect('rebuild');
}
function GetAttachment() {
    //alert();
    $.ajax({
        url: '/Communication/ViewBulkMailAttachments?AttRefId=' + $('#Id').val() + '&AppName=Student',
        mtype: 'GET',
        async: false,
        datatype: 'json',
        success: function (data) {
            if (data != "None") {
                $('#ViewMailAtt').html(data);
            }
            else if (data == "None") {
                $('#ViewMailAtt').html('');
            }
            else { }
        }
    });
}

function delatt(delid) {
    var mailstatus = $('#Status').val();
    if (mailstatus != "SuccessfullyCompleted" && mailstatus != "PartiallyCompleted" && mailstatus != "CompletedWithErrors") {
        $.ajax({
            url: '/Communication/DeleteEmailAtt?AttId=' + delid,
            type: 'POST',
            dataType: 'json',
            traditional: true,
            success: function (data) {
                GetAttachment();
            }
        });
    }
    else { ErrMsg("Email sent already so you cannot able to delete"); return false; }
}


/*---------------------------------grade---------------------------*/
function GetGrade() {
    var campus = $("#ddlCampus").val();
    $.ajax({
        type: 'POST',
        async: false,
        dataType: "json",
        url: '/Communication/FillGrades?campus=' + campus,
        success: function (data) {
            $("#Gradeddl").empty();
            $("#Gradeddl").multiselect('destroy');
            if (data != null) {
                $("#Gradeddl").append("<option value=' '> All </option>");
                for (var k = 0; k < data.length; k++) {
                    $("#Gradeddl").append("<option value='" + data[k].Value + "'>" + data[k].Text + "</option>");
                    //$("#ddlGrades option:selected").prop("selected", false);
                }
                var grade = $('#Grade').val();
                var GradeArr = grade.split(',');
                if (GradeArr != undefined & GradeArr != null) {
                    for (var j = 0; j < GradeArr.length; j++) {
                        $('#Gradeddl option').filter(function () { return $(this).text() == '' + GradeArr[j] + '' }).attr('selected', 'selected').prop('checked', true);
                    }
                }
            }
            Refreshgradeddl();
        }
    });

}

/*------------------------------End Grade dll--------------------------*/
function resethtml2() {
    //    alert('hre');
    $('#clear2').html($('#clear2').html());
    var div = document.getElementById('Attachfiles2');
    div.innerHTML = 'Attached files,<br/> &nbsp;';
    //    $.ajax({
    //        url: '/Communication/BulkEmailDeleteAttachment?PreRegNum=' + $('#Id').val(),
    //        type: 'POST',
    //        dataType: 'json',
    //        traditional: true
    //    });
    //alert();
    $('#file2').val('');
    //$('#file2').replaceWith("<input type='file' class='btn btn-mini' title='Browse file to add' name='file2' id='file2' value='' multiple='multiple' style='height: 22px' />");
}
function GetMailBody() {
    $.ajax({
        url: '/Communication/ShowMessage?Id=' + $('#Id').val(),
        mtype: 'GET',
        async: false,
        datatype: 'json',
        success: function (data) {
            if (data != 0) {
                $('#showDiv').attr('disabled', true).css("background-color", "#EEEEEE");
                $('#ShowMessage').html(data);

            }
        }
    });
}
//--------------Van No------------------------/
function VanNumberddl(campus) {
    if (campus != "") {
        $.getJSON("/Base/GetVanRouteNoByCampus", { campus: campus },
    function (fillig) {
        var ddlcam = $("#vanno");
        ddlcam.empty();
        ddlcam.append($('<option/>', { value: "", text: "Select One" }));
        $.each(fillig, function (index, itemdata) { ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text })); });
    });
    }
    //$("#vanno").val('@Model.VanNo');
}

$('#file2').ace_file_input();
//-----------------End Van No--------------------/