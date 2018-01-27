$(function () {
    var grid_selector = "#ReportCardRequestGrid";
    var pager_selector = "#ReportCardRequestGridPager";

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
    var Year = "@ViewBag.CurYear";
    $("#ddlAcademic").val(Year);
    $(grid_selector).jqGrid({
        url: "/Assess360/ReportCardRequestJqGrid",
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'RequestNo', 'Campus', 'Grade', 'Section', 'AcademicYear', 'Created By', 'Created Date', 'ModifiedBy', 'ModifiedDate', ''],
        colModel: [
                      { name: 'Id', index: 'Id', key: true, hidden: true },
                      { name: 'RequestNo', index: 'RequestNo', align: '     center', width: 200 },
                      { name: 'Campus', index: 'Campus' },
                      { name: 'Grade', index: 'Grade' },
                      { name: 'Section', index: 'Section', sortable: true },
                      { name: 'AcademicYear', index: 'AcademicYear' },
                      { name: 'CreatedBy', index: 'CreatedBy' },
                      { name: 'CreatedDate', index: 'CreatedDate' },
                      { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true },
                      { name: 'CreatedBy', index: 'CreatedBy', editable: true, hidden: true }
        ],
        pager: pager_selector,
        rowList: [20, 50, 100],
        sortname: 'Id',
        sortorder: 'Desc',
        height: 250,
        autowidth: true,
        viewrecords: true,
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;ReportCard Request List',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }
    });
    //trigger window resize to make the grid get the correct size
    $(window).triggerHandler('resize.jqGrid');
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
    }
    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    $("#ddlCampus").change(function () {
        gradeddl();
    });
    $('#search').click(function () {
        $(grid_selector).setGridParam({
            datatype: "json",
            url: "/Assess360/ReportCardRequestJqGrid",
            postData: {
                Campus: $('#ddlCampus').val(),
                Grade: $('#ddlGrade').val(),
                Section: $('#ddlSection').val(),
                AcYear: $('#ddlAcademic').val()
            },
            page: 1
        }).trigger("reloadGrid");
    });

    $('#reset').click(function () {
        window.location.href = "/Assess360/ReportCardRequest";
    });

    $('#saveRequest').click(function () {
        var campus = $('#ddlCampus').val();
        var grade = $('#ddlGrade').val();
        var section = $('#ddlSection').val();
        var ayear = $('#ddlAcademic').val();
        if (campus == "") { ErrMsg("Please fill the Campus"); return false; }
        else if (grade == "") { ErrMsg("Please fill the Grade"); return false; }
        else if (section == "") { ErrMsg("Please fill the Section"); return false; }
        else if (ayear == "") { ErrMsg("Please fill the AcademicYear"); return false; }
        else {
            $.ajax({
                type: 'GET',
                async: false,
                dataType: "json",
                url: '/Assess360/CreateReportCardRequest?Campus=' + campus + '&Grade=' + grade + '&Section=' + section + '&AcYear=' + ayear,
                success: function (data) {
                    if (data == "Success") {
                        InfoMsg("Successfully Created Report Card Request");
                        $(grid_selector).trigger("reloadGrid");
                    }
                    else
                        ErrMsg(data);
                }
            });

        }
    });
});
function gradeddl() {
    var e = document.getElementById('ddlCampus');
    var campus = e.options[e.selectedIndex].value;
    $.getJSON("/Admission/CampusGradeddl/", { Campus: campus },
            function (modelData) {
                var select = $("#ddlGrade");
                select.empty();
                select.append($('<option/>', { value: "", text: "--Select Grade--" }));
                $.each(modelData, function (index, itemData) {
                    select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                });
            });
}
function ShowReportRequest(Id) {
    window.location.href = "/Assess360/ReportCardCBSE?RptRequestId=" + Id;
}