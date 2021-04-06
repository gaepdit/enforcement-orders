﻿// JS dependencies
import $ from 'jquery';

// Don't submit empty form fields
$(document).ready(function () {
    $('#SearchButton').click(function DisableEmptyInputs() {
        $('input').each(function () {
            const $input = $(this);
            if ($input.val() === '')
                $input.attr('disabled', 'disabled');
        });
        $('select').each(function () {
            const $input = $(this);
            if ($input.val() === '')
                $input.attr('disabled', 'disabled');
        });
        return true;
    });
});
