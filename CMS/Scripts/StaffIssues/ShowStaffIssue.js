jQuery(function ($) {
    $("#txtDescription").attr("readonly", true).css("background-color", "#F5F5F5");
    $("#txtResolution").attr("readonly", true).css("background-color", "#F5F5F5");
    var Id = $("#Id").val();

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#DescriptionForSelectedId").jqGrid('setGridWidth', $(".col-sm-8").width());
        $("#Uploadedfileslist").jqGrid('setGridWidth', $(".col-xs-12").width());
    })

    jQuery("#DescriptionForSelectedId").jqGrid({

        url: '/StaffIssues/DescriptionForSelectedIdJqGrid?Id=' + $("#Id").val(),
        datatype: 'json',
        mtype: 'GET',
        height: '130',
        width: '1160',
        autowidth: true,
        colNames: ['Commented By', 'Commented On', 'Rejection Comments', 'Resolution Comments'],
        colModel: [
        // { name: 'EntityRefId', index: 'EntityRefId', width: 80, align: 'left'  },
              {name: 'CommentedBy', index: 'UploadedBy', align: 'left' },
              { name: 'CommentedOn', index: 'UploadedOn',  align: 'left' },
              { name: 'RejectionComments', index: 'RejectionComments', sortable: false },
              { name: 'ResolutionComments', index: 'ResolutionComments', sortable: false }
              ],
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: '',
        sortorder: "",
        viewrecords: true,
        multiselect: false,
        altRows: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-comments-o"></i> Discussion Forum'
    });


    jQuery("#Uploadedfileslist").jqGrid({
        mtype: 'GET',
        url: '/StaffIssues/StaffDocumentsjqgrid?Id=' + Id,
        datatype: 'json',
        height: '50',
        shrinkToFit: true,
        colNames: ['Uploaded By', 'Uploaded On', 'File Name'],
        colModel: [
                          { name: 'UploadedBy', index: 'UploadedBy', align: 'left', sortable: false, width: 60 },
                          { name: 'UploadedOn', index: 'UploadedOn', align: 'left', sortable: false, width: 60 },
                          { name: 'FileName', index: 'FileName', align: 'left', sortable: false, width: 90 }
                          ],
        pager: '#uploadedfilesgridpager',
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        //           sortname: 'IssueNumber',
        //           sortorder: "Desc",
        multiselect: true,
        viewrecords: true,
        altRows: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: '<i class="fa fa-folder-open-o"></i> Uploaded Documents'
    });

    $(".flip").click(function () {

        var icon = $('.icon', this);
        $(".panel").slideToggle("slow");
        icon.attr("src", this.attr("src") == up ? down : up);
    });
    $("#btnBack").click(function () {
        window.location.href = '/StaffIssues/StaffIssueManagement';
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
        $("#DescriptionForSelectedId").jqGrid('GridUnload');
        $("#Uploadedfileslist").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});

function uploaddat(id) {
    window.location.href = "/StaffIssues/uploaddisplay?Id=" + id;
    processBusy.dialog('close');
}



