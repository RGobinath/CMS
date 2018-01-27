
function getCampus() {
    $.getJSON("/Base/FillAllBranchCode",
function (fillig) {
    var Campusddl = $("#ddlCampus");
    Campusddl.empty();
    Campusddl.append($('<option/>', { value: "", text: "Select One" }));
    $.each(fillig, function (index, itemdata) {
        Campusddl.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
    });
});
}

function getYear() {
    var Yearddl = $("#ddlYear");
    Yearddl.empty();
    Yearddl.append($('<option/>', { value: "", text: "Select One" }));

    var AcademicYear = new Date().getFullYear();
    for (var i = AcademicYear; i <= AcademicYear + 1; i++) {
        Yearddl.append($('<option/>', { value: i, text: i }));
    }
}
function getAcademicYear() {
    var spec;
    var tempData = '';
    var AcademicYear = new Date().getFullYear();
    for (var i = AcademicYear; i <= AcademicYear + 1; i++) {
        tempData += i + ':' + i;
        if (i != AcademicYear + 1)
            tempData += ';';
    }
    spec = tempData;
    return spec;
}
function GetMonth() {
    var spc;
    var temp = '';
    var Month = new Date().getMonth();
    for (var i = Month; i <= Month; i++) {
        temp += i + ':' + i;
        if (i != Month)
            temp += ';';
    }
    spc = temp;
    return spc;
}
$(function () {
    getCampus();
    getYear();
    var grid_selector = "#StaffHolidaysMasterGridListJqGrid";
    var pager_selector = "#StaffHolidaysMasterGridListJqGridpager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        jQuery(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    var NoOfHolidaysEditId="";
    var jqGridstartDate = new Date();
    var jqGridstartDate = 01 + '/' + 04 + '/' + 1947;
    jQuery(grid_selector).jqGrid({

        url: '/StaffManagement/StaffHolidaysMasterGridListJqGrid',
        datatype: 'json',
        height: 300,
        mtype: 'GET',
        colNames: ['StaffHolidaysMaster_Id','Holiday Type', 'Campus', 'Holiday Date', 'Year', 'Month', 'No.of holidays', 'Created By', 'Created Date', 'Modified By', 'Modified Date', 'Month Number', 'Descriptions'],
        colModel: [
                    { name: 'StaffHolidaysMaster_Id', index: 'StaffHolidaysMaster_Id', hidden: true, editable: true, key: true },
                    {
                        name: 'HolidayType', index: 'HolidayType', sortable: true, editable: true, editrules: { required: true }, edittype: "select", editoptions: {
                            sopt: ['eq'],
                            value: {
                                '': 'Select', 'WeekOff': 'Week Off', 'Holiday': 'Holiday'
                            },
                            style: "width: 160px; height: 25px; font-size: 0.9em"
                        }, editrules: { required: true },
                        sortable: true, stype: 'select'
                    },
                    {
                        name: 'Campus', index: 'Campus', sortable: true, editable: true, edittype: 'select', editrules: { required: true },
                        editoptions: {
                            style: "width: 160px; height: 25px; font-size: 0.9em",
                            dataUrl: '/Base/FillAllBranchCode',
                            buildSelect: function (data) {
                                Campus = jQuery.parseJSON(data);
                                var s = '<select>';
                                s += '<option value=" ">Select</option>';
                                if (Campus && Campus.length) {
                                    for (var i = 0, l = Campus.length; i < l; i++) {
                                        s += '<option value="' + Campus[i].Value + '">' + Campus[i].Text + '</option>';
                                    }
                                }
                                return s + "</select>";
                            }
                        }, search: true,
                    },
                      {
                          name: 'HolidayDate', index: 'HolidayDate', sortable: true, editable: true,
                          hidden: true, editrules: { edithidden: true, required: true }, editoptions: {
                              dataEvents: [{
                                  type: 'change',
                                  fn: function (e) {
                                      jqGridstartDate = $("#HolidayDate").val();
                                      var Holidaydatestring = jqGridstartDate.split(',');
                                      var HolidayDateCount = Holidaydatestring.length;
                                      $("#NoOfHolidays").attr("value", HolidayDateCount);
                                  }
                              }],
                              dataInit: function (elem) {
                                  $(elem).keypress(function (e) {
                                      if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                          return false;
                                      }
                                  }).attr('readonly', 'readonly');
                                  $(elem).datepicker({
                                      format: "dd/mm/yyyy",
                                      multidate: true,
                                  }).closeAfterAdd;
                              }
                          }, search: false
                      },
                    {
                        name: 'Year', index: 'Year', sortable: true, editable: false, edittype: 'select', editoptions: {
                            value: getAcademicYear(),
                            style: "width: 160px; height: 22px; font-size: 0.9em"
                        },
                    },
                    {
                        name: 'Month', index: 'Month', sortable: true, editable: false, editrules: { required: true }, edittype: "select", editoptions: {
                            sopt: ['eq'],
                            value: {
                                '': 'Select', 'January': 'January', 'Feburary': 'Feburary', 'March': 'March', 'April': 'April', 'May': 'May', 'June': 'June',
                                'July': 'July', 'August': 'August', 'September': 'September', 'October': 'October', 'November': 'November', 'December': 'December'
                            },
                            style: "width: 160px; height: 25px; font-size: 0.9em"
                        }, editrules: { required: true },
                        sortable: true, stype: 'select'
                    },
                    {
                        name: 'NoOfHolidays', index: 'NoOfHolidays', sortable: true, editable: true, editoptions: { readonly: "readonly" }
                    },
                    { name: 'CreatedBy', index: 'CreatedBy', sortable: true, hidden: true },
                    { name: 'CreatedDate', index: 'CreatedBy', sortable: true, hidden: true },
                    { name: 'ModifiedBy', index: 'ModifiedBy', sortable: true, hidden: true },
                    { name: 'ModifiedDate', index: 'ModifiedDate', sortable: true, hidden: true },
                    { name: 'MonthNumber', index: 'MonthNumber', sortable: true, hidden: true },
                    { name: 'Descriptions', index: 'Descriptions', sortable: true, editable: true, hidden: false, edittype: 'textarea', editoptions: { style: "height:120px;" }},

        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        sortname: 'StaffHolidaysMaster_Id',
        sortorder: 'desc',
        altRows: true,
        multiselect: true,
        userDataOnFooter: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);

        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp; Staff Holidays Master",
    });

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size    
    //navButtons
    debugger;
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
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
            {
                url: '/StaffManagement/SaveOrUpdateStaffHolidaysMaster', modal: false, closeAfterEdit: true,
                beforeShowForm: function (params){
                    $("#tr_NoOfHolidays").hide();
                    $("#tr_HolidayDate").hide();
                    $("#tr_Campus").hide();
                    },
            }, //Edit
            {
                url: '/StaffManagement/SaveOrUpdateStaffHolidaysMaster', modal: false, closeAfterAdd: false,NoOfHolidaysEditId:false

            }, //Add
            {
                url: '/StaffManagement/DeleteStaffHolidaysMaster', beforeShowForm: function (params) {
                    selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                    return { Id: selectedrows }
                }
            },
            {},
            {});
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
    $('#btnSearch').click(function () {
        $(grid_selector).setGridParam(
                       {
                           datatype: "json",
                           url: "/StaffManagement/StaffHolidaysMasterGridListJqGrid",
                           postData: { Campus: $('#ddlCampus').val(), Year: $('#ddlYear').val(), Month: $('#ddlMonth').val() },
                           page: 1
                       }).trigger('reloadGrid');
    });

    $('#btnReset').click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                       {
                           datatype: "json",
                           url: "/StaffManagement/StaffHolidaysMasterGridListJqGrid",
                           postData: { Campus: $('#ddlCampus').val(), Year: $('#ddlYear').val(), Month: $('#ddlMonth').val() },
                           page: 1
                       }).trigger('reloadGrid');
    });
});