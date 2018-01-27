$(function () {
    $('.datepicker').datepicker({
        daysOfWeekDisabled: [0],
        format: "dd/mm/yyyy",
        numberOfMonths: 1,
        autoclose: true,
        endDate: '+0d'
    });

    var grid_selector = "#EmpOTDetailsList";
    var pager_selector = "#EmpOTDetailsListPager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".col-sm-9").width());
    })

    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                var page_width = $(".page-content").width();
                page_width = page_width - 300;
                $(grid_selector).jqGrid('setGridWidth', page_width);
            }, 0);
        }
    })
    jQuery(grid_selector).jqGrid({
        url: '/Employee/EmployeeOTListJqGrid',
        datatype: 'json',
        height: 215,
        colNames: ['Id', 'Campus', 'Employee Name', 'Employee Id No', 'Absent Date', 'Absent Type', 'Created Date', 'Created By'],
        colModel: [
        //if any column added need to check formateadorLink
             { name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'Campus', index: 'Campus', editable: true },
             { name: 'EmployeeName', index: 'EmployeeName', editable: true },
             { name: 'EmployeeIdNo', index: 'EmployeeIdNo', editable: true },
             { name: 'AbsentDate', index: 'AbsentDate', search: true },
             { name: 'AbsentType', index: 'AbsentType', editable: true },
             { name: 'CreatedDate', index: 'CreatedDate', editable: true, search: true, hidden: false },
             { name: 'CreatedBy', index: 'CreatedBy', editable: true, hidden: false },
        ],
        viewrecords: true,
        rowNum: 8,
        //width:500,
        rowList: [7, 10, 30],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Desc',
        multiselect: true,
        loadComplete: function () {
            var ids = jQuery(grid_selector).jqGrid('getDataIDs');
            $("tr.jqgrow:odd").addClass('RowBackGroundColor');
            for (var i = 0; i < ids.length; i++) {
                rowData = jQuery(grid_selector).jqGrid('getRowData', ids[i]);
                if (rowData.AbsentType == "Leave") {
                    $(grid_selector).setCell(ids[i], "AbsentType", "", { "background-color": "#99FF99" });
                }
                else if (rowData.AbsentType == "Absent") {
                    $(grid_selector).setCell(ids[i], "AbsentType", "", { "background-color": "#FF9999" });
                }
                else { }

            }
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-user'></i>&nbsp;Employee OT"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });

    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, {
            }, {}, {}, {})

    //For pager Icons
    function styleCheckbox(table) {
    }
    function updateActionIcons(table) {
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
    debugger;

    if ($("#SuccessMsg").val() != null && $("#SuccessMsg").val() == "Yes") {
        InfoMsg("Attendance Details successfully created");
    }
    else if ($("#SuccessMsg").val() != null && $("#SuccessMsg").val() == "No") {

        ErrMsg("Attendance Details for particular employee already exits on this particular date " + Absent);
    }
    else { }
    //Nav Grid Icons Tool Tip
    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }
    $("#gs_CreatedDate, #gs_AbsentDate").datepicker({
        format: 'dd/mm/yyyy',
        autoClose: true
    });




    $("#gs_EmployeeName").autocomplete({

        source: function (request, response) {
            debugger;
            var Campus = $("#Campus").val();
            $.getJSON('/Employee/GetAutoCompleteEmployeeNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        select: function (event, ui) {
            FillEmployeeIdNo(ui.item.value);
        },
        minLength: 1,
        delay: 100
    });



    function FillEmployeeIdNo(EmployeeName) {
        debugger;
        var Campus = $("#Campus").val();
        // var EmployeeName = $("#gs_EmployeeName").val();
        $.ajax({
            type: 'POST',
            url: "/Employee/GetEmployeeDetailsByNameAndCampus",
            data: { Campus: Campus, EmployeeName: EmployeeName },
            success: function (data) {
                $("#EmployeeIdNo").val(data.IdNumber);

            }
        });
    }



    $("#AbsentDate").datepicker({

        // showOn: "button",
        // buttonImage: "../../Images/date.gif",
        buttonImageOnly: true,
        changeYear: true,
        showButtonPanel: true,
        // dateFormat: 'dd/MM/yyyy',
        onClose: function (dateText, inst) {
            // var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            $(this).datepicker('setDate', new Date(year, 1));
        }
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
    });


    //$("#btnSelect").click(function () {

    //    debugger;
    //    var Campus = $("#Campus").val();
    //    var DriverName = $('#DriverName').val();
    //    var DriverIdNo = $('#DriverIdNo').val();
    //    var AbsentDate = $("#AbsentDate").val();
    //    var AbsentType = $("#AbsentType").val();
    //    if (Campus == '' || DriverName == '' || DriverIdNo == '' || AbsentDate == '' || AbsentType == 'Select') {
    //        ErrMsg("Please fill all the mandatory fields.");
    //        return false;
    //    }

    //});
    $("#OTType").change(function () {
        var OTType = $(this).val();
        if (OTType == "")
            $("#Allowance").val('');
        if (OTType == "Remedial Trip")
            $("#Allowance").val(100);
        if (OTType == "Cleaning")
            $("#Allowance").val(200);
        if (OTType == "Field trip")
            $("#Allowance").val(100);
        if (OTType == "Field trip stay")
            $("#Allowance").val(200);
        if (OTType == "Function duty")
            $("#Allowance").val(200);
        if (OTType == "MD residence")
            $("#Allowance").val(100);
        if (OTType == "Public holidays")
            $("#Allowance").val(200);
        if (OTType == "Week-Off")
            $("#Allowance").val(200);
        if (OTType == "Evening transport")
            $("#Allowance").val(75);
    });


});

function EmployeeOT() {
    debugger;
    var Campus = $("#Campus").val();
    var EmployeeName = $('#gs_EmployeeName').val();
    var EmployeeIdNo = $('#EmployeeIdNo').val();
    var OTDate = $("#OTDate").val();
    var OTType = $("#OTType").val();
    var Allowance = $("#Allowance").val();
    if (Campus == '' || EmployeeName == '' || EmployeeIdNo == '' || OTDate == '' || OTType == 'Select') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }

    $.ajax({
        type: 'POST',
        url: "/Employee/AddEmployeeOT",
        data: {
            Campus: Campus, EmployeeName: EmployeeName, EmployeeIdNo: EmployeeIdNo, OTDate: OTDate,
            OTType: OTType, Allowance: Allowance
        },
        success: function (data) {
            $("#EmpOTDetailsList").trigger('reloadGrid');
            $("input[type=text], textarea").val("");
        }
    });

}