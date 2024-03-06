$('#State').change(function () {
    var State = $('#State option:selected').val();
    $.ajax({
        url: '/Home/GetAllDistricts?StateID=' + State,
        type: "GET",
        data: '{}',
        contentType: 'application/json; charset=utf-8',
        success: (data) => {
            console.log(data);
            var OptionHtml = '<option Value="">--Select District--</option>';
            data.forEach((item) => {
                OptionHtml += `<option Value="${item.Key}">${item.Value}</option>`;
            });
            $('#District').html(OptionHtml);
        },
        error: function () {
            alert("error");
        }
    });
});