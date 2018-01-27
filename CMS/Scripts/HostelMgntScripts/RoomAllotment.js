

$(function () {

    $("#landingPage").jqGrid({
        url: '/HostelManagement/JqgridRoomAllotment',
        datatype: 'json',
        type: 'GET',
        colNames: ['Id', 'StudID', 'Name', 'NewId', 'Campus', 'Grade', 'Section', 'AcademicYear', 'HstlMst_Id', 'HostelName', 'HostelType', 'Floor', 'Boarding Type', 'RoomMst_Id', 'BedMst_Id', 'BedNumber', 'SIP No', 'Room Allotment', 'Change Room Allotment'],
        colModel: [
        { key: true, name: 'Id', index: 'Id', hidden: true, search: false },//0
        { key: false, name: 'StudID', index: 'StudID', hidden: true, search: false },//1
        { key: false, name: 'Name', index: 'Name', width: 300 },//2
        { key: false, name: 'NewId', index: 'NewId' },//3
        { key: false, name: 'Campus', index: 'Campus' },//4
        { key: false, name: 'Grade', index: 'Grade', width: 100 },//5
        { key: false, name: 'Section', index: 'Section', width: 100 },//6
        { key: false, name: 'AcademicYear', index: 'AcademicYear' },//7
        { key: false, name: 'HstlMst_Id', index: 'HstlMst_Id', hidden: true, search: false },//8
        { key: false, name: 'HostelName', index: 'HostelName', hidden: true, search: false },//9
        { key: false, name: 'HostelType', index: 'HostelType', hidden: true, search: false },//10
        { key: false, name: 'Floor', index: 'Floor', hidden: true, search: false },//11
         { key: false, name: 'BoardingType', index: 'BoardingType' },//12
        { key: false, name: 'RoomMst_Id', index: 'RoomMst_Id', hidden: true, search: false },//13
        { key: false, name: 'BedMst_Id', index: 'BedMst_Id', hidden: true, search: false },//14
        { key: false, name: 'BedNumber', index: 'BedNumber', hidden: true, search: false },//15
          { key: false, name: 'SIPNo', index: 'SIPNo' },//16
        { key: false, name: 'Room_Allotment', index: 'Room_Allotment', formatter: formateadRoomLink, search: false },//17
        { key: false, name: 'ChangeRoomAllotment', index: 'ChangeRoomAllotment', formatter: formateadChangeRoomAllotmentLink, search: false }//18
        ],
        gridComplete: function () {
            debugger;
            var rdata = $("#landingPage").getRowData();
            if (rdata.length > 0) {
                $('.ViewPDF').click(function () {
                    pStudID = $(this).attr('vStudID'); pRoom_Allotment = $(this).attr('vRoom_Allotment'); pChangeRoomAllotment = $(this).attr('vChangeRoomAllotment'); pName = $(this).attr('vName'); pNewId = $(this).attr('vNewId'); pCampus = $(this).attr('vCampus');
                    BookNewRooms($(this).attr('vStudID'), $(this).attr('vRoom_Allotment'), $(this).attr('vName'), $(this).attr('vNewId'), $(this).attr('vCampus'), $(this).attr('vChangeRoomAllotment'));
                });
                $('.ChangePDF').click(function () {
                    pStudID = $(this).attr('vStudID'); pRoom_Allotment = $(this).attr('vRoom_Allotment'); pChangeRoomAllotment = $(this).attr('vChangeRoomAllotment'); pName = $(this).attr('vName'); pNewId = $(this).attr('vNewId'); pCampus = $(this).attr('vCampus');
                    BookChangeRoomAllotment($(this).attr('vStudID'), $(this).attr('vRoom_Allotment'), $(this).attr('vName'), $(this).attr('vNewId'), $(this).attr('vCampus'), $(this).attr('vChangeRoomAllotment'));

                });
            }
        },
        rowNum: 20,
        rowList: [20, 50, 100, 200, 500],
        pager: '#landingPager',
        sortname: 'HostelName',
        viewrecords: true,
        multiselect: false,
        sortorder: "Desc",
        width: 1265,
        height: 350,
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


    function formateadRoomLink(cellvalue, options, rowObject) {
        if (cellvalue == "Allotted") {
            return "<button id='" + rowObject[0] + "' class='btn btn-success btn-block btn-sm ViewPDF' type='button'  vStudID='" + rowObject[1] + "'  vRoom_Allotment='" + rowObject[17] + "' vName='" + rowObject[2] + "' vNewId='" + rowObject[3] + "' vCampus='" + rowObject[4] + "' vChangeRoomAllotment='" + rowObject[18] + "' >" + cellvalue + "</button>";

        } else {

            return "<button id='" + rowObject[0] + "' class='btn btn-danger btn-warning btn-block btn-sm ViewPDF' type='button'  vStudID='" + rowObject[1] + "'  vRoom_Allotment='" + rowObject[17] + "' vName='" + rowObject[2] + "' vNewId='" + rowObject[3] + "' vCampus='" + rowObject[4] + "' vChangeRoomAllotment='" + rowObject[18] + "' >" + cellvalue + "</button>";

        }
    }

    function formateadChangeRoomAllotmentLink(cellvalue, options, rowObject) {
        debugger;
        if (cellvalue == "Room Change") {
            return "<button id='" + rowObject[0] + "' class='btn btn-primary btn-info btn-block btn-sm ChangePDF' type='button'  vStudID='" + rowObject[1] + "'  vRoom_Allotment='" + rowObject[17] + "' vName='" + rowObject[2] + "' vNewId='" + rowObject[3] + "' vCampus='" + rowObject[4] + "' vChangeRoomAllotment='" + rowObject[18] + "' >" + cellvalue + "</button>";

        } else {
            return "<button id='" + rowObject[0] + "' class='btn btn-danger btn-warning btn-block btn-sm ChangePDF' type='button'  vStudID='" + rowObject[1] + "'  vRoom_Allotment='" + rowObject[17] + "' vName='" + rowObject[2] + "' vNewId='" + rowObject[3] + "' vCampus='" + rowObject[4] + "' vChangeRoomAllotment='" + rowObject[18] + "' >" + cellvalue + "</button>";
        }
    }
    function BookNewRooms(pStudID, vRoom_Allotment, pName, pNewId, pCampus, pChangeRoomAllotment) {
        if (vRoom_Allotment == "Allotted") {
            LoadPopupDynamicaly1("/HostelManagement/RoomBookingForm?studId=" + pStudID + '&Flag=Allotted', $('#Booking'),
                function () {
                    // LoadSetGridParam($('#ActivitiesList'), "/Home/ActivitiesListJqGrid?Id=" + ActivityId)
                }, function () { }, 900, 500, "Room Booking Details");
        } else {
            LoadPopupDynamicaly1("/HostelManagement/RoomBookingForm?studId=" + pStudID, $('#Booking'),
               function () {
                   // LoadSetGridParam($('#ActivitiesList'), "/Home/ActivitiesListJqGrid?Id=" + ActivityId)
               }, function () { }, 900, 500, "Room Booking Details");
        }

    }

    function BookChangeRoomAllotment(pStudID, vRoom_Allotment, pName, pNewId, pCampus, pChangeRoomAllotment) {
        if (pChangeRoomAllotment == "Room Change") {
            LoadPopupDynamicaly1("/HostelManagement/RoomBookingForm?studId=" + pStudID + '&Flag=ChangeRoomAllotment', $('#Booking'),
                function () {
                    // LoadSetGridParam($('#ActivitiesList'), "/Home/ActivitiesListJqGrid?Id=" + ActivityId)
                }, function () { }, 900, 500, "Room Booking Details");
        } else {
            alert('Not Changed');
            //LoadPopupDynamicaly1("/HostelManagement/RoomBookingForm?studId=" + pStudID, $('#Booking'),
            //   function () {
            //       // LoadSetGridParam($('#ActivitiesList'), "/Home/ActivitiesListJqGrid?Id=" + ActivityId)
            //   }, function () { }, 900, 500, "Room Booking Details");
        }

    }

    function ShowComments(ActivityId, UserType) {

    }

    function LoadSetGridParam(GridId, brUrl) {
        GridId.setGridParam({
            url: brUrl,
            datatype: 'json',
            mtype: 'GET',
            page: 1
        }).trigger("reloadGrid");
    }
    var clbPupGrdSel = null;
    function LoadPopupDynamicaly1(dynURL, ModalId, loadCalBack, onSelcalbck, width) {
        if (width == undefined) { width = 700; }
        //if (ModalId.html().length == 0) {
        $.ajax({
            url: dynURL,
            type: 'GET',
            async: false,
            closeOnEscape: false,
            dataType: 'html', // <-- to expect an html response
            success: function (data) {
                ModalId.dialog({
                    height: 'auto',
                    width: width,
                    modal: true,
                    closeOnEscape: false,
                    close: function (event, ui) {
                        $("#landingPage").trigger("reloadGrid");
                        // $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
                    },
                    title: '<i class="fa fa-list"></i>&nbsp;<label>Room Details</label>',
                    buttons: {}
                });
                ModalId.html(data);
            }
        });
        clbPupGrdSel = onSelcalbck;
        ModalId.dialog('open');
        CallBackFunction(loadCalBack);
    }



});