jQuery(function ($) {

    
    var grid_selector = "#VehicleCostDetailsJqGrid";
    var pager_selector = "#VehicleCostDetailsJqGridPager";
    var VehicleCost;
    

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
    var oldAddRowData = $.fn.jqGrid.addRowData;

    $.jgrid.extend({
        addRowData: function (rowid, rdata, pos, src) {
            //  alert(rowid);
            //  debugger;
            //   if (pos === 'afterSelected' || pos === 'beforeSelected') {
            if (typeof src === 'undefined' && this[0].p.selrow !== null) {
                src = this[0].p.selrow;
                pos = (pos === "first") ? 'after' : 'before';
                return oldAddRowData.call(this, rowid, rdata, pos, src);
            } else {
                //pos = (pos === "afterSelected") ? 'last' : 'first';

                alert("Please select row")
            }
            //  }

        }
    });
    var VehicleId = $("#hdnVehicleId").val();
   
    jQuery(grid_selector).jqGrid({
       
        url: '/Transport/VehicleCostDetailsJqGrid?VehicleId=' + VehicleId,
        datatype: 'json',
        height: 200,
        colNames: ['ID', 'Vehicle Cost Id', 'Vehicle Id', 'IndividualDate', 'Campus', 'Type Of Trip', 'Vehicle No', 'Driver Name', 'Helper Name', 'Vehicle Route', 'Start Kms', 'End Kms', 'Driver OT', 'Helper OT', 'Diesel', 'Maintenance', 'Created Date', 'Created By', 'Modified Date', 'Modified By', 'Add', 'Edit'],
        colModel: [


              { name: 'Id', index: 'Id', editable: true, hidden: true, key: true },
            { name: 'VehicleCostId', index: 'VehicleCostId', editable: true, hidden: true },
            { name: 'VehicleId', index: 'VehicleId', editable: true, hidden: true },
             {
                 name: 'IndividualDate', index: 'IndividualDate', editable: true,

                 formoptions: { rowpos: 3, colpos: 3 }, editoptions: {
                     dataInit: function (el) {
                         $(el).keypress(function (e) {
                             if (e.which != 8 && e.which != 0 && e.which != 47 && (e.which < 48 || e.which > 57)) {
                                 return false;
                             }
                         });
                         var elid = el.id;
                         elid = elid.slice(0, -11);
                         $(el).datepicker({
                             format: 'mm/dd/yyyy',
                             autoclose: true
                         });

                     }
                 }
             },



             {
                 name: 'Campus', index: 'Campus', editable: true, edittype: 'select',
                 editoptions: { dataUrl: '/Assess360/GetCampusddl' },
                 editrules: { required: true, custom: true, custom_func: checkvalid },
                 sortable: true
             },


               {
                   name: 'TypeOfTrip', index: 'TypeOfTrip', editable: true, edittype: 'select',
                   editoptions: { dataUrl: '/Transport/TripTypeddl' },
                   editrules: { required: true, custom: true, custom_func: checkvalid },
                   sortable: true
               },
              { name: 'VehicleNo', index: 'VehicleNo', width: 150, editable: true, sortable: false, editoptions: { size: "20", maxlength: "30" } },
             { name: 'DriverName', index: 'DriverName', width: 150, editable: true, sortable: false, editoptions: { size: "20", maxlength: "30" } },
              { name: 'HelperName', index: 'HelperName', width: 150, editable: true, sortable: false, editoptions: { size: "20", maxlength: "30" } },
               { name: 'VehicleRoute', index: 'VehicleRoute', width: 150, editable: true, sortable: false, editoptions: { size: "20", maxlength: "30" } },
              { name: 'StartKmrs', index: 'StartKmrs', width: 150, editable: true, sortable: false, editoptions: { size: "20", maxlength: "30" } },
               { name: 'EndKmrs', index: 'EndKmrs', width: 150, editable: true, sortable: false, editoptions: { size: "20", maxlength: "30" } },
            { name: 'DriverOt', index: 'DriverOt', width: 150, editable: true, sortable: false, editoptions: { size: "20", maxlength: "30" } },
               { name: 'HelperOt', index: 'HelperOt', width: 150, editable: true, sortable: false, editoptions: { size: "20", maxlength: "30" } },
              { name: 'Diesel', index: 'Diesel', width: 150, editable: true, sortable: false, editoptions: { size: "20", maxlength: "30" } },
               { name: 'Maintenance', index: 'Maintenance', width: 150, editable: true, sortable: false, editoptions: { size: "20", maxlength: "30" } },
                { name: 'CreatedBy', index: 'CreatedBy', editable: true, hidden: true },
               { name: 'CreatedDate', index: 'CreatedDate', editable: true, search: false, hidden: true },
                 { name: 'ModifiedBy', index: 'ModifiedBy', editable: true, hidden: true },
           { name: 'ModifiedDate', index: 'ModifiedDate', editable: true, search: false, hidden: true },




         {
             name: 'Add', index: 'Add', search: false, align: 'center'

         },
         {
             name: 'Edit', index: 'Edit', search: false, formatoptions: { keys: false, editbutton: true, delbutton: false }, formatter: 'actions'


         },


        ],


        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        autowidth: true,
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
        editurl: '/Transport/SaveOrUpdateVehicleCostDetails',

        caption: "<i class='ace-icon fa fa-truck'> </i>&nbsp;Vehicle Cost Details"



    });
    jQuery(document).ready(function () {
        jQuery("#VehicleCostDetailsJqGrid").jqGrid('jqPivot', "data.json", // pivot options
            {

                aggregates: [
                    { member: 'Price', aggregator: 'sum', width: 80, formatter: 'number', align: 'right', summaryType: 'sum' }
                ],
                rowTotals: true, colTotals: true
            }, // grid options 
            { width: 700, rowNum: 10, pager: "#VehicleCostDetailsJqGridPager" });
       
    });

    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid');

    // For Pager Icons
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

    function enableTooltips(table) {
        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
    }
    function showFormattedStatus(cellvalue, options, rowObject) {
        if (cellvalue == 'True') {
            return 'Active';
        }
        else {
            return 'Inactive';
        }
    }


    function beforeDeleteCallback(e) {
        var form = $(e[0]);
        if (form.data('styled')) return false;

        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
        style_delete_form(form);

        form.data('styled', true);
    }

    function beforeEditCallback(e) {
        var form = $(e[0]);
        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
        style_edit_form(form);
    }

    function beforeAddCallback(e) {
        var form = $(e[0]);
        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')
        style_add_form(form);
    }

    $("#bedata").click(function () {
        jQuery("#VehicleCostDetailsJqGrid").jqGrid('editGridRow', "new", { height: 280, reloadAfterSubmit: false });
    });

    $(grid_selector).trigger("reloadGrid");

});



function AddNewRow(Id) {
    debugger;
    var parameters =
       {
           initdata: {
               
           },
           position: "first",
           addRowParams: {
               position: "afterSelected",
               keys: true,
               extraparam: {},
               oneditfunc: function (Id) {
                  
               }
           }
           //addRowParams: {
           //    position: "afterSelected",
           //    keys: true,
           //    oneditfunc: onInlineEdit(Id)


           //    //aftersavefunc: function (Id) {
           //    //    //lastSavedAddData = $("#VehicleCostDetailsJqGrid").jqGrid("getRowData", Id);
           //    //    var IndividualDate = $("#VehicleCostDetailsJqGrid").jqGrid('getCell', rowId, 'IndividualDate');
           //    //    alert(IndividualDate);
           //    //    $("#jqg" + rowid + "_" + IndividualDate).val(lastSavedAddData[IndividualDate]);
           //    //},
           //    //oneditfunc: function (rowid) {

           //    //    if (IndividualDate != "") {
           //    //        $("#" + $.jgrid.jqID(rowid + "_" + name)).val(lastSavedAddData[name]);
           //    //    }
           //    //}
           //},
           //editParams: {
           //    keys: true,
           //    oneditfunc: onInlineEdit(Id)
           //},

       };

    //$grid.jqGrid('inlineNav', topPagerSelector, {
    //    addParams: {
    //        position: "beforeSelected",
    //        rowID: '_empty',
    //        useDefValues: true,
    //        addRowParams: {
    //            keys: true,
    //            oneditfunc: onInlineEdit
    //        }
    //    },
    //    editParams: {
    //        keys: true,
    //        oneditfunc: onInlineEdit
    //    },
    //    add: true,
    //    edit: false,
    //    save: true,
    //    cancel: true
    //});
    //jQuery("#vendedores").jqGrid('addRow', parameters);

    $("#VehicleCostDetailsJqGrid").jqGrid('addRow', parameters);

    //$("#VehicleCostDetailsJqGrid").jqGrid("addRow", "#VehicleCostDetailsJqGridPager", { addParams: { position: "afterSelected" } });

}

function onInlineEdit(Id) {
    //if (rowId && rowId !== lastSelectedRow) {
    //    cancelEditing($grid);
    //    lastSelectedRow = rowId;
    //}
    debugger;
    var IDate = $("#VehicleCostDetailsJqGrid").jqGrid('getCell', Id, 'IndividualDate');
    alert(IDate);
    $("#jqg1_IndividualDate").val(IDate);
    //console.log(rowData.Phrase);
    //console.log(Id);
    //alert( $("#jqg" + rowid + "_" + IndividualDate).val())
    //$("#" + $.jgrid.jqID(rowid + "_" + IndividualDate)).val(IDate);
}

function checkvalid(value, column) {
    if (value == 'nil') {
        return [false, column + ": Field is Required"];
    }
    else {
        return [true];
    }
}
//function TripTypeddl() {
//    debugger;
//    $.getJSON("/Transport/TripTypeddl",
//      function (fillbc) {
//          var ddlbc = $("#TripNametxt").val();
//          ddlbc.empty();
//          ddlbc.append($('<option/>', { value: "", text: "Select" }));

//          $.each(fillbc, function (index, itemdata) {
//              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
//          });
//      });
//}





