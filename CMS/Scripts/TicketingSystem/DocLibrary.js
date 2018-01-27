$(function () {
        var grid_selector = "#DocLibGrid";
        var pager_selector = "#DocLibGridPage";

        var grdDoc = $(grid_selector);

        //resize to fit page size
        $(window).on('resize.jqGrid', function () {
            $(grid_selector).jqGrid('setGridWidth', $("#JqGrid").width());
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
        grdDoc.jqGrid({
            mtype: 'GET',
            url: '/Assess360/DocumentsList?AppName=ETKT&EntityRefId=' + $('#Id').val(),
            datatype: 'json',
            height: '70',
            autowidth: true,
            shrinkToFit: true,
            colNames: ['', 'Document Type', 'Document Name', 'Document Size', 'Uploaded By', 'Uploaded Date'],
            colModel: [
                          { name: 'Id', index: 'Id', sortable: false, hidden: true, key: true },
                          { name: 'DocumentType', index: 'DocumentType', sortable: false },
                          { name: 'DocumentName', index: 'DocumentName', sortable: false },
                          { name: 'DocumentSize', index: 'DocumentSize', sortable: false },
                          { name: 'UploadedBy', index: 'UploadedBy', sortable: false },
                          { name: 'UploadedDate', index: 'UploadedDate', sortable: false }
                          ],
            pager: pager_selector,
            rowNum: '10',
            rowList: [5, 10, 20, 50, 100, 150, 200],
            multiselect: true,
            viewrecords: true,
            loadComplete: function (data) {
                var ids = grdDoc.getDataIDs();
                for (var i = 0; i < ids.length; i++) {
                    var id = ids[i];
                    var rowData = grdDoc.getRowData(id);
                    if (rowData.UploadedBy.toUpperCase() != $('#loggedInUserId').val().toUpperCase()) {
                        $('#jqg_' + id).attr("disabled", true);
                        $('#cb_DocLibGrid').hide();
                    }
                }
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(window).triggerHandler('resize.jqGrid');
            },
            caption:"<i class='ace-icon fa fa-folder-o'></i>&nbsp;&nbsp;Document List",
        });
        //$(window).triggerHandler('resize.jqGrid');
        //navButtons Add, edit, delete
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
            {}, {}, {})

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

        /* upload button code //Ajax File Uploading */

        function SpecialCharacters(strValidate) {
            if (strValidate.indexOf('&') != -1 || strValidate.indexOf("'") != -1 ||
                strValidate.indexOf(";") != -1 || strValidate.indexOf("#") != -1) {
                return true;
            } else {
                return false;
            }
        }

        function validateDocReq(msg, reqField, isValid) {
            var fieldVal = $('#' + reqField).val();
            if ((typeof isValid != 'undefined' && isValid) || fieldVal == null || fieldVal == "") {
                ErrMsg(msg, function () { $('#' + reqField).focus(); });
                return false;
            } else {
                return true;
            }
        }

        $('#btnUploadDoc').click(function () {
            var splitstr = $('#uploadedFile').val().split('\\');
            var fileName = splitstr[splitstr.length - 1];
            var DocTypeText = $('#doctype option:selected').text();
            if (!validateDocReq("Please Select Document Type.", 'doctype')) { }
            else if (!validateDocReq("No document found. Please select a file to upload.", 'uploadedFile')) { }
            else if (!validateDocReq("Special characters (&,#,;') are not supported in document file names. Please amend the file name before upload.", 'uploadedFile', SpecialCharacters(fileName))) { }
            else if (!validateDocReq("The attached file does not contain file extension.", 'uploadedFile', (fileName.lastIndexOf('.') == -1))) { }
            else {
                ajaxUploadDocs();
                return false;
            }
        });

        function ajaxUploadDocs() {
            //
            var DocTypeText = $('#doctype option:selected').text();

            $.ajaxFileUpload({
                url: '/Assess360/UploadDocuments?AppName=ETKT&EntityRefId=' + $('#Id').val() + "&docType=" + DocTypeText,
                secureuri: false,
                fileElementId: 'uploadedFile',
                dataType: 'json',
                success: function (data, status) {
                    grdDoc.setGridParam({ url: '/Assess360/DocumentsList?AppName=ETKT&EntityRefId=' + $('#Id').val() }).trigger("reloadGrid");
                    $('#doctype').val('');
                    if (typeof data.result != 'undefined' && data.result != '') {
                        if (typeof data.success != 'undefined' && data.success == true) {
                            InfoMsg(data.result);
                        } else {
                            ErrMsg(data.result);
                        }
                    }
                },
                error: function (data, status, e) {
                    grdDoc.setGridParam({ url: '/Assess360/DocumentsList?AppName=ETKT&EntityRefId=' + $('#Id').val() }).trigger("reloadGrid");
                    $('#doctype').val('');
                }
            });
        }

        $('#bntRemoveDoc').click(function () {
            var selDocs;
            selDocs = $(grid_selector).getGridParam('selarrrow');
            if (selDocs.length == 0) {
                ErrMsg('Please select the document', function () { $('#bntRemoveDoc').focus(); });
                return;
            }
            $(grid_selector).setGridParam({
                url: '<% =Url.Content("~/Assess360/DeleteDocs/")%>?delDocs=' + selDocs.toString()
            }).trigger("reloadGrid");
            });
});
    function DownloadDocument(id1) {
        window.location.href = "/Assess360/DownloadDocuments?AppName=ETKT&DocId=" + id1;
    }