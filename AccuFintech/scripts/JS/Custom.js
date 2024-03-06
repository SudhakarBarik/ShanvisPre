//////////////////////LayOut Control/////////////////////
$('#sidebarCollapse').on('click', function () {
    $('#sidebar').toggleClass('active');
    $('#side-menu').toggleClass('hoverControl');
    $('.UlMenuClass').removeClass('show');
});

$("#sidebar").hover(function () {
    if ($('#side-menu').hasClass('hoverControl') && $(window).width() > 700) {
        $('#sidebar').removeClass('active');
        $('.UlMenuClass').removeClass('show');
    }
}, function () {
    if ($('#side-menu').hasClass('hoverControl') && $(window).width() > 700) {
        $('#sidebar').addClass('active');
        $('.UlMenuClass').removeClass('show');
    }
});

$("#nav-back").on("click", () => {
    if ($(window).width() < 700) {
        $('#sidebar').removeClass('active');
    }
})
////////////////////alert control///////////////////////////////
window.setTimeout(() => {
    $(".alert").fadeTo(500, 0).slideUp(500, function () {
        $(this).remove();
    });
}, 5000);
///////////////////general control//////////////////////////////
let refreshPage = () => {
    window.location.reload();
}