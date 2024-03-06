$(document).ready(() => {
    $('#franchaiselist').DataTable({
        "destroy": true,
        "ajax": {
            "url": "/Franchaise/GetFranchaiseList",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "FranchaiseCode" },
            { "data": "FranchaiseName" },
            { "data": "FranchaiseOpeningDate" },
            { "data": "FranchaiseAddress" },
            { "data": "FranchaiseManager" },
            { "data": "FranchaisePrefix" },
            { "data": "FranchaiseMobileNo" },
            { "data": "CityName" },
            { "data": "StateName" },           
            {
                'data': null, title: 'Action',
                wrap: true,
                "render": function (item) { 
                    return `<div class="btn-group"><button type="button" onclick="editFranchaise('${item.FranchaiseId}')" class="btn btn-primary btn-sm"><i class="fas fa-edit" aria-hidden="true"></i></button>${item.FranchaiseCode != '001' ? `<button type="button" class="btn btn-danger btn-sm" onclick="deleteFranchaise('${item.FranchaiseId}')"><i class="fa fa-trash" aria-hidden="true"></i></button>` : ``}</div>`
                }
            }
        ]
    });

})

$("#btnCreate").click(() => {
    $("#opstring").val("Insert");
    $('#txtBCode').attr('readonly', false);

    $("#btnsave").show();
    $("#btnUpdate").hide();
    $("#btnDelete").hide();

    $("#exampleModalLongTitle1").show();
    $("#exampleModalLongTitle2").hide();
    $("#exampleModalLongTitle3").hide();
});

let editFranchaise = (brnid) => {
    $.ajax({
        url: '/Franchaise/GetFranchaisebyID',
        type: "GET",
        data: {'Code':brnid},
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            data = JSON.parse(data);
            console.log(data);
            $("#txtBrId").val(data.FranchaiseId);
            $("#txtBCode").val(data.FranchaiseCode);
            $("#txtBName").val(data.FranchaiseName);
            $("#txtAddress").val(data.FranchaiseAddress);
            $("#txtPrefix").val(data.FranchaisePrefix);
            $("#txtPhoneNo").val(data.FranchaiseMobileNo);
            $("#txtManagerName").val(data.FranchaiseManager);
            $("#datepicker").val(data.FranchaiseOpeningDate);
            $("#txtStateName").val(data.StateName);
            $("#txtCityName").val(data.CityName);
            $("#txtOpeningBalance").val(data.OpeningBalance);

            $("#opstring").val("Update");
            $('#txtBCode').attr('readonly', true);
            $('#franchaiseModal').modal('show');
            $("#btnUpdate").show();
            $("#btnDelete").hide();
            $("#btnsave").hide();

            $("#exampleModalLongTitle2").show();
            $("#exampleModalLongTitle1").hide();
            $("#exampleModalLongTitle3").hide();
            $("#Status").hide();
        },
        error: function () {
            alert("Error in data Fetching..");
        }
    });
}

let deleteFranchaise = (brnid) => {
    let checkstr = confirm('Are you sure you want to delete this Franchaise?');
    if (checkstr) {
        $.ajax({
            url: "/Franchaise/DeleteFranchaise",
            method: "POST",
            data: "brId=" + brnid,
            success: (data) => { alert(data); refreshPage(); },
            error: (err) => { console.log(err); }
        })

    } else {
        return false;
    }
}
$("#datepicker").datepicker({ dateFormat: "dd/mm/yy" });


function UserCheck() {
    $("#btnsave").prop("disabled", true);
    if ($("#txtBCode").val() != '') {
        $("#Status").text("Checking...");
        $.ajax({
            url: "/Franchaise/CheckFranchaise",
            method: "GET",
            data: "FranchaiseCode=" + $("#txtBCode").val(),
            success: function (data) {
                if (data == "False") {
                    $("#Status").text('Available');
                    $("#Status").css("color", "Green");
                    $("#btnsave").prop("disabled", false);
                }
                else {
                    $("#Status").text('Franchaise code already register');
                    $("#Status").css("color", "Red");
                    $("#btnsave").prop("disabled", true);
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
