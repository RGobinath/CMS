    $(function () {

        var grid_selector = "#BulkRptCreation";
        var pager_selector = "#BulkRptCreationPager";

        //resize to fit page size
        $(window).on('resize.jqGrid', function () {
            $(grid_selector).jqGrid('setGridWidth', $("#jqgrid").width());
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
        // Autocomplete Search Example....
        $("#txtTeachName").autocomplete({
            source: function (request, response) {
                $.getJSON('/AchievementReport/GetTeacherName?term=' + request.term, function (data) {
                    response(data);
                });
            },
            minLength: 1,
            delay: 100
        });

        $(grid_selector).jqGrid({
            url: '/AchievementReport/JqgridBulkRptCreation',
            datatype: 'Json',
            type: 'GET',
            height: 220,
            autowidth: true,
            colNames: ['Id', 'StudentId', 'Id No', 'Name', 'Campus Name', 'Grade', 'Section', 'Academic Year'],
            colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true }, //0
                       {name: 'PreRegNum', index: 'PreRegNum', hidden: true }, //7
                       {name: 'IdNo', index: 'IdNo', sortable: false }, //3
                       {name: 'Name', index: 'Name', sortable: false }, //4
                       {name: 'Campus', index: 'Campus', sortable: false }, //2
                       {name: 'Grade', index: 'Grade', sortable: false }, //6
                       {name: 'Section', index: 'Section', sortable: false }, //5
                       {name: 'AcademicYear', index: 'AcademicYear', sortable: false }, //5
                          ],
            rowNum: 50,
            rowList: [50, 100, 150, 200],
            sortname: 'Id',
            sortorder: 'asc',
            viewrecords: true,
            shrinkToFit: true,
            pager: pager_selector,
            caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbspBulk Report Card Creation',
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
            }
        });

        //navButtons Add, edit, delete
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
            {}, {}, {})

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
        $('#btnsearch').click(function () {
//            if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
//            if ($('#Grade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
//            if ($('#semester').val() == "") { ErrMsg("Please fill the semester"); return false; }
//            if ($('#AcademicYear').val() == "Select") { ErrMsg("Please fill the AcademicYear"); return false; }

            $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: "/AchievementReport/JqgridBulkRptCreation",
                    postData: { campus: $('#Campus').val(), grade: $('#Grade').val(), semester: $('#semester').val(), academicyear: $('#AcademicYear').val(), section: $('#Section').val() },
                    page: 1
                }).trigger("reloadGrid");

        });

        $('#btnreset').click(function () {
            $('#Campus').val("");
            $('#Grade').val("");
            $('#AcademicYear').val("");
            $('#Section').val("");
            $('#semester').val("");
            $('#txtTeachName').val("");
            $('#txtDate').val("");
            jQuery(grid_selector).jqGrid('clearGridData')
                    .jqGrid('setGridParam', { data: data, page: 1 }).trigger('reloadGrid');

            // window.location.href = '@Url.Action("BulkReportCardCreationMYP", "AchievementReport")';
        });

        $('#btnass360').click(function () {
            if ($('#Campus').val() == "") { ErrMsg("Please fill the Campus"); return false; }
            if ($('#Grade').val() == "") { ErrMsg("Please fill the Grade"); return false; }
            if ($('#AcademicYear').val() == "Select") { ErrMsg("Please fill the AcademicYear"); return false; }
            if ($('#semester').val() == "") { ErrMsg("Please fill the semester"); return false; }
            if ($('#txtTeachName').val() == "") { ErrMsg("Please fill the Teacher Name"); return false; }
            if ($('#txtDate').val() == "") { ErrMsg("Please fill the Date"); return false; }


            $.ajax({
                url: "/AchievementReport/SaveBulkReportCardCreation",
                type: "POST",
                data: { campus: $('#Campus').val(), grade: $('#Grade').val(), semester: $('#semester').val(), academicyear: $('#AcademicYear').val(), section: $('#Section').val(), teachName: $('#txtTeachName').val(), rptDate: $('#txtDate').val() },
                success: function (data, textStatus, jqXHR) {
                    //data - response from server
                    InfoMsg("successfully created.", function () { window.location.href = "/AchievementReport/ReportCardInbox/" });
                },
                error: function (jqXHR, textStatus, errorThrown) {

                }
            });

        });

        $.getJSON("/Base/FillBranchCode",
        function (fillig) {
            var ddlcam = $("#Campus");
            ddlcam.empty();
            ddlcam.append($('<option/>',
                            {
                                value: "",
                                text: "Select one"
                            }));
            $.each(fillig, function (index, itemdata) {
                ddlcam.append($('<option/>',
                {
                    value: itemdata.Value,
                    text: itemdata.Text
                }));
            });
        });
    });
