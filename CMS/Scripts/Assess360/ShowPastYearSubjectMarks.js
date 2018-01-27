$(function () {


    var grid_selector = "#ShowPastYearMarks";
    var pager_selector = "#ShowPastYearPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {

        $(grid_selector).jqGrid('setGridWidth', $('#grid').width());
    })
    //resize on sidebar collapse/expand 
    debugger;
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {

        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!s
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
        url: "/Assess360/GetJqgridShowPastYearMarks",
        datatype: 'Json',
        type: 'GET',

        height: 187,
        colNames: ['Id', 'PreRegNum', 'Student Name', 'Test I (10)', 'Test II (10)', 'Test III (10)', 'Project (20)', 'Total (50)', 'Total (100)', 'OutOf (50)', 'FA + SA', 'Grade', 'Subject', 'Semester', 'Campus', 'Grade', 'Section', 'Created By'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true },
            { name: 'PreRegNum', index: 'PreRegNum', key: true },
             { name: 'Name', index: 'Name', width: 300 },
            { name: 'Test1', index: 'Test1' },
            { name: 'Test2', index: 'Test2' },
            { name: 'Test3', index: 'Test3' },
            { name: 'Project', index: 'Project' },
            { name: 'FATotal', index: 'FATotal' },
            { name: 'SATotal', index: 'SATotal' },
            { name: 'Outof', index: 'Outof' },
            { name: 'Fa1andFa2', index: 'Fa1andFa2' },
            { name: 'SEMGrade', index: 'SEMGrade' },
            { name: 'Subject', index: 'Subject', hidden: true },
            { name: 'Semester', index: 'Semester', hidden: true },
            { name: 'Campus', index: 'Campus', hidden: true },
            { name: 'Grade', index: 'Grade',width:40,hidden: true },
            { name: 'Section', index: 'Section', hidden: true },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true },

        ],
        rowNum: 100,
        rowList: [100, 150, 200],
        sortname: 'Id',
        sortorder: 'Asc',
        autowidth: true,
        shrinkToFit: true,
        pager: pager_selector,
        caption: '<i class="fa fa-list-alt"></i>&nbsp;Past Year Subject Marks',
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
    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: " &nbsp;&nbsp;<i class='fa fa-file-excel-o'>&nbsp;</i><u>Export To Excel</u>",
        onClickButton: function () {
            window.open("GetJqgridShowPastYearMarks" + '?rows=9999' +
                         '&page=1' +
                         '&sidx=Id' +
                         '&sord=Asc' +
                         '&campus=' + $('#Campus').val() +
                         '&grade=' + $('#Grade').val() +
                         '&section=' + $('#Section').val() +
                         '&subject=' + $('#Subject').val() +
                         '&semester=' + $('#Semester').val() +
                         '&academicyear=' + $('#AcademicYear').val() +
                         '&ExportType=Excel'
                         );
        }
    });
    //        jQuery(grid_selector).jqGrid('setGroupHeaders', {
    //            useColSpanStyle: false,
    //            groupHeaders: [
    //             { startColumnName: 'Test1', numberOfColumns: 5, titleText: '<em>Formative Assessment (FA)</em>' },
    //             { startColumnName: 'SATotal', numberOfColumns: 2, titleText: '<em>Summative Assessment (SA)</em>' },
    //             { startColumnName: 'Fa1andFa2', numberOfColumns: 2, titleText: '<em>SEM</em>' },
    //            ]
    //        });
    jQuery(grid_selector).jqGrid('setGroupHeaders', {
        useColSpanStyle: true,
        groupHeaders: [
             { startColumnName: 'Test1', numberOfColumns: 5, titleText: '<em>Formative Assessment (FA)</em>', useColSpanStyle: false },
             { startColumnName: 'SATotal', numberOfColumns: 2, titleText: '<em>Summative Assessment (SA)</em>', useColSpanStyle: false },
             { startColumnName: 'Fa1andFa2', numberOfColumns: 2, titleText: '<em>SEM</em>', useColSpanStyle: false },
            ]
    });



    $('#search').click(function () {
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#Grade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
        if ($('#Section').val() == "") { ErrMsg("Please fill the Section"); return false; }
        if ($('#Subject').val() == "") { ErrMsg("Please fill the Subject"); return false; }
        if ($('#Semester').val() == "") { ErrMsg("Please fill the Semester"); return false; }
        if ($('#AcademicYear').val() == "") { ErrMsg("Please fill the Academic Year"); return false; }

        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: "/Assess360/GetJqgridShowPastYearMarks",
                    postData: { Campus: $('#Campus').val(), Grade: $('#Grade').val(), Section: $('#Section').val(), Semester: $('#Semester').val(), Subject: $('#Subject').val(), AcademicYear: $('#AcademicYear').val() },
                    page: 1
                }).trigger("reloadGrid");

    });


    $('#reset').click(function () {
        $('#Campus').val("");
        $('#Grade').val("");
        $('#Section').val("");
        $('#Subject').val("");
        $('#Semester').val("");
        $('#AcademicYear').val("");
        jQuery(grid_selector).jqGrid('clearGridData')
.jqGrid('setGridParam', { data: data, page: 1 })
.trigger('reloadGrid');
    });


    //        $('#reset').click(function () {
    //            window.location.href = '@Url.Action("ShowPastYearSubjectMarks", "Assess360")';
    //        });

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
        gradeddl();
    });
});



function gradeddl() {

    var campus = $("#Campus").val();
    $.getJSON("/Admission/Gradeddl1/", { campus: campus },
                    function (modelData) {
                        var select = $("#Grade");
                        select.empty();
                        select.append($('<option/>'
            , {
                value: "",
                text: "Select Grade"
            }));
                        $.each(modelData, function (index, itemData) {

                            select.append($('<option/>',
            {
                value: itemData.gradcod,
                text: itemData.gradcod
            }));
                        });
                    });
}
