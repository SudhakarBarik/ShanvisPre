$(document).ready(function () {
    $('#StudentID').chosen();
});

$(document).on('change', '#StudentID', function () {
    var StudentID = $('#StudentID option:selected').val();
    if (StudentID != "") {
        $.ajax({
            type: 'GET',
            url: "/StudentICard/GetStudentICard?StudentID=" + StudentID,
            data: {},
            success: function (response) {
                console.log(response);
                $('#StudentICard').html(response);
            }
        });
    }
});

$(document).on('click', '#btnPrintICard', function () {
    var StudentID = $('#StudentID option:selected').val();
    if (StudentID != "") {
        window.print();
    } else {
        alert("Please Select a Student");
    }
    
});