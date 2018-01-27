jQuery(function ($) {
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#jqGridFinesAndPenalities").jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $("#jqGridFinesAndPenalities").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#jqGridFinesAndPenalities").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    var VehicleId = $("#hdnVehicleId").val();
    $("#jqGridFinesAndPenalities").jqGrid({
        url: '../../Transport/FinesAndPenalitiesjqgrid?VehicleId=' + VehicleId,
        datatype: 'json',
        type: 'GET',
        colNames: ['Penality Id', 'Vehicle Id', 'Vehicle No', 'Penality Date', 'Penality Area', 'Penality Reason', 'Penality Rupees', 'Penality Due Date', 'Penality Paid By', 'Created Date', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true,width:190 },
            { name: 'VehicleId', index: 'VehicleId', hidden: true, width: 290 },
            { name: 'VehicleNo', index: 'VehicleNo' },
            { name: 'PenalityDate', index: 'PenalityDate' },
            { name: 'PenalityArea', index: 'PenalityArea' },
            { name: 'PenalityReason', index: 'PenalityReason' },
            { name: 'PenalityRupees', index: 'PenalityRupees' },
            { name: 'PenalityDueDate', index: 'PenalityDueDate' },
            { name: 'PenalityPaidBy', index: 'PenalityPaidBy' },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true, width: 230 },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true, width: 230 }
            ],
        pager: '#jqGridFinesAndPenalitiesPager',
        rowNum: '5',
        rowList: [5, 10, 15, 20],
        sortname: 'Id',
        sortorder: 'Desc',
       // width: 1225,
       // shrinktofit: true,
        //autowidth: true,
        height: 130,
        viewrecords: true,
        multiselect: true,
        altRows: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: 'Fines And Penalities'
    });
    $("#jqGridFinesAndPenalities").jqGrid('filterToolbar', { searchOnEnter: true });
    //navButtons
    jQuery("#jqGridFinesAndPenalities").jqGrid('navGrid', '#jqGridFinesAndPenalitiesPager',
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
            }, { url: '/Transport/EditeFines' }, {},
            { url: '/Transport/DeleteFinesAndPenalitiesById' }, {})
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
        $("#jqGridFinesAndPenalities").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    $("#jqGridFinesAndPenalities").jqGrid('navButtonAdd', '#jqGridFinesAndPenalitiesPager', {
        caption: '<i class="fa fa-file-excel-o"></i>&nbsp Export Excel',
        onClickButton: function () {
            
            //var s=document.getElementById("gs_VehicleNo").value
            //alert(s);
            var VehiNo = $("#gs_VehicleNo").val();
            var PenalityDate = $("#gs_PenalityDate").val();
            var PenalityArea = $("#gs_PenalityArea").val();
            var PenalityReason = $("#gs_PenalityReason").val();
            var PenalityRupees = $("#gs_PenalityRupees").val();
            var PenalityDueDate = $("#gs_PenalityDueDate").val();
            var PenalityPaidBy = $("#gs_PenalityPaidBy").val();
            var CreatedDate = $("#gs_CreatedDate").val();
            var CreatedBy = $("#gs_CreatedBy").val();
            window.open("/Transport/FinesAndPenalitiesjqgrid?ExportType=Excel"
                    + "&VehicleId=" + VehicleId
                    + "&VehicleNo=" + VehiNo
                    + '&PenalityDate=' + PenalityDate
                    + '&PenalityArea=' + PenalityArea
                    + '&PenalityReason=' + PenalityReason
                    + '&PenalityRupees=' + PenalityRupees
                    + '&PenalityDueDate=' + PenalityDueDate
                    + '&PenalityPaidBy=' + PenalityPaidBy
                    + '&CreatedDate=' + CreatedDate
                    + '&CreatedBy=' + CreatedBy
                    + '&rows=9999');
        }
    });
    $("#PenalityDatePickerFine").datepicker({
        dateFormat: "dd/mm/yy"
    });
    $("#PenalityDueDatePickerFine").datepicker({
        dateFormat: "dd/mm/yy"
    });

    $("#PDriverName").autocomplete({
        source: function (request, response) {
            var Campus = $("#Campus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                // alert(data);
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });

});

function FineCreate() {
    var VehicleId = $("#hdnVehicleId").val();
    var VehicleNo = $('#VehicleNo').val();
    var PenalityDatePicker = $('#PenalityDatePickerFine').val();
    var PenalityArea = $('#PenalityAreaFine').val();
    var PenalityReason = $('#PenalityReasonFine').val();
    var PenalityRupees = $('#PenalityRupeesFine').val();
    var PenalityDueDate = $('#PenalityDueDate').val();
    var PenalityPaidBy = $('#PenalityPaidByFine').val();
    var DriverName = $("#PDriverName").val();

    if (VehicleId == '' || VehicleNo == '' || PenalityDatePicker == '' || PenalityArea == '' || PenalityReason == '' || PenalityRupees == ''
         || PenalityDueDate == '' || PenalityPaidBy == '' || DriverName == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    if (!parseInt(PenalityRupees)) {
        ErrMsg('Numbers only allowed.');
        $('#PenalityRupeesFine').focus();
        return false;
    }

    $.ajax({
        type: 'POST',
        url: "/Transport/AddFinesAndPenalities",
        data: { VehicleId: VehicleId, VehicleNo: VehicleNo, PenalityDate: PenalityDatePicker, PenalityArea: PenalityArea, PenalityReason: PenalityReason, PenalityRupees: PenalityRupees, PenalityDueDate: PenalityDueDate, PenalityPaidBy: PenalityPaidBy, DriverName: DriverName },
        success: function (data) {
            $("#jqGridFinesAndPenalities").trigger('reloadGrid');
            $("input[type=text], textarea, select").val("");
        }
    });

    $('#PenalityDatePicker').val("");
    $('#PenalityArea').val("");
    $('#PenalityReason').val("");
    $('#PenalityRupees').val("");
    $('#PenalityDueDatePicker').val("");
    $('#PenalityPaidBy').val("");
}