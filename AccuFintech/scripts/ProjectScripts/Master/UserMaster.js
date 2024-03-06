$(document).ready(function () {
    $('#opsection').val('INSERT');
    GetAllUser();
});

function GetAllUser() {
    $.ajax({
        type: "GET",
        url: '/UserMaster/GetAllUserJson',
        data: '{}',
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
}

function OnSuccess(Jdata) {

    var table;

    table = $("#TBL_UserList").DataTable(
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
        columns: [{ 'data': 'UserID' },
                  { 'data': 'UserType' },
                  { 'data': 'Fullname' },
                  { 'data': 'Phone' },
                  { 'data': 'LockModeStatus' },
                  {
                      "data": null, title: 'Action', wrap: true, "render": function (item) {
                          return `<a href="#" class="text-success font-weight-bold" onclick="GetUserDetails('${item.UserID}')">Edit</a> | <a href="#" class="text-danger font-weight-bold" onclick="DeleteUser('${item.UserID}')">Delete</a>`
                      }
                  }
        ]
    });

}

function GetUserDetails(UserID) {
    $.ajax({
        type: "GET",
        url: '/UserMaster/GetUserByUserID?UserID=' + UserID,
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response);
            $('#UserID').prop('readonly', true);
            $('#UserID').val(response.UserID);
            $('#Password').val(response.Password);
            $('#UserType').val(response.UserType);
            $('#Fullname').val(response.Fullname);
            $('#Phone').val(response.Phone);
            if (response.LockMode == true) {
                $('#IsActive').prop('checked', true);
            } else {
                $('#IsActive').prop('checked', false);
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

function DeleteUser(UserID) {
    if (confirm("Are you sure? you want to delete this User.")) {
        var obj = {
            UserID: UserID
        };

        $.ajax({
            type: "POST",
            url: '/UserMaster/DeleteUser',
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
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