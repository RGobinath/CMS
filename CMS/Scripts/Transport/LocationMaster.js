jQuery(function ($) {
    var grid_selector = "#jqGridLocationMaster";
    var pager_selector = "#jqGridLocationMasterPager";

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
        url: '/Transport/LocationMasterJqGrid',
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'Location Id', 'Campus', 'Location Name', 'Created Date', 'Created By', 'Modified Date', 'Modified By'],
        colModel: [
            { name: 'Id', index: 'Id', key: true, hidden: true },
            { name: 'LocationId', index: 'LocationId', editable: true },
            { name: 'Campus', index: 'Campus', editable: true, edittype: 'select', editrules: { required: true }, formoptions: { elmsuffix: ' *' },
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
                }, search: true
            },
            { name: 'LocationName', index: 'LocationName', editable: true, editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
             { name: 'CreatedDate', index: 'CreatedDate', editable: true, search: false },
            { name: 'CreatedBy', index: 'CreatedBy', editable: true },
            { name: 'ModifiedDate', index: 'ModifiedDate', editable: true, search: false },
            { name: 'ModifiedBy', index: 'ModifiedBy', editable: true }
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
        caption: "<i class='ace-icon fa fa-location-arrow'></i>&nbsp;Location Master Details"
    });

    //    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
        onClickButton: function () {
            window.open("LocationMasterJqGrid" + '?LocationId=' + $("#txtLocationId").val() + '&Campus=' + $("#txtCampus").val() + '&LocationName=' + $("#txtLocationName").val() + '&CreatedBy=' + $("#txtLocCreatedBy").val() + '&ModifiedBy=' + $("#txtLocModifiedBy").val() + ' &rows=9999 ' + '&ExportType=Excel');
        }
    });
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
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
            { url: '/Transport/EditLocationMasterDetails',
                beforeShowForm: function (form) {
                    $('#tr_LocationId', form).hide(); $('#tr_CreatedDate', form).hide(); $('#tr_CreatedBy', form).hide(); $('#tr_ModifiedDate', form).hide(); $('#tr_ModifiedBy', form).hide();
                }
            }, //Edit
            {url: '/Transport/AddLocationMasterDetails',
            beforeShowForm: function (form) {
                $('#tr_LocationId', form).hide(); $('#tr_CreatedDate', form).hide(); $('#tr_CreatedBy', form).hide(); $('#tr_ModifiedDate', form).hide(); $('#tr_ModifiedBy', form).hide();
            }
        }, //Add
              {
              url: '/Transport/DeleteLocationMasterDetails'
          }, {}, {})






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
    $("#btnLocSearch").click(function () {
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/LocationMasterJqGrid',
                    postData: { LocationId: $("#txtLocationId").val(), Campus: $("#txtCampus").val(), LocationName: $("#txtLocationName").val(), CreatedBy: $("#txtLocCreatedBy").val(), ModifiedBy: $("#txtLocModifiedBy").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnLocReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/LocationMasterJqGrid',
                    postData: { LocationId: $("#txtLocationId").val(), Campus: $("#txtCampus").val(), LocationName: $("#txtLocationName").val(), CreatedBy: $("#txtLocCreatedBy").val(), ModifiedBy: $("#txtLocModifiedBy").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
});
