$(function () {
    var StaffPreRegNumber = $('#StaffPreRegNumber').val();
    //var PopUpCampus = $('#PopUpCampus').val();
    var PopUpCampus = "IB MAIN";
    
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        jQuery('#ShowCampusBasedStaffDetailsGrid').jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $('#ShowCampusBasedStaffDetailsGrid').closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery('#ShowCampusBasedStaffDetailsGrid').jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    });
    jQuery('#ShowCampusBasedStaffDetailsGrid').jqGrid({
        url: '/StaffManagement/CampusBasedStaffDetailsJqGrid?StaffPreRegNumber=' + StaffPreRegNumber,
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['Id', 'Staff PreReg Number', 'Campus', 'Grade', 'Section', 'Subject','Academic Year'],
        colModel: [
                      { name: 'Id', index: 'Id', width: 130, hidden: true, editable: true, key: true },
                      { name: 'StaffPreRegNumber', index: 'StaffPreRegNumber', width: 130, hidden: true, editable: true },
                      { name: 'Campus', index: 'Campus', width: 130, hidden: true, editable: true },
                      //{ name: 'Grade', index: 'Grade', width: 130, editable: true },
                      //{
                      //    name: 'Grade', index: 'Grade', width: 80, editable: true, edittype: 'select', editoptions: {
                      //        dataUrl: '/Admission/FillGradesWithArrayCriteria?campus=' + PopUpCampus
                      //    }, editrules: { custom: true, custom_func: checkvalid }
                      //},
                      {
                          name: 'Grade', index: 'Grade', editable: true, edittype: 'select', editoptions: {
                              dataUrl: '/Base/FillGradesWithArrayCriteria?campus=' + PopUpCampus,
                              buildSelect: function (data) {
                                  var CourseName = jQuery.parseJSON(data);
                                  var s = '<select>';
                                  s += '<option value="">----Select----</option>';
                                  if (CourseName && CourseName.length) {
                                      for (var i = 0, l = CourseName.length; i < l; i++) {
                                          s += '<option value="' + CourseName[i].Value + '">' + CourseName[i].Text + '</option>';
                                      }
                                  }
                                  return s + "</select>";
                              },
                              style: "width: 167px; height: 20px; font-size: 0.9em"
                          }, editrules: { custom: true, custom_func: checkvalid }, formoptions: { elmsuffix: ' *' }, sortable: true
                      },
                      //{ name: 'Section', index: 'Section', width: 130, editable: true },
                      {
                          name: 'Section', index: 'Section', editable: true, edittype: 'select', editoptions: {
                              dataUrl: '/Base/FillSectionBasedCampusGrade',
                              buildSelect: function (data) {
                                  var CourseName = jQuery.parseJSON(data);
                                  var s = '<select>';
                                  s += '<option value="">----Select----</option>';
                                  if (CourseName && CourseName.length) {
                                      for (var i = 0, l = CourseName.length; i < l; i++) {
                                          s += '<option value="' + CourseName[i].Value + '">' + CourseName[i].Text + '</option>';
                                      }
                                  }
                                  return s + "</select>";
                              },
                              style: "width: 167px; height: 20px; font-size: 0.9em"
                          }, editrules: { custom: true, custom_func: checkvalid }, formoptions: { elmsuffix: ' *' }, sortable: true
                      },
                      //{ name: 'Subject', index: 'Subject', width: 130, editable: true }
                        {
                            name: 'Subject', index: 'Subject', editable: true, edittype: 'select', editoptions: {
                                dataUrl: '/Base/FillSubjectByCampusAndGrade',
                                buildSelect: function (data) {
                                    var CourseName = jQuery.parseJSON(data);
                                    var s = '<select>';
                                    s += '<option value="">----Select----</option>';
                                    if (CourseName && CourseName.length) {
                                        for (var i = 0, l = CourseName.length; i < l; i++) {
                                            s += '<option value="' + CourseName[i].Value + '">' + CourseName[i].Text + '</option>';
                                        }
                                    }
                                    return s + "</select>";
                                },
                                style: "width: 167px; height: 20px; font-size: 0.9em"
                            }, editrules: { custom: true, custom_func: checkvalid }, formoptions: { elmsuffix: ' *' }, sortable: true
                        },
                        {
                            name: 'AcademicYear', index: 'AcademicYear', editable: true, edittype: 'select', editoptions: {
                                dataUrl: '/Base/GetJsonAcademicYear',
                                 buildSelect: function (data) {
                                     var CourseName = jQuery.parseJSON(data);
                                     var s = '<select>';
                                     s += '<option value="">----Select----</option>';
                                     if (CourseName && CourseName.length) {
                                         for (var i = 0, l = CourseName.length; i < l; i++) {
                                             s += '<option value="' + CourseName[i].Value + '">' + CourseName[i].Text + '</option>';
                                         }
                                     }
                                     return s + "</select>";
                                 },
                                 style: "width: 167px; height: 20px; font-size: 0.9em"
                             }, editrules: { custom: true, custom_func: checkvalid }, formoptions: { elmsuffix: ' *' }, sortable: true
                         }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: '#ShowCampusBasedStaffDetailsGridPager',
        altRows: true,
        sortorder: "Asc",
        autowidth: false,
        width: 775,
        shrinkToFit: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                //styleCheckbox(table);
                //updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Handled Subjects Details",
    })
    //navButtons
    $('#ShowCampusBasedStaffDetailsGrid').jqGrid('navGrid', '#ShowCampusBasedStaffDetailsGridPager',
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
            }, {
                url: '/StaffManagement/EditCampusBasedStaffDetails?StaffPreRegNum=' + StaffPreRegNumber
                , closeOnEscape: true
                , beforeShowForm: function (frm)
                { }
                , width:'auto'
            }, {
                url: '/StaffManagement/AddCampusBasedStaffDetails?StaffPreRegNum=' + StaffPreRegNumber, closeOnEscape: true, beforeShowForm: function (frm)
                { }
            }, {}, {}, {});


    // Configuration Grid Details
    $(window).on('resize.jqGrid', function () {
        jQuery('#AttendanceReportConfigurationDetailsGrid').jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $('#AttendanceReportConfigurationDetailsGrid').closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery('#AttendanceReportConfigurationDetailsGrid').jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    });

    jQuery('#AttendanceReportConfigurationDetailsGrid').jqGrid({
        url: '/StaffManagement/StaffAttendanceReportConfigurationJqGrid?StaffPreRegNum=' + StaffPreRegNumber,
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['Id', 'StaffPreRegNum', 'Campus', 'Department', 'Designation','Staff_AttendanceReportConfig_Id', 'Reporting Designation'
            , 'Reporting Head Name', 'Reporting Head', 'Assigned By', 'Assigned Date'],
        colModel: [
                      { name: 'Id', index: 'Id', width: 130, hidden: true, editable: true, key: true },
                      { name: 'StaffPreRegNum', index: 'StaffPreRegNum', hidden: true },
                      { name: 'Campus', index: 'Campus', hidden: true, editable: true },
                      { name: 'Department', index: 'Department', editable: true, hidden: true },
                      { name: 'Designation', index: 'Designation', hidden: true },
                      { name: 'Staff_AttendanceReportConfig_Id', index: 'Staff_AttendanceReportConfig_Id', hidden: true, editable: true },
                      { name: 'ReportingHeadDesignation', index: 'ReportingHeadDesignation', hidden: false },
                      //{
                      //    name: 'ReportingHeadDesignation', index: 'ReportingHeadDesignation', editable: true, edittype: 'select', editoptions: {
                      //        dataUrl: '/StaffManagement/FillReportingManagersNameAndDesignationByCampus?campus=' + PopUpCampus,
                      //        buildSelect: function (data) {
                      //            var CourseName = jQuery.parseJSON(data);
                      //            var s = '<select>';
                      //            s += '<option value="">----Select----</option>';
                      //            if (CourseName && CourseName.length) {
                      //                for (var i = 0, l = CourseName.length; i < l; i++) {
                      //                    s += '<option value="' + CourseName[i].Value + '">' + CourseName[i].Text + '</option>';
                      //                }
                      //            }
                      //            return s + "</select>";
                      //        },
                      //        style: "width: 200px; height: 20px; font-size: 0.9em"
                      //    }, editrules: { custom: true, custom_func: checkvalid }, formoptions: { elmsuffix: ' *' }, sortable: true
                      //},
                      { name: 'ReportingHeadName', index: 'ReportingHeadName', hidden: false },
                      //{ name: 'ReportingHeadPreRegNum', index: 'ReportingHeadPreRegNum', hidden: true },
                      {
                          name: 'ReportingHeadDesignation', index: 'ReportingHeadDesignation', editable: true, edittype: 'select', editoptions: {
                              dataUrl: '/StaffManagement/FillReportingManagersNameAndDesignationByCampus?campus=' + PopUpCampus,
                              buildSelect: function (data) {
                                  var CourseName = jQuery.parseJSON(data);
                                  var s = '<select>';
                                  s += '<option value="">----Select----</option>';
                                  if (CourseName && CourseName.length) {
                                      for (var i = 0, l = CourseName.length; i < l; i++) {
                                          s += '<option value="' + CourseName[i].Value + '">' + CourseName[i].Text + '</option>';
                                      }
                                  }
                                  return s + "</select>";
                              },
                              style: "width: 200px; height: 20px; font-size: 0.9em"
                          }, editrules: { edithidden: true }, hidden: true, formoptions: { elmsuffix: ' *' }, sortable: true
                      },
                      { name: 'ModifiedBy', index: 'ModifiedDate', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: '#AttendanceReportConfigurationDetailsGridPager',
        altRows: true,
        sortorder: "Asc",
        autowidth: false,
        width: 775,
        shrinkToFit: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                //styleCheckbox(table);
                //updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;Reporting Heads Details",
    });
    //navButtons
    $('#AttendanceReportConfigurationDetailsGrid').jqGrid('navGrid', '#AttendanceReportConfigurationDetailsGridPager',
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
            }, {
                url: '/StaffManagement/SaveOrUpdateOnStaffAttendanceReportConfiguration?StaffPreRegNum=' + StaffPreRegNumber
                , closeOnEscape: true
                , beforeShowForm: function (frm)
                { }
                , width: 'auto'

            }, {
                url: '/StaffManagement/SaveOrUpdateOnStaffAttendanceReportConfiguration?StaffPreRegNum=' + StaffPreRegNumber
                , closeOnEscape: true
                , beforeShowForm: function (frm)
                { }
                , width: 'auto'
            }, {}, {}, {});
});
function checkvalid(value, column) {
    if (value == 'nil' || value == "" || value == " ") {
        return [false, column + ": Field is Required"];
    }
    else {
        return [true];
    }
}