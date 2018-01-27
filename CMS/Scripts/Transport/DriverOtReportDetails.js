$(function () {
    var grid_selector = "#DriverOtReport";
    var pager_selector = "#DriverOTPager";

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

    $(grid_selector).jqGrid({

        url: "/Transport/DriverOtReportDetailsJqgrid",
        datatype: 'json',
        type: 'GET',
        colNames: [ ' Name ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28', '29', '30', '31', 'OT Count', 'Total Allowance'],
        //colModel: [{ name: 'PreRegNum', index: 'PreRegNum', hidden: true, key: true },
              colModel:     [{ name: 'Name', index: 'Name', width: 200 },
                   { name: 'Date1', index: 'Date1', align: 'center', sortable: false ,width:30},
                   { name: 'Date2', index: 'Date2', align: 'center', sortable: false, width: 30 },
                   { name: 'Date3', index: 'Date3', align: 'center', sortable: false, width: 30 },
                   { name: 'Date4', index: 'Date4', align: 'center', sortable: false, width: 30 },
                   { name: 'Date5', index: 'Date5', align: 'center', sortable: false, width: 30 },
                   { name: 'Date6', index: 'Date6', align: 'center', sortable: false, width: 30 },
                   { name: 'Date7', index: 'Date7', align: 'center', sortable: false, width: 30 },
                   { name: 'Date8', index: 'Date8', align: 'center', sortable: false, width: 30 },
                   { name: 'Date9', index: 'Date9', align: 'center', sortable: false, width: 30 },
                   { name: 'Date10', index: 'Date10', align: 'center', sortable: false, width: 30 },
                   { name: 'Date11', index: 'Date11', align: 'center', sortable: false, width: 30 },
                   { name: 'Date12', index: 'Date12', align: 'center', sortable: false, width: 30 },
                   { name: 'Date13', index: 'Date13', align: 'center', sortable: false, width: 30 },
                   { name: 'Date14', index: 'Date14', align: 'center', sortable: false, width: 30 },
                   { name: 'Date15', index: 'Date15', align: 'center', sortable: false, width: 30 },
                   { name: 'Date16', index: 'Date16', align: 'center', sortable: false, width: 30 },
                   { name: 'Date17', index: 'Date17', align: 'center', sortable: false, width: 30 },
                   { name: 'Date18', index: 'Date18', align: 'center', sortable: false, width: 30 },
                   { name: 'Date19', index: 'Date19', align: 'center', sortable: false, width: 30 },
                   { name: 'Date20', index: 'Date20', align: 'center', sortable: false, width: 30 },
                   { name: 'Date21', index: 'Date21', align: 'center', sortable: false, width: 30 },
                   { name: 'Date22', index: 'Date22', align: 'center', sortable: false, width: 30 },
                   { name: 'Date23', index: 'Date23', align: 'center', sortable: false, width: 30 },
                   { name: 'Date24', index: 'Date24', align: 'center', sortable: false, width: 30 },
                   { name: 'Date25', index: 'Date25', align: 'center', sortable: false, width: 30 },
                   { name: 'Date26', index: 'Date26', align: 'center', sortable: false, width: 30 },
                   { name: 'Date27', index: 'Date27', align: 'center', sortable: false, width: 30 },
                   { name: 'Date28', index: 'Date28', align: 'center', sortable: false, width: 30 },
                   { name: 'Date29', index: 'Date29', align: 'center', sortable: false, width: 30 },
                   { name: 'Date30', index: 'Date30', align: 'center', sortable: false, width: 30 },
                   { name: 'Date31', index: 'Date31', align: 'center', sortable: false, width: 30 },
                   { name: 'OT count', index: 'noofAbsentdate', align: 'center', sortable: false,width:82 },
                   { name: 'Total Allowance', index: 'totalPercentage', align: 'center', sortable: false,width:100 }, ],
        rowNum: 50,
        rowList: [5, 10, 20, 50, 100],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: "Desc",
        height: 230,
        shrinkToFit: false,
        //autowidth: true,
        viewrecords: true,
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Report List',
        loadComplete: function () {

            var Searchtype = $('#ddlmonth').val();
            var year = (new Date).getFullYear();
            var month = Searchtype;
            var days = Math.round(((new Date(year, month)) - (new Date(year, month - 1))) / 86400000);
            if (days == '31') {
                jQuery(grid_selector).jqGrid('showCol', "Date31");
                jQuery(grid_selector).jqGrid('showCol', "Date29");
                jQuery(grid_selector).jqGrid('showCol', "Date30");
            }
            else if (days == '30') {
                jQuery(grid_selector).jqGrid('showCol', "Date29");
                jQuery(grid_selector).jqGrid('showCol', "Date30");
                jQuery(grid_selector).jqGrid('hideCol', "Date31");
            }
            else if (days == '28') {
                jQuery(grid_selector).jqGrid('hideCol', "Date29");
                jQuery(grid_selector).jqGrid('hideCol', "Date30");
                jQuery(grid_selector).jqGrid('hideCol', "Date31");
            } else if (days == '29') {
                jQuery(grid_selector).jqGrid('showCol', "Date29");
                jQuery(grid_selector).jqGrid('hideCol', "Date30");
                jQuery(grid_selector).jqGrid('hideCol', "Date31");
            }
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(grid_selector).jqGrid('setGridWidth');
        }

    });
    $("#btnSearch").click(function () {
        debugger;
        jQuery('#DriverOtReport').clearGridData();
        $(grid_selector).setGridParam(
                {
                    datatype: 'json',
                    url: "/Transport/DriverOtReportDetailsJqgrid",
                    postData: { Campus: $('#ddlCampus').val(), Name: $('#DriverName').val(), Month: $('#ddlmonth').val(), OTType: $('#OTType').val(),ExportType:""},
                    page: 1
                }).trigger('reloadGrid');
    });
  
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
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
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
        onClickButton: function () {
            debugger;
            var Campus = $("#ddlCampus").val();
            var Name = $("#DriverName").val();
            var Month = $("#ddlmonth").val();
            var OTType = $("#OTType").val();
            window.open("/Transport/DriverOtReportDetailsJqgrid" + '?ExportType=Excel'
                    + '&Campus=' + Campus
                    + '&Name=' + Name
                    + '&Month=' + Month
                    + '&OTType=' + OTType
                    + '&rows=9999');
        }
    });
    //Pager icons
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
        //$("#thumbnail").append(response + '&nbsp;')
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
$("#DriverName").autocomplete({
    source: function (request, response) {
        var Campus = $("#ddlCampus").val();
        $.getJSON('/Transport/GetAutoCompleteDriverNamesByCampus?term=' + request.term + '&Campus=' + Campus, function (data) {
            response(data);
        });
    },
    select: function (event, ui) {
        FillDriverIdNo(ui.item.value);
    },
    minLength: 1,
    delay: 100
});
var Month = ($('#CurMonth')).val();
$.getJSON("/Base/FillMonth",
     function (fillig) {
         debugger;
         var mior = $("#ddlmonth");
         mior.empty();
         mior.append("<option value=' '> Select </option>");
         $.each(fillig, function (index, itemdata) {
             if (itemdata.Value != Month) {
                 mior.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
             }
             else {
                 mior.append($('<option/>',
             {
                 value: itemdata.Value,
                 text: itemdata.Text,
                 selected: "Selected"

             }));
             }
         });
     });
function FillDriverIdNo(DriverName) {
    var Campus = $("#ddlCampus").val();
    $.ajax({
        type: 'POST',
        url: "/Transport/GetDriverDetailsByNameAndCampus",
        data: { Campus: Campus, DriverName: DriverName },
        success: function (data) {
            $("#DriverId").val(data.DriverIdNo);
        }
    });
}

