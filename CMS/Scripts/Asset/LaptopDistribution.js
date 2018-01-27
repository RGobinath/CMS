// -- Campus DDl Loading
$.getJSON("/Base/FillBranchCode",
  function (fillig) {

      var ddlcam = $("#Campus");
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
//--Grid Loading
jQuery(function ($) {
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

    jQuery(grid_selector).jqGrid({

        url: '/Asset/LaptopDistributionJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['Id', 'NewId', 'Name', 'Campus', 'Grade', 'Section'],
        colModel: [
                                  { name: 'Id', index: 'Id', key: true, hidden: true },
                                  { name: 'NewId', index: 'NewId', sortable: true },
                                  { name: 'Name', index: 'Name', sortable: true },
                                  { name: 'Campus', index: 'Campus', sortable: false },
                                  { name: 'Grade', index: 'Grade', sortable: false },
                                  { name: 'Section', index: 'Section', sortable: false },

        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        sortname: 'Name',
        sortorder: 'Asc',
        altRows: true,
        multiselect: true,
        userDataOnFooter: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);

        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp; Students Distribution",
    });

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size    
    //navButtons
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
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {}, //Edit
            {}, //Add
            {},
            {},
            {});
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
    //--Campus onclick loading Grade
    $("#Campus").change(function () {
        FillddlGradeByCampus();
    });

    $('#btnGetStdntLst').click(function () {
        if ($('#ddlacademicyear').val() == "") { ErrMsg("Please fill the Academic Year"); return false; }
        if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#Grade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
        if ($('#Section').val() == "") { ErrMsg("Please fill the Section"); return false; }

        $(grid_selector).setGridParam(
                  {
                      datatype: "json",
                      url: "/Asset/LaptopDistributionJqGrid",
                      postData: { academicyear: $('#ddlacademicyear').val(), campus: $('#Campus').val(), grade: $('#Grade').val(), section: $('#Section').val() },
                      page: 1
                  }).trigger('reloadGrid');
    });
    $('#btnReset').click(function () {
        $("input[type=text], textarea, select").val("");
        FillddlGradeByCampus();
        $('#grid-table').setGridParam(
         {
             datatype: "json",
             url: '/Asset/LaptopDistributionJqGrid',
             postData: { academicyear: "", campus: "", grade: "", section: "" },
             page: 1
         }).trigger("reloadGrid");
    });

    $("#distributeBtn").click(function () {

        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];
        var MainrowData1 = "";

        //--Assigning Academic Year
        var a = document.getElementById("ddlacademicyear");
        var academicyr = a.options[a.selectedIndex].value;

        //--Assigning Campus
        var b = document.getElementById("Campus");
        var campus = b.options[b.selectedIndex].value;

        //--Assigning Grade
        var c = document.getElementById("Grade");
        var grade = c.options[c.selectedIndex].value;

        //--Assigning Grade
        var d = document.getElementById("Section");
        var section = d.options[d.selectedIndex].value;

        if (GridIdList.length > 0) {

            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);

                rowData1[i] = rowData[i].Id;
                if (MainrowData1 != "") {
                    MainrowData1 = MainrowData1 + ',' + rowData1[i];
                }
                else {
                    MainrowData1 = rowData1[i];
                }
            }
            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                async: false,
                url: '/Asset/LaptopDistributionProcess?sId=' + MainrowData1,
                data: { AcademicYear: academicyr, Campus: campus, Grade: grade, Section: section },
                success: function (data) {
                    if (data == "success") {
                        SucessMsg('Success!!');
                        reloadGrid();
                        return true;
                    }
                    else {
                        ErrMsg('There is no Stock Available');
                        return false
                    }
                }
            });
        }
        else {
            ErrMsg("Please Select List!!");
            return false;
        }
    });
});

function reloadGrid() {
    $('#grid-table').setGridParam(
         {
             datatype: "json",
             url: '/Asset/LaptopDistributionJqGrid',
             postData: { academicyear: $('#ddlacademicyear').val(), campus: $('#Campus').val(), grade: $('#Grade').val(), section: $('#Section').val() },
             page: 1
         }).trigger("reloadGrid");
}

function FillddlGradeByCampus() {
    var Campus = $("#Campus").val();
    var ddlbc = $("#Grade");
    if (Campus != "") {
        $.getJSON("/Base/FillGradesWithArrayCriteria?campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select One" }));
    }
}