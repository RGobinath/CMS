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
function FillAcademicYearDll() {
    $.getJSON("/Base/GetJsonAcademicYear",
      function (fillbc) {
          var ddlbc = $("#ddlAcademicYear");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
jQuery(function ($) {
    FillCampusDll();
    FillAcademicYearDll();
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
    var jqGridstartDate = new Date();    
    var jqGridstartDate = 01 + '/' + 04 + '/' + 1947;
    jQuery(grid_selector).jqGrid({
        url: '/Assess360/SemesterJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['SemesterMasterId', 'Campus', 'Grade', 'Academic Year', 'Sem', 'From Date', 'To Date', 'Exam Date', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'SemesterMasterId', index: 'SemesterMasterId', key: true, width: 130, hidden: true, editable: true },
                      {
                          name: 'Campus', index: 'Campus', editable: true, edittype: 'select', editrules: { required: true }, editoptions: {
                              dataUrl: '/Assess360/GetCampusddl',
                              dataEvents: [{
                                  type: 'change',
                                  fn: function (e) {
                                      var Campus = $(e.target).val();
                                      if (Campus != '') {
                                          $.getJSON('@Url.Action("GetGradeByCampus", "Assess360")',
                                          { Campus: Campus },
                                          function (recipes) {
                                              var selectHtml = "";
                                              selectHtml += '<option value=" ">--select--</option>';
                                              $.each(recipes, function (jdIndex, jdData) {
                                                  selectHtml = selectHtml + "<option value='" + jdData.Value + "'>" + jdData.Text + "</option>";
                                              });
                                              if ($(e.target).is('.FormElement')) {
                                                  var form = $(e.target).closest('form.FormGrid');
                                                  $("select#Grade.FormElement", form[0]).html(selectHtml);
                                              }
                                          });
                                      }
                                  }
                              }], style: "width: 265px; height: 25px; font-size: 0.9em"
                          },
                          stype: 'select', sortable: true,
                          searchoptions: {
                              dataUrl: '/Assess360/GetCampusddl',
                              dataEvents: [{
                                  type: 'change',
                                  fn: function (e) {
                                      var Campus = $(e.target).val();
                                      if (Campus != '') {
                                          $.getJSON('@Url.Action("GetGradeByCampus", "Assess360")',
                                                                      { Campus: Campus },
                                                                      function (cmp) {
                                                                          debugger;
                                                                          var selectHtml1 = "";
                                                                          selectHtml1 += '<option value="">-----select-----</option>';
                                                                          $.each(cmp, function (jdIndex1, jdData1) {
                                                                              selectHtml1 = selectHtml1 + "<option value='" + jdData1.Value + "'>" + jdData1.Text + "</option>";
                                                                          });
                                                                          $("#gs_Grade").html(selectHtml1);
                                                                      });
                                      }
                                  }
                              }],
                              style: "width: 265px; height: 25px; font-size: 0.9em"
                          }
                      },
                       {
                           name: 'Grade', index: 'Grade', editable: true, search: true, edittype: 'select',
                           editoptions: { dataUrl: '/Assess360/GetGradeddl', style: "width: 265px; height: 25px; font-size: 0.9em" },
                           editrules: { required: true, custom: false }, sortable: true, stype: 'select',
                           searchoptions: { dataUrl: '/Assess360/GetGradeddl' }
                       },
                       { name: 'AcademicYear', index: 'AcademicYear', editable: true, search: true, edittype: 'select', editoptions: { dataUrl: '/Assess360/AcademicYearddl', style: "width: 265px; height: 25px; font-size: 0.9em" }, editrules: { required: true, custom: false }, sortable: true, stype: 'select', searchoptions: { dataUrl: '/Assess360/AcademicYearddl' } },
                       { name: 'Sem', index: 'Sem', editable: true, search: true, edittype: 'select', editoptions: { value: ":--Select--;Sem I:Sem I;Sem II:Sem II;Sem III:Sem III", style: "width: 265px; height: 25px; font-size: 0.9em" }, sortable: true, stype: 'select' },
                       {
                           name: 'FromDate', index: 'FromDate', editable: true,search:false,editoptions: {
                               size: 10, maxlengh: 10,
                               dataEvents: [{
                                   type: 'change',
                                   fn: function (e) {
                                       jqGridstartDate = $(e.target).val();
                                       var jqGridToDate = $("#ToDate").val();
                                       if (jqGridToDate != "") {
                                           if (jqGridstartDate > jqGridToDate) {                                               
                                               ErrMsg("To Date must be greater than From Date");
                                               $('#ToDate').attr("value", "");
                                           }
                                       }
                                       //if(jqGridstartDate)
                                       //if ($(e.target).is('.FormElement')) {
                                       //    debugger;
                                       //    var form = $(e.target).closest('form.FormGrid');
                                       //    $("#ToDate.FormElement", form[0]).datepicker({
                                       //        startDate: $(e.target).val(),
                                       //        format: "dd/mm/yyyy",
                                       //        //endDate: jqGridEndDate,
                                       //        todayHighlight: true,
                                       //        autoclose: true
                                       //    });
                                       //}
                                   }
                               }],
                               dataInit: function (element) {
                                   var id = $(grid_selector).getGridParam('selrow');
                                   celValue = $(grid_selector).jqGrid('getCell', id, 'FromDate');
                                   $(element).datepicker({
                                       autoopen: false,
                                       startDate: jqGridstartDate,
                                       format: "dd/mm/yyyy",
                                       //endDate: jqGridEndDate,
                                       todayHighlight: true,
                                       autoclose: true
                                   }).attr('readonly', 'readonly');
                               }, style: "width: 265px; height: 25px; font-size: 0.9em"
                           }, editrules: { required: true }

                       },
            {
                name: 'ToDate', index: 'ToDate', editable: true,search:false,editoptions: {                    
                    dataInit: function (element) {
                        $(element).attr('readonly', 'readonly');
                        $(element).click(function () {                            
                            $(element).datepicker('remove');
                            $(element).datepicker({
                                autoclose: true,
                                format: "dd/mm/yyyy",
                                startDate:jqGridstartDate
                            }).datepicker('show');
                        })
                    },
                    style: "width: 265px; height: 25px; font-size: 0.9em"
                }, editrules: {required:true}
            },
                       {
                           name: 'ExamDate', index: 'ExamDate', search: false, editable: true, editrules: { required: true }, editoptions: {
                               dataInit: function (elem) {
                                   $(elem).datepicker({
                                       format: "dd/mm/yyyy",
                                   }).attr('readonly', 'readonly');
                               },
                               style: "width: 265px; height: 25px; font-size: 0.9em"
                           }
                       },
                      { name: 'CreatedBy', index: 'CreatedBy', width: 20, hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 20, hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedDate', width: 20, hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', width: 20, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        multiselect: true,
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
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Semester Master",
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
                add: true,
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
            {
                width: 'auto', url: '/Assess360/EditSemester', modal: false,
                //url: '/Common/AddAcademicMaster/?test=edit', closeAfterEdit: true, closeOnEscape: true, beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Edit
            {
                width: 'auto', url: '/Assess360/AddSemester', modal: false,
                //url: '/Common/AddAcademicMaster', closeOnEscape: true, beforeShowForm: function (frm)
                //{ $('#FormCode').attr('readonly', 'readonly'); }
            }, //Add
              {
                  width: 'auto', url: '/Assess360/DeleteSemester', beforeShowForm: function (params) {
                      selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                      return { Id: selectedrows }
                  }
              },
               {},
                {})

    $(grid_selector).jqGrid('filterToolbar', {
        searchOnEnter: true, enableClear: false,
        afterSearch: function () { $(grid_selector).clearGridData(); }
    });
    $("#ddlCampus").change(function () {
        FillGradeByCampus();
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Assess360/SemesterJqGrid',
           postData: { Campus: "", Grade: "", Sem: "", AcademicYear: "" },
           page: 1
       }).trigger("reloadGrid");
    });
    $("#btnSearch").click(function () {
        var Campus = $("#ddlCampus").val();
        var Grade = $("#ddlGrade").val();
        var Sem = $("#ddlSem").val();
        var AcademicYear = $("#ddlAcademicYear").val();
        jQuery(grid_selector).setGridParam(
       {
           datatype: "json",
           url: '/Assess360/SemesterJqGrid',
           postData: { Campus: Campus, Grade: Grade, AcademicYear: AcademicYear, Sem: Sem },
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
