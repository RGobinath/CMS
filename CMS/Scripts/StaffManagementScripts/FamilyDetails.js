jQuery(function ($) {
    var grid_selector = "#FamilyDetailsList";
    var pager_selector = "#FamilyDetailsPager";
    var truefalse;//To assign True or False value for edit and delete(icon) in JqGrid
    //For Edit and View page of staff details
    if ($('#ForPage').val() == "StaffProfile") {
        debugger;
        $("#DivAddFamily").hide();
        truefalse = false;
        $(window).on('resize.jqGrid', function () {
            var page_width = $(".page-content").width();
            page_width = page_width - 46;
            $(grid_selector).jqGrid('setGridWidth', page_width);
        })
    }
    else {
        truefalse = true;
        $("#DivAddFamily").show();
        $(window).on('resize.jqGrid', function () {
            $(grid_selector).jqGrid('setGridWidth', $(".tab-content").width() - 40);
        })
    }    
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
        url: '/StaffManagement/FamilyDetailsJqgrid',
        datatype: 'json',
        height: 100,
        colNames: ['Name', 'Occupation', 'Age', 'Relationship', '', ''],
        colModel: [
                          { name: 'Name', index: 'Name',  align: 'left', sortable: false, editable: true, formatter: nildata,width:300 },
                          { name: 'Occupation', index: 'Occupation', align: 'left', sortable: false, editable: true, formatter: nildata,width:300 },
                          { name: 'Age', index: 'Age',  align: 'left', sortable: false, editable: true, formatter: nildata,width:277 },
                          { name: 'Relationship', index: 'Relationship', align: 'left', sortable: false, editable: true, formatter: nildata,width:300 },
                          { name: 'Id', index: 'Id',  align: 'left', sortable: false, hidden: true, key: true, editable: true, formatter: nildata },
                          { name: 'PreRegNum', index: 'PreRegNum', align: 'left', sortable: false, hidden: true, editable: true, formatter: nildata }
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
        caption: "<i class='ace-icon fa fa-users'></i>&nbsp;Family Details List"
    });

    //    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });

    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: truefalse,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: truefalse,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: truefalse,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            { url: '/StaffManagement/EditFamilyDetails/', left: '10%', top: '10%', height: '50%', width: 400, labelswidth: 60, closeAfterEdit: true, closeOnEscape: true, reloadAfterSubmit: true }, //Edit
            {}, { url: '/StaffManagement/DeleteStaffFamilyDetails' }, {}, {})

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
    function nildata(cellvalue, options, rowObject) {
        if ((cellvalue == '') || (cellvalue == null) || (cellvalue == 0)) {
            return ''
        }
        else {
            cellvalue = cellvalue.replace('&', 'and');
            //str = str.replace(/\&lt;/g, '<');
            return cellvalue
        }
    }

    $("#btnAddFamilyData").click(function () {
        if (document.getElementById("FamilyName").value == "") {
            ErrMsg("Please Enter Name!");
            return false;
        }
        else {
            var Name = $("#FamilyName").val();
            var Occupation = $("#FamilyOccupation").val();
            var Age = $("#FamilyAge").val();
            var Relationship = $("#FamilyRelationship").val();
            $.ajax({
                url: '/StaffManagement/AddFamilyDetails',
                type: 'POST',
                dataType: 'json',
                data: { Name: Name, Occupation: Occupation, Age: Age, Relationship: Relationship },
                traditional: true,
                success: function (data) {

                    $("#FamilyDetailsList").setGridParam({ url: '/StaffManagement/FamilyDetailsJqgrid' }).trigger("reloadGrid");
                    document.getElementById("FamilyName").value = '';
                    document.getElementById("FamilyOccupation").value = '';
                    document.getElementById("FamilyAge").value = '';
                    document.getElementById("FamilyRelationship").value = '';
                },
                loadError: function (xhr, status, error) {
                    msgError = $.parseJSON(xhr.responseText).Message;
                    ErrMsg(msgError, function () { });
                }
            });
            return true;
        }
    });
});