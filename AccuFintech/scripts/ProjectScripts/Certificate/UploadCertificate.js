$(document).ready(function () {
    $('#Franchaise').chosen();
    $('#Course').chosen();
    $('#Student').chosen();
});

$(document).on('change', '#Course', function () {
    GetAllStudent();
});

$(document).on('change', '#Franchaise', function () {
    GetAllStudent();
});

function GetAllStudent() {
    var Course = $('#Course option:selected').val();
    var Franchaise = $('#Franchaise option:selected').val();

    if (Course != "" && Franchaise != "") {
        $.ajax({
            url: '/UploadCertificate/Getstudents?Course=' + Course + '&Franchaise=' + Franchaise,
            type: "GET",
            data: '{}',
            contentType: 'application/json; charset=utf-8',
            success: (data) => {
                $('#Student').html('');
                var StudentDropdown = '<option Value="">--Select Student--</option>';
                data.forEach((item) => {
                    StudentDropdown += `<option Value="${item.Key}">${item.Value}</option>`;
                });
                $('#Student').html(StudentDropdown);
                $('#Student').trigger("chosen:updated")
            },
            error: function () {
                alert("error");
            }
        });
    }
}

$(document).on('click', '#UploadCertificate', function () {
    var Student = $('#Student option:selected').val();
    var Franchaise = $('#Franchaise option:selected').val();
    var Course = $('#Course option:selected').val();
    var StrCertificate = $('#StrCertificate').val();
    var StrMarksheet = $('#StrMarksheet').val();

    if ($('#Certificate')[0].files[0] == "") {
        alert("Please Upload PDF Document");
        return false;
    }

    if ($('#Certificate')[0].files[0].type != "application/pdf") {
        alert("Please Upload PDF Document");
        return false;
    }

    if (Student != "") {
        var Certificate = $('#Certificate')[0].files[0];

        let formData = new FormData();
        formData.append("Student", Student);
        formData.append("Franchaise", Franchaise);
        formData.append("Course", Course);
        formData.append("Certificate", Certificate);
        formData.append("StrCertificate", StrCertificate);
        formData.append("StrMarksheet", StrMarksheet);

        $.ajax({
            url: '/UploadCertificate/UploadDocuments',
            type: "POST",
            data: formData,
            dataType: 'json',
            mimeType: 'multipart/form-data',
            contentType: false,
            cache: false,
            processData: false,
            success: (data) => {
                console.log(data);
                if (data.status == "0") {
                    alert("Dcoument uploaded successfully");
                    $('#Student').trigger('change');
                } else {
                    alert("Failed to upload document");
                }
                
            },
            error: function () {
                alert("error");
            }
        });
    }
});

$(document).on('click', '#UploadMarksheet', function () {
    var Student = $('#Student option:selected').val();
    var Franchaise = $('#Franchaise option:selected').val();
    var Course = $('#Course option:selected').val();
    var Marksheet = $('#Marksheet')[0].files[0];
    var StrCertificate = $('#StrCertificate').val();
    var StrMarksheet = $('#StrMarksheet').val();

    if ($('#Marksheet')[0].files[0] == "") {
        alert("Please Upload PDF Document");
        return false;
    }

    if ($('#Marksheet')[0].files[0].type != "application/pdf") {
        alert("Please Upload PDF Document");
        return false;
    }

    if (Student != "") {
       
        let formData = new FormData();
        formData.append("Student", Student);
        formData.append("Franchaise", Franchaise);
        formData.append("Course", Course);
        formData.append("Marksheet", Marksheet);
        formData.append("StrCertificate", StrCertificate);
        formData.append("StrMarksheet", StrMarksheet);

        $.ajax({
            url: '/UploadCertificate/UploadDocuments',
            type: "POST",
            data: formData,
            dataType: 'json',
            mimeType: 'multipart/form-data',
            contentType: false,
            cache: false,
            processData: false,
            success: (data) => {
                console.log(data);
                if (data.status == "0") {
                    alert("Dcoument uploaded successfully");
                    $('#Student').trigger('change');
                } else {
                    alert("Failed to upload document");
                }
            },
            error: function () {
                alert("error");
            }
        });
    }
});

$(document).on('change', '#Student', function () {
    var Student = $('#Student option:selected').val();
    var Franchaise = $('#Franchaise option:selected').val();
    var Course = $('#Course option:selected').val();

    var obj = {
        Student: Student,
        Course: Course,
        Franchaise: Franchaise
    };

    $.ajax({
        url: '/UploadCertificate/GetUploadedDocuments',
        type: "POST",
        data: JSON.stringify(obj),
        contentType: 'application/json; charset=utf-8',
        success: (data) => {
            console.log(data);
            if (data.hasOwnProperty('Certificate') && data.Certificate != "") {
                $('#StrCertificate').val(data.Certificate);
                $("#ViewCertificate").attr("href", data.Certificate);
                $("#ViewCertificate").attr("target", "_blank");
                $("#UploadCertificate").html('<i class="fa fa-upload fa-lg text-color" aria-hidden="true"></i>');
                $("#ViewCertificate").html('<i class="fa fa-eye fa-lg text-success" aria-hidden="true"></i>');
            } else {
                $("#UploadCertificate").html('<i class="fa fa-upload fa-lg text-primary" aria-hidden="true"></i>');
                $("#ViewCertificate").html('<i class="fa fa-eye fa-lg text-color" aria-hidden="true"></i>');
            }

            if (data.hasOwnProperty('Marksheet') && data.Marksheet != "") {
                $('#StrMarksheet').val(data.Marksheet);
                $("#ViewMarksheet").attr("href", data.Marksheet);
                $("#ViewMarksheet").attr("target", "_blank");
                $("#UploadMarksheet").html('<i class="fa fa-upload fa-lg text-color" aria-hidden="true"></i>');
                $("#ViewMarksheet").html('<i class="fa fa-eye fa-lg text-success" aria-hidden="true"></i>');
            } else {
                $("#UploadMarksheet").html('<i class="fa fa-upload fa-lg text-primary" aria-hidden="true"></i>');
                $("#ViewMarksheet").html('<i class="fa fa-eye fa-lg text-color" aria-hidden="true"></i>');
            }
        },
        error: function () {
            alert("error");
        }
    });
});