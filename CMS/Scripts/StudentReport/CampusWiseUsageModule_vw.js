$(function () {
    jQuery("#CampusWiseUsageModule_vwGrid").jqGrid({
        autowidth: true,
        //  shrinkToFit: true,
        url: '/StudentsReport/GetCampusWiseUsageModule_vwListJqGrid',
        datatype: "json",
        colNames: ['Id', 'Module', 'IB Main', 'IB KG', 'Chennai Main', 'Chennai City', 'Ernakulam', 'ErnakulamKG', 'Karur', 'KarurKG', 'Tirupur', 'TirupurKG', 'TipsSaran', 'CreatedBy', 'CreatedDate', 'ModifiedBy', 'ModifiedDate', 'Delete'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true, editable: true, key: true },
            { name: 'Module', index: 'Module',editable:true,width: '300'},
            { name: 'IBMain', index: 'IBMain', stype: "select", editable: true,searchoptions: { sopt: ['eq'], value: { true: 'Yes', false: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'IBKG', index: 'IBKG', stype: 'select', editable: true, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'ChennaiMain', index: 'ChennaiMain', stype: 'select', editable: true, searchptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'ChennaiCity', index: 'ChennaiCity', stype: 'select', editable: true, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'Ernakulam', index: 'Ernakulam', stype: 'select', editable: true, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'ErnakulamKG', index: 'ErnakulamKG',  stype: 'select', editable: true, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'Karur', index: 'Karur',  stype: 'select', editable: true, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'KarurKG', index: 'KarurKG', stype: 'select', editable: true, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'Tirupur', index: 'Tirupur', stype: 'select', editable: true, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'TirupurKG', index: 'TirupurKG', stype: 'select', editable: true, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'TipsSaran', index: 'TipsSaran',  stype: 'select', editable: true, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'CreatedBy', index: 'CreatedBy', hidden: true, editable: true },
            { name: 'CreatedDate', index: 'CreatedDate', hidden: true, editable: true },
            { name: 'ModifiedBy', index: 'ModifiedBy', hidden: true, editable: true },
            { name: 'ModifiedDate', index: 'ModifiedDate', hidden: true, editable: true },
            { name: 'Delete', index: 'Delete', hidden: true, sorttype: "int", formatter: 'actions', formatoptions: { keys: true, editformbutton: true } }
        ],
        rowNum: 10,
        rowList: [10, 20, 30, 40,120],
        pager: '#CampusWiseUsageModule_vwPager',
        sortname: 'Id',
        viewrecords: true,
        onSelectRow:function(id){
            jQuery('#CampusWiseUsageModule_vwGrid').editRow(id, true);
        },
        height: 300,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        sortorder: "Asc",
        caption: "Campus Wise Usage Module",
        editurl: '/StudentsReport/AddCampusWiseModuleRecords',
        multiselect: true,
        
        onPaging : function(but) {
            alert("Button: "+but + " is clicked");
        }
       
    });
    jQuery("#CampusWiseUsageModule_vwGrid").jqGrid('navGrid', '#CampusWiseUsageModule_vwPager',
        {
            edit: false,
            editicon: 'ace-icon fa fa-pencil blue',
            add: false,
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
        {
            //Edit the records
            url: '/StudentsReport/EditCampusWiseModuleRecords/',
        },
        {
            //Add the records
            url: '/StudentsReport/AddCampusWiseModuleRecords/'
        },
        {
            //Delete the records
            url: '/StudentsReport/DeleteCampusWiseUsageModule_vw/', closeOnEscape: true, beforeShowForm: function (frm)
            { selectedrows = $("#CampusWiseUsageModule_vwGrid").jqGrid("getGridParam", "selarrrow"); return { Id: selectedrows } }
        },
        {})

    $("#CampusWiseUsageModule_vwGrid").jqGrid('filterToolbar', { searchOnEnter: false, enableClear: false });
    jQuery("#CampusWiseUsageModule_vwGrid").jqGrid('inlineNav', "#CampusWiseUsageModule_vwPager",
         {
             edit: false,
             editicon: 'ace-icon fa fa-pencil blue',
             add: true,
             addicon: 'ace-icon fa fa-plus-circle purple',
             del: false,
             delicon: 'ace-icon fa fa-trash-o red',
             search: false,
             searchicon: 'ace-icon fa fa-search orange',
             refresh: false,
             refreshicon: 'ace-icon fa fa-refresh green',
             view: false,
             viewicon: 'ace-icon fa fa-search-plus grey'
         }, {}, {}, {}, {});
    function styleCheckbox(table) { }

    function updateActionIcons(table) {}

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

    
});












