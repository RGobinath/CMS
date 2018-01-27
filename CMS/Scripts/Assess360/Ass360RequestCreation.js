$(function () {

    $('#CreatedBy').val($("#CreatedBy").val());
    $('#UserRole').val($("#UserRole").val());

    var grid_selector = "#Ass360ReqCreation";
    var pager_selector = "#Ass360ReqCreationPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $('#grid').width());
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

    //pager icon
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

    $(grid_selector).jqGrid({
        url: '/Assess360/GetjQgridAss360RequestCreation',
        datatype: 'json',
        type: 'GET',
        autowidth: true,
        height: 217,
        colNames: ['Id', 'PreRegNum', 'NewId', 'Name', 'Campus', 'Grade', 'Section', 'AdmissionStatus', 'AcademicYear'],
        colModel: [
            { name: 'Id', index: 'Id' },
            { name: 'PreRegNum', index: 'PreRegNum' },
            { name: 'NewId', index: 'NewId' },
            { name: 'Name', index: 'Name',width:300 },
            { name: 'Campus', index: 'Campus' },
            { name: 'Grade', index: 'Grade' },
            { name: 'Section', index: 'Section' },
            { name: 'AdmissionStatus', index: 'AdmissionStatus' },
            { name: 'AcademicYear', index: 'AcademicYear' }
            ],
        rowNum: 100,
        rowList: [100, 150, 200, 250, 300],
        pager: pager_selector,
        viewrecords: true,
        autowidth: true,
        sortname: 'Id',
        sortorder: 'Asc',
        caption: '<i class="fa fa-pencil-square-o">&nbsp;</i>Assess360 Request Creation',
        loadComplete: function () {
            var table = this;

            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });
    jQuery(grid_selector).navGrid(pager_selector, { add: false, edit: false, del: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green', search: false });

    $('#search').click(function () {
        if ($('#Campus').val() == "") {
            ErrMsg("Please fill the Campus");
            return false;
        }
        if ($('#Grade').val() == "") {
            ErrMsg("Please fill the Grade");
            return false;
        }
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: "/Assess360/GetjQgridAss360RequestCreation",
                    postData: { campus: $('#Campus').val(), grade: $('#Grade').val(), section: $('#Section').val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $('#reset').click(function () {
        $('#Campus').val("");
        $('#Grade').val("");
        $('#Section').val("");
        jQuery(grid_selector).jqGrid('clearGridData')
                     .jqGrid('setGridParam', { data: data, page: 1 })
                     .trigger('reloadGrid');
    });

    $('#ass360btn').click(function () {
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#Grade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: "/Assess360/Ass360RequestSaveFunction",
                    postData: { Campus: $('#Campus').val(), Grade: $('#Grade').val(), Section: $('#Section').val(), CreatedBy: $('#CreatedBy').val(), UserRole: $('#UserRole').val() },
                    page: 1
                }).trigger("reloadGrid");
        // InfoMsg("Assess360 is Created", function () { window.location.href = '@Url.Action("Assess360Inbox", "Assess360")' });
               // InfoMsg("Assess360 is Created", function () { window.location.href = url });

        InfoMsg("Assess360 is Created", function () {window.location.href = $("#BackUrl").val();});
    });

    $.getJSON("/Base/FillBranchCode",
    function (fillig) {
        var ddlcam = $("#Campus");
        ddlcam.empty();
        ddlcam.append($('<option/>', { value: "", text: "Select One" }));

        $.each(fillig, function (index, itemdata) {
            ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
        });
    });
});