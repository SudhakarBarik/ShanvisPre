$(document).ready(function () {
    $("input[type=checkbox]").on('change', function () {
        var self = $(this);
        var chkName = self.attr("id");
        var divname = chkName.replace("ChkParrent_", "subDiv_");
        $('#' + divname).find(':checkbox').each(function () {
            $(this).prop('checked', $('#' + chkName).is(':checked'));
        });

        var altname = self.attr("alt");

        if (altname != undefined) {
            var selfdivname = altname.replace("ChkParrent_", "subDiv_");

            var allchk = false;

            $('#' + selfdivname).find(':checkbox').each(function () {
                if ($(this).prop('checked') == true) {
                    allchk = true;
                }
            });

            if (allchk) {
                $('#' + altname).prop('checked', true);
            }
            else {
                $('#' + altname).prop('checked', false);
            }
        }
    });

    $("#ddlUserType").on("change", function () {
        var Aid = $("#ddlUserType").val();
        window.location.href = "/MenuPermission/LoadAsignedMenu?userType=" + Aid;
    });
});