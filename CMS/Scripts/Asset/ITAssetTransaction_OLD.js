$(function ($) {
    $('#divToCampusLocation').hide();
    $('#divService').hide();
    $('#divScrap').hide();
    $('#divTransactionComment').hide();
    $('.date-picker').datepicker({
        format: "dd/mm/yyyy",
        autoclose: true
    });
    $('#ddlTransactionType').change(function () {
        if ($('#ddlTransactionType').val() == "IntraCampus") {
            $('#divService').hide();
            $('#divScrap').hide();
            $('#divToCampus').hide();
            $('#txtToCampus').attr('readonly', true);
            $('#divToCampusLocation').show();
            $('#divTransactionComment').show();
        }
        else if ($('#ddlTransactionType').val() == "InterCampus") {
            $('#divService').hide();
            $('#divScrap').hide();
            $('#txtToCampus').attr('readonly', false);
            $('#divToCampusLocation').show();
            $('#divTransactionComment').show();
            $('#divToCampus').show();
        }
        else if ($('#ddlTransactionType').val() == "Service") {
            $('#divToCampusLocation').hide();
            $('#divService').show();
            $('#divScrap').hide();
            $('#divTransactionComment').hide();
        }
        else if ($('#ddlTransactionType').val() == "Scrap") {
            $('#divService').hide();
            $('#divToCampusLocation').hide();
            $('#divScrap').show();
            $('#divTransactionComment').hide();
        } else if ($('#ddlTransactionType').val() == "") {
            $('#divToCampusLocation').hide();
            $('#divService').hide();
            $('#divScrap').hide();
            $('#divTransactionComment').hide();
        }
    });
    $('#btnback').click(function () {
        window.location.href = '/Asset/ITAssetManagement';
    });

    $(window).on('resize.jqGrid', function () {
        $("#Uploadedfileslist").jqGrid('setGridWidth', $(".col-sm-8").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $("#Uploadedfileslist").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#Uploadedfileslist").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    jQuery("#Uploadedfileslist").jqGrid({
        mtype: 'GET',
        url: '/Asset/Documentsjqgrid?id=' + $('#AssetDet_Id').val(),
        datatype: 'json',
        height: '50',
        autowidth: true,
        shrinkToFit: true,
        colNames: ['Upload Id', 'Document Type', 'Document Name', 'Document Size', 'Uploaded Date'],
        colModel: [
                      { name: 'Id', index: 'Id', hidden: true, key: true },
                      { name: 'DocumentType', index: 'DocumentType', width: 307, align: 'left', sortable: false },
                       { name: 'DocumentName', index: 'DocumentName', width: 307, align: 'left', sortable: false },
                       { name: 'DocumentSize', index: 'DocumentSize', width: 307, align: 'left', sortable: false },
                       { name: 'UploadedDate', index: 'UploadedDate', width: 307, align: 'left', sortable: false }
        ],
        pager: '#uploadedfilesgridpager',
        rowNum: 10,
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'desc',
        multiselect: true,
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
            {}, {}, {});
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
});
function validateFormSubmit() {
    var FromCampus = $('#txtFromCampus').val();
    var ToCampus = $('#ddlToCampus').val();

    if ($('#ddlTransactionType').val() == "") {
        ErrMsg("Select Transaction Type!!");
        return false;
    }
    if ($('#ddlTransactionType').val() == "IntraCampus") {
        if ($('#txtToLocation').val() == "" || $('#txtInstalledOn').val() == "" || $('#txtTransactionCommentArea').val() == "") {
            ErrMsg("All fields are required!!");
            return false;
        }
    }
    else if ($('#ddlTransactionType').val() == "InterCampus") {
        if ($('#ddlToCampus').val() == "" || $('#txtToLocation').val() == "" || $('#txtInstalledOn').val() == "" || $('#txtTransactionCommentArea').val() == "") {
            ErrMsg("All fields are required!!");
            return false;
        }
        if (FromCampus == ToCampus) {
            ErrMsg("From campus and To campus should not be same!!");
            return false;
        }
    }
    else if ($('#ddlTransactionType').val() == "Service") {
        if ($('#txtDCNo').val() == "" || $('#txtDCDate').val() == "" || $('#txtPhysicalCondition').val() == "" || $('#txtServiceProblem').val() == "" || $('#txtVendor').val() == "") {
            ErrMsg("All fields are required!!");
            return false;
        }
    }
    else if ($('#ddlTransactionType').val() == "Scrap") {
        if ($('#txtInwardDate').val() == "" || $('#txtScrapPhysicalCondition').val() == "" || $('#txtScrapProblem').val() == "") {
            ErrMsg("All fields are required!!");
            return false;
        }
    }
}
function uploadvalidate() {
    if ($('#ddlDocumentType').val() == " " || $('#ddlDocumentType').val() == "") {
        ErrMsg("Please Select Document Type!");
        return false;
    }
    else if ($('#file1').val() == " " || $('#file1').val() == "") {
        ErrMsg("Please Upload a Document!");
        return false;
    }
    else {
        return true;
    }

}