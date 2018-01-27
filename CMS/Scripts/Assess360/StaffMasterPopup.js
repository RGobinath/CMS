$(function () {
    var brUrl1 = '/Assess360/GetStaffMasterDetails?Grade=' + $('#StaffGrade').val()
            + '&SubjectTeaching=' + $('#SubjectTeaching').val()
            + '&StaffName=' + $('#StaffName').val();
    $(function () {
        var grid_selector = "#StaffMasterList";
        var pager_selector = "#StaffMasterPager";
        $(grid_selector).jqGrid({
            datatype: 'local',
            colNames: ['Id', 'AppCode', 'RoleCode', 'BranchCode', 'UserId', 'Staff Name', 'DeptCode'],
            colModel: [
              { name: 'Id', index: 'Id', key: true, hidden: true },
              { name: 'AppCode', index: 'AppCode', hidden: true },
              { name: 'RoleCode', index: 'RoleCode', hidden: true },
              { name: 'BranchCode', index: 'BranchCode', hidden: true },
              { name: 'UserId', index: 'UserId', hidden: true },
              { name: 'UserName', index: 'UserName' },
              { name: 'DeptCode', index: 'DeptCode', hidden: true }
            ],
            pager: pager_selector,
            rowNum: 10,
            rowList: [10, 15, 20, 50],
            sortname: 'StaffName',
            sortorder: 'asc',
            viewrecords: true,
            autowidth: true,
            caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;Staff Details',
            onSelectRow: function (rowid, status) {
                rowData = $(grid_selector).getRowData(rowid);
                if (clbPupGrdSel1 != undefined && clbPupGrdSel1) { clbPupGrdSel1(rowData); }
                clbPupGrdSel1 = null;
                $('#DivStaffMasterSearch').dialog('close');
            },
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(window).triggerHandler('resize.jqGrid');
            }
        });
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
            },{},{}, {}, {})
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


        /*enter key press event*/
        function defaultFunc(e) {
            if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                $('#btnStaffSearch').click();
                return false;
            }
            return true;
        };
        $('#StaffName').click(function () {
            ('#StaffName').val() = "";
        });
        
        $('#StaffName').keypress(function (event) {
            if (isEmptyorNull($('#StaffName').val())) {
                return true;
            }
            else {
                return defaultFunc(event);
            }
        });
        $('#SubjectTeaching').keypress(function (event) { if (isEmptyorNull($('#SubjectTeaching').val())) { return true; } else { return defaultFunc(event); } });
        /*enter key press event*/

        $("#StaffGrade").attr("readonly", true).css("background-color", "#F5F5F5");

        $("#btnStaffSearch").click(function () {
            $(grid_selector).clearGridData();
            if (!isEmptyorNull($("#StaffGrade").val()) || !isEmptyorNull($("#StaffName").val())
            || !isEmptyorNull($("#SubjectTeaching").val())) {
                LoadSetGridParam($(grid_selector), '/Assess360/GetStaffMasterDetails?Grade=' + $('#StaffGrade').val()
            + '&SubjectTeaching=' + $('#SubjectTeaching').val()
            + '&StaffName=' + $('#StaffName').val());
            } else {
                ErrMsg('Please enter the feilds', function () { $("#name").focus(); });
                return false;
            }
        });

        $("#btnStaffReset").click(function () {
            // 
            $(grid_selector).clearGridData();
            $('#StaffName').val("");
            $('#SubjectTeaching').val("");
            LoadSetGridParam($(grid_selector), "/Assess360/GetStaffMasterDetails?Grade=" + $('#StaffGrade').val());
        });
    });
});