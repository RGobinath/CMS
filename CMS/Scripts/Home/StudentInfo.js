$(function () {
    var frmdate = 'From Date'; var todate = 'To Date'; var frmDate = ""; var toDate = ""; var idno = ""; var name = ""; var ret1 = ""; var section = ""; var campname = ""; var grade = "";
    var bType = ""; var astatus = ""; var acayear = ""; var Gender = ""; var vanno = "";

    var grid_selector = "#StudentList";
    var pager_selector = "#StudentListPager";
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
    gridload();

    function gridload() {

        idno = $("#idno").val();
        name = $("#name").val();
        section = $("#ddlSection").val();
        campname = $("#ddlCampus").val();
        grade = $("#ddlGrade").val();
        bType = $("#ddlBoardingType").val();
        astatus = $("#ddlAdmissionStatus").val();
        acayear = $("#ddlacademicyear").val();
        Gender = $("#ddlGender").val();
        vanno = $("#vanno").val();

        jQuery(grid_selector).jqGrid({

            url: '/Home/StudentInfoListJqGrid',
            postData: { idno: idno, name: name, Gender: Gender, section: section, campname: campname, grade: grade, bType: bType, vanno: vanno, astatus: astatus, acayear: acayear, frmDate: frmDate, toDate: toDate },
            datatype: 'json',
            height: 250,
            colNames: ['Campus', 'Roll No', 'Student Name', 'Gender', 'DOB', 'Grade', 'Section', 'Academic Year', 'Boarding Type', 'Blood Group', 'Food Habit', 'Required Transport', 'Route No', 'Father Name', 'Mother Name', 'General Mobile Number', 'Father Mobile Number', 'Mother Mobile Number', 'General Email', 'Father Email', 'Mother Email', 'Address', 'Father Occupation', 'Mother Occupation', 'Applied Date'],
            colModel: [
              { name: 'Campus', index: 'Campus', width: 85 },              
              { name: 'NewId', index: 'NewId', width: 70 },
              { name: 'Name', index: 'Name', width: 120 },
              { name: 'Gender', index: 'Gender', width: 50 },
              { name: 'DOB', index: 'DOB', width: 75 },
              { name: 'Grade', index: 'Grade', width: 40 },
              { name: 'Section', index: 'Section', width: 50 },
              { name: 'AcademicYear', index: 'AcademicYear', width: 75 },
              { name: 'BoardingType', index: 'BoardingType', width: 70 },
              { name: 'BGRP', index: 'BGRP', width: 40 },
              { name: 'FoodType', index: 'FoodType', width: 50 },

              { name: 'TransportRequired', index: 'TransportRequired', width: 60 },

              { name: 'VanNo', index: 'VanNo', width: 40 },
              { name: 'FatherName', index: 'FatherName', width: 120 },
              { name: 'MotherName', index: 'MotherName', width: 120 },
              { name: 'MobileNo', index: 'MobileNo', width:75},
              { name: 'FatherMobileNumber', index: 'FatherMobileNumber', width: 75 },
              { name: 'MotherMobileNumber', index: 'MotherMobileNumber', width: 75 },
              { name: 'EmailId', index: 'EmailId', width: 125 },
              { name: 'FatherEmail', index: 'FatherEmail', width: 125 },
              { name: 'MotherEmail', index: 'MotherEmail', width: 125 },
              { name: 'Address', index: 'Address', width: 130 },
              { name: 'FatherOccupation', index: 'FatherOccupation', width: 80 },
              { name: 'MotherOccupation', index: 'MotherOccupation', width: 80 },
              { name: 'CreatedDate', index: 'CreatedDate', width: 75 }
              ],
            viewrecords: true,
            rowNum: '5',
            rowList: [5, 10, 20, 50, 100, 150, 200],
            
            autowidth: true,
            sortname: 'Name',
            sortorder: 'asc',
            height: '150',
            pager: pager_selector,
            altRows: true,
            shrinkToFit: false,            
            multiselect: true,
            multiboxonly: true,
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
            },
            caption: "<i class='ace-icon fa fa-user'></i>&nbsp;Student List"

        });
        $(window).triggerHandler('resize.jqGrid');
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
            {}, {}, {});

        jQuery(grid_selector).jqGrid('navButtonAdd', pager_selector, {
            caption: "<i class='fa fa-file-excel-o'></i> Export To Excel",
            onClickButton: function () {
                window.open("StudentInfoListJqGrid" + '?idno=' + idno + '&name=' + name + '&section=' + section + '&acayear=' + acayear + '&Gender=' + Gender + '&campname=' + campname + '&grade=' + grade + '&bType=' + bType + '&astatus=' + astatus + '&rows=9999' + '&ExptXl=1');
            }
        });

        //trigger window resize to make the grid get the correct size
        //navButtons Add, edit, delete

    }


    $.getJSON("/Base/FillBranchCode",
             function (fillig) {
                 var ddlcam = $("#ddlCampus");
                 ddlcam.empty();
                 ddlcam.append($('<option/>', { value: "", text: "Select One" }));

                 $.each(fillig, function (index, itemdata) {
                     ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text }));
                 });
             });
   

    //    //init, set watermark text and class
    //        $('#txtFromDate').val(frmdate).addClass('watermark');
    //        //if blur and no value inside, set watermark text and class again.
    //        $('#txtFromDate').blur(function () {
    //            if ($(this).val().length == 0) {
    //                $(this).val(frmdate).addClass('watermark');
    //            }
    //        });

    //     $('#txtFromDate').focus(function () {
    //            if ($(this).val() == frmdate) {
    //                $(this).val('').removeClass('watermark');
    //            }
    //        });

    //        $('#txtToDate').val(todate).addClass('watermark');

    //      $('#txtToDate').blur(function () {
    //            if ($(this).val().length == 0) {
    //                $(this).val(todate).addClass('watermark');
    //            }
    //        });
    //     $('#txtToDate').focus(function () {
    //            if ($(this).val() == todate) {
    //                $(this).val('').removeClass('watermark');
    //            }
    //        });

    $('#reset').click(function () {
        var url = $('#BackUrl').val();
        window.location.href = url;
    });

    $("#ddlCampus").change(function () {
        gradeddl();
        VanNumberddl($("#ddlCampus").val()); // $("#Search").click();

    });
    $("#Search").click(function () {

        frmDate = $("#txtFromDate").val();
        toDate = $("#txtToDate").val();
        if (frmDate == "From Date" && toDate == "To Date") {
            frmDate = "";
            toDate = "";
        }
        else {
            frmDate = $("#txtFromDate").val();
            toDate = $("#txtToDate").val();
        }

        idno = $("#idno").val();
        name = $("#name").val();
        section = $("#ddlSection").val();
        campname = $("#ddlCampus").val();
        grade = $("#ddlGrade").val();
        bType = $("#ddlBoardingType").val();
        astatus = $("#ddlAdmissionStatus").val();
        acayear = $("#ddlacademicyear").val();
        Gender = $("#ddlGender").val();

        vanno = $("#vanno").val();
        $(grid_selector).GridUnload();
        gridload();
    });
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
function gradeddl() {

    var campus = $("#ddlCampus").val();
    $.getJSON("/Admission/CampusGradeddl/", { campus: campus },
            function (modelData) {
                var select = $("#ddlGrade");
                select.empty();
                select.append($('<option/>'
                               , {
                                   value: "",
                                   text: "Select Grade"
                               }));
                $.each(modelData, function (index, itemData) {

                    select.append($('<option/>',
                                  {
                                      value: itemData.gradcod,
                                      text: itemData.gradcod
                                  }));
                });
            });
}
//--------------Van No------------------------/
function VanNumberddl(campus) {
    if (campus != "") {
        $.getJSON("/Base/GetVanRouteNoByCampus", { campus: campus },
                      function (fillig) {
                          var ddlcam = $("#vanno");
                          ddlcam.empty();
                          ddlcam.append($('<option/>', { value: "", text: "Select One" }));
                          $.each(fillig, function (index, itemdata) { ddlcam.append($('<option/>', { value: itemdata.Value, text: itemdata.Text })); });
                      });
                  }

                  $(document).on('ajaxloadstart', function (e) {
                      $(grid_selector).jqGrid('GridUnload');
                      $('.ui-jqdialog').remove();
                  });
}
//-----------------End Van No--------------------/
    