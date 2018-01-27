jQuery(function ($) {

    //debugger;
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";

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

    // DummyRec has been changed as Date of Incident
    jQuery(grid_selector).jqGrid({

        url: '/HostelManagement/JqgridMaterialDamage/',
        datatype: 'json',
        mtype: 'GET',
        shrinkToFit: true,
        height: 250,
        colNames: ['Id', 'MDId', 'Stud_Id', 'Name', 'NewId', 'Date Of Incident', 'Details Of Incident', 'Amount', 'Remarks', 'DummyRec'],
        colModel: [
                { name: 'Id', index: 'Id', width: 230, editable: true, hidden: true },//0
                { name: 'MDId', index: 'MDId', width: 230, editable: true, hidden: true, key: true },//1
                { name: 'Stud_Id', index: 'Stud_Id', width: 260, editable: true, hidden: true },//2
                {
                    name: 'Name', index: 'Name', width: 260, editable: true, editoptions: { size: 50 },
                },//3
                { name: 'NewId', index: 'NewId', width: 260, editable: true },//4
                {
                    name: 'DummyRec', index: 'DummyRec', editable: true, search: true, formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' }, searchoptions: {
                        dataInit: function (elem) {
                            jQuery(elem).datepicker({
                                format: "dd/mm/yyyy",
                                autoclose: true
                            });
                        },
                        dataEvents: [
                                {
                                    type: 'change', fn: function (e) {
                                        //$(e.target).val();
                                        $(grid_selector)[0].triggerToolbar();
                                        //   HOW TO SUBMIT THE FORM HERE?
                                    }
                                }]
                    },
                    editrules: { required: true }
                }, //5

                { name: 'DetailsOfIncident', index: 'DetailsOfIncident', width: 260, editable: true, edittype: "textarea", editoptions: { cols: 20 } },//6
                { name: 'Amount', index: 'Amount', width: 260, editable: true },//7
                { name: 'Remarks', index: 'Remarks', width: 190, editable: true, edittype: "textarea", editoptions: { cols: 20 } },//7
        {
            name: 'DateOfIncident', index: 'DateOfIncident', editable: true, search: true, hidden: true
        },//8

        ],
        viewrecords: true,
        rowNum: 20,
        rowList: [20, 50, 100, 200, 500],
        pager: pager_selector,
        height: 300,
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        gridComplete: function () {
            var mydata = jQuery(grid_selector).jqGrid("getGridParam", "data");
        },
        caption: " Material Damage"
    });

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', {
        searchOnEnter: true,
        enableClear: false,
        defaultSearch: 'cn'
    });
    //switch element when editing inline
    function aceSwitch(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=checkbox]')
                    .addClass('ace ace-switch ace-switch-5')
                    .after('<span class="lbl"></span>');
        }, 0);
    }
    //enable datepicker
    function pickDate(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=text]')
                        .datepicker({ format: 'yyyy-mm-dd', autoclose: true });
        }, 0);
    }
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            { 	//navbar options
                edit: false,
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
            {}, //Edit
            {
                url: '/HostelManagement/SaveAndEditMaterialDamage',
                afterShowForm: function (formid) {
                    // Autocomplete Search Example....
                    $("#Name").autocomplete({
                        source: function (request, response) {
                            $.getJSON('/HostelManagement/GetStudentNameWithNewIds?term=' + request.term, function (data) {
                                response(data);
                            });
                        },

                        select: function (e, ui) {
                            var slt = ui.item.value.split("/");
                            $('#Name').val(slt[0]);
                            $('#NewId').val(slt[1]);
                        },
                        change: function (e, ui) {
                        },
                        minLength: 1,
                        delay: 10
                    });
                    $('#DummyRec').datepicker({
                        format: "dd/mm/yyyy",
                        autoclose: true
                    });
                },
                onclickSubmit: function (params, postdata) {
                    // postdata.DummyRec = ChangeDateFunction(postdata.DummyRec);
                    var res = postdata.Name.split('/');
                    postdata.Name = res[0];
                    postdata.NewId = res[1];
                    return postdata;
                },
                //beforeSubmit: function (postdata, formid) {

                //},
                width: 700,
                closeAfterAdd: true
            }, //Add
              { url: '/HostelManagement/DeleteMaterialDamage/', closeAfterDelete: true, beforeShowForm: function (params) { var gsr = $(grid_selector).getGridParam('selrow'); var sdata = $(grid_selector).getRowData(gsr); return { Id: sdata.Id } } }, //Delete 
              {}, {})

    function ChangeDateFunction(dateVal) {
        var res = dateVal.split('/');
        var rtn = res[1] + '/' + res[0] + '/' + res[2];
        return rtn;
    }

    function style_edit_form(form) {
        //enable datepicker on "sdate" field and switches for "stock" field
        form.find('input[name=sdate]').datepicker({ format: 'yyyy-mm-dd', autoclose: true })
                .end().find('input[name=stock]')
                    .addClass('ace ace-switch ace-switch-5').after('<span class="lbl"></span>');
        //don't wrap inside a label element, the checkbox value won't be submitted (POST'ed)
        //.addClass('ace ace-switch ace-switch-5').wrap('<label class="inline" />').after('<span class="lbl"></span>');

        //update buttons classes
        var buttons = form.next().find('.EditButton .fm-button');
        buttons.addClass('btn btn-sm').find('[class*="-icon"]').hide(); //ui-icon, s-icon
        buttons.eq(0).addClass('btn-primary').prepend('<i class="ace-icon fa fa-check"></i>');
        buttons.eq(1).prepend('<i class="ace-icon fa fa-times"></i>')

        buttons = form.next().find('.navButton a');
        buttons.find('.ui-icon').hide();
        buttons.eq(0).append('<i class="ace-icon fa fa-chevron-left"></i>');
        buttons.eq(1).append('<i class="ace-icon fa fa-chevron-right"></i>');
    }

    function style_delete_form(form) {
        var buttons = form.next().find('.EditButton .fm-button');
        buttons.addClass('btn btn-sm btn-white btn-round').find('[class*="-icon"]').hide(); //ui-icon, s-icon
        buttons.eq(0).addClass('btn-danger').prepend('<i class="ace-icon fa fa-trash-o"></i>');
        buttons.eq(1).addClass('btn-default').prepend('<i class="ace-icon fa fa-times"></i>')
    }

    function style_search_filters(form) {
        form.find('.delete-rule').val('X');
        form.find('.add-rule').addClass('btn btn-xs btn-primary');
        form.find('.add-group').addClass('btn btn-xs btn-success');
        form.find('.delete-group').addClass('btn btn-xs btn-danger');
    }
    function style_search_form(form) {
        var dialog = form.closest('.ui-jqdialog');
        var buttons = dialog.find('.EditTable')
        buttons.find('.EditButton a[id*="_reset"]').addClass('btn btn-sm btn-info').find('.ui-icon').attr('class', 'ace-icon fa fa-retweet');
        buttons.find('.EditButton a[id*="_query"]').addClass('btn btn-sm btn-inverse').find('.ui-icon').attr('class', 'ace-icon fa fa-comment-o');
        buttons.find('.EditButton a[id*="_search"]').addClass('btn btn-sm btn-purple').find('.ui-icon').attr('class', 'ace-icon fa fa-search');
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

    //var selr = jQuery(grid_selector).jqGrid('getGridParam','selrow');

    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });



});