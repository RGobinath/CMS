jQuery(function () {
    var lastsel2 = '';
    var HoliDays = "";
//    $('#txtSchDate').val($("#RetainCurDate").val());
    var grid_selector = "#HolidayList";
    var pager_selector = "#HolidayListPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".col-sm-9").width());
    });
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
    $(".Attendancedatepicker").datepicker({
        weekStart: 1,
        format: "dd/mm/yy",
        maxDate: new Date(),
        changeMonth: true,
        timeFormat: 'hh:mm:ss',
        autowidth: true,
        autoclose: true,
        daysOfWeekDisabled: [0],
        beforeShowDay: function (date) {
            var day = date.getDate();
            var dateAsString = date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();
            var result = (($.inArray(dateAsString, HoliDays) == -1) && (day != 0)) ? true : false;
            return result;
        }
    });

    //enable datepicker
    function pickDate(cellvalue, options, cell) {
        setTimeout(function () {
            jQuery(cell).find('input[type=text]')
					.datepicker({ format: 'dd-mm-yyyy', autoclose: true }).attr('readonly', 'readonly');
        }, 0);
    }

    $(grid_selector).jqGrid({
        url: "/Attendance/AddandListHolidaysJqGrid",
        datatype: 'json',
        type: 'GET',
        colNames: ['', 'Date', 'Comments', 'Campus', 'Grade', 'Academic Year'],
        colModel: [
                                  { name: 'Id', index: 'Id', hidden: true, key: true },
                                  { name: 'Holiday', index: 'Holiday', editable: true, sorttype: "date", unformat: pickDate },
                                  { name: 'Comments', index: 'Comments', search: false, editable: true },
                                  { name: 'Campus', index: 'Campus', editable: true, edittype: 'select', editrules: { required: true }, editoptions: {
                                      dataUrl: '/Assess360/GetCampusddl',
                                      dataEvents: [{
                                          type: 'change',
                                          fn: function (e) {
                                              var Campus = $(e.target).val();
                                              if (Campus != '') {
                                                  $.getJSON("/Assess360/GetGradeByCampus",
                                       { Campus: Campus },
                                       function (recipes) {
                                           var selectHtml = "";
                                           selectHtml += '<option value=" ">--select--</option>';
                                           $.each(recipes, function (jdIndex, jdData) {
                                               selectHtml = selectHtml + "<option value='" + jdData.Value + "'>" + jdData.Text + "</option>";
                                           });
                                           if ($(e.target).is('.FormElement')) {
                                               var form = $(e.target).closest('form.FormGrid');
                                               $("select#Grade.FormElement", form[0]).html(selectHtml);
                                           }
                                       });
                                              }
                                          }
                                      }]
                                  },
                                      stype: 'select', sortable: true,
                                      searchoptions: {
                                          dataUrl: '/Assess360/GetCampusddl',
                                          dataEvents: [{
                                              type: 'change',
                                              fn: function (e) {
                                                  var Campus = $(e.target).val();
                                                  if (Campus != '') {
                                                      $.getJSON("/Assess360/GetGradeByCampus",
                                                                   { Campus: Campus },
                                                                   function (cmp) {
                                                                       var selectHtml1 = "";
                                                                       selectHtml1 += '<option value="">--select--</option>';
                                                                       $.each(cmp, function (jdIndex1, jdData1) {
                                                                           selectHtml1 = selectHtml1 + "<option value='" + jdData1.Value + "'>" + jdData1.Text + "</option>";
                                                                       });
                                                                       $("#gs_Grade").html(selectHtml1);
                                                                   });
                                                  }
                                              }
                                          }]
                                      }
                                  },
                             { name: 'Grade', index: 'Grade', editable: true, search: true, edittype: 'select',
                                 editoptions: { dataUrl: '/Assess360/GetGradeddl' },
                                 editrules: { required: true, custom: false }, sortable: true, stype: 'select',
                                 searchoptions: { dataUrl: '/Assess360/GetGradeddl' }
                             },
                            { name: 'AcademicYear', index: 'AcademicYear', sortable: true, align: 'center', editable: true, edittype: 'select', editoptions: {
                                dataUrl: '/Base/GetJsonAcademicYear',
                                buildSelect: function (data) {
                                    var response, s = '<select>', i;
                                    response = jQuery.parseJSON(data);
                                    if (response && response.length) {
                                        $.each(response, function (i) {
                                            s += '<option value="' + response[i].Text + '">' + response[i].Value + '</option>';
                                        });
                                    }
                                    return s + '</select>';
                                }

                            }
                            }
                                  ],
        onSelectRow: function (id) {
            if (id && id !== lastsel2) {
                $(grid_selector).restoreRow(lastsel2);
                $(grid_selector).editRow(id, true);
                lastsel2 = id;
            }
        },
        serializeRowData: function (data) {
            
            //data.Holiday = MMDDYYFormatFunc(data.Holiday);
            return data;
        },
        editurl: '/Attendance/SaveHodlidaysEditRecords',
        rowNum: 50,
        rowList: [5, 10, 20, 50, 100],
        pager: pager_selector,
        sortname: 'Name',
        viewrecords: true,
        multiselect: true,
        sortorder: "Asc",
        height: 157,
        autowidth: true,
        editRecords: true,
        caption: '<i class="ace-icon fa fa-child"></i>&nbsp;&nbsp;Holiday List',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }

    });

    //navButtons Add, edit, delete
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
            {}, {
                url: '/Attendance/Delete/', left: '10%', top: '10%', height: '50%', width: 400, beforeShowForm: function (params) {
                    selectedrows = $("#HolidayList").jqGrid("getGridParam", "selarrrow");
                    return { Id: selectedrows }
                }
            },
            {})

    $('#txtHolidays').click(function () {
        
        if ($('#txtFromDate').val() == "") { ErrMsg("Please fill the FromDate"); return false; }
        if ($('#txtToDate').val() == "") { ErrMsg("Please fill the ToDate"); return false; }
        if ($('#txtComments').val() == "") { ErrMsg("Please fill the Comments"); return false; }
        $.ajax({
            type: 'POST',
            async: false,
            url: '/Attendance/Holidays?&fromdate=' + $('#txtFromDate').val() + '&todate=' + $('#txtToDate').val() + '&Comments=' + $('#txtComments').val() + '&Campus=' + $('#ddlCampus').val() + '&Grade=' + $('#ddlGrade').val() + '&Academicyear=' + $('#ddlacademicyear').val(),
            success: function (date) {
                InfoMsg("Holidays added successfully.");
                $(grid_selector).trigger("reloadGrid");
            }
        });
        $('#txtFromDate').val("");
        $('#txtToDate').val("");
        $('#txtComments').val("");
        $('#ddlCampus').val("");
        $('#ddlacademicyear').val("");
        $('#ddlCampus').multiselect('rebuild');
        GetGrade()
    });

    $.getJSON("/Base/FillBranchCode",
    function (fillig) {
        var ddlcam = $("#ddlCampus");
        $.each(fillig, function (index, itemdata) {
            ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
        });
        $('#ddlCampus').multiselect('rebuild');
    });

    $.getJSON("/Base/FillBranchCode",
    function (fillig) {
        var ddlcam = $("#ddlSrchCampus");
        $.each(fillig, function (index, itemdata) {
            ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
        });
        $('#ddlSrchCampus').multiselect('rebuild');
    });

    $("#ddlSrchCampus").change(function () {
        GetSrchGrade();
    });

    $("#ddlCampus").change(function () {
        GetGrade();
    });


    $('#ddlCampus').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '300',
        numberDisplayed: 2,
        includeSelectAllDivider: true
    });
    $('#ddlSrchCampus').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '300',
        numberDisplayed: 2,
        includeSelectAllDivider: true
    });

    $('#ddlGrade').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '300',
        numberDisplayed: 2,
        includeSelectAllDivider: true
    });

    $('#ddlSrchGrade').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '300',
        numberDisplayed: 2,
        includeSelectAllDivider: true
    });

    $("#btnSearch").click(function () {
        jQuery(grid_selector).clearGridData();
        var Date = $("#txtSchDate").val() || "";
        var Campus = $("#ddlSrchCampus").val() || "";
        var Grade = $("#ddlSrchGrade").val() || "";
        var AcademicYear = $("#ddlSrchAcademicYear").val() || "";
        jQuery(grid_selector).setGridParam(
                    {
                        datatype: "json",
                        url: "/Attendance/AddandListHolidaysJqGrid",
                        postData: { Date: Date, Campus: Campus.toString(), Grade: Grade.toString(), AcademicYear: AcademicYear },
                        page: 1
                    }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        window.location.href = "/Attendance/Holidays";
    });
    //        $('#txtSchDate').val("");
    //        $('#ddlSrchCampus').multiselect('rebuild');
    //        //    $("#ddlSrchCampus").multiselect('destroy');
    //        //    $("#ddlSrchCampus").multiselect();
    //        //    $("#ddlSrchCampus").multiselect('None selected');
    //        //$('#ddlSrchCampus').dropdownchecklist('destroy');
    //        $('#ddlSrchGrade').val("");
    //        $('#ddlSrchAcademicYear').val("");
    //        $(grid_selector).setGridParam(
    //                        {
    //                            datatype: "json",
    //                            url: "/Attendance/AddandListHolidaysJqGrid",
    //                            postData: { Date: "", Campus: "", Grade: "", AcademicYear: "" },
    //                            page: 1
    //                        }).trigger("reloadGrid");
    //    });


    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    $("#ddlCampus").click(function () {
        var SDate = $("#txtToDate").val();
        if (SDate == "") {
            ErrMsg("Please select Date");
            return false;
        }
    });

    $("#ddlGrade").click(function () {
        var SDate = $("#ddlCampus").val();
        if (SDate == "") {
            ErrMsg("Please select Campus");
            return false;
        }
    });

    function updatePagerIcons(table) {
        var replacement = {
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
    function MMDDYYFormatFunc(rowObject) {
        var value = rowObject.split("/");
        var dd = value[0];
        var mm = value[1];
        var yy = value[2];
        var resultField = mm + "/" + dd + "/" + yy;
        return resultField;

    }
});

function GetSrchGrade() {
    $.ajax({
        type: 'POST',
        async: false,
        dataType: "json",
        url: '/Communication/FillGradesWithArrayCriteria?campus=' + $("#ddlSrchCampus").val(),
        success: function (data) {
            $("#ddlSrchGrade").empty();
            if (data != null && data != "") {
                for (var k = 0; k < data.length; k++) {
                    $("#ddlSrchGrade").append("<option value='" + data[k].Value + "'>" + data[k].Text + "</option>");
                }
            }
            $('#ddlSrchGrade').multiselect('rebuild');
        }
    });
}

function GetGrade() {
    $.ajax({
        type: 'POST',
        async: false,
        dataType: "json",
        url: '/Communication/FillGradesWithArrayCriteria?campus=' + $("#ddlCampus").val(),
        success: function (data) {
            $("#ddlGrade").empty();
            if (data != null && data != "") {
                for (var k = 0; k < data.length; k++) {
                    $("#ddlGrade").append("<option value='" + data[k].Value + "'>" + data[k].Text + "</option>");
                }
            }
            $('#ddlGrade').multiselect('rebuild');
        }
    });
}
  
        
    
    
    
    
