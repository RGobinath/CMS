// for adding subject
jQuery(function ($) {
    var grid_selector = "#TTSubject";
    var pager_selector = "#TTSubjectPager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $("#TTSubject").jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $("#TTSubject").jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    jQuery(grid_selector).jqGrid({
        url: '/TimeTable/SubjectDetailsJqGrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus',  'Subject', 'Academic Year', 'Subject Details'],
        colModel: [
        //if any column added need to check formateadorLink
             { name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'Campus', index: 'Campus' },
             { name: 'Subject', index: 'Subject', search: false, align: 'center' },
              { name: 'AcademicYear', index: 'AcademicYear', search: false, align: 'center' },
  { name: 'SubjectDetails', index: 'SubjectDetails', search: false, align: 'center' },
        ],
        pager: '#TTSubjectPager',
        rowNum: '20',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'desc',
        width: 1090,
        loadComplete: function () {
            $(window).triggerHandler('resize.jqGrid');
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        viewrecords: true,
        altRows: true,
        select: true,
        viewrecords: true,
        caption: 'Add Subjects'
    });
    $.getJSON("/Base/FillBranchCode",
  function (fillig) {
      var ddlcam = $("#TTCampus");
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
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
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
            }, {}, {}, {}, {}, {})

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
        //
    }

    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }

});

function AddSubject() {
    debugger;
    var Campus = $('#TTCampus').val();
    var Subject = $('#Subject').val();
    var AcaYear = $('#AcaYear').val();

    if (Campus == '' || Subject == '' || AcaYear == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    if (Subject == 'English' || Subject == 'Maths' || Subject == 'History') {
        ErrMsg("Please Enter Staff Name insted of Subject Name.");
        return false;
    }
    $.ajax({
        type: 'POST',
        url: "/TimeTable/AddSubjects",
        data: {
            Campus: Campus, Subject: Subject, AcademicYear: AcaYear
        },
        success: function (data) {
            $("#TTSubject").trigger('reloadGrid');
            $('#Subject').val('');
            $('#AcaYear').val('');
        }
    });
}


function SubjectDetails(Subject) {
    debugger;
    ModifiedLoadPopupDynamicaly("/TimeTable/ShowSubjectDetails?Subject=" + Subject, $('#SubDetails'),
            function () {
                LoadSetGridParam($('#SubjectList'), "/TimeTable/StaffLessonsJqGrid?Subject=" + Subject)
            }, function () { }, 925, 410, "Subject List");
}

