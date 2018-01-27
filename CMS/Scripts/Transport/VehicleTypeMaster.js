
$(function () {
    var grid_selector = "#VehicleTypeMasterList";
    var pager_selector = "#VehicleTypeMasterListPager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".tab-content").width());
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

    jQuery(grid_selector).jqGrid({
        url: '/Transport/VehicleTypeMasterJqGrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Vehicle Type'],
        colModel: [
                { name: 'Id', index: 'Id', width: 450, align: 'left', editable: true, hidden: true, edittype: 'text', key: true, sortable: false },
                { name: 'VehicleType', index: 'VehicleType', width: 400, align: 'left', edittype: 'text', editable: true, editrules: { required: true }, editoptions: { maxlength: 50} },
                ],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '300',
        autowidth: true,
        viewrecords: true,
        caption: "<i class='ace-icon fa fa-truck'></i>&nbsp;Vehicle Type Master",
        multiselect: true,
        subGrid: true,
        loaderror: function (xhr, status, error) {
            $(grid_selector).cleargriddata();
            errmsg($.parsejson(xhr.responsetext).message);
        },
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        subGridOptions: {
            plusicon: "ace-icon fa fa-plus center bigger-110 blue",
            minusicon: "ace-icon fa fa-minus center bigger-110 blue",
            openicon: "ace-icon fa fa-chevron-right center orange",
            // load the subgrid data only once // and the just show/hide 
            "reloadOnExpand": false,
            // select the row when the expand column is clicked 
            "selectOnExpand": true
        },
        subGridRowExpanded: function (SKUList, Id) {
            var SKUListTable, SKUListPager;
            SKUListTable = SKUList + "_t";
            SKUListPager = "p_" + SKUListTable;
            $("#" + SKUList).html("<table id='" + SKUListTable + "' ></table><div id='" + SKUListPager + "' ></div>");
            jQuery("#" + SKUListTable).jqGrid({
                url: '/Transport/VehicleSubTypeMasterJqSubGrid?Id=' + Id,
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Vehicle No', 'Fuel Type', 'Campus', 'Engine Type', 'Engine Number', 'First RegisteredDate',
                        'Make', 'Type', 'Chassis No', 'BHP', 'CC', 'Wheel Base', 'Unladen Weight', 'Color', 'GVW', 'RC Attachment', 'Model', 'Address', 'Purpose', 'Id'],
                colModel: [
                    { name: 'VehicleNo', index: 'VehicleNo', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 1, colpos: 1} },
                    { name: 'FuelType', index: 'FuelType', editable: true, editrules: { required: false }, edittype: 'select', editoptions: { value: ":Select One;Petrol:Petrol;Diesel:Diesel;Gas:Gas" }, formoptions: { elmsuffix: ' *', rowpos: 1, colpos: 2} },
                      { name: 'Campus', index: 'Campus', width: 150, align: 'left', edittype: 'select', editable: true, editrules: { required: false },
                          editoptions: {
                              dataUrl: '/Base/FillAllBranchCode',
                              buildSelect: function (data) {
                                  Campus = jQuery.parseJSON(data);
                                  var s = '<select>';
                                  s += '<option value=" ">------Select------</option>';
                                  if (Campus && Campus.length) {
                                      for (var i = 0, l = Campus.length; i < l; i++) {
                                          s += '<option value="' + Campus[i].Value + '">' + Campus[i].Text + '</option>';
                                      }
                                  }
                                  return s + "</select>";
                              }
                          }, formoptions: { elmsuffix: ' *', rowpos: 1, colpos: 3 }
                      },


                { name: 'EngineType', index: 'EngineType', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 2, colpos: 1} },
                { name: 'EngineNumber', index: 'EngineNumber', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 2, colpos: 2} },
                { name: 'FirstRegisteredDate', index: 'FirstRegisteredDate', editable: true, editrules: { required: false }, editoptions: {
                    dataInit: function (el) {
                        $(el).datepicker({ format: "dd/mm/yy",
                            //changeMonth: true,
                            //timeFormat: 'hh:mm:ss',
                            autowidth: true,
                            //changeYear: true
                            // minDate: '+0d'
                        }).attr('readonly', 'readonly');
                    }
                }, formoptions: { elmsuffix: ' *', rowpos: 2, colpos: 3 }
                },
                { name: 'Make', index: 'Make', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 3, colpos: 1} },
                { name: 'Type', index: 'Type', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 3, colpos: 2} },
                { name: 'ChassisNo', index: 'ChassisNo', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 3, colpos: 3} },
                { name: 'BHP', index: 'BHP', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 4, colpos: 1} },
                { name: 'CC', index: 'CC', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 4, colpos: 2} },
                { name: 'WheelBase', index: 'WheelBase', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 4, colpos: 3} },
                { name: 'UnladenWeight', index: 'UnladenWeight', editable: true, hidden: true, editrules: { required: false, edithidden: true }, formoptions: { elmsuffix: ' *', rowpos: 5, colpos: 1} },
                { name: 'Color', index: 'Color', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 5, colpos: 2} },
                { name: 'GVW', index: 'GVW', editable: true, hidden: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 5, colpos: 3} },
                { name: 'RCAttachment', index: 'RCAttachment', editable: true, editrules: { required: false }, edittype: 'file', editoptions: { enctype: "multipart/form-data" }, formoptions: { elmsuffix: ' *', rowpos: 6, colpos: 1} },
                { name: 'Model', index: 'Model', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 6, colpos: 2} },
                { name: 'Address', index: 'Address', editable: true, hidden: true, editrules: { required: false }, edittype: 'textarea', editoptions: { rows: "4", cols: "18", maxlength: 400 }, formoptions: { elmsuffix: ' *', rowpos: 6, colpos: 3} },
                { name: 'Purpose', index: 'Purpose', width: 150, align: 'left', editable: true, hidden: true, editrules: { required: false }, edittype: 'textarea', editoptions: { rows: "4", cols: "18", maxlength: 400 }, formoptions: { elmsuffix: ' *', rowpos: 7, colpos: 1} },
                { name: 'Id', index: 'Id', hidden: true, key: true, editable: true },
                    ],
                pager: SKUListPager,
                rowNum: '5',
                rowList: [5, 10, 20, 50],
                sortname: 'Id',
                sortorder: 'Desc',
                height: '130',
                autowidth: true,
                viewrecords: true,
                loadComplete: function () {
                    var table = this;
                    setTimeout(function () {
                        updatePagerIcons(table);
                        enableTooltips(table);
                    }, 0);
                }
            });
            $("#" + SKUListTable).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
            jQuery("#" + SKUListTable).jqGrid('navGrid', "#" + SKUListPager, { edit: true, add: false, addicon: 'ace-icon fa fa-plus-circle purple', del: true, search: false, searchicon: 'ace-icon fa fa-search orange', refresh: true, refreshicon: 'ace-icon fa fa-refresh green' },
            { width: 'auto', url: '/Transport/AddVehicleSubType?test=edit' + '&VehicleTypeId=' + Id, modal: false, beforeSubmit: function (postdata, formid) {

                postdata.RCAttachment = $("#RCAttachment").val();
                return [true, ''];
            }, afterSubmit: UploadImage
            },
             { width: 'auto', url: '/Transport/AddVehicleSubType?VehicleTypeId=' + Id, modal: false, beforeSubmit: function (postdata, formid) {

                 postdata.RCAttachment = $("#RCAttachment").val();
                 return [true, ''];
             }, afterSubmit: UploadImage
             },
              { url: '/Transport/DeleteVehicleSubTypeMasterById' });
        }
 });
 $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
    jQuery(grid_selector).jqGrid('navGrid', pager_selector, {
        //navbar options
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
     { width: 'auto', url: '/Transport/VehicleTypeCRUD?Type=edit' },
     { width: 'auto', url: '/Transport/VehicleTypeCRUD?Type=add' },
      { width: 'auto', url: '/Transport/VehicleTypeCRUD?Type=delete' },{})

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
});


function formateadorLink(cellvalue, options, rowObject) {
    var status = $("#ddlStatus").val();
    if (status == "Available" && rowObject[5] == "CreateMatRequest") {
        return "<a href=/Store/MaterialRequest?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
    }
    else if ((status == "Available" || status == "Assigned") && rowObject[5] == "IssueMatRequest") {
        return "<a href=/Store/MaterialIssueNote?Id=" + rowObject[0] + "&activityId=" + rowObject[7] + ">" + cellvalue + "</a>";
    }
    else if (status == "Available" || status == "Assigned") {
        return "<a href=/Store/ActOnMaterialRequest?id=" + rowObject[0] + "&activityId=" + rowObject[7] + "&activityName=" + rowObject[8] + "&activityFullName=" + rowObject[9] + ">" + cellvalue + "</a>";
    }
    else if (status == "Sent" || status == "Completed") {
        return "<a href=/Store/ShowMaterialRequest?id=" + rowObject[0] + "&activityId=" + rowObject[7] + "&activityName=" + rowObject[8] + "&activityFullName=" + rowObject[9] + ">" + cellvalue + "</a>";
    }
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
function statusimage(cellvalue, options, rowObject) {
    var i;
    var cellValueInt = parseInt(cellvalue);
    var mrl = $(grid_selector).jqGrid();
    for (i = 0; i < mrl.length; i++) {

        if (rowObject[6] != "") {
            if (cellValueInt <= 24) {
                return '<img src="../../Images/yellow.jpg" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
            else if (cellValueInt > 24 && cellValueInt <= 48) {
                return '<img src="../../Images/orange.jpg" height="10px" width="10px"  alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
            else if (cellValueInt > 48) {
                return '<img src="../../Images/redblink3.gif" height="10px" width="10px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
            else if (cellvalue == 'Completed') {
                return '<img src="../../Images/green.jpg" height="12px" width="12px" alt=' + cellvalue + ' title=' + cellvalue + ' />'
            }
        }
    }
}

function ShowIssueNote(cellvalue, options, rowObject) {
    return "<a href=/Store/ShowMaterialIssueNote?Id=" + rowObject[7] + ">" + cellvalue + "</a>";
}

