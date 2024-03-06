var TotalMarksObtained = 0;
$(document).ready(function () {
    $('#StudentID').chosen();
});

$(document).on('click', '#btnStudentSearch', function () {
    var StudentID = $('#StudentID option:selected').val();
    $.ajax({
        url: '/MarksEntry/GetStudentDetails?StudentID=' + StudentID,
        type: "GET",
        data: '{}',
        contentType: 'application/json; charset=utf-8',
        success: (data) => {
            console.log(data);
            $('#StudentIDView').val(data.StudentIDView);
            $('#Studentname').val(data.Studentname);
            $('#Gurdianname').val(data.Gurdianname);
            $('#DOJ').val(data.DOJ);
            $('#ExamRegdate').val(data.ExamRegdate);
            $('#Examdate').val(data.Examdate);
            $('#Course').val(data.Course);
            $('#Franchaisecode').val(data.Franchaisecode);
            $('#Franchaise').val(data.Franchaise);
            $('#CourseID').val(data.CourseID);

            GetMarksDetails(data.CourseID, data.StudentID);
        },
        error: function () {
            alert("error");
        }
    });
});

function GetMarksDetails(Course, StudentID) {
    $.ajax({
        url: '/MarksEntry/GetAllSubject?CourseID=' + Course + '&StudentID=' + StudentID,
        type: "GET",
        data: '{}',
        contentType: 'application/json; charset=utf-8',
        success: (data) => {
            console.log(data);
            var SubjectHTML = '';
            var i = 0;
            var FullMarks = 0;
            data.forEach((item) => {
                SubjectHTML += `<tr><td style="display:none;">${item.SubjectID}</td><td class="font-weight-bold">${item.Subject}</td><td class="font-weight-bold bg-light">${item.FullMarks}</td><td class="font-weight-bold bg-light">${item.PassMarks}</td><td><input type="text" class="form-control" onblur="CalculateTotalMarks(${i})" id="TheoryMarks_${i}" Value="${item.Theory}" /></td><td><input type="text" onblur="CalculateTotalMarks(${i})" class="form-control" id="PracticalMarks_${i}" Value="${item.Practical}" /></td><td><input type="text" class="form-control" id="MarksObtained_${i}" Value="${item.MarksObtained}" readonly /></td></tr>`;
                FullMarks += parseInt(item.FullMarks);
                i++;
            });
            $('#TotalFullMarks').text(FullMarks);
            $('#TBL_MarksEntrylist').html(SubjectHTML);

            GetTotalMarks();
        },
        error: function () {
            alert("error");
        }
    });
}

function CalculateTotalMarks(i) {
    var TheoryMarks = parseFloat($('#TheoryMarks_' + i).val());
    var PracticalMarks = isNaN(parseFloat($('#PracticalMarks_' + i).val())) == true ? 0 : parseFloat($('#PracticalMarks_' + i).val());
    var MarksObtained = TheoryMarks + PracticalMarks;
    $('#MarksObtained_' + i).val(MarksObtained);

    GetTotalMarks();

    var tableobj = [];
    $('#TBL_MarksEntrylist tr').each(function () {
        if ($(this).find('td:eq(4)').find('input').val() != "" && $(this).find('td:eq(5)').find('input').val() != "" && $(this).find('td:eq(6)').find('input').val() != "") {
            var obj = {
                StudentID: $('#StudentIDView').val(),
                SubjectID: $(this).find('td:eq(0)').text(),
                CourseID: $('#CourseID').val(),
                FullMarks: $(this).find('td:eq(2)').text(),
                PassMarks: $(this).find('td:eq(3)').text(),
                Theory: $(this).find('td:eq(4)').find('input').val(),
                Practical: $(this).find('td:eq(5)').find('input').val(),
                MarksObtained: $(this).find('td:eq(6)').find('input').val(),
            };
            tableobj.push(obj);
        }
    });
    $('#StudentMarksJson').val(JSON.stringify(tableobj));
}

function GetTotalMarks() {
    var TotalMarksObtained = 0;
    $('#TBL_MarksEntrylist tr').each(function () {
        TotalMarksObtained += isNaN(parseFloat($(this).find('td:eq(6)').find('input').val())) == true ? 0 : parseFloat($(this).find('td:eq(6)').find('input').val());
    });
    $('#TotalMarksObtained').text(TotalMarksObtained);
}