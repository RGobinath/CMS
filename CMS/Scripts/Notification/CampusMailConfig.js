$(function () {

    var grid_selector = "#CampusMailIdGrid";
    var pager_selector = "#CampusMailIdGridPager";

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

    //Pager icons
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


    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });
    $(grid_selector).jqGrid({

        url:"/Communication/JqGridCampusMailId",
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Campus', 'Server', 'EmailId', 'Password', 'AlternateEmailId', 'AlternateEmailIdPassword', 'FBLink', 'PhoneNumber', 'CreatedBy', 'CreatedDate', 'ModifiedBy', 'ModifiedDate'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
                       { name: 'Campus', index: 'Campus', editable: true, search: true, edittype: 'select',
                           editoptions: { dataUrl: '/Assess360/GetCampusddl' },
                           editrules: { required: true, custom: false }, sortable: true, stype: 'select',
                           searchoptions: { dataUrl: '/Assess360/GetCampusddl' }
                       },
                       { name: 'Server', index: 'Server', editable: true, search: true, edittype: 'select', editrules: { required: true },
                           editoptions: { value: ":--Select--;Live:Live;Test:Test" }, sortable: true, stype: 'select'
                       },
                       { name: 'EmailId', index: 'EmailId', align: 'left', editable: true, editrules: { email: true }, sortable: true },
                       { name: 'Password', index: 'Password', align: 'left', editable: true, editrules: { custom: true, custom_func: checkPwdvalid }, sortable: true },
                       { name: 'AlternateEmailId', index: 'AlternateEmailId', align: 'left', editable: true, editrules: { email: true }, sortable: true },
                       { name: 'AlternateEmailIdPassword', index: 'AlternateEmailIdPassword', align: 'left', editable: true, editrules: { custom: true, custom_func: checkAlterPwdvalid }, sortable: true },
                       { name: 'FBLink', index: 'FBLink', align: 'left', editable: true },
                       { name: 'PhoneNumber', index: 'PhoneNumber', align: 'left', editable: true },
                       { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
                       { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
                       { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true },
                       { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true}],
        pager: pager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Campus',
        sortorder: 'Asc',
        height: '230',
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: '<i class="fa fa-envelope"></i>&nbsp;Campus MailId Configuration Master',
        multiselect: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }

    });


    
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
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
    //Edit options
            {width: '400', left: '10%', top: '10%', height: '50%', url: "/Communication/AddorUpdateCampusMailId?edit=edit", modal: false, beforeSubmit: function (postdata, formid) {
                return [true, ''];
            }
        },
    //Add options
            {width: '400', left: '10%', top: '10%', height: '50%', url: "/Communication/AddorUpdateCampusMailId/", modal: false, beforeSubmit: function (postdata, formid) {
                return [true, ''];
            }
        },
        {}
        );



    //For Filter Search
    $(grid_selector).jqGrid('filterToolbar', {
        searchOnEnter: true, enableClear: false,
        afterSearch: function () { $(grid_selector).clearGridData(); }
    });


    function checkPwdvalid(value, column) {
        if (value == "") {
            if ($('#EmailId').val() == "") {
                return [true];
            }
            else {
                return [false, 'Please fill the ' + column];
            }
        }
        else {
            return [true];
        }
    }
    function checkAlterPwdvalid(value, column) {
        if (value == "") {
            if ($('#AlternateEmailId').val() == "") {
                return [true];
            }
            else {
                return [false, 'Please fill the ' + column];
            }
        }
        else {
            return [true];
        }
    }







}); ;