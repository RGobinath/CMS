$(function () {
    $.getJSON("/Base/FillBranchCode",
     function (fillbc) {
         var ddlbc = $("#Campus");
         ddlbc.empty();
         ddlbc.append($('<option/>',
        {
            value: "",
            text: "Select One"
        }));
         $.each(fillbc, function (index, itemdata) {
             ddlbc.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
         });
         });


         function formateadorLink(cellvalue, options, rowObject) {
             return "<a href=/Assess360/Assess360?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
         }
         if ($("#A360Searched").val() != null) {
             var srchitems = $("#A360Searched").val();
             var srchitemsArr = srchitems.split(',');
             $("#RequestNo").val(srchitemsArr[0]);
             $("#Campus").val(srchitemsArr[1]);
             $("#Name").val(srchitemsArr[2]);
             $("#Section").val(srchitemsArr[3]);
             $("#Grade").val(srchitemsArr[4]);
         }

         var grid_selector = "#Assess360InboxList";
         var pager_selector = "#Assess360InboxPage";
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



         $(grid_selector).jqGrid({
             mtype: 'GET',
             url: '/Assess360/GetAssess360Inbox?ReqeustNo=' + $('#RequestNo').val() + '&Name=' + $('#Name').val() + '&Campus=' + $('#Campus').val() + '&Section=' + $('#Section').val() + '&Grade=' + $('#Grade').val(),
             postData: { Assess360Id: $('#Id').val() },
             datatype: 'json',
             height: '250',
             autowidth: true,
             shrinkToFit: true,
             colNames: ['Id', 'RequestNo', 'Academic Year', 'Campus Name', 'Id No', 'Name', 'Section', 'Grade',
            'Consolidated Marks', 'Status', 'Created By', 'Date Created', 'Student Id', ''],
             colModel: [
                          { name: 'Id', index: 'Id', hidden: true, key: true },
                          { name: 'RequestNo', index: 'RequestNo',width:'200', formatter: formateadorLink },
                          { name: 'AcademicYear', index: 'AcademicYear' },
                          { name: 'Campus', index: 'Campus' },
                          { name: 'IdNo', index: 'IdNo' },
                          { name: 'Name', index: 'Name',width:'250' },
                          { name: 'Dection', index: 'Section', width: '80' },
                          { name: 'Grade', index: 'Grade' },
                          { name: 'ConsolidatedMarks', index: 'ConsolidatedMarks' },
                          { name: 'Status', index: 'Status', hidden: true },
                          { name: 'CreatedBy', index: 'CreatedBy',width:'150' },
                          { name: 'DateCreated', index: 'DateCreated' },
                          { name: 'StudentId', index: 'StudentId', hidden: true },
                          { name: 'CreatedBy', index: 'CreatedBy', editable: true, hidden: true }
                          ],
             pager: pager_selector,
             rowNum: '10',
             rowList: [20, 50, 100, 150, 200],
             sortname: 'RequestNo',
             sortorder: 'asc',
             multiselect: true,
             viewrecords: true,
             caption: '<i class="fa fa-list">&nbsp;</i>Assess360 Inbox',
             loadComplete: function () {

                 var table = this;
                 setTimeout(function () {
                    
                     updatePagerIcons(table);
                     enableTooltips(table);
                 }, 0);
                 $(window).triggerHandler('resize.jqGrid');
             }
         });
         $(grid_selector).navGrid(pager_selector, { add: false, edit: false, del: false, search: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green' });
         $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
             caption: '&nbsp;&nbsp;<i class="fa fa-file-excel-o fa-lg blue">&nbsp;</i><u>Export To Excel<u/>',
             onClickButton: function () {
                 //  alert('in');
                 window.open("ExportToExcel" + '?rows=9999' + '&RequestNo=' + $('#RequestNo').val() +
            '&Name=' + $('#Name').val() +
            '&Campus=' + $('#Campus option:selected').val() +
            '&Section=' + $('#Section option:selected').val() +
            '&Grade=' + $('#Grade option:selected').val());
             }
         });

         $('#btnSearch').click(function () {
             $(grid_selector).clearGridData();
            
             if ($("#Grade").val() == 'Select') {
                 ErrMsg("Please select Grade");
                 $(grid_selector).clearGridData();
                 return false;
             }
             else {
                 LoadSetGridParam($(grid_selector), "/Assess360/GetAssess360Inbox?RequestNo=" + $('#RequestNo').val() +
            '&Name=' + $('#Name').val() +
            '&Campus=' + $('#Campus option:selected').val() +
            '&Section=' + $('#Section option:selected').val() +
            '&Grade=' + $('#Grade option:selected').val()
            );
             }
             //            if ((!isEmptyorNull($('#RequestNo').val()) || !isEmptyorNull($('#Name').val()) ||
             //                $('#Campus option:selected').val() != 'Select' || $('#Section option:selected').val() != 'Select' ) &&
             //                $('#Grade option:selected').val() != 'Select') {
             //                $('#Assess360InboxList').clearGridData();
             //                LoadSetGridParam($('#Assess360InboxList'), "/Assess360/GetAssess360Inbox?RequestNo=" + $('#RequestNo').val() +
             //            '&Name=' + $('#Name').val() +
             //            '&Campus=' + $('#Campus option:selected').val() +
             //            '&Section=' + $('#Section option:selected').val() +
             //            '&Grade=' + $('#Grade option:selected').val()
             //            );
             //            } else {
             //                ErrMsg('Please enter or select any values', function () { $("#RequestNo").focus(); });
             //                return false;
             //            }
         });

         $('#btnReset').click(function () {
             $('#Assess360InboxList').clearGridData();
             $('#RequestNo').val('');
             $('#Name').val('');
             $('#Campus').val('');
             $('#Section').val('Select');
             $('#Grade').val('Select');
             $('#SavedSearchTempl').val('Select');
             $('#IsDefaultSST').attr("checked", false);
             $('#SSTIsDefault').val('false');
             LoadSetGridParam($(grid_selector), "/Assess360/GetAssess360Inbox?page=1");
         });

         $("#btnNewAssess360").click(function () {
             window.location.href =  $("#BackUrl").val();
         });

         $("#btnBack2StaffInbox").click(function () {
            // window.location.href = '@Url.Action("ComponentInbox", "Assess360")';
             window.location.href = $("#BackUrl1").val();
         });
         /*enter key press event*/
         function defaultFunc(e) {
             if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                 $('#btnSearch').click();
                 return false;
             }
             return true;
         };

         $('#RequestNo').keypress(function (event) { if (isEmptyorNull($('#RequestNo').val())) { return true; } else { return defaultFunc(event); } });
         $('#Name').keypress(function (event) { if (isEmptyorNull($('#Name').val())) { return true; } else { return defaultFunc(event); } });
         $('#Campus').keypress(function (event) { if (isEmptyorNull($('#Campus').val())) { return true; } else { return defaultFunc(event); } });
         /*enter key press event*/

});