jQuery(function ($) {

    $('#file1').ace_file_input();

    var grid_selector = "#Documents-grid-table";
    var pager_selector = "#Documents-grid-pager";

    //resize to fit page size 
    $(window).on('resize.jqGrid', function () {
        var page_width = $(".page-content").width();
        //var page_width = $(grid_selector).closest('.tab-pane').width();
        page_width = page_width - 27;
        $(grid_selector).jqGrid('setGridWidth', page_width);
    })

    //resize on sidebar collapse/expand 
    //var page_width = $(grid_selector).closest('.tab-pane').width();
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(grid_selector).jqGrid('setGridWidth', page_width);
            }, 0);
        }
    })

    function loadgrid() {
        //var page_width = $(grid_selector).closest('.tab-pane').width();
        jQuery(grid_selector).jqGrid({
            url: '/Admission/Documentsjqgrid',
            datatype: 'json',
            colNames: ['Document Type', 'Document Name', 'Document Size', 'Uploaded Date', ''],
            colModel: [{ name: 'DocumentType', index: 'DocumentType',width:307, align: 'left', sortable: false },
                       { name: 'DocumentName', index: 'DocumentName',width:307, align: 'left', sortable: false },
                       { name: 'DocumentSize', index: 'DocumentSize',width:307, align: 'left', sortable: false },
                       { name: 'UploadedDate', index: 'UploadedDate',width:307, align: 'left', sortable: false },
                       { name: 'Id', index: 'Id', align: 'left', sortable: false, hidden: true, key: true }
            ],
            viewrecords: true,
            //width: page_width,
            rowNum: 7,
            rowList: [10, 20, 30],
            pager: pager_selector,
            altRows: true,
            multiselect: true,
            multiboxonly: true,
            loadComplete: function () {
                $(window).triggerHandler('resize.jqGrid');
                var table = this;
                setTimeout(function () {
                    styleCheckbox(table);
                    updateActionIcons(table);
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
            },
            caption: "Documents Details"
        });
    }
    window.onload = loadgrid();
    
    $(window).triggerHandler('resize.jqGrid');

   


    //trigger window resize to make the grid get the correct size //
    
    $("#doctype").change(function () {

        if (document.getElementById("doctype").value == "Pickup Card") {

            document.getElementById("nam0").style.visibility = "visible";
            document.getElementById("phn0").style.visibility = "visible";
            document.getElementById("nam1").style.visibility = "visible";
            document.getElementById("phn1").style.visibility = "visible";
        }
        else {

            document.getElementById("nam0").style.visibility = "hidden";
            document.getElementById("phn0").style.visibility = "hidden";
            document.getElementById("nam1").style.visibility = "hidden";
            document.getElementById("phn1").style.visibility = "hidden";
        }
    });

    

    
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
{ //navbar options
    edit: false,
    editicon: 'ace-icon fa fa-pencil blue',
    add: false,
    addicon: 'ace-icon fa fa-plus-circle purple',
    del: true,
    delicon: 'ace-icon fa fa-trash-ored',
    search: false,
    searchicon: 'ace-icon fa fa-search orange',
    refresh: false,
    refreshicon: 'ace-icon fa fa-refresh green',
    view: false, viewicon: 'ace-icon fa fa-search-plus grey'
}, {}, {},
{ url: '/Admission/DeleteUploadedFiles/' }, {},
{})
});

function uploadvalidate() {
    if (document.getElementById("doctype").value == "") {
        ErrMsg("Please Select Document Type!");
        return false;
    }
    else if (document.getElementById("doctype").value == "Student Photo") {
        if (load_image() == true) {
            return true;
            //   alert('loop1');
        }
        else {
            ErrMsg('Please Upload a Valid Image!');
            return false;
        }
    }
    else if (document.getElementById("file1").value == "") {
        ErrMsg("Please Upload a Document!");
        return false;
    }
    else {
        //    alert('loop3');
        return true;
    }

}

function aceSwitch(cellvalue, options, cell) {
    setTimeout(function () {
        $(cell).find('input[type=checkbox]').addClass('ace ace-switch ace-switch-5').after('<span class="lbl"></span>');
    }, 0);
} //enable datepicker
function pickDate(cellvalue, options, cell) {
    setTimeout(function () {
        $(cell).find('input[type=text]').datepicker({
            format: 'yyyy-mm-dd', autoclose:
true
        });
    }, 0);
}

function uploaddat(id1, type, PreRegNum) {

    if (type == "BonafidePDF") {
        LoadPopupDynamicaly("/Admission/AddressDetailsPopUp?Id=" + id1 + '&type=' + type + '&PreRegNum=' + PreRegNum, $('#AddressDetailsDiv'),
        function () {

        });
    }
    else
        window.location.href = "/Admission/uploaddisplay?Id=" + id1;
    //processBusy.dialog('close');
}
function load_image() {
    var imgpath = document.getElementById('file1').value;
    if (imgpath != "") {
        // code to get File Extension..
        var arr1 = new Array;
        arr1 = imgpath.split("\\");
        var len = arr1.length;
        var img1 = arr1[len - 1];
        var filext = img1.substring(img1.lastIndexOf(".") + 1);
        // Checking Extension
        if (filext == "jpg" || filext == "jpeg" || filext == "gif" || filext == "bmp" || filext == "png" || filext == "JPG" || filext == "JPEG" || filext == "BMP" || filext == "GIF" || filext == "PNG")
            return true
        else {
            ErrMsg("Invalid Image File Selected");
            return false;
        }
    }
}
