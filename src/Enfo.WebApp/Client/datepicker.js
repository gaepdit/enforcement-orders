// JS dependencies
import $ from 'jquery';
import 'jquery-ui/ui/widgets/datepicker';

// CSS dependencies
import 'jquery-ui/themes/base/core.css';
import 'jquery-ui/themes/base/theme.css';
import 'jquery-ui/themes/base/datepicker.css';

// Set up date pickers
$(document).ready(function () {
    $(".date-picker").datepicker({
        dateFormat: 'm/d/yy',
        changeMonth: true,
        changeYear: true,
        yearRange: "1998:+0"
    });
});
