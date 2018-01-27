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

        url: '/Asset/StudentLaptopDistributionJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['Id', 'studentId', 'New Id', 'Name', 'Grade', 'TransactionType', 'Received Date', 'Lap Code', 'Laptop Type', 'Windows OS'],
        colModel: [
                                  { name: 'Id', index: 'Id', hidden: true },
                                  { name: 'studentId', index: 'Id', key: true, hidden: true },
                                  { name: 'NewId', index: 'NewId', sortable: true },
                                  { name: 'Name', index: 'Name', sortable: true },
                                  { name: 'Grade', index: 'Grade', sortable: true },
                                  { name: 'TransactionType', index: 'TransactionType', sortable: true },
                                  { name: 'ReceivedDate', index: 'ReceivedDate', sortable: true },
                                  { name: 'AssetCode', index: 'AssetCode', sortable: true },
                                  { name: 'LTSize', index: 'LTSize', sortable: true },
                                  { name: 'OperatingSystemDtls', index: 'OperatingSystemDtls', sortable: true },
                                  

        ],

        viewrecords: true,
        rowNum: 10000,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        sortname: 'NewId',
        sortorder: 'Asc',
        altRows: true,
        multiselect: true,
        userDataOnFooter: true,
        multiboxonly: true,
        loadComplete: function () {
            var ids = jQuery(grid_selector).jqGrid('getDataIDs');
            var total = 0;

            for (var i = 0; i < ids.length; i++) {
                rowData = jQuery(grid_selector).jqGrid('getRowData', ids[i]);
                if (rowData.TransactionType == "Distributed") {
                    total = total + 1;
                }
            }
            //$('#txtTotal').val(total);
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp; Students For Distribution",
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

    $("#grid-table").navButtonAdd('grid-pager', {
        caption: "Distribute Laptop",
        title: "Click here to Distribute Laptop",
        buttonicon: "ace-icon fa fa-plus-circle purple",
        onClickButton: function () {
            var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
            var rowData = [];
            var rowData1 = [];
            var MainrowData1 = "";

            if ($('#ddlacademicyear').val() == "") { ErrMsg("Please fill the Academic Year"); return false; }
            if ($('#ddlCampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
            if ($('#ddlGrade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
            if ($('#ddlScreenSize').val() == "") { ErrMsg("Please fill the Laptop Type"); return false; }
            if ($('#ddlOperatingSystem').val() == "") { ErrMsg("Please fill the Windows OS"); return false; }

            //--Assigning Academic Year
            var a = document.getElementById("ddlacademicyear");
            var academicyr = a.options[a.selectedIndex].value;
            //--Assigning Campus
            var b = document.getElementById("ddlCampus");
            var campus = b.options[b.selectedIndex].value;
            //--Assigning Grade
            var c = document.getElementById("ddlGrade");
            var grade = c.options[c.selectedIndex].value;
            //--Assigning Grade
            var d = document.getElementById("ddlSection");
            var section = d.options[d.selectedIndex].value;
            //--Assigning ScreenSize
            var e = document.getElementById("ddlScreenSize");
            var ScreenSize = e.options[e.selectedIndex].value;
            //--Assigning OperatingSystemDtls
            var f = document.getElementById("ddlOperatingSystem");
            var OperatingSystem = f.options[f.selectedIndex].value;

            

            if (GridIdList.length > 0) {
                for (i = 0; i < GridIdList.length; i++) {
                    rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);
                    rowData1[i] = rowData[i].studentId;
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
                    data: { AcademicYear: academicyr, Campus: campus, Grade: grade, Section: section, ScreenSize: $('#ddlScreenSize').val(), OperatingSystem: $('#ddlOperatingSystem').val() },
                    success: function (data) {
                        if (data == "success") {
                            SucessMsg('Success');
                            reloadGrid();
                            return true;
                        }
                        else { ErrMsg('There is no Stock Available'); return false }
                    }
                });
            }
            else {
                ErrMsg("Please Select List");
                return false;
            }
        },
        position: "first"
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
    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });


    //--Academic Year Change 
    $("#ddlacademicyear").change(function () {
        reloadGrid();
    });

    //--Grade Loading
    $("#ddlCampus").change(function () {
        FillddlGradeByCampus();
    });

    //--Search
    $('#btnSearch').click(function () {
        if ($('#ddlacademicyear').val() == "") { ErrMsg("Please fill the Academic Year"); return false; }
        if ($('#ddlCampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        
    //--Lap Distribution Count
        GetDistributionCount();
        $(grid_selector).setGridParam(
                  {
                      datatype: "json",
                      url: "/Asset/StudentLaptopDistributionJqGrid",
                      postData: { AcademicYear: $('#ddlacademicyear').val(), Campus: $('#ddlCampus').val(), Grade: $('#ddlGrade').val(), Section: $('#ddlSection').val(), NewId: $('#txtId').val(), SS: $('#ddlScreenSize').val(), OS: $('#ddlOperatingSystem').val() },
                      page: 1
                  }).trigger('reloadGrid');
    });

    //--Reset 
    $('#btnReset').click(function () {
        $("input[type=text], textarea, select").val("");
        FillddlGradeByCampus();
        reloadGrid();
    });

    function GetDistributionCount() {

        $.ajax({
            type: 'POST',
            dataType: "json",
            async: true,
            url: "/Asset/GetDistributionCount",
            data: { AcademicYear: $('#ddlacademicyear').val(), Campus: $('#ddlCampus').val(), Grade: $('#ddlGrade').val(), Section: $('#ddlSection').val(), RollNo: $('#txtId').val() },
            success: function (data) {
                $('#txtTotal').val(data);
            }
        });
    }
});
// -- Campus Loading
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


function reloadGrid() {
    if ($("#ddlacademicyear").val() != "" && $("#ddlCampus").val() != "") {
        $('#grid-table').setGridParam(
             {

                 datatype: "json",
                 url: '/Asset/StudentLaptopDistributionJqGrid',
                 postData: { AcademicYear: $('#ddlacademicyear').val(), Campus: $('#ddlCampus').val(), Grade: $('#ddlGrade').val(), Section: $('#ddlSection').val(), NewId: $('#txtId').val(), SS: $('#ddlScreenSize').val(), OS: $('#ddlOperatingSystem').val() },
                 page: 1
             }).trigger("reloadGrid");
    }

}

function FillddlGradeByCampus() {
   
    var Campus = $("#ddlCampus").val();
    var ddlbc = $("#ddlGrade");
    if (Campus != "") {
        $.getJSON("/Base/FillGradesWithArrayCriteria?campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select One" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });
        reloadGrid();
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select One" }));
    }
}

