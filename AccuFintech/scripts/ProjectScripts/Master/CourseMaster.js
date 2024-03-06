$(document).ready(function () {
    $('#TBL_CourseDetails').dataTable({});
    $('#opsection').val('INSERT');
});

function GetCourseDetails(CourseID) {
    $.ajax({
        url: '/CourseMaster/GetCourseByID?CourseID=' + CourseID,
        type: "GET",
        data: '{}',
        contentType: 'application/json; charset=utf-8',
        success: (data) => {
            console.log(data);
            $('#Programcode').val(data.Programcode);
            $('#Programcode').prop('readonly', true);

            $('#Coursename').val(data.Coursename);
            $('#MonthDuration').val(data.MonthDuration);
            $('#HourDuration').val(data.HourDuration);
            $('#Eligibility').val(data.Eligibility);
            $('#CourseModule').val(data.CourseModule);
            $('#CareerOportunities').val(data.CareerOportunities);
            $('#opsection').val('UPDATE');
        },
        error: function () {
            alert("error");
        }
    });
}

function RemoveCourseDetails(Programcode) {
    if (confirm('Are you sure? you want to remove this course.')) {
        var obj = {
            Programcode: Programcode
        };

        $.ajax({
            url: '/CourseMaster/RemoveCourse',
            type: "POST",
            data: JSON.stringify(obj),
            contentType: 'application/json; charset=utf-8',
            success: (data) => {
                console.log(data);
                window.location.reload();
            },
            error: function () {
                alert("error");
            }
        });
    } else {
        return false;
    }
}




$(document).on('change', '#Programcode', function () {
    UserCheck();
});


function UserCheck() {
    $("#Save-bt").prop("disabled", true);
    if ($("#Programcode").val() != '') {
        $("#Status").text("Checking...");
        $.ajax({
            url: "/CourseMaster/CheckProgramCode",
            method: "GET",
            data: "pgcode=" + $("#Programcode").val(),
            success: function (data) {
                if (data == "False") {
                    $("#Status").text('Available');
                    $("#Status").css("color", "Green");
                    $("#Save-bt").prop("disabled", false);
                }
                else {
                    $("#Status").text('ProgramCode Already Register');
                    $("#Status").css("color", "Red");
                    $("#Save-bt").prop("disabled", true);
                }
            },
            error: function (err) {
                alert(err);
            }
        });
    } else {
        $("#Status").text('');
    }
}







