// Set up date pickers
$(document).ready(function () {
    $(".date-picker").datepicker({
        dateFormat: 'm/d/yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "1998:+0"
    });
});
