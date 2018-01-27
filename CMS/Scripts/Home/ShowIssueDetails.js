$(function () {
    var grid_selector = "#DescriptionForSelectedId";
    var pager_selector = "#DescriptionForSelectedIdPager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $("#jqgrid").width());
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
    var status = $("#Status").val();

    //$("#txtResolution").attr("disabled", true).css("background-color", "#F5F5F5");
    //$("#txtDescription").attr("disabled", true).css("background-color", "#F5F5F5");
    $("#txtResolution").attr("disabled", true);
    $("#txtDescription").attr("disabled", true);

    if (status == "LogIssue") {
        $("#tblIssueComments").hide();
        $(grid_selector).hide();
    }

    var id = $("#Id").val();

    $("#Back").click(function () {
        window.location.href = "/Home/StatusReport";
    });

    jQuery(grid_selector).jqGrid({

        url: "/Home/DescriptionForSelectedIdJqGrid?Id=" + $("#Id").val(),
        datatype: 'json',
        mtype: 'GET',
        height: '50',
        autowidth: true,
        colNames: ['Commented By', 'Commented On', 'Rejection Comments', 'Resolution Comments'],
        colModel: [
        // { name: 'EntityRefId', index: 'EntityRefId', width: 80, align: 'left', sortable: true },
              {name: 'CommentedBy', index: 'UploadedBy', align: 'left', sortable: true },
              { name: 'CommentedOn', index: 'UploadedOn', align: 'left', sortable: true },
              { name: 'RejectionComments', index: 'RejectionComments', sortable: false },
              { name: 'ResolutionComments', index: 'ResolutionComments', sortable: false }
              ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: '',
        sortorder: "",
        viewrecords: true,
        multiselect: false,
        caption: '<i class="ace-icon fa fa-list"></i>&nbsp;&nbsp;Discussion Forum',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(grid_selector).jqGrid('setGridWidth');
        }
    });
    //$(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
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