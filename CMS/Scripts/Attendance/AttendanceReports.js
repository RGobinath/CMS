$(function () {
    var grid_selector = "#ReportList";
    var pager_selector = "#ReportListPager";

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
        url: "/Attendance/GetAttendanceReportsJqGrid",
        datatype: 'json',
        type: 'GET',
        colNames: ['', ' Name ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28', '29', '30', '31', 'No Of Days Absent', 'No Of Days Present', 'Percentage', 'Bought Forward', 'Total Attendance', 'Total Percentage'],
        colModel: [
                                  { name: 'PreRegNum', index: 'PreRegNum', hidden: true, key: true },
                                  { name: 'Name', index: 'Name', width: 60 },
                                  { name: 'Date1', index: 'Date1', width: 60, align: 'center', sortable: false },
                                  { name: 'Date2', index: 'Date2', width: 60, align: 'center', sortable: false },
                                  { name: 'Date3', index: 'Date3', width: 60, align: 'center', sortable: false },
                                  { name: 'Date4', index: 'Date4', width: 60, align: 'center', sortable: false },
                                  { name: 'Date5', index: 'Date5', width: 60, align: 'center', sortable: false },
                                  { name: 'Date6', index: 'Date6', width: 60, align: 'center', sortable: false },
                                  { name: 'Date7', index: 'Date7', width: 60, align: 'center', sortable: false },
                                  { name: 'Date8', index: 'Date8', width: 60, align: 'center', sortable: false },
                                  { name: 'Date9', index: 'Date9', width: 60, align: 'center', sortable: false },
                                  { name: 'Date10', index: 'Date10', width: 60, align: 'center', sortable: false },
                                  { name: 'Date11', index: 'Date11', width: 60, align: 'center', sortable: false },
                                  { name: 'Date12', index: 'Date12', width: 60, align: 'center', sortable: false },
                                  { name: 'Date13', index: 'Date13', width: 60, align: 'center', sortable: false },
                                  { name: 'Date14', index: 'Date14', width: 60, align: 'center', sortable: false },
                                  { name: 'Date15', index: 'Date15', width: 60, align: 'center', sortable: false },
                                  { name: 'Date16', index: 'Date16', width: 60, align: 'center', sortable: false },
                                  { name: 'Date17', index: 'Date17', width: 60, align: 'center', sortable: false },
                                  { name: 'Date18', index: 'Date18', width: 60, align: 'center', sortable: false },
                                  { name: 'Date19', index: 'Date19', width: 60, align: 'center', sortable: false },
                                  { name: 'Date20', index: 'Date20', width: 60, align: 'center', sortable: false },
                                  { name: 'Date21', index: 'Date21', width: 60, align: 'center', sortable: false },
                                  { name: 'Date22', index: 'Date22', width: 60, align: 'center', sortable: false },
                                  { name: 'Date23', index: 'Date23', width: 60, align: 'center', sortable: false },
                                  { name: 'Date24', index: 'Date24', width: 60, align: 'center', sortable: false },
                                  { name: 'Date25', index: 'Date25', width: 60, align: 'center', sortable: false },
                                  { name: 'Date26', index: 'Date26', width: 60, align: 'center', sortable: false },
                                  { name: 'Date27', index: 'Date27', width: 60, align: 'center', sortable: false },
                                  { name: 'Date28', index: 'Date28', width: 60, align: 'center', sortable: false },
                                  { name: 'Date29', index: 'Date29', width: 60, align: 'center', sortable: false },
                                  { name: 'Date30', index: 'Date30', width: 60, align: 'center', sortable: false },
                                  { name: 'Date31', index: 'Date31', width: 60, align: 'center', sortable: false },
                                  { name: 'noofAbsentdate', index: 'noofAbsentdate', width: 60, align: 'center', sortable: false },
                                  { name: 'noofpre', index: 'noofpre', width: 60, align: 'center', sortable: false },
                                  { name: 'Percentage', index: 'Percentage', width: 60, align: 'center', sortable: false },
                                { name: 'boughtForward', index: 'boughtForward', width: 60, align: 'center', sortable: false },
                                { name: 'totalAttendance', index: 'totalAttendance', width: 60, align: 'center', sortable: false },
                                { name: 'totalPercentage', index: 'totalPercentage', width: 60, align: 'center', sortable: false },
                      ],
        rowNum: 50,
        rowList: [5, 10, 20, 50, 100],
        pager: pager_selector,
        sortname: 'Name',
        sortorder: "Desc",
        height: 230,
        autowidth: true,
        viewrecords: true,
        caption: '<i class="ace-icon fa fa-user"></i>&nbsp;&nbsp;Report List',
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
        caption: "Export To Excel",
        onClickButton: function () {
            var ExportType = "Export To Excel";
            window.open("GetAttendanceReportsJqGrid" + '?rows=9999' +
                        '&page=1' +
                        '&sidx=Name' +
                        '&sord=desc' +
                        '&campus=' + $('#Campus').val() +
                        '&grade=' + $('#Grade').val() +
                        '&section=' + $('#Section').val() +
                        '&date=' + $('#txtFrmDate').val() +
                        '&searchmonth=' + $('#ddlSearchType').val() +
                        '&AcademicYear=' + $('#AcademicYear').val() +
                        '&allCampus=' + $('#allCampus').val() +
                        '&ExportType=Excel'
                         );
            $("#thumbnail").append(response + '&nbsp;')
        }
    });
    $("#AcademicYear").on("change", function (e) {
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
    });
    $("#Campus").change(function () {
        var campus = $("#Campus").val();
        $.getJSON("/Admission/Gradeddl1/", { campus: campus },
        function (modelData) {
            var select = $("#Grade");
            select.empty();
            select.append($('<option/>'
    , {
        value: "All",
        text: "All"
    }));
            $.each(modelData, function (index, itemData) {
                select.append($('<option/>',
        {
            value: itemData.gradcod,
            text: itemData.gradcod
        }));
            });
        });
    });
    $('#btnGetReport').click(function () {
        debugger;
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#Grade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
        if ($('#Section').val() == "Select") { ErrMsg("Please fill the Section"); return false; }
        if ($('#AcademicYear').val() == "Select") { ErrMsg("Please select the Academic Year"); return false; }
        if ($('#ddlSearchType').val() == "") { ErrMsg("Please select the Month"); return false; }
        $(grid_selector).setGridParam(
                  {
                      datatype: "json",
                      url: "/Attendance/GetAttendanceReportsJqGrid",
                      postData: { campus: $('#Campus').val(), grade: $('#Grade').val(), section: $('#Section').val(), searchmonth: $('#ddlSearchType').val(), AcademicYear: $('#AcademicYear').val(), allCampus: $('#allCampus').val() },
                      page: 1
                  }).trigger('reloadGrid');
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
});
