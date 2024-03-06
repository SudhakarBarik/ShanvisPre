$(document).ready(function () {
    $('#FranchaiseCode').chosen();
    $("#DOB").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "1970:" + parseInt(new Date().getFullYear() + 50) + '"' });
    $("#inpFaculty").change(function () { readFacultyPhotoURL(this); });

});

$(document).on('change', '#FranchaiseCode', function () {
    var FranchaiseCode = $('#FranchaiseCode option:selected').val();
    $.ajax({
        url: '/FacultyMaster/GetFacultyFranchaiseWise?FranchaiseCode=' + FranchaiseCode,
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
    table = $("#TBL_FacultyList").DataTable(
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
                  { 'data': 'FacultyID' },
                  { 'data': 'FranchaiseCode' },
                  { 'data': 'FacultyName' },
                  { 'data': 'Phone' },
                  { 'data': 'Gender' },
                  { 'data': 'Email' },
                  { 'data': 'MaxQualification' },
                  { 'data': 'Department' },
                  { 'data': 'TotalExp' },
                  {
                      "data": null, title: 'Action', wrap: true, "render": function (item) {
                          return `<a href="/FacultyMaster/Edit?FacultyID=${item.FacultyID}" class="text-primary font-weight-bold">Edit<a/> | <a href="#" class="text-danger font-weight-bold" onClick="RemoveFaculty('${item.FacultyID}')">Remove<a/>`
                      }
                  }
        ]
    });
}


function RemoveFaculty(FacultyID) {
    if (confirm('Are you sure? you want to remove this Faculty.')) {
        var obj = {
            FacultyID: FacultyID
        };

        $.ajax({
            url: '/FacultyMaster/RemoveFaculty',
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
function readFacultyPhotoURL(input) {
    var re = /(\.jpg|\.jpeg|\.png)$/i;
    if (!re.exec(input.files[0].name)) {
        alert("Profile Image must be JPG OR JPEG OR PNG");
    }
    else {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#ptbFaculty').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
}