jQuery(function ($) {
    var grid_selector = "#TrainingList";
    var pager_selector = "#TrainingListPager";
    var truefalse;//To assign True or False value for edit and delete(icon) in JqGrid
    //For Edit and View page of staff details
    if ($('#ForPage').val() == "StaffProfile") {
        debugger;
        $("#DivTrainingAdd").hide();
        truefalse = false;
        $(window).on('resize.jqGrid', function () {
            var page_width = $(".page-content").width();
            page_width = page_width - 46;
            $(grid_selector).jqGrid('setGridWidth', page_width);
        })
    }
    else {
        truefalse = true;
        $("#DivTrainingAdd").show();
        $(window).on('resize.jqGrid', function () {
            var page_width = $(".page-content").width();
            page_width = page_width - 25;
            $(grid_selector).jqGrid('setGridWidth', page_width);
        })
    }
    //$(window).on('resize.jqGrid', function () {
    //    var page_width = $(".page-content").width();
    //    page_width = page_width - 200;
    //    $(grid_selector).jqGrid('setGridWidth', page_width);
    //})

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
        url: '/StaffManagement/Trainingjqgrid',
        datatype: 'json',
        height: 100,
        colNames: ['Particulars', 'Place', 'Date', 'Sponsored By', '', ''],
        colModel: [
                          { name: 'Particulars', index: 'Particulars', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'Place', index: 'Place', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'Date', index: 'Date', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'SponsoredBy', index: 'SponsoredBy', align: 'left', sortable: false, editable: true, formatter: nildata },
                          { name: 'Id', index: 'Id', width: '30%', align: 'left', sortable: false, hidden: true, key: true, editable: true, formatter: nildata },
                          { name: 'PreRegNum', index: 'PreRegNum', width: 50, align: 'left', sortable: false, hidden: true, editable: true, formatter: nildata }
                          ],
        viewrecords: true,
        rowNum: 7,
        rowList: [7, 10, 30],
        pager: pager_selector,
        sortname: 'Id',
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
        caption: "<i class='ace-icon fa fa-history'></i>&nbsp;Training List"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
                edit: truefalse,
                editicon: 'ace-icon fa fa-pencil blue',
                add: false,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: truefalse,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, { url: '/StaffManagement/EditTraining/', left: '10%', top: '10%', height: '50%', width: 400, labelswidth: 60, closeAfterEdit: true, closeOnEscape: true, reloadAfterSubmit: true },
            {}, { url: '/StaffManagement/DeleteTraining' }, {})

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
    $("#AddTraining").click(function () {
        if (document.getElementById("particulars").value == "") {
            ErrMsg("Please Enter Particulars!");
            return false;
        }
        else {
            var particulars = document.getElementById("particulars").value;
            var place = document.getElementById("place").value;
            var date = document.getElementById("date").value;
            var sponsoredby = document.getElementById("sponsoredby").value;

            $.ajax({
                url: '/StaffManagement/AddTraining',
                type: 'POST',
                dataType: 'json',
                data: { particulars: particulars, place: place, date: date, sponsoredby: sponsoredby },
                traditional: true,
                success: function (data) {
                    //  alert('2');
                    $(grid_selector).setGridParam({ url: '/StaffManagement/Trainingjqgrid' }).trigger("reloadGrid");
                    document.getElementById("particulars").value = '';
                    document.getElementById("place").value = '';
                    document.getElementById("date").value = '';
                    document.getElementById("sponsoredby").value = '';
                },
                loadError: function (xhr, status, error) {
                    msgError = $.parseJSON(xhr.responseText).Message;
                    ErrMsg(msgError, function () { });
                }
            });
        }
    });
});