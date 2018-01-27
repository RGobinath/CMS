
    $(function () {
    
     var grid_selector = "#StudentAchievementlistgrid";
        var pager_selector = "#StudentAchievementlistgridPager";
        $('#btnAddAchievement').click(function () {
            var semtype = $('#SASemester').val();
            var sadesc = $('#SADescription').val();
            var satype=$('#SAAchieveType').val();
            objAssess360Achievement = {
                Id: $('#AchievementId').val(),
                RefId: $('#Id').val(),
                Campus: $('#hdnCampus').val(),
                Grade: $('#Grade').val(),
                AcademicYear: $('#AcademicYear').val(),
                SemesterType: semtype,
                AchievementType:satype,
                AchievementDescription: sadesc,
                CreatedDate: $("#createddate").val(),
                CreatedBy: $("#createdby").val(),
            };
            if (semtype != "" && sadesc != ""&&satype!="") {
                $.ajax({
                    url: '/Assess360/AddReportCardAchievementdetails/',
                    type: 'POST',
                    dataType: 'json',
                    data: objAssess360Achievement,
                    traditional: true,
                    success: function (data) {
                        if (data == "Updated") 
                            InfoMsg("Student Achievement "+ data +" successfully");
                        else
                            InfoMsg("Student Achievement Added successfully");
                        $(grid_selector).trigger("reloadGrid");
                        $('#btnResetAchieve').click();
                    },
                    loadError: function (xhr, status, error) {
                        msgError = $.parseJSON(xhr.responseText).Message;
                        ErrMsg(msgError, function () { });
                    }
                });
            }
            else {
                ErrMsg("Please fill all mandatory fields!");
                return false;
            }

        });
        $('#btnResetAchieve').click(function () {
           // $("input[type=text], textarea, select").val("");
            $('#SASemester').val("");
            $('#SADescription').val("");
            $('#SAAchieveType').val("");
            $('#StudentAchieveTab').click();
        });



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

        function styleCheckbox(table) {
        }


        //unlike navButtons icons, action icons in rows seem to be hard-coded
        //you can change them like this in here if you want
        function updateActionIcons(table) {

        }
        //pager icon


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
            mtype: 'GET',
            url: '/Assess360/GetStudentAchievementListJqGrid',
            postData: { RefId: $('#Id').val() },
            datatype: 'json',
            height: '120',
            autowidth:true,
            shrinkToFit: true,
            colNames: ['Id', 'RefId', 'Semester Type', 'Achievement Type', 'Achievement', 'Created Date', 'Created By', '', ''],
            colModel: [
                          { name: 'Id', index: 'Id', hidden: true, key: true },
                          { name: 'RefId', index: 'RefId', hidden: true },
                          { name: 'SemesterType', index: 'SemesterType' },
                          { name: 'AchievementType', index: 'AchievementType' },
                          { name: 'AchievementDescription', index: 'AchievementDescription', sortable: false },
                          { name: 'CreatedDate', index: 'CreatedDate', sortable: false },
                          { name: 'CreatedBy', index: 'CreatedBy', sortable: false },
                          { name: 'Update', index: 'Update', width: 30, align: 'center', sortable: false, formatter: frmSAUpdate },
                          { name: 'Delete', index: 'Delete', width: 30, align: "center", sortable: false, formatter: frmSADel }
                          ],
            pager: pager_selector,
            rowNum: '10',
            rowList: [5, 10, 20, 50, 100, 150, 200],
            viewrecords: true,
            sortname: 'Id',
            sortorder: 'Desc',
            caption:'<i class="fa fa-trophy"></i>&nbsp;Achievement / Awards',
            loadComplete: function () {
            var table=this;

                var rdata = $(grid_selector).getRowData();
                if (rdata.length > 0) {
                    $('.SAUpdate').click(function () { UpdateAchievement($(this).attr('rowid')); });
                    $('.SADel').click(function () { DeleteAchievement($(this).attr('rowid')); });
                }
                 setTimeout(function () {

                    styleCheckbox(table);
                    updateActionIcons(table);
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(window).triggerHandler('resize.jqGrid');
            }
        });

    });
    function UpdateAchievement(id) {
        $('#btnAddAchievement').val('Update');
        var rowData = $('#StudentAchievementlistgrid').getRowData(id);
        $('#AchievementId').val(rowData.Id);
        $('#SASemester').val(rowData.SemesterType);
        $('#SAAchieveType').val(rowData.AchievementType);
        $('#SADescription').val(rowData.AchievementDescription);
        $("#createddate").val(rowData.CreatedDate);
        $("#createdby").val(rowData.CreatedBy);
    }
    function DeleteAchievement(id) {
    debugger;
            if (confirm("Are you sure you want to delete this item?")) {
                // your deletion code
                DeleteComponentIds(
                '/Assess360/DeleteStudentAchievement?studAchieveIds=' + id, //delURL, 
                '/Assess360/GetStudentAchievementListJqGrid?RefId=' + $('#Id').val(), //reloadURL, 
                $('#StudentAchievementlistgrid') //GridId, 
                );
            }
            return false;
        }
    function frmSAUpdate(cellvalue, options, rowdata) {
        var saveBtn = "";
        if (rowdata[6] == $('#loggedInUserId').val()) {
            saveBtn = "<span id='SAbtnUpdate_" + options.rowId + "'class='fa fa-pencil blue SAUpdate'  rowid='" + options.rowId + "' title='Update'></span>";
        }
        return saveBtn;
    }
    function frmSADel(cellvalue, options, rowdata) {
        var delBtn = "";
        if (rowdata[6] == $('#loggedInUserId').val()) {
            delBtn = "<span id='SAbtnDel_" + options.rowId + "'class='fa fa-trash-o red SADel' rowid='" + options.rowId + "' title='Delete'></span>";
        }
        return delBtn;
    }