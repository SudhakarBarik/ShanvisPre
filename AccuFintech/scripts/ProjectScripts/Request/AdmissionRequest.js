$(document).ready(function () {
    $("#Fromdate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "1970:" + parseInt(new Date().getFullYear() + 50) + '"' });
    $('#Fromdate').datepicker('setDate', new Date());
    $("#Todate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "1970:" + parseInt(new Date().getFullYear() + 50) + '"' });
    $('#Todate').datepicker('setDate', new Date());
});

$(document).on('click', '#btnAdmissionRequestSearch', function () {
    var Fromdate = $('#Fromdate').val();
    var Todate = $('#Todate').val();

    var obj = {
        Fromdate: Fromdate,
        Todate: Todate
    };

    $.ajax({
        url: '/AdmissionRequest/AdmissionRequestList',
        type: "POST",
        data: JSON.stringify(obj),
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
    table = $("#TBL_AdmissionRequestList").DataTable(
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
        columns: [{ 'data': 'ReqID' },
                  { 'data': 'Franchaise' },
                  { 'data': 'Student' },
                  { 'data': 'Father' },
                  { 'data': 'Location' },
                  { 'data': 'Phone' },
                  { 'data': 'Altphone' },
                  { 'data': 'Qualification' },
                  { 'data': 'Programmode' },
                  { 'data': 'Programname' }
        ]
    });
}