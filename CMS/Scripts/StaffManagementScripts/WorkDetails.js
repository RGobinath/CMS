jQuery(function ($) {
    var grid_selector = "#DocumentList";
    var pager_selector = "#DocumentListPager";
    StaffGroupByCampus();    
    if ($('#ForPage').val() == "StaffProfile") {
        debugger;
        $(window).on('resize.jqGrid', function () {
            var page_width = $(".page-content").width();
            page_width = page_width - 90;
            $(grid_selector).jqGrid('setGridWidth', page_width);
        })
    }
    else {
        $(window).on('resize.jqGrid', function () {
            var page_width = $(".page-content").width();
            page_width = page_width - 380;
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
    $("#CaptureImg").click(function () {
        $.ajax({
            type: 'POST',
            async: false,
            url: '/Base/IsPhotoUploadedFor?RefNum=' + $('#PreRegNum').val() + '&docType=Staff Photo&docFor=Staff',
            success: function (data) {
                debugger;
                if (data == "Success") {
                    ModifiedLoadPopupDynamicaly("/StaffManagement/StaffCaptureImage?PreRegNo=" + $("#PreRegNum").val(), $('#ImgCaptureDiv'), function () { }, "", 800, 450, "Capture Image");
                }
                else
                    ErrMsg(data + " Photo was uploaded already!");
            }

        });

    });
    $('#btnUploadDoc').ace_file_input(); //For Upload Button Color

    // written by felix kinoiya
    $("#ddlcampus").change(function () {
        StaffGroupByCampus();
        FillStaffWorkGroup();
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
    $("#staffgroup").change(function () {
        FillStaffSubGroup();
    });
    $("#stafftype").change(function () {
        FillStaffWorkGroup();
    });
    // end
    FillStaffWorkGroup();

    jQuery(grid_selector).jqGrid({
        url: '/StaffManagement/Documentsjqgrid',
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
        },
        caption: "<i class='ace-icon fa fa-documents'></i>&nbsp;Document Grid"

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

    function ajaxUploadDocs() {
        debugger;
        var e = document.getElementById('ddldocumenttype');
        var doctype = e.options[e.selectedIndex].value;
        var files = document.getElementById('uploadedFile').files;
        var regno = $('#PreRegNum').val();
        $.ajaxFileUpload({
            //url: '/StaffManagement/UploadDocuments?docType=' + doctype + '',
            url: '/Common/UploadDocuments?docType=' + doctype + '&documentFor=Staff&RegNo=' + regno,
            secureuri: false,
            fileElementId: 'uploadedFile',
            dataType: 'json',
            success: function (data, status) {
                jQuery("#DocumentList").setGridParam({ url: '/StaffManagement/Documentsjqgrid' }).trigger("reloadGrid");
            },
            error: function (data, status, e) {
            }
        });
    }
    var buttons = $('.Wrkdetls');
    buttons.find('button').button();
    buttons.find('a').button({
        icons: { primary: "ui-icon-search" },
        text: false
    });

    $("#stfsrch").click(function () {

        LoadPopupDynamicaly("/StaffManagement/StaffDetailsSearch", $('#DivStaffSearch'),
            function () {
                LoadSetGridParam1($('#StaffList2'), "/StaffManagement/StaffListPopupJqGrid");
            });
    });

    var clbPupGrdSel = null;
    function LoadPopupDynamicaly(dynURL, ModalId, loadCalBack, onSelcalbck, width) {
        //alert('popup');
        if (width == undefined) { width = 597; }
        if (ModalId.html().length == 0) {
            $.ajax({
                url: dynURL,
                type: 'GET',
                async: false,
                dataType: 'html', // <-- to expect an html response
                success: function (data) {
                    ModalId.dialog({
                        height: 553,
                        width: width,
                        modal: true,
                        title: 'Staff List',
                        buttons: {}
                    });
                    ModalId.html(data);
                    //    alert(data);
                }
            });
            // alert('popup');
        }

        clbPupGrdSel = onSelcalbck;
        ModalId.dialog('open');
        CallBackFunction(loadCalBack);

    }

    function LoadSetGridParam1(GridId, brUrl) {
        GridId.setGridParam({
            url: brUrl,
            datatype: 'json',
            mtype: 'GET'
        }).trigger("reloadGrid");
    }

});
function StaffGroupByCampus() {    
    var StaffGroup = $("#staffgroup");    
    if ($("#ddlcampus").val() != "") {
        $.getJSON("/Base/StaffGroupByCampus?campus=" + $("#ddlcampus").val(),
                  function (fillbc) {                      
                      StaffGroup.empty();
                      StaffGroup.append($('<option/>', { value: "", text: "Select One" }));                      
                      $.each(fillbc, function (index, itemdata) {                          
                          if (itemdata.Text == $('#StaffGroup').val()) {                              
                              StaffGroup.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");                                                            
                          }
                          else {
                              StaffGroup.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                          }                          
                      });                      
                      FillStaffSubGroup();
                  });        
    }
    else {
        StaffGroup.empty();
        StaffGroup.append($('<option/>', { value: "", text: "Select One" }));
    }
}
function FillStaffSubGroup()
{
    var StaffSubGroup = $("#staffsubgroup");    
    var StaffGroup = $("#staffgroup").val();
    if (StaffGroup == "undefined")
    {
        StaffGroup = $("#StaffGroup").val();
    }    
    if ($("#ddlcampus").val() != "" && $("#staffgroup").val() != "") {
        $.getJSON("/Base/StaffSubGroupByCampusandGroupName?campus=" + $("#ddlcampus").val() + '&GroupName=' + StaffGroup,
              function (fillbc) {                  
                  StaffSubGroup.empty();
                  StaffSubGroup.append($('<option/>', { value: "", text: "Select One" }));
                  $.each(fillbc, function (index, itemdata) {                      
                      if (itemdata.Text == $('#StaffSubGroup').val()) {                          
                          StaffSubGroup.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
                      }
                      else {
                          StaffSubGroup.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                      }
                  });

              });
    }
    else {
        StaffSubGroup.empty();
        StaffSubGroup.append($('<option/>', { value: "", text: "Select One" }));
    }
}
function FillStaffWorkGroup() {
    var Campus = $("#ddlcampus").val();
    var StaffType = $("#stafftype").val();
    $.getJSON("/StaffManagement/FillProgrammeByCampusAndStaffType?Campus=" + Campus + '&StaffType=' + StaffType,
      function (fillbc) {
          var ddlbc = $("#ddlProgramme");
          var hdnCampusId = $("#hdnProgrammeId").val();
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "---Select---" }));

          $.each(fillbc, function (index, itemdata) {
              if (itemdata.Value == hdnCampusId)
                  ddlbc.append("<option value='" + itemdata.Value + "'selected='selected' >" + itemdata.Text + "</option>");
              else
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}