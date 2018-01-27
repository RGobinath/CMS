jQuery(function ($) {
    $("#Approve").click(function () {

        var feestruct = document.getElementById("feestructddl").value;

        var status = document.getElementById("Status").value;

        var asignsection = document.getElementById("AssignSection").value;

        var grade = document.getElementById("grade").value;
        var nam = document.getElementById("nam").value;

        if ((status == 'Registered')) {
            if (status == "") {
                ErrMsg('Please Select Status!');
                return false;
            }
            if (feestruct == "") {
                ErrMsg('Please Select Fee Structure Year!');
                return false;
            }
            if (asignsection == "") {
                ErrMsg('Please Select Section!');
                return false;
            }
        }
        if ((status == '')) {
            if (status == "") {
                ErrMsg('Please Select Status!');
                return false;
            }
            if (feestruct == "") {
                ErrMsg('Please Select Fee Structure Year!');
                return false;
            }
            if (asignsection == "") {
                ErrMsg('Please Select Section!');
                return false;
            }
        }
        else {
            return true;
        }
    });

    $("#Status").change(function () {
        if (document.getElementById("Status").value != "Registered") {

            if ((document.getElementById("Status").value == "Inactive") || (document.getElementById("Status").value == "")) {
            }
            else {
                var e = document.getElementById('feestructddl');
                e.options[0].selected = true; // "Select";
                var f = document.getElementById('AssignSection');
                f.options[0].selected = true; // "Select";
                document.getElementById("studentcnt").innerHTML = 0;
            }
            document.getElementById("feestructddl").disabled = true;
            document.getElementById("AssignSection").disabled = true;
        }
        else {
            if ((document.getElementById('feestructddl').value == "") && (document.getElementById('AssignSection').value == "")) {
                document.getElementById("feestructddl").disabled = false;
                document.getElementById("AssignSection").disabled = false;
            }
            else {
                document.getElementById("feestructddl").disabled = true;
                document.getElementById("AssignSection").disabled = true;
            }
        }
    });



});