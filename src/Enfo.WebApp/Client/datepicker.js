// JS dependencies
import 'jquery-ui/ui/widgets/datepicker';

// CSS dependencies
import 'jquery-ui/themes/base/core.css';
import 'jquery-ui/themes/base/theme.css';
import 'jquery-ui/themes/base/datepicker.css';

$(document).ready(function () {
    // Set up date pickers
    $(".date-picker").datepicker({
        dateFormat: 'm/d/yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "1998:+0"
    });
});
