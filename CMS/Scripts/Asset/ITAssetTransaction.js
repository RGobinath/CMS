function FillAssetTypeDll() {
    $.getJSON("/Asset/FillITSubAssetName",
      function (fillbc) {
          var ddlbc = $("#ddlAssetType");
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select Asset Type" }));

          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
          });
      });
}
function FillToBlockByCampus() {
    var Campus = "";
    if ($("#ddlTransactionType").val() == "InterCampus") {
        Campus = $("#ddlToCampus").val();
    }
    else {
        Campus = $("#txtFromCampus").val();
    }
    var ddlbc = $("#ddlToBlock");
    if (Campus != "" && Campus != null) {
        debugger;
        $.getJSON("/Asset/GetBlockByCampus?Campus=" + Campus,
          function (fillbc) {
              debugger;
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillToLocationByCampusWithBlock(Block) {
    debugger;
    var Campus = "";
    if ($("#ddlTransactionType").val() == "InterCampus") {
        Campus = $("#ddlToCampus").val();
    }
    else {
        Campus = $("#txtFromCampus").val();
    }
    if ($("#IsSubAsset").val() == "True") {
    }
    else {
        Block = $("#ddlToBlock").val();
    }
    var ddlbc = $("#ddlToLocation");
    if (Campus != "" && Campus != null && Block != "" && Block != null) {
        debugger;
        $.getJSON("/Asset/GetLocationByCampusWithBlock?Campus=" + Campus + '&Block=' + Block,
        function (fillbc) {
            debugger;
            ddlbc.empty();
            ddlbc.append($('<option/>', { value: "", text: "Select" }));
            $.each(fillbc, function (index, itemdata) {
                if ($("#IsSubAsset").val() == "True" && itemdata.Text == $("#hiddenToLocation").val()) {
                    ddlbc.append("<option value='" + itemdata.Text + "' selected='selected'>" + itemdata.Text + "</option>");
                }
                else {
                    ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
                }
            });
            if ($("#ddlTransactionType").val() == "IntraCampus") {
                if ($("#txtFromBlock").val() == $("#ddlToBlock").val()) {
                    $("#ddlToLocation option[value='" + $("#txtFromLocation").val() + "']").hide();
                }
            }
        });

    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function GetVendorNameByVendorType() {
    debugger;
    var ddlbc = $("#ddlVendor");
    $.getJSON("/Asset/GetVendorNameByVendorType?VendorType=Service,Both",
      function (fillbc) {
          debugger;
          ddlbc.empty();
          ddlbc.append($('<option/>', { value: "", text: "Select" }));
          $.each(fillbc, function (index, itemdata) {
              ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
          });
      });
}
function FillStaffGroupByCampus() {
    debugger;
    var Campus = "";
    if ($("#ddlTransactionType").val() == "IntraCampus") {
        Campus = $("#txtFromCampus").val();
    }
    if ($("#ddlTransactionType").val() == "InterCampus") {
        Campus = $("#ddlToCampus option:selected").text();
    }
    var ddlbc = $("#ddlNewStaffGroup");
    if (Campus != "" && Campus != null) {
        $.getJSON("/Base/StaffGroupByCampus?Campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillDepatmentByCampusandGroup() {
    var Campus = "";
    if ($("#ddlTransactionType").val() == "IntraCampus") {
        Campus = $("#txtFromCampus").val();
    }
    if ($("#ddlTransactionType").val() == "InterCampus") {
        Campus = $("#ddlToCampus option:selected").text();
    }
    var StaffGroup = $("#ddlNewStaffGroup").val();
    var ddlbc = $("#ddlNewDepartment");
    if (Campus != "" && Campus != null && StaffGroup != "" && StaffGroup != null) {
        $.getJSON("/Base/DepatmentByCampusandGroup?Campus=" + Campus + '&GroupName=' + StaffGroup,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  if (itemdata.Text == $("#hiddenNewDepartment").val()) {
                      ddlbc.append("<option value='" + itemdata.Text + "' selected='selected'>" + itemdata.Text + "</option>");
                  }
                  else {
                      ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
                  }
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillStaffNameByCampusWithGroupandDepartment() {
    var Campus = "";
    if ($("#ddlTransactionType").val() == "IntraCampus") {
        Campus = $("#txtFromCampus").val();
    }
    if ($("#ddlTransactionType").val() == "InterCampus") {
        Campus = $("#ddlToCampus option:selected").text();
    }
    var StaffGroup = $("#ddlNewStaffGroup").val();
    var Department = $("#ddlNewDepartment").val();
    var ddlbc = $("#ddlIdNum");
    if (Campus != "" && Campus != null && StaffGroup != "" && StaffGroup != null) {
        $.getJSON("/Base/StaffNameByCampusWithGroupandDepartment?Campus=" + Campus + '&GroupName=' + StaffGroup + '&Department=' + Department,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  if (itemdata.Value == $("#hiddenIdNum").val())
                  {
                      ddlbc.append("<option value='" + itemdata.Value + "' selected='selected'>" + itemdata.Text + "</option>");
                  }
                  else
                  {
                      ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                  }
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillGradeByCampus() {
    var Campus = "";
    if ($("#ddlTransactionType").val() == "IntraCampus") {
        Campus = $("#txtFromCampus").val();
    }
    if ($("#ddlTransactionType").val() == "InterCampus") {
        Campus = $("#ddlToCampus option:selected").text();
    }
    var ddlbc = $("#ddlNewGrade");
    if (Campus != "" && Campus != null) {
        $.getJSON("/Assess360/GetGradeByCampus?Campus=" + Campus,
          function (fillbc) {
              ddlbc.empty();
              ddlbc.append($('<option/>', { value: "", text: "Select" }));
              $.each(fillbc, function (index, itemdata) {
                  ddlbc.append($('<option/>', { value: itemdata.Text, text: itemdata.Text }));
              });
          });
    }
    else {
        ddlbc.empty();
        ddlbc.append($('<option/>', { value: "", text: "Select" }));
    }
}
function FillStudNameByCampusWithGrade() {
    var Campus = "";
    if ($("#ddlTransactionType").val() == "IntraCampus") {
        Campus = $("#txtFromCampus").val();
    }
    if ($("#ddlTransactionType").val() == "InterCampus") {
        Campus = $("#ddlToCampus option:selected").text();
    }
    var Grade = $("#ddlNewGrade").val();
    var ddlbc = $("#ddlIdNum");
    if (Campus != "" && Campus != null && Grade != "" && Grade != null) {
        $.getJSON("/Base/StudentNameByCampusWithGrade?Campus=" + Campus + '&Grade=' + Grade,
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
var AssetTransactionType = $("#ddlTransactionType").val();
var AssetUserType = $("#UserType").val();
$(function ($) {            
    FillAssetTypeDll();
    CalculatePendingAge();
    GetVendorNameByVendorType();
    $("#ddlTransactionType option[value=Inward]").hide();
    if (AssetUserType == "Student")
    {
        $("#ddlTransactionType option[value=Stock]").hide();
    }
    if (AssetTransactionType == "Scrap" || AssetTransactionType == "Service") {
        document.getElementById("ddlTransactionType").disabled = true;        
        document.getElementById("btnSave").disabled = true;
    }
    $("#ddlTransactionType").val('');
    if (AssetTransactionType == "Stock" || $("#txtFromLocation").val() == "Stock") {
        $("#ddlTransactionType option[value=Stock]").hide();
        $("#ddlTransactionType option[value=Service]").hide();
        $("#ddlTransactionType option[value=Scrap]").hide();
    }
    if (AssetUserType == "Student" && $("#IsSubAsset").val() == "True") {        
        $("#ddlTransactionType option[value=InterCampus]").hide();
        $("#ddlTransactionType option[value=IntraCampus]").hide();
    }
    $("#ddlUserType").val('');
    $("#UserTypeDiv").hide();
    $('#divToCampusLocation').hide();
    $('#divInstallDate').hide();
    $('#divService').hide();
    $('#divScrap').hide();
    $('#divTransactionComment').hide();
    $('#StudentDiv').hide();
    $('#StaffDiv').hide();
    $('#DivIdNum').hide();
    $("#txtITAssetCode").val('');
    $('.date-picker').datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        startDate: new Date(),
        endDate: new Date(),
    });
    $("#txtExpectedDate").datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        startDate: new Date()
    });
    var todaydate = new Date();
    var dd = todaydate.getDate();
    var mm = todaydate.getMonth() + 1; //January is 0!

    var yyyy = todaydate.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    todaydate = dd + '/' + mm + '/' + yyyy;
    $("#txtInstalledOn").val(todaydate);
    $("#txtInwardDate").val(todaydate);
    $("#txtDCDate").val(todaydate);
    $('#ddlTransactionType').change(function () {
        //$("#ddlToBlock").val("");
        $("#ddlToBlock,#hiddenToLocation,#txtITAssetCode,#HiddenAssetDet_Id,#ddlIdNum").val('');
        if ($('#ddlTransactionType').val() == "IntraCampus") {
            if (AssetTransactionType == "Stock") {
                if (AssetUserType == "Student") {
                    $("#UserTypeDiv").hide();
                } else {
                    $('#UserTypeDiv').show();
                }
            }
            else {
                if (AssetUserType == "Student") {
                    $("#UserTypeDiv").hide();
                } else {
                    $('#UserTypeDiv').show();
                    $("#ddlUserType option[value=Student]").hide();
                }
            }
            $('#divService').hide();
            $('#divScrap').hide();
            $('#divToCampus').hide();
            $('#txtToCampus').attr('readonly', true);
            $('#divToCampusLocation').show();
            $('#divTransactionComment').show();
            $('#divInstallDate').show();
            FillToBlockByCampus();
            //FillLocationByCampusWithBlock(); // commented By prabakaran
            FillToLocationByCampusWithBlock("");
            FillStaffGroupByCampus();
            FillGradeByCampus();
        }
        else if ($('#ddlTransactionType').val() == "InterCampus") {
            debugger;
            if (AssetTransactionType == "Stock") {
                if (AssetUserType == "Student") {
                    $("#UserTypeDiv").hide();
                } else {
                    $('#UserTypeDiv').show();
                }
            }
            else {
                if (AssetUserType == "Student") {
                    $("#UserTypeDiv").hide();
                } else {
                    $('#UserTypeDiv').show();
                    $("#ddlUserType option[value=Student]").hide();
                }
            }
            $('#divService').hide();
            $('#divScrap').hide();
            $('#txtToCampus').attr('readonly', false);
            $('#divToCampusLocation').show();
            $('#divInstallDate').show();
            $('#divTransactionComment').show();
            $('#divToCampus').show();
            $("#ddlToCampus option[value='" + $("#txtFromCampus").val() + "']").hide();
            FillToBlockByCampus();
            FillToLocationByCampusWithBlock("");
            FillStaffGroupByCampus();
            FillGradeByCampus();
        }
        else if ($('#ddlTransactionType').val() == "Service") {
            $('#UserTypeDiv').hide();
            $('#divToCampusLocation').hide();
            $('#divService').show();
            $('#divScrap').hide();
            $('#divTransactionComment').hide();
            $('#divInstallDate').hide();
            $('#StudentDiv').hide();
            $('#StaffDiv').hide();
            $('#DivIdNum').hide();
            $('#ddlUserType,#ddlNewStaffGroup,#ddlNewDepartment,#ddlIdNum').val('');
        }
        else if ($('#ddlTransactionType').val() == "Scrap") {
            $('#UserTypeDiv').hide();
            $('#divService').hide();
            $('#divToCampusLocation').hide();
            $('#divScrap').show();
            $('#divTransactionComment').hide();
            $('#divInstallDate').hide();
            $('#StudentDiv').hide();
            $('#StaffDiv').hide();
            $('#DivIdNum').hide();
            $('#ddlUserType,#ddlNewStaffGroup,#ddlNewDepartment,#ddlIdNum').val('');
        }
        else if ($('#ddlTransactionType').val() == "Stock") {
            $('#UserTypeDiv').hide();
            $('#divService').hide();
            $('#divToCampusLocation').hide();
            $('#divScrap').hide();
            $('#divTransactionComment').show();
            $('#divInstallDate').show();
            $('#StudentDiv').hide();
            $('#StaffDiv').hide();
            $('#DivIdNum').hide();
            $('#ddlUserType,#ddlNewStaffGroup,#ddlNewDepartment,#ddlIdNum').val('');
        }
        else if ($('#ddlTransactionType').val() == "") {
            $('#UserTypeDiv').hide();
            $('#divToCampusLocation').hide();
            $('#divService').hide();
            $('#divScrap').hide();
            $('#divTransactionComment').hide();
            $('#divInstallDate').hide();
            $('#StudentDiv').hide();
            $('#StaffDiv').hide();
            $('#DivIdNum').hide();
            $('#ddlUserType,#ddlNewStaffGroup,#ddlNewDepartment,#ddlIdNum').val('');
        }
    });
    $('#ddlUserType').change(function () {
        cacheval = {};
        $("#ddlToBlock,#hiddenToLocation,#txtITAssetCode,#ddlIdNum").val('');
        $("#txtITAssetCode,").text('');
        FillToLocationByCampusWithBlock("");
        if ($('#ddlUserType').val() == "Staff") {
            $('#StudentDiv').hide();
            $('#StaffDiv').show();
            $('#DivIdNum').show();
            $("#ddlNewStaffGroup,#ddlNewDepartment").val('');
        }
        else if ($('#ddlUserType').val() == "Student") {
            $('#StaffDiv').hide();
            $('#StudentDiv').show();
            $('#DivIdNum').show();
        }
        else {
            $('#StudentDiv').hide();
            $('#StaffDiv').hide();
            $('#DivIdNum').hide();
        }
    });
    $("#ddlNewStaffGroup").change(function () {
        FillDepatmentByCampusandGroup();
    });
    $("#ddlNewDepartment").change(function () {
        FillStaffNameByCampusWithGroupandDepartment();
    });
    $('#btnback').click(function () {
        window.location.href = '/Asset/ITAssetManagement';
    });
    $('#btnback1').click(function () {
        window.location.href = '/Asset/ITAssetTransaction?AssetDet_Id=' + $("#AssetRefId").val();
    });
    $("#AddSubAsset").click(function () {
        if ($("#ddlAssetType").val() == "") {
            ErrMsg("Please Select Asset Type");
            return false;
        }
        else {
            ModifiedLoadPopupDynamicaly("/Asset/AddNewITSubAsset?AssetDet_Id=" + $('#AssetDet_Id').val() + '&AssetId=' + $("#ddlAssetType").val(), $('#newITSubAsset'), function () {
                LoadSetGridParam($('#newITSubAsset'))
            }, function () { }, 400, 500, "Add Sub Asset");
        }
    });
    //$(window).on('resize.jqGrid', function () {
    //    $("#Uploadedfileslist").jqGrid('setGridWidth', $(".col-sm-8").width());
    //})
    ////resize on sidebar collapse/expand
    //var parent_column = $("#Uploadedfileslist").closest('[class*="col-"]');
    //$(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
    //    if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
    //        //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
    //        setTimeout(function () {
    //            $("#Uploadedfileslist").jqGrid('setGridWidth', parent_column.width());
    //        }, 0);
    //    }
    //})
    $(window).on('resize.jqGrid', function () {
        jQuery("#Uploadedfileslist").jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column1 = $("#Uploadedfileslist").closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery("#Uploadedfileslist").jqGrid('setGridWidth', parent_column1.width());
            }, 0);
        }
    })
    jQuery("#Uploadedfileslist").jqGrid({
        mtype: 'GET',
        url: '/Asset/Documentsjqgrid?id=' + $('#AssetDet_Id').val(),
        datatype: 'json',
        height: '50',
        autowidth: true,
        shrinkToFit: true,
        colNames: ['Invoice Details Id', 'Vendor Name', 'Invoice No.', 'Document Type', 'Document Name', 'Document Size', 'Uploaded Date', 'Invoice Date', 'Amount', 'Warranty', 'Expiry Date', 'Is Expired', 'Warranty Age'],
        colModel: [
                        { name: 'InvoiceDetailsId', index: 'InvoiceDetailsId', hidden: true },
                        { name: 'VendorId', index: 'VendorId', width: 307, align: 'left', sortable: false },
                        { name: 'InvoiceNo', index: 'InvoiceNo', width: 307, align: 'left', sortable: false },
                        { name: 'DocumentType', index: 'DocumentType', width: 307, align: 'left', sortable: false },
                        { name: 'DocumentName', index: 'DocumentName', width: 307, align: 'left', sortable: false },
                        { name: 'DocumentSize', index: 'DocumentSize', width: 307, align: 'left', sortable: false },
                        { name: 'UploadedDate', index: 'UploadedDate', width: 200, align: 'left', sortable: false },
                        { name: 'InvoiceDate', index: 'InvoiceDate', width: 200, align: 'left', sortable: false },
                        { name: 'Amount', index: 'Amount', width: 307, formatter: 'currency', formatoptions: { prefix: "<i class='fa fa-inr'></i> ", thousandsSeparator: ",", decimalPlaces: 2, defaultValue: '0.00' }, sortable: false },
                        { name: 'Warranty', index: 'Warranty', width: 250, align: 'left', sortable: false },
                        { name: 'ExpiryDate', index: 'ExpiryDate', width: 200, align: 'left', sortable: false },
                        { name: 'IsExpired', index: 'IsExpired', width: 130, align: 'left', sortable: false },
                        { name: 'WarrantyAge', index: 'WarrantyAge', width: 130, align: 'left', sortable: false },
                      //{ name: 'Id', index: 'Id', hidden: true, key: true },
                      //{ name: 'DocumentType', index: 'DocumentType', width: 307, align: 'left', sortable: false },
                      //{ name: 'DocumentType', index: 'DocumentType', width: 307, align: 'left', sortable: false },
                      //{ name: 'DocumentType', index: 'DocumentType', width: 307, align: 'left', sortable: false },

                      //{ name: 'DocumentType', index: 'DocumentType', width: 307, align: 'left', sortable: false },
                      //{ name: 'DocumentName', index: 'DocumentName', width: 307, align: 'left', sortable: false },
                      //{ name: 'DocumentSize', index: 'DocumentSize', width: 307, align: 'left', sortable: false },
                      //{ name: 'UploadedDate', index: 'UploadedDate', width: 307, align: 'left', sortable: false }
        ],
        pager: '#uploadedfilesgridpager',
        rowNum: 10,
        rowList: [10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'desc',
        multiselect: true,
        viewrecords: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: '<i class="ace-icon fa fa-upload bigger-110"></i> Uploaded Documents'
    });
    $(window).triggerHandler('resize.jqGrid');
    jQuery("#Uploadedfileslist").jqGrid('navGrid', '#uploadedfilesgridpager',
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
            {}, {}, {});
    jQuery("#Uploadedfileslist").jqGrid('navButtonAdd', '#uploadedfilesgridpager', {
        caption: "<i class='ace-icon fa fa-plus-circle purple fa-2x'></i>",
        onClickButton: function () {
            ModifiedLoadPopupDynamicaly("/Asset/AddInvoiceDetails?AssetDet_Id=" + $('#AssetDet_Id').val(), $('#newAssetInvoiceDetails'), function () {
                LoadSetGridParam($('#newAssetInvoiceDetails'))
            }, function () { }, 400, 300, "Add Invoice");            
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
    /*File Upload Related Start*/
    /* upload button code //Ajax File Uploading */
    function SpecialCharacters(strValidate) {
        if (strValidate.indexOf('&') != -1 || strValidate.indexOf("'") != -1 ||
            strValidate.indexOf(";") != -1 || strValidate.indexOf("#") != -1) {
            return true;
        } else {
            return false;
        }
    }

    function validateDocReq(msg, reqField, isValid) {
        var fieldVal = $('#' + reqField).val();
        if ((typeof isValid != 'undefined' && isValid) || fieldVal == null || fieldVal == "") {
            ErrMsg(msg, function () { $('#' + reqField).focus(); });
            return false;
        } else {
            return true;
        }
    }
    $('#Upload').click(function () {
        var splitstr = $('#file1').val().split('\\');
        var fileName = splitstr[splitstr.length - 1];
        var DocTypeText = $('#ddlDocumentType option:selected').text();
        if (!validateDocReq("No document found. Please select a file to upload!!!", 'file1')) { }
        else if (!validateDocReq("Special characters (&,#,;') are not supported in document file names. Please amend the file name before upload!!!", 'file1', SpecialCharacters(fileName))) { }
        else if (!validateDocReq("The attached file does not contain file extension.", 'file1', (fileName.lastIndexOf('.') == -1))) { }
        else {
            ajaxUploadDocs();
            return false;
        }
    });
    function ajaxUploadDocs() {
        debugger;
        var DocTypeText = $('#ddlDocumentType option:selected').text();
        $.ajaxFileUpload({
            url: '/Asset/ITAssetDocumentsUpload?AssetDet_Id=' + $('#AssetDet_Id').val() + '&DocType=' + DocTypeText,
            secureuri: false,
            fileElementId: 'file1',
            dataType: 'json',
            success: function (data, status) {
                debugger;
                $('#Uploadedfileslist').trigger("reloadGrid");
                $('#file1').val('');
                if (typeof data.result != 'undefined' && data.result != '') {
                    debugger;
                    if (typeof data.success != 'undefined' && data.success == true) {
                        $('#file1').val('');
                        SucessMsg(data.result);
                    } else {
                        ErrMsg(data.result);
                    }
                }
            },
            error: function (data, status, e) {
            }
        });
    }
    /*File Upload Related End*/
    $("#ddlToBlock").change(function () {
        debugger;
        FillToLocationByCampusWithBlock("");
        //if ($("#ddlTransactionType").val() == "IntraCampus" || $("#ddlTransactionType").val() == "Stock") {
        //    //FillLocationByCampusWithBlock();//Commented By Prabakaran
        //    FillToLocationByCampusWithBlock();
        //}
        //if ($("#ddlTransactionType").val() == "InterCampus") {
        //    FillToLocationByCampusWithBlock();
        //}
    });
    $("#ddlToCampus").change(function () {
        debugger;
        FillToBlockByCampus();
        FillToLocationByCampusWithBlock("");
        FillStaffGroupByCampus();
        FillGradeByCampus();
    });
    $("#ddlNewGrade").change(function () {
        FillStudNameByCampusWithGrade();
    });

    //Sub Asset
    var grid_selector1 = "#grid-table";
    var pager_selector1 = "#grid-pager";

    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        jQuery(grid_selector1).jqGrid('setGridWidth', $(".page-content").width());
    })
    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector1).closest('[class*="col-"]');
    jQuery(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                jQuery(grid_selector1).jqGrid('setGridWidth', parent_column.width());
            }, 0);
        }
    })
    jQuery(grid_selector1).jqGrid({
        url: '/Asset/ITSubAssetManagementjqGrid?AssetRefId=' + $('#AssetDet_Id').val(),
        datatype: 'json',
        height: 200,
        mtype: 'GET',
        colNames: ['AssetDet_Id', 'Asset Code', 'Asset Id', 'Asset Type', 'Brand', 'Model', 'Serial Number', 'Transaction Type', 'Campus', 'Block', 'Location', 'User Type', 'Name', 'Created By', 'Created Date', 'Modified By', 'Modified Date'],
        colModel: [
                      { name: 'AssetDet_Id', index: 'AssetDet_Id', hidden: true },
                      { name: 'AssetCode', index: 'AssetCode' },
                      { name: 'Asset_Id', index: 'Asset_Id', hidden: true, search: false },
                      { name: 'AssetType', index: 'AssetType' },
                      { name: 'Make', index: 'Make' },
                      { name: 'Model', index: 'Model' },
                      { name: 'SerialNo', index: 'SerialNo' },
                      { name: 'TransactionType', index: 'TransactionType' },
                      { name: 'CurrentCampus', index: 'CurrentCampus' },
                      { name: 'CurrentBlock', index: 'CurrentBlock' },
                      { name: 'CurrentLocation', index: 'CurrentLocation' },
                      { name: 'UserType', index: 'UserType' },
                      { name: 'IdNum', index: 'IdNum', sortable: true },
                      //{ name: 'FormId', index: 'FormId', search: false },
                      //{ name: 'Location', index: 'Location' },
                      { name: 'CreatedBy', index: 'CreatedBy', hidden: true },
                      { name: 'CreatedDate', index: 'CreatedDate', hidden: true },
                      { name: 'ModifiedBy', index: 'ModifiedDate', hidden: true },
                      { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector1,
        altRows: true,
        sortname: 'AssetDet_Id',
        sortorder: 'Asc',
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='ace-icon fa fa-list'></i>&nbsp;IT Sub Asset Management",
    })
    //$(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size  
    jQuery(grid_selector1).jqGrid('navGrid', pager_selector1,
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
            {}, {}, {});
    var cacheval = {};
    $("#txtITAssetCode").autocomplete({
        source: function (request, response) {
            debugger;
            var Campus = "";
            if ($("#ddlTransactionType").val() == "IntraCampus") {
                Campus = $("#txtFromCampus").val();
            }
            if ($("#ddlTransactionType").val() == "InterCampus") {
                Campus = $("#ddlToCampus").val();
            }
            var term = request.term;
            if (term in cacheval) {
                debugger;
                response($.map(cacheval[term], function (item) {
                    debugger;
                    return { label: item.Text, value: item.Value }
                }))
                return;
            }
            //else {
            $.getJSON("/Asset/ITAssetCodeAutoComplete?Campus=" + Campus + '&UserType=' + $("#ddlUserType").val(), request, function (data, status, xhr) {
                cacheval[term] = data;
                //response(data);
                response($.map(data, function (item) {
                    return { label: item.Text, value: item.Value }
                }))
            });
            //}
        },
        minLength: 1,
        delay: 100,
        select: function (event, ui) {
            debugger;
            event.preventDefault();
            $("#txtITAssetCode").text(ui.item.value);
            $("#txtITAssetCode").val(ui.item.label);
            $("#AssetRefId").val(ui.item.value);
            if ($("#viewbagAssetCode").val() == ui.item.label) {
                $("#txtITAssetCode").val('');
                $("#txtITAssetCode").text('');
                ErrMsg("You select Current Asset.Please Select Other Asset.");
                return false;
            }
            else {
                $("#ddlToBlock,#ddlToLocation,#ddlNewStaffGroup,#ddlNewDepartment,#ddlIdNum").attr('disabled', true);
                FillDetailsByAssetDet_Id(ui.item.value);
            }
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#txtITAssetCode").text(ui.item.label);
        },
        messages: {
            noResults: "", results: ""
        }
    });
    $("#txtITAssetCode").focus(function () {
        if ($("#ddlUserType").val() == "") {
            ErrMsg("Please Select User Type");
            return false;
        }
        else if ($("#ddlTransactionType").val() == "InterCampus" && $("#ddlToCampus").val() == "") {
            ErrMsg("Please Select To Campus");
            return false;
        }
        else {
            return true;
        }
    });
    //$("#btnSave").hover(function () {
    //    $("#ddlToBlock,#ddlToLocation,#ddlNewStaffGroup,#ddlNewDepartment,#ddlIdNum").attr('disabled', false);
    //});
    //$("#btnSave").mouseout(function () {
    //    if ($("#IsSubAsset").val() == "True") {
    //        $("#ddlToBlock,#ddlToLocation,#ddlNewStaffGroup,#ddlNewDepartment,#ddlIdNum").attr('disabled', true);
    //    }
    //});
});
function validateFormSubmit() {
    debugger;
    var FromCampus = $('#txtFromCampus').val();
    var ToCampus = $('#ddlToCampus').val();
    var StaffGroup = $("#ddlNewStaffGroup").val();
    var Department = $("#ddlNewDepartment").val();
    //var StaffName = $("#ddlNewStaffName").val();
    var IdNum = $("#ddlIdNum").val();
    var Grade = $("ddlNewGrade").val();
    if ($('#ddlTransactionType').val() == "") {
        ErrMsg("Select Transaction Type!!");
        return false;
    }
    if ($('#ddlTransactionType').val() == "IntraCampus") {
        if ($('#ddlToBlock').val() == "" || $('#ddlToLocation').val() == "" || $('#txtInstalledOn').val() == "" || $('#txtTransactionCommentArea').val() == "") {
            ErrMsg("All fields are required!!");
            return false;
        }
        if (FromCampus == "Service") {
            ErrMsg("Please Return the Asset From Service!!");
            return false;
        }
        if ($("#ddlUserType").val() == "Staff") {
            if (StaffGroup == "" || Department == "" || IdNum == "") {
                ErrMsg("All Fields are Required!!");
                return false;
            }
        }
        if ($("#ddlUserType").val() == "Student") {
            if (IdNum == "" || Grade == "") {
                ErrMsg("All Fields are Required!!");
                return false;
            }
        }
        if ($("#IsSubAsset").val() == "True" && $("#txtITAssetCode").val() == "") {
            $("#txtITAssetCode").text('');
            ErrMsg("Please Enter Asset Code");
            return false;
        }
        if ($("#IsSubAsset").val() == "True" && $("#txtITAssetCode").val() != "" && $("#txtITAssetCode").text() == "" || $("#txtITAssetCode").val() == 0) {
            ErrMsg("Please Enter Valid Asset Code");
            return false;
        }
    }
    else if ($('#ddlTransactionType').val() == "InterCampus") {
        if ($('#ddlToBlock').val() == "" || $('#ddlToCampus').val() == "" || $('#ddlToLocation').val() == "" || $('#txtInstalledOn').val() == "" || $('#txtTransactionCommentArea').val() == "") {
            ErrMsg("All fields are required!!");
            return false;
        }
        if (FromCampus == ToCampus) {
            ErrMsg("From campus and To campus should not be same!!");
            return false;
        }
        if (FromCampus == "Service") {
            ErrMsg("Please Return the Asset From Service!!");
            return false;
        }
        if (AssetUserType == "Student") {
            if (ToCampus != $("#UserCampus").val()) {
                ErrMsg("User Campus Mismatched!!");
                return false;
            }
        }
        if ($("#ddlUserType").val() == "Staff") {
            if (StaffGroup == "" || Department == "" || IdNum == "") {
                ErrMsg("All fields are required!!");
                return false;
            }
        }
        if ($("#ddlUserType").val() == "Student") {
            if (IdNum == "" || Grade == "") {
                ErrMsg("All fields are required!!");
                return false;
            }
        }
        if ($("#IsSubAsset").val() == "True" && $("#txtITAssetCode").val() == "") {
            $("#txtITAssetCode").text('');
            $("#txtITAssetCode").val('');
            ErrMsg("Please Enter Asset Code");
            return false;
        }
        if ($("#IsSubAsset").val() == "True" && $("#txtITAssetCode").val() != "" && $("#txtITAssetCode").text() == "" || $("#txtITAssetCode").text() == 0) {
            ErrMsg("Please Enter Valid Asset Code");
            return false;
        }
    }
    else if ($('#ddlTransactionType').val() == "Service") {
        CalculatePendingAge();
        if ($('#txtDCNo').val() == "" || $('#txtDCDate').val() == "" || $('#txtPhysicalCondition').val() == "" || $('#txtServiceProblem').val() == "" || $('#txtVendor').val() == "" || $('#txtExpectedDate').val() == "") {
            ErrMsg("All fields are required!!");
            return false;
        }
        if (FromCampus == $('#ddlTransactionType').val() && $("#IsSubAsset").val() == "False") {
            ErrMsg("Already in Service!!");
            return false;
        }
    }
    else if ($('#ddlTransactionType').val() == "Scrap") {
        if (FromCampus == "Service") {
            ErrMsg("Please Return the Asset From Service!!");
            return false;
        }
        if ($('#txtInwardDate').val() == "" || $('#txtScrapPhysicalCondition').val() == "" || $('#txtScrapProblem').val() == "") {
            ErrMsg("All fields are required!!");
            return false;
        }
    }
    else if ($('#ddlTransactionType').val() == "Stock") {
        if (FromCampus == "Service") {
            ErrMsg("Please Return the Asset From Service!!");
            return false;
        }
        if ($('#txtTransactionCommentArea').val() == "" || $('#txtInstalledOn').val() == "") {
            ErrMsg("All fields are required!!");
            return false;
        }
    }    
}
function uploadvalidate() {
    if ($('#ddlDocumentType').val() == " " || $('#ddlDocumentType').val() == "") {
        ErrMsg("Please Select Document Type!!");
        return false;
    }
    else if ($('#file1').val() == " " || $('#file1').val() == "") {
        ErrMsg("Please Upload a Document!!");
        return false;
    }
    else {
        return true;
    }
}
function uploaddat(id1) {
    //window.location.href = "/Asset/uploaddisplay?Id=" + id1 + '&DocumentFor=ITAsset';
    window.location.href = "/Asset/uploaddisplay?Id=" + id1;
    //processBusy.dialog('close');
}
function CalculatePendingAge() {
    var DCDate = $("#viewbagDCDate").val();
    if (DCDate == "") {
        $("#txtPendingAge").val('');
    }
    else {
        var Today = new Date();
        var oneDay = 24 * 60 * 60 * 1000;
        DCDate = DCDate.split('-');
        var firstDate = new Date(DCDate[2], DCDate[1], DCDate[0]);
        var secondDate = new Date(Today.getFullYear(), (Today.getMonth() + 1), Today.getDate());
        var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
        $("#txtPendingAge").val(diffDays);
    }
}
function FillDetailsByAssetDet_Id(AssetDet_Id) {
    $.ajax({
        type: 'POST',
        url: "/Asset/GetAssetDetails",
        data: { AssetDet_Id: AssetDet_Id },
        success: function (data) {
            $("#ddlToBlock").val(data.Block);
            FillToLocationByCampusWithBlock(data.Block);
            $("#AssetDetailsTransaction_ToBlock").val(data.Block);
            $("#AssetDetailsTransaction_ToLocation").val(data.Location);
            $("#hiddenToLocation").val(data.Location);
            $("#ddlNewStaffGroup").val(data.StaffGroup);
            $("#hiddenNewDepartment").val(data.Department);            
            FillDepatmentByCampusandGroup();
            FillStaffNameByCampusWithGroupandDepartment();
            $("#hiddenIdNum").val(data.IdNum);
        }
    });
}