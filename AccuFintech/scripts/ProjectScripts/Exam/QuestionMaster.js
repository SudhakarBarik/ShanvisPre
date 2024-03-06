$(document).ready(function () {
    $('#opsection').val('INSERT');
});

function GetRightAns() {
    var answer = $('input[name="anslist[]"]:checked').val();
    $('#RightAns').val(answer);
}

$(document).on('change', '#QSet', function () {
    var QSet = $('#QSet option:selected').val();

    $.ajax({
        type: "GET",
        url: '/QuestionMaster/GetquestionLists?Qset=' + QSet,
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response);
            OnSuccess(response);
        },
        failure: function (response) {
            console.log('Failure : ' + response);
        },
        error: function (response) {
            console.log('Error : ' + response);
        }
    });
});

function OnSuccess(Jdata) {

    var table;

    table = $("#TBL_QuestionList").DataTable(
    {
        "bDestroy": true,
        processing: true,
        bLengthChange: true,
        lengthMenu: [[10, 50, 100, -1], [10, 50, 100, "All"]],
        bFilter: true,
        bSort: false,
        bPaginate: true,
        buttons: ["copy", "csv", "excel", "pdf", "print", "colvis"],
        data: Jdata,
        columns: [{ 'data': 'Question' },
                  { 'data': 'Answer1' },
                  { 'data': 'Answer2' },
                  { 'data': 'Answer3' },
                  { 'data': 'Answer4' },
                  { 'data': 'RightAnswer' },
                  {
                      "data": null, title: 'Action', wrap: true, "render": function (item) {
                          return `<a href="#" class="font-weight-bold text-primary" onclick="GetQuestionDetails('${item.QID}')">Edit</a> | <a href="#" class="font-weight-bold text-danger" onclick="RemoveQuestion('${item.QID}')">Remove</a>`
                      }
                  }
        ]
    });
}

function GetQuestionDetails(QID) {
    $.ajax({
        type: "GET",
        url: '/QuestionMaster/GetQuestion?QID=' + QID,
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response);
            $('#Qid').val(response.Qid);
            $('#Question').val(response.Question);
            $('#Ans1').val(response.Ans1);
            $('#Ans2').val(response.Ans2);
            $('#Ans3').val(response.Ans3);
            $('#Ans4').val(response.Ans4);
            if (response.RightAns == "A") {
                $('#A_ID').attr('checked', true);
            } else if (response.RightAns == "B") {
                $('#B_ID').attr('checked', true);
            } else if (response.RightAns == "C") {
                $('#C_ID').attr('checked', true);
            } else {
                $('#D_ID').attr('checked', true);
            }
            $('#opsection').val('UPDATE');
        },
        failure: function (response) {
            console.log('Failure : ' + response);
        },
        error: function (response) {
            console.log('Error : ' + response);
        }
    });
}

function RemoveQuestion(QID) {
    if (confirm('Are you sure? you want to remove this Question.')) {
        var obj = {
            QID: QID
        };

        $.ajax({
        type: "POST",
        url: '/QuestionMaster/RemoveQuestion',
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response);
            window.location.reload();
        },
        failure: function (response) {
            console.log('Failure : ' + response);
        },
        error: function (response) {
            console.log('Error : ' + response);
        }
    });
    } else {
        return false;
    }
}