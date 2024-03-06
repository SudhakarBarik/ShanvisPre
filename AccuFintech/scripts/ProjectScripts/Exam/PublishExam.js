$(document).ready(function () {
    $("#Examdate").datepicker({ dateFormat: "dd/mm/yy", changeMonth: true, changeYear: true, yearRange: "1970:" + parseInt(new Date().getFullYear() + 50) + '"' });
    $('#Examdate').datepicker('setDate', new Date());
});