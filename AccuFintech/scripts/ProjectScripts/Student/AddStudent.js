$(document).ready(function () {
    $("#DOB").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "1970:" + parseInt(new Date().getFullYear() + 50) + '"' });
    $("#Joinindate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "1970:" + parseInt(new Date().getFullYear() + 50) + '"' });
    $('#Password').css('pointer-events', 'none');
    $('#Age').css('pointer-events', 'none');
    $('#Course').chosen();

    $("#inpStudent").change(function () { readStudentURL(this); });
    $("#inpIdProof").change(function () { readStudentIDURL(this); });
    $("#inpsignProof").change(function () { readStdSignUrl(this); });
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
function readStdSignUrl(input) {
    var re = /(\.jpg|\.jpeg|\.png)$/i;
    if (!re.exec(input.files[0].name)) {
        alert("Signuture Image must be JPG OR JPEG OR PNG");
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

$(document).on('change', '#Joinindate', function () {
    GetCourseDetails();
});

$(document).on('change', '#Course', function () {
    GetCourseDetails();
    GetBatchList();
});

function GetCourseDetails() {
    var Coursecode = $('#Course option:selected').val();
    var Joinindate = $('#Joinindate').val();

    if (Coursecode != "" && Joinindate != "") {
        $.ajax({
            type: 'GET',
            url: "/StudentRegistration/GetCourseDetails?CourseID=" + Coursecode + "&CourseStartdate=" + Joinindate,
            data: {},
            success: function (response) {
                console.log(response);
                $('#CourseCode').val(response.Coursecode);
                $('#CourseEnddate').val(response.CourseEnddate);
                $('#CFees').val(response.Fees);
            }
        });
    }
}

$(document).on('change', '#Batch', function () {
    GetBatchDetails();
});

function GetBatchDetails() {
    var BatchID = $('#Batch option:selected').val();
    if (BatchID != "") {
        $.ajax({
            type: "GET",
            url: "/BatchMaster/GetBatchDetailsById?BatchID=" + BatchID,
            data: {},
            success: function (Result) {
                console.log(Result);
                $('#BStartDate').val(Result.BStartDate);
                $('#BEndDate').val(Result.BEndDate);
            }
        });
    }

}

function GetBatchList() {
    var Coursecode = $('#Course option:selected').val();
    if (Coursecode != "") {
        $.ajax({
            url: "/StudentRegistration/GetBatchCourseWise?CourseID=" + Coursecode,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            datatype: JSON,
            success: function (result) {
                $("#Batch").empty();
                $("#Batch").append('<option value="">--Select Batch--</option>');
                $(result).each(function () {
                    $("#Batch").append($("<option></option>").val(this.Key).html(this.Value));
                });
            },
            error: function (data) {
                $("#Batch").empty();
                $("#Batch").append('<option value="">--Select Batch--</option>');
            }
        });
    }
}