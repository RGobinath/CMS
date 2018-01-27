$(function () {
    jQuery("#DistanceCoveredList").jqGrid({
        url: '../../Transport/DistanceCoveredListJqGrid',
        datatype: 'json',
        mtype: 'GET',
        colNames: ['Id', 'Ref No', 'Campus', 'Vehicle Name', 'Vehicle No', 'Source', 'Destination', 'Distance Covered', 'Created Date', 'Created By', 'Status'],
        colModel: [
        //if any column added need to check formateadorLink
             {name: 'Id', index: 'Id', hidden: true, key: true },
             { name: 'RefNo', index: 'RefNo', formatter: formateadorLink, sortable: true },
             { name: 'Campus', index: 'Campus', sortable: true },
             { name: 'Type', index: 'Type', sortable: true },
             { name: 'VehicleNo', index: 'VehicleNo', sortable: true },
             { name: 'Source', index: 'Source', sortable: true },
             { name: 'Destination', index: 'Destination', sortable: true },
             { name: 'DistanceCovered', index: 'DistanceCovered', sortable: true },
             { name: 'CreatedDate', index: 'CreatedDate', sortable: true, width: 170 },
             { name: 'CreatedBy', index: 'CreatedBy', sortable: true, width: 170 },
             { name: 'Status', index: 'Status', sortable: true },
             ],
        pager: '#DistanceCoveredListPager',
        rowNum: '10',
        rowList: [5, 10, 20, 50, 100, 150, 200],
        sortname: 'Id',
        sortorder: 'Desc',
        height: '230',
        width: 1225,
        // autowidth: true,
        // shrinkToFit: true,
        viewrecords: true,
        caption: 'Distance Covered List'
        // forceFit: true,
        //  multiselect: true
    });
    $("#DistanceCoveredList").navGrid('#DistanceCoveredListPager', { add: false, edit: false, del: false, search: false, refresh: false });
});
function formateadorLink(cellvalue, options, rowObject) {
    return "<a href=/Transport/ShowVehicleDistanceCovered?Id=" + rowObject[0] + ">" + cellvalue + "</a>";
}