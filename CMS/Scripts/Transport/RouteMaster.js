//jQuery(function ($) {
//    var grid_selector = "#jqGridRouteMaster";
//    var pager_selector = "#jqGridRouteMasterPager";

//    $(window).on('resize.jqGrid', function () {
//        $(grid_selector).jqGrid('setGridWidth', $(".tab-content").width());
//    })


//    //resize on sidebar collapse/expand
//    var parent_column = $(grid_selector).closest('[class*="col-"]');
//    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
//        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
//            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
//            setTimeout(function () {
//                $(grid_selector).jqGrid('setGridWidth', parent_column.width());
//            }, 0);
//        }
//    })
//    jQuery(grid_selector).jqGrid({
//        url: '/Transport/RouteMasterJqGrid',
//        datatype: 'json',
//        height: 180,
//        colNames: ['Id', 'Route Id', 'Route No', 'Campus', 'Source', 'Destination', 'Via', 'Created Date', 'Created By', 'Modified Date', 'Modified By'],
//        colModel: [
//            { name: 'Id', index: 'Id', key: true, hidden: true },
//            { name: 'RouteId', index: 'RouteId', editable: true },
//            { name: 'RouteNo', index: 'RouteNo', editable: true, editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
//            { name: 'Campus', index: 'Campus', editable: true, edittype: 'select', editrules: { required: true }, formoptions: { elmsuffix: ' *' },
//                editoptions: {
//                    dataUrl: '/Base/FillAllBranchCode',
//                    buildSelect: function (data) {
//                        Campus = jQuery.parseJSON(data);
//                        var s = '<select>';
//                        s += '<option value=" ">------Select------</option>';
//                        if (Campus && Campus.length) {
//                            for (var i = 0, l = Campus.length; i < l; i++) {
//                                s += '<option value="' + Campus[i].Value + '">' + Campus[i].Text + '</option>';
//                            }
//                        }
//                        return s + "</select>";
//                    }
//                }
//            },
////            { name: 'Source', index: 'Source', editable: true, edittype: 'select', editrules: { required: true }, formoptions: { elmsuffix: ' *' },
////                editoptions: {
////                    dataUrl: '/Base/FillAllLocationCode',
////                    buildSelect: function (data) {
////                        Location = jQuery.parseJSON(data);
////                        var s = '<select>';
////                        s += '<option value=" ">------Select------</option>';
////                        if (Location && Location.length) {
////                            for (var i = 0, l = Location.length; i < l; i++) {
////                                s += '<option value="' + Location[i].Value + '">' + Location[i].Text + '</option>';
////                            }
////                        }
////                        return s + "</select>";
////                    }
////                }
////            },
//        //             {name: 'Destination', index: 'Destination', editable: true, edittype: 'select', editrules: { required: true }, formoptions: { elmsuffix: ' *' },
//        //             editoptions: {
//        //                 dataUrl: '/Base/FillAllLocationCode',
//        //                 buildSelect: function (data) {
//        //                     Destination = jQuery.parseJSON(data);
//        //                     var s = '<select>';
//        //                     s += '<option value=" ">------Select------</option>';
//        //                     if (Destination && Destination.length) {
//        //                         for (var i = 0, l = Destination.length; i < l; i++) {
//        //                             s += '<option value="' + Destination[i].Value + '">' + Destination[i].Text + '</option>';
//        //                         }
//        //                     }
//        //                     return s + "</select>";
//        //                 }
//        //             }
//            //         },
//         {name: 'Source', index: 'Source', hidden: false, editable: true,
//         editoptions: {
//             dataInit: function (elem) {
//                 myAutocomplete(elem, "/Transport/GetDestination");
//             }
//         }
//     },
// { name: 'Destination', index: 'Destination', hidden: false, editable: true,
// editoptions: {
//     dataInit: function (elem) {
//         myAutocomplete(elem, "/Transport/GetDestination");
//     }
// }
//},
//{ name: 'Via', index: 'Via', hidden: false, editable: true,
//    editoptions: {
//        dataInit: function (elem) {
//            myAutocomplete(elem, "/Transport/GetDestination");
//        }
//    }
//},
////            { name: 'Via', index: 'Via', editable: true },
//        //            { name: 'Distance', index: 'Distance', editable: false },
//        //            { name: 'District', index: 'District', editable: false, hidden: true },
//        //            { name: 'State', index: 'State', editable: false, hidden: true },
//        //            { name: 'Country', index: 'Country', editable: false, hidden: true },
//            {name: 'CreatedDate', index: 'CreatedDate', editable: true, search: false, hidden: true },
//            { name: 'CreatedBy', index: 'CreatedBy', editable: true, hidden: true },
//            { name: 'ModifiedDate', index: 'ModifiedDate', editable: true, search: false, hidden: true },
//            { name: 'ModifiedBy', index: 'ModifiedBy', editable: true, hidden: true }
//            ],
//        viewrecords: true,
//        rowNum: 7,
//        rowList: [7, 10, 30],
//        pager: pager_selector,
//        sortname: 'Id',
//        sortorder: 'Desc',
//        multiselect: true,
//        loadComplete: function () {
//            var table = this;
//            setTimeout(function () {
//                styleCheckbox(table);
//                updateActionIcons(table);
//                updatePagerIcons(table);
//                enableTooltips(table);
//            }, 0);
//        },
//        caption: "<i class='ace-icon fa fa-road'></i>&nbsp;Route Master"

//    });
//    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
//    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });

//    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
//        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
//        onClickButton: function () {
//            window.open("RouteMasterJqGrid" + '?RouteId=' + $("#txtRouteId").val() + '&RouteNo=' + $("#txtRouteNo").val()
//            + '&Campus=' + $("#txtRouteCampus").val() + '&Source=' + $("#txtRouteSource").val() + '&Destination=' + $("#txtRouteDestination").val()
//            + '&Via=' + $("#txtRouteVia").val()
//             + ' &rows=9999 ' + '&ExportType=Excel');
//        }
//    });
//    //navButtons
//    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
//            {
//                edit: false,
//                editicon: 'ace-icon fa fa-pencil blue',
//                add: true,
//                addicon: 'ace-icon fa fa-plus-circle purple',
//                del: true,
//                delicon: 'ace-icon fa fa-trash-o red',
//                search: false,
//                searchicon: 'ace-icon fa fa-search orange',
//                refresh: true,
//                refreshicon: 'ace-icon fa fa-refresh green',
//                view: false,
//                viewicon: 'ace-icon fa fa-search-plus grey'
//            }, { beforeShowForm: function (form) {
//                $('#tr_RouteId', form).hide(); $('#tr_CreatedDate', form).hide(); $('#tr_CreatedBy', form).hide(); $('#tr_ModifiedDate', form).hide(); $('#tr_ModifiedBy', form).hide();
//            }, url: '/Transport/EditRouteMasterDetails'
//            }, { beforeShowForm: function (form) {
//                $('#tr_RouteId', form).hide(); $('#tr_CreatedDate', form).hide(); $('#tr_CreatedBy', form).hide(); $('#tr_ModifiedDate', form).hide(); $('#tr_ModifiedBy', form).hide();
//            }, url: '/Transport/AddRouteMasterDetails'
//            }, { url: '/Transport/DeleteRouteMasterDetails' }, {})


//    //For pager Icons
//    function styleCheckbox(table) {
//    }
//    function updateActionIcons(table) {
//    }

//    //replace icons with FontAwesome icons like above
//    function updatePagerIcons(table) {
//        var replacement =
//            {
//                'ui-icon-seek-first': 'ace-icon fa fa-angle-double-left bigger-140',
//                'ui-icon-seek-prev': 'ace-icon fa fa-angle-left bigger-140',
//                'ui-icon-seek-next': 'ace-icon fa fa-angle-right bigger-140',
//                'ui-icon-seek-end': 'ace-icon fa fa-angle-double-right bigger-140'
//            };
//        $('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
//            var icon = $(this);
//            var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

//            if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
//        })
//    }

//    function enableTooltips(table) {
//        $('.navtable .ui-pg-button').tooltip({ container: 'body' });
//        $(table).find('.ui-pg-div').tooltip({ container: 'body' });
//    }
//    $("#btnRouteSearch").click(function () {
//        $(grid_selector).setGridParam(
//                {
//                    datatype: "json",
//                    url: '/Transport/RouteMasterJqGrid',
//                    postData: { RouteId: $("#txtRouteId").val(), RouteNo: $("#txtRouteNo").val(), Campus: $("#txtRouteCampus").val(), Source: $("#txtRouteSource").val(), Destination: $("#txtRouteDestination").val(), Via: $("#txtRouteVia").val() },
//                    page: 1
//                }).trigger("reloadGrid");
//    });
//    $("#btnRouteReset").click(function () {
//        $("input[type=text], textarea, select").val("");
//        $(grid_selector).setGridParam(
//                {
//                    datatype: "json",
//                    url: '/Transport/RouteMasterJqGrid',
//                    postData: { RouteId: $("#txtRouteId").val(), RouteNo: $("#txtRouteNo").val(), Campus: $("#txtRouteCampus").val(), Source: $("#txtRouteSource").val(), Destination: $("#txtRouteDestination").val(), Via: $("#txtRouteVia").val() },
//                    page: 1
//                }).trigger("reloadGrid");
//    });
//    function myAutocomplete(elem, url) {
//        setTimeout(function () {
//            $(elem).autocomplete({
//                source: url,
//                minLength: 1,
//                select: function (event, ui) {
//                    $(elem).val(ui.item.value);
//                    $(elem).trigger('change');
//                }
//            });
//        }, 50);
//    }
//});



jQuery(function ($) {
    var grid_selector = "#jqGridRouteMaster";
    var pager_selector = "#jqGridRouteMasterPager";

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
        url: '/Transport/RouteMasterJqGrid',
        datatype: 'json',
        height: 180,
        colNames: ['Id', 'Campus', 'Vehicle No', 'Route Id', 'Route No', 'Source', 'Destination', 'Via', 'Created Date', 'Created By', 'Modified Date', 'Modified By', 'IMEI Number'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            {
                name: 'Campus', index: 'Campus', editable: true, edittype: 'select', editrules: { required: true }, formoptions: { elmsuffix: ' *' },
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
                    },
                    dataEvents:
                  [
                   {
                       type: 'change', fn: function (e) {
                           var fillbc = $.ajax({
                               url: '/Transport/FillVehicleNoOnDailyUsageVehicleMasterByCampus?Campus=' + $("#Campus").val(),
                               async: false
                           }).responseText;
                           var response = jQuery.parseJSON(fillbc);
                           var Boardjq = $("#VehicleNo");
                           Boardjq.empty();
                           Boardjq.append($('<option/>', { value: "", text: "---Select---" }));
                           if (fillbc != null) {
                               if (response.length == 1) {
                                   Boardjq.append("<option value='" + response[0].Value + "'selected='selected' >" + response[0].Text + "</option>");
                               }
                               else {
                                   for (var i = 0, l = response.length; i < l; i++) {
                                       Boardjq.append($('<option/>', { value: response[i].Value, text: response[i].Text }));
                                   }
                               }
                           }
                       }
                   }
                  ], style: "width: 175px; height: 20px; font-size: 0.9em"
                }
            },
             {
                 name: 'VehicleNo', index: 'VehicleNo', editable: true, edittype: 'select', editrules: { required: true }, formoptions: { elmsuffix: ' *' },
                 editoptions: {
                     dataUrl: '/Transport/FillVehicleNoOnDailyUsageVehicleMasterByCampus',
                     buildSelect: function (data) {
                         VehicleNo = jQuery.parseJSON(data);
                         var s = '<select>';
                         s += '<option value=" ">------Select------</option>';
                         if (VehicleNo && VehicleNo.length) {
                             for (var i = 0, l = VehicleNo.length; i < l; i++) {
                                 s += '<option value="' + VehicleNo[i].Value + '">' + VehicleNo[i].Text + '</option>';
                             }
                         }
                         return s + "</select>";
                     }
                     , style: "width: 175px; height: 20px; font-size: 0.9em"
                 }
             },
            { name: 'RouteId', index: 'RouteId', editable: false, edithidden: true },
            { name: 'RouteNo', index: 'RouteNo', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *' } },


//            { name: 'Source', index: 'Source', editable: true, edittype: 'select', editrules: { required: true }, formoptions: { elmsuffix: ' *' },
//                editoptions: {
//                    dataUrl: '/Base/FillAllLocationCode',
//                    buildSelect: function (data) {
//                        Location = jQuery.parseJSON(data);
//                        var s = '<select>';
//                        s += '<option value=" ">------Select------</option>';
//                        if (Location && Location.length) {
//                            for (var i = 0, l = Location.length; i < l; i++) {
//                                s += '<option value="' + Location[i].Value + '">' + Location[i].Text + '</option>';
//                            }
//                        }
//                        return s + "</select>";
//                    }
//                }
//            },
        //             {name: 'Destination', index: 'Destination', editable: true, edittype: 'select', editrules: { required: true }, formoptions: { elmsuffix: ' *' },
        //             editoptions: {
        //                 dataUrl: '/Base/FillAllLocationCode',
        //                 buildSelect: function (data) {
        //                     Destination = jQuery.parseJSON(data);
        //                     var s = '<select>';
        //                     s += '<option value=" ">------Select------</option>';
        //                     if (Destination && Destination.length) {
        //                         for (var i = 0, l = Destination.length; i < l; i++) {
        //                             s += '<option value="' + Destination[i].Value + '">' + Destination[i].Text + '</option>';
        //                         }
        //                     }
        //                     return s + "</select>";
        //                 }
        //             }
            //         },
         {
             name: 'Source', index: 'Source', hidden: false, editable: true,
             editoptions: {
                 dataInit: function (elem) {
                     myAutocomplete(elem, "/Transport/GetDestination");
                 }
             }
         },
 {
     name: 'Destination', index: 'Destination', hidden: false, editable: true,
     editoptions: {
         dataInit: function (elem) {
             myAutocomplete(elem, "/Transport/GetDestination");
         }
     }
 },
{
    name: 'Via', index: 'Via', hidden: false, editable: true,
    editoptions: {
        dataInit: function (elem) {
            myAutocomplete(elem, "/Transport/GetDestination");
        }
    }
},
//            { name: 'Via', index: 'Via', editable: true },
        //            { name: 'Distance', index: 'Distance', editable: false },
        //            { name: 'District', index: 'District', editable: false, hidden: true },
        //            { name: 'State', index: 'State', editable: false, hidden: true },
        //            { name: 'Country', index: 'Country', editable: false, hidden: true },
            { name: 'CreatedDate', index: 'CreatedDate', editable: true, search: false, hidden: true },
            { name: 'CreatedBy', index: 'CreatedBy', editable: true, hidden: true },
            { name: 'ModifiedDate', index: 'ModifiedDate', editable: true, search: false, hidden: true },
            { name: 'ModifiedBy', index: 'ModifiedBy', editable: true, hidden: true },
            { name: 'IMEINmber', index: 'IMEINmber', editable: false }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30, 50],
        pager: pager_selector,
        sortname: 'RouteNo',
        sortorder: 'Asc',
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-road'></i>&nbsp;Route Master"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    // $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
        onClickButton: function () {
            window.open("RouteMasterJqGrid" + '?RouteId=' + $("#txtRouteId").val() + '&RouteNo=' + $("#txtRouteNo").val()
            + '&Campus=' + $("#txtRouteCampus").val() + '&Source=' + $("#txtRouteSource").val() + '&Destination=' + $("#txtRouteDestination").val()
            + '&Via=' + $("#txtRouteVia").val()
             + ' &rows=9999 ' + '&ExportType=Excel');
        }
    });
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
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
            {
                width: 'auto', url: '/Transport/EditRouteMasterDetails', modal: false, closeAfterEdit: true
            }, //Edit
            //{
            //    beforeShowForm: function (form) {
            //    $('#tr_RouteId', form).hide(); $('#tr_CreatedDate', form).hide(); $('#tr_CreatedBy', form).hide(); $('#tr_ModifiedDate', form).hide(); $('#tr_ModifiedBy', form).hide();
            //}, url: '/Transport/EditRouteMasterDetails'
            //},
            {
                beforeShowForm: function (form) {
                    $('#tr_RouteId', form).hide(); $('#tr_CreatedDate', form).hide(); $('#tr_CreatedBy', form).hide(); $('#tr_ModifiedDate', form).hide(); $('#tr_ModifiedBy', form).hide();
                }, url: '/Transport/AddRouteMasterDetails'
            }, { url: '/Transport/DeleteRouteMasterDetails' }, {})


    //For pager Icons
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
    $("#btnRouteSearch").click(function () {
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/RouteMasterJqGrid',
                    postData: { RouteId: $("#txtRouteId").val(), RouteNo: $("#txtRouteNo").val(), Campus: $("#Campusddl").val(), VehicleNo: $("#txtVehicleNo").val(), Source: $("#txtRouteSource").val(), Destination: $("#txtRouteDestination").val(), Via: $("#txtRouteVia").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnRouteReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/RouteMasterJqGrid',
                    postData: { RouteId: $("#txtRouteId").val(), RouteNo: $("#txtRouteNo").val(), Campus: $("#Campusddl").val(), VehicleNo: $("#txtVehicleNo").val(), Source: $("#txtRouteSource").val(), Destination: $("#txtRouteDestination").val(), Via: $("#txtRouteVia").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    function myAutocomplete(elem, url) {
        setTimeout(function () {
            $(elem).autocomplete({
                source: url,
                minLength: 1,
                select: function (event, ui) {
                    $(elem).val(ui.item.value);
                    $(elem).trigger('change');
                }
            });
        }, 50);
    }
});
$.getJSON("/Base/FillAllBranchCode",
     function (fillcampus) {
         var ddlcam = $("#Campusddl");
         ddlcam.empty();
         ddlcam.append($('<option/>',
        {
            value: "",
            text: "Select"

        }));

         $.each(fillcampus, function (index, itemdata) {
             ddlcam.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
         });
     });

