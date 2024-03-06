$(document).ready(function () {
    $("#FDate").datepicker({ dateFormat: "yy/mm/dd", changeMonth: true, changeYear: true, yearRange: "2024:" + parseInt(new Date().getFullYear() + 50) + '"' });

    $('#TDate').datepicker({
        dateFormat: "yy/mm/dd",
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
});




$(document).ready(() => {
    $('#TBL_SessionsList').DataTable({
        "destroy": true,
        "ajax": {
            "url": "/SessionYear/GetSessionList",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
        { "data": "Name" },
        { "data": "FDate" },
        { "data": "TDate" },
        { "data": "CreateDate" },
        {
            'data': null, title: 'Action',
            wrap: true,
            "render": function (item) {
                return `<a href="#" class="text-danger font-weight-bold" onClick="RemoveSession('${item.Id}')">Remove<a />`
            }
        }
        ]
    });

})

function RemoveSession(Id) {
    if (confirm('Are you sure? you want to remove this Session.')) {
        var obj = {
            Id: Id
        };

        $.ajax({
            url: '/SessionYear/DeleteSession',
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