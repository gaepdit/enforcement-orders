$(document).ready(function () {
    // Don't submit empty form fields
    $('#SearchButton').click(function DisableEmptyInputs() {
        $('input').each(function (i) {
            const $input = $(this);
            if ($input.val() === '')
                $input.attr('disabled', 'disabled');
        });
        $('select').each(function (i) {
            const $input = $(this);
            if ($input.val() === '')
                $input.attr('disabled', 'disabled');
        });
        return true;
    });
});
