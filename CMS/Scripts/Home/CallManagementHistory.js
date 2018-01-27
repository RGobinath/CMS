function FillCampusDll() {
    $.getJSON("/Base/FillAllBranchCode",
      function (fillbc) {
          var ddlbc = $("#ddlCampus");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillGradeByCampus() {
    var Campus = $("#ddlCampus").val();
    $.getJSON("/Assess360/GetGradeByCampus?Campus=" + Campus,
      function (fillbc) {
          var ddlbc = $("#ddlGrade");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillIssueGroupDll() {
    $.getJSON("/Home/FillIssueGroup",
      function (fillbc) {
          var ddlbc = $("#ddlIssueGroup");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillIssueTypeByIssueGroup() {
    var IssueGroup = $("#ddlIssueGroup").val();    
    $.getJSON("/Home/FillIssueType?IssueGroup=" + IssueGroup,
      function (fillbc) {
          var ddlbc = $("#ddlIssueType");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));
          $.each(fillbc, function (index, itemdata) {
              for (var i = 0; i < itemdata.length; i++) {
                  ddlbc.append($('<option/>', { value: itemdata[i].Value, text: itemdata[i].Text }));
              }
          });
      });
}
jQuery(function ($) {
    FillCampusDll();
    FillIssueGroupDll();
    //FillAcademicYearDll();
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        jQuery(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    jQuery(grid_selector).jqGrid({
        url: '/Home/CallManagementHistoryJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['CallManagementTrigger_Id', 'Id', 'Issue Number', 'Description', 'User Inbox', 'Information For', 'Issue Group', 'Issue Type', 'Status', 'Activity FullName', 'Issue Date',
                    'Campus', 'Student Name', 'Grade', 'Section', 'Is Hosteller', 'Email', 'Is Issue Completed', 'Call Phone Number', 'Call From', 'Caller Name', 'StudentNumber', 'StudentType',
                    'Receiver', 'ReceiverGroup', 'Approver', 'InstanceId', 'User Role Name', 'Resolution', 'IsInformation', 'BranchCode', 'DeptCode', 'WaitingForParentCnfm', 'LeaveType',
                    'ActionDate', 'Performer', 'UserType', 'BoardingType', 'Action Name', 'Deleted Date', 'Delete Comments', 'Revert Date', 'Revert Comments'
        ],
        colModel: [
                      { name: 'CallManagement_TriggerId', index: 'CallManagement_TriggerId', key: true, hidden: true, editable: true },
                      { name: 'Id', index: 'Id', hidden: true, editable: true },
                      { name: 'IssueNumber', index: 'IssueNumber' },
                      { name: 'Description', index: 'Description', hidden: true, editable: false,search:false },
                      { name: 'UserInbox', index: 'UserInbox', hidden: true, editable: false, search: false },
                      { name: 'InformationFor', index: 'InformationFor', hidden: true, editable: false, search: false },
                      { name: 'IssueGroup', index: 'IssueGroup', hidden: false, editable: false, search: true },
                      { name: 'IssueType', index: 'IssueType', hidden: false, editable: false, search: true },
                      { name: 'Status', index: 'Status', hidden: true, editable: false },
                      { name: 'ActivityFullName', index: 'ActivityFullName', hidden: true, editable: false },
                      { name: 'IssueDate', index: 'IssueDate', hidden: false, editable: false, search: false, width: '100' },
                      { name: 'Campus', index: 'Campus', editable: false },
                      { name: 'StudentName', index: 'StudentName', editable: false, search: true },
                      { name: 'Grade', index: 'Grade', editable: false, search: true },
                      { name: 'Section', index: 'Section', hidden: false, editable: false },
                      { name: 'IsHosteller', index: 'IsHosteller', hidden: true, editable: false },
                      { name: 'Email', index: 'Email', hidden: true, editable: false },
                      { name: 'IsIssueCompleted', index: 'IsIssueCompleted', hidden: true, editable: false },
                      { name: 'CallPhoneNumber', index: 'CallPhoneNumber', hidden: true, editable: false },
                      { name: 'CallFrom', index: 'CallFrom', hidden: true, editable: false },
                      { name: 'CallerName', index: 'CallerName', hidden: true, editable: false },
                      { name: 'StudentNumber', index: 'StudentNumber', hidden: false, editable: false },
                      { name: 'StudentType', index: 'StudentType', hidden: true, editable: false },
                      { name: 'Receiver', index: 'Receiver', hidden: true, editable: false },
                      { name: 'ReceiverGroup', index: 'ReceiverGroup', hidden: true, editable: false },
                      { name: 'Approver', index: 'Approver', hidden: true, editable: false },
                      { name: 'InstanceId', index: 'InstanceId', hidden: true, editable: false },
                      { name: 'UserRoleName', index: 'UserRoleName', hidden: true, editable: false },
                      { name: 'Resolution', index: 'Resolution', hidden: true, editable: false },
                      { name: 'IsInformation', index: 'IsInformation', hidden: true, editable: false },
                      { name: 'BranchCode', index: 'BranchCode', hidden: true, editable: false },
                      { name: 'DeptCode', index: 'DeptCode', hidden: true, editable: false },
                      { name: 'WaitingForParentCnfm', index: 'WaitingForParentCnfm', hidden: true, editable: false },
                      { name: 'LeaveType', index: 'LeaveType', hidden: true, editable: false },
                      { name: 'ActionDate', index: 'ActionDate', hidden: true, editable: false },
                      { name: 'Performer', index: 'Performer', hidden: true, editable: false },
                      { name: 'UserType', index: 'UserType', hidden: true, editable: false },
                      { name: 'BoardingType', index: 'BoardingType', hidden: true, editable: false },
                      { name: 'ActionName', index: 'ActionName', hidden: true, editable: false, search: false },
                      { name: 'TriggerDate', index: 'TriggerDate', hidden: false, editable: false, search: false, width: '100' },
                      { name: 'DeleteComments', index: 'DeleteComments', hidden: false, editable: true, editrules: { required: true }, search: false },
                      { name: 'RevertDate', index: 'RevertDate', hidden: true, editable: false, search: false },
                      { name: 'RevertComments', index: 'RevertComments', hidden: true, editable: true, search: false, editrules: {edithidden:true} }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        multiselect: true,
        shrinkToFit: false,
        autowidth: false,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Call Management History",
    })
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size


    //switch element when editing inline
    function aceSwitch(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=checkbox]')
                    .addClass('ace ace-switch ace-switch-5')
                    .after('<span class="lbl"></span>');
        }, 0);
    }
    //enable datepicker
    function pickDate(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=text]')
                        .datepicker({ format: 'yyyy-mm-dd', autoclose: true });
        }, 0);
    }


    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: true,
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
                viewicon: 'ace-icon fa fa-search-plus grey',                
            },
            {
                //width: 'auto', url: '/Assess360/EditSemester', modal: false,
                url: '/Home/EditCallManagementHistory',
                closeAfterEdit: true, closeOnEscape: true,
                beforeSubmit: function (postdata, formid) {
                    if (postdata.RevertComments != "") {
                        var yes = confirm("Do You Want To Revert this Issue?")
                        if (yes) {
                            return [true];
                            //return [success, message];
                            //$.ajax({
                            //    Type: "POST",
                            //    url: "/Home/EditCallManagementHistory",
                            //    data: { DeleteComments: postdata.DeleteComments, CallManagement_TriggerId: postdata.CallManagement_TriggerId, Id: postdata.Id,RevertComments:postdata.RevertComments },
                            //    success: function (data) {
                            //        if (data == "successrevert") {                                        
                            //            SucessMsg("Successfully Reverted");                                                                                
                            //            $(grid_selector).trigger("reloadGrid");
                            //        }
                            //        if (data == "failed") {
                            //            ErrMsg("Failed to Update");                                        
                            //        }
                            //    }
                            //});
                        }
                        else {                            
                            ErrMsg("Canceled");
                            $("#RevertComments").val('');
                            return [false];
                        }
                    }
                    if (postdata.RevertComments=="")
                    {
                        return [true];
                        //$.ajax({
                        //    Type: "POST",
                        //    url: "/Home/EditCallManagementHistory",
                        //    data: { DeleteComments: postdata.DeleteComments, CallManagement_TriggerId: postdata.CallManagement_TriggerId, Id: postdata.Id,RevertComments:postdata.RevertComments },
                        //    success: function (data) {
                        //        if (data == "success") {                                                                        
                        //            SucessMsg("Successfully Updated");                                    
                        //            $(grid_selector).trigger("reloadGrid");
                        //        }
                        //        if (data == "failed")
                        //        {
                        //            ErrMsg("Failed to Update");
                        //            $(grid_selector).trigger("reloadGrid");
                        //        }
                        //    }
                        //});
                    }
                    
                }
                //afterSubmit : function(response, postdata)
                //{
                //    var successdata = response.responseText;
                //    successdata = successdata.replace(/\"/g, "");
                //    if (successdata == "successrevert");
                //    {
                //        SucessMsg("Reverted Successfully");
                //        return [true];
                //    }
                //    if (successdata == "success");
                //    {
                //        SucessMsg('Updated Successfully');
                //        return [true];
                //    }
                //    if (successdata == "failed");
                //    {
                //        SucessMsg("Failed");
                //        return [true];
                //    }
                //} 
                //beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Edit
            {
                // width: 'auto', url: '/Assess360/AddSemester', modal: false,
                //url: '/Common/AddAcademicMaster', closeOnEscape: true, beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Add
              {
                  //width: 'auto', url: '/Assess360/DeleteSemester', beforeShowForm: function (params) {
                  //    selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                  //    return { Id: selectedrows }
                  //}
              },
               {},
                {})
    $("#ddlCampus").change(function () {
        FillGradeByCampus();
    });
    $("#txtIssueDate").datepicker({
        format: "dd/mm/yyyy",
        endDate: new Date(),
        autoclose:true
    });
    $("#txtDeletedDate").datepicker({
        format: "dd/mm/yyyy",
        endDate: new Date(),
        autoclose:true
    });
    $("#txtIssueNumber1").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Home/CallMgmntHistoryIssueNumberAutoComplete",
                type: "POST",
                dataType: "json",
                data: { term: request.term,Campus:$("#ddlCampus").val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.IssueNumber, value: item.IssueNumber };
                    }))
                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });
    $("#ddlIssueGroup").change(function () {
        FillIssueTypeByIssueGroup();
    });
    $(grid_selector).jqGrid('filterToolbar', {
        searchOnEnter: true, enableClear: false,
        afterSearch: function () { $(grid_selector).clearGridData(); }
    });
    //$("#ddlCampus").change(function () {
    //    FillGradeByCampus();
    //});
    $("#btnDelete").click(function () {
        ModifiedLoadPopupDynamicaly("/Home/DeleteCallManagement", $('#DeleteCallMgmnt'),
                        function () {
                        }, function () { }, 300, 230, "Delete Call Management");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Home/CallManagementHistoryJqGrid',
           postData: { Campus: "", Grade:"",Section:"",StudentName:"",StudentNumber:"",IssueNumber:"",IssueGroup:"",IssueType:""},
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        var Campus = $("#ddlCampus").val();
        var Grade = $("#ddlGrade").val();
        var Section = $("#ddlSection").val();
        var StudentName = $("#txtStudentName").val();
        var StudentNumber = $("#txtStudentNumber").val();        
        var IssueNumber = $("#txtIssueNumber1").val();
        var IssueGroup = $("#ddlIssueGroup").val();
        var IssueType = $("#ddlIssueType").val();
        //var IssueDate = $("#txtIssueDate").val();
        //var DeletedDate = $("#txtDeletedDate").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Home/CallManagementHistoryJqGrid',
           postData: { Campus: Campus, Grade: Grade, Section: Section, StudentName: StudentName, StudentNumber: StudentNumber, IssueNumber: IssueNumber, IssueGroup: IssueGroup, IssueType: IssueType},
           page: 1
       }).trigger("reloadGrid");
    });
    function style_edit_form(form) {
        //enable datepicker on "sdate" field and switches for "stock" field
        form.find('input[name=sdate]').datepicker({ format: 'yyyy-mm-dd', autoclose: true })
                .end().find('input[name=stock]')
                    .addClass('ace ace-switch ace-switch-5').after('<span class="lbl"></span>');
        //don't wrap inside a label element, the checkbox value won't be submitted (POST'ed)
        //.addClass('ace ace-switch ace-switch-5').wrap('<label class="inline" />').after('<span class="lbl"></span>');

        //update buttons classes
        var buttons = form.next().find('.EditButton .fm-button');
        buttons.addClass('btn btn-sm').find('[class*="-icon"]').hide(); //ui-icon, s-icon
        buttons.eq(0).addClass('btn-primary').prepend('<i class="ace-icon fa fa-check"></i>');
        buttons.eq(1).prepend('<i class="ace-icon fa fa-times"></i>')

        buttons = form.next().find('.navButton a');
        buttons.find('.ui-icon').hide();
        buttons.eq(0).append('<i class="ace-icon fa fa-chevron-left"></i>');
        buttons.eq(1).append('<i class="ace-icon fa fa-chevron-right"></i>');
    }

    function style_delete_form(form) {
        var buttons = form.next().find('.EditButton .fm-button');
        buttons.addClass('btn btn-sm btn-white btn-round').find('[class*="-icon"]').hide(); //ui-icon, s-icon
        buttons.eq(0).addClass('btn-danger').prepend('<i class="ace-icon fa fa-trash-o"></i>');
        buttons.eq(1).addClass('btn-default').prepend('<i class="ace-icon fa fa-times"></i>')
    }

    function style_search_filters(form) {
        form.find('.delete-rule').val('X');
        form.find('.add-rule').addClass('btn btn-xs btn-primary');
        form.find('.add-group').addClass('btn btn-xs btn-success');
        form.find('.delete-group').addClass('btn btn-xs btn-danger');
    }
    function style_search_form(form) {
        var dialog = form.closest('.ui-jqdialog');
        var buttons = dialog.find('.EditTable')
        buttons.find('.EditButton a[id*="_reset"]').addClass('btn btn-sm btn-info').find('.ui-icon').attr('class', 'ace-icon fa fa-retweet');
        buttons.find('.EditButton a[id*="_query"]').addClass('btn btn-sm btn-inverse').find('.ui-icon').attr('class', 'ace-icon fa fa-comment-o');
        buttons.find('.EditButton a[id*="_search"]').addClass('btn btn-sm btn-purple').find('.ui-icon').attr('class', 'ace-icon fa fa-search');
    }

    function beforeDeleteCallback(e) {
        var form = $(e[0]);
        if (form.data('styled')) return false;

        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
        style_delete_form(form);

        form.data('styled', true);
    }

    function beforeEditCallback(e) {
        var form = $(e[0]);
        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
        style_edit_form(form);
    }



    //it causes some flicker when reloading or navigating grid
    //it may be possible to have some custom formatter to do this as the grid is being created to prevent this
    //or go back to default browser checkbox styles for the grid
    function styleCheckbox(table) {
        /**
        $(table).find('input:checkbox').addClass('ace')
        .wrap('<label />')
        .after('<span class="lbl align-top" />')
        
        
        $('.ui-jqgrid-labels th[id*="_cb"]:first-child')
        .find('input.cbox[type=checkbox]').addClass('ace')
        .wrap('<label />').after('<span class="lbl align-top" />');
        */
    }


    //unlike navButtons icons, action icons in rows seem to be hard-coded
    //you can change them like this in here if you want
    function updateActionIcons(table) {
        /**
        var replacement = 
        {
        'ui-ace-icon fa fa-pencil' : 'ace-icon fa fa-pencil blue',
        'ui-ace-icon fa fa-trash-o' : 'ace-icon fa fa-trash-o red',
        'ui-icon-disk' : 'ace-icon fa fa-check green',
        'ui-icon-cancel' : 'ace-icon fa fa-times red'
        };
        $(table).find('.ui-pg-div span.ui-icon').each(function(){
        var icon = $(this);
        var $class = $.trim(icon.attr('class').replace('ui-icon', ''));
        if($class in replacement) icon.attr('class', 'ui-icon '+replacement[$class]);
        })
        */
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


    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});