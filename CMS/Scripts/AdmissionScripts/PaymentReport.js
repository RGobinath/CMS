$(function () {
    var grid_selector = "#paymentReportList";
    var pager_selector = "#paymentReportPage";

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
     $("#ddlcampus").change(function () {
        gradeddl();
    });
    
    $(grid_selector).jqGrid({
        mtype: 'GET',
        url: '/Admission/PaymentReportJqgrid',
        //postData: { Assess360Id: $('#Id').val() },
        datatype: 'json',
        height: '250',
        autowidth: true,
        colNames: ['Id', 'PreRegNum', 'Name', 'Campus', 'Grade', 'Boarding Type', 'Academic Year', 'Fee Type', 'Mode Of Payment', 'Chq/DD Date', 'ReferenceNo', 'Bank Name', 'Amount', 'Remarks','Paid Date','Cleared Date'],
        colModel: [{ name: 'Id', index: 'Id', hidden: true, key: true },
                   { name: 'PreRegNum', index: 'PreRegNum',hidden:true },
                   { name: 'Name', index: 'Name',width:200 },
                   { name: 'Campus', index: 'Campus',width:120 },
                   { name: 'Grade', index: 'Grade',width:50 },
                   { name: 'BoardingType', index: 'BoardingType',width:100 },
                   { name: 'AcademicYear', index: 'AcademicYear',hidden:true },
                   { name: 'FeeType', index: 'FeeType',width:100 },
                   { name: 'ModeOfPayment', index: 'ModeOfPayment',width:120 },
                   { name: 'ChequeDate', index: 'ChequeDate',width:100 },
                   { name: 'ReferenceNo', index: 'ReferenceNo',  },
                   { name: 'BankName', index: 'BankName' },
                   { name: 'Amount', index: 'Amount',width:100},
                   { name: 'Remarks', index: 'Remarks' },
                   { name: 'PaidDate', index: 'PaidDate',width:100 },
                   { name: 'ClearedDate', index: 'ClearedDate',width:100 }
                   ],
        pager: pager_selector,
        rowNum: '20',
        rowList: [20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        viewrecords: true,
        caption: '<i class="ace-icon fa fa-th-list"></i>&nbsp;&nbsp;Payment Details Report',
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
     jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: "Export To Excel",
            onClickButton: function () {
            var name= $('#Name').val();
        var cam=$('#ddlcampus').val();
        var grd=$('#ddlgrade').val();
                var ExpExcel = "Excel";
                window.open("PaymentReportJqgrid?Name=" + name+'&Campus=' + cam + '&Grade=' + grd + '&sidx=Id&sord=desc&rows=9999' + '&ExpExcel=' + ExpExcel);
            }
        });
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
    $('#btnSearch').click(function () {
       var name= $('#Name').val();
        var cam=$('#ddlcampus').val();
        var grd=$('#ddlgrade').val();
        var feetype=$('#feetypeddl').val();
        var modpay=$('#modeofpmtddl').val();
        $(grid_selector).clearGridData();
        LoadSetGridParam($(grid_selector), "/Admission/PaymentReportJqgrid?Name=" + name + '&Campus=' + cam + '&Grade=' + grd +'&feetype=' + feetype +'&modeofpmtddl='+ modpay);
    });

    $('#btnReset').click(function () {
        $(grid_selector).clearGridData();
        $('#Name').val('');
        $('#ddlcampus').val('');
        $('#ddlgrade').val('');
        $('#feetypeddl').val('');
        $('#modeofpmtddl').val('');
        //jQuery(grid_selector).jqGrid('clearGridData').jqGrid('setGridParam').trigger('reloadGrid');
        LoadSetGridParam($(grid_selector), "/Admission/PaymentReportJqgrid?page=1");
    });

    /*enter key press event*/
    function defaultFunc(e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('#btnSearch').click();
            return false;
        }
        return true;
    };
});
function gradeddl() {
    var e = document.getElementById('ddlcampus');
    var campus = e.options[e.selectedIndex].value;
    //     alert(campus);
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
                function (modelData) {
                    var select = $("#ddlgrade");
                    select.empty();
                    select.append($('<option/>', { value: "", text: "--Select Grade--" }));
                    $.each(modelData, function (index, itemData) {
                        select.append($('<option/>', { value: itemData.gradcod, text: itemData.gradcod }));
                    });
                });
}
function GeneratePDF(assessId, semester) {
    window.open('/Assess360/GeneratePDF?assessid=' + assessId + '&term=' + semester);
}
