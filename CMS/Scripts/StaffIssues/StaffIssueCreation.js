jQuery(function ($) {
    FillCampusDll();
    $.getJSON("/StaffIssues/FillStaffIssueGroup",
     function (fillig) {

         var ddlig = $("#ddlIssueGroup");
         ddlig.empty();
         ddlig.append($('<option/>', { value: "", text: "Select One" }));

         $.each(fillig, function (index, itemdata) {
             if (itemdata.Text == $('#IssueGroup').val()) {
                 ddlig.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
                 GetIssueType();
             }
             else {
                 ddlig.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
             }
         });

     });


    $("#btnBack").click(function () {
        window.location.href = '/StaffIssues/StaffIssueManagement';
    });

    var Id = $("#Id").val();
    if (Id > 0) { GetResolverUserID();}
    $("#Save").click(function () {
        ValidateIssueDescription();
        var Campus = $("#ddlBranchCode").val();
        if (ValidateIssueDescription() == false) {
            ErrMsg("Issue Description should not exceed 4000 characters.");
            return false;
        }
        if (Campus == "") {
            ErrMsg("Campus is Mandatory.");
            return false;
        }
    });

    $("#CompleteLogIssue").click(function () {
        if (ValidateIssueDescription() == false) {
            ErrMsg("Issue Description should not exceed 4000 characters.");
            return false;
        }
        var issgrp = $("#ddlIssueGroup").val();
        var isstyp = $("#ddlIssueType").val();
        var issdesc = $("#txtDescription").val();
        var Campus = $("#ddlBranchCode").val();
        if (Campus == "") {
            ErrMsg("Campus is Mandatory.");
            return false;
        }
        if (issgrp == "") {
            ErrMsg("Issue Group is Mandatory.");
            return false;
        }
        else if (isstyp == "") {
            ErrMsg("Issue Type is Mandatory.");
            return false;
        }
        else if (issdesc == "") {
            ErrMsg("Issue Description is Mandatory.");
            return false;
        }
        else {
            return true;
        }
    });

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#CommentList").jqGrid('setGridWidth', $(".col-sm-8").width());
        $("#Uploadedfileslist").jqGrid('setGridWidth', $(".col-sm-8").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $("#CommentList").closest('[class*="col-"]');
    var parent_column = $("#Uploadedfileslist").closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#CommentList").jqGrid('setGridWidth', parent_column.width());
                $("#Uploadedfileslist").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })


    $("#CommentList").jqGrid({
        url: '/StaffIssues/DescriptionForSelectedIdJqGrid?Id=' + $('#Id').val(),
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Commented By', 'Commented On', 'Rejection Comments', 'Resolution Comments'],
        colModel: [
        // { name: 'Issue Number', index: 'EntityRefId', sortable: false },
              { name: 'CommentedBy', index: 'CommentedBy', sortable: false, width: 60 },
              { name: 'CommentedOn', index: 'CommentedOn', sortable: false, width: 90 },
              { name: 'RejectionComments', index: 'RejectionComments', sortable: false, width: 90 },
              { name: 'ResolutionComments', index: 'ResolutionComments', sortable: false, width: 90 }
        ],
        rowNum: -1,
        //width: 1160,
        shirnkToFit: true,
        autowidth: true,
        height: 150,
        sortname: 'EntityRefId',
        sortorder: "desc",
        viewrecords: true,
        altRows: true,
        multiselect: true,
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
        colNames: ['Uploaded By', 'Uploaded On', 'File Name'],
        colModel: [
                          { name: 'UploadedBy', index: 'UploadedBy', width: 30, align: 'left', sortable: false },
                          { name: 'UploadedOn', index: 'UploadedOn', width: 60, align: 'left', sortable: false },
                          { name: 'FileName', index: 'FileName', width: 60, align: 'left', sortable: false }],
        pager: '#uploadedfilesgridpager',
        rowNum: '10',
        rowList: [10, 20, 50, 100, 150, 200],
        multiselect: true,
        viewrecords: true,
        altRows: true,
        multiselect: true,
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

    $("#file1").click(function () {
        var Id = $("#Id").val();
        if (Id == 0) {
            ErrMsg("Please create Issue.");
            return false;
        }
    });

    $('#btnIdRejectIssue').click(function () {
        return rejectionValidation("Please enter the comments to reject.");
    });
    $('#btnReply').click(function () {
        return rejectionValidation("Please enter the comments to reply.");
    });
    $('#btnIdResolveIssue').click(function () {
        if ($('#txtResolution').val() == null || $('#txtResolution').val() == "") {
            ErrMsg("Please enter the Resolution Comments", function () { $('#txtResolution').focus(); });
            return false;
        } else {
            return true;
        }
        return false;
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
        $("#CommentList").jqGrid('GridUnload');
        $("#Uploadedfileslist").jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    $("#btnAssign").click(function () {
        var ResolverId = $('#ResolverUserId').val();
        if (ResolverId != "") {
            $.ajax({
                type: 'GET',
                async: false,
                dataType: "json",
                //url: '/Home/AssignActivityjson?ActivityId=' + $('#ActivityId').val() + "&UserId=" + ResolverId + "&IsType=Staff",
                url: '/StaffIssues/AssignActivityjson?ActivityId=' + $('#ActivityId').val() + "&UserId=" + ResolverId + "&IsType=Staff",
                success: function (data) {
                    if (data == true) {
                        SucessMsg("Issue Assigned to " + ResolverId + ".", function () { window.location.href = $("#ReturnUrl").val(); });
                    } else {
                        ErrMsg("Issue not assigned. Please try again later.");
                        return false;
                    }
                }
            });
        } else {
            ErrMsg("Please select User to whom do you want to assign.", function () { $('#ResolverUserId').focus(); });
            return false;
        }
    });


});

function uploaddat(id) {
    window.location.href = "/StaffIssues/uploaddisplay?Id=" + id;
    processBusy.dialog('close');
}

function rejectionValidation(msg) {

    if ($('#txtRejCommentsArea').val() == null || $('#txtRejCommentsArea').val() == "") {
        ErrMsg(msg, function () { $('#txtRejCommentsArea').focus(); });
        $('#txtRejCommentsArea').attr("readonly", false).css("background-color", "white");
        return false;
    } else {
        return true;
    }
    return false;
}

function ValidateIssueDescription() {
    var Issdesc = $("#txtDescription").val();
    // alert(Issdesc.length);
    if (Issdesc.length > 4000)
        return false;
}
function GetIssueType() {
    var value = $('#ddlIssueGroup option:selected').val();
    if (value != "") {
        $.ajax({
            type: 'POST',
            async: false,
            url: '/StaffIssues/FillStaffIssueType/?IssueGroup=' + value,
            success: function (data) {
                $("#ddlIssueType").empty();
                $("#ddlIssueType").append("<option value=''> Select One </option>");
                for (var i = 0; i < data.rows.length; i++) {
                    if (data.rows[i].Text == $('#IssueType').val()) {
                        $("#ddlIssueType").append("<option value='" + data.rows[i].Value + "' selected='selected'>" + data.rows[i].Text + "</option>");
                    } else {
                        $("#ddlIssueType").append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
                    }
                }
            },
            dataType: "json"
        });
    }
    else {
        $("#ddlIssueType").empty();
        $("#ddlIssueType").append("<option value=''> Select One </option>");
    }
}

function GetResolverUserID() {
    //var campus = $('#BranchCode').val();
    var issueGroup=$('#IssueGroup').val();
    $.ajax({
        type: 'GET',
        async: true,
        dataType: "json",
        url: "/Home/ReceiverGroup?Campus="+ $("#BranchCode").val() +"&RoleCode=GRL&AppCode=SIM&IssGroup=" + issueGroup,
        success: function (data) {
            $("#ResolverUserId").empty();
            $("#ResolverUserId").append("<option value=''> --Select One-- </option>");
            for (var i = 0; i < data.rows.length; i++) {
                $("#ResolverUserId").append("<option value='" + data.rows[i].UserId + "'>" + data.rows[i].UserName + "</option>");
                //$("#ResolverUserId").append("<option value='" + data.rows[i].UserId + "'>" + data.rows[i].UserId + "</option>");
            }
        }
    });
}
function FillCampusDll() {
    debugger;
    $.getJSON("/Base/FillBranchCode",
      function (fillbc) {
          var ddlbc = $("#ddlBranchCode");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select One" }));
          $.each(fillbc, function (index, itemdata) {
              if (itemdata.Text == $('#BranchCode').val()) {
                  ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
              }
              else {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              }

          });
      });
}