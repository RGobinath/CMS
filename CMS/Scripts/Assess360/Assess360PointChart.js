var cam = "";
var grade = "";
var section = "";
var grid_selector = "#Assess360PointChartList";
var pager_selector = "#Assess360PointChartListPager";
$(function () {
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
    InitAssess360PointChart(cam, grade, section);
    $("#btnStdnPopup").button({ icons: { primary: "ui-icon-search" },
        text: false
    });

    if ('@Session["emailsent"]' == 'yes') {
        InfoMsg('Email Sent Successfully');
        $.ajax({
            url: '/Assess360/ResetSession',
            type: 'GET',
            dataType: 'json',
            traditional: true
        });
    }

    $.getJSON("/Base/FillBranchCode",
     function (fillbc) {
         var ddlbc = $("#ddlBranchCode");
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

    $("#Search").click(function () {
        cam = $("#ddlBranchCode").val();
        grade = $("#ddlGrade").val();
        section = $("#ddlSection").val();
        if (cam == '') {
            ErrMsg('Please select the Campus.', function () { $("#ddlBranchCode").focus(); });
            return false;
        } if (grade == '') {
            ErrMsg('Please select the Grade.', function () { $("#ddlGrade").focus(); });
            return false;
        } if (section == '') {
            ErrMsg('Please select the Section.', function () { $("#ddlSection").focus(); });
            return false;
        }
        $(grid_selector).GridUnload();
        InitAssess360PointChart(cam, grade, section);
    });

    $("#reset").click(function () {
        $('#ddlBranchCode').val("");
        $('#ddlGrade').val("");
        $('#ddlSection').val("");
        jQuery(grid_selector).jqGrid('clearGridData')
                    .jqGrid('setGridParam', { data: data, page: 1 }).trigger('reloadGrid');
    });

    $("#Email").click(function () {
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = "";

        //  (GridIdList.length);
        if (GridIdList.length > 0) {

            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                if (rowData1 == "") {
                    rowData1 = rowData[i].StudentId;
                }
                else {
                    rowData1 = rowData1 + "," + rowData[i].StudentId;
                }
            }

            ModifiedLoadPopupDynamicaly2("/Assess360/Pointchartmail", $('#EmailDiv'),
            function () {
                $.ajax({
                    url: '/Assess360/Pointchartmail',
                    type: 'GET',
                    dataType: 'json',
                    data: { PreRegNo: rowData1, cam: cam, grade: grade, section: section },
                    traditional: true
                });
            });
        }
    });

    $("#BulkEmail").click(function () {
        ModifiedLoadPopupDynamicaly1("/Assess360/Pointchartbulkmail", $('#EmailDiv1'),
                            function () {
                                $.ajax({
                                    url: '/Assess360/Pointchartbulkmail',
                                    type: 'GET',
                                    dataType: 'json',
                                    data: { cam: cam, grade: grade, section: section },
                                    traditional: true
                                });
                            });
    });

    function ModifiedLoadPopupDynamicaly1(dynURL, ModalId, loadCalBack, onSelcalbck, width) {
        if (width == undefined) { width = 550; }
        if (ModalId.html().length == 0) {
            $.ajax({
                url: dynURL,
                type: 'GET',
                // type: 'POST',
                async: false,
                dataType: 'html', // <-- to expect an html response
                //    data: { ActivityId: rowData1 },
                success: function (data) {
                    ModalId.dialog({
                        height: 150,
                        width: width,
                        modal: true,
                        title: 'Email',
                        buttons: {}
                    });
                    ModalId.html(data);
                }
            });
        }
        clbPupGrdSel = onSelcalbck;
        ModalId.dialog('open');
        CallBackFunction(loadCalBack);
    }

    function ModifiedLoadPopupDynamicaly2(dynURL, ModalId, loadCalBack, onSelcalbck, width) {
        if (width == undefined) { width = 550; }
        if (ModalId.html().length == 0) {
            $.ajax({
                url: dynURL,
                type: 'GET',
                // type: 'POST',
                async: false,
                dataType: 'html', // <-- to expect an html response
                //    data: { ActivityId: rowData1 },
                success: function (data) {
                    ModalId.dialog({
                        height: 150,
                        width: width,
                        modal: true,
                        title: 'Email',
                        buttons: {}
                    });
                    ModalId.html(data);
                }
            });
        }
        clbPupGrdSel = onSelcalbck;
        ModalId.dialog('open');
        CallBackFunction(loadCalBack);
    }

    function InitAssess360PointChart(cam, grade, section) {
        jQuery(grid_selector).jqGrid({
            url: "/Assess360/Assess360PointChartJQGrid",
            postData: { cam: cam, grade: grade, section: section },
            datatype: 'json',
            mtype: 'GET',
            colNames: ['Id', 'S:No', 'StudentId', 'Report Gen. Date', 'Student Name', 'Character', 'Attn / Punctuality', 'HW Completion', 'HW Accuracy', 'Weekly / Chapter Test', 'SLC', 'Term Assessment', 'Total Score'],
            colModel: [
              { name: 'Id', index: 'Id', hidden: true, sortable: true },
              { name: 'Id', index: 'Id', sortable: true },
              { name: 'StudentId', index: 'StudentId', hidden: true, sortable: true },
              { name: 'ReportGenDate', index: 'ReportGenDate', sortable: true },
              { name: 'StudName', index: 'StudName', sortable: false },
              { name: 'Character', index: 'Character', sortable: true },
              { name: 'AttPunctuality', index: 'AttPunctuality', sortable: true },
              { name: 'HwCompletion', index: 'HwCompletion', sortable: true },
              { name: 'HwAccuracy', index: 'HwAccuracy', sortable: true },
              { name: 'WkChapterTests', index: 'WkChapterTests', sortable: true },
              { name: 'SLCParentAssessment', index: 'SLCParentAssessment', sortable: true },
              { name: 'TermAssessment', index: 'TermAssessment', sortable: true },
              { name: 'Total', index: 'Total', sortable: true },
              ],
            pager: pager_selector,
            rowNum: '200',
            rowList: [30, 50, 100, 200],
            sortname: 'Id',
            sortorder: 'Desc',
            height: '230',
            autowidth: true,
            viewrecords: true,
            caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;Assess 360◦ Point Chart',
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(window).triggerHandler('resize.jqGrid');
            }
        });
        //navButtons Add, edit, delete
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
            {}, {}, {})

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
        jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: "Export To Excel",
            onClickButton: function () {
                var ExportType = "Excel";
                window.open("Assess360PointChartJQGrid" + '?cam=' + cam + '&grade=' + grade + '&section=' + section + '&rows=9999' + '&ExportType=' + ExportType);
            }
        });

        jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: "Export To PDF",
            onClickButton: function () {
                var ExportType = "PDF";
                window.open("Assess360PointChartJQGrid" + '?cam=' + cam + '&grade=' + grade + '&section=' + section + '&rows=9999' + '&ExportType=' + ExportType);
            }
        });
    }
});