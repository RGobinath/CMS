﻿$(function () {


    var grid_selector = "#Grid";
    var pager_selector = "#Pager";
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

    //pager icon
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
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

    $(grid_selector).jqGrid({
        url: '/Assess360/ShowPastYearFinalResultsJqGrid',
        datatype: 'json',
        type: 'GET',
        autowidth: true,
        height: 250,
        colNames: [],
        colModel: [],
        rowNum: 50,
        rowList: [50, 100, 150, 200],
        sortname: 'Id',
        sortroder: 'Desc',
        viewrecords: true,
        caption: '<i class="fa fa-list">&nbsp;</i>Final Result Sheet',
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });

    $("#ddlSection").change(function () {
        if ($('#ddlCampus').val() == "") {
            ErrMsg("Please fill the Campus");
            return false;
        }
        if ($('#ddlGrade').val() == "") {
            ErrMsg("Please fill the Grade");
            return false;
        }
        if ($('#ddlAcademicYear').val() == "") {
            ErrMsg("Please fill the Academic year");
            return false;
        }

        if ($('#ddlSection').val() == "") {
            ErrMsg("Please fill the Section");
            return false;
        }
        $(grid_selector).GridUnload();
        window.onload = loadgrid();
        function loadgrid() {

            if (($('#ddlGrade').val() == "IX" || $('#ddlGrade').val() == "X") && ($('#ddlSection').val() == "A" || $('#ddlSection').val() == "B")) {
                colnames = "['', 'Student Id', 'Name'"

                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade', 'Grand Total (700)', ' % ', 'Grade']";
                colmodel = "["
                    + "{ name: 'PreRegNum', index: 'PreRegNum', editable: true, hidden: true, key: true }"
                    + ",{ name: 'NewId', index: 'NewId', align: 'center', width: 80, sortable: false }"
                    + ",{ name: 'Name', index: 'Name', width: 2000 }"
                    + ",{ name: 'EngSemI', index: 'EngSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EngSemII', index: 'EngSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EngSemIII', index: 'EngSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EngTotal', index: 'EngTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EngGrade', index: 'EngGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'LangSemI', index: 'LangSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'LangSemII', index: 'LangSemII', align: 'center', width: 40, sortable: false }"
                     + ",{ name: 'LangSemIII', index: 'LangSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'LangTotal', index: 'LangTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'LangGrade', index: 'LangGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'EcoSemI', index: 'EcoSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EcoSemII', index: 'EcoSemII', align: 'center', width: 40, sortable: false }"
                     + ",{ name: 'EcoSemIII', index: 'EcoSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EcoTotal', index: 'EcoTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EcoGrade', index: 'EcoGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'MathsSemI', index: 'MathsSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'MathsSemII', index: 'MathsSemII', align: 'center', width: 40, sortable: false }"
                     + ",{ name: 'MathsSemIII', index: 'MathsSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'MathsTotal', index: 'MathsTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'MathsGrade', index: 'MathsGrade', align: 'center', width: 40, sortable: false }"


                    + ",{ name: 'CombSemI', index: 'CombSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'CombSemII', index: 'CombSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'CombSemIII', index: 'CombSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'CombTotal', index: 'CombTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'CombGrade', index: 'CombGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'IctSemI', index: 'IctSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'IctSemII', index: 'IctSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'IctSemIII', index: 'IctSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'IctTotal', index: 'IctTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'IctGrade', index: 'IctGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'PhyEduSemI', index: 'PhyEduSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'PhyEduSemII', index: 'PhyEduSemII', align: 'center', width: 40, sortable: false }"
                     + ",{ name: 'PhyEduSemIII', index: 'PhyEduSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'PhyEduTotal', index: 'PhyEduTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'PhyEduGrade', index: 'PhyEduGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'GrandTotal', index: 'GrandTotal', align: 'center', width: 50, sortable: false }"
                    + ",{ name: 'Percentage', index: 'Percentage', align: 'center', width: 50, sortable: false }"
                    + ",{ name: 'OverallGrade', index: 'OverallGrade', align: 'center', width: 50, sortable: false }"
                    + "]";
            }
            else if (($('#ddlGrade').val() == "IX" || $('#ddlGrade').val() == "X") && ($('#ddlSection').val() == "C" || $('#ddlSection').val() == "D")) {

                colnames = "['', 'Student Id', 'Name'"
                    + ", 'Sem I (100)', 'Sem II (100)','Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)','Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)','Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)','Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)','Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)','Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)','Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)','Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Grand Total (800)', ' % ', 'Grade']";
                colmodel = "["
                    + "{ name: 'PreRegNum', index: 'PreRegNum', editable: true, hidden: true, key: true }"
                    + ",{ name: 'NewId', index: 'NewId', align: 'center', width: 80, sortable: false }"
                    + ",{ name: 'Name', index: 'Name', width: 90 }"
                    + ",{ name: 'EngSemI', index: 'EngSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EngSemII', index: 'EngSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EngSemIII', index: 'EngSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EngTotal', index: 'EngTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EngGrade', index: 'EngGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'LangSemI', index: 'LangSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'LangSemII', index: 'LangSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'LangSemIII', index: 'LangSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'LangTotal', index: 'LangTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'LangGrade', index: 'LangGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'EcoSemI', index: 'EcoSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EcoSemII', index: 'EcoSemII', align: 'center', width: 40, sortable: false }"
                     + ",{ name: 'EcoSemIII', index: 'EcoSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EcoTotal', index: 'EcoTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EcoGrade', index: 'EcoGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'MathsSemI', index: 'MathsSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'MathsSemII', index: 'MathsSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'MathsSemIII', index: 'MathsSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'MathsTotal', index: 'MathsTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'MathsGrade', index: 'MathsGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'BioSemI', index: 'BioSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'BioSemII', index: 'BioSemII', align: 'center', width: 40, sortable: false }"
                     + ",{ name: 'BioSemIII', index: 'BioSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'BioTotal', index: 'BioTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'BioGrade', index: 'BioGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'PhySemI', index: 'PhySemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'PhySemII', index: 'PhySemII', align: 'center', width: 40, sortable: false }"
                      + ",{ name: 'PhySemIII', index: 'PhySemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'PhyTotal', index: 'PhyTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'PhyGrade', index: 'PhyGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'CheSemI', index: 'CheSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'CheSemII', index: 'CheSemII', align: 'center', width: 40, sortable: false }"
                     + ",{ name: 'CheSemIII', index: 'CheSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'CheTotal', index: 'CheTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'CheGrade', index: 'CheGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'IctSemI', index: 'IctSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'IctSemII', index: 'IctSemII', align: 'center', width: 40, sortable: false }"
                     + ",{ name: 'IctSemIII', index: 'IctSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'IctTotal', index: 'IctTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'IctGrade', index: 'IctGrade', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'GrandTotal', index: 'GrandTotal', align: 'center', width: 50, sortable: false }"
                    + ",{ name: 'Percentage', index: 'Percentage', align: 'center', width: 50, sortable: false }"
                    + ",{ name: 'OverallGrade', index: 'OverallGrade', align: 'center', width: 50, sortable: false }"
                    + "]";
            }
            else {
                colnames = "['', 'Student Id', 'Name'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade'"
                    + ", 'Sem I (100)', 'Sem II (100)', 'Sem III (100)', 'Total (100)', 'Grade', 'Grand Total (800)', ' % ', 'Grade']";
                colmodel = "["
                    + "{ name: 'PreRegNum', index: 'PreRegNum', editable: true, hidden: true, key: true }"
                    + ",{ name: 'NewId', index: 'NewId', align: 'center', width: 80, sortable: false }"
                    + ",{ name: 'Name', index: 'Name', width: 90 }"
                    + ",{ name: 'EngSemI', index: 'EngSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EngSemII', index: 'EngSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EngSemIII', index: 'EngSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EngTotal', index: 'EngTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'EngGrade', index: 'EngGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'LangSemI', index: 'LangSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'LangSemII', index: 'LangSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'LangSemIII', index: 'LangSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'LangTotal', index: 'LangTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'LangGrade', index: 'LangGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'HcSemI', index: 'HcSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'HcSemII', index: 'HcSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'HcSemIII', index: 'HcSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'HcTotal', index: 'HcTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'HcGrade', index: 'HcGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'MathsSemI', index: 'MathsSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'MathsSemII', index: 'MathsSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'MathsSemIII', index: 'MathsSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'MathsTotal', index: 'MathsTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'MathsGrade', index: 'MathsGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'BioSemI', index: 'BioSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'BioSemII', index: 'BioSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'BioSemIII', index: 'BioSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'BioTotal', index: 'BioTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'BioGrade', index: 'BioGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'PhySemI', index: 'PhySemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'PhySemII', index: 'PhySemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'PhySemIII', index: 'PhySemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'PhyTotal', index: 'PhyTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'PhyGrade', index: 'PhyGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'CheSemI', index: 'CheSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'CheSemII', index: 'CheSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'CheSemIII', index: 'CheSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'CheTotal', index: 'CheTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'CheGrade', index: 'CheGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'IctSemI', index: 'IctSemI', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'IctSemII', index: 'IctSemII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'IctSemIII', index: 'IctSemIII', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'IctTotal', index: 'IctTotal', align: 'center', width: 40, sortable: false }"
                    + ",{ name: 'IctGrade', index: 'IctGrade', align: 'center', width: 40, sortable: false }"

                    + ",{ name: 'GrandTotal', index: 'GrandTotal', align: 'center', width: 50, sortable: false }"
                    + ",{ name: 'Percentage', index: 'Percentage', align: 'center', width: 50, sortable: false }"
                    + ",{ name: 'OverallGrade', index: 'OverallGrade', align: 'center', width: 50, sortable: false }"
                    + "]";
            }

            columnName = eval(colnames);
            columnModel = eval(colmodel);

            $(grid_selector).jqGrid({
                url: '/Assess360/ShowPastYearFinalResultsJqGrid',
                postData: { campus: $('#ddlCampus').val(), academicYear: $('#ddlAcademicYear').val(), grade: $('#ddlGrade').val(), section: $('#ddlSection').val() },
                datatype: 'json',
                type: 'GET',
                autowidth: true,
                height: 250,
                colNames: columnName,
                colModel: columnModel,
                rowNum: 50,
                shrinkToFit: true,
                rowList: [50, 100, 150, 200],
                viewrecords: true,
                sortname: 'Id',
                sortorder: 'Desc',
                pager: pager_selector,
                caption: '<i class="fa fa-list">&nbsp;</i>Semester Marks List',
                loadComplete: function () {
                    var table = this;
                    setTimeout(function () {
                        updatePagerIcons(table);
                        enableTooltips(table);
                    }, 0);
                    $(window).triggerHandler('resize.jqGrid');
                }
            });
            jQuery(grid_selector).navGrid(pager_selector, { add: false, edit: false, del: false, refresh: true, refreshicon: 'ace-icon fa fa-refresh green', search: false });
            $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
                caption: "&nbsp;&nbsp;<i class='fa fa-file-excel-o'>&nbsp;</i><u>Export To Excel</u>",
                onClickButton: function () {
                    window.open("ShowPastYearFinalResultsJqGrid" + '?rows=9999' +
                           '&page=1' +
                           '&sidx=Id' +
                           '&sord=desc' +
                           '&campus=' + $('#ddlCampus').val() +
                           '&grade=' + $('#ddlGrade').val() +
                           '&academicYear=' + $('#ddlAcademicYear').val() +
                           '&section=' + $('#ddlSection').val() +
                           '&ExportType=Excel'
                           );
                }
            });
            if ($('#ddlGrade').val() == "IX" && ($('#ddlSection').val() == "A" || $('#ddlSection').val() == "B")) {
                jQuery(grid_selector).jqGrid('setGroupHeaders', {
                    useColSpanStyle: true,
                    groupHeaders: [
                                 { startColumnName: 'EngSemI', numberOfColumns: 5, titleText: '<em>English</em>' },
                                 { startColumnName: 'LangSemI', numberOfColumns: 5, titleText: '<em>Lang</em>' },
                                 { startColumnName: 'EcoSemI', numberOfColumns: 5, titleText: '<em>Economics</em>' },
                                 { startColumnName: 'MathsSemI', numberOfColumns: 5, titleText: '<em>Mathematics</em>' },
                                 { startColumnName: 'CombSemI', numberOfColumns: 5, titleText: '<em>Combined science</em>' },
                                 { startColumnName: 'IctSemI', numberOfColumns: 5, titleText: '<em>ICT</em>' },
                                  { startColumnName: 'PhyEduSemI', numberOfColumns: 5, titleText: '<em>Physical Education</em>' },
                                ]
                });
            }
            else if ($('#ddlGrade').val() == "IX" && ($('#ddlSection').val() == "C" || $('#ddlSection').val() == "D")) {
                jQuery(grid_selector).jqGrid('setGroupHeaders', {
                    useColSpanStyle: true,
                    groupHeaders: [
                 { startColumnName: 'EngSemI', numberOfColumns: 5, titleText: '<em>English</em>' },
                 { startColumnName: 'LangSemI', numberOfColumns: 5, titleText: '<em>Lang</em>' },
                 { startColumnName: 'EcoSemI', numberOfColumns: 5, titleText: '<em>Economics</em>' },
                 { startColumnName: 'MathsSemI', numberOfColumns: 5, titleText: '<em>Mathematics</em>' },
                 { startColumnName: 'BioSemI', numberOfColumns: 5, titleText: '<em>Biology</em>' },
                 { startColumnName: 'PhySemI', numberOfColumns: 5, titleText: '<em>Physics</em>' },
                 { startColumnName: 'CheSemI', numberOfColumns: 5, titleText: '<em>Chemistry</em>' },
                 { startColumnName: 'IctSemI', numberOfColumns: 5, titleText: '<em>ICT</em>' },
                ]
                });
            }
            else {
                jQuery(grid_selector).jqGrid('setGroupHeaders', {
                    useColSpanStyle: true,
                    groupHeaders: [
                 { startColumnName: 'EngSemI', numberOfColumns: 5, titleText: '<em>English</em>' },
                 { startColumnName: 'LangSemI', numberOfColumns: 5, titleText: '<em>Lang</em>' },
                 { startColumnName: 'HcSemI', numberOfColumns: 5, titleText: '<em>HC&C</em>' },
                 { startColumnName: 'MathsSemI', numberOfColumns: 5, titleText: '<em>Mathematics</em>' },
                 { startColumnName: 'BioSemI', numberOfColumns: 5, titleText: '<em>Biology</em>' },
                 { startColumnName: 'PhySemI', numberOfColumns: 5, titleText: '<em>Physics</em>' },
                 { startColumnName: 'CheSemI', numberOfColumns: 5, titleText: '<em>Chemistry</em>' },
                 { startColumnName: 'IctSemI', numberOfColumns: 5, titleText: '<em>ICT</em>' },
                ]
                });
            }
        }
    });

    $("#search").click(function () {

        //var campus = $('#ddlCampus').val();
        //var grade = $('#ddlGrade').val();
        //var section = $('#ddlSection').val();

        if ($('#ddlCampus').val() == "") {
            ErrMsg("Please fill the Campus");
            return false;
        }
        if ($('#ddlGrade').val() == "") {
            ErrMsg("Please fill the Grade");
            return false;
        }
        if ($('#ddlSection').val() == "") {
            ErrMsg("Please fill the Section");
            return false;
        }
        $(grid_selector).setGridParam(
             {
                 datatype: "json",
                 url: "/Assess360/test",
                 type: 'POST',
                 postData: { campus: $('#ddlCampus').val(), grade: $('#ddlGrade').val(), section: $('#ddlSection').val() },
                 page: 1
             }).trigger("reloadGrid");

    });

    //    $("#reset").click(function () {
    //        window.location.href = '@Url.Action("FinalResult", "Assess360")';
    //    });


         $('#reset').click(function () {
             var url = $("#BackUrl").val();
             window.location.href = url;
         });


//    $('#reset').click(function () {
//        $('#ddlCampus').val("");
//        $('#ddlGrade').val("");
//        $('#ddlAcademicYear').val("");
//        $('#ddlSection').val("");
//        jQuery(grid_selector).jqGrid('clearGridData')
//                     .jqGrid('setGridParam', { data: data, page: 1 })
//                     .trigger('reloadGrid');
   // });
    // fill the branch code or campus from the base controller....

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

    $('#ddlAcademicYear').change(function () {
        $('#ddlSection').find('option:first').attr('selected', 'selected');
    });

});


function gradeddl() {
    //  var campus = $("#ddlCampus").val();
    $.getJSON("/Admission/CampusGradeddl/", { campus: $("#ddlCampus").val() },
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