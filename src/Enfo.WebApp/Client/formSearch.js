﻿// JS dependencies
import $ from 'jquery';

// Don't submit empty form fields
$(document).ready(function () {
    function disableEmptyInput(n, el) {
        const $input = $(el);
        if ($input.val() === '')
            $input.attr('disabled', 'disabled');
    }

    $('#SearchButton').click(function DisableEmptyInputs() {
        $('input').each(disableEmptyInput);
        $('select').each(disableEmptyInput);
        return true;
    });
});
