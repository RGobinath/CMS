$(function () {

    var grid_selector = "#DriverAttendanceReportList";
    var pager_selector = "#DriverAttendanceReportListPager";

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
        url: "/Transport/GetDriverAttendanceReportsJqGrid",
        datatype: 'json',
        type: 'GET',
        colNames: ['Campus',' Driver Name ','Driver Id', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28', '29', '30', '31', 'No.Of Days Absent','No.of Days Leave','No.of.Holydays', 'No.Of Days Present', 'Percentage', 'Total Attendance', 'Total Percentage'],
        colModel: [
                                  { name:'Campus',index:'Campus',align:'center',sortable:false},
                                  { name: 'Name', index: 'Name',align:'center'},
                                  { name: 'DriverIdNo', index: 'DriverIdNo', align: 'center', sortable: false },
                                  { name: 'Date1', index: 'Date1', align: 'center', sortable: false },
                                  { name: 'Date2', index: 'Date2', align: 'center', sortable: false },
                                  { name: 'Date3', index: 'Date3', align: 'center', sortable: false },
                                  { name: 'Date4', index: 'Date4', align: 'center', sortable: false },
                                  { name: 'Date5', index: 'Date5', align: 'center', sortable: false },
                                  { name: 'Date6', index: 'Date6', align: 'center', sortable: false },
                                  { name: 'Date7', index: 'Date7', align: 'center', sortable: false },
                                  { name: 'Date8', index: 'Date8', align: 'center', sortable: false },
                                  { name: 'Date9', index: 'Date9', align: 'center', sortable: false },
                                  { name: 'Date10', index: 'Date10', align: 'center', sortable: false },
                                  { name: 'Date11', index: 'Date11', align: 'center', sortable: false },
                                  { name: 'Date12', index: 'Date12', align: 'center', sortable: false },
                                  { name: 'Date13', index: 'Date13', align: 'center', sortable: false },
                                  { name: 'Date14', index: 'Date14', align: 'center', sortable: false },
                                  { name: 'Date15', index: 'Date15', align: 'center', sortable: false },
                                  { name: 'Date16', index: 'Date16', align: 'center', sortable: false },
                                  { name: 'Date17', index: 'Date17', align: 'center', sortable: false },
                                  { name: 'Date18', index: 'Date18', align: 'center', sortable: false },
                                  { name: 'Date19', index: 'Date19', align: 'center', sortable: false },
                                  { name: 'Date20', index: 'Date20', align: 'center', sortable: false },
                                  { name: 'Date21', index: 'Date21', align: 'center', sortable: false },
                                  { name: 'Date22', index: 'Date22', align: 'center', sortable: false },
                                  { name: 'Date23', index: 'Date23', align: 'center', sortable: false },
                                  { name: 'Date24', index: 'Date24', align: 'center', sortable: false },
                                  { name: 'Date25', index: 'Date25', align: 'center', sortable: false },
                                  { name: 'Date26', index: 'Date26', align: 'center', sortable: false },
                                  { name: 'Date27', index: 'Date27', align: 'center', sortable: false },
                                  { name: 'Date28', index: 'Date28', align: 'center', sortable: false },
                                  { name: 'Date29', index: 'Date29', align: 'center', sortable: false },
                                  { name: 'Date30', index: 'Date30', align: 'center', sortable: false },
                                  { name: 'Date31', index: 'Date31', align: 'center', sortable: false },
                                  { name: 'noofAbsentdate', index: 'noofAbsentdate', align: 'center', sortable: false },
                                  { name: 'noofLeavedate', index: 'noofLeavedate', align: 'center', sortable: false },
                                  { name:'noofholydays',index:'noofholydays',align:'center',sortable:false},
                                  { name: 'noofpre', index: 'noofpre', align: 'center', sortable: false },
                                  { name: 'Percentage', index: 'Percentage', align: 'center', sortable: false },
                               
                                { name: 'totalAttendance', index: 'totalAttendance', align: 'center', sortable: false },
                                { name: 'totalPercentage', index: 'totalPercentage', align: 'center', sortable: false },
        ],
        rowNum: 50,
        rowList: [5, 10, 20, 50, 100],
        pager: pager_selector,
        sortname: 'Name',
        sortorder: "Desc",
        height: 230,
        shrinkToFit: false,
        autowidth: false,
        viewrecords: true,
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Report List',
        loadComplete: function () {
            var Searchtype = $('#ddlSearchType').val();
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
        $("#thumbnail").append(response + '&nbsp;')
    }
    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            var ExportType = "Export To Excel";
            window.open("GetDriverAttendanceReportsJqGrid" + '?rows=9999' +
                        '&page=1' +
                        '&sidx=Name' +
                        '&sord=desc' +
                        '&campus=' + $('#Campus').val() +
                        '&drivername='+$('#Name').val()+
                        '&driverid='+$('#DriverIdNo').val()+
                        '&searchmonth=' + $('#ddlSearchType').val() +
                        '&year=' + $('#ddlyear').val() +
                        '&ExportType=Excel'
                         );
            $("#thumbnail").append(response + '&nbsp;')
        }
    });


    // Fill Branch Code Details
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

    $("#Name").autocomplete({
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

    $.getJSON("/Attendance/GetMonthValbyAcademicYear?academicYear=" + $("#AcademicYear").val(),
         function (fillbc) {
             var monthVal = $("#ddlSearchType");
             monthVal.empty();
             monthVal.append($('<option/>',
            {
                value: "",
                text: "Select One"

            }));

             $.each(fillbc, function (index, itemdata) {
                 monthVal.append($('<option/>',
                     {
                         value: itemdata.Value,
                         text: itemdata.Text
                     }));

             });
         });

    $('#btnGetReport').click(function () {
        debugger;
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#ddlSearchType').val() == "") { ErrMsg("Please select the Month"); return false; }
        if ($('#ddlyear').val() == "") { ErrMsg("Please select the Year"); return false; }
      
        $(grid_selector).setGridParam(
                  {
                      datatype: "json",
                      url: "/Transport/GetDriverAttendanceReportsJqGrid",
                      postData: { campus: $('#Campus').val(), drivername: $('#Name').val(), driverid: $('#DriverIdNo').val(), searchmonth: $('#ddlSearchType').val(),year:$('#ddlyear').val() },
                      page: 1
                  }).trigger('reloadGrid');
    });


});