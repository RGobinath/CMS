jQuery(function ($) {



    var grid_selector = "#AddStudentLocationDetailsList";
    var pager_selector = "#AddStudentLocationDetailsListPager";

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
  
    jQuery(grid_selector).jqGrid({
        url: '/Transport/AddStudentLocationDetailsListJqGrid',
        datatype: 'json',
        height: 200,
        colNames: ['Id', 'Campus', 'Location Name', 'Location Tamil Name'],
        colModel: [
                { name: 'Id', index: 'Id', width: 50, align: 'left', hidden: true, key: true },
                 { name: 'Campus', index: 'Campus', search: true, editable: false, width: 90, editoptions: { disabled: true, class: 'NoBorder' }, stype: 'select', searchoptions: {
                     sopt: ["eq", "ne"],
                     value: ":All;IB KG:IB KG;IB MAIN:IB MAIN;KARUR KG:KARUR KG;KARUR:KARUR MAIN;TIRUPUR KG:TIRUPUR KG;TIRUPUR:TIRUPUR MAIN;ERNAKULAM KG:ERNAKULAM KG;ERNAKULAM:ERNAKULAM MAIN;CHENNAI CITY:CHENNAI CITY;CHENNAI MAIN:CHENNAI MAIN;TIPS SALEM:TIPS SALEM;TIPS SARAN:TIPS SARAN"
                 }
                     },
                { name: 'LocationName', index: 'LocationName', width: 150, align: 'left' },
                { name: 'TamilDescription', index: 'TamilDescription', width: 150, align: 'left', search: false }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: pager_selector,
        altRows: true,
        multiselect: true,
        multiboxonly: true,
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        },
        caption: "<i class='fa fa-location-arrow'></i>&nbsp;Location Name Details"
    });

    $(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true });
    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size


    //switch element when editing inline
    function aceSwitch(cellvalue, options, cell) {
        setTimeout(function () {
            $(cell).find('input[type=checkbox]')
                    .addClass('ace ace-switch ace-switch-5')
                    .after('<span class="lbl"></span>');
        }, 0);
    }
  

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
                refresh: true,
                refreshicon: 'ace-icon fa fa-refresh green',
                view: false,
                viewicon: 'ace-icon fa fa-search-plus grey'
            },
            {}, { width: 'auto', url: '/Transport/AddType' }, {}, {}, {})

    function styleCheckbox(table) {
    }
    function updateActionIcons(table) {
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


    $(document).on('ajaxloadstart', function (e) {
        $(grid_selector).jqGrid('GridUnload');
        $('.ui-jqdialog').remove();
    });

    $("#btnAddLocations").click(function () {
        if ($('#branchcodeddl').val() == "") {
            ErrMsg("Please select Campus!");
            return false;
        }
        if ($('#LocationName').val() == "") {
            ErrMsg("Please select Location Name!");
            return false;
        }        
        $.ajax({
            type: 'POST',
            async: false,
            url: '/Transport/AddStudentLocationDetailsList?Campus=' + $("#branchcodeddl").val() + '&LocationName=' + $("#LocationName").val() + '&TamilDescription=' + $("#LocationTamilDescription").val(),
            success: function (data) {
                $('#LocationName').val("");
                $('#Campus').val("")
                if (data != null)
                    InfoMsg("Location added successfully!");
                $("input[type=text], textarea, select").val("");
            }
        });
        $(grid_selector).trigger("reloadGrid");
    });
    $("#btnReset").click(function () {
        $("input[type=text], textarea, select").val("");
        $(grid_selector).setGridParam(
                {
                    datatype: "json",
                    url: '/Transport/AddStudentLocationDetailsListJqGrid',
                    postData: { Campus: $("#branchcodeddl").val(), LocationName: $("#LocationName").val() },
                    page: 1
                }).trigger("reloadGrid");
    });
  

    google.maps.event.addDomListener(window, 'load', function () {
        var places = new google.maps.places.Autocomplete(document.getElementById('LocationName'));
        google.maps.event.addListener(places, 'place_changed', function () {
            var place = places.getPlace();
            var address = place.formatted_address;
            var latitude = place.geometry.location.k;
            var longitude = place.geometry.location.D;
            var mesg = "Address: " + address;
            mesg += "\nLatitude: " + latitude;
            mesg += "\nLongitude: " + longitude;
            
            $.ajax({
                type: 'POST',
                async: false,
                url: '/Transport/CheckCampusLocationMaster?Campus=' + $("#branchcodeddl").val() + '&LocationName=' + address.split(',')[0],
                success: function (data) {
                    if (data == false) {
                        var newScript = document.createElement('script');
                        newScript.type = 'text/javascript';
                        var source = 'https://www.googleapis.com/language/translate/v2?key=AIzaSyAyPscCtnWh7bXzDxPN0fiKHLseUPJki8c&source=en&target=ta&callback=translateText&q=' + address.split(',')[0];
                        newScript.src = source;
                        // When we add this script to the head, the request is sent off.
                        document.getElementsByTagName('head')[0].appendChild(newScript);
                    }
                    else {
                        $("#LocationName").val("");
                        InfoMsg("Already added");
                    }
                }
            });
        });
    });



    });

$.getJSON("/Base/FillAllBranchCode",
     function (fillcampus) {
         var ddlcam = $("#branchcodeddl");
         ddlcam.empty();
         ddlcam.append($('<option/>',
        {
            value: "",
            text: "Select One"

        }));

         $.each(fillcampus, function (index, itemdata) {
             ddlcam.append($('<option/>',
                 {
                     value: itemdata.Value,
                     text: itemdata.Text
                 }));
         });
     });

   
