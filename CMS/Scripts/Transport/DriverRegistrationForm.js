jQuery(function ($) {
    $("#Back").click(function () {
        window.location.href = '/Transport/NewDriverManagement';
    });

    $("#Back1").click(function () {
        window.location.href = '/Transport/DriverManagement';
    });
    var grid_selector = "#DocumentList";
    var pager_selector = "#DocumentListPager";
    
    $(window).on('resize.jqGrid', function () {
        var page_width = $(".tab-content").width();
        page_width = page_width - 300;
        $(grid_selector).jqGrid('setGridWidth', page_width);
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

    $('#btnUploadDoc').ace_file_input(); //For Upload Button Color

    $("#CaptureImg").click(function () {
        $.ajax({
            type: 'POST',
            async: false,
            url: '/Base/IsPhotoUploadedFor?RefNum=' + $('#DriverRegNo').val() + '&docType=Driver Photo&docFor=Driver',
            success: function (data) {
                if (data == "Success") {
                    ModifiedLoadPopupDynamicaly("/Transport/PhotoCapture?PreRegNo=" + $("#DriverRegNo").val(), $('#ImgCaptureDiv'), function () { }, "", 800, 450, "Capture Image");
                }
                else
                    ErrMsg(data + " Photo was uploaded already!");
            }

        });
        
    });
    // written by felix kinoiya
    $("#ddlcampus").change(function () {
        $.getJSON("/Base/DesignationByCampus?campus=" + $("#ddlcampus").val(),
          function (fillbc) {
              var designation = $("#designation");
              designation.empty();
              designation.append($('<option/>', { value: "", text: "Select One" }));

              $.each(fillbc, function (index, itemdata) {
                  designation.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });

        $.getJSON("/Base/staffTypeByCampus?campus=" + $("#ddlcampus").val(),
          function (fillbc) {
              var StaffType = $("#stafftype");
              StaffType.empty();
              StaffType.append($('<option/>', { value: "", text: "Select One" }));

              $.each(fillbc, function (index, itemdata) {
                  StaffType.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });

          });

        $.getJSON("/Base/DepartmentByCampus?campus=" + $("#ddlcampus").val(),
          function (fillbc) {
              var department = $("#department");
              department.empty();
              department.append($('<option/>', { value: "", text: "Select One" }));

              $.each(fillbc, function (index, itemdata) {
                  department.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });

          });

    });
    // end

    debugger;
    jQuery(grid_selector).jqGrid({
        
        url: '/Transport/DriverDocumentsjqgrid?DriverRegNo=' + $('#DriverRegNo').val(),
        datatype: 'json',
        height: 100,
        colNames: ['Document Type', 'Document Name', 'Document Size', 'Uploaded Date', ''],
        colModel: [
                    { name: 'DocumentType', index: 'DocumentType', align: 'left', sortable: false },
                    { name: 'DocumentName', index: 'DocumentName', align: 'left', sortable: false },
                    { name: 'DocumentSize', index: 'DocumentSize', align: 'left', sortable: false },
                    { name: 'UploadedDate', index: 'UploadedDate', align: 'left', sortable: false },
                    { name: 'Id', index: 'Id', align: 'left', sortable: false, hidden: true, key: true }
                    ],
        viewrecords: true,
        rowNum: 10,
        rowList: [25, 50, 100, 500],
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
        }
        //caption: "<i class='ace-icon fa fa-documents'></i>&nbsp;Document Grid"

    });




    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, {}, {}, {}, {})

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
    $('#btnAddDocuments').click(function () {
        debugger;
        if (document.getElementById("ddldocumenttype").value == "") {
            ErrMsg("Please Select Document Type!");
            return false;
        }
        else if (document.getElementById('uploadedFile').value == "") {
            ErrMsg("Please Select A File!");
            return false;
        }
        else {
            ajaxUploadDocs();
            return false;
        }
    });
 

    function ajaxUploadDocs() {
        debugger;
        var e = document.getElementById('ddldocumenttype');
        var doctype = e.options[e.selectedIndex].value;
        var files = document.getElementById('uploadedFile').files;
        var regno = $('#DriverRegNo').val();
        alert(regno);
        $.ajaxFileUpload({
            url: '/Common/UploadDocuments?docType=' + doctype + '&documentFor=Driver&RegNo='+regno+'',
            secureuri: false,
            fileElementId: 'uploadedFile',
            dataType: 'json',
            success: function (data, status) {
                jQuery("#DocumentList").setGridParam({ url: '/Transport/DriverDocumentsjqgrid' }).trigger("reloadGrid");
            },
            error: function (data, status, e) {
            }
        });
    }
    $('#ddldocumenttype').change(function () {
        if (document.getElementById("ddldocumenttype").value == "Staff Photo") {
            document.getElementById("doctypenote").innerHTML = '*Please upload your passport photo';
        }
        else { document.getElementById("doctypenote").innerHTML = ''; }
    });

    $('#sname').keyup(function (event) {
        if (event.keyCode == 13) {
            var message = $('#sname').val();
        } else {
            return true;
        }
    });

//    function ajaxUploadDocs() {
//        var e = document.getElementById('ddldocumenttype');
//        var doctype = e.options[e.selectedIndex].value;
//        var files = document.getElementById('uploadedFile').files;

//        $.ajaxFileUpload({
//            url: '/StaffManagement/UploadDocuments?docType=' + doctype + '',
//            secureuri: false,
//            fileElementId: 'uploadedFile',
//            dataType: 'json',
//            success: function (data, status) {
//                jQuery("#DocumentList").setGridParam({ url: '/StaffManagement/Documentsjqgrid' }).trigger("reloadGrid");
//            },
//            error: function (data, status, e) {
//            }
//        });
//    }
    var buttons = $('.Wrkdetls');
    buttons.find('button').button();
    buttons.find('a').button({ icons: { primary: "ui-icon-search" },
        text: false
    });

//    $("#stfsrch").click(function () {

//        LoadPopupDynamicaly("/StaffManagement/StaffDetailsSearch", $('#DivStaffSearch'),
//            function () {
//                LoadSetGridParam1($('#StaffList2'), "/StaffManagement/StaffListPopupJqGrid");
//            });
//    });

//    var clbPupGrdSel = null;
//    function LoadPopupDynamicaly(dynURL, ModalId, loadCalBack, onSelcalbck, width) {
//        //alert('popup');
//        if (width == undefined) { width = 597; }
//        if (ModalId.html().length == 0) {
//            $.ajax({
//                url: dynURL,
//                type: 'GET',
//                async: false,
//                dataType: 'html', // <-- to expect an html response
//                success: function (data) {
//                    ModalId.dialog({
//                        height: 553,
//                        width: width,
//                        modal: true,
//                        title: 'Staff List',
//                        buttons: {}
//                    });
//                    ModalId.html(data);
//                    //    alert(data);
//                }
//            });
//            // alert('popup');
//        }

//        clbPupGrdSel = onSelcalbck;
//        ModalId.dialog('open');
//        CallBackFunction(loadCalBack);

//    }
    var date = new Date();
    var currentMonth = date.getMonth();
    var currentDate = date.getDate();
    var currentYear = date.getFullYear();

    $('#DOB').datepicker({
        format: 'dd/mm/yyyy',
        autoClose: true
    }).on('changeDate', function (selected) {
        var selDate = new Date(selected.date.valueOf());
        var today = new Date();
        years = Math.floor((today.getTime() - selDate.getTime()) / (365.25 * 24 * 60 * 60 * 1000));
        $("#age").val(years);
    });
    $('#DOB').blur(function () {
        getAge(document.getElementById('DOB').value);
    });

});
function LoadSetGridParam1(GridId, brUrl) {
    GridId.setGridParam({
        url: brUrl,
        datatype: 'json',
        mtype: 'GET'
    }).trigger("reloadGrid");
}
function getAge(dateString) {
    var today = new Date();
    var bD = dateString.split('/');
    if (bD.length == 3) {
        born = new Date(bD[2], bD[1] * 1 - 1, bD[0]);
        years = Math.floor((today.getTime() - born.getTime()) / (365.25 * 24 * 60 * 60 * 1000));
        $("#age").val(years);
    }
}
function validate1() {
    var docrecs = parseInt($("#DocumentList").getGridParam("records"), 10);
    var qualrecs = parseInt($("#QualificationList").getGridParam("records"), 10);
    document.getElementById("doccheck").value = "";
    document.getElementById("qualcheck").value = "";
    if (document.getElementById("sname").value == "") {
        ErrMsg("Please Enter Name!");
        return false;
    }
    else if (document.getElementById("ddlcampus").value == "") {
        ErrMsg("Please Select Campus!");
        return false;
    }
    else if ((document.getElementById("dateofjoin").value == "")|| (document.getElementById("dateofjoin").value =="01/01/0001 00:00:00")) {
        ErrMsg("Please Select Date of join!");
        return false;
    }
    else if (document.getElementById("Status").value == "") {
        ErrMsg("Please Select Status!");
        return false;
    }
        //else if (document.getElementById("relationtype").value == "") {

        //    ErrMsg("Please Enter Family Details!");
        //    return false;
        //}
        //else if (document.getElementById("FMName").value == "") {

        //    ErrMsg("Please Enter Family Details!");
        //    return false;
        //}
        //else if (document.getElementById("FMDOB").value == "") {

        //    ErrMsg("Please Enter Family Details!");
        //    return false;
        //}
        //else if (document.getElementById("FMoccupation").value == "") {

        //    ErrMsg("Please Enter Family Details!");
        //    return false;
        //}
    else if (document.getElementById("dateofjoin").value == "") {

        ErrMsg("Please Enter Date of join!");
        return false;
    }

        //else if (isNaN(docrecs) || docrecs == 0) {
        //    //ErrMsg("Please Upload Documents!");
        //    $("#tabs").tabs({ selected: 0 });
        //    document.getElementById("doccheck").value = "yes";
        //    return true;
        //}
        //else if (isNaN(qualrecs) || qualrecs == 0) {
        //    //  ErrMsg("Please Enter Qualification Details!");
        //    document.getElementById("qualcheck").value = "yes";
        //    return true;
        //    $("#tabs").tabs({ selected: 1 });
        //}
    else { return true; }
}

function validate2() {
    if (document.getElementById("Status").value == "") {
        ErrMsg("Please Select Status!");
        return false;
    }
    else {
        return true;
    }
}