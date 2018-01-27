var grid_selector = "#grid-table";
var pager_selector = "#grid-pager";

$(window).on('resize.jqGrid', function () {
    $(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
})

//resize to fit page size

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

$(function () {
        $.getJSON("/Base/FillBranchCode",
             function (fillig) {
                 var ddlcam = $("#ddlCampus");
                 ddlcam.empty();
                 ddlcam.append($('<option/>',
                {
                    value: "",
                    text: "Select One"
                }));
                 $.each(fillig, function (index, itemdata) {
                     ddlcam.append($('<option/>',
                         {
                             value: itemdata.Value,
                             text: itemdata.Text
                         }));
                 });
             });

        $("#ddlCampus").change(function () {
            gradeddl();
        });

        $(grid_selector).jqGrid({
            url: '/Home/AdmissionCountListJqGrid',
            datatype: 'json',
            type: 'GET',
            colNames: ['Id', 'Academic Year', 'Campus', 'Grade', 'Section', 'Total Count', 'Vacancy', 'Max Allowed Strength'],
            colModel: [
                 { name: 'Id', index: 'Id', hidden: true, key: true },
                 { name: 'AcademicYear', index: 'AcademicYear', sortable: true },
                 { name: 'Campus', index: 'Campus', sortable: true },
                 { name: 'Grade', index: 'Grade', sortable: true },
                 { name: 'Section', index: 'Section', sortable: true },
                 { name: 'TotalCount', index: 'TotalCount', sortable: true },
                 { name: 'Vacancy', index: 'Vacancy', sortable: true },
                 { name: 'i', index: 'i', sortable: true },
            ],
            pager: pager_selector,
            rowNum: '10',
            rowList: [5, 10, 20, 50, 100, 150, 200],
            sortname: 'Id',
            sortorder: 'Desc',
            height: '240',
            viewrecords: true,
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
            caption: '<i class="menu-icon fa fa-pencil-square-o"></i>&nbsp;&nbsp;Admission Count List'
        });
        $(window).triggerHandler('resize.jqGrid');
        jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options

                edit: false,
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
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {},
            {}, {}, {});
        jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
            onClickButton: function () {
                acayear = $("#ddlacademicyear").val();
                campname = $("#ddlCampus").val();
                grade = $("#ddlGrade").val();
                section = $("#ddlSection").val();
                window.open("AdmissionCountListJqGrid" + '?acayear=' + acayear + '&campname=' + campname + '&grade=' + grade + '&section=' + section + '&rows=9999' + '&ExptType=Excel');
            }
        });

        $("#Search").click(function () {

            $(grid_selector).clearGridData();
            acayear = $("#ddlacademicyear").val();
            campname = $("#ddlCampus").val();
            grade = $("#ddlGrade").val();
            section = $("#ddlSection").val();
            $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Home/AdmissionCountListJqGrid/',
                    postData: { acayear: acayear, campname: campname, grade: grade, section: section },
                    page: 1
                }).trigger("reloadGrid");
        });
            $('#reset').click(function () {
                var url = $('#BackUrl').val();
                window.location.href = url;
            });

    });
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

    function gradeddl() {

        var campus = $("#ddlCampus").val();
        $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
            function (modelData) {
                var select = $("#ddlGrade");
                select.empty();
                select.append($('<option/>'
                               , {
                                   value: "",
                                   text: "Select Grade"
                               }));
                $.each(modelData, function (index, itemData) {

                    select.append($('<option/>',
                                  {
                                      value: itemData.gradcod,
                                      text: itemData.gradcod
                                  }));
                });
            });
    }
   