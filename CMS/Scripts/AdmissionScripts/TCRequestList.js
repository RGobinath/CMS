$(function () {
    var grid_selector = "#TCRequestList";
    var pager_selector = "#TCRequestListPager";
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
    $("#Campus").change(function () {
        gradeddl();
    });
    $('#fromdate').datepicker({
        format: 'dd/mm/yyyy',
        maxDate: 0,
        numberOfMonths: 1,
        autoclose: true,
        endDate: new Date(),
    });
    $('#fromdate').datepicker();
    $('#todate').datepicker({
        format: 'dd/mm/yyyy',
        maxDate: 0,
        numberOfMonths: 1,
        autoclose: true,
        endDate: new Date(),
    });
    $('#todate').datepicker();
    $(grid_selector).jqGrid({
        mtype: 'GET',
        url: '/Admission/TCRequestListJqGrid',
        datatype: 'json',
        height: '250',
        autowidth: true,
        shrinkToFit: true,
        colNames: ['Id', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Id No', 'Academic Year', 'Status', 'Requested Date', 'Generated Date', 'Last Attendance'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
                     { name: 'PreRegNum', index: 'PreRegNum', hidden: true },
                     { name: 'Name', index: 'Name', width: '250' },
                     { name: 'Campus', index: 'Campus' },
                     { name: 'Grade', index: 'Grade' },
                     { name: 'NewId', index: 'NewId' },
                     { name: 'AcademicYear', index: 'AcademicYear' },
                     { name: 'Status', index: 'Status' },
                     //{ name: 'CreatedBy', index: 'CreatedBy', width: '150' },
                     //{ name: 'CreatedBy', index: 'CreatedBy', editable: true, hidden: true },
                     { name: 'CreatedDate', index: 'CreatedDate' },
                      { name: 'ModifiedDate', index: 'ModifiedDate' },
                      { name: 'LastDateOfAttendance', index: 'LastDateOfAttendance', }
        ],
        pager: pager_selector,
        rowNum: '400',
        rowList: [20, 50, 100, 300, 400],
        sortname: 'Id',
        sortorder: 'asc',
        multiselect: true,
        viewrecords: true,
        caption: '<i class="fa fa-list">&nbsp;</i>TCRequest Inbox',
        loadComplete: function () {

            var table = this;
            setTimeout(function () {

                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });
    $(grid_selector).navGrid(pager_selector, { add: false, edit: false, del: false, search: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green' });
    //$(grid_selector).jqGrid('navButtonAdd', pager_selector, {
    //    caption: '&nbsp;&nbsp;<i class="fa fa-file-excel-o fa-lg blue">&nbsp;</i><u>Export To Excel<u/>',
    //    onClickButton: function () {
    //        //  alert('in');
    //        window.open("ExportToExcel" + '?rows=9999' + '&RequestNo=' + $('#RequestNo').val() +
    //   '&Name=' + $('#Name').val() +
    //   '&Campus=' + $('#Campus option:selected').val() +
    //   '&Section=' + $('#Section option:selected').val() +
    //   '&Grade=' + $('#Grade option:selected').val());
    //    }
    //});

    $('#btnSearch').click(function () {
        debugger;
        $(grid_selector).clearGridData();
        name = $('#Name').val();
        Campus = $('#Campus').val();
        grade = $('#ddlgrade').val();
        status = $('#ddlstatus').val();
        academicyear = $('#Acadyear').val();
        TcType = $('#tctype').val();
        fromdate = $('#fromdate').val();
        todate = $('#todate').val();
        Tc = $('#ddltc').val();
        LoadSetGridParam($(grid_selector), "/Admission/TCRequestListJqGrid?campus=" + Campus + '&status=' + status + '&fromdate=' + "" + '&todate=' + "" + '&AcademicYear=' + academicyear + '&TcType=' + TcType + '&Tc=' + Tc + '&grade=' + grade);

    });

    $('#btnReset').click(function () {
        $(grid_selector).clearGridData();
        $('#Name').val('');
        $('#Campus').val('');
        $('#ddlgrade').val('');
        $('#Acadyear').val('');
        $('#tctype').val('');
        $('#fromdate').val('');
        $('#todate').val('');
        $('#ddlstatus').val('');
        $('#ddltc').val('');
        LoadSetGridParam($(grid_selector), "/Admission/TCRequestListJqGrid?name=&campus=&grade=&status=" + $('#ddlstatus').val());
    });


    /*enter key press event*/
    function defaultFunc(e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('#btnSearch').click();
            return false;
        }
        return true;
    };

});
function gradeddl() {
    $.getJSON("/Admission/CampusGradeddl/", { Campus: $('#Campus').val() },
            function (modelData) {
                var select = $("#ddlgrade");
                select.empty();
                select.append($('<option/>', { value: "", text: "Select Grade" }));
                $.each(modelData, function (index, itemData) {
                    select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                });
            });
}

function getTcRequestdata(id1) {
    window.location.href = "/Admission/TCRequest?PreRegNo=&Id=" + id1;
}
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