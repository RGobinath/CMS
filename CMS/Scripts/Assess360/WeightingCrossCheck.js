$(function () {

    var grid_selector = "#WeightingCrossCheck";
    var pager_selector = "#WeightingCrossCheckPager";
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

    //replace icons with FontAwesome icons like above
    

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    $(grid_selector).jqGrid({
        url: "/Assess360/WeightingCrossCheckJqGrid",
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Grade', 'Academic Year', 'Subject', 'Assignment Name', 'Weightings', 'TotalMarks', 'IsMatched with Assignment'],
        colModel: [
               { name: 'Id', index: 'Id', hidden: true, key: true },
               { name: 'Campus', index: 'Campus' },
               { name: 'Grade', index: 'Grade' },
               { name: 'AcademicYear', index: 'AcademicYear' },
               { name: 'Subject', index: 'Subject' },
               { name: 'ComponentTitle', index: 'ComponentTitle'},
               { name: 'Weightings', index: 'Weightings'},
               { name: 'TotalMarks', index: 'TotalMarks'},
               { name: 'IsMatched', index: 'IsMatched'},
        ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '250',
        closeAfterEdit: true,
        closeAfterAdd: true,
        autowidth: true,
        // shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="fa fa-list">&nbsp;</i>Weighting Cross Check',
        forceFit: true,
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        }
    });
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
        }, {}, {}, {}
     );
    jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: '&nbsp;<i class="fa fa-file-excel-o fa-lg blue"></i>&nbsp;<u>Export To Excel<u/>',
        onClickButton: function () {
            window.open("WeightingCrossCheckJqGrid" + '?Campus=' + $("#ddlCampus").val() + '&Grade=' + $("#ddlGrade").val() + '&AcademicYear=' + $("#ddlyear").val() + '&Subject=' + $("#Subject").val() + '&rows=9999' + '&ExptXl=1');
        }
    });
    $.getJSON("/Base/FillBranchCode",
         function (fillig) {
             var srchddlcam = $("#ddlCampus");
             srchddlcam.empty();
             srchddlcam.append($('<option/>', { value: "", text: "Select One" }));
             $.each(fillig, function (index, itemdata) {
                 srchddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
             });
         });

    $("#Search").click(function () {

        $(grid_selector).clearGridData();
        var cam = $("#ddlCampus").val();
        var gra = $("#ddlGrade").val();
        var sub = $("#Subject").val();
        var acyear  = $("#ddlyear").val();
        $(grid_selector).setGridParam(
            {
                datatype: "json",
                url: "/Assess360/WeightingCrossCheckJqGrid/",
                postData: { Campus: cam, Grade: gra, AcademicYear: acyear, Subject: sub },
                page: 1
            }).trigger("reloadGrid");

    });

    $("#ddlGrade").change(function () {
        GetSubjectsByGrade($(this).val());
    });


    $("#ddlCampus").change(function () {
        gradeddl($("#ddlCampus").val());
    });
    $("#reset").click(function () {
        $("#ddlCampus").val('');
        $("#ddlGrade").val('');
        $("#AssSubject").val('');
        $("#ddlyear").val('');
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

function gradeddl(campus) {
    //var campus = $("#ddlCampus").val();
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
        function (modelData) {
            var select = $("#ddlGrade");
            select.empty();
            select.append($('<option/>', { value: "", text: "Select Grade" }));
            $.each(modelData, function (index, itemData) {
                select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
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

function GetSubjectsByGrade(grade) {
    var srchsubddl = $("#Subject");
    $.ajax({
        type: 'GET',
        async: false,
        dataType: "json",
        url: '/Assess360/GetSubjectsByGrade?Grade=' + grade,
        success: function (data) {
            srchsubddl.empty();
            srchsubddl.append("<option value=''> Select </option>");
            for (var i = 0; i < data.rows.length; i++) {
                srchsubddl.append("<option value='" + data.rows[i].Value + "'>" + data.rows[i].Text + "</option>");
            }
        },
        error: function (xhr, status, error) {
            ErrMsg($.parseJSON(xhr.responseText).Message);
        }
    });
}


