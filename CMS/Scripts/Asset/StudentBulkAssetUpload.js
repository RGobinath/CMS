$(function () {
    $.extend({
        handleError: function (s, xhr, status, e) {
            // If a local callback was specified, fire it
            if (s.error)
                s.error(xhr, status, e);
                // If we have some XML response text (e.g. from an AJAX call) then log it in the console
            else if (xhr.responseText)
                console.log(xhr.responseText);
        }
    });
});

//--Grid Loading
jQuery(function ($) {
    var grid_selector = "#grid-table";
    var pager_selector = "#grid-pager";
    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        jQuery(grid_selector).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery(grid_selector).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })

    jQuery(grid_selector).jqGrid({

        url: '/Asset/LoadGridLaptopDtlsInvoiceNoWiseJqGrid',
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['AssetDet_Id', 'Lap Code', 'Brand', 'Model', 'Serial No', 'Lap Size', 'Operating System','Transaction Type','Campus','Grade','Id','Name'],
        colModel: [
                                  { name: 'AssetDet_Id', index: 'AssetDet_Id', key: true, hidden: true },
                                  { name: 'AssetCode', index: 'AssetCode', sortable: true },
                                  { name: 'Make', index: 'Make', sortable: true },
                                  { name: 'Model', index: 'Model', sortable: true },
                                  { name: 'SerialNo', index: 'SerialNo', sortable: false },
                                  { name: 'LTSize', index: 'LTSize', sortable: true },
                                  { name: 'OperatingSystemDtls', index: 'OperatingSystemDtls', sortable: true },
                                  { name: 'TransactionType', index: 'TransactionType', sortable: true },
                                  { name: 'ReceivedCampus', index: 'ReceivedCampus', sortable: true },
                                  { name: 'ReceivedGrade', index: 'ReceivedGrade', sortable: true },
                                  { name: 'NewId', index: 'NewId', sortable: false},
                                  { name: 'Name', index: 'Name', sortable: false},
        ],
        viewrecords: true,
        rowNum: 9999,
        rowList: [10, 20, 30],
        autowidth: true,
        pager: pager_selector,
        sortname: 'AssetDet_Id',
        sortorder: 'desc',
        altRows: true,
        multiselect: true,
        userDataOnFooter: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);

        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp; Laptop Entry List",
    });

    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size    
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
                refresh: false,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {}, //Edit
            {}, //Add
            {},
            {},
            {});
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


    $('#btnAdd').click(function () {

        ModifiedLoadPopupDynamicaly("/Asset/AddNewDistribution", $('#newDistribution'),
                function () { LoadSetGridParam($('#newDistribution')) }, function () { }, 400, 550, "Add Students Laptop");
        //}
    });

    $("#btnexcel").click(function () {
        window.location.href = "/Asset/LaptopGenerateExcelFormat";
    });

    $("#btnUpload").click(function () {

        if ($("#ddlAcademicYear1").val() == "") {
            ErrMsg("Please Select Academic Year");
            return false;
        }

        if ($("#ddlNewCampus1").val() == "") {
            ErrMsg("Please Select Campus");
            return false;
        }

        else if ($("#ddlVendorName1").val() == "") {
            ErrMsg("Please Select Vendor Name");
            return false;
        }
        else if ($("#ddlInvoiceNo1").val() == "") {
            ErrMsg("Please Select Invoice No.");
            return false;
        }
        else if ($("#uploadedFile").val() == "") {
            ErrMsg("Please Upload the File");
            return false;
        }

        else {
            $.ajaxFileUpload({
                url: '/Asset/StudentsBulkAssetUpload?FormId=' + $("#ddlNewCampus1").val() + '&IsSubAsset=false' + '&InvoiceDetailsId=' + $("#ddlInvoiceNo1").val(),
                secureuri: false,
                fileElementId: 'uploadedFile',
                dataType: 'json',
                success: function (data, status) {
                    $('#uploadedFile').val('');
                    if (typeof data.result != 'undefined' && data.result != '') {
                        if (typeof data.success != 'undefined' && data.success == true && data.status == "success") {
                            InfoMsg(data.result);
                            reloadGrid();
                        }
                        else if (typeof data.success != 'undefined' && data.success == true && data.status == "failed") {
                            ErrMsg(data.result);
                        }
                        else {
                            ErrMsg(data.result);
                        }
                    }
                },
                error: function (data, status, e) {
                }
            });
        }
    });

    $.getJSON("/Base/FillCampus", function (fillig) {
        var ddlcam = $("#ddlNewCampus1");
        ddlcam.empty();
        ddlcam.append("<option value=''> Select </option>");
        $.each(fillig, function (index, itemdata) {
            ddlcam.append($('<option/>',
                {
                    value: itemdata.Value,
                    text: itemdata.Text
                }));
        });
    });

    var VendorType = "Purchase";
    $.getJSON("/Asset/GetVendorNameWithIdByVendorType?VendorType=" + VendorType,
      function (fillbc) {
          
          var ddlbc = $("#ddlVendorName1");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });

    $("#ddlVendorName1").change(function () {
        GetInvoiceNoByVendorIdandDocumentType1();
        if ($("#grid-table").getGridParam("reccount") > 0)
            unloadGrid();

    });

    function GetInvoiceNoByVendorIdandDocumentType1() {
        var VendorId = $("#ddlVendorName1").val();
        var ddlbc = $("#ddlInvoiceNo1");
        if (VendorId > 0) {
            $.getJSON("/Asset/GetInvoiceNoByVendorIdandDocumentType?VendorId=" + VendorId + '&DocumentType=PurchaseInvoice',
              function (fillbc) {
                  ddlbc.empty();
                  ddlbc.append($('<option/>', { value: "", text: "Select" }));
                  $.each(fillbc, function (index, itemdata) {
                      ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                  });
              });
        }
        else {
            ddlbc.empty();
            ddlbc.append($('<option/>', { value: "", text: "Select" }));
        }
    }
    
    $('#ddlInvoiceNo1').change(function () {
    
        if ($('#ddlInvoiceNo1').val() != "" && $('#ddlInvoiceNo1').val() != "Select") {
        {
        var ltest = $('#ddlInvoiceNo1').val();

            $(grid_selector).setGridParam(
                      {
                          datatype: "json",
                          url: "/Asset/LoadGridLaptopDtlsInvoiceNoWiseJqGrid",
                          postData: { lInvoiceNo: ltest },
                          page: 1
                      }).trigger('reloadGrid');
        }}
    });

    $('#ddlAcademicYear1').change(function () {
        $("select#ddlNewCampus1").val('0');
        $("select#ddlVendorName1").val('0');
        $("select#ddlInvoiceNo1").empty();
        $("select#ddlInvoiceNo1").append($('<option/>', { value: "", text: "Select" }));
        clearGrid();
    });

    $('#ddlNewCampus1').change(function ()
    {
        $("select#ddlVendorName1").val('0');
        $("select#ddlInvoiceNo1").empty();
        $("select#ddlInvoiceNo1").append($('<option/>', { value: "", text: "Select" }));
        clearGrid();
    });

    $('#btnReset').click(function () {
        
        $("select#ddlAcademicYear1").val('0');
        $("select#ddlNewCampus1").val('0');
        $("select#ddlVendorName1").val('0');
        $("select#ddlInvoiceNo1").empty();
        $("#uploadedFile").val('');
        $("select#ddlInvoiceNo1").append($('<option/>', { value: "", text: "Select" }));
        clearGrid();
    });

    //---Search Panel

    $.getJSON("/Base/FillCampus", function (fillig) {
        var ddlcam = $("#ddlsCampus");
        ddlcam.empty();
        ddlcam.append("<option value=''> Select </option>");
        $.each(fillig, function (index, itemdata) {
            ddlcam.append($('<option/>',
                {
                    value: itemdata.Text,
                    text: itemdata.Text
                }));
        });
        $('#txtsTotalLaptop').val("0");
        $('#txtsStock').val("0");
        $('#txtsDistributed').val("0");
    });

    var VendorType = "Purchase";
    $.getJSON("/Asset/GetVendorNameWithIdByVendorType?VendorType=" + VendorType,
      function (fillbc) {
          var ddlbc = $("#ddlsVendorName");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });

    $("#ddlsaAcademicYear").change(function () {
        GetInvoiceNoByVendorIdandDocumentTypeandAcademicYear();
        $("#txtsTotalLaptop").val('0');
        $("#txtsStock").val('0');
        $("#txtsDistributed").val('0');
        $("#ddlsTransactionType").val('0');
        clearGrid();
    });

    $("#ddlsCampus").change(function () {
        $("#txtsTotalLaptop").val('0');
        $("#txtsStock").val('0');
        $("#txtsDistributed").val('0');
        $("#ddlsTransactionType").val('0');
        clearGrid();
    });

    $("#ddlsVendorName").change(function () {
            clearGrid();
    });

    $("#ddlsInvoiceNo").change(function () {
        clearGrid();
    });

    function GetInvoiceNoByVendorIdandDocumentTypeandAcademicYear()
    {
        if ($("#ddlsaAcademicYear").val() == "") {
            ErrMsg("Please Select Academic Year");
            return false;
        }
        var FromDt = "";
        var ToDt = "";
        if ($("#ddlsaAcademicYear").val() != "") {
            var lAcademicYearSplited=$('#ddlsaAcademicYear').val().split("-");
            FromDt=lAcademicYearSplited[0]+"-01-01";
            ToDt=lAcademicYearSplited[1]+"-12-30";
        var ddlbc = $("#ddlsInvoiceNo");
        $.getJSON("/Asset/GetInvoiceNoByVendorIdandDocumentTypeandAcademicYear?DocumentType=PurchaseInvoice&FromDt="+FromDt+"&ToDt="+ToDt,
              function (fillbc) {
                  ddlbc.empty();
                  ddlbc.append($('<option/>', { value: "", text: "Select" }));
                  $.each(fillbc, function (index, itemdata) {
                      ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                  });
              });
        }
        else {
            ddlbc.empty();
            ddlbc.append($('<option/>', { value: "", text: "Select" }));
        }
    }
    //--Search
    $('#btnSearch').click(function () {
        
        if ($('#ddlsaAcademicYear').val() == "") { ErrMsg("Please fill the Academic Year"); return false; }
        if ($('#ddlsCampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }

        if ($("#ddlsaAcademicYear").val() == "") {
            ErrMsg("Please Select Academic Year");
            return false;
        }
        var FromDt = "";
        var ToDt = "";
        if ($("#ddlsaAcademicYear").val() != "") {
            var lAcademicYearSplited = $('#ddlsaAcademicYear').val().split("-");
            FromDt = lAcademicYearSplited[0] + "-01-01";
            ToDt = lAcademicYearSplited[0] + "-12-30";
        }

        $(grid_selector).setGridParam(
                  {
                      datatype: "json",
                      url: "/Asset/LoadGridLaptopDtlsInvoiceNoWiseJqGrid1",
                      postData: { FromDt: FromDt, ToDt: ToDt, Campus: $('#ddlsCampus').val(), VendorId: $('#ddlsVendorName').val(), InvoiceNo: $('#ddlsInvoiceNo').val(), LaptopType: $('#ddlsLaptopType').val(), OS: $('#ddlsOperatingSystem').val(), TransactionType: $('#ddlsTransactionType').val() },
                      page: 1
                  }).trigger('reloadGrid');
                  //--Laptop EntryStatus
                    GetLaptopEntryStatus();
    });

    $('#btnsReset').click(function () {

        $("select#ddlsaAcademicYear").val('0');
        $("select#ddlsCampus").val('0');
        $("select#ddlsVendorName").val('0');
        $("select#ddlsInvoiceNo").empty();
        $("select#ddlsInvoiceNo").append($('<option/>', { value: "", text: "Select" }));
        $("select#ddlsLaptopType").val('0');
        $("select#ddlsOperatingSystem").val('0');
        $("select#ddlsTransactionType").val('0');

        $("#txtsTotalLaptop").val('0');
        $("#txtsStock").val('0');
        $("#txtsDistributed").val('0');
        clearGrid();
    });
});

function GetLaptopEntryStatus() {
    if ($('#ddlsaAcademicYear').val() == "") { ErrMsg("Please fill the Academic Year"); return false; }
    if ($('#ddlsCampus').val() == "") { ErrMsg("Please fill the Campus"); return false; }

    if ($("#ddlsaAcademicYear").val() == "") {
        ErrMsg("Please Select Academic Year");
        return false;
    }
    var FromDt = "";
    var ToDt = "";
    if ($("#ddlsaAcademicYear").val() != "") {
        var lAcademicYearSplited = $('#ddlsaAcademicYear').val().split("-");
        FromDt = lAcademicYearSplited[0] + "-01-01";
        ToDt = lAcademicYearSplited[0] + "-12-30";
    }

    $.ajax({
        type: 'POST',
        dataType: "json",
        async: true,
        url: "/Asset/GetLaptopEntryStatus",
        data: { FromDt: FromDt, ToDt: ToDt, Campus: $('#ddlsCampus').val(), VendorId: $('#ddlsVendorName').val(), InvoiceNo: $('#ddlsInvoiceNo').val(), LaptopType: $('#ddlsLaptopType').val(), OS: $('#ddlsOperatingSystem').val(), TransactionType: $('#ddlsTransactionType').val() },
        success: function (data) {
            var ldata = data.to;
            if (data != "") {
                $('#txtsTotalLaptop').val(data[0]);
                $('#txtsStock').val(data[1]);
                $('#txtsDistributed').val(data[2]);
            }
            else {
                $('#txtsTotalLaptop').val("0");
                $('#txtsStock').val("0");
                $('#txtsDistributed').val("0");
            }

        }
    });

}



function reloadGrid() {
    $('#grid-table').setGridParam(
         {
             datatype: "json",
             url: '/Asset/LoadGridLaptopDtlsInvoiceNoWiseJqGrid',
             postData: { lInvoiceNo: $('#ddlInvoiceNo1').val() },
             page: 1
         }).trigger("reloadGrid");
}

function unloadGrid() {
    $('#grid-table').setGridParam(
         {
             datatype: "json",
             url: '/Asset/LoadGridLaptopDtlsInvoiceNoWiseJqGrid',
             postData: { lInvoiceNo: "" },
             page: 1
         }).trigger("reloadGrid");
}

function clearGrid()
{
    if ($("#grid-table").getGridParam("reccount") > 0)
    $("#grid-table").jqGrid("clearGridData");
}

