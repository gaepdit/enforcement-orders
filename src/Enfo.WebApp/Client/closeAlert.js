import $ from 'jquery';

$(document).ready(function () {
    $('.gaepd-button-closealert').click(function () {
        $(this.parentNode).slideUp();
    });
});
