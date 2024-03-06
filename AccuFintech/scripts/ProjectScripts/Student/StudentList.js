$(document).ready(function () {
    $('#CourseID').chosen();
});

$(document).on('change', '#CourseID', function () {
    var CourseID = $('#CourseID option:selected').val();
    $.ajax({
        url: '/StudentRegistration/GetStudentCourseWise?CourseID=' + CourseID,
        type: "GET",
        data: '{}',
        contentType: 'application/json; charset=utf-8',
        success: (data) => {
            console.log(data);
            OnSuccess(data);
        },
        error: function () {
            alert("error");
        }
    });
});


function OnSuccess(Jdata) {
    var table;
    table = $("#TBL_StudentList").DataTable(
    {
        "bDestroy": true,
        processing: true,
        bLengthChange: true,
        lengthMenu: [[10, 50, 100, -1], [10, 50, 100, "All"]],
        bFilter: true,
        bSort: true,
        bPaginate: true,
        dom: 'lBfrtip',
        data: Jdata,
        columns: [
                  { 'data': 'JoiningDate' },
                  { 'data': 'StudentID' },
                  { 'data': 'Studentname' },
                  { 'data': 'Gurdian' },
                  { 'data': 'Phone' },
                  { 'data': 'Course' },
                  { 'data': 'ExamRegDate' },
                  { 'data': 'Examdate' },
                  { 'data': 'Franchaise' },
                  {
                      "data": null, title: 'Action', wrap: true, "render": function (item) {
                          return `<a href="/StudentRegistration/Edit?StudentID=${item.StudentID}" class="text-primary font-weight-bold">Edit<a/> | <a href="#" class="text-danger font-weight-bold" onClick="RemoveStudent('${item.StudentID}')">Remove<a/>`
                      }
                  }
        ]
    });
}

function RemoveStudent(StudentID) {
    if(confirm('Are you sure? you want to remove this student.')){
        var obj = {
            StudentID: StudentID
        };

        $.ajax({
            url: '/StudentRegistration/RemoveStudent',
            type: "POST",
            data: JSON.stringify(obj),
            contentType: 'application/json; charset=utf-8',
            success: (data) => {
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