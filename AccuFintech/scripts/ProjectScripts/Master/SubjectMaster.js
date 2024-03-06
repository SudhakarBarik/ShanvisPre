$(document).ready(function () {
    $('#CourseID').chosen();
});

$(document).on('click', '#btnAddSubject', function () {
    var CourseID = $('#CourseID option:selected').val();
    var Coursename = $('#CourseID option:selected').text();
    var Subject = $('#Subject').val();
    var FullMarks = $('#FullMarks').val();
    var PassMarks = $('#PassMarks').val();

    if (CourseID == "") {
        alert('Please Select a Course');
        return false;
    }
    if (Subject == "") {
        alert('Please Enter subject');
        return false;
    }
    if (FullMarks == "") {
        alert('Please Enter Full Marks');
        return false;
    }
    if (PassMarks == "") {
        alert('Please Enter Pass Marks');
        return false;
    }

    var TbodyHtml = ``;
    TbodyHtml += `<tr><td style="display:none;">${CourseID}</td><td>${Coursename}</td><td>${Subject}</td><td>${FullMarks}</td><td>${PassMarks}</td><td><a href="#" id="RemoveRow" class="text-danger"><i class="fa fa-trash"></i></a></td></tr>`;
    $('#Tbody_SubjectList').append(TbodyHtml);

    $('#Subject').val('');
    $('#FullMarks').val('');
    $('#PassMarks').val('');
    $("#CourseID_chosen").css('pointer-events', 'none');

    GenerateJson();
});

$('#Tbody_SubjectList').on('click', '#RemoveRow', function () {
    if(confirm('Are you sure? you want to remove this row.')){
        var Currentrow = $(this).closest("tr");
        Currentrow.remove();

        GenerateJson();
    } else {
        return false;
    }
});

function GenerateJson() {
    var tableObj = [];
    var j = 0
    var head = ["CourseID", "Course", "Subject", "FullMarks", "PassMarks"];
    $.each($("#Tbody_SubjectList tr"), function () {
        var $row = $(this),
        rowObj = {};
        i = 0;
        $.each($("td", $row), function () {
            var $col = $(this);
            if (head[i] != undefined) {
                rowObj[head[i]] = $col.text().trim();
            }
            i++;
        });
        tableObj.push(rowObj);
        j++;
    });
    $('#SubjectString').val(JSON.stringify(tableObj));
}

$(document).on('change', '#CourseID', function () {
    var CourseID = $('#CourseID option:selected').val();
    if (CourseID != "") {
        $.ajax({
            url: '/SubjectMaster/GetAllSubjectByCourse?CourseID=' + CourseID,
            type: "GET",
            data: '{}',
            contentType: 'application/json; charset=utf-8',
            success: (data) => {
                console.log(data);
                $('#Tbody_SubjectList').html('');
                var TbodyHtml = ``;
                data.forEach((item) => {
                    TbodyHtml += `<tr><td style="display:none;">${item.CourseID}</td><td>${item.Course}</td><td>${item.Subject}</td><td>${item.FullMarks}</td><td>${item.PassMarks}</td><td><a href="#" id="RemoveRow" class="text-danger"><i class="fa fa-trash"></i></a></td></tr>`;
                });
                $('#Tbody_SubjectList').append(TbodyHtml);
            },
            error: function () {
                alert("error");
            }
        });
    }
});