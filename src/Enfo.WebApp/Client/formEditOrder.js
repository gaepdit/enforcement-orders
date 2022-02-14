// JS dependencies
import $ from 'jquery';
import 'jquery-ui-timepicker-addon';

// CSS dependencies
import 'jquery-ui-timepicker-addon/dist/jquery-ui-timepicker-addon.min.css'

// Set up new order page functionality
$(document).ready(function () {

    // Initially hidden items
    let hiddenItems = [];
    let hearingCheckbox = $("#Item_IsHearingScheduled");
    if (!hearingCheckbox.is(":checked")) {
        hiddenItems.push('#public-hearing-details');
    }

    let executedCheckbox=$("#Item_IsExecutedOrder");
    if (!executedCheckbox.is(":checked")){
        hiddenItems.push("#executed-details");
        hiddenItems.push("#executed-published");
    }

    let progress = $("#Item_Progress");
    if (progress.find(":selected").text() === "Draft") {
        hiddenItems.push('#publication-date');
    }

    hideItemsByIds(hiddenItems);

    // Public hearing checkbox listener
    hearingCheckbox.change(function () {
        if (this.checked) {
            showItem("#public-hearing-details");
            enableItemsByClass(".hearing-item");
        } else {
            hideItem("#public-hearing-details");
            disableItemsByClass(".hearing-item");
        }
    });

    // Executed order checkbox listener
    executedCheckbox.change(function () {
        if (this.checked) {
            enableItemsByClass(".executed-item");
            showItem("#executed-details");
            showItem("#executed-published");
        } else {
            disableItemsByClass(".executed-item");
            hideItem("#executed-details");
            hideItem("#executed-published");
        }
    });

    // Publication status select listener
    progress.change(function () {
        if ($(this).find(":selected").text() === "Draft") {
            hideItem("#publication-date");
        } else {
            showItem("#publication-date");
        }
    });

    // Set up date-time pickers
    $(".date-time-picker").datetimepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "1998:+0",
        dateFormat: 'm/d/yy',
        controlType: 'select',
        timeFormat: 'h:mm TT',
        stepMinute: 15,
        oneLine: true
    });

    // Functions
    function showItem(item) {
        return $(item).slideDown(400);
    }

    function hideItem(item) {
        return $(item).slideUp(400);
    }

    function hideItemsByIds(ids) {
        $(ids.join()).each(function (i, item) {
            return $(item).hide();
        });
    }

    function enableItemsByClass(c) {
        $(c).each(function (i, item) {
            return $(item).removeAttr('disabled');
        });
    }

    function disableItemsByClass(c) {
        $(c).each(function (i, item) {
            return $(item).attr('disabled', 'disabled');
        });
    }

});
