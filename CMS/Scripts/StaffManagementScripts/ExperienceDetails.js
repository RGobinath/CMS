jQuery(function ($) {
    var grid_selector = "#ExperienceList";
    var pager_selector = "#ExperienceListPager";
    var truefalse;//To assign True or False value for edit and delete(icon) in JqGrid
    //For Edit and View page of staff details
    if ($('#ForPage').val() == "StaffProfile") {
        truefalse = false;
        $("#divexpadd").hide();
        $("#divpreviousadd").hide();
        $(window).on('resize.jqGrid', function () {
            var page_width = $(".page-content").width();
            page_width = page_width - 0;
            $(grid_selector).jqGrid('setGridWidth', page_width);
        })
    }
    else {
        truefalse = true;
        $("#divexpadd").show();
        $("#divpreviousadd").show();
       // $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
        $(window).on('resize.jqGrid', function () {
            var page_width = $(".page-content").width();
            page_width = page_width - 46;
            $(grid_selector).jqGrid('setGridWidth', page_width);
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
        url: '/StaffManagement/Experiencejqgrid',
        datatype: 'json',
        height: 65,
        colNames: ['Organisation Name', 'Location', 'Start Date', 'Till Date', 'Last Designation', 'SpecificReasonForLeaving', '', ''],
        colModel: [
                          { name: 'EmployerName', index: 'EmployerName', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'Location', index: 'Location', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'StartDate', index: 'StartDate', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'TillDate', index: 'TillDate', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'LastDesignation', index: 'LastDesignation', align: 'left', sortable: false, editable: true, formatter: nildata },
                           { name: 'SpecificReasonForLeaving', index: 'SpecificReasonForLeaving', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'Id', index: 'Id', width: '30%', align: 'left', sortable: false, hidden: true, key: true, editable: true, formatter: nildata },
                            { name: 'PreRegNum', index: 'PreRegNum', width: 50, align: 'left', sortable: false, hidden: true, editable: true, formatter: nildata }
                          ],
        viewrecords: true,
        rowNum: 7,
        rowList: [7, 10, 30],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Asc',
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
        caption: "<i class='ace-icon fa fa-building'></i>&nbsp;Experience List"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
                edit: truefalse,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: truefalse,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, { url: '/StaffManagement/EditExperience/', left: '10%', top: '10%', height: '50%', width: 400, labelswidth: 60, closeAfterEdit: true, closeOnEscape: true, reloadAfterSubmit: true },
            {}, { url: '/StaffManagement/DeleteExperience' }, {})

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
    $("#ExpAdd").click(function () {

        if (document.getElementById("empname").value == "") {
            ErrMsg("Please Enter Organisation Name!");
            return false;
        }
        else {

            var empname = document.getElementById("empname").value;
            var location = document.getElementById("location").value;
            var strtdate = document.getElementById("startdate").value;
            var enddate = document.getElementById("tilldate").value;
            var lastdesig = document.getElementById("lastdesignation").value;
            var specificreasonforleaving = document.getElementById("specificreasonforleaving").value;

            $.ajax({
                url: '/StaffManagement/AddExperience',
                type: 'POST',
                dataType: 'json',
                data: { empname: empname, location: location, strtdate: strtdate, enddate: enddate, lastdesig: lastdesig, specificreasonforleaving: specificreasonforleaving },
                traditional: true,
                success: function (data) {
                    //  alert('2');
                    $(grid_selector).setGridParam({ url: '/StaffManagement/Experiencejqgrid' }).trigger("reloadGrid");
                    document.getElementById("empname").value = '';
                    document.getElementById("location").value = '';
                    document.getElementById("startdate").value = '';
                    document.getElementById("tilldate").value = '';
                    document.getElementById("lastdesignation").value = '';
                    document.getElementById("specificreasonforleaving").value = '';
                },
                loadError: function (xhr, status, error) {
                    msgError = $.parseJSON(xhr.responseText).Message;
                    ErrMsg(msgError, function () { });
                }
            });
        }
    });
});