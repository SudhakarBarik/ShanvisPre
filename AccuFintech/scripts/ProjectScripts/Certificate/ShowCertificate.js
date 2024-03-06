$(document).ready(function () {
    $('#Course').chosen();
});

$(document).on('change', '#Course', function () {
    var Course = $('#Course option:selected').val();
    if (Course != "") {
        $.ajax({
            url: '/ShowCertificate/GetCertificateDetails?Course=' + Course,
            type: "GET",
            data: '{}',
            contentType: 'application/json; charset=utf-8',
            success: (data) => {
                console.log(data);
                $('#ShowCertificate').html('');
                var CertificateHTML = '';
                if(data.hasOwnProperty('Certificate') && data.Certificate != ""){
                    CertificateHTML += `<div class="col-md-4 col-12 text-center">
                                             <div class ="d-flex justify-content-center">
                                                <div class ="ImgPicDiv mb-2">
                                                    <img src="/Content/Image/Pdf.png" id="ptbStudent" height="100" width="150" alt="Certificate">
                                                </div>
                                            </div>
                                            <a href="${data.Certificate}" class ="btn btn-primary btn-sm" target="_blank">View Certificate</a>
                                        </div>`;
                }

                if (data.hasOwnProperty('Marksheet') && data.Marksheet != "") {
                    CertificateHTML += `<div class="col-md-4 col-12 text-center">
                                            <div class ="d-flex justify-content-center">
                                                 <div class ="ImgPicDiv mb-2">
                                                     <img src="/Content/Image/Pdf.png" id="ptbStudent" height="100" width="150" alt="Marksheet">
                                                </div>                                                
                                            </div>
                                            <a href="${data.Marksheet}" class ="btn btn-primary btn-sm" target="_blank">View Marksheet</a>
                                        </div>`;
                }

                $('#ShowCertificate').html(CertificateHTML);
            },
            error: function () {
                alert("error");
            }
        });
    }
});