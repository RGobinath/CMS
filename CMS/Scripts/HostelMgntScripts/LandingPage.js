

$(function () {

    $("#landingPage").jqGrid({
        url: '/HostelManagement/JqgridLandingPage',
        datatype: 'json',
        type: 'GET',
        colNames: ['HstlMst_Id', 'HostelName', 'HostelType', 'Campus', 'Rooms', 'Total Capacity', 'Supervisor'],
        colModel: [
        { key: true, name: 'HstlMst_Id', index: 'HstlMst_Id', hidden: true, search: false },
        { key: false, name: 'HostelName', index: 'HostelName' },
        { key: false, name: 'HostelType', index: 'HostelType' },
        { key: false, name: 'Campus', index: 'Campus' },
        { key: false, name: 'Rooms', index: 'Rooms', search: false },
        { key: false, name: 'Beds', index: 'Beds', search: false },
        { key: false, name: 'InCharge', index: 'InCharge', search: false }
        ],
        rowNum: 20,
        rowList: [20, 50, 100, 200, 500],
        pager: '#landingPager',
        sortname: 'HostelName',
        viewrecords: true,
        multiselect: true,
        sortorder: "Desc",
        width: 1265,
        height: 200,
        caption: 'Hostel Management'
    });
    $("#landingPage").jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size

    $.getJSON("/HostelManagement/GetHostelName/",
    function (fillig) {
        var ddlcam = $("#srchhostelddl");
        ddlcam.empty();
        ddlcam.append($('<option/>',
    {
        value: "",
        text: "Select One"

    }));

        $.each(fillig, function (index, itemdata) {
            ddlcam.append($('<option/>',
    {
        value: itemdata.Value,
        text: itemdata.Text
    }));
        });
    });

    $("#srchhostelddl").change(function () {
        SrchGetTypeddl();
    });

    $('#btnSearch').click(function () {
        if ($('#srchhostelddl').val() == "") { ErrMsg("Please fill the Hostel Name"); return false; }
        $("#landingPage").setGridParam(
                  {
                      datatype: "json",
                      url: '/HostelManagement/JqgridLandingPage',
                      postData: { hstName: $('#srchhostelddl').val(), type: $('#srchddlType').val() },
                      page: 1
                  }).trigger('reloadGrid');
    });
    $("#btnReset").click(function () {
        window.location.href = '/HostelManagement/LandingPage';
    });

    function formateadorLink(cellvalue, options, rowObject) {
        return "<a href=/HostelManagement/HostelDetlsPage?HstlMst_Id=" + rowObject[0] + ">" + cellvalue + "</a>";
    }
    function SrchGetTypeddl() {
        $.getJSON("/HostelManagement/GetType/", { hostelNm: $('#srchhostelddl').val() },
    function (modelData) {
        var select = $("#srchddlType");
        select.empty();
        select.append($('<option/>'
    , {
        value: "",
        text: "Select Type"
    }));
        $.each(modelData, function (index, itemData) {
            select.append($('<option/>',
    {
        value: itemData.Value,
        text: itemData.Text
    }));
        });
    });


    }











});