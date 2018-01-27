jQuery(function ($) {
    var date = new Date();
    var currentMonth = date.getMonth();
    var currentDate = date.getDate();
    var currentYear = date.getFullYear();

    $('#DOB').datepicker({
        format: 'dd/mm/yyyy',
        autoClose: true
    }).on('changeDate', function (selected) {
        var selDate = new Date(selected.date.valueOf());
        var today = new Date();
        years = Math.floor((today.getTime() - selDate.getTime()) / (365.25 * 24 * 60 * 60 * 1000));
        $("#age").val(years);
    });
    $('#DOB').blur(function () {
        getAge(document.getElementById('DOB').value);
    });
    function getAge(dateString) {
        var today = new Date();
        var bD = dateString.split('/');
        if (bD.length == 3) {
            born = new Date(bD[2], bD[1] * 1 - 1, bD[0]);
            years = Math.floor((today.getTime() - born.getTime()) / (365.25 * 24 * 60 * 60 * 1000));
            $("#age").val(years);
        }
    }
});
