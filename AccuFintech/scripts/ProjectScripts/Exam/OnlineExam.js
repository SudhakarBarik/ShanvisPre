var myTimer;
var QuestionSetArr = [];
var GlobalI = 0;

$(document).on('fullscreenchange', function (e) {
    if (document.fullscreenElement) {
        // Entered fullscreen
        console.log('Entered fullscreen');
    } else {
        $('#StartExamScreen').show();
        $('#ExamPortal').hide();
        //$('#OnlineExamResultModal').modal('show');
        //$('#btnSubmitexam').click();
        // Left fullscreen; run your code here
        console.log('Left fullscreen');
    }
});

$(document).on('webkitfullscreenchange', function (e) {
    if (document.fullscreenElement) {
        // Entered fullscreen
        console.log('Entered fullscreen');
    } else {
        $('#StartExamScreen').show();
        $('#ExamPortal').hide();
        //$('#OnlineExamResultModal').modal('show');
        //$('#btnSubmitexam').click();
        // Left fullscreen; run your code here
        console.log('Left fullscreen');
    }
});

$(document).on('mozfullscreenchange', function (e) {
    if (document.fullscreenElement) {
        // Entered fullscreen
        console.log('Entered fullscreen');
    } else {
        $('#StartExamScreen').show();
        $('#ExamPortal').hide();
        //$('#OnlineExamResultModal').modal('show');
        //$('#btnSubmitexam').click();
        // Left fullscreen; run your code here
        console.log('Left fullscreen');
    }
});

$(document).ready(function () {
    $('#TotalSeconds').val('1800');
    GetStudentDetails();
    //checkconnection();
});

function checkconnection() {
    var status = navigator.onLine;
    if (status) {
        alert('Internet connected !!');
    } else {
        alert('No internet Connection !!');
    }
}

$(document).keydown(function (e) {
    if (e.keyCode == '91') {
        //$('#OnlineExamResultModal').modal('show');
        //$('#btnSubmitexam').click();
    }
    e.preventDefault();
});

function CountdownTimer() {
    var totalSeconds = parseInt($('#TotalSeconds').val());

    myTimer = setInterval(myClock, 1000);

    function myClock() {
        var hours = Math.floor(totalSeconds / 3600);
        var minutes = Math.floor((totalSeconds % 3600) / 60);
        var seconds = totalSeconds % 60;

        document.getElementById("TimeReamainingTimer").innerHTML =
          leftPad(hours,2) + " : " + leftPad(minutes, 2) + " : " + leftPad(seconds, 2);

        totalSeconds--;
        $('#TotalSeconds').val(totalSeconds);
        if (totalSeconds < 0) {
            clearInterval(myTimer);
            GetExamTime();
            $('#OnlineExamResultModal').modal('show');
        }
    }
}

function leftPad(value, length) {
    return ('0'.repeat(length) + value).slice(-length);
}

function GetStudentDetails() {
    $.ajax({
        type: "GET",
        url: '/OnlineExam/GetStudentdetails',
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#StudentID').val(response.StudentID);
            $('#Studentname').val(response.Studentname);
            $('#QuestionSet').val(response.QSet);
            $('#StuExamdate').val(response.StuExamdate);

            GetAllQuestions(response.QSet);
        },
        failure: function (response) {
            console.log('Failure : ' + response);
        },
        error: function (response) {
            console.log('Error : ' + response);
        }
    });
}

function GetAllQuestions(Qset) {
    $.ajax({
        type: "GET",
        url: '/OnlineExam/GetAllQuestionLists?Qset=' + Qset,
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#QuestionnoList').html('');
            var Questionnos = "";
            var i = 1;
            response.forEach((item) => {
                if (item.IsCompleted == "0") {
                    Questionnos += `<div class="col-md-4 col-12 mt-2 p-1">
                                    <button type="button" onclick="GetQuestionByQid('${item.QuestionID}', '${i}')" id="btnQuestionGet_${i}" class ="btn btn-light btn-sm w-100">${i}</button>
                                </div>`;
                } else if (item.IsCompleted == "1") {
                    Questionnos += `<div class="col-md-4 col-12 mt-2 p-1">
                                    <button type="button" onclick="GetQuestionByQid('${item.QuestionID}', '${i}')" id="btnQuestionGet_${i}" class ="btn btn-success btn-sm w-100">${i}</button>
                                </div>`;
                } else if (item.IsCompleted == "2") {
                    Questionnos += `<div class="col-md-4 col-12 mt-2 p-1">
                                    <button type="button" onclick="GetQuestionByQid('${item.QuestionID}', '${i}')" id="btnQuestionGet_${i}" class ="btn btn-warning btn-sm w-100">${i}</button>
                                </div>`;
                }
               
                QuestionSetArr.push(item);
                i++;
            });
            $('#QuestionnoList').html(Questionnos);

            $('#QuestionID').val(response[0]["QuestionID"]);
            $('#Question').html(response[0]["Question"]);
            $('#Answer1').html(response[0]["Ans1"]);
            $('#Answer2').html(response[0]["Ans2"]);
            $('#Answer3').html(response[0]["Ans3"]);
            $('#Answer4').html(response[0]["Ans4"]);

            if (QuestionSetArr[0]["AnsOption"] == "A") {
                $("#ANS_A").prop("checked", true);
            } else if (QuestionSetArr[0]["AnsOption"] == "B") {
                $("#ANS_B").prop("checked", true);
            } else if (QuestionSetArr[0]["AnsOption"] == "C") {
                $("#ANS_C").prop("checked", true);
            } else if (QuestionSetArr[0]["AnsOption"] == "D") {
                $("#ANS_D").prop("checked", true);
            } else {
                $('input[name="ans[]"]').prop('checked', false);
            }

            GlobalI = 1;


            GetAnsweredQuestions();
        },
        failure: function (response) {
            console.log('Failure : ' + response);
        },
        error: function (response) {
            console.log('Error : ' + response);
        }
    });
}

function GetQuestionByQid(Qid, i) {
    var QSet = $('#QuestionSet').val();
    $.ajax({
        type: "GET",
        url: '/OnlineExam/GetQuestionByQid?Qid=' + Qid + '&QSet=' + QSet,
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#qtnno').html(i);
            $('#QuestionID').val(response["QuestionID"]);
            $('#Question').html(response["Question"]);
            $('#Answer1').html(response["Ans1"]);
            $('#Answer2').html(response["Ans2"]);
            $('#Answer3').html(response["Ans3"]);
            $('#Answer4').html(response["Ans4"]);

            if (response["AnsOption"] == "A") {
                $("#ANS_A").prop("checked", true);
            } else if (response["AnsOption"] == "B") {
                $("#ANS_B").prop("checked", true);
            } else if (response["AnsOption"] == "C") {
                $("#ANS_C").prop("checked", true);
            } else if (response["AnsOption"] == "D") {
                $("#ANS_D").prop("checked", true);
            } else {
                $('input[name="ans[]"]').prop('checked', false);
            }

            GlobalI = i;
        },
        failure: function (response) {
            console.log('Failure : ' + response);
        },
        error: function (response) {
            console.log('Error : ' + response);
        }
    });
}

function openFullScreen() {
    $('#StartExamScreen').hide();
    $('#ExamPortal').show();
    if (!document.fullscreenElement && !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) {  
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen().catch(err => {
                console.log(`Error attempting to enable full-screen mode: ${err.message} (${err.name})`);
            });
        } else if (document.documentElement.msRequestFullscreen) {
            document.documentElement.msRequestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) {
            document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.webkitRequestFullscreen) {
            document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        }
    }   
}

function CloseFullScreen() {
    if (document.exitFullscreen) {
        document.exitFullscreen();
    } else if (document.msExitFullscreen) {
        document.msExitFullscreen();
    } else if (document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
    } else if (document.webkitExitFullscreen) {
        document.webkitExitFullscreen();
    }
}

function GetExamTime() {
    var NowTime = "";
    $.ajax({
            type: "GET",
            url: '/OnlineExam/GetExamTime',
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if($('#EStartTime').val() == ""){
                    $('#EStartTime').val(response);
                } else {
                    $('#EEndtime').val(response);
                }
                
            },
            failure: function (response) {
                console.log('Failure : ' + response);
            },
            error: function (response) {
                console.log('Error : ' + response);
            }
    });
}

$(document).on('click', '#btnMarkAsDone', function () {
    var StudentID = $('#StudentID').val();
    var QuestionSet = $('#QuestionSet').val();
    var QuestionID = $('#QuestionID').val();
    var AnsOption = $('input[name="ans[]"]:checked').val();

    if (StudentID != "" && QuestionSet != "" && QuestionID != "" && AnsOption != undefined) {
        var obj = {
            StudentID: StudentID,
            QuestionSet: QuestionSet,
            QuestionID: QuestionID,
            AnsOption: AnsOption
        };

        $.ajax({
            type: "POST",
            url: '/OnlineExam/StuAnsweredQtn',
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (QuestionSetArr.length != GlobalI) {
                    $('input[name="ans[]"]').prop('checked', false);
                }
                
                if (QuestionSetArr.length > GlobalI) {
                    $('#QuestionID').val(QuestionSetArr[GlobalI]["QuestionID"]);
                    $('#Question').html(QuestionSetArr[GlobalI]["Question"]);
                    $('#Answer1').html(QuestionSetArr[GlobalI]["Ans1"]);
                    $('#Answer2').html(QuestionSetArr[GlobalI]["Ans2"]);
                    $('#Answer3').html(QuestionSetArr[GlobalI]["Ans3"]);
                    $('#Answer4').html(QuestionSetArr[GlobalI]["Ans4"]);
                    if (QuestionSetArr[GlobalI]["AnsOption"] == "A") {
                        $("#ANS_A").prop("checked", true);
                    }else if (QuestionSetArr[GlobalI]["AnsOption"] == "B") {
                        $("#ANS_B").prop("checked", true);
                    }else if (QuestionSetArr[GlobalI]["AnsOption"] == "C") {
                        $("#ANS_C").prop("checked", true);
                    }else if (QuestionSetArr[GlobalI]["AnsOption"] == "D") {
                        $("#ANS_D").prop("checked", true);
                    } else {
                        $('input[name="ans[]"]').prop('checked', false);
                    }
                    $(`#btnQuestionGet_${GlobalI}`).removeClass('btn-light');
                    $(`#btnQuestionGet_${GlobalI}`).removeClass('btn-warning');
                    $(`#btnQuestionGet_${GlobalI}`).addClass('btn-success');
                    GlobalI++;
                    $('#qtnno').html(GlobalI);
                } else {
                    $(`#btnQuestionGet_${GlobalI}`).removeClass('btn-light');
                    $(`#btnQuestionGet_${GlobalI}`).removeClass('btn-warning');
                    $(`#btnQuestionGet_${GlobalI}`).addClass('btn-success');
                }

                GetAnsweredQuestions();
            },
            failure: function (response) {
                console.log('Failure : ' + response);
            },
            error: function (response) {
                console.log('Error : ' + response);
            }
        });
    }

});

function CheckExamDate() {
    var StuExamdate = $('#StuExamdate').val();
    var Questionset = $('#QuestionSet').val();

    var obj = {
        Questionset: Questionset,
        Examdate: StuExamdate
    };

    $.ajax({
        type: "POST",
        url: '/OnlineExam/ExamdateValidate',
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response["Status"] == "1") {
                alert(response["Message"]);
            } else {
                $('#TimeReamainingTimer').html('');
                GetExamTime();
                CountdownTimer();
                openFullScreen();
            }
        },
        failure: function (response) {
            console.log('Failure : ' + response);
        },
        error: function (response) {
            console.log('Error : ' + response);
        }
    });
}

function GetAnsweredQuestions() {
    var StudentID = $('#StudentID').val();
    var QuestionSet = $('#QuestionSet').val();

    var obj = {
        Qset: QuestionSet,
        StudentID: StudentID
    };

    $.ajax({
        type: "POST",
        url: '/OnlineExam/GetAnsweredQuestionDetails',
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#NoOfReview').text(response.NoOfReviewedQuestion);
            $('#NoOfAttempts').text(response.NoOfQuestionAnswered);
            $('#PendingQuestion').text(response.NoOfPendingQuestion);

            $('#AETotalNoOfQuestions').val(response.TotalNoOfQuestions);
            $('#AENoOfAttemptedQuestions').val(response.NoOfQuestionAnswered);
            $('#AENoOfReviewedQuestions').val(response.NoOfReviewedQuestion);
            $('#AENoOfPendingQuestions').val(response.NoOfPendingQuestion);
            $('#AETotalMarks').val(response.TotalMarks);
        },
        failure: function (response) {
            console.log('Failure : ' + response);
        },
        error: function (response) {
            console.log('Error : ' + response);
        }
    });
}

$(document).on('click', '#btnSubmitExam', function () {
    GetExamTime();
    $('#OnlineExamResultModal').modal('show');
});

$(document).on('click', '#btnMarkAsReview', function () {
    var QuestionID = $('#QuestionID').val();
    var QuestionSet = $('#QuestionSet').val();

    var obj = {
        QSet: QuestionSet,
        QID: QuestionID
    };

    $.ajax({
        type: "POST",
        url: '/OnlineExam/ReviewMarked',
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $(`#btnQuestionGet_${GlobalI}`).removeClass('btn-success');
            $(`#btnQuestionGet_${GlobalI}`).removeClass('btn-light');
            $(`#btnQuestionGet_${GlobalI}`).addClass('btn-warning');

            GetAnsweredQuestions();
        },
        failure: function (response) {
            console.log('Failure : ' + response);
        },
        error: function (response) {
            console.log('Error : ' + response);
        }
    });
});

/*Loader Jquery*/

$('body').append('<div id="loadingDiv" style="display:none;"><div class="loading"></div></div>');

$(document).ajaxSend(function () {
    $('#loadingDiv').show();
});
$(document).ajaxComplete(function () {
    $('#loadingDiv').hide();
});

$(document).on('click', '#btnSubmitModalClose', function () {
    var TotalSeconds = $('#TotalSeconds').val();
    if (TotalSeconds != '-1') {
        $('#OnlineExamResultModal').modal('hide');
    } else {
        $('#btnMarkAsDone').prop('disabled', true);
        $('#btnMarkAsReview').prop('disabled', true);
    }
});