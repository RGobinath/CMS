jQuery(function ($) {
    // alert();
    $("#ddlTeacher").change(function () {
        var temp = $("#ddlTeacher").val();
        $.ajax({
            url: '/TimeTable/getTeacherTimeTableHtml?StaffId=' + temp,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                $('#techerTimeTable').html(data);
            }
        });
        $.ajax({
            url: '/TimeTable/getTeacherDetailsHtml?StaffId=' + temp,
            type: 'GET',
            dataType: 'json',
            success: function (StaffDetls) {
                debugger;
                $('#Gender').text(StaffDetls.Gender);
                $('#Teacher').text(StaffDetls.TeacherName);
                $('#NoOfPrdDetls').text(StaffDetls.max_periods);
                $('#RemnPrdDetls').text(StaffDetls.rem_periods);
            }
        });
    });

    $('#SubjectTecherTbl tr').on('click', function (id) {
        debugger;
        var $this = $(this);
        var Subject = $this.context.cells[0].innerHTML;
        var Teacher = $this.context.cells[1].innerHTML;
        var NoOfPrdDetls = $this.context.cells[2].innerHTML;
        var RemnPrdDetls = $this.context.cells[3].innerHTML;

        $('#Subject').text(Subject);
        $('#Teacher').text(Teacher);
        $('#NoOfPrdDetls').text(NoOfPrdDetls);
        $('#RemnPrdDetls').text(RemnPrdDetls);

        bid = (this.id); // button ID 
        //alert(bid);
        $.ajax({
            url: '/TimeTable/getTeacherTimeTableHtml?StaffId=' + bid,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                $('#techerTimeTable').html(data);
            }
        });
        //        $(this).addClass('selected').siblings().removeClass('selected');
        //        var value = $(this).find('td:first').html();
    });
    //$("#btnGetCalendar").click(function () {
    //    var SearchCampus = $('#ddlcampus').val();
    //    var SearchGrade = $('#ddlgrade').val();
    //    var SearchSection = $('#ddlsection').val();
    //    $.ajax({
    //        url: '/TimeTable/UpdateSearchTimeTable?SearchCampus=' + SearchCampus + '&SearchGrade=' + SearchGrade + '&SearchSection=' + SearchSection,
    //        type: 'GET',
    //        dataType: 'json',
    //        success: function (data) {
    //            window.location.href = "/TimeTable/TimeTable"
    //        }
    //    });
    //});
    $("#btnCancel").click(function () {
        $("#newSubject").val("");
        $("#StaffName").val("");
        $("#dialog-form").dialog("close");
    });

    $("#newSubject").autocomplete({
        source: function (request, response) {
            $.getJSON('/TimeTable/RetrieveSubjectLst?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 1,
        height: 50
    });

    $("#StaffName").autocomplete({
        source: function (request, response) {
            $.getJSON('/TimeTable/RetrieveStaffLst?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 1,
        height: 50
    });
    var dialog;
    dialog = $("#dialog-form").dialog({
        autoOpen: false,
        height: 200,
        width: 500,
        modal: false
    });

    //    $("#ddlgrade").change(function () {
    //        var SearchCampus = $('#ddlcampus').val();
    //        var SearchGrade = $('#ddlgrade').val();
    //        var SearchSection = $('#ddlsection').val();
    //        $.ajax({
    //            url: '/TimeTable/UpdateSearchTimeTable?SearchCampus=' + SearchCampus + '&SearchGrade=' + SearchGrade + '&SearchSection=' + SearchSection,
    //            type: 'GET',
    //            dataType: 'json',
    //            success: function (data) {
    //                if (data == true)
    //                    window.location.href = "/TimeTable/TimeTable"
    //                else
    //                    InfoMsg("Sorry ! The time table not alloted for selected section!!!");
    //            }
    //        });
    //    });
    // $("#ddlsection").change(function () {
    //        var SearchCampus = $('#ddlcampus').val();
    //        var SearchGrade = $('#ddlgrade').val();
    //        var SearchSection = $('#ddlsection').val();
    //        $.ajax({
    //            url: '/TimeTable/UpdateSearchTimeTable?SearchCampus=' + SearchCampus + '&SearchGrade=' + SearchGrade + '&SearchSection=' + SearchSection,
    //            type: 'GET',
    //            dataType: 'json',
    //            success: function (data) {
    //                if (data == true)
    //                    window.location.href = "/TimeTable/TimeTable"
    //                else
    //                    InfoMsg("Sorry ! The time table not alloted for selected section!!!");
    //            }
    //        });
    //    });


    //    $("#ddlsection").change(function () {
    //        var SearchCampus = $('#ddlcampus').val();
    //        var SearchGrade = $('#ddlgrade').val();
    //        var SearchSection = $('#ddlsection').val();
    //        $.ajax({
    //            url: '/TimeTable/getSubTeachrTymTbl?SearchCampus=' + SearchCampus + '&SearchGrade=' + SearchGrade + '&SearchSection=' + SearchSection,
    //            type: 'GET',
    //            dataType: 'json',
    //            success: function (htmlTable) {
    //                if (htmlTable != null)
    //                    $('#htmlTable').html(htmlTable);
    //            }
    //        });
    //    });

    function loadTeacher() {
        var teacher
        debugger; 
      //  $.getJSON("/TimeTable/getTeachersBydivision?Campus=" + $('#ddlcampus').val() + '&Grade=' + $('#ddlgrade').val() + '&Section=' + $('#ddlsection').val(),
        var Campus = $('#ddlcampus').val(); var Grade = $('#ddlgrade').val();
        var Section = $('#ddlsection').val();
        if (Campus == "" || Campus==null) {
            Campus = 'IB MAIN';
        }
        if (Grade == "" || Grade==null) {
            Grade = 'VI';
        }
        if (Section == "" || Section==null) {
            Section = 'A';
        }
        $.getJSON("/TimeTable/getTeachersBydivision?Campus=" + Campus + '&Grade=' + Grade + '&Section=' + Section,
        function (fillbc) {
                  teacher = $("#ddlTeacher");
                 teacher.empty();
                 teacher.append($('<option/>', { value: "", text: "-----Select-----" }));
                 $.each(fillbc, function (index, itemdata) {
                     teacher.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                 });
             });
        // teacher = $("#ddlTeacher");
      
    }

    //var sourceFullView = { url: '/TimeTable/GetTimeTableData/?division=' + devision };
    var eventsLoaded = false;
    debugger;
    var calendar = $('#calendar').fullCalendar({
        defaultView: 'agendaWeek',
        defaultDate: moment('2015-06-01'),
        editable: true,
        allDaySlot: false,
        selectable: true,
        minTime: '08:40:00',
        maxTime: '14:46:00',
        hiddenDays: [0, 6],


        select: function (start, end, allDay) {
            debugger;
            var endDate = formatDate(end, "YYYY-MM-DD HH:mm:ss");
            var stDate = formatDate(start, "YYYY-MM-DD HH:mm:ss");

            dialog.dialog("open");
            $("#newSubject").val("");
            $("#StaffName").val("");
            $("#btnAdd").click(function () {
                subject = $('#newSubject').val();
                staff = $('#StaffName').val();
                var Grade = $('#ddlgrade').val();
                var Campus = $('#ddlcampus').val();
                var Section = $('#ddlsection').val();
                if (Grade == "") {
                    CustomErrorMessage("Error", "Grade Should Not Empty");
                    return false;
                }
                if (Campus == "") {
                    CustomErrorMessage("Error", "Campus Should Not Empty");
                    return false;
                }
                if (Section == "") {
                    CustomErrorMessage("Error", "Section Should Not Empty");
                    return false;
                }
                if (subject == "") {
                    CustomErrorMessage("Error", "Subject Should Not Empty");
                    return false;
                }
                if (staff == "") {
                    CustomErrorMessage("Error", "Staff Name Should Not Empty");
                    return false;
                }
                $.ajax({
                    url: '/TimeTable/ValidStaff?staff=' + staff,
                    type: "POST",
                    success: function (result) {
                        if (result == false) {
                            CustomErrorMessage("Error Message", "Enter valid Staff Name");
                            // alert("Enter valid Staff Name");
                            $("#StaffName").val("");
                            return false;
                        }
                    }
                });
                $.ajax({
                    url: '/TimeTable/ValidSubject?subject=' + subject,
                    type: "POST",
                    success: function (result) {
                        if (result == true) {
                            $.ajax({
                                url: '/TimeTable/AddPeriod?Subject=' + subject + '&start=' + stDate + '&end=' + endDate + '&staff=' + staff + '&Campus=' + Campus + '&Grade=' + Grade + '&Section=' + Section,
                                //data: 'title=' + title + '&start=' + start1 + '&end=' + end1,
                                type: "POST",
                                success: function (Id) {
                                    debugger;
                                    if (Id == "nosub") {
                                        staff = "";
                                        $("#newSubject").val("");
                                        CustomErrorMessage("Error", "The subject is not allocated to this class!!!");
                                        //alert("The subject is not allocated to this class!!!");             
                                        return false;
                                    }
                                    else if (Id == "tehrnot") {
                                        CustomErrorMessage("Error", "The teacher is not allocated to this subject!!!");
                                        // alert("The teacher is not allocated to this subject!!!");
                                        return false;
                                    }
                                    else if (Id == "remaining") {
                                        CustomErrorMessage("Error", "check remaining period of this subject!!!");
                                        //  alert("check remaining period of this subject!!!");
                                        return false;
                                    }
                                    else if (Id == "already alloc") {
                                        $("#StaffName").val("");
                                        $("#newSubject").val("");
                                        CustomErrorMessage("Error Message", "The period is already allocated to this class!!!");
                                        //alert("The period is already allocated to this class!!!");
                                        return false;
                                    }
                                    else {
                                        $("#StaffName").val("");
                                        $("#newSubject").val("");

                                        dialog.dialog("close");
                                        CustomSuccessMessage("Success", "Subject Added Successfully");
                                        calendar.fullCalendar('renderEvent',
                                        {
                                            id: Id,
                                            title: subject,
                                            start: start,
                                            end: end,
                                            color: '#378006'
                                        },
                                            true // make the event "stick"
                                    );
                                    }
                                }
                            });
                        }
                        else {
                            $("#newSubject").val("");
                            CustomErrorMessage("Error", "Enter valid subject");
                        }

                    }
                });

            });
            //   calendar.fullCalendar('unselect');
        },
        eventRender: function (event, element) {
            element.bind('dblclick', function () {
                //debugger;
                //event.id.hide();
                var Subject = event.title;
                var id = event.id; //timetableid
                //  var allotmentId = event.id1;
                var start = event.start;
                var end = event.end;

                var r = confirm("Are You Remove this subject?");
                if (r == true) {
                    //x = "You pressed OK!";

                    $.ajax({
                        url: '/TimeTable/DropSubject?Subject=' + Subject + '&start=' + start + '&end=' + end + '&id=' + id, // + '&allotmentId=' + allotmentId,
                        //data: 'title=' + title + '&start=' + start1 + '&end=' + end1,
                        type: "POST",
                        success: function (result) {
                            $('#calendar').fullCalendar('removeEvents', event.id);
                            CustomSuccessMessage("Success", "Subject Removed Successfully");
                            return false;
                        }
                    });
                }

            });

        },
        eventDrop: function (calEvent, jsEvent, revertFunc) {
            //debugger;
            var Grade = $('#ddlgrade').val();
            var Campus = $('#ddlcampus').val();
            var Section = $('#ddlsection').val();
            if (Grade == "") {
                CustomErrorMessage("Error", "Grade Should Not Empty");
                return false;
            }
            if (Campus == "") {
                CustomErrorMessage("Error", "Campus Should Not Empty");
                return false;
            }
            if (Section == "") {
                CustomErrorMessage("Error", "Section Should Not Empty");
                return false;
            }

            var ttableid = calEvent.id;
            var Subject = calEvent.title;
            var start = calEvent.start; //alert(start);
            var end = calEvent.end; //alert(end);
            //var allotment_Id = calEvent.id1;
            //var teacher_Id = calEvent.id2;
            var endDate = formatDate(end, "YYYY-MM-DD HH:mm:ss");
            var stDate = formatDate(start, "YYYY-MM-DD HH:mm:ss");
            $.ajax({
                url: '/TimeTable/ChangePeriod?Subject=' + Subject + '&start=' + stDate + '&end=' + endDate + '&ttableid=' + ttableid + '&Campus=' + Campus + '&Grade=' + Grade + '&Section=' + Section,
                //data: 'title=' + title + '&start=' + start1 + '&end=' + end1,
                type: "POST",
                success: function (result) {
                    //debugger;
                    if (result == "") {
                        //  CustomMessage("Error Message", "Period Changed");
                        //return false;

                        CustomSuccessMessage("Success Message", "Period Changed");
                        $('#calendar').fullCalendar('rerenderEvents');
                    }
                    else {
                        CustomErrorMessage("Error", result);
                        revertFunc();
                        return false;
                    }
                }
            });

        },
        // events: '/TimeTable/GetTimeTableData'

        events: function () {
            $.ajax({
                type: "POST",
                url: '/TimeTable/GetTimeTableData?SearchCampus=' + 'IB MAIN' + '&SearchGrade=' + 'VI' + '&SearchSection=' + 'A',
                success: function (events) {
                    $('#calendar').fullCalendar('removeEvents');
                    $('#calendar').fullCalendar('addEventSource', events);
                    $('#calendar').fullCalendar('rerenderEvents');
                }
            });
            $.ajax({
                type: "POST",
                url: '/TimeTable/getSubTeachrTymTbl?SearchCampus=' + 'IB MAIN' + '&SearchGrade=' + 'VI' + '&SearchSection=' + 'A',
                success: function (subjectteacherHtml) {
                    $('#htmlTable').html(subjectteacherHtml);
                }
            }); loadTeacher();
            $("#ddlsection").change(function () {
                var Campus = $('#ddlcampus').val();
                if (Campus == null || Campus == "") {
                    ErrMsg("Please select anyone campus!!!");
                    return false;
                }
                var Grade = $('#ddlgrade').val();
                if (Grade == null || Grade == "") {
                    ErrMsg("Please select anyone Grade!!!");
                    return false;
                }
                var Section = $('#ddlsection').val();
                if (Section == null || Grade == "") {
                    ErrMsg("Please select anyone Section!!!");
                    return false;
                }
                loadTeacher();
                $.ajax({
                    type: "POST",
                    url: '/TimeTable/GetTimeTableData?SearchCampus=' + Campus + '&SearchGrade=' + Grade + '&SearchSection=' + Section,
                    success: function (events) {
                        $('#calendar').fullCalendar('removeEvents');
                        $('#calendar').fullCalendar('addEventSource', events);
                        $('#calendar').fullCalendar('rerenderEvents');
                    }
                });
                $.ajax({
                    type: "POST",
                    url: '/TimeTable/getSubTeachrTymTbl?SearchCampus=' + Campus + '&SearchGrade=' + Grade + '&SearchSection=' + Section,
                    success: function (subjectteacherHtml) {
                        $('#htmlTable').html(subjectteacherHtml);
                    }
                });
            });
        }
    });


    function formatDate(date, formatStr) {
        return formatDateWithChunks(date, getFormatStringChunks(formatStr));
    }
    function formatDateWithChunks(date, chunks) {
        var s = '';
        var i;

        for (i = 0; i < chunks.length; i++) {
            s += formatDateWithChunk(date, chunks[i]);
        }

        return s;
    }
    var formatStringChunkCache = {};


    function getFormatStringChunks(formatStr) {
        if (formatStr in formatStringChunkCache) {
            return formatStringChunkCache[formatStr];
        }
        return (formatStringChunkCache[formatStr] = chunkFormatString(formatStr));
    }
    function formatDateWithChunk(date, chunk) {
        var token;
        var maybeStr;

        if (typeof chunk === 'string') { // a literal string
            return chunk;
        }
        else if ((token = chunk.token)) { // a token, like "YYYY"
            if (tokenOverrides[token]) {
                return tokenOverrides[token](date); // use our custom token
            }
            return momentFormat(date, token);
        }
        else if (chunk.maybe) { // a grouping of other chunks that must be non-zero
            maybeStr = formatDateWithChunks(date, chunk.maybe);
            if (maybeStr.match(/[1-9]/)) {
                return maybeStr;
            }
        }

        return '';
    }
    var tokenOverrides = {
        t: function (date) { // "a" or "p"
            return momentFormat(date, 'a').charAt(0);
        },
        T: function (date) { // "A" or "P"
            return momentFormat(date, 'A').charAt(0);
        }
    };
    function momentFormat(mom, formatStr) {
        //alert();
        //debugger;
        return moment.fn.format.call(mom, formatStr);
    }
    // Break the formatting string into an array of chunks
    function chunkFormatString(formatStr) {
        var chunks = [];
        var chunker = /\[([^\]]*)\]|\(([^\)]*)\)|(LT|(\w)\4*o?)|([^\w\[\(]+)/g; // TODO: more descrimination
        var match;

        while ((match = chunker.exec(formatStr))) {
            if (match[1]) { // a literal string inside [ ... ]
                chunks.push(match[1]);
            }
            else if (match[2]) { // non-zero formatting inside ( ... )
                chunks.push({ maybe: chunkFormatString(match[2]) });
            }
            else if (match[3]) { // a formatting token
                chunks.push({ token: match[3] });
            }
            else if (match[5]) { // an unenclosed literal string
                chunks.push(match[5]);
            }
        }

        return chunks;
    }

    function CustomErrorMessage(title, text) {
        debugger;
        return $("<div class='dialog' title=" + title + "><div class='alert alert-danger'><strong><i class='ace-icon fa fa-times'></i></strong>Oh snap!&nbsp;" + text + "</div></div>")
        //return $("<div class='dialog' title='<div><i class='ace-icon fa fa-exclamation-triangle'></i> Error</div>'><div class='alert alert-danger'><strong><i class='ace-icon fa fa-times'></i></strong>Oh snap!&nbsp;" + text + "</div></div>")
        .dialog({
            resizable: false,
            height: 230,
            width: 250,
            modal: false,
            closeOnEscape: true,
            buttons:
                {
                    //"Confirm": function () {
                    //    $(this).dialog("close");
                    //},
                    "Close": function () {
                        $(this).remove(); //dialog("close");
                    }
                }
        });
    }

    function CustomSuccessMessage(title, text) {
        return $("<div class='dialog' title=" + title + "><div class='alert alert-block alert-success'><strong><i class='ace-icon fa fa-check'></i></strong>Well done!&nbsp;" + text + "</div></div>")
        .dialog({
            resizable: false,
            height: 230,
            width: 250,
            modal: false,
            closeOnEscape: true,
            buttons:
                {
                    //"Confirm": function () {
                    //    $(this).dialog("close");
                    //},
                    "Close": function () {
                        $(this).remove(); //dialog("close");
                    }
                }
        });
    }


    $("#ddlcampus").change(function () {
        gradeddl();
    });
})
function gradeddl() {
    var e = document.getElementById('ddlcampus');
    var campus = e.options[e.selectedIndex].value;
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
                function (modelData) {
                    var select = $("#ddlgrade");
                    select.empty();
                    select.append($('<option/>', { value: "", text: "---Select---" }));
                    $.each(modelData, function (index, itemData) {
                        select.append($('<option/>', { value: itemData.gradcod, text: itemData.Grade }));
                    });
                });
}
function OpenOldRouteStudList(bid) {
    //    debugger;
    //    var $this = $(this);
    //    var Subject = $this.context.cells[0].innerHTML;
    //    var Teacher = $this.context.cells[1].innerHTML;
    //    var NoOfPrdDetls = $this.context.cells[2].innerHTML;
    //    var RemnPrdDetls = $this.context.cells[3].innerHTML;

    //    $('#Subject').text(Subject);
    //    $('#Teacher').text(Teacher);
    //    $('#NoOfPrdDetls').text(NoOfPrdDetls);
    //    $('#RemnPrdDetls').text(RemnPrdDetls);

    $.ajax({
        url: '/TimeTable/getTeacherTimeTableHtml?StaffId=' + bid,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#techerTimeTable').html(data);
        }
    });

    $.ajax({
        url: '/TimeTable/getTeacherDetailsHtml?StaffId=' + bid,
        type: 'GET',
        dataType: 'json',
        success: function (StaffDetls) {
            
            $('#Gender').text(StaffDetls.Gender);
            $('#Teacher').text(StaffDetls.TeacherName);
            $('#NoOfPrdDetls').text(StaffDetls.max_periods);
            $('#RemnPrdDetls').text(StaffDetls.rem_periods);
        }
    });
    //    $(this).addClass('selected').siblings().removeClass('selected');
    //    var value = $(this).find('td:first').html();
}