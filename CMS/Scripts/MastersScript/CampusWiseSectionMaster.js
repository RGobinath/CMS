jQuery(function ($) {
    GetCampus();
    FillAcademicYearDll();

    var grid_selector = "#CampusWiseSectionMasterJqGrid";
    var pager_selector = "#CampusWiseSectionMasterJqGridpager";

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
    debugger;
    jQuery(grid_selector).jqGrid({
        url: '/Common/CampusWiseSectionMasterJqGrid',
        datatype: 'json',
        height: 240,
        colNames: ['CampusWiseSectionMasterId', 'Academic Year', 'Campus', 'Grade', 'Section', 'IsActive', 'Created By', 'Created Date', 'Update By', 'Update Date'],
        colModel: [
                      { name: 'CampusWiseSectionMasterId', index: 'CampusWiseSectionMasterId', key: true, hidden: true, editable: true },
                         {
                             name: 'AcademicyrMaster.FormId', index: 'AcademicyrMaster.FormId', editable: true, search: true, edittype: 'select', editoptions: {
                                 dataUrl: '/Base/GetAcademicYearddl',
                                 buildSelect: function (data) {
                                     var AcademicYear = jQuery.parseJSON(data);
                                     var s = '<select>';
                                     s += '<option value="">----Select----</option>';
                                     if (AcademicYear && AcademicYear.length) {

                                         for (var i = 0, l = AcademicYear.length; i < l; i++) {

                                             s += '<option value="' + AcademicYear[i].Value + '">' + AcademicYear[i].Text + '</option>';
                                         }
                                     }
                                     return s + "</select>";
                                 },
                                 style: "width: 140px;"
                             }, editrules: { required: true }, sortable: true
                         },
                    	 {
                    	     name: 'CampusMaster.FormId', index: 'CampusMaster.FormId',
                    	     viewable: true,
                    	     editable: true,
                    	    // hidden: true,
                    	     edittype: 'select',
                    	     editoptions: {
                    	         style: "width: 140px; height: 22px; font-size: 0.9em",
                    	         dataUrl: "/Base/FillCampus",
                    	         buildSelect: function (data) {
                    	             debugger;
                    	             var response, s = '<select><option value="">Select</option>', i = 0;
                    	             response = jQuery.parseJSON(data);
                    	             if (response && response.length) {
                    	                 $.each(response, function (i) {
                    	                     s += '<option value="' + this.Value + '">' + this.Text + '</option>';
                    	                 });
                    	             }
                    	             return s + '</select>';
                    	         },
                    	         dataEvents:
                                     [{
                                         type: 'change', fn: function (e) {
                                             debugger;
                                             var e = document.getElementById('CampusMaster.FormId');
                                             var grad = document.getElementById('CampusGradeMaster.Id');
                                             var Campus = e.options[e.selectedIndex].value;
                                             var optionval = '';
                                             //var division = $(e.target).val();
                                             //var campus = $('#Campus').val();
                                             $.getJSON("/Base/GetCampusGradeddl?FormId=" + Campus,
                                                      function (fillbc) {
                                                          debugger;
                                                          grad.options.length = 0;
                                                          grad.innerHTML += "<option value=\"\">Select</option>";
                                                          //var option = document.createElement("option");                                                                                                                   
                                                          $.each(fillbc, function (index, itemdata) {
                                                              debugger;
                                                               grad.innerHTML += "<option value=\"" + itemdata.Value + "\">" + itemdata.Text + "</option>";
                                                              //$("#CampusGradeMaster.gradeId").append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                                                              //option.value = itemdata.Value;
                                                              //option.text = itemdata.Text;
                                                              //grad.options.add(option);
                                                          });
                                                          
                                                      });
                                         }
                                     },
                                     ]
                    	     }, editrules: { required: true, edithidden: true }
                    	 },
                                      {
                                          name: 'CampusGradeMaster.Id', index: 'CampusGradeMaster.Id',
                                          viewable: true,
                                          editable: true,
                                          //hidden: true,
                                          edittype: 'select',
                                          editoptions: {
                                              style: "width: 140px; height: 22px; font-size: 0.9em",
                                              dataUrl: "/Base/GetCampusGradeddl",
                                              buildSelect: function (data) {
                                                  debugger;
                                                  var response, s = '<select><option value="">Select</option>', i = 0;
                                                  response = jQuery.parseJSON(data);
                                                  if (response && response.length) {
                                                      $.each(response, function (i) {
                                                          s += '<option value="' + this.Value + '">' + this.Text + '</option>';
                                                      });
                                                  }
                                                  return s + '</select>';
                                              },
                                          }, editrules: { required: true, edithidden: true }
                                      },
                    //{ name: 'CampusGradeMaster.gradeId', index: 'CampusGradeMaster.gradeId', editable: true, search: true, edittype: 'select'},
                      { name: 'Section', index: 'Section', editable: true, search: true },
                      { name: 'IsActive', index: 'IsActive', editable: false, editrules: { required: true }, edittype: "select", editoptions: { sopt: ['eq'], value: { '': '--Select--', 'true': 'Yes', 'false': 'No' }, style: "width: 165px; height: 25px; font-size: 0.9em" }, editrules: { required: true }, sortable: true, stype: 'select' },
                      { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', width: 20, hidden: true },
                      { name: 'UpdateBy', index: 'UpdateBy', hidden: true },
                      { name: 'UpdateDate', index: 'UpdateDate', hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        autowidth: true,
        sortname: 'CampusWiseSectionMasterId',
        sortorder: 'Asc',
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
        caption: "Campus Wise Section Master"
    });


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
                del: true,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {
                url: '/Common/SaveOrUpdateCampusWiseSectionMaster', closeAfterEdit: true, closeOnEscape: true, beforeShowForm: function (frm)
                { $('#FormCode').attr('readonly', 'readonly'); }
            }, //Edit
            {
                url: '/Common/SaveOrUpdateCampusWiseSectionMaster', closeOnEscape: true, beforeShowForm: function (frm)
                { $('#FormCode').attr('readonly', 'readonly'); }
            }, //Add
                { url: '/Common/DeleteCampusWiseSectionMaster' },
              {},
               {},
                {})



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
    function styleCheckbox(table) {
    }
    function updateActionIcons(table) {
    }
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
    $("#Campus").change(function () {
        gradeddl();
    });
    $("#btnSearch").click(function () {
        debugger;
        var AcademicYearId = $("#AcademicYear").val();
        var CampusId = $("#Campus").val();
        var GradeId = $("#Grade").val();
        var Section = $("#txtSection").val();
        var IsActive = $("#ddlIsActive").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Common/CampusWiseSectionMasterJqGrid',
                    postData: { AcademicYearId: AcademicYearId, CampusId: CampusId, GradeId: GradeId,Section:Section, IsActive: IsActive, },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Common/CampusWiseSectionMasterJqGrid',
                    postData: { AcademicYearId: $("#AcademicYear").val(), CampusId: $("#Campus").val(), GradeId: $("#Grade").val(),Section:$("#txtSection").val(), IsActive: $("#ddlIsActive").val(), },
                    page: 1
                }).trigger("reloadGrid");
    });
});
function GetCampus() {
    var ddlcam = $("#Campus");
    $.ajax({
        type: 'POST',
        async: true,
        dataType: "json",
        url: "/Base/FillCampus",
        success: function (data) {
            ddlcam.empty();
            ddlcam.append("<option value=''> Select </option>");
            for (var i = 0; i < data.length; i++) {
                ddlcam.append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
            }
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}
function FillAcademicYearDll() {
    $.getJSON("/Base/GetAcademicYearddl",
      function (fillbc) {
          var ddlbc = $("#AcademicYear");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select " }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}

function gradeddl() {
    debugger;
    var e = document.getElementById('Campus');
    var campus = e.options[e.selectedIndex].value;
    $.getJSON("/Base/GetCampusGradeddl/", { FormId: campus },
            function (fillbc) {
                var select = $("#Grade");
                select.empty();
                select.append($('<option/>', { value: "", text: "Select Grade" }));
                $.each(fillbc, function (index, itemData) {
                    debugger;
                    select.append($('<option/>', { value: itemData.Value, text: itemData.Text }));
                });
            });
}
function checkvalid(value, column) {
    if (value == 'nil') {
        return [false, column + ": Field is Required"];
    }
    else {
        return [true];
    }
}
