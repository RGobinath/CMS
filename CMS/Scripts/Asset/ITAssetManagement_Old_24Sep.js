﻿$(function ($) {
    "use strict";
    var grid_selector = "#ITAssetjqGrid";
    var pager_selector = "#ITAssetjqGridPager";

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

    buildjqGridskeleton();
    FillAssetName();


    $('#btnSearch').click(function () {
        var AssetId = $('#ddlAsset').val();
        buildITAssetDetailsGrid(AssetId);
    });

    $('#btnReset').click(function () {
        $(grid_selector).clearGridData();
        $("input[type=text], textarea, select").val("");
        buildjqGridskeleton();
    });
    $('#btnAdd').click(function () {
        if ($('#ddlAsset').val() == null || $('#ddlAsset').val() == 'Undefined' || $('#ddlAsset').val() == " ") {
            ErrMsg('Asset Name reuired!!');
            return false;
        } else {
            ModifiedLoadPopupDynamicaly("/Asset/AddNewITAsset?AssetId=" + $('#ddlAsset').val(), $('#newITAsset'), function () {
                LoadSetGridParam($('#newITAsset'))
            }, function () { }, 550, 335, "Add Asset");
        }

    });
    $.getJSON("/Base/FillCampus",
            function (fillig) {
                var ddlcam = $("#ddlCampus");
                ddlcam.empty();
                ddlcam.append("<option value=' '> Select </option>");
                $.each(fillig, function (index, itemdata) {
                    ddlcam.append($('<option/>',
                        {
                            value: itemdata.Value,
                            text: itemdata.Text
                        }));
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
    function showAssetTransactionFormat(cellvalue, options, rowObject) {
        return '<a href=/Asset/ITAssetTransaction?AssetDet_Id=' + options.rowId + '>' + cellvalue + '</a>';
    }
    function buildITAssetDetailsGrid(AssetId) {
        $(grid_selector).clearGridData();
        $(grid_selector).jqGrid('GridUnload');
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '/Asset/GetITAssetManagementColModelAndColName?AssetId=' + AssetId,
            success: function (data) {
                var ColNameList = JSON.stringify(data.ColNames);
                var ColNameString = new String();
                ColNameString = ColNameList.toString().replace(/"/g, "");
                var ColNames = eval(ColNameString);

                var ColModelList = JSON.stringify(data.ColModels);
                var ColModelString = new String();
                ColModelString = ColModelList.toString().replace(/"/g, "");
                var ColModels = eval(ColModelString);

                var colD = data.list;

                $(grid_selector).jqGrid({
                    jsonReader: {
                        cell: "",
                        id: "0"
                    },
                    datatype: 'jsonstring',
                    mtype: 'POST',
                    height: 250,
                    width: 1260,
                    colNames: ColNames,
                    colModel: ColModels,
                    datastr: colD,
                    pager: pager_selector,
                    altRows: true,
                    multiselect: true,
                    multiboxonly: true,
                    sortname: 'Id',
                    rowNum: 5,
                    rowList: [5, 10, 20, 50],
                    viewrecords: true,
                    loadComplete: function () {
                        var table = this;
                        //var i = getColumnIndexByName.call(this, 'AssetCode');
                        //$("tbody>tr.jqgrow>td:nth-child(" + (i + 1) + ")>a", this).click(function (e) {
                        //    var hash = e.currentTarget.hash;// string like "#?id=0"
                        //    if (hash.substring(0, 5) === '#?id=') {
                        //        var id = hash.substring(5, hash.length);
                        //        var text = this.textContent || this.innerText;
                        //        alert("clicked the row with id='" + id + "'. Link contain '" + text + "'");
                        //        location.href = "http://en.wikipedia.org/wiki/" + text;
                        //    }
                        //    e.preventDefault();
                        //});
                        setTimeout(function () {
                            updatePagerIcons(table);
                            enableTooltips(table);
                        }, 0);
                    },
                    caption: '<i class="ace-icon fa fa-list"></i> Asset Management List'
                });

            },
            error: function (x, e) {
                alert(x.readyState + " " + x.status + " " + e.msg);
            }
        });
    }



});
function FillAssetName() {
    FormId = $('#ddlCampus').val();
    $.getJSON("/Asset/FillITAssetName", { FormId: FormId },
           function (fillig) {
               var ddlcam = $("#ddlAsset");
               ddlcam.empty();
               ddlcam.append("<option value=' '> ----Select---- </option>");
               $.each(fillig, function (index, itemdata) {
                   ddlcam.append($('<option/>',
                       {
                           value: itemdata.Value,
                           text: itemdata.Text
                       }));
               });
           });
}
function buildjqGridskeleton() {
    $('#ITAssetjqGrid').clearGridData();
    $('#ITAssetjqGrid').jqGrid({
        datatype: 'local',
        datatype: "jsonstring",
        height: 200,
        colNames: ['Id'],
        colModel: [
                      { name: 'Id', index: 'Id', width: 90, key: true, hidden: true }
        ],
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: '#ITAssetjqGridPager',
        caption: '<i class="ace-icon fa fa-list"></i> Asset Management List'
    });
}