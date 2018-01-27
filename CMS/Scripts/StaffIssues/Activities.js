jQuery(function ($) {
        var grid_selector = "#ActivitiesList";
        var pager_selector = "#ActivitiesListPager";

        $(grid_selector).jqGrid({
            datatype: 'local',
            mtype: 'GET',
            height: '300',
            width: 'auto',
            autowidth: true,
            shrinkToFit: false,
            colNames: ['Id', 'Activity Name', 'Status', 'Performer', 'Created Date', 'Application Role', 'Reference Id'],
            colModel: [
              { name: 'Id', index: 'Id',width:40,  hidden: true },
              { name: 'ActivityFullName', width: 160, index: 'ActivityFullName', sortable: true },
              { name: "Status", index: "Status", width: 90, sortable: false },
              { name: 'Performer', index: 'Performer', width: 160, align: 'left', sortable: true },
              { name: 'CreatedDate', index: 'CreatedDate', width: 90, align: 'left', sortable: true },
              { name: 'AppRole', index: 'AppRole', width: 130, align: 'left', sortable: true },
              { name: 'Id', index: 'Id', width: 100, align: 'left', sortable: false },
              ],
            pager: pager_selector,
            rowNum: '10',
            rowList: [5, 10, 20, 50, 100, 150, 200],
            sortname: 'Id',
            sortorder: "Desc",
            viewrecords: true,
            altRows: true,
            multiselect: true,
            multiboxonly: true,
            loadComplete: function () {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
            },
            caption: 'Activities List'
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


        $(document).on('ajaxloadstart', function (e) {
            $(grid_selector).jqGrid('GridUnload');
            $('.ui-jqdialog').remove();
        });
    });
