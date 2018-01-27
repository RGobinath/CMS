$(function () {
    var grid_selector = "#jqGridSurveyQuestionList";
    var pager_selector = "#jqGridSurveyQuestionListPager";
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
    $(grid_selector).jqGrid({
        url: '/StaffManagement/GetJqGridsurveyList',
        datatype: 'Json',
        mtype: 'GET',
        colNames: ['SurveyQuestionId', 'Survey Group', 'Survey Question', 'Is Active', ],
        colModel: [
                     { name: 'SurveyQuestionId', index: 'SurveyQuestionId', key: true, hidden: true, editable: true },
                     {
                         name: 'SurveyGroupMaster.SurveyGroupId', index: 'SurveyGroupMaster.SurveyGroupId', editable: true, edittype: 'select',
                         editoptions: {
                             dataUrl: '/StaffManagement/GetSurveyGroupddl',
                             buildSelect: function (data) {
                                 SurveyGroup = jQuery.parseJSON(data);
                                 var s = '<select>';
                                 s += '<option value=" ">Select</option>';
                                 if (SurveyGroup && SurveyGroup.length) {
                                     for (var i = 0, l = SurveyGroup.length; i < l; i++) {
                                         s += '<option value="' + SurveyGroup[i].Value + '">' + SurveyGroup[i].Text + '</option>';
                                     }
                                 }
                                 return s + "</select>";
                             },
                             style: "width: 160px; height: 25px; font-size: 0.9em"
                         }
                     },
                    { name: 'SurveyQuestion', index: 'SurveyQuestion', sortable: true, editable: true },
                    {
                        name: 'IsActive', index: 'IsActive', sortable: true, editable: true, edittype: 'select', editoptions: {
                            value: {
                                'true': 'Yes',
                                'false': 'No',

                            },
                        }, width: 50
                    },

        ],

        viewrecords: true,
        altRows: true,
        autowidth: true,
        multiselect: true,
        // multiboxonly: true,
        height: 230,
        rowNum: 10,
        rowList: [5, 10, 20],
        sortname: 'SurveyQuestionId',
        sortorder: 'Desc',
        pager: pager_selector,

        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="fa fa-th-list"></i>&nbsp;Survey Details'
    })
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size            
    //navButtons
    $(grid_selector).jqGrid('navGrid', pager_selector,
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
               width: 'auto', url: '/StaffManagement/EditSurveyQuestionMaster', modal: false, closeAfterEdit: true
               //url: '/Common/AddAcademicMaster/?test=edit', closeAfterEdit: true, closeOnEscape: true, beforeShowForm: function (frm)
               //{ $('#FormCode').attr('readonly', 'readonly'); }
           }, //Edit
           {
               width: 'auto', url: '/StaffManagement/AddSurveyQuestionMaster', modal: false, closeAfterAdd: true
               //url: '/Common/AddAcademicMaster', closeOnEscape: true, beforeShowForm: function (frm)
               //{ $('#FormCode').attr('readonly', 'readonly'); }
           }, //Add
             {
                 width: 'auto', url: '/StaffManagement/DeleteSurveyQuestionMaster', beforeShowForm: function (params) {
                     selectedrows = $(grid_selector).jqGrid("getGridParam", "selarrrow");
                     return { Id: selectedrows }
                 }
             },
              {},
               {})



    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });


    //$('#btnSave').click(function () {
    //    var StudentSurvayGroup = $("#ddlStudentSurveyGroupMaster").val();
    //    var StudentSurveyQuestion = $("#addStudentSurveyQuestionMaster").val();
    //    var AcademicYear = $("#ddlAcademicYearmaster").val();
    //    var Campus = $("#ddlCampusmaster").val();
    //    var Grade = $("#ddlgrademaster").val();

    //    if (AcademicYear == "" || Campus == "" || Grade == "" || StudentSurvayGroup == "" || StudentSurveyQuestion == "") {
    //        ErrMsg("Please Fill the Required Details");
    //        return false;
    //    }
    //    $.ajax({
    //        Type: 'POST',
    //        dataType: 'json',
    //        url: '/StaffManagement/AddOrEditStudentSurveyQuestion?StudentSurveyGroupId=' + StudentSurvayGroup + '&AcademicYear=' + AcademicYear + '&Campus=' + Campus + '&Grade=' + Grade + '&StudentSurveyQuestion=' + StudentSurveyQuestion + '&IsActive=' + "",
    //        success: function (data) {

    //            $(grid_selector).trigger('reloadGrid');
    //            if (data == "insert") {
    //                $("input[type=text], textarea, select").val("");
    //                SucessMsg("Added Successfully");
    //                return true;
    //            }
    //            else {
    //                $("input[type=text]").val("");
    //                ErrMsg("Already Exist");
    //                return false;
    //            }


    //        }
    //    });

    //});
    //$("#btnReset").click(function () {
    //    $("input[type=text], textarea, select").val("");
    //    $(grid_selector).setGridParam(
    //        {
    //            datatype: "json",
    //            url: '/StaffManagement/GetJqGridstudsurveyList',
    //            postData: { AcademicYear: "", Campus: "", Grade: "", StudentSurveyGroupId: "", StudentSurveyQuestion: "", IsActive: "" },
    //            page: 1
    //        }).trigger("reloadGrid");
    //});
    $("#btnSearch").click(function () {
        var SurveyGoup = $("#ddlStudentSurveyGroup option:selected").val();
        var SurveyQuestion = $("#txtstudentsurveyquestion").val();
        var IsActive = $("#ddlIsActive").val();
        $(grid_selector).clearGridData();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/StaffManagement/GetJqGridsurveyList',
                    postData: { SurveyGroupId: SurveyGoup, SurveyQuestion: SurveyQuestion, IsActive: IsActive },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#SrchbtnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
            {
                datatype: "json",
                url: '/StaffManagement/GetJqGridsurveyList',
                postData: { SurveyGroupId: "", SurveyQuestion: "", IsActive: "" },
                page: 1
            }).trigger("reloadGrid");
    });
    $.getJSON("/StaffManagement/GetSurveyGroupddl/",
                    function (modelData) {
                        var select = $("#ddlStudentSurveyGroup");
                        select.empty();
                        select.append($('<option/>', { value: "", text: "Select One" }));
                        $.each(modelData, function (index, itemData) {
                            select.append($('<option/>', { value: itemData.Value, text: itemData.Text }));
                        });
                    });
});

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

