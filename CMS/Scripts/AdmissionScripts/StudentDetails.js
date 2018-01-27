jQuery(function ($) {

    //window.onload = $('.nav li').not('.active').addClass('disabled');
    //window.onload = $('.nav li').not('.active').find('a').removeAttr("data-toggle");

    //$("#dob").datepicker();
    $('#accordion').on('shown.bs.collapse', function (e) {
        if ($(e.target).is('#collapseTwo')) drawChartNow();
    });
    //enable datepicker
    function pickDate(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=text]')
                        .datepicker({
                            format: 'yyyy-mm-dd',
                            autoclose: true
                        });
        }, 0);
    }
    $("#CaptureImg").click(function () {
        $.ajax({
            type: 'POST',
            async: false,
            url: '/Base/IsPhotoUploadedFor?RefNum=' + $('#PreRegNum').val() + '&docType=Student Photo&docFor=Student',
            success: function (data) {
                debugger;
                if (data == "Success") {
                    ModifiedLoadPopupDynamicaly('/Admission/CaptureImage?PreRegNo=' + $("#PreRegNum").val()+'&IsPhotoUploaded=No', $('#ImgCaptureDiv'), function () { }, "", 800, 450, "Capture Image");
                }
                else {
                    if (confirm(data + " Photo was uploaded already! \n Are you sure you want to Change?")) {
                        ModifiedLoadPopupDynamicaly('/Admission/CaptureImage?PreRegNo=' + $("#PreRegNum").val() + '&IsPhotoUploaded=Yes', $('#ImgCaptureDiv'), function () { }, "", 800, 450, "Capture Image");
                    }
                    else { }
                }
            }
        });
    });

    $("#ddlcampus").change(function () {
        gradeddl();
        var campus = $("#ddlcampus").val();
        VanNumberddl(campus);
    });

    $("#dob").change(function () {
        var acYear = $("#academicyear").val();
        var grade = $("#stdntgrade").val();
        if (acYear != null && acYear != "") {
            if (grade != null && grade != "") {
                $.ajax({
                    url: '/Admission/ValidateAgeCutOff?AcYear=' + acYear + '&Grade=' + grade + '&Dob=' + $("#dob").val(),
                    mtype: 'GET',
                    async: false,
                    datatype: 'json',
                    success: function (data) {
                        if (data != "Success")
                            InfoMsg(data);
                    }
                });
            }
            else { ErrMsg("Please Provide Applicant's Grade!"); }
        }
        else {
            ErrMsg("Please Provide Applicant's Academic Year!");
        }
    });
    function isNumber(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        //alert(charCode);
        var txtMobileNum = $('#phno').val();
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        else if (txtMobileNum.length >= 10 && charCode != 8) {
            return false;
        }
        else { }
        return true;
    }

    function gradeddl() {
        var e = document.getElementById('ddlcampus');
        var campus = e.options[e.selectedIndex].value;
        $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
        function (modelData) {
            var select = $("#stdntgrade");
            select.empty();
            select.append($('<option/>', { value: '', text: "Select Grade" }));
            $.each(modelData, function (index, itemData) {
                select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
            });
        });
    }
});