$(function () {
//    FillCampusDll();
    var lastSel;
    jQuery("#CampusWiseUsageModule_vwGrid").jqGrid({
        autowidth: true,
        shrinkToFit: false,
        url: '/StudentsReport/GetCampusWiseUsageModule_vwListJqGrid',
        datatype: "json",
        //colNames: ['Id', 'CampusWiseUsageModule_Id', 'Module', 'Hit Count', 'Is Used?', 'Hit Count', 'Is Used?', 'Hit Count', 'Is Used?', 'Hit Count', 'Is Used?', 'Hit Count', 'Is Used?', 'Hit Count', 'Is Used?'],
        colNames: ['Id', 'CampusWiseUsageModule_Id', 'Module', 'Hit Count', 'Is Used?', 'Hit Count', 'Is Used?', 'Hit Count', 'Is Used?', 'Hit Count', 'Is Used?', 'Hit Count', 'Is Used?', 'Hit Count', 'Is Used?','Hit Count', 'Is Used?','Hit Count', 'Is Used?','Hit Count', 'Is Used?','Hit Count', 'Is Used?','Hit Count', 'Is Used?'],
        colModel: [
            { name: 'Id', index: 'Id', hidden: true, editable: true },
            { name: 'CampusWiseUsageModule_Id', index: 'CampusWiseUsageModule_Id', hidden: true, editable: true, key: true },
            {name: 'Module', index: 'Module', editable: true, width: '250', editoptions: {
                    size: 25, maxlengh: 30,
                    dataInit: function (element) {
                                $(element).autocomplete({
                                source: function (request, response) {
                                  $.ajax({
                                      url: "/StudentsReport/ModuleAutoComplete",
                                        type: "POST",   
                                        dataType: "json",
                                        data: { term: request.term },
                                        success: function (data) {
                                       
                                            response($.map(data, function (item) {
                                                
                                                return { label: item.MenuName, value: item.MenuName };
                                            }))

                                        }
                                    })
                                },
                                messages: {
                                    noResults: "", results: ""
                                }
                            });
                        
                    }
                }
            },
            { name: 'IBMain_Count', index: 'IBMain_Count', align: 'center' },
            { name: 'IBMain', index: 'IBMain', stype: "select", editable: true, searchoptions: { sopt: ['eq'], value: { true: 'Yes', false: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'IBKG_Count', index: 'IBKG_Count', align: 'center' },
            { name: 'IBKG', index: 'IBKG', stype: 'select', editable: true, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'ChennaiMain_Count', index: 'ChennaiMain_Count', align: 'center' },
            { name: 'ChennaiMain', index: 'ChennaiMain', stype: 'select', editable: true,  searchptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'ChennaiCity_Count', index: 'ChennaiCity_Count', align: 'center' },
            { name: 'ChennaiCity', index: 'ChennaiCity', stype: 'select', editable: true,searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'Ernakulam_Count', index: 'Ernakulam_Count', align: 'center' },
            { name: 'Ernakulam', index: 'Ernakulam', stype: 'select', editable: true, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'ErnakulamKG_Count', index: 'ErnakulamKG_Count', align: 'center' },
            { name: 'ErnakulamKG', index: 'ErnakulamKG', stype: 'select', editable: true,  searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'Karur_Count', index: 'Karur_Count', align: 'center' },
            { name: 'Karur', index: 'Karur', stype: 'select', editable: true,  searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'KarurKG_Count', index: 'KarurKG_Count', align: 'center' },
            { name: 'KarurKG', index: 'KarurKG', stype: 'select', editable: true,  searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'Tirupur_Count', index: 'Tirupur_Count', align: 'center' },
            { name: 'Tirupur', index: 'Tirupur', stype: 'select', editable: true,  searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'TirupurKG_Count', index: 'TirupurKG_Count', align: 'center' },
            { name: 'TirupurKG', index: 'TirupurKG', stype: 'select', editable: true,  searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            { name: 'TipsSaran_Count', index: 'TipsSaran_Count', align: 'center' },
            { name: 'TipsSaran', index: 'TipsSaran', stype: 'select', editable: true, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            //{ name: 'MCS_ANTHIYUR_Count', index: 'MCS_ANTHIYUR_Count', align: 'center',width:'75'},
            //{ name: 'MCS_ANTHIYUR', index: 'MCS_ANTHIYUR', stype: 'select', align: 'center',width:'75', editable: true, hidden: false, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            //{ name: 'MHSS_ANTHIYUR_Count', index: 'MHSS_ANTHIYUR_Count', align: 'center', width: '75' },
            //{ name: 'MHSS_ANTHIYUR', index: 'MHSS_ANTHIYUR', stype: 'select', align: 'center', width:'75',editable: true, hidden: false, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            //{ name: 'MMS_ANTHIYUR_Count', index: 'MMS_ANTHIYUR_Count', align: 'center', width: '75' },
            //{ name: 'MMS_ANTHIYUR', index: 'MMS_ANTHIYUR', stype: 'select', align: 'center', width:'75',editable: true, hidden: false, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            //{ name: 'MCOE_ANTHIYUR_Count', index: 'MCOE_ANTHIYUR_Count', align: 'center', width: '75' },
            //{ name: 'MCOE_ANTHIYUR', index: 'MCOE_ANTHIYUR', stype: 'select', align: 'center', width:'75',editable: true, hidden: false, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            //{ name: 'MTTI_ANTHIYUR_Count', index: 'MTTI_ANTHIYUR_Count', align: 'center', width: '75' },
            //{ name: 'MTTI_ANTHIYUR', index: 'MTTI_ANTHIYUR', stype: 'select', align: 'center', width:'75',editable: true, hidden: false, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
            //{ name: 'RPS_KOTAGIRI_Count', index: 'RPS_KOTAGIRI_Count', align: 'center', width: '75' },
            //{ name: 'RPS_KOTAGIRI', index: 'RPS_KOTAGIRI', stype: 'select', align: 'center', width:'75',editable: true, hidden: false, searchoptions: { sopt: ['eq'], value: { Yes: 'Yes', No: 'No' } }, edittype: 'select', editoptions: { sopt: ['eq'], value: { 'true': 'Yes', 'false': 'No' } } },
        ],
        rowNum: 30,
        rowList: [ 30, 40, 120],
        pager: '#CampusWiseUsageModule_vwGridPager',
        sortname: 'Id',
        viewrecords: true,
        onSelectRow: function (id) {
            jQuery('#CampusWiseUsageModule_vwGrid').editRow(id, true);
            if (id && id !== lastSel) {
                $("#CampusWiseUsageModule_vwGrid").jqGrid('restoreRow', lastSel);
                jQuery("#CampusWiseUsageModule_vwGrid").jqGrid('editRow', id, true);
                $("#" + lastSel + "_IsCheque").prop("checked", false);
                lastSel = id;
            }
            if (id > 0) {
                $("#" + id + "_Module").prop("disabled", true);
            }
        },
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        height: 270,
        sortorder: "Asc",
        caption: "Campus Wise Usage Module",
        editurl: '/StudentsReport/AddCampusWiseModuleRecords',
        multiselect: true,
        multiboxonly: true

    }).trigger("reloadGrid");
    //jQuery("#CampusWiseUsageModule_vwGrid").jqGrid('setGroupHeaders', {
    //    useColSpanStyle: false, groupHeaders: [
    //        { startColumnName: 'MCS_ANTHIYUR_Count', numberOfColumns: 2, titleText: 'MCS_ANTHIYUR' },
    //        { startColumnName: 'MHSS_ANTHIYUR_Count', numberOfColumns: 2, titleText: 'MHSS_ANTHIYUR' },
    //        { startColumnName: 'MMS_ANTHIYUR_Count', numberOfColumns: 2, titleText: 'MMS_ANTHIYUR' },
    //        { startColumnName: 'MCOE_ANTHIYUR_Count', numberOfColumns: 2, titleText: 'MCOE_ANTHIYUR' },
    //        { startColumnName: 'MTTI_ANTHIYUR_Count', numberOfColumns: 2, titleText: 'MTTI_ANTHIYUR' },
    //        { startColumnName: 'RPS_KOTAGIRI_Count', numberOfColumns: 2, titleText: 'RPS_KOTAGIRI' }
    //    ]

    //});
    jQuery("#CampusWiseUsageModule_vwGrid").jqGrid('setGroupHeaders', {
        useColSpanStyle: false, groupHeaders: [
            { startColumnName: 'IBMain_Count', numberOfColumns: 2, titleText: 'IBMain' },
            { startColumnName: 'IBKG_Count', numberOfColumns: 2, titleText: 'IBKG' },
            { startColumnName: 'ChennaiMain_Count', numberOfColumns: 2, titleText: 'ChennaiMain' },
            { startColumnName: 'ChennaiCity_Count', numberOfColumns: 2, titleText: 'ChennaiCity' },
            { startColumnName: 'Ernakulam_Count', numberOfColumns: 2, titleText: 'Ernakulam' },
            { startColumnName: 'ErnakulamKG_Count', numberOfColumns: 2, titleText: 'ErnakulamKG' },
            { startColumnName: 'Karur_Count', numberOfColumns: 2, titleText: 'Karur' },
            { startColumnName: 'KarurKG_Count', numberOfColumns: 2, titleText: 'KarurKG' },
            { startColumnName: 'Tirupur_Count', numberOfColumns: 2, titleText: 'Tirupur' },
            { startColumnName: 'TirupurKG_Count', numberOfColumns: 2, titleText: 'TirupurKG' },
            { startColumnName: 'TipsSaran_Count', numberOfColumns: 2, titleText: 'TipsSaran' }

        ]

    });
    jQuery("#CampusWiseUsageModule_vwGrid").jqGrid('navGrid', '#CampusWiseUsageModule_vwGridPager',
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
         viewicon: 'ace-icon fa fa-search-plus grey',
     },
     {
  
     },
     {
         
     },
     {
         //Delete the records
         url: '/StudentsReport/DeleteCampusWiseUsageModule/', closeOnEscape: true, beforeShowForm: function (frm)
         { selectedrows = $("#CampusWiseUsageModule_vwGrid").jqGrid("getGridParam", "selarrrow"); return { Id: selectedrows } }
        
     }).navButtonAdd("#CampusWiseUsageModule_vwGridPager", {
         caption: "Export To Excel",
         buttonicon: "ui-icon-add",
         onClickButton: function () {
             window.open("/StudentsReport/GetCampusWiseUsageModule_vwListJqGrid" + '?rows=9999&ExprtToExcel=Excel');
         },
         position: "last"
     })
    jQuery("#CampusWiseUsageModule_vwGrid").jqGrid('inlineNav', '#CampusWiseUsageModule_vwGridPager',
             {
                 add: true,
                 addicon: 'ace-icon fa fa-plus-circle purple',
                 addParams: {
                     addRowParams: {
                         aftersavefunc: function (val) {
                             var $self = $(this);
                             setTimeout(function () {
                                 $self.trigger("reloadGrid");
                             }
                                 );
                         }
                     }
                 },
                 edit: false
             },
             {},
             {},
             {})


    //$("#btnSearch").click(function () {
    //    var Module = $("#Module").val();
    //    var Campus = $("#Campus").val();
    //    var IsUsage = $("#IsUsage").val();
    //    if (IsUsage != "" && Campus == "")
    //        return alert("Please Select Campus");
    //    jQuery("#CampusWiseUsageModule_vwGrid").setGridParam(
    //   {
    //       datatype: "json",
    //       url: '/Reports/GetCampusWiseUsageModule_vwListJqGrid',
    //       postData: { Module:Module, Campus:Campus,IsUsage:IsUsage },
    //       page: 1
    //   }).trigger("reloadGrid");
    //});
    //$("#btnReset").click(function () {
    //    $("input[type=text], textarea, select").val("");

    //    jQuery("#CampusWiseUsageModule_vwGrid").setGridParam(
    //   {
    //       datatype: "json",
    //       url: '/Reports/GetCampusWiseUsageModule_vwListJqGrid',
    //       postData: { Module: "",Campus: "",IsUsage: ""},
    //       page: 1
    //   }).trigger("reloadGrid");
    //});
});
//function FillCampusDll() {
//    $.getJSON("/Base/FillAllBranchCode",
//      function (fillbc) {
//          var ddlbc = $("#Campus");
//          ddlbc.empty();
//          ddlbc.append($('<option/>', { value: "", text: "---Select---" }));

//          $.each(fillbc, function (index, itemdata) {
//              ddlbc.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
//          });
//      });
//}



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







