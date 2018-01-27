function FillGradeByCampus() {
    debugger;
    var Campus = [];
    Campus = $("#ddlBranchCode").val();
    var ddlbc = $("#ddlGrade");
    if (Campus != null && Campus != "") {
        $.getJSON("/Base/FillGradesWithArrayCriteria?campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select One" }));
              ddlbc.append($('<option/>', { value: "All", text: "All" }));
              $.each(fillbc, function (index, itemdata) {
                  if (itemdata.Text == '@Model.Grade') {
                      ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
                  }
                  else {
                      ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                  }
              });
          });
    }
    if (Campus == "" || Campus == null) {
        ddlbc.empty;
        ddlbc.append($('<option/>',
    {
        value: "",
        text: "Select One"

    }));
    }
}
$(function () {

    //$('#File').ace_file_input();

    window.onbeforeunload = function (e) { processBusy.dialog('open'); $(".ui-dialog-titlebar").hide(); };

    $(".ui-dialog-titlebar").hide();

    if ($("#SuccessPGMsg").val() != null & $("#SuccessPGMsg").val() != "") {
        InfoMsg("PhotoGallery Album Created Successfully.", function () { $("#SuccessPGMsg").val(""); });
    }

    $("#File").click(function () {

        var ddlCampus = $("#ddlBranchCode").val();
        var txtAlbumName = $("#txtAlbumName").val();
        var ddlPublishedTo = $("#ddlPublishedTo").val();

        if (ddlCampus == "" || ddlCampus == null || txtAlbumName == "" || txtAlbumName == null || ddlPublishedTo == "" || ddlPublishedTo == null) {
            ErrMsg("Please Fill Required Details.");
            return false;
        }
    });


    $.getJSON("/Notify/FillCampusList",
     function (fillbc) {
         var ddlbc = $("#ddlBranchCode");
         ddlbc.empty();
         ddlbc.append($('<option/>',
        {
            value: "",
            text: "Select One"

        }));
         $.each(fillbc, function (index, itemdata) {
             if (itemdata.Text == '@Model.Campus') {
                 ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
             }
             else {
                 ddlbc.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
             }
         });
     });    
    $("#Submit").click(function () {

        var ddlCampus = $("#ddlBranchCode").val();
        var ddlGrade = $("#ddlGrade").val();
        var txtAlbumName = $("#txtAlbumName").val();
        var ddlPublishedTo = $("#ddlPublishedTo").val();

        if (ddlCampus == "" || ddlCampus == null || txtAlbumName == "" || txtAlbumName == null || ddlPublishedTo == "" || ddlPublishedTo == null || ddlGrade == "" || ddlGrade == null) {

            ErrMsg("Please Fill Required Details.");
            return false;
        }

    });



    var grid_selector = "#PhotoGalleryList";
    var pager_selector = "#PhotoGalleryListPager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
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



    //Pager icons
   

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


    jQuery(grid_selector).jqGrid({
        url: "/Notify/PhotoGalleryListJqGrid",
        datatype: 'json',
        mtype: 'GET',
       
        colNames: ['Id', 'Campus', 'Grade','Album Name', 'Description', 'Publish To', 'Performer', 'Created On', 'Status','Is Active'],
        colModel: [
        // if any column added in future have to check rowObject for SLA status image.... 
              {name: 'Id', index: 'Id', hidden: true, editable: true, key: true },
              { name: 'Campus', index: 'Campus', editable: false, sortable: true },
              { name: 'Grade', index: 'Grade', editable: false, sortable: true },
              { name: 'AlbumName', index: 'AlbumName', editable: false, sortable: true },
              { name: 'Description', index: 'Description', editable: false, sortable: true },
              { name: 'PublishedTo', index: 'PublishedTo',  editable: false, sortable: true },
              { name: 'Performer', index: 'Performer',  editable: false, sortable: true },
              { name: 'CreatedOn', index: 'CreatedOn', editable: false, sortable: true },
              { name: 'Status', index: 'Status', hidden: true, editable: false, sortable: true },
              { name: 'IsActive', index: 'IsActive', hidden: false, editable: true, sortable: true, width: 50, edittype: 'select', editoptions: { sopt: ['eq'], value: { '': '--Select--', 'true': 'Yes', 'false': 'No' }, style: "width: 169px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, stype: 'select', search: true  }
              ],

        rowNum: 10,
        rowList: [5, 10, 20, 50, 100, 150, 200],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Desc',
        height: '220',
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="fa fa-picture-o"></i>&nbsp;Photo Gallery List',
        forceFit: true,
        multiselect: true,
        loadComplete: function () {
            var table = this;

            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        ondblClickRow: function (rowid, iRow, iCol, e) {
            $(grid_selector).editGridRow(rowid, prmGridDialog);
        },
        loadError: function (xhr, status, error) {
            $(grid_selector).clearGridData();
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }

    });
    jQuery(grid_selector).navGrid(pager_selector, { add: false, edit: true, del: true, search: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green' },

            { url: '/Notify/EditPhotoGallery/',closeAfterEdit:true },        //edit options
            {},
            { url: '/Notify/DeletePhotoGallery/' });

    $("#ddlBranchCode").change(function () {
        FillGradeByCampus();
    });
});


var splitstr = "";

function resethtml2() {
    //    alert('hre');
    $('#clear2').html($('#clear2').html());
    var div = document.getElementById('Uploadedfiles');
    div.innerHTML = 'Uploaded Files : &nbsp;&nbsp;&nbsp;  ';
    $.ajax({
        url: "/Notify/PhotoGalleryDeleteUploadedFiles/",
        type: 'POST',
        dataType: 'json',
        traditional: true
    });
}

var totFsize = 0;

function Checkfiles() {

    var inp = document.getElementById('File');

    var imgCount = inp.files.length;

    var cnt = 0;

    for (var i = 0; i < inp.files.length; i++) {

        var fname = inp.files.item(i).name;
        // alert("here is a file name: " + fname);

        if (fname != "") {
            // code to get File Extension..
            var arr1 = new Array;
            arr1 = fname.split("\\");
            var len = arr1.length;
            var img1 = arr1[len - 1];
            var filext = img1.substring(img1.lastIndexOf(".") + 1);
            // Checking Extension
            if (filext == "jpg" || filext == "jpeg" || filext == "gif" || filext == "bmp" || filext == "png" || filext == "JPG" || filext == "JPEG" || filext == "BMP" || filext == "GIF" || filext == "PNG") {
                cnt++;
            }
            else {
                ErrMsg("Invalid Image File Selected");
                return false;
            }
        }

    }

    if (imgCount == cnt) {
        return true;
    }
}

function uploaddoc2() {
    debugger;
    if (document.getElementById("File").value == "") {
        ErrMsg("Please Browse a Photo");
    }
    else if (Checkfiles() == false) {
        ErrMsg("Invalid Image File Selected");
    }
    else {
        splitstr = splitstr + $('#File').val().split('\\');
        var files = document.getElementById("File").files;
        for (var i = 0; i < files.length; i++) {
            totFsize = totFsize + parseInt(files[i].size);
        }
        // 1048576 -1 MB, 5242880 - 5MB , 4194304 - 4MB 
        if (totFsize < Number(4194304)) {
            $.ajaxFileUpload({
                url: '/Notify/PhotoGalleryFilesUpload?PGPreId=' + $('#PGPreId').val(),
                secureuri: false,
                fileElementId: 'File',
                dataType: 'json',
                success: function (data, success) {
                    var div = document.getElementById('Uploadedfiles');
                    if ((div.innerHTML == 'Uploaded Files &nbsp; ')) {
                        div.innerHTML = div.innerHTML + data.result;
                    }
                    else {
                        div.innerHTML = div.innerHTML + ', ' + data.result;
                    }
                }
            });
            $('#clear2').html($('#clear2').html());
            totFsize = 0;
        }
        else {
            ErrMsg("file size must be less than 4 MB");
            $('#clear2').html($('#clear2').html());
            totFsize = 0;
        }

    }
}
