$(function () {
    var grid_selector = "#AttendanceList";
    var pager_selector = "#AttendanceListPager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".col-xs-12").width());
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


    $('#tblBulk').hide();
    $('#chkBox').hide();
    $('#chkLabel').hide();
    $('#tblhalfday').hide();
    $('#Chkhalfday').hide();
    $('#ChkHalfAbsnt').hide();
    //alert($("#RetainDate").val());
    var HoliDays = "";
    $('#dvLoading')
          .hide()  // hide it initially.
          .ajaxStart(function () {
              $(this).show(); // show on any Ajax event.
          })
          .ajaxStop(function () {
              $(this).hide(); // hide it when it is done.
          });
   
    $("#HlfdayAbs").click(function () {

        if ($(this).is(":checked")) {
            $('#tblhalfday').show();
            //$('#txtDate').val('');
            $('#txtDate').attr("disabled", "disabled");

        } else {
            $('#tblhalfday').hide();
            $('#txtDate').val($("#RetainDate").val());
            $('#txtDate').removeAttr("disabled", "disabled");
        }
    });
    //Validation Date Picker
    var startDate = '';
    var FromEndDate = '';
    var ToEndDate = '';

    $(".txtFromDate").datepicker({
        format: "dd/mm/yyyy",
        weekStart: 1,
        startDate: startDate,
        endDate: FromEndDate,
        autoclose: true,
        daysOfWeekDisabled: [0]
    }).on('changeDate', function (selected) {
        startDate = new Date(selected.date.valueOf());
        startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
        $(".txtToDate").datepicker('setStartDate', startDate);
    });
    $(".txtToDate").datepicker({
        format: "dd/mm/yyyy",
        weekStart: 1,
        startDate: startDate,
        endDate: ToEndDate,
        autoclose: true,
        daysOfWeekDisabled: [0]
    }).on('changeDate', function (selected) {
        FromEndDate = new Date(selected.date.valueOf());
        FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
        $(".txtFromDate").datepicker('setEndDate', FromEndDate);
    });

    $("#txtToDate").click(function () {
        var SDate = $("#txtFromDate").val();
        if (SDate == "") {
            ErrMsg("Please select From Date");
            return false;
        }
    });

    //$(".Attendancedatepicker").datepicker({
    //    weekStart: 1,
    //    format: "dd/mm/yyyy",
    //    maxDate: new Date('@DateTime.Now.ToString()'),
    //    changeMonth: true,
    //    timeFormat: 'hh:mm:ss',
    //    autowidth: true,
    //    autoclose: true,
    //    daysOfWeekDisabled: [0],
    //    beforeShowDay: function (date) {
    //        debugger;
    //        var day = date.getDate();
    //        var dateAsString = date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();
    //        var result = (($.inArray(dateAsString, HoliDays) == -1) && (day != 0)) ? true : false;
    //        return result;
    //    }
    //});
    //    function isAvailable(date) {
    //        var day = date.getDay();
    //        var dateAsString = date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();
    //        var result = (($.inArray(dateAsString, HoliDays) == -1) && (day != 0)) ? [true] : [false];
    //        return result
    //    }

    jQuery(grid_selector).jqGrid({
        url: "/Attendance/GetAttendanceViewJqGrid",
        datatype: 'json',
        type: 'GET',
        colNames: ['', 'Student Id', 'Name', 'Campus', 'Grade', 'Section', 'Date', 'Present / Absent', '', 'Complete Status'],
        colModel: [
                                  { name: 'PreRegNum', index: 'PreRegNum', key: true, hidden: true },
                                  { name: 'NewId', index: 'NewId', sortable: false },
                                  { name: 'Name', index: 'Name' },
                                  { name: 'Campus', index: 'Campus', sortable: false },
                                  { name: 'Grade', index: 'Grade', sortable: false },
                                  { name: 'Section', index: 'Section', sortable: false },
                                  { name: 'StudentAbsent', index: 'StudentAbsent', editable: true, sortable: false },
                                  { name: 'IsAbsent', index: 'IsAbsent', sortable: false },
                                  { name: 'AttId', index: 'AttId', hidden: true, sortable: false },
                                  { name: 'CompleteStatus', index: 'CompleteStatus', sortable: false, hidden: true },

        ],
        rowNum: 300,
        rowList: [300, 500, 800],
        pager: pager_selector,
        sortname: 'Name',
        viewrecords: true,
        sortorder: "Asc",
        height: 220,
        autowidth: true,
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Attendance List',
        multiselect: true,
        forceFit: true,
        cellEdit: true,
        cellsubmit: 'clientArray',
        loadComplete: function () {
            var rowIds = $(grid_selector).jqGrid('getDataIDs');
            for (i = 0; i < rowIds.length; i++) {
                rowData = $(grid_selector).jqGrid('getRowData', rowIds[i]);
                if (rowData.AttId != "") {
                    // $("#" + rowIds[i]).find("td").css("background-color", "#FF0000");
                    $(grid_selector).jqGrid('setRowData', rowIds[i], false, { color: '#FF0000' });
                }

                if (rowData.CompleteStatus == "Completed") {
                    $(grid_selector).jqGrid('setColProp', 'StudentAbsent', { editable: false });
                    $("#btnAbsent_" + rowIds[i]).attr('disabled', 'disabled');
                    $("#" + rowIds[i]).find("td").css("background-color", "#DEE3E4");
                } else {
                    $(grid_selector).jqGrid('setColProp', 'StudentAbsent', { editable: true });
                    $("#btnAbsent_" + rowIds[i]).removeAttr('disabled');
                }

            }

            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);

            $(grid_selector).jqGrid('setGridWidth');
        },
        afterEditCell: function (id, name, val, iRow, iCol) {
            if (name == 'StudentAbsent') {
                jQuery("#" + iRow + "_StudentAbsent", grid_selector).datepicker({
                    dateFormat: 'dd/mm/yy', maxDate: '+0d', beforeShowDay: isAvailable,
                    onClose: function (data) {
                        var GridIdList = $(grid_selector).jqGrid('getRowData', id);
                        $.ajax({
                            type: 'POST',
                            async: false,
                            dataType: "json",
                            url: '/Attendance/FindAbsList?PreRegNum=' + id + '&date=' + data,
                            success: function (date) {
                                if (date != null) {
                                    $(grid_selector).jqGrid('setRowData', id, { 'AttId': "" });
                                    $(grid_selector).jqGrid('setRowData', id, { 'StudentAbsent': data });
                                    $(grid_selector).jqGrid('setRowData', id, { 'IsAbsent': "<button type=\"button\" class=\"btn btn-danger\" id=\"btnAbsent\" >Mark Absent</button>" });
                                }
                                else {
                                    ErrMsg('Already Absent on this day');
                                    $(grid_selector).trigger("reloadGrid");
                                }
                            }
                        });
                    }
                });
            }
        },
        beforeSelectRow: function (rowid, e) {
            var GridIdList = $(this).jqGrid('getRowData', rowid);
            var attid = GridIdList.AttId;
            var name = GridIdList.Name;
            var campus = GridIdList.Campus;
            var absentdate = GridIdList.StudentAbsent;
            var selecteddate = $('#txtDate').val();
            if ($(e.target).is('button[type="button"]')) {
                var dialog = "";
                if (attid == "") {
                    dialog = $('<div id="div_Confirm"></div>').text("Are you sure to Mark Absent for " + GridIdList.Name + "?");
                } else {
                    dialog = $('<div id="div_Confirm"></div>').text("Are you sure to Mark Present for " + GridIdList.Name + "?");
                }
                $('body').append(dialog);
                $('#div_Confirm').dialog({
                    autoOpen: true,
                    modal: true,
                    title: ' Comfirm Message',
                    buttons: {

                        "Yes":
                              {
                                  priority: 'primary', class: "btn btn-success", text: "Yes",
                                  click: function () {
                                      $.ajax({
                                          type: 'GET',
                                          async: false,
                                          dataType: "json",
                                          url: '/Attendance/SaveAttendance?PreRegNum=' + rowid + '&Attid=' + attid + '&AbsentDate=' + absentdate + '&SelectedDate=' + selecteddate + '&Name=' + name + '&campus=' + campus,
                                          success: function (data) {
                                              debugger;
                                              if (data != null && data == "sucess") {
                                                  InfoMsg("Message has been sent");
                                                  $(grid_selector).trigger("reloadGrid");
                                              }
                                          },
                                      });
                                      $('#div_Confirm').remove();
                                  }
                              },
                        "No":
                              {
                                  priority: 'secondary', class: "btn btn-danger", text: "No", click: function () {
                                      //write your logic here for no button
                                      $('#div_Confirm').remove();
                                  }
                              }
                    }
                }).prev().find(".ui-dialog-titlebar-close").hide();

            }
            return true; // allow row selection
        }
    });

    $('#txtDate').val($("#RetainDate").val());

    $('#btnGetStdntLst').click(function () {
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#Grade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
        if ($('#Section').val() == "Select") { ErrMsg("Please fill the Section"); return false; }
        if ($('#txtDate').val() == "") { ErrMsg("Please fill the Date"); return false; }

        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: "/Attendance/GetAttendanceViewJqGrid",
                    postData: { campus: $('#Campus').val(), grade: $('#Grade').val(), section: $('#Section').val(), date: $('#txtDate').val() },
                    page: 1
                }).trigger("reloadGrid");

        $('#chkBox').show();
        $('#chkLabel').show();
        $('#Chkhalfday').show();
        $('#ChkHalfAbsnt').show();
    });
    $('#btnBlkSave').click(function () {
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#Grade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
        if ($('#Section').val() == "Select") { ErrMsg("Please fill the Section"); return false; }
        if ($('#txtFromDate').val() == "") { ErrMsg("Please fill the Date"); return false; }
        if ($('#txtToDate').val() == "") { ErrMsg("Please fill the To Date"); return false; }
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];

        if (GridIdList.length > 0) {
            if (GridIdList.length > 1) {
                ErrMsg("Only 1 Student can be Mark Bulk Absent"); return false;
            }
        } else {
            ErrMsg("Please select the student"); return false;
        }

        rowData = $(grid_selector).jqGrid('getRowData', GridIdList[0]);
        $.ajax({
            type: 'GET',
            async: false,
            dataType: "json",
            url: '/Attendance/BulkSaveFunc?preRegNum=' + GridIdList[0] + '&name=' + rowData.Name + '&campus=' + $('#Campus').val() + '&grade=' + $('#Grade').val() + '&section=' + $('#Section').val() + '&fromdate=' + $('#txtFromDate').val() + '&todate=' + $('#txtToDate').val(),
            success: function (data) {
                if (data != null && data == "sucess") { InfoMsg("Message has been sent"); }
                $(grid_selector).trigger("reloadGrid");
            }
        });
    });
    $.getJSON("/Base/FillBranchCode",
    function (fillig) {
        var ddlcam = $("#Campus");
        ddlcam.empty();
        ddlcam.append($('<option/>',
    {
        value: "",
        text: "Select One"

    }));

        $.each(fillig, function (index, itemdata) {
            ddlcam.append($('<option/>',
    {
        value: itemdata.Value,
        text: itemdata.Text
    }));
        });
    });

    $("#Campus").change(function () {
        debugger;
        var campus = $('#Campus').val();
        gradeddl();
        $.ajax({
            type: 'GET',
            async: false,
            dataType: "json",
            url: '/Attendance/DisableDateinDatepicker?Campus=' + campus,
            success: function (data) {
                HoliDays = data;
            }
        });        
        if (HoliDays != null && HoliDays != "") {            
            for (i = 0; i < HoliDays.length; i++) {
                var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();                
                
                //if (dd < 10) {
                //    dd = '0' + dd
                //}
                //if (mm < 10) {
                //    mm = '0' + mm
                //}
                var today = dd + '-' + mm + '-' + yyyy;
                if (today == HoliDays[i]) {
                    $("#txtDate").val('');
                    return ErrMsg("Today is Holiday for " + campus + " Campus");
                }
                else {
                    var d = new Date().getDay();                    
                    if (d == 0) {
                        $("#txtDate").val('');
                        return ErrMsg("Sunday is Holiday for All Campus");
                    }
                     return $('#txtDate').val($("#RetainDate").val());
                }
            }
        }
        if(HoliDays==null || HoliDays == "") {
            var d = new Date().getDay();            
            if (d == 0)
            {
                $("#txtDate").val('');
                return ErrMsg("Sunday is Holiday for All Campus");
            }
            return $('#txtDate').val($("#RetainDate").val());
        }
    });


    $('input[type="checkbox"]').on('change', function () {
        $('input[type="checkbox"]').not(this).prop('checked', false);
    });
    $("#ChkHalfAbsnt").click(function () {
        if ($('#HlfdayAbs').is(':checked')) {
            $('#tblBulk').hide();
            $(this).parents('tr').prop("checked", true);
        }
    });

    $("#chkBox").click(function () {
        $('#tblhalfday').hide();
        if ($('#chbBulkAbs').is(':checked')) {
            $(this).parents('tr').prop("checked", true);
        }
    });
    //$('#txtDate').datepicker({
    //    format: "dd/mm/yyyy",
    //    beforeShowDay: function (Date) {
    //        debugger;
    //        var curr_date = Date.getDate();
    //        var curr_month = Date.getMonth() + 1;
    //        var curr_year = Date.getFullYear();
    //        var curr_date = curr_date + '/' + curr_month + '/' + curr_year;
    //        debugger;
    //        if (HoliDays.indexOf(curr_date) > -1) return false;
    //    }
    //})
    $("#txtDate").datepicker({
        weekStart: 1,
        startView: 1,
        format: 'dd-mm-yyyy',
        todayHighlight: true,
        changeMonth: true,
        autowidth: true,
        autoclose: true,
        minvalue: 0,
        endDate: new Date(),
        daysOfWeekDisabled: [0],
        beforeShowDay: function (date) {
            var day = date.getDate();
            var dateAsString = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
            var result = (($.inArray(dateAsString, HoliDays) == -1) && (day != 0)) ? true : false;
            return result;
        }
    }).attr('readonly','readonly'); 
    $("#chbBulkAbs").click(function () {
        if ($(this).is(":checked")) {
            $('#tblBulk').show();
            $('#txtDate').val('');
            $('#txtDate').attr("disabled", "disabled");
        } else {
            $('#tblBulk').hide();
            $('#txtDate').val($("#RetainDate").val());
            $('#txtDate').removeAttr("disabled", "disabled");
        }
    });

    $('#btnhalfday').click(function () {
        debugger;
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#Grade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
        if ($('#Section').val() == "Select") { ErrMsg("Please fill the Section"); return false; }
        // if ($('#AbsentDate').val() == "") { ErrMsg("Please fill the Date"); return false; }
        //if ($('#txtToDate').val() == "") { ErrMsg("Please fill the To Date"); return false; }
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        
        if (GridIdList.length > 0) {
            if (GridIdList.length > 1) {
                ErrMsg("Only 1 Student can be Mark Bulk Absent"); return false;
            }
        } else {
            ErrMsg("Please fill select the student"); return false;
        }
        if ($('.Btn:checked').length) {
            var chkId = '';
            $('.Btn:checked').each(function () {
                chkId += $(this).val() + ",";
            });
            chkId = chkId.slice(0, -1);
            alert(chkId);
        }
        rowData = $(grid_selector).jqGrid('getRowData', GridIdList[0]);
        $.ajax({
            type: 'GET',
            async: false,
            dataType: "json",
            url: '/Attendance/HalfDayAbsent?preRegNum=' + GridIdList[0] + '&name=' + rowData.Name + '&campus=' + $('#Campus').val() + '&grade=' + $('#Grade').val() + '&section=' + $('#Section').val() + '&absdate=' + $('#txtDate').val() + '&FN=' + chkId,
            success: function (data) {
                if (data != null && data == "sucess") { InfoMsg("Message has been sent"); }
                $(grid_selector).trigger("reloadGrid");
            }
        });
    });

    $('#btnCompleted').click(function () {

        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#Grade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
        if ($('#Section').val() == "Select") { ErrMsg("Please fill the Section"); return false; }
        if ($('#txtDate').val() == "") { ErrMsg("Please fill the Date"); return false; }
        var status = "Completed";
        $.ajax({
            type: 'GET',
            async: false,
            dataType: "json",
            url: '/Attendance/AttendanceCompleted?campus=' + $('#Campus').val() + '&grade=' + $('#Grade').val() + '&section=' + $('#Section').val() + '&date=' + $('#txtDate').val() + '&status=' + status,
            success: function (data) {
                $(grid_selector).trigger("reloadGrid");
            }
        });
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
});
function gradeddl() {

    var campus = $("#Campus").val();
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
        function (modelData) {
            var select = $("#Grade");
            select.empty();
            select.append($('<option/>',
        {
            value: "", text: "Select Grade"
        }));
            $.each(modelData, function (index, itemData) {

                select.append($('<option/>',
        {
            value: itemData.gradcod,
            text: itemData.gradcod
        }));
            });
        });
}

