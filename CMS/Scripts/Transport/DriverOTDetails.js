$(function () {
    var grid_selector = "#DriverOTDetailsList";
    var pager_selector = "#DriverOTDetailsListPager";

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
        url: '/Transport/DriverOTDetailsListJqGrid',
        datatype: 'json',
        height: 265,
        colNames: ['Id', 'Campus', 'Driver Name', 'Driver Id No', 'OT Date', 'OT Type', 'OT Allowance', 'Created Date', 'Created By'],
        colModel: [
        //if any column added need to check formateadorLink
             {name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'Campus', index: 'Campus', editable: true },
             { name: 'DriverName', index: 'DriverName', editable: true },
             { name: 'DriverIdNo', index: 'DriverIdNo', editable: true },
             { name: 'OTDate', index: 'OTDate', search: true },
             { name: 'OTType', index: 'OTType', editable: true },
             { name: 'Allowance', index: 'Allowance', editable: true },
             { name: 'CreatedDate', index: 'CreatedDate', editable: true, search: true },
             { name: 'CreatedBy', index: 'CreatedBy', editable: true },
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
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-user'></i>&nbsp;Driver OT Details List"

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
                del: true,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, { url: 'Transport/EditDriverOTDetails',
                beforeShowForm: function (form) {
                    $('#tr_CreatedDate', form).hide();
                    $('#tr_CreatedBy', form).hide();
                }
            }, {}, { width: 'auto', url: '/Transport/DeleteDriverOTDetails/' }, {})

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
    if ($("#SuccessMsg").val() != null && $("#SuccessMsg").val() == "Yes") {
        InfoMsg("OT Details successfully created");
    }
    else if ($("#SuccessMsg").val() != null && $("#SuccessMsg").val() == "No") {
        ErrMsg("OT Details already put for this OT Type on this OT Date for this Driver");
    }
    else { }
    //Nav Grid Icons Tool Tip
    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }
    $("#gs_CreatedDate, #gs_OTDate").datepicker({
        format: 'dd/mm/yyyy',
        autoClose: true
    });

    //   Autocomplete Search Example....
    $("#DriverName").autocomplete({
        source: function (request, response) {
            var Campus = $("#Campus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        select: function (event, ui) {
            debugger;
            FillDriverIdNo(ui.item.value);
        },
        minLength: 1,
        delay: 100
    });
    $("#gs_DriverName").autocomplete({
        source: function (request, response) {
            var Campus = $("#Campus").val();
            $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
    });
    $("#Campus").change(function () {
        $("#OTType").val('');
        $("#Allowance").val('');
        $("#DriverIdNo").val('');
        $.getJSON("/Transport/GetDriverByCampus?Campus=" + $(this).val(),
             function (fillig) {
                 var Dri = $("#DriverName");
                 Dri.empty();
                 Dri.append($('<option/>',
                {
                    value: "",
                    text: "Select One"

                }));
                 $.each(fillig, function (index, itemdata) {
                     Dri.append($('<option/>',
                         {
                             value: itemdata.Value,
                             text: itemdata.Text
                         }));
                 });

             });
    });
    $("#OTType").change(function () {
        var Campus = $("#Campus").val();
        if (Campus == "" || Campus == null || Campus == "undefined")
        {
            ErrMsg("Please Fill Campus");
            $("#OTType").val('');
            return false;
        }
        var OTType = $(this).val();
        if (OTType == "")
            $("#Allowance").val('');
        if (OTType == "Evening")
            $("#Allowance").val(100);
        if (OTType == "Night") {
            if (Campus == "ERNAKULAM") {
                $("#Allowance").val(200);
            }
            else {
                $("#Allowance").val(250);
            }
        }
        if (OTType == "Out Station")
            $("#Allowance").val(300);
        if (OTType == "Holiday") {
            if (Campus == "ERNAKULAM") {
                $("#Allowance").val(200);
            }
            else {
                $("#Allowance").val(250);
            }
        }
        if (OTType == "Remedial Trip")
            $("#Allowance").val(150);
    });

    $("#OTDate").blur(function () {
        $("#OTType").focus();
        $("#OTType").expand();
        //  $("#OTType").simulate('mousedown');
        // document.getElementById('OTType').size = 1;
    });
    $("#OTDate").datepicker({
        // showOn: "button",
        // buttonImage: "../../Images/date.gif",
        buttonImageOnly: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: 'dd/MM/yyyy',
        onClose: function (dateText, inst) {
            // var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            $(this).datepicker('setDate', new Date(year, 1));
        }
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
    });
});

function SaveDriverOTDetails() {
    var Campus = $("#Campus").val();
    var DriverName = $('#DriverName').val();
    var DriverIdNo = $('#DriverIdNo').val();
    var OTDate = $("#OTDate").val();
    var OTType = $("#OTType").val();
    var Allowance = $("#Allowance").val();
    if (Campus == '' || DriverName == '' || DriverIdNo == '' || OTDate == '' || OTType == '' || Allowance == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
//    $.ajax({
//        type: 'POST',
//        url: "/Transport/AddDriverOTDetails",
//        data: { Campus: Campus, DriverName: DriverName, DriverIdNo: DriverIdNo, OTDate: OTDate, OTType: OTType, Allowance: Allowance },
//        success: function (data) {
//            $("#DriverOTDetailsList").trigger('reloadGrid');
//            if (data == true)
//                InfoMsg("OT Details successfully created");
//            else
//                ErrMsg("OT Details already put for this OT Type on this OT Date for this Driver");
//            // $("input[type=text], textarea, select").val("");
//            $("#OTType").val('');
//            $("#Allowance").val('');
//        }
//    });
}


function FillDriverIdNo(DriverName) {
    debugger;
    var Campus = $("#Campus").val();
    $.ajax({
        type: 'POST',
        url: "/Transport/GetDriverDetailsByNameAndCampus",
        data: { Campus: Campus, DriverName: DriverName },
        success: function (data) {
            $("#DriverIdNo").val(data.DriverIdNo);
        }
    });
}