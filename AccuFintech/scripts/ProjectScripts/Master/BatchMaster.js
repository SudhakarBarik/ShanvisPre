//$(document).ready(function () {
//    $("#FDate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "2024:" + parseInt(new Date().getFullYear() + 50) + '"' });
//    $("#TDate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "2024:" + parseInt(new Date().getFullYear() + 50) + '"' });
//});



$(document).ready(function () {
    $("#FDate").datepicker({
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        yearRange: "2024:" + (new Date().getFullYear() + 50)
    });

    $('#TDate').datepicker({
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        yearRange: "2024:" + (new Date().getFullYear() + 50),
        startDate: $('#FDate').val(),
        onSelect: function (dateText) {
            var start_date = new Date($('#FDate').val()),
                end_date = new Date(dateText),
                diff = new Date(end_date - start_date),
                days = parseInt(diff / (1000 * 60 * 60 * 24), 10);

            if (days < 1) {
                alert('End date should be greater than start date!');
                $(this).val('');
                return false;
            }
        }
    });
    $("#BStartDate").datepicker({ dateFormat: "yy/mm/dd", changeMonth: true, changeYear: true, yearRange: "2024:" + parseInt(new Date().getFullYear() + 50) + '"' });
    $('#Frequncy').css('pointer-events', 'none');
    $('#Sessionlist').chosen();
}); 

$('#CountTimes').focusout(() => {
    GenerateFrequncy();
});

$('#Mode').change(function () {
    GenerateFrequncy();
    var mode = $('#Mode').val();
    if (mode === ".Dly") {
        $('#CountTimes').val(1);
    }
});

function GenerateFrequncy() {
    var Count = $('#CountTimes').val();
    var Mode = $('#Mode').val();
    var GeneratedPFrequncy = "";

    if (Count != "" && Mode != "") {
        GeneratedPFrequncy = Count + ' - ' + Mode;
        $('#Frequncy').val(GeneratedPFrequncy);
        populateTable();
    }
}


function populateTable() {
    var startingDate = $("#BStartDate").val();
    var Times = $('#CountTimes').val();
    var mode = $('#Mode').val();
    if (mode === ".Dly") {
        return $("#Tbody_BatchList").empty();
    }
    var dates = [];
    var currentDate = new Date(startingDate);

    var offset = 0;
    while (dates.length < 7) {
        var nextDate = new Date();
        nextDate.setDate(currentDate.getDate() + offset);
        dates.push(nextDate);
        offset++;
    }

    $("#Tbody_BatchList").empty();
    const weekday = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

    for (var i = 0; i < dates.length; i++) {
        var row = $("<tr>");
        var dateCell = $("<td>").text(dates[i].toLocaleDateString());
        var dayCell = $("<td>").text(weekday[dates[i].getDay()]);
        var checkboxCell = $("<td>");
        var checkbox = $("<input>").attr("type", "checkbox");
        checkbox.change(function () {
            validateCheckboxes();
        });
        checkboxCell.append(checkbox);
        row.append(dateCell, dayCell, checkboxCell);
        $("#Tbody_BatchList").append(row);
    }
    validateCheckboxes();
}





function validateCheckboxes() {
    var checkedCount = $("#Tbody_BatchList input[type='checkbox']:checked").length;
    var timesValue = parseInt($('#CountTimes').val());

    if (checkedCount > timesValue) {
        $('#Tbody_BatchList input[type="checkbox"]:checked:last').prop('checked', false);
        alert("The number of checked checkboxes must match the value of 'Times'.");
    }
}



$("#Confirm_Submit").click(function () {
    var checkedDatesAndDays = [];
    $("#Tbody_BatchList tr").each(function () {
        var checkbox = $(this).find("input[type='checkbox']");
        if (checkbox.is(":checked")) {
            var date = $(this).find("td:first-child").text();
            var day = $(this).find("td:nth-child(2)").text();
            checkedDatesAndDays.push({ date: date, day: day });
        }
    });

    var checkedDatesAndDaysJSON = JSON.stringify(checkedDatesAndDays);

    $("#hiddenCheckedData").val(JSON.stringify(checkedDatesAndDays));
    $("#formId").submit();
});







$(document).on('change', '#BStartDate', function () {
    GetCourseDetails();
});

$(document).on('change', '#CourseID', function () {
    GetCourseDetails();
});

function GetCourseDetails() {
    var Coursecode = $('#CourseID option:selected').val();
    var Joinindate = $('#BStartDate').val();

    if (Coursecode != "" && Joinindate != "") {
        $.ajax({
            type: 'GET',
            url: "/StudentRegistration/GetCourseDetails?CourseID=" + Coursecode + "&CourseStartdate=" + Joinindate,
            data: {},
            success: function (response) {
                console.log(response);
                var cd = response.Coursecode;
                $('#BEndDate').val(response.CourseEnddate);
            }
        });
    }
}

$(document).on('change', '#BatchID', function () {
    UserCheck();
});

function UserCheck() {
    $("#Confirm_Submit").prop("disabled", true);
    if ($("#BatchID").val() != '') {
        $("#Status").text("Checking...");
        $.ajax({
            url: "/BatchMaster/CheckBatchId",
            method: "GET",
            data: "Batchid=" + $("#BatchID").val(),
            success: function (data) {
                if (data == "False") {
                    $("#Status").text('Available');
                    $("#Status").css("color", "Green");
                    $("#Confirm_Submit").prop("disabled", false);
                }
                else {
                    $("#Status").text('BatchID already register');
                    $("#Status").css("color", "Red");
                    $("#Confirm_Submit").prop("disabled", true);
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




$(document).on('change', '#Sessionlist', function () {
    var SessionID = $('#Sessionlist option:selected').val();
    $.ajax({
        url: '/BatchMaster/GetBatchDetailsBySession?SessionID=' + SessionID,
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


function OnSuccess(jdata) {
    var table;
    table = $("#TBL_BatchMaster").DataTable(
    {
        "bDestroy": true,
        processing: true,
        bLengthChange: true,
        lengthMenu: [[10, 50, 100, -1], [10, 50, 100, "All"]],
        bFilter: true,
        bSort: true,
        bPaginate: true,
        dom: 'lBfrtip',
        data: jdata,
        columns: [
                  { 'data': 'BatchID' },
                  { 'data': 'BatchName' },
                  { 'data': 'CourseID' },
                  { 'data': 'Session' },
                  { 'data': 'BStartDate' },
                  { 'data': 'BEndDate' },
                  { 'data': 'Frequncy' },
                  {
                      "data": null, title: 'Action', wrap: true, "render": function (item) {
                          return `<a href="#" class="text-danger font-weight-bold" onClick="RemoveBatch('${item.BatchID}')">Remove<a/>`
                      }
                  }
        ]
    });
}




function RemoveBatch(BatchID) {
    if (confirm('Are you sure? you want to remove this Batch.')) {
        var obj = {
            BatchID: BatchID
        };

        $.ajax({
            url: '/BatchMaster/RemoveBatchDetails',
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