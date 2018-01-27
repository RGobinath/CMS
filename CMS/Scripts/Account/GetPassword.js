function FillCampusDll() {
    $.getJSON("/Base/FillAllBranchCode",
      function (fillbc) {
          var ddlbc = $("#ddlCampus");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillGradeByCampus() {
    var Campus = $("#ddlCampus").val();
    $.getJSON("/Assess360/GetGradeByCampus?Campus=" + Campus,
      function (fillbc) {
          var ddlbc = $("#ddlGrade");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillAcademicYearDll() {
    $.getJSON("/Base/GetJsonAcademicYear",
      function (fillbc) {
          var ddlbc = $("#ddlAcademicYear");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
jQuery(function ($) {
    FillCampusDll();
    FillAcademicYearDll();
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";
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
        url: '/Account/GetPasswordJqGrid',
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'Campus', 'Grade', 'Section','Academic Year', 'NewId', 'PreRegNum', 'Employee Id', 'User Id', 'User Type', 'User Name', 'EmailId', 'Password', 'Is Active'],
        colModel: [

            { name: 'Id', index: 'Id', hidden: true },
            { name: 'Campus', index: 'Campus' },
            { name: 'Grade', index: 'Grade' },
            { name: 'Section', index: 'Section' },
            { name: 'AcademicYear', index: 'AcademicYear' },
            { name: 'NewId', index: 'NewId', hidden: true },
            { name: 'PreRegNum', index: 'PreRegNum', hidden: true },
            { name: 'EmployeeId', index: 'EmployeeId' },
            { name: 'UserId', index: 'UserId' },
            { name: 'UserType', index: 'UserType' },
            { name: 'UserName', index: 'UserName' },
            { name: 'EmailId', index: 'EmailId' },
            { name: 'Password', index: 'Password' },
            { name: 'IsActive', index: 'IsActive' },
        ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        autowidth: true,
        height: 250,
        viewrecords: true,
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                //styleCheckbox(table);
                //updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: 'User List'
    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
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
            {

            }, //Edit
            {

            }, //Add
              {

              },
               {},
                {});
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            window.open("GetPasswordJqGrid" + '?Campus=' + $("#ddlCampus").val() + '&Grade=' + $("#ddlGrade").val() + '&Section=' + $("#ddlSection").val() + '&UserType=' + $("#ddlUserType").val() + '&userid=' + $('#txtsearch').val() + '&AcademicYear=' + $('#ddlAcademicYear').val() + '&rows=9999' + '&ExptXl=1');
        }
    });
    //switch element when editing inline
    function aceSwitch(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=checkbox]')
                    .addClass('ace ace-switch ace-switch-5')
                    .after('<span class="lbl"></span>');
        }, 0);
    }
    //enable datepicker
    function pickDate(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=text]')
                        .datepicker({ format: 'yyyy-mm-dd', autoclose: true });
        }, 0);
    }
    $("#ddlCampus").change(function () {
        FillGradeByCampus();
    });
    $('#btnsearch').click(function () {
        debugger;
        var userid = $('#txtsearch').val();
        var Campus = $('#ddlCampus').val();
        var Grade = $('#ddlGrade').val();
        var Section = $("#ddlSection").val();
        var UserType = $("#ddlUserType").val();
        var AcademicYear = $("#ddlAcademicYear").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Account/GetPasswordJqGrid',
                    postData: { userid: userid, Campus: Campus, Grade: Grade, Section: Section, UserType: UserType, AcademicYear: AcademicYear },
                    page: 1
                }).trigger("reloadGrid");
    });

    $("#reset").click(function () {
        $(grid_selector).clearGridData();
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam({
            url: '/Account/GetPassword',
            postData: { userid: "", Campus: "", Grade: "", Section: "", UserType: "" },

        }).trigger("reloadGrid");
    });

    // Autocomplete Search Example....
    $("#txtsearch").autocomplete({
        source: function (request, response) {
            $.getJSON('/Account/GetUserIds?term=' + request.term, function (data) {
                response(data);
            });
        },
        minLength: 1,
        delay: 100
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
    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});
