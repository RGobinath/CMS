$(function () {
    var grid_selector = "#DescriptionForSelectedId";
    var pager_selector = "#DescriptionForSelectedIdPager";

    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".col-sm-6").width());
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


    var Id = $("#Id").val();
    var status = $("#Status").val();



    var id = $("#Id").val();
    $('#Back').click(function () {
        var url = $('#BackUrl').val();
        window.location.href = url;
    });

    jQuery(grid_selector).jqGrid({

        url: '/Home/DescriptionForSelectedIdJqGrid?Id=' + $("#Id").val(),
        datatype: 'json',
        mtype: 'GET',
        height: '80',
        autowidth: true,
        colNames: ['Commented By', 'Commented On', 'Rejection Comments', 'Resolution Comments'],
        colModel: [
        // { name: 'EntityRefId', index: 'EntityRefId', width: 80, align: 'left', sortable: true },
              {name: 'CommentedBy', index: 'UploadedBy', align: 'left', sortable: true },
              { name: 'CommentedOn', index: 'UploadedOn', align: 'left', sortable: true },
              { name: 'RejectionComments', index: 'RejectionComments', sortable: false },
              { name: 'ResolutionComments', index: 'ResolutionComments', sortable: false }
              ],
        rowNum: '10',
        autowidth: true,
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: '',
        pager: DescriptionForSelectedIdpager,
        sortorder: "",
        viewrecords: true,
        multiselect: false,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="fa fa-comments"></i> Discussion Forum'
    });
    $(window).triggerHandler('resize.jqGrid');
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
            {}, {}, {});

    jQuery("#Uploadedfileslist").jqGrid({
        mtype: 'GET',
        //url: '/Home/Documentsjqgrid?Id=' + Id,
        url: '/Home/Documentsjqgrid?Id=' + Id + '&AppName=CMS',
        datatype: 'json',
        height: '50',
        shrinkToFit: true,
        colNames: ['Upload Id', 'Uploaded By', 'Uploaded On', 'File Name', 'Document Type'],
        colModel: [
                          { name: 'Upload_Id', index: 'Upload_Id', hidden: true, key: true },
                          { name: 'UploadedBy', index: 'UploadedBy', width: '30%', align: 'left', sortable: false },
                          { name: 'UploadedOn', index: 'UploadedOn', width: '30%', align: 'left', sortable: false },
                          { name: 'FileName', index: 'FileName', width: '30%', align: 'left', sortable: false },
                          { name: 'DocumentType', index: 'DocumentType', width: '30%', align: 'left', sortable: false }
                          ],
        pager: '#uploadedfilesgridpager',
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        //           sortname: 'IssueNumber',
        //           sortorder: "Desc",
        multiselect: true,
        autowidth: true,
        viewrecords: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="ace-icon fa fa-upload bigger-110"></i> Uploaded Documents'
    });

    $(window).triggerHandler('resize.jqGrid');
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
            {}, {}, {});
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
function uploaddat(id1) {
    window.location.href = "/Home/uploaddisplay?Id=" + id1;
    processBusy.dialog('close');
}