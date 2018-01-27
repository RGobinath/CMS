$(function () {

    // unit of measurement
    var StrUnitgrid_selector = "#StoreUnitsList";
    var StrUnitpager_selector = "#StoreUnitsListPager";
    // Material group master
    var MatGrpgrid_selector = "#StoreMaterialGroupList";
    var MatGrppager_selector = "#StoreMaterialGroupListPager";
    // Material Sub Group master
    var MatSubGrpgrid_selector = "#StoreMaterialSubGroupList";
    var MatSubGrppager_selector = "#StoreMaterialSubGroupListPager";
    // Store SKU Master
    var StrSKUgrid_selector = "#StoreMaterialsList";
    var StrSKUpager_selector = "#StoreMaterialsListPager";
    // Store functionaries
    var StrSupgrid_selector = "#StoreSupplierList";
    var StrSuppager_selector = "#DivSupplierPager";

    $(window).on('resize.jqGrid', function () {
        $(StrUnitgrid_selector).jqGrid('setGridWidth', $(".tab-content").width());
        $(MatGrpgrid_selector).jqGrid('setGridWidth', $(".tab-content").width());
        $(MatSubGrpgrid_selector).jqGrid('setGridWidth', $(".tab-content").width());
        $(StrSKUgrid_selector).jqGrid('setGridWidth', $(".tab-content").width());
        $(StrSupgrid_selector).jqGrid('setGridWidth', $(".tab-content").width());
    })
    //resize on sidebar collapse/expand
    var StrUnit_column = $(StrUnitgrid_selector).closest('[class*="col-"]');
    var MatGrp_column = $(MatGrpgrid_selector).closest('[class*="col-"]');
    var MatSubGrp_column = $(MatSubGrpgrid_selector).closest('[class*="col-"]');
    var StrSKU_column = $(StrSKUgrid_selector).closest('[class*="col-"]');
    var StrSup_column = $(StrSupgrid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                $(StrUnitgrid_selector).jqGrid('setGridWidth', StrUnit_column.width());
                $(MatGrpgrid_selector).jqGrid('setGridWidth', MatGrp_column.width());
                $(MatGrpgrid_selector).jqGrid('setGridWidth', MatSubGrp_column.width());
                $(StrSKUgrid_selector).jqGrid('setGridWidth', StrSKU_column.width());
                $(StrSupgrid_selector).jqGrid('setGridWidth', StrSup_column.width());
            }, 0);
        }
    })
    jQuery(StrUnitgrid_selector).jqGrid({
        url: '/Store/StoreUnitsListJqGrid',
        datatype: 'json',
        mtype: 'GET',
        height: '265',
        colNames: ['Id', 'Unit Code', 'Unit Description'],
        colModel: [
                { name: 'Id', index: 'Id', align: 'left', editable: true, hidden: true, edittype: 'text', key: true, sortable: false },
                { name: 'UnitCode', index: 'UnitCode', width: 350, align: 'left', edittype: 'text', editable: true, hidden: false, sortable: true, editrules: { required: true }, editoptions: { size: 5, maxlength: 5} },
                { name: 'Units', index: 'Units', width: 350, align: 'left', edittype: 'text', editable: true, hidden: false, sortable: true, editrules: { required: true }, editoptions: { maxlength: 25} },
                ],
        pager: StrUnitpager_selector,
        rowNum: '8',
        rowList: [5, 10, 20, 50],
        sortname: 'UnitCode',
        sortorder: "",
        viewrecords: true,
        multiselect: true,
        closeAfterEdit: true,
        closeAfterAdd: true,
        autowidth: true,
        // loadonce: true, 
        reloadAfterSubmit: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: 'Store Units List'
    });
    jQuery(StrUnitgrid_selector).jqGrid('navGrid', StrUnitpager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, { width: 400, url: '/Store/AddStoreUnits?test=edit' }, { width: 400, url: '/Store/AddStoreUnits' }, {}, {}, {})

    $(StrUnitgrid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () {
        $(StrUnitgrid_selector).clearGridData();
        return false;
    }
    });

    jQuery(MatGrpgrid_selector).jqGrid({
        url: '/Store/StoreMaterialGroupMasterListJqGrid',
        datatype: 'json',
        mtype: 'GET',
        height: '265',
        colNames: ['Id', 'Material Group', 'Material Group Code'],
        colModel: [{ name: 'Id', index: 'Id', align: 'left', editable: true, hidden: true, edittype: 'text', key: true, sortable: false },
                   { name: 'MaterialGroup', index: 'MaterialGroup', width: 350, align: 'left', edittype: 'text', editable: true, hidden: false, sortable: true, editrules: { required: true }, editoptions: { maxlength: 50} },
                   { name: 'MatGrpCode', index: 'MatGrpCode', width: 300, align: 'left', edittype: 'text', editable: true, hidden: false, sortable: true, editrules: { required: true, custom: true, custom_func: checkvalid }, editoptions: { maxlength: 3} }, ],
        pager: MatGrppager_selector,
        rowNum: '10',
        rowList: [5, 10, 20, 50],
        sortname: 'MaterialGroup',
        sortorder: "",
        //labelswidth: 60,
        viewrecords: true,
        multiselect: true,
        closeAfterEdit: true,
        closeAfterAdd: true,
        autowidth: true,
        reloadAfterSubmit: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: 'Storer Material Group List'
    });

    jQuery(MatGrpgrid_selector).jqGrid('navGrid', MatGrppager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, { width: 400, url: '/Store/AddMaterialGroupMaster?test=edit' }, { width: 400, url: '/Store/AddMaterialGroupMaster' }, {}, {}, {})

    $(MatGrpgrid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () {
        $(MatGrpgrid_selector).clearGridData();
        return false;
    }
    });
    jQuery(MatSubGrpgrid_selector).jqGrid({
        url: '/Store/StoreMaterialSubGroupMasterListJqGrid',
        datatype: 'json',
        mtype: 'GET',
        height: '265',
        colNames: ['Id', 'Material Group', 'Material Group', 'Material Sub Group', 'Material Sub Group Code'],
        colModel: [{ name: 'Id', index: 'Id', align: 'left', editable: true, hidden: true, edittype: 'text', key: true, sortable: false },
                { name: "MaterialGroupId", index: "MaterialGroupId", align: "left", hidden: true, editable: true, editrules: { required: true, edithidden: true }, edittype: 'select',
                    editoptions: {
                        dataUrl: '/Store/FillMaterialGroup',
                        buildSelect: function (data) {
                        debugger;
                            jqGridMaterialGroupId = jQuery.parseJSON(data);
                            var s = '<select>';
                            s += '<option value=" ">------Select------</option>';
                            if (jqGridMaterialGroupId && jqGridMaterialGroupId.length) {
                                for (var i = 0, l = jqGridMaterialGroupId.length; i < l; i++) {
                                    var mg = jqGridMaterialGroupId[i];
                                    s += '<option value="' + jqGridMaterialGroupId[i].Value + '">' + jqGridMaterialGroupId[i].Text + '</option>';
                                }
                            }
                            return s + "</select>";
                        },
                    }
                },
                { name: 'MaterialGroup', index: 'MaterialGroup', width: 250, sortable: true, editrules: { required: true} },
                { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 250, align: 'left', edittype: 'text', editable: true, hidden: false, sortable: true, editrules: { required: true }, editoptions: { maxlength: 50} },
                { name: 'MatSubGrpCode', index: 'MatSubGrpCode', width: 200, align: 'left', edittype: 'text', editable: true, hidden: false, sortable: true, editrules: { required: true, custom: true, custom_func: checkvalid }, editoptions: { maxlength: 3} },
                ],
        pager: MatSubGrppager_selector,
        rowNum: '8',
        rowList: [5, 10, 20, 50],
        sortname: 'MaterialSubGroup',
        sortorder: 'asc',
        //labelswidth: 60,
        viewrecords: true,
        multiselect: true,
        closeAfterEdit: true,
        closeAfterAdd: true,
        autowidth: true,
        reloadAfterSubmit: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
            $(window).triggerHandler('resize.jqGrid');
        },
        caption: 'Storer Material Sub Group List'
    });
    jQuery(MatSubGrpgrid_selector).jqGrid('navGrid', MatSubGrppager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, { width: 400, url: '/Store/AddMaterialSubGroupMaster?test=edit' }, { width: 400, url: '/Store/AddMaterialSubGroupMaster' }, {}, {})

    $(MatSubGrpgrid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () {
        $(MatSubGrpgrid_selector).clearGridData();
        return false;
    }
    });
    jQuery(StrSKUgrid_selector).jqGrid({
        url: '/Store/StoreSKUMasterListJqGrid',
        datatype: 'json',
        mtype: 'GET',
        height: '265',
        //width: '970',
        // shrinkToFit: true,
        colNames: ['Id', 'Material Group', 'Material Sub Group', 'Material Group', 'Material Sub Group', 'Material', 'Units', 'Item Code', 'Item Location', 'Notes', 'Is Active'],
        colModel: [
                { name: 'Id', index: 'Id', width: 10, align: 'left', editable: true, hidden: true, edittype: 'text', key: true, sortable: false },
                { name: "MaterialGroupId", index: "MaterialGroupId", width: 20, align: "left", editable: true, hidden: true, formoptions: { elmsuffix: ' *' }, editrules: { edithidden: true }, edittype: 'select',
                    editoptions: {dataUrl: '/Store/FillMaterialGroup',
                        buildSelect: function (data) {
                            jqGridMaterialGroup = jQuery.parseJSON(data);
                            var s = '<select>';
                            s += '<option value=" ">------Select------</option>';
                            if (jqGridMaterialGroup && jqGridMaterialGroup.length) {
                                for (var i = 0, l = jqGridMaterialGroup.length; i < l; i++) {
                                    var mg = jqGridMaterialGroup[i];
                                    s += '<option value="' + jqGridMaterialGroup[i].Value + '">' + jqGridMaterialGroup[i].Text + '</option>';
                                }
                            }
                            return s + "</select>";
                        },
                        dataEvents: [{ type: 'change',
                            fn: function (e) {
                                var selectedCategory = $(e.target).val();
                                if (selectedCategory != '') {
                                    $.getJSON('/Store/MaterialSubGroupddl',
                                                    { MaterialGroupId: selectedCategory },
                                                    function (recipes) {
                                                        var selectHtml = "";
                                                        selectHtml += '<option value=" ">------Select------</option>';
                                                        $.each(recipes, function (jdIndex, jdData) {
                                                            selectHtml = selectHtml + "<option value='" + jdData.Value + "'>" + jdData.Text + "</option>";
                                                        });
                                                        if ($(e.target).is('.FormElement')) {
                                                            var form = $(e.target).closest('form.FormGrid');
                                                            $("select#MaterialSubGroupId.FormElement", form[0]).html(selectHtml);
                                                        }
                                                    });
                                }
                            }
                        }
                                       ]
                    }
                },
                { name: 'MaterialSubGroupId', index: 'MaterialSubGroupId', width: 20, align: 'left', sortable: false, editable: true, hidden: true, edittype: 'select',
                    editrules: { edithidden: true }, formoptions: { elmsuffix: ' *' }, searchtype: "text",
                    editoptions: {
                        dataUrl: '/Store/FillMaterialSubGroupWithoutMaterialGroup',
                        buildSelect: function (data) {
                            var response = jQuery.parseJSON(data);
                            var s = '<select>';
                            s += '<option value=" ">------Select------</option>';
                            if (response && response.length) {
                                for (var i = 0, l = response.length; i < l; i++) {
                                    var ri = response[i];
                                    s += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                                }
                            }
                            return s + "</select>";
                        }
                    }
                },
        //  { name: 'MaterialSubGroupId', index: 'MaterialSubGroupId', width: 20, align: 'left', sortable: false, editable: true, hidden: true, edittype: 'select', editrules: { edithidden: true }, formoptions: { elmsuffix: ' *' }, formatter: 'select' },
                {name: 'MaterialGroup', index: 'MaterialGroup', width: 250, align: 'left', sortable: true, editrules: { required: true} },
                { name: 'MaterialSubGroup', index: 'MaterialSubGroup', width: 250, align: 'left', sortable: true, editrules: { required: true} },
                { name: 'Material', index: 'Material', width: 350, align: 'left', sortable: true, editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *' }, editoptions: { maxlength: 50} },
                { name: 'UnitCode', index: 'UnitCode', width: 150, align: 'left', sortable: true, editable: true, search: true, stype: 'text',
                    //sorttype: function (cell) { return buildDataforDrpDwn(@Html.Raw(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ViewBag.StoreUnitsList)))[cell]; },
                    edittype: 'select', editrules: { required: true }, formoptions: { elmsuffix: ' *' },
                    // editoptions: { value: buildDataforDrpDwn(@Html.Raw(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ViewBag.StoreUnitsList))) },
                    editoptions: {
                        dataUrl: '/Store/Unitsddl',
                        buildSelect: function (data) {
                            jqGridMaterialGroup = jQuery.parseJSON(data);
                            var s = '<select>';
                            s += '<option value=" ">------Select------</option>';
                            if (jqGridMaterialGroup && jqGridMaterialGroup.length) {
                                for (var i = 0, l = jqGridMaterialGroup.length; i < l; i++) {
                                    var mg = jqGridMaterialGroup[i];
                                    s += '<option value="' + mg + '">' + mg + '</option>';
                                }
                            }
                            return s + "</select>";
                        }
                    }
                },
                { name: 'ItemCode', index: 'ItemCode', editable: true, editoptions: { readonly: true }, sortable: true },
                { name: 'ItemLocation', index: 'ItemLocation', editable: true, sortable: true, editoptions: { maxlength: 50} },
                { name: 'Notes', index: 'Notes', sortable: true, editable: true, edittype: 'textarea', editoptions: { rows: "4", cols: "18" }, editoptions: { maxlength: 4000} },
                { name: 'IsActive', index: 'IsActive', width: 200, align: 'left', stype: 'select', formatter: showYesOrNo, sortable: true, editable: true, edittype: 'select', editrules: { required: true }, formoptions: { elmsuffix: ' *' }, editoptions: { value: "True:Yes;False:No"} },
                ],
        pager: StrSKUpager_selector,
        rowNum: '8',
        rowList: [5, 10, 20, 50],
        sortname: 'Material',
        sortorder: "Desc",
        viewrecords: true,
        multiselect: true,
        closeAfterEdit: true,
        closeAfterAdd: true,
        // autowidth: true,
        reloadAfterSubmit: true,
        caption: 'Storer Materials List'
    });

    jQuery(StrSKUgrid_selector).jqGrid('navGrid', StrSKUpager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, { beforeShowForm: function (form) { $('#tr_ItemCode', form).hide(); },
                height: 380, width: 480,
                url: '/Store/AddMaterial?test=edit'
            }, {
                beforeShowForm: function (form) { $('#tr_ItemCode', form).hide(); },
               height: 380, width: 480, 
                url: '/Store/AddMaterial'
            }, {}, {})

    //    jQuery(StrSKUgrid_selector).navGrid(StrSKUpager_selector, { add: true, edit: true, del: false, search: false, refresh: false },
    //        {
    //            beforeShowForm: function (form) { $('#tr_ItemCode', form).hide(); },
    //            // height: 350, width: 480,
    //            url: '/Store/AddMaterial?test=edit'
    //        },
    //        {
    //            beforeShowForm: function (form) { $('#tr_ItemCode', form).hide(); },
    //            //height: 350, width: 480, 
    //            url: '/Store/AddMaterial'
    //        });

    jQuery(StrSKUgrid_selector).jqGrid('navButtonAdd', StrSKUpager_selector, {
        caption: "Export To Excel",
        onClickButton: function () {
            var ExptType = 'Excel';
            window.open("/Store/StoreSKUMasterListJqGrid" + '?rows=9999' + '&ExptType=' + ExptType);
        }
    });

    $(StrSKUgrid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () {
        $('#StoreMaterialsList').clearGridData();
        return false;
    }
    });

    jQuery(StrSupgrid_selector).jqGrid({
        url: '/Store/StoreSupplierListJqGrid1',
        datatype: 'json',
        mtype: 'GET',
        height: '265',
        //width: '970',
        // shrinkToFit: true,
        colNames: ['Name', 'Contact Name', 'PAN Number', 'Mobile Number', 'Phone Number', 'Email', 'City', 'State', 'PIN Code', 'Country', 'Type', 'TIN Number', 'Credit Terms', 'Is Preferred Supplier', 'Is Active', 'Address', 'Notes', 'Id', 'Form Code', 'Form Code'],
        colModel: [
        { name: 'SupplierName', index: 'SupplierName', width: 50, align: 'left', sortable: true, editable: true, edittype: 'text', formoptions: { elmsuffix: ' *', rowpos: 1, colpos: 1 }, editrules: { required: true }, editoptions: { maxlength: 50} },
        { name: 'CompanyName', index: 'CompanyName', width: 120, align: 'left', sortable: true, editable: true, edittype: 'text', formoptions: { elmsuffix: ' *', rowpos: 1, colpos: 2 }, editrules: { required: true }, editoptions: { maxlength: 50} },
        { name: 'PANNumber', index: 'PANNumber', width: 50, align: 'left', sortable: true, editable: true, edittype: 'text', formoptions: { elmsuffix: ' *', rowpos: 1, colpos: 3 }, editrules: { required: true }, editoptions: { maxlength: 50} },
        { name: 'MobileNumber', index: 'MobileNumber', width: 50, align: 'left', sortable: true, editable: true, edittype: 'text', formoptions: { elmsuffix: ' *', rowpos: 2, colpos: 1 }, editrules: { required: true} },
        { name: 'PhoneNumber', index: 'PhoneNumber', width: 50, align: 'left', sortable: true, editable: true, edittype: 'text', formoptions: { elmsuffix: ' *', rowpos: 2, colpos: 2 }, editrules: { required: true} },
        { name: 'Email', index: 'Email', width: 50, align: 'left', edittype: 'text', editable: true, editrules: { required: true, edithidden: true, email: true }, sortable: true, formoptions: { elmsuffix: ' *', rowpos: 2, colpos: 3 }, editoptions: { maxlength: 50} },
        { name: 'City', index: 'City', width: 50, align: 'left', sortable: true, hidden: true, editable: true, editrules: { required: true, edithidden: true }, formoptions: { elmsuffix: ' *', rowpos: 3, colpos: 1 }, editoptions: { maxlength: 50} },
        { name: 'State', index: 'State', width: 50, align: 'left', sortable: true, editable: true, edittype: 'text', hidden: true, editrules: { required: true, edithidden: true }, formoptions: { elmsuffix: ' *', rowpos: 3, colpos: 2 }, editoptions: { maxlength: 50} },
        { name: 'ZipCode', index: 'ZipCode', width: 50, align: 'left', sortable: true, editable: true, edittype: 'text', hidden: true, editrules: { required: true, integer: true, edithidden: true }, formoptions: { elmsuffix: ' *', rowpos: 3, colpos: 3 }, editoptions: { maxlength: 10} },
        { name: 'Country', index: 'Country', width: 50, align: 'left', sortable: true, editable: true, edittype: 'text', hidden: true, editrules: { required: true, edithidden: true }, formoptions: { elmsuffix: ' *', rowpos: 4, colpos: 1 }, editoptions: { maxlength: 50} },
        { name: 'Type', index: 'Type', width: 50, align: 'left', sortable: true, editable: true, edittype: 'select', hidden: true, editrules: { required: true, edithidden: true }, editoptions: { value: ":----Select----;Supplier:Supplier;Transporter:Transporter;Courier:Courier;Agent:Agent" }, formoptions: { elmsuffix: ' *', rowpos: 4, colpos: 2} },
        { name: 'TINNumber', index: 'TINNumber', width: 50, align: 'left', sortable: true, editable: true, edittype: 'text', formoptions: { elmsuffix: ' *', rowpos: 4, colpos: 3 }, editrules: { required: true }, editoptions: { maxlength: 50} },
        { name: 'CreditTerms', index: 'CreditTerms', width: 50, align: 'left', sortable: true, editable: true, edittype: 'select', hidden: true, editrules: { required: true, edithidden: true }, editoptions: { value: ":----Select----;Immediate:Immediate;30 Days:30 Days;60 Days:60 Days;90 Days:90 Days" }, formoptions: { elmsuffix: ' *', rowpos: 5, colpos: 1} },
        { name: 'IsPreferredSupplier', index: 'IsPreferredSupplier', formatter: showYesOrNo, stype: 'select', width: 50, align: 'left', sortable: true, editable: true, edittype: 'select', editoptions: { value: "True:Yes;False:No" }, formoptions: { elmsuffix: ' *', rowpos: 5, colpos: 2 }, editrules: { required: true} },
        { name: 'IsActive', index: 'IsActive', width: 50, align: 'left', formatter: showYesOrNo, stype: 'select', sortable: true, editable: true, edittype: 'select', editoptions: { value: "True:Yes;False:No" }, formoptions: { elmsuffix: ' *', rowpos: 5, colpos: 3 }, editrules: { required: true} },
        { name: 'Address', index: 'Address', width: 50, align: 'left', edittype: 'textarea', editoptions: { rows: "4", cols: "20" }, editable: true, hidden: true, editrules: { required: true, edithidden: true }, sortable: true, formoptions: { elmsuffix: ' *', rowpos: 6, colpos: 1 }, editoptions: { maxlength: 4000} },
        { name: 'Notes', index: 'Notes', width: 50, align: 'left', sortable: true, editable: true, edittype: 'textarea', editoptions: { rows: "4", cols: "20" }, formoptions: { rowpos: 6, colpos: 2 }, editoptions: { maxlength: 4000} },
        { name: 'Id', index: 'Id', width: 50, align: 'left', editable: true, hidden: true, edittype: 'text', key: true, sortable: true },
        { name: 'FormCode', index: 'FormCode', width: 50, align: 'left', edittype: 'text', editable: true, hidden: true, sortable: true },
        { name: 'FrmCode', index: 'FrmCode', width: 50, align: 'left', sortable: true, hidden: true },
        ],
        pager: '#DivSupplierPager',
        rowNum: '8',
        rowList: [5, 10, 20, 50],
        sortname: 'SupplierName',
        sortorder: "Desc",
        // labelswidth: 60,
        viewrecords: true,
        multiselect: true,
        closeAfterEdit: true,
        closeAfterAdd: true,
        // autowidth: true,
        // loadonce: true, 
        reloadAfterSubmit: true,
        caption: 'Storer Functionaries List',
        beforeShowForm: function () {
            var $tr = $("#tr_SupplierName"), // name' is the column name
                $label = $tr.children("td.CaptionTD"),
                $data = $tr.children("td.DataTD");
            $data.attr("colspan", "5");
            $data.children("input").css("width", "95%");
            $label.css("width", "20%");
        }
    });
    jQuery(StrSupgrid_selector).jqGrid('navGrid', StrSuppager_selector,
            { 	//navbar options
                edit: true,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
                addicon: 'ace-icon fa fa-plus-circle purple',
                del: false,
                delicon: 'ace-icon fa fa-trash-o red',
                search: false,
                searchicon: 'ace-icon fa fa-search orange',
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            }, { height: 350, width: 970, url: '/Store/AddStoreSupplier?test=edit' }, { height: 350, width: 970, url: '/Store/AddStoreSupplier' }, {}, {})

    //    jQuery(StrSupgrid_selector).navGrid(StrSuppager_selector, { add: true, edit: true, del: false, search: false, refresh: false },
    //        { height: 350, width: 970, url: '/Store/AddStoreSupplier?test=edit' },
    //        { height: 350, width: 970, url: '/Store/AddStoreSupplier' },
    //        { closeOnEscape: true, recreateForm: true, height: 380, width: 900 },   // Edit options
    //        {closeOnEscape: true, recreateForm: true, height: 380, width: 900 });

    jQuery(StrSupgrid_selector).jqGrid('navButtonAdd', StrSuppager_selector, {
        caption: "Export To Excel",
        onClickButton: function () {

            var ExptType = 'Excel';
            window.open("/Store/StoreSupplierListJqGrid1" + '?rows=9999' + '&ExptType=' + ExptType);
        }
    });
    $(StrSupgrid_selector).jqGrid('filterToolbar', { stringResult: false, searchOnEnter: true, beforeSearch: function () {
        $(StrSupgrid_selector).clearGridData();
        return false;
    }
    });

});
function checkvalid(value, column) {
    if (value.length != 3) {
        return [false, column + ' Should be 3 characters in length'];
    }
    else {
        return [true];
    }
}
function showYesOrNo(cellvalue, options, rowObject) {
    if (cellvalue == 'True') {
        return 'Yes';
    }
    else {
        return 'No';
    }
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