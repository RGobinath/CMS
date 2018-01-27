jQuery(function ($) {
    var grid_selector = "#QualificationList";
    var pager_selector = "#QualificationListPager";

    var truefalse;//To assign True or False value for edit and delete(icon) in JqGrid
    //For Edit and View page of staff details
    if ($('#ForPage').val() == "StaffProfile") {
        truefalse = false;
        $(window).on('resize.jqGrid', function () {
            var page_width = $(".page-content").width();
            page_width = page_width - 90;
            $(grid_selector).jqGrid('setGridWidth', page_width);
        })
    }
    else {
        truefalse = true;
        $(window).on('resize.jqGrid', function () {
            $(grid_selector).jqGrid('setGridWidth', $(".tab-content").width() - 40);
        })
    }   
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
    jQuery(grid_selector).jqGrid({
        url: '/StaffManagement/qualificationjqgrid',
        datatype: 'json',
        height: 100,
        colNames: ['Course', 'Board/University', 'School/College', 'Year Of Completion', 'Major Subjects', 'Percentage', '', ''],
        colModel: [
                          { name: 'Course', index: 'Course', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'Board', index: 'Board', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'Institute', index: 'Institute', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'YearOfComplete', index: 'YearOfComplete', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'MajorSubjects', index: 'MajorSubjects', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'Percentage', index: 'Percentage', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'Id', index: 'Id', align: 'left', sortable: false, hidden: true, key: true, editable: true, formatter: nildata },
                          { name: 'PreRegNum', index: 'PreRegNum', align: 'left', sortable: false, hidden: true, editable: true, formatter: nildata }
                          ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        autowidth: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-book'></i>&nbsp;Qualification Grid"
    });

    //    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });

    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: truefalse,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: truefalse,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: truefalse,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            { url: '/StaffManagement/EditQualification/', left: '10%', top: '10%', height: '50%', width: 400, labelswidth: 60, closeAfterEdit: true, closeOnEscape: true, reloadAfterSubmit: true }, //Edit
            {}, { url: '/StaffManagement/Deletequalification' }, {}, {})

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
    function nildata(cellvalue, options, rowObject) {
        if ((cellvalue == '') || (cellvalue == null) || (cellvalue == 0)) {
            return ''
        }
        else {
            cellvalue = cellvalue.replace('&', 'and');
            //str = str.replace(/\&lt;/g, '<');
            return cellvalue
        }
    }
    $("#btnAddQualData").click(function () {
        if (document.getElementById("course").value == "") {
            ErrMsg("Please Enter Course!");
            return false;
        }
        else {
            debugger;
            var course = document.getElementById("course").value;
            var board = document.getElementById("board").value;
            var school = document.getElementById("institute").value;
            var yearofcomplete = document.getElementById("yearofcomplete").value;
            var subjects = document.getElementById("majorsubject").value;
            var percent = document.getElementById("percentage").value;
            $.ajax({
                url: '/StaffManagement/AddQualification',
                type: 'POST',
                dataType: 'json',
                data: { course: course, board: board, school: school, yearofcomplete: yearofcomplete, subjects: subjects, percent: percent },
                traditional: true,
                success: function (data) {
                    $(grid_selector).setGridParam({ url: '/StaffManagement/qualificationjqgrid' }).trigger("reloadGrid");
                    document.getElementById("course").value = '';
                    document.getElementById("board").value = '';
                    document.getElementById("institute").value = '';
                    document.getElementById("yearofcomplete").value = '';
                    document.getElementById("majorsubject").value = '';
                    document.getElementById("percentage").value = '';
                },
                loadError: function (xhr, status, error) {
                    msgError = $.parseJSON(xhr.responseText).Message;
                    ErrMsg(msgError, function () { });
                }
            });
            return true;
        }
    });
});