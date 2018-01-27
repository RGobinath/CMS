$(function () {
    $("#ddlBranchCode").change(function () {
        gradeddl();
    });
    gradeddl();
    sectionddl();
    $('#file2').ace_file_input();
    //Validation Date Picker
    var startDate = new Date();
    var FromEndDate = new Date();
    var ToEndDate = new Date();

    ToEndDate.setDate(ToEndDate.getDate() + 365);

    $('.txtPublishDate').datepicker({
        format: "dd-mm-yyyy",
        weekStart: 1,
        startDate: startDate,
        autoclose: true
    }).on('changeDate', function (selected) {
        startDate = new Date(selected.date.valueOf());
        startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
        $('.txtExpireDate').datepicker('setStartDate', startDate);
    });
    $('.txtExpireDate').datepicker({
        format: "dd-mm-yyyy",
        weekStart: 1,
        startDate: startDate,
        endDate: ToEndDate,
        autoclose: true
    }).on('changeDate', function (selected) {
        FromEndDate = new Date(selected.date.valueOf());
        FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
        $('.txtPublishDate').datepicker('setEndDate', FromEndDate);
    });
    $('#ddlgrade').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '300',
        numberDisplayed: 4,
        includeSelectAllDivider: true
    });
    $('#ddlsection').multiselect({
        includeSelectAllOption: true,
        selectAllText: ' Select all',
        enableCaseInsensitiveFiltering: true,
        enableFiltering: true,
        maxHeight: '300',
        numberDisplayed: 4,
        includeSelectAllDivider: true
    });
    $.ajax({
        url: "/Notify/ViewNotificationAttachments?NotePreId=" + $('#NotePreId').val(),
        mtype: 'GET',
        async: false,
        datatype: 'json',
        success: function (data) {

            $('#ViewNoteAtt').html(data);

        }

    });


    $('#cancel').click(function () {
        var url = $("#BackUrl").val();
        window.location.href = url;
    });



    var grid_selector = "#NotificationList";
    var pager_selector = "#NotificationListPager";

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



    if ($("#SuccessNotifyMsg").val() != null & $("#SuccessNotifyMsg").val() != "") {
        InfoMsg("Notification Created Successfully.", function () { $("#SuccessNotifyMsg").val(""); });
    }

    $("#ddlNoteTypeEdit").val();



    $("#file2").click(function () {

        var txtTopic = $("#txtTopic").val();
        var txtPublishDate = $("#txtPublishDate").val();
        var txtExpireDate = $("#txtExpireDate").val();
        var ddlPublishTo = $("#ddlPublishTo").val();

        if (txtTopic == "" || txtTopic == null || txtPublishDate == "" || txtPublishDate == null || txtExpireDate == "" || txtExpireDate == null || ddlPublishTo == "" || ddlPublishTo == null) {
            ErrMsg("Please Fill Required Notification Details.");
            return false;
        }
    });

    $("#Submit").click(function () {

        var txtTopic = $("#txtTopic").val();
        var txtPublishDate = $("#txtPublishDate").val();
        var txtExpireDate = $("#txtExpireDate").val();
        var ddlPublishTo = $("#ddlPublishTo").val();

        if (txtTopic == "" || txtTopic == null || txtPublishDate == "" || txtPublishDate == null || txtExpireDate == "" || txtExpireDate == null || ddlPublishTo == "" || ddlPublishTo == null) {
            ErrMsg("Please Fill Required Notification Details.");
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
             if (itemdata.Text == $('#Campus').val()) {
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
        url: "/Notify/NotificationListJqGrid",
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Topic', 'Message', 'PublishDate', 'ExpireDate', 'PublishTo', 'NoteType', 'Campus', 'NewIds List', 'Status', 'Valid', 'NotePreId', 'PublishedOn'],
        colModel: [
        // if any column added in future have to check rowObject for SLA status image.... 
              {name: 'Id', index: 'Id', width: 50, editable: true, hidden: true, edittype: 'text', key: true, sortable: false },
              { name: 'Topic', index: 'Topic', width: 100, editable: true, editoptions: { dataInit: function (el) { $(el).addClass('CSSTextBox'); } } },
              { name: 'Message', index: 'Message', width: 250, sortable: true, editable: true, edittype: 'textarea', editoptions: { dataInit: function (el) {
                  $(el).addClass('CSSTextArea');
              }
              }
              },
              { name: 'PublishDate', index: 'PublishDate', width: 50, sortable: true, editable: true, editoptions: { dataInit: function (elem) {
                  $(elem).datepicker({ dateFormat: 'dd-m-yy', changeMonth: true, changeYear: true, minDate: 0, onSelect: function (selected) { $("#EndDate").datepicker("option", "minDate", selected) } });
                  $(elem).addClass('datepicker1');
              }
              }
              },
              { name: 'ExpireDate', index: 'ExpireDate', width: 50, sortable: true, editable: true, editoptions: { dataInit: function (elem) {
                  $(elem).datepicker({ dateFormat: 'dd-m-yy', changeMonth: true, changeYear: true, minDate: 0, onSelect: function (selected) { $("#EndDate").datepicker("option", "minDate", selected) } });
                  $(elem).addClass('datepicker1');
              }
              }
              },
              { name: 'PublishTo', index: 'PublishTo', width: 50, sortable: true, editable: true, edittype: 'select', editoptions: { dataInit: function (el) { $(el).addClass('CSSTextBox'); }, dataUrl: '/Notify/publishtoddl',
                  buildSelect: function (data) {
                      jqGridPublishTo = jQuery.parseJSON(data);
                      var s = '<select>';
                      if (jqGridPublishTo && jqGridPublishTo.length) {
                          for (var i = 0, l = jqGridPublishTo.length; i < l; i++) {
                              var mg = jqGridPublishTo[i];
                              s += '<option value="' + mg + '">' + mg + '</option>';
                          }
                      }
                      return s + "</select>";
                  }
              }
              },
              { name: 'NoteType', index: 'NoteType', width: 50, sortable: true, editable: true, edittype: 'select', editoptions: { dataInit: function (el) { $(el).addClass('CSSTextBox'); }, dataUrl: '/Notify/notetypeddl', buildSelect: function (data) {

                  jqGridNoteType = jQuery.parseJSON(data);
                  var sn = '<select id="ddlNT">';
                  if (jqGridNoteType && jqGridNoteType.length) {
                      for (var i = 0, l = jqGridNoteType.length; i < l; i++) {
                          var mgnt = jqGridNoteType[i];
                          sn += '<option value="' + mgnt + '">' + mgnt + '</option>';
                      }
                  }
                  return sn + "</select>";
              }
              }
              },
              { name: 'Campus', index: 'Campus', width: 50, sortable: true, editable: false },
              { name: 'NewIds', index: 'NewIds', width: 100, sortable: true, editable: true, edittype: 'textarea', editoptions: { dataInit: function (el) { $(el).addClass('CSSTextArea'); } } },
              { name: 'Status', index: 'Status', width: 50, editable: true, hidden: true, edittype: 'text', sortable: false },
              { name: 'Valid', index: 'Valid', width: 50, editable: true, hidden: true, edittype: 'text', sortable: false },
              { name: 'NotePreId', index: 'NotePreId', width: 50, editable: true, hidden: true, edittype: 'text', sortable: false },
              { name: 'PublishedOn', index: 'PublishedOn', width: 50, editable: true, hidden: true, edittype: 'text', sortable: false }

              ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '145',
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="fa fa-list-alt"></i>&nbsp;Notification List',
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

    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: true,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {},
    //{ url: '/Notify/EditNotification/', left: '10%', top: '10%', height: '50%', width: 400, labelswidth: 60, closeAfterEdit: true, beforeSubmit: validate_edit, closeOnEscape: true, reloadAfterSubmit: true },        //edit options
            {},
            { url: '/Notify/DeleteNotification/' });



});



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

function validate_edit(posdata, obj) {


    if (posdata.NoteType == "GradeLevel" || posdata.NoteType == "Individual") {
        if (posdata.NewIds == null || posdata.NewIds == "" || posdata.NewIds == undefined)
            return [false, "Please fill required Student Id"];
    }

    return [true, ""];
}

var splitstr = "";

function resethtml2() {
    //    alert('hre');
    $('#clear2').html($('#clear2').html());
    var div = document.getElementById('Attachfiles2');
    div.innerHTML = 'Attached Files &nbsp;&nbsp;&nbsp;  ';
    $.ajax({
        url: '/Notify/DeleteAttachment/',
        type: 'POST',
        dataType: 'json',
        traditional: true
    });
}

function uploaddoc2() {
    if (document.getElementById("file2").value == "") {
        ErrMsg("Please Browse a Document");
    }
    else {
        splitstr = splitstr + $('#file2').val().split('\\');


        $.ajaxFileUpload({

            url: "/Notify/MailAttachments2?NotePreId=" + $('#NotePreId').val(),
            secureuri: false,
            fileElementId: 'file2',
            dataType: 'json',
            success: function (data, success) {
                var div = document.getElementById('Attachfiles2');

                if ((div.innerHTML == 'Attached Files &nbsp;&nbsp;&nbsp;  ')) {
                    div.innerHTML = div.innerHTML + data.result;
                }
                else {
                    div.innerHTML = div.innerHTML + ', ' + data.result;
                }
            }
        });
        $('#clear2').html($('#clear2').html());
    }
}


function uploaddat(id1) {
    window.location.href = "/Notify/displayAtt?Id=" + id1;
    processBusy.dialog('close');
}
//function gradeddl() {
//    //var e = document.getElementById('ddlBranchCode');
//    //var campus = e.options[e.selectedIndex].value;
//    var campus = $("#Campus").val();
//    if (campus != null && campus != "") {
//        $.ajax({
//            type: 'POST',
//            async: false,
//            dataType: "json",
//            url: '/Communication/FillGrades?campus=' + campus,
//            success: function (data) {
//                $("#ddlgrade").empty();
//                //            $("#ddlgrade").dropdownchecklist('destroy');
//                if (data != null) {
//                    //                $("#ddlgrade").append("<option value=' '> All </option>");
//                    for (var k = 0; k < data.length; k++) {
//                        $("#ddlgrade").append("<option value='" + data[k].Value + "'>" + data[k].Text + "</option>");
//                    }
//                }
//                $('#ddlgrade').multiselect('rebuild');
//                //Refreshddl();
//            }
//        });
//    }

//}
function gradeddl() {    
    var campus = $("#Campus").val();    
    $.ajax({
        type: 'POST',
        async: false,
        dataType: "json",
        url: '/Communication/FillGrades?campus=' + campus,
        success: function (data) {
            $("#ddlgrade").empty();
            $("#ddlgrade").multiselect('destroy');
            if (data != null) {                
                for (var k = 0; k < data.length; k++) {
                    $("#ddlgrade").append("<option value='" + data[k].Value + "'>" + data[k].Text + "</option>");
                    //$("#ddlGrades option:selected").prop("selected", false);
                }
                var grade = $('#Grade').val();
                var GradeArr = grade.split(',');
                if (GradeArr != undefined & GradeArr != null) {
                    for (var j = 0; j < GradeArr.length; j++) {
                        $('#ddlgrade option').filter(function () { return $(this).text() == '' + GradeArr[j] + '' }).attr('selected', 'selected').prop('checked', true);
                    }
                }
            }
            Refreshgradeddl();
        }
    });

}
function Refreshgradeddl() {
    $("#ddlgrade").multiselect({
        firstItemChecksAll: true,
        maxDropHeight: 150,
        Height: 80,
        width: 155
    });
    $('#ddlgrade').multiselect('rebuild');
}
function sectionddl() {
    $("#ddlsection").append("<option value='A'>A</option>");
    $("#ddlsection").append("<option value='B'>B</option>");
    $("#ddlsection").append("<option value='C'>C</option>");
    $("#ddlsection").append("<option value='D'>D</option>");
    $("#ddlsection").append("<option value='E'>E</option>");
    $("#ddlsection").append("<option value='F'>F</option>");
    if ($('#Section').val() != "" & $('#Section').val() != null) {
        var sec = $('#Section').val();
        var secArr = sec.split(',');
        if (secArr != "") {
            for (var j = 0; j < secArr.length; j++) {
                $('#ddlsection option').filter(function () { return $(this).text() == '' + secArr[j] + '' }).attr('selected', 'selected').prop('checked', true);
            }
        }
    }
    Refreshsectionddl();
}
function Refreshsectionddl() {
    $("#ddlsection").multiselect({
        firstItemChecksAll: true,
        maxDropHeight: 150,
        Height: 80,
        width: 150
    });
    $('#ddlsection').multiselect('rebuild');
}
function delatt(delid) {

    $.ajax({
        url: '/Notify/DeleteAtt?delid=' + delid,
        type: 'POST',
        dataType: 'json',
        traditional: true,
        success: function (data) {
            window.location.href = "/Notify/ViewNotifications?npreid=" + $('#NotePreId').val();

        }
    });
}



bkLib.onDomLoaded(function () {

    nicEditors.allTextAreas({ maxHeight: 70, fullPanel: true });

});