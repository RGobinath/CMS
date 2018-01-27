jQuery(function ($) {
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";

    if ('@ViewBag.results' == "No") { ErrMsg('Student cannot be promoted.'); } else { }

    if ('@ViewBag.IsSaveList' == "True") {
        $('#btnsaveList').hide();
        $('#RequestName').attr('readonly', true); $('#RequestName').css('background', '#E3E8E9');
        $('#ddlcampus').attr('disabled', true); $('#ddlcampus').css('background', '#E3E8E9');
        $('#ddlgrade').attr('disabled', true); $('#ddlgrade').css('background', '#E3E8E9');
        $('#Newpromotion').show();
    } else {
        $('#btnsaveList').show();
        $('#RequestName').attr('readonly', false);
        $('#ddlcampus').attr('disabled', false);
        $('#ddlgrade').attr('disabled', false);
        $('#Newpromotion').hide();
    }

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
    debugger;
    jQuery(grid_selector).jqGrid({

        url: '/Admission/BulkPromTransferJqGrid?BulkPromTransferRequestId=' + '@Model.BulkPromTransferRequestId',
        datatype: 'json',
        type: 'GET',
        height: 250,
        colNames: ['Id', 'Application No', 'Pre-Reg No', 'Name of Applicant', 'Grade', 'Section', 'Campus', 'Fee Structure Year', 'Admission Status', 'Student Id', 'Academic Year', 'Applied Date'],
        colModel: [
              { name: 'Id', index: 'Id', hidden: true },
              { name: 'ApplicationNo', index: 'ApplicationNo' },
              { name: 'PreRegNum', index: 'PreRegNum' },
              { name: 'Name', index: 'Name', width: 400 },
              { name: 'Grade', index: 'Grade' },
              { name: 'Section', index: 'Section' },
              { name: 'Campus', index: 'Campus' },
              { name: 'FeeStructYear', index: 'FeeStructYear' },
              { name: 'AdmissionStatus', index: 'AdmissionStatus' },
              { name: 'NewId', index: 'NewId' },
              { name: 'AcademicYear', index: 'AcademicYear' },
              { name: 'AppliedDate', index: 'CreatedDate' }
              ],
        viewrecords: true,
        sortname: 'Id',
        sortorder: 'Desc',
        rowNum: 50,
        rowList: [50, 100, 150, 200, 250, 300],
        pager: pager_selector,
        altRows: true,
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
        caption: "Promotion and Transfer"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size

    $("#Newpromotion").click(function () {
        $("#NewPromoteDiv").html("");
        var GridIdList = $(grid_selector).jqGrid('getGridParam', 'selarrrow');
        var rowData = [];
        var rowData1 = [];
        var rowcampus = [];
        var rowgrade = [];

        if (GridIdList.length > 0) {
            for (i = 0; i < GridIdList.length; i++) {
                rowData[i] = $(grid_selector).jqGrid('getRowData', GridIdList[i]);

                if (rowData1 == "") {
                    rowData1 = rowData[i].PreRegNum;
                    rowgrade = rowData[i].Grade;
                    rowcampus = rowData[i].Campus;
                }
                else {
                    rowData1 = rowData1 + "," + rowData[i].PreRegNum;
                    rowgrade = rowgrade + "," + rowData[i].Grade;
                    rowcampus = rowcampus + "," + rowData[i].Campus;
                }
            }
            $.ajax({
                url: '/Admission/CheckPromotionDetails',
                type: 'GET',
                //  dataType: 'json',
                data: { PreRegNo: rowData1, campus: rowcampus, grade: rowgrade, check: 'yes' },
                traditional: true,
                success: function (data) {
                    if (data == "True") {
                        $.ajax({
                            url: '/Admission/CheckPromotionDetails',
                            type: 'GET',
                            data: { PreRegNo: rowData1, campus: rowcampus },
                            traditional: true
                        });
                        LoadPopupDynamicaly("/Admission/NewPromotion?BulkPromTransferRequestId=" + '@Model.BulkPromTransferRequestId', $('#NewPromoteDiv'),
                                                                            function () {
                                                                            });
                        var e = document.getElementById("ddlcampus");
                        var campus = e.options[e.selectedIndex].value;
                    }
                    else {
                        ErrMsg('Students to be promoted should belong to same grade and campus.');
                    }
                }
            });
        }
    });




    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });

    $('#btnSearch').click(function () {
        debugger;
        //            alert('Search');
        if ($('#ddlcampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#ddlgrade').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Admission/BulkPromTransferJqGrid/',
                    postData: { BulkPromTransferRequestId: '@Model.BulkPromTransferRequestId', campus: $('#ddlcampus').val(), grade: $('#ddlgrade').val(), section: $('#Section').val().toString(), admStatus: $('#admstats').val(), feeStructure: $('#feestructddl').val(), appName: $('#appname').val(), idNum: $('#idnumber').val(), isHosteller: $('#ishosteller').val(), academicYear: $('#academicyear').val(), searchType: "" },
                    page: 1
                }).trigger("reloadGrid");
    });
    $('#btnreset').click(function () {
        debugger;
        alert('Reset');
        $(grid_selector).clearGridData();
        admStatus: $('#admstats').val('')
        feeStructure: $('#feestructddl').val('')
        appName: $('#appname').val('')
        idNum: $('#idnumber').val('')
        isHosteller: $('#ishosteller').val('')
        academicYear: $('#academicyear').val('')
    });
    $('#btnsaveList').click(function () {
        debugger;
        //            alert('Save');
        if ($('#RequestName').val() == "") { ErrMsg("Please fill the Request"); return false; }
        if ($('#ddlcampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
        if ($('#ddlgrade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
    });
    $('#btnCancel').click(function () {
        debugger;
        //            alert('Cancel');
        //        window.location.href = '@Url.Action("BulkPromTransferRequestDetails", "Admission")';
        window.location.href = '/Admission/BulkPromTransferRequestDetails';
    });

    $("#Section").dropdownchecklist({
        firstItemChecksAll: true,
        maxDropHeight: 150,
        Height: 80,
        width: 155
    });
    /*-------------------------------------------section--------------------------------------------------*/
    $("#Section").empty();
    $("#Section").append("<option value=' '> All </option>");
    $("#Section").dropdownchecklist('destroy');
    $("#Section").append("<option value='A'>A</option>");
    $("#Section").append("<option value='B'>B</option>");
    $("#Section").append("<option value='C'>C</option>");
    $("#Section").append("<option value='D'>D</option>");
    $("#Section").append("<option value='E'>E</option>");
    $("#Section").append("<option value='F'>F</option>");
    if ('@Model.Section' != "" & '@Model.Section' != null) {
        var sec = '@Model.Section';
        var secArr = sec.split(',');

        if (secArr != "") {
            for (var j = 0; j < secArr.length; j++) {
                //alert(secArr[j]);
                $('#Section option').filter(function () { return $(this).text() == '' + secArr[j] + '' }).attr('selected', 'selected').prop('checked', true);
            }
        }
    }



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
            }, {}, {}, {}, {}
        )



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



    //it causes some flicker when reloading or navigating grid
    //it may be possible to have some custom formatter to do this as the grid is being created to prevent this
    //or go back to default browser checkbox styles for the grid
    function styleCheckbox(table) {
        /**
        $(table).find('input:checkbox').addClass('ace')
        .wrap('<label />')
        .after('<span class="lbl align-top" />')
        
        
        $('.ui-jqgrid-labels th[id*="_cb"]:first-child')
        .find('input.cbox[type=checkbox]').addClass('ace')
        .wrap('<label />').after('<span class="lbl align-top" />');
        */
    }


    //unlike navButtons icons, action icons in rows seem to be hard-coded
    //you can change them like this in here if you want
    function updateActionIcons(table) {
        /**
        var replacement = 
        {
        'ui-ace-icon fa fa-pencil' : 'ace-icon fa fa-pencil blue',
        'ui-ace-icon fa fa-trash-o' : 'ace-icon fa fa-trash-o red',
        'ui-icon-disk' : 'ace-icon fa fa-check green',
        'ui-icon-cancel' : 'ace-icon fa fa-times red'
        };
        $(table).find('.ui-pg-div span.ui-icon').each(function(){
        var icon = $(this);
        var $class = $.trim(icon.attr('class').replace('ui-icon', ''));
        if($class in replacement) icon.attr('class', 'ui-icon '+replacement[$class]);
        })
        */
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