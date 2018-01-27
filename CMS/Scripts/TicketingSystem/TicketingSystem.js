$(function () {
        if ($("#Status").val() == "LogETicket") {
            $("#ddTicketStatus").val("Open");
            $(".dpldplTS").attr('disabled', true);
            $("#TicketStatus").val("OPEN");
        }
        $("#btnbkToAvailable").click(function () {
            $.ajax({
                url: '/TicketingSystem/MoveBackToAvailable?ActivityId=' + $('#ActivityId').val(),
                type: 'GET',
                dataType: 'json',
                traditional: true,
                success: function (data) {
                    if (data & data == true) {
                        SucessMsg("Eticket-" + $('#Id').val() + " is moved back to available.", function () { window.location.href = "/TicketingSystem/TicketingSystemInbox/"});
                    } else {

                    }
                },
                error: function (xhr, status, error) {
                    ErrMsg($.parseJSON(xhr.responseText).Message);
                }
            });
        });
        if ($('#Status').val() == "Completed" || $('#Status').val() == "Sent") {
            $('input,select,textarea,button').attr('disabled', true);
            $('#btnUploadDoc').removeAttr("disabled");
            $('#uploadedFile').removeAttr("disabled");
            $('#btnbkInbox').removeAttr("disabled");
        }
        $('#btnReopen').attr('disabled', false); // Added by Gobi
        $('input,select,textarea,button').attr('disabled', false); //added by gobi
        if ($.trim($("#AlrtDskMsg").val()) != "") { InfoMsg($("#AlrtDskMsg").val()); }

        $('#btnSave').click(function () { $('#isrejction').val(false); if (eTicketValidtion('Save')) { return true; } return false; });
        $('#btnComplete').click(function () { $('#isrejction').val(false); if (eTicketValidtion('Complete')) { return true; } return false; });
        $('#btnReject').click(function () { $('#isrejction').val(true); if (eTicketValidtion('Reject')) { return true; } return false; });

        $('#btnReopen').click(function () {
            $('#isrejction').val(true);
            if (eTicketValidtion('Reopen')) {
                return true;
            }
            return false;
        });

        $('#btnTicketStatus').show();
        if ($('#activityName').val() == "LogETicket" || $('#activityName').val() == "CloseETicket") {
            $('#btnTicketStatus').hide();
        }

        function eTicketValidtion(btnType) {
            //
            TType = $('#TicketType').val();
            TModule = $('#Module').val();
            //TSvrty = $('#Severity').val();
            TPrty = $('#Priority').val();
            TTStatus = $('#TicketStatus').val();
            TAsgnd = $('#AssignedTo  ').val();
            TSmry = $('#Summary').val();
            TCmnt = $('#Comments').val();
            TActName = $('#activityName').val();

            if (TActName == "LogETicket" && (btnType == "Save" || btnType == "Complete")) {
                if (TType != "Select" && TModule != "Select"
                //&& TSvrty != "Select" 
                    && TPrty != "Select" && TTStatus != "Select" && !isEmptyorNull(TSmry)) {
                    return true;
                } else { ErrMsg("Please enter the mandatory fields.", function () { $("#TicketType").focus(); }); }
            } else if ((TActName == "ResolveETicket" || TActName == "CloseETicket")
             && btnType == "Reject") {
                if (!isEmptyorNull(TCmnt)) { return true; } else {
                    ErrMsg("Please enter the comments for rejection.", function () { $("#Comments").focus(); });
                }
            } else if ((TActName == "ResolveETicket" || TActName == "CloseETicket" || TActName == "ReopenETicket")
             && btnType == "Complete") {
                $('#TicketStatus').val('RESOLVED');
                return true;
            }
            else if ((TActName == "ResolveETicketRejection" || TActName == "CloseETicketRejection" || TActName == "TicketReopen")
             && btnType == "Complete") {
                if (!isEmptyorNull(TCmnt)) { return true; } else { ErrMsg("Please enter the comments for rejection.", function () { $("#Comments").focus(); }); }
            }
            else if ((TActName == "CompleteETicket") && btnType == "Reopen") {
                return true;
            }
            return false;
        }

        bindSelectedVal(document.getElementById("ddTicketType"), $('#TicketType').val());
        bindSelectedVal(document.getElementById("ddModule"), $('#Module').val());
        //bindSelectedVal(document.getElementById("ddSeverity"), $('#Severity').val());
        bindSelectedVal(document.getElementById("ddPriority"), $('#Priority').val());
        bindSelectedVal(document.getElementById("ddTicketStatus"), $('#TicketStatus').val());
        bindSelectedVal(document.getElementById("ddAssignedTo"), $('#AssignedTo').val());

        function bindSelectedVal(DrpDwnObj, SelVal) {
            //
            for (var g = 0; g < DrpDwnObj.options.length; g++) {
                if (DrpDwnObj.options[g].text == SelVal) {
                    DrpDwnObj.selectedIndex = g;
                }
            }
        }

        $('#ddTicketType').change(function () {
            $('#TicketType').val($('#ddTicketType option:selected').text());
        });

        $('#ddModule').change(function () {
            $('#Module').val($('#ddModule option:selected').text());
        });

        //        $('#ddSeverity').change(function () {
        //            $('#Severity').val($('#ddSeverity option:selected').text());
        //        });

        $('#ddPriority').change(function () {
            $('#Priority').val($('#ddPriority option:selected').text());
        });

        $('#ddTicketStatus').change(function () {
            $('#TicketStatus').val($('#ddTicketStatus option:selected').text());
        });

        $('#btnAssign').hide();
        $('#ddAssignedTo').change(function () {
            $('#AssignedTo').val($('#ddAssignedTo option:selected').text());
            //
            if ($('#AssignedTo').val() == "Select") {
                $('#btnAssign').hide();
            } else {
                $('#btnAssign').show();
            }
        });

        $('#btnAssign').click(function () {
            $.ajax({
                url: '/TicketingSystem/AssignActivityToUser?Id=' + $('#Id').val() + '&ActivityId=' + $('#ActivityId').val() + '&UserId=' + $('#AssignedTo').val(),
                type: 'GET',
                dataType: 'json',
                traditional: true,
                success: function (data) {
                    //
                    if (data & data == true) {
                        $('#btnbkInbox').click();
                    } else {
                        ErrMsg("Eticket-" + $('#Id').val() + " is not assigned. Please try again.");
                    }
                },
                error: function (xhr, status, error) {
                    ErrMsg($.parseJSON(xhr.responseText).Message);
                }
            });
        });

        if ($('#activityName').val() == "LogETicket") {
            $('.dpldplAT').attr('disabled', 'disabled');
            $('#Comments').attr('disabled', 'disabled');
            $("#CommentList").hide();
        } else {
            $('.dpldpl').attr('disabled', 'disabled');
        }

        function SaveConfigList(isrejction) {
            return true;
        };

        $("#btnbkInbox").click(function () {
            window.location.href = "/TicketingSystem/TicketingSystemInbox/";
        });
            

        //Comment List Grid
        var grid_selector = "#CommentList";
        var pager_selector = "#CommentListPage";
        //note list Grid
        var grid_selector1 = "#NoteList";
        var pager_selector1 = "#NotePage";

        //resize to fit page size
        $(window).on('resize.jqGrid', function () {
            $(grid_selector).jqGrid('setGridWidth', $("#JqGrid").width());
            $(grid_selector1).jqGrid('setGridWidth', $("#JqGrid").width());
        })
        $(grid_selector).jqGrid({
            mtype: 'GET',
            url: "/TicketingSystem/GetTicketCommentDtlsbyTicketId",
            postData: { TicketId: $('#Id').val() },
            datatype: 'json',
            height: '70',
            autowidth: true,
            colNames: ['Id', 'TicketId', 'Commented By', 'Comment Date', 'Comment Details', 'ResolutionComments', 'Note'],
            colModel: [
                          { name: 'Id', index: 'Id', hidden: true, key: true },
                          { name: 'TicketId', index: 'TicketId', hidden: true },
                          { name: 'CommentedBy', index: 'CommentedBy', hidden: false },
                          { name: 'CommentedOn', index: 'CommentedOn', },
                          { name: 'RejectionComments', index: 'RejectionComments' },
                          { name: 'ResolutionComments', index: 'ResolutionComments', hidden: true },
                          { name: 'Note', index: 'Note', hidden: true }
                          ],
            rowNum: '-1',
            sortname: 'CommentedOn',
            sortorder: 'desc',
            viewrecords: true,
            loadComplete: function (data) {
                    var table = this;
                    setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(window).triggerHandler('resize.jqGrid');
            },
            caption:"<i class='ace-icon fa fa-bars'></i>&nbsp;&nbsp;Comment List"
        });
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
        jQuery.extend(jQuery.jgrid.edit, {
            ajaxEditOptions: { contentType: "application/json" },
            recreateForm: true,
            serializeEditData: function (postData) {
                //
                if (postData.Note === undefined) { postData.Note = null; }
                if (postData.TicketId === undefined || postData.TicketId == 0) { postData.TicketId = $('#Id').val(); }
                return JSON.stringify(postData);
            }
        });


        //note list Grid
        $(grid_selector1).jqGrid({
            mtype: 'GET',
            url: "/TicketingSystem/GetTicketNoteDtlsbyTicketId",
            postData: { TicketId: $('#Id').val() },
            datatype: 'json',
            height: '70',
            autowidth: true,
            shrinkToFit: true,
            colNames: ['Id', 'TicketId', 'Note Added By', 'Note Date', 'RejectionComments', 'ResolutionComments', 'Note Details', ''],
            colModel: [
                        { name: 'Id', index: 'Id', hidden: true, editable: false, key: true },
                        { name: 'TicketId', index: 'TicketId', hidden: true, editable: true },
                        { name: 'CommentedBy', index: 'CommentedBy', editable: true, editrules: { edithidden: true, required: true }, editoptions: { readonly: true} },
                        { name: 'CommentedOn', index: 'CommentedOn'},
                        { name: 'RejectionComments', index: 'RejectionComments', hidden: true },
                        { name: 'ResolutionComments', index: 'ResolutionComments', hidden: true },
                        { name: 'Note', index: 'Note', editable: true, edittype: 'textarea', editrules: { required: true }, editoptions: { size: 70, maxlength: 500, rows: "5", cols: "40"} },
                        { name: 'DelNote', index: 'DelNote', align: 'center', sortable: false, formatter: DelBtnFrmtr}],
            pager: pager_selector1,
            rowNum: '-1',
            sortname: 'CommentedOn',
            sortorder: 'desc',
            viewrecords: true,
            loadComplete: function (data) {
                var table = this;
                setTimeout(function () {
                    updatePagerIcons(table);
                    enableTooltips(table);
                }, 0);
                $(window).triggerHandler('resize.jqGrid');
            },
            caption:"<i class='ace-icon fa fa-file-text-o'></i>&nbsp;&nbsp;Note List",
            gridComplete: function () {
                var rds = $(grid_selector1).getRowData();
                if (rds.length > 0) {
                    $('.bdelNt').button({ icons: { primary: "ui-icon-trash" }, text: false })
                           .click(function () { DeleteNotes($(this).attr('rowid'), 'Note'); });
                }
            },
            editurl: "/TicketingSystem/SaveTicketComments"
        });

        function DelBtnFrmtr(cellvalue, options, rowdata) {
            var NoteAddedBy;
            if ($.isArray(rowdata)) { NoteAddedBy = rowdata[2]; } else { NoteAddedBy = rowdata.Task; }
            if (NoteAddedBy && NoteAddedBy.toUpperCase() == $('#loggedInUserId').val().toUpperCase()) {
                return "<button id='btnDelNote_" + options.rowId + "'class='bdelNt' rowid='" + options.rowId + "' title='Delete Note' type='button'> Del Note </button>";
            } else { return ""; }
        }

        function DeleteNotes(NoteId, CmntsType) {//CmntsTyoe-Comments or Notes
            //
            $.ajax({
                url: '/TicketingSystem/DeleteComments?NoteId=' + NoteId,
                type: 'GET',
                dataType: 'json',
                traditional: true,
                success: function (data) {
                    //
                    if (CmntsType == "Comments") {
                        LoadSetGridParam($('#CommentList'), "/TicketingSystem/GetTicketCommentDtlsbyTicketId?TicketId=" + $('#Id').val());
                        InfoMsg("Comments deleted successfully.");
                    }
                    else {
                        LoadSetGridParam($(grid_selector), "/TicketingSystem/GetTicketNoteDtlsbyTicketId?TicketId=" + $('#Id').val());
                        InfoMsg("Note deleted successfully.");
                    }
                },
                error: function (xhr, status, error) {
                    ErrMsg($.parseJSON(xhr.responseText).Message);
                }
            });
        }
        $(window).triggerHandler('resize.jqGrid');
        //navButtons Add, edit, delete
        jQuery(grid_selector1).jqGrid('navGrid', pager_selector1,
            { 	//navbar options
                edit: false,
                editicon: 'ace-icon fa fa-pencil blue',
                add: true,
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
            {
            // add options
                    width:395, jqModal: true, closeAfterAdd: true, addCaption: 'Add Note',
                    beforeShowForm: function (frm) {
                    },
                    afterShowForm: function (frm) {
                        $('#TicketId').val($('#Id').val());
                        $('#CommentedBy').val($('#loggedInUserId').val());
                        $('#Note').focus();
                    }
                    
            
            }, {}, {})

        //replace icons with FontAwesome icons like above
        $('#btnTicketStatus').click(function () {
            $.ajax({
                url: '/TicketingSystem/UpdateTicketStatus?TicketId=' + $('#Id').val() + '&TicketStatus=' + $('#TicketStatus').val(),
                type: 'GET',
                dataType: 'json',
                traditional: true,
                success: function (data) {
                    InfoMsg("Ticket Status changed successfully.");
                },
                error: function (xhr, status, error) {
                    ErrMsg($.parseJSON(xhr.responseText).Message);
                }
            });
        });

        $('#search_NoteList').hide();

        if ($('#Id').val() > 0) {
            $('#add_NoteList').show();
        } else {
            $('#add_NoteList').hide();
            $('#docPanel').hide();
            $('#InstanceId').val(0);
        }
    });

