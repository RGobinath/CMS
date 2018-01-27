$(function () {
    $.fn.parseValToNumber = function () {
        return parseInt($(this).val().replace(':', ''), 10);
    }
    var grid_selector = "#EventMasterJqgrid";
    var pager_selector = "#EventMasterJqgridPager";
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                var page_width = $(".tab-content").width();
                $(grid_selector).jqGrid('setGridWidth', page_width);
            }, 0);
        }
    })

    jQuery(grid_selector).jqGrid({
        url: '/Asset/EventListDetailsListJqGrid?PageName=Request',
        datatype: 'json',
        height: 265,
        autowidth: true,
        colNames: ['Id', 'Request No', 'Campus', 'Hall', 'Booked on', 'Start Time', 'End Time', 'Event Description', 'Status', 'Created Date', 'Created By', 'CreatedBy', '', ''],
        colModel: [ //if any column added need to check formateadorLink
             { name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'RequestNo', index: 'RequestNo', search: false },
             { name: 'Campus', index: 'Campus' },
             { name: 'AssetName', index: 'AssetName' },
             { name: 'Date', index: 'Date', search: false },
             { name: 'StartTimeString', index: 'StartTimeString', search: false },
             { name: 'EndTimeString', index: 'EndTimeString', search: false },
             { name: 'ReasonForBooking', index: 'ReasonForBooking' },
             { name: 'Status', index: 'Status', stype: 'select', searchoptions: { sopt: ["eq", "ne"], value: "Initiated:Initiated;Approved:Approved;Rejected:Rejected" } },
             { name: 'CreatedDate', index: 'CreatedDate' },
             { name: 'CreatedName', index: 'CreatedName' },
             { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
             { name: 'Update', index: 'Update', width: 30, align: 'center', sortable: false, formatter: frmtrUpdate },
             { name: 'Delete', index: 'Delete', width: 30, align: "center", sortable: false, formatter: frmtrDel }
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
            var rdata = $(grid_selector).getRowData();
            var table = this;
            if (rdata.length > 0) {
                $('.EventUpdate').click(function () { UpdateEventDtls($(this).attr('rowid')); });
                $('.EventDelete').click(function () { DeleteEventDtls($(this).attr('rowid')); });
            }
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-user'></i>&nbsp;Event History"

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
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, {},
             {}, {}, {})

    //For pager Icons
    function styleCheckbox(table) {
    }
    function updateActionIcons(table) {
    }
   

    $("#txtDate").datepicker({
        minDate: today,
        format: 'dd/mm/yyyy',
        weekStart: 1,
        autowidth: true,
        autoclose: true,
        changeMonth: true
    });
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    var today = dd + '/' + mm + '/' + yyyy;
    $('#txtDate').datepicker('setStartDate', today)
    $('#timepicker1').timepicker({
        minuteStep: 1,
        showSeconds: true,
        showMeridian: false
    }).next().on(ace.click_event, function () {
        $(this).prev().focus();
    });
     
    $("#btnaddReset").click(function () {
        $("input[type=text], textarea, select").val("");
    });

    $(function () {
        $('#FrmTime, #ToTime').datetimepicker({
            //format: 'hh:mm',
            format: 'HH:mm',
            pickDate: false,
            pick12HourFormat: true
        });
    });
    function UpdateEventDtls(id) {
        var rowData = $(grid_selector).getRowData(id);
        $("#rqstNo").val(rowData.RequestNo);
        $("#btnAdd").text("Update");
        $('#Id').val(rowData.Id);
        $('#txtDate').val(rowData.Date);
        $('#FrmTime').val(rowData.StartTimeString);
        $('#ToTime').val(rowData.EndTimeString);
        $('#campusddl').val(rowData.Campus)
        getAssetdll(rowData.Campus, '#Assetddl', rowData.AssetName);
        $('#Assetddl').val(rowData.AssetName);
        $('#EventDescrip').val(rowData.ReasonForBooking);
    }
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

//Nav Grid Icons Tool Tip
function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}

function frmtrUpdate(cellvalue, options, rowdata) {
    var saveBtn = "";
    if (rowdata[11] == $('#loggedInUserId').val() && rowdata[8] != 'Approved') {
        saveBtn = "<span id='grdbtnUpdate_" + options.rowId + "'class='fa fa-pencil blue EventUpdate'  rowid='" + options.rowId + "' title='Update'></span>";
    }
    return saveBtn;
}

function frmtrDel(cellvalue, options, rowdata) {
    var delBtn = "";
    if (rowdata[11] == $('#loggedInUserId').val() && rowdata[8] != 'Approved') {
        delBtn = "<span id='grdbtnDel_" + options.rowId + "'class='fa fa-trash-o red EventDelete' rowid='" + options.rowId + "' title='Delete'></span>";
    }
    return delBtn;
}

function DeleteEventDtls(id) {
    if (confirm("Are you sure you want to delete this item?")) {
        // your deletion code
        $.ajax({
            url: '/Asset/DeleteEvent?EvntId=' + id,
            type: 'POST',
            dataType: 'json',
            traditional: true,
            success: function (data) {
                $("#EventMasterJqgrid").trigger('reloadGrid');
                //LoadSetGridParam($("#EventMasterJqgrid"), null);
                InfoMsg(data, function () { });
            },
            loadError: function (xhr, status, error) {
                msgError = $.parseJSON(xhr.responseText).Message;
                ErrMsg(msgError, function () { });
            }
        });

    }
    return false;
}

//Create new event

function NewEvent() {
    debugger;
    var Campus = $('#campusddl').val();
    var AssetName = $('#Assetddl').val();
    var Date = $('#txtDate').val();
    var id = $('#Id').val();
    //var txtDate = DateChangeDetails($("#txtDate"));
    //alert(Date);
    var FromTime = $('#FrmTime').val();
    var ToTime = $('#ToTime').val();
    var ReasonForBooking = $('#EventDescrip').val();
    if (Campus == '' || AssetName == '' || Date == '' || FromTime == '' || ToTime == '' || ReasonForBooking == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    var txtDate = $("#txtDate").datepicker('getDate');
    var endTime = txtDate.getTime() + $('#ToTime').parseValToNumber();
    var startTime = txtDate.getTime() + $('#FrmTime').parseValToNumber();
    if (endTime < startTime) {
        ErrMsg("End time must be after start time");
        return false;
    }

    //function DateChangeDetails(cellvalue) {
    //    //debugger;
    //    var Date = cellvalue.split(" ");
    //    var dd = Date[0];
    //    var time = Date[1];
    //    var value = Date[0].split("/");
    //    var dd = value[0];
    //    var mm = value[1];
    //    var yy = value[2];
    //    var resultField = mm + "/" + dd + "/" + yy + "";
    //    return resultField;

    //}

    $.ajax({
        type: 'POST',
        url: "/Asset/AddEventList",
        data: { Id: id, Campus: Campus, AssetName: AssetName, Date: Date, FromTime: FromTime, ToTime: ToTime, ReasonForBooking: ReasonForBooking },
        success: function (data) {
            if (data == "Success") {
                $("#EventMasterJqgrid").trigger('reloadGrid');
                $("input[type=text], textarea, select").val("");
                if (id != "") {
                    InfoMsg("Your Request has been Updated Successfully!...");
                    return true;
                }
                else {
                    InfoMsg("Your Request has been Initiated Successfully!...");
                    return true;
                }

            }
            else if (data == "Booked")
            { ErrMsg("Requested Hall Already Booked on this time Period!..."); }
            else { ErrMsg("Your Request was not Processed!..."); }
        }
    });
}

