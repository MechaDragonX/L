$(document).ready(function() {
    $('#hide').click(function () {
        $('#details').hide();
    });
    $('#surnameFilter').keyup(function() { 
        $("#applicantTable td.surname:contains('" + $(this).val() + "')").parent().show();
        $("#applicantTable td.surname:not(:contains('" + $(this).val() + "'))").parent().hide();
    });
});
