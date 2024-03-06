$(document).ready(function () {
    $('#FranchaiseCode').chosen();
    $('#Studentdd').chosen();
});




$(document).on('click', '#btnSearchFees', function () {
    var FranchaiseCode = $('#FranchaiseCode option:selected').val();
    var StudentID = $('#Studentdd option:selected').val();
    $.ajax({
        url: "/FeesStructure/GetFeesStrFilter?FranchaiseCode=" + FranchaiseCode + "&StudentId=" + StudentID,
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
    table = $("#TBL_FeesStructure").DataTable(
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
                  { 'data': 'FranchaiseCode' },
                  { 'data': 'Studentid' },
                  { 'data': 'Course' },
                  { 'data': 'Batch' },
                  { 'data': 'TotalFees' },
                  { 'data': 'TotalPaid' },
                  { 'data': 'RemainFee' },
                  { 'data': 'PayDate' },
                  {
                      "data": null, title: 'Action', wrap: true, "render": function (item) {
                          return `<div class="btn-group"><a href="#" class="text-primary font-weight-bold" onClick="ViewStudentFees('${item.Studentid}')">View<a/>  |  ${item.RemainFee != '0' ? `<a href="#" onclick="Paystudentid('${item.Studentid}')" class="text-Success font-weight-bold">Pay<a/>` : ``}</div>`

                          //return `<a href="#" onclick="Paystudentid('${item.Studentid}')" class="text-Success font-weight-bold">Pay<a/> | <a href="#" class="text-danger font-weight-bold" onClick="ViewStudentFees('${item.Studentid}')">View<a/>`
                      }
                  }
        ]
    });
}


let ViewStudentFees = (StudentId) => {
    $.ajax({
        url: '/FeesStructure/GetAllFeesByStudentId?StudentId=' + StudentId,
        type: "GET",
        data: '{}',
        contentType: 'application/json; charset=utf-8',
        success: (data) => {
            console.log(data);
            OnSuccess2(data);
            $('#FessViewModal').modal('show');
        },
        error: function () {
            alert("error");
        }
    });
}


function OnSuccess2(Jdata) {
    var table;
    table = $("#TBL_FeesStudentId").DataTable(
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
                  { 'data': 'FranchaiseCode' },
                  { 'data': 'Studentid' },
                  { 'data': 'Batch' },
                  { 'data': 'Course' },
                  { 'data': 'TotalFees' },
                  { 'data': 'PaidFees' },
                  { 'data': 'PayDate' },
        ]
    });
}





let Paystudentid = (StudentId) => {
    $.ajax({
        url: '/FeesStructure/GetFeesByStudentID',
        type: "GET",
        data: { 'StudentId': StudentId },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            data = JSON.parse(data);
            console.log(data);
            $("#txtFranchaiseCode").val(data.FranchaiseCode);
            $("#txtStudentId").val(data.Studentid);
            $("#txtCourse").val(data.Course);
            $("#txtBatch").val(data.Batch);
            $("#txtTotalFees").val(data.TotalFees);
            $("#txtTotalPaid").val(data.TotalPaid);
            $("#txtRemainFee").val(data.RemainFee);
            $('#FeesStPayModal').modal('show');
        },
        error: function () {
            alert("Error in data Fetching..");
        }
    });
}