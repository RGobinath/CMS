$(function () {


    var grid_selector = "#driverfamilyjqgrid";
    var pager_selector = "#driverfamilygridpager";


 


    //resize to fit page size
    $(window).on('resize.jqGrid', function () {
        $(grid_selector).jqGrid('setGridWidth', $(".tab-content").width());
    })

    //resize on sidebar collapse/expand
    var parent_column = $(grid_selector).closest('[class*="col-"]');
    $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
        if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
            //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
            setTimeout(function () {
                var page_width = $(".page-content").width();
                page_width = page_width - 300;
                $(grid_selector).jqGrid('setGridWidth', page_width);
            }, 0);
        }
    })


    jQuery(grid_selector).jqGrid({
        url: '/Transport/familyjqgrid?DriverRegNo=' + $('#DriverRegNo').val(),
        datatype: 'json',
        height: 100,
        colNames: ['Id','Relation Type', 'Name', 'Age', 'Occupation', 'Contact No.'],
        colModel: [
                    { name: 'Id', index: 'Id', align: 'left', sortable: false, hidden: true, key: true },
                    { name: 'Relationship', index: 'Relationship', align: 'left', sortable: false },
                    { name: 'Name', index: 'Name', align: 'left', sortable: false },
                    { name: 'Age', index: 'Age', align: 'left', sortable: false },
                    { name: 'Occupation', index: 'Occupation', align: 'left', sortable: false },
                    { name: 'ContactNo', index: 'ContactNo', align: 'left', sortable: false, hidden: false, key: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [25, 50, 100, 500],
        pager: pager_selector,
        sortname: 'Id',
        sortorder: 'Asc',
        autowidth:true,
        multiselect: true,
       // caption: "<i class='fa fa-users'></i>&nbsp;Driver Family Details",
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);
                updateActionIcons(table);
                updatePagerIcons(table);
                enableTooltips(table);
            }, 0);
        }
       

    });


    $(window).triggerHandler('resize.jqGrid'); //trigger window resize to make the grid get the correct size
    //$(grid_selector).jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
    //navButtons
    jQuery(grid_selector).jqGrid('navGrid', pager_selector,
            {
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
            }, {}, {}, {}, {})

    //For pager Icons
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
    $('#addFamily').click(function () {
       
        var e = document.getElementById("relationtype");
        
        if (document.getElementById("relationtype").value == "") {
            ErrMsg('Please Enter Relation type');
            return false;
        }
      
        if (document.getElementById("FMname").value == "") {
            ErrMsg('Please Enter Name');
            return false;
        }
      
        else {
            if (document.getElementById("FMDOB").value == "") {
                ErrMsg('Please Enter Date of Birth');
            }
            if (document.getElementById("FMoccupation").value == "") {
                document.getElementById("FMoccupation").value == "";
            }
         
            if (document.getElementById("FMmobile").value == "") {
                document.getElementById("FMmobile").value == "";
            }
            var relationtype = e.options[e.selectedIndex].value;
            var FMname = document.getElementById("FMname").value;
            var FMDOB = document.getElementById("FMDOB").value;
            var FMoccupation = document.getElementById("FMoccupation").value;
            var FMage = document.getElementById("FMage").value;
            var FMmobile = document.getElementById("FMmobile").value;
            var regno = $('#DriverRegNo').val();
          
          
            $.ajax({
                //url: '/Admission/AddFamilydetails?relationtype=' + relationtype + '&name1=' + name1 + '&name2=' + name2 + '&mobile=' + mobile + '&age=' + age + '&email=' + email + '&occupation=' + occupation + '&compn=' + compname + '&compa=' + compadd + '&emptype=' + emptype + '&stayingwithchild=' + stayingwithchild + '&pickupcard=' + pickupcard,
                url: '/Transport/AddDriverFamilydetails/',
                type: 'POST',
                dataType: 'json',
                data: { relationtype: relationtype, FMname: FMname, FMDOB: FMDOB, FMoccupation: FMoccupation, age: FMage, FMmobile: FMmobile, DriverRegNo: regno },
                traditional: true,
                success: function (data) {
                    $(grid_selector).setGridParam({ url: '/Transport/familyjqgrid?DriverRegNo=' + $('#DriverRegNo').val() }).trigger("reloadGrid");
                },
                loadError: function (xhr, status, error) {
                    msgError = $.parseJSON(xhr.responseText).Message;
                    ErrMsg(msgError, function () { });
                }
            });


            return true;
        }
    });



    $("#relationtype").change(function () {
        //          alert('hss');
        document.getElementById("FMname").value = "";
        document.getElementById("FMDOB").value = "";
        document.getElementById("FMage").value = "";
        document.getElementById("FMoccupation").value = "";
        document.getElementById("FMmobile").value = "";    //FMage
    
    });


    $('#FMDOB').datepicker({
        format: 'dd/mm/yyyy',
        autoClose: true
    }).on('changeDate', function (selected) {
        var selDate = new Date(selected.date.valueOf());
        var today = new Date();
        years = Math.floor((today.getTime() - selDate.getTime()) / (365.25 * 24 * 60 * 60 * 1000));
        $("#FMage").val(years);
    });
    $('#FMDOB').blur(function () {
        getAge(document.getElementById('FMDOB').value);
    });




});