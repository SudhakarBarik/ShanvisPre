$(document).ready(function () {
    $("#DOB").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "1970:" + parseInt(new Date().getFullYear() + 50) + '"' });
    $("#Joinindate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "1970:" + parseInt(new Date().getFullYear() + 50) + '"' });
    $('#Password').css('pointer-events', 'none');
    $('#Age').css('pointer-events', 'none');

    $("#inpStudent").change(function () { readStudentURL(this); });
    $("#inpIdProof").change(function () { readStudentIDURL(this); });
    $("#intSignPic").change(function () { readStudentSignURL(this); });
});

function readStudentIDURL(input) {
    var re = /(\.jpg|\.jpeg|\.png)$/i;
    if (!re.exec(input.files[0].name)) {
        alert("Profile Image must be JPG OR JPEG OR PNG");
    }
    else {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#ptbIdProof').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
}

function readStudentURL(input) {
    var re = /(\.jpg|\.jpeg|\.png)$/i;
    if (!re.exec(input.files[0].name)) {
        alert("Profile Image must be JPG OR JPEG OR PNG");
    }
    else {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#ptbStudent').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
}


function readStudentSignURL(input) {
    var re = /(\.jpg|\.jpeg|\.png)$/i;
    if (!re.exec(input.files[0].name)) {
        alert("Signature Image must be JPG OR JPEG OR PNG");
    }
    else {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#ptbsignProof').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
}

$("#DOB").change(() => {
    calculateAge();
});
let calculateAge = () => {
    let dob = $("#DOB").val();
    var newDob = dob.split("/").reverse().join("-");

    let Age = new Date().getFullYear() - new Date(newDob).getFullYear();
    $('#Age').val(Age);
}

$('#Phone').focusout(() => {
    GeneratePassword();
});

$('#DOB').change(function () {
    GeneratePassword();
});

function GeneratePassword() {
    var Phone = $('#Phone').val();
    var DOB = $('#DOB').val();
    var GeneratedPwd = "";

    if (Studentname != "" && DOB != "") {
        var GeneratedDOB = DOB.split('/');
        GeneratedPwd = GeneratedDOB[2] + GeneratedDOB[1] + GeneratedDOB[0] + Phone.substr(Phone.length - 4);
        $('#Password').val(GeneratedPwd);
    }
}