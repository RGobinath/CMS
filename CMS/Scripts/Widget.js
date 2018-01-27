$(function () {

    /*Admission Management*/
    $("#ADM").hover(function () {
        $("#ADMClose", $(this)).stop(true).fadeIn();
    });

    $("#ADMClose", $(this)).stop(true).fadeOut();
    $("#ADMClose").click(function () {
        $("#ADM").hide();
    });

    /*Issue Management*/
    $("#ISM").hover(function () {
        $("#ISMClose", $(this)).stop(true).fadeIn();
    });

    $("#ISMClose", $(this)).stop(true).fadeOut();
    $("#ISMClose").click(function () {
        $("#ISM").hide();
    });


    /*360 Management*/
    $("#ASS").hover(function () {
        $("#360Close", $(this)).stop(true).fadeIn();
    });

    $("#ASSClose", $(this)).stop(true).fadeOut();
    $("#ASSClose").click(function () {
        $("#ASS").hide();
    });


    /*Notification Management*/
    $("#NOT").hover(function () {
        $("#NOTClose", $(this)).stop(true).fadeIn();
    });

    $("#NOTClose", $(this)).stop(true).fadeOut();
    $("#NOTClose").click(function () {
        $("#NOT").hide();
    });


    /*Store Management*/
    $("#STR").hover(function () {
        $("#STRClose", $(this)).stop(true).fadeIn();
    });

    $("#STRClose", $(this)).stop(true).fadeOut();
    $("#STRClose").click(function () {
        $("#STR").hide();
    });


    /*Staff Management*/
    $("#STM").hover(function () {
        $("#STMClose", $(this)).stop(true).fadeIn();
    });

    $("#STMClose", $(this)).stop(true).fadeOut();
    $("#STMClose").click(function () {
        $("#STM").hide();
    });


    /* Attendance */
    $("#ATT").hover(function () {
        $("#ATTClose", $(this)).stop(true).fadeIn();
    });

    $("#ATTClose", $(this)).stop(true).fadeOut();
    $("#ATTClose").click(function () {
        $("#ATT").hide();
    });


    /* Transport */
    $("#TRA").hover(function () {
        $("#TRAClose", $(this)).stop(true).fadeIn();
    });

    $("#TRAClose", $(this)).stop(true).fadeOut();
    $("#TRAClose").click(function () {
        $("#TRA").hide();
    });



    /*Individual Notification Management*/
    $("#INNOT").hover(function () {
        $("#INNOTClose", $(this)).stop(true).fadeIn();
    });

    $("#INNOTClose", $(this)).stop(true).fadeOut();
    $("#INNOTClose").click(function () {
        $("#INNOT").hide();
    });


    /* Outward Store Management */
    $("#OUTSTR").hover(function () {
        $("#OUTSTRClose", $(this)).stop(true).fadeIn();
    });

    $("#OUTSTRClose", $(this)).stop(true).fadeOut();
    $("#OUTSTRClose").click(function () {
        $("#OUTSTR").hide();
    });


    $("#ADM").hover(function () {
        $("#close", $(this)).stop(true).fadeIn();
    });

    $("#close", $(this)).stop(true).fadeOut();
    $("#close").click(function () {
        alert();
        $("#ADM").hide();
    });




});