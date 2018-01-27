jQuery(function ($) {
    
    window.onload = DisableTextbox();
    //$("#EduGoalYes").attr('checked', true).trigger('click');
    if ($('#EduGoalYes').is(':checked')) {
        EnableTextbox();
    }
});
function EnableTextbox() {
    $("#EduGoal").attr("readonly", false);
    //document.getElementById("EduGoal").disabled = false;
}
function DisableTextbox() {
    $("#EduGoal").attr("readonly", true);
    //document.getElementById("EduGoal").readonly = true;
}