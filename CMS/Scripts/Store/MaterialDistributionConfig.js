var grid_selector = "#materialDistributionListjqGrid";
var pager_selector = "#materialDistributionListjqGridPager";

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
$(function () {

    $('#ddlCampus').change(function () {
        if ($('#ddlCampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        gradeddl();
    });

    FillMaterialSubGroup();

    $("#ddlMaterialSubGroup").change(function () {
        if ($("#ddlMaterialGroup").val() == "" && $("#ddlMaterialSubGroup").val() == "")
            $("#ddlMaterial").empty();
        else
            FillMaterial();
    });

    $("#btnSearch").click(function () {

        $(grid_selector).clearGridData();
        StoreCampus = $("#ddlCampus").val();
        Material = $("#txtMaterial").val();
        MaterialGroup = $("#txtMaterialGroup").val();
        MaterialSubGroup = $("#txtMaterialSubGroup").val();
        AMonth = $("#ddlmonth").val();
        AYear = $("#ddlyear").val();
        OpeningBalance = $("#txtOpeningBalance").val();
        Inward = $("#txtInward").val();
        Outward = $("#txtOutward").val();
        ClosingBalance = $("#txtClosingBalance").val();
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Store/MaterialsInOutJqGrid/',
                    postData: { StoreCampus: StoreCampus, MaterialGroup: MaterialGroup, MaterialSubGroup: MaterialSubGroup, Material: Material, AMonth: AMonth, AYear: AYear, OpeningBalance: OpeningBalance, Inward: Inward, Outward: Outward, ClosingBalance: ClosingBalance },
                    page: 1
                }).trigger("reloadGrid");
    });

    $('#btnReset').click(function () {
        var url = $('#BackUrl').val();
        window.location.href = url;
    });

    $('#btnAdd').click(function () {
        
        AddMaterialDistribution();
    });
    $("#btnsearch").click(function () {
        $("#materialDistributionListjqGrid").clearGridData();
         var AcademicYear = $("#ddlAcademicYear").val();
         var Campus = $("#ddlCampus").val();
         var Grade = $("#ddlgrade").val();
         var Section = $("ddlsection").val();
         var Gender = $("#ddlGender").val();
         var IsHostler = $("#ddlIshosteller").val();
         var MaterialGroup = $("#ddlMaterialGroup").val();         
         var Quantity = $("#txtQuantity").val();

         var Material = $("#ddlMaterialSubGroup option:selected").text();
         Material = Material == "Select" ? "" : Material;
         var MaterialId = $("#ddlMaterialSubGroup option:selected").val();

        $("#materialDistributionListjqGrid").setGridParam(
                {
                    datatype: "json",
                    url: '/Store/MaterialsDistribution_vwListJqGrid/',
                    
                    postData: { AcademicYear: AcademicYear, Campus: Campus, Grade: Grade, Section: Section, Gender: Gender, IsHosteller: IsHostler, MaterialGroupId: MaterialGroup, MaterialSubGroup: Material, Quantity: Quantity },
                    page: 1
                }).trigger("reloadGrid");
    });
    

    $("#btnreset").click(function () {
        
        $("input[type=text], textarea, select").val("");
        
        debugger;
        $("#materialDistributionListjqGrid").setGridParam(
            {

                datatype: "json",
                url: '/Store/MaterialsDistribution_vwListJqGrid/',
                postData: { AcademicYear: "", Campus: "", Grade: "", Section:"",Gender: "", IsHosteller: "", MaterialGroupId: "", MaterialSubGroup: "", Material: "", Quantity: "" },
                page: 1
            }).trigger("reloadGrid");
    });
    jQuery(grid_selector).jqGrid({
        url: '/Store/MaterialsDistribution_vwListJqGrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'AcademicYear', 'Campus', 'Grade', 'Section', 'Gender', 'Is Hosteller', 'MaterialSubGroupId', 'Material', 'Quantity', '', ''],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'AcademicYear', index: 'AcademicYear', width: 90 },
              { name: 'Campus', index: 'Campus', width: 90 },
              { name: 'Grade', index: 'Grade', width: 90 },
              { name: 'Section', index: 'Section', width: 90, hidden: true },
              { name: 'Gender', index: 'Gender', width: 90 },
              { name: 'IsHosteller', index: 'IsHosteller', width: 90 },
            
              { name: 'MaterialSubGroupId', index: 'MaterialSubGroupId', width: 90, hidden:true  },
              { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 90 },
              { name: 'Quantity', index: 'Quantity', width: 90},
              { name: 'CreatedBy', index: 'CreatedBy', width: 90 ,hidden:true},
              { name: 'CreatedDate', index: 'CreatedDate', width: 90,hidden:true },
        ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '220',
        //width: 1225,
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="fa fa-exchange"></i> Material Distribution Configuration List',
        forceFit: true
    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    //$(grid_selector).jqGrid('filterToolbar', { searchOnEnter: false, enableClear: true })
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


    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
        onClickButton: function () {
            Material = $("#txtMaterial").val();
            MaterialGroup = $("#txtMaterialGroup").val();
            MaterialSubGroup = $("#txtMaterialSubGroup").val();
            AMonth = $("#ddlmonth").val();
            AYear = $("#ddlYear").val();
            OpeningBalance = $("#txtOpeningBalance").val();
            Inward = $("#txtInward").val();
            Outward = $("#txtOutward").val();
            ClosingBalance = $("#txtClosingBalance").val();
            window.open("MaterialsInOutJqGrid" + '?Material=' + Material +
                '&MaterialGroup=' + MaterialGroup +
                '&MaterialSubGroup=' + MaterialSubGroup +
                '&AMonth=' + AMonth + '&AYear=' + AYear + '&OpeningBalance=' + OpeningBalance + '&Inward=' + Inward + '&Outward=' + Outward + '&ClosingBalance' + ClosingBalance + '&rows=9999' + '&ExptXl=1');
        }
    });
});

function AddMaterialDistribution() {
    var acaYear = $("#ddlAcademicYear").val();
    var campus = $("#ddlCampus").val();
    var grade = $("#ddlgrade").val();
    var Section = $("#ddlSection").val();   
    var Gender = $("#ddlGender option:selected").val();
    var Ishosteller = $("#ddlIshosteller option:selected").val(); 
   
    var Material = $("#ddlMaterialSubGroup option:selected").text();    
    var MaterialId = $("#ddlMaterialSubGroup option:selected").val();
    
    var Quantity = $("#txtQuantity").val();

    
    if (acaYear == '' || campus == '' || grade == '' ||Gender == '' || Ishosteller == '' || Quantity == '') {
        ErrMsg("Please fill all the mandatory fields.");
        return false;
    }
    
    $.ajax({
        type: 'POST',
        url: '/Store/SaveMaterialDistributionConfig',
        data: {
            AcademicYear: acaYear, Campus: campus, Grade: grade, Section: Section, Gender: Gender, IsHosteller: Ishosteller,
            MaterialSubGroupId: MaterialId, MaterialSubGroup: Material, Quantity: Quantity
        },
        
        success: function (data) {
            if (data == "insert") {
                SucessMsg("Added Sucessfully");
            }
            if (data == "Failed") {
                ErrorMsg("Already Exist");
            }
            $("input[type=text], textarea, select").val("");
            //$("#VehicleElectricalMaintenanceList").trigger('reloadGrid');
            $(grid_selector).trigger('reloadGrid');
            //UploadEM_SparePartsUsedfile(Id);
        }
    });
}

function FillMaterial() {
    var matgrp = $("#ddlMaterialGroup").val();
    var matsubgrp = $("#ddlMaterialSubGroup").val();
    $.ajax({
        type: 'POST',
        async: false,
        url: '/Store/FillMaterial?MaterialGroupId=' + matgrp + "&MaterialSubGroupId=" + matsubgrp,
        success: function (data) {
            $("#ddlMaterial").empty();
            $("#ddlMaterial").append("<option value=''> Select One </option>");
            for (var i = 0; i < data.length; i++) {
                $("#ddlMaterial").append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
            }
        },
        dataType: "json"
    });
}

function FillMaterialSubGroup() {
    var ddlbc = $("#ddlMaterialSubGroup");
    if ($("#ddlMaterialGroup").val() != "") {
        $.getJSON("/Store/FillMaterialSubGroup?MaterialGroupId=" + $("#ddlMaterialGroup").val(),
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}

function gradeddl() {

    var e = document.getElementById('ddlCampus');
    var campus = e.options[e.selectedIndex].value;
    //alert(campus);
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
                function (modelData) {
                    var select = $("#ddlgrade");
                    select.empty();
                    select.append($('<option/>', { value: "", text: "Select Grade" }));
                    $.each(modelData, function (index, itemData) {
                        select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                    });
                });
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
function enableTooltips(table) {
    $('.navtable .ui-pg-button').tooltip({ container: 'body' });
    $(table).find('.ui-pg-div').tooltip({ container: 'body' });
}