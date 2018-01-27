jQuery(function ($) {
    debugger;
    if ($('#IsOtherReason').val() == "True") {
        $("#othrReason").show();
        $('#otherReason').val($('#ReasonForTCRequest').val());
        $('#ddlReasonForTCRequest').val("Other");

    }
    else { $("#othrReason").hide(); }
    $("#Back").click(function () {
        // window.location.href = 'Url.Action("AdmissionManagement", "Admission")';
        window.location.href = "/Admission/TCRequestList";
    });
    $("#file1").click(function () {
        var Id = $("#Id").val();
        if (Id == 0) {
            ErrMsg("Please create TC Request by clicking Save button.");
            return false;
        }
    });
    $('#file1').ace_file_input();

    var grid_selector = "#Documents-grid-table";
    var pager_selector = "#Documents-grid-pager";

    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#Uploadedfileslist").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    $("#ddlReasonForTCRequest").change(function () {
        if ($("#ddlReasonForTCRequest").val() == "Other") {
            $("#othrReason").show();
        }
        else {
            $("#othrReason").hide();
        }
    });


    jQuery("#Uploadedfileslist").jqGrid({
        mtype: 'GET',
        url: '/Home/Documentsjqgrid?Id=' + $("#Id").val() + '&AppName=Admission',
        datatype: 'json',
        height: '70',
        autowidth: true,
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
        sortname: 'Upload_Id',
        sortorder: 'desc',
        multiselect: true,
        viewrecords: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }
        //caption: '<i class="ace-icon fa fa-upload bigger-110"></i> Uploaded Documents'
    });
    $(window).triggerHandler('resize.jqGrid');
    jQuery("#Uploadedfileslist").jqGrid('navGrid', '#uploadedfilesgridpager',
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
        {}, {
            url: '/Home/DeleteUploadedFileById/',
            beforeShowForm: function (params) {
                debugger;
                var selectedrow = $("#Uploadedfileslist").getGridParam('selrow');
                var sdata = $("#Uploadedfileslist").getRowData(selectedrow);
                if (sdata.UploadedBy == '@Session["UserId"]') {
                    return { Upload_Id: parseInt(sdata.Upload_Id) }
                }
                else {
                    //$('#delmodUploadedfileslist').dialog('close');
                    ErrMsg("You cannot delete the file uploaded by others.");
                    $("#DelError", "#" + dtbl).hide();
                    return false;
                }
                //  return { Upload_Id: sdata.Upload_Id }
            }
        }, {});


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
    window.location.href = "/Home/uploaddisplay?Id=" + id1 + '&AppName=Admission';
    //processBusy.dialog('close');
}