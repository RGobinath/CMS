jQuery(function ($) {
    var grid_selector = "#jqGridDriverMaster";
    var pager_selector = "#jqGridDriverMasterPager";

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
        url: '/Transport/DriverMasterJqGrid',
        datatype: 'json',
        height: 170,
        colNames: ['Campus', 'Name', 'Dob', 'Age', 'Gender', 'Contact No', 'License No', 'Driver Id No', 'BatchNo', 'License Val.Date', 'NonTraLicense Val.Date', 'Driver Photo', 'Status', 'Present Address', 'Permanent Address', 'Created Date', 'Created By', 'Modified Date', 'Modified By', 'Id'],
        colModel: [

            { name: 'Campus', index: 'Campus', editable: true, edittype: 'select', editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 1, colpos: 1 },
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
                }
            },
            { name: 'Name', index: 'Name', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 1, colpos: 2} },
            { name: 'Dob', index: 'Dob', editable: true, editrules: { required: false }, editoptions: {
                dataInit: function (el) {
                    $(el).datepicker({
                        format: "dd/mm/yy",
                        //timeFormat: 'hh:mm:ss',
                        showButtonPanel: false,
                        autoclose: true,
                        onSelect: function (value, ui) {
                            var now = new Date();
                            //  alert(now);
                            bD = value.split('/');
                            if (bD.length == 3) {
                                born = new Date(bD[2], bD[1] * 1 - 1, bD[0]);
                                years = Math.floor((now.getTime() - born.getTime()) / (365.25 * 24 * 60 * 60 * 1000));
                                $("#Age").val(years).attr("readonly", true);
                            }
                        }
                    }).attr('readonly', 'readonly');
                }
            },
                formoptions: { elmsuffix: ' *', rowpos: 1, colpos: 3 }
            },
            { name: 'Age', index: 'Age', width: 120, editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 2, colpos: 1} },
            { name: 'Sex', index: 'Sex', editable: true, edittype: 'select', editrules: { required: false }, editoptions: { value: "Male:Male;Female:Female;Others:Others" }, formoptions: { elmsuffix: ' *', rowpos: 2, colpos: 2} },
            { name: 'ContactNo', index: 'ContactNo', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 2, colpos: 3} },
            { name: 'LicenseNo', index: 'LicenseNo', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 3, colpos: 1} },
            { name: 'DriverIdNo', index: 'DriverIdNo', width: 170, editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 3, colpos: 2} },
             { name: 'BatchNo', index: 'BatchNo', editable: true, editrules: { required: false }, formoptions: { elmsuffix: ' *', rowpos: 3, colpos: 3} },
             { name: 'LicenseValDate', index: 'LicenseValDate', editable: true, editrules: { required: false }, editoptions: {
                 dataInit: function (el) {
                     $(el).datepicker({ format: "dd/mm/yy",
                         //changeMonth: true,
                         //timeFormat: 'hh:mm:ss',
                         autowidth: true,
                         //changeYear: true
                         // minDate: '+0d'
                     }).attr('readonly', 'readonly');
                 }
             },
                 formoptions: { elmsuffix: ' *', rowpos: 4, colpos: 1 }
             },
             { name: 'NonTraLicenseValDate', index: 'NonTraLicenseValDate', editable: true, editrules: { required: false }, editoptions: {
                 dataInit: function (el) {
                     $(el).datepicker({ format: "dd/mm/yy",
                         //changeMonth: true,
                         //timeFormat: 'hh:mm:ss',
                         autowidth: true,
                         //changeYear: true
                         // minDate: '+0d'
                     }).attr('readonly', 'readonly');
                 }
             },
                 formoptions: { elmsuffix: ' *', rowpos: 4, colpos: 2 }
             },
              { name: 'DriverPhoto', index: 'DriverPhoto', editable: true, editrules: { required: false }, edittype: 'file', editoptions: {
                  enctype: "multipart/form-data"
              }, formoptions: { elmsuffix: ' *', rowpos: 4, colpos: 3 }
              },
            { name: 'Status', index: 'Status', editable: true, edittype: 'select', formatter: showFormattedStatus, editoptions: { value: "True:Active;False:Inactive" }, formoptions: { elmsuffix: ' *', rowpos: 5, colpos: 1} },
            { name: 'PresentAddress', index: 'PresentAddress', editable: true, editrules: { required: false }, edittype: 'textarea', editoptions: { rows: "4", cols: "20", maxlength: 4000 }, formoptions: { elmsuffix: ' *', rowpos: 5, colpos: 2} },
            { name: 'PermanentAddress', index: 'PermanentAddress', editable: true, editrules: { required: false }, edittype: 'textarea', editoptions: { rows: "4", cols: "20", maxlength: 4000 }, formoptions: { elmsuffix: ' *', rowpos: 5, colpos: 3} },
            { name: 'CreatedDate', index: 'CreatedDate', editable: true, hidden: true },
            { name: 'CreatedBy', index: 'CreatedBy', editable: true, hidden: true },
            { name: 'ModifiedDate', index: 'ModifiedDate', editable: true, editoptions: { readonly: true }, hidden: true },
            { name: 'ModifiedBy', index: 'ModifiedBy', editable: true, hidden: true },
            { name: 'Id', width: 30, index: 'Id', key: true, hidden: true },
            ],
        viewrecords: true,
        rowNum: 7,
        rowList: [7, 10, 30],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Desc',
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
        caption: "<i class='ace-icon fa fa-dot-circle-o'></i>&nbsp;Driver Master"

    });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });

    $(grid_selector).jqGrid('navButtonAdd', pager_selector, {
        caption: "<i class='fa fa-file-excel-o'></i> &nbsp;Export To Excel",
        onClickButton: function () {
            window.open("DriverMasterJqGrid" + '?Campus=' + $("#txtDrvrCampus").val() + '&Name=' + $("#txtDrvrName").val()
            + '&Dob=' + $("#txtDrvrDob").val() + '&Age=' + $("#txtDrvrAge").val() + '&Sex=' + $("#txtDrvrSex").val()
            + '&LicenseNo=' + $("#txtDrvrLicenseNo").val() + '&DriverIdNo=' + $("#txtDriverIdNo").val() + '&BatchNo=' + $("#txtDrvrBatchNo").val()
            + '&LicenseValDate=' + $("#txtDrvrLicenseValDate").val() + '&NonTraLicenseValDate=' + $("#txtDrvrNonTraLicenseValDate").val() + '&Status=' + $("#txtDrvrStatus").val()
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
            }, { height: 400, width: 1100, url: '/Transport/EditDriverMasterDetails', modal: false,
                beforeSubmit: function (postdata, formid) {

                    postdata.DriverPhoto = $("#DriverPhoto").val();
                    return [true, ''];
                },
                afterSubmit: UploadImage,
                beforeShowForm: function (form) {

                    // getter
                    var changeMonth = $("#LicenseValDate").datepicker("option", "changeMonth");
                    // setter
                    $("#LicenseValDate").datepicker("option", "changeMonth", true);
                    $('#tr_CreatedDate', form).hide();
                    $('#tr_CreatedBy', form).hide();
                    $('#tr_ModifiedDate', form).hide();
                    $('#tr_ModifiedBy', form).hide();
                }
            }, { height: 400, width: 1100, url: '/Transport/AddDriverMasterDetails', modal: false,
                beforeSubmit: function (postdata, formid) {

                    postdata.DriverPhoto = $("#DriverPhoto").val();
                    return [true, ''];
                },
                afterSubmit: UploadImage,
                beforeShowForm: function (form) {
                    $("#LicenseValDate").datepicker("option", "changeMonth", true);
                    $('#tr_CreatedDate', form).hide();
                    $('#tr_CreatedBy', form).hide();
                    $('#tr_ModifiedDate', form).hide();
                    $('#tr_ModifiedBy', form).hide();
                }
            }, { url: '/Transport/DeleteDriverMasterDetails' }, {})

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
    function showFormattedStatus(cellvalue, options, rowObject) {
        if (cellvalue == 'True') {
            return 'Active';
        }
        else {
            return 'Inactive';
        }
    }
    $("#btnDrvrSearch").click(function () {
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/DriverMasterJqGrid',
                    postData: { Campus: $("#txtDrvrCampus").val(), Name: $("#txtDrvrName").val(), Dob: $("#txtDrvrDob").val(), Age: $("#txtDrvrAge").val(), Sex: $("#txtDrvrSex").val(), LicenseNo: $("#txtDrvrLicenseNo").val(), DriverIdNo: $("#txtDriverIdNo").val(), BatchNo: $("#txtDrvrBatchNo").val(), LicenseValDate: $("#txtDrvrLicenseValDate").val(), NonTraLicenseValDate: $("#txtDrvrNonTraLicenseValDate").val(), Status: $("#txtDrvrStatus").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
    $("#btnDrvrReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/DriverMasterJqGrid',
                    postData: { Campus: $("#txtDrvrCampus").val(), Name: $("#txtDrvrName").val(), Dob: $("#txtDrvrDob").val(), Age: $("#txtDrvrAge").val(), Sex: $("#txtDrvrSex").val(), LicenseNo: $("#txtDrvrLicenseNo").val(), DriverIdNo: $("#txtDriverIdNo").val(), BatchNo: $("#txtDrvrBatchNo").val(), LicenseValDate: $("#txtDrvrLicenseValDate").val(), NonTraLicenseValDate: $("#txtDrvrNonTraLicenseValDate").val(), Status: $("#txtDrvrStatus").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
});
function UploadImage(response, postdata) {

    var data = $.parseJSON(response.responseText);
    //  if (data.success == true) {
    if ($("#DriverPhoto").val() != "") {
        ajaxFileUpload(data, "DRI");
    }
    // }
    return [data.success, data.message, data.id];
}
function ajaxFileUpload(Id, AppName) {
    $.ajaxFileUpload({
        url: '@Url.Action("UploadDocuments")',
        secureuri: false,
        fileElementId: 'DriverPhoto',
        dataType: 'json',
        data: { Id: Id, AppName: AppName },
        success: function (data) {
            alert(data);
            InfoMsg(data);
        }
    })
}

function uploaddat(id1, FileName) {
    var AppName = 'DRI';
    window.location.href = "/Transport/uploaddisplay?Id=" + id1 + '&FileName=' + FileName + '&AppName=' + AppName;
    // processBusy.dialog('close');
}

function showFormattedStatus(cellvalue, options, rowObject) {
    if (cellvalue == 'True') {
        return 'Active';
    }
    else {
        return 'Inactive';
    }
}