$(document).ready(function () {
    $("#Fromdate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "1970:" + parseInt(new Date().getFullYear() + 50) + '"' });
    $('#Fromdate').datepicker('setDate', new Date());
    $("#Todate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "1970:" + parseInt(new Date().getFullYear() + 50) + '"' });
    $('#Todate').datepicker('setDate', new Date());
});

$(document).on('click', '#btnSearchExamReport', function () {
    var Fromdate = $('#Fromdate').val();
    var Todate = $('#Todate').val();

    var obj = {
        Fromdate: Fromdate,
        Todate: Todate
    };

    $.ajax({
        type: "POST",
        url: '/ExamReport/GetExamReportList',
        data: JSON.stringify(obj),
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

    table = $("#TBL_ExamReportList").DataTable(
    {
        "bDestroy": true,
        processing: true,
        bLengthChange: true,
        lengthMenu: [[10, 50, 100, -1], [10, 50, 100, "All"]],
        bFilter: true,
        bSort: true,
        bPaginate: true,
        dom: 'lBfrtip',
        buttons: [
               {
                   extend: 'pdf',
                   footer: false,
                   title: 'Exam Report',
                   exportOptions: {
                       columns: [0, 1, 2, 3, 4, 5, 6, 7]
                   },
                   customize: function (doc) {
                       doc.defaultStyle.fontSize = 8; 
                       doc.styles.tableHeader.fontSize = 10; 
                       doc.content[1].table.widths = ['12%', '20%', '10%', '10%', '15%', '12%', '10%', '10%'];
                   }
               },
               {
                   extend: 'excel',
                   footer: false,
                   title: 'Exam Report',
                   exportOptions: {
                       columns: [0, 1, 2, 3, 4, 5, 6 , 7]
                   }
               }
        ],
        data: Jdata,
        columns: [{ 'data': 'StudentID' },
                  { 'data': 'Studentname' },
                  { 'data': 'Starttime' },
                  { 'data': 'Endtime' },
                  { 'data': 'TotalQuestion' },
                  { 'data': 'NoOfAttempt' },
                  { 'data': 'NoOfPending' },
                  { 'data': 'Marks' },
                  { 'data': 'Discount' },
        ]
    });

    $('#ExamReportExportButtons').show();
}