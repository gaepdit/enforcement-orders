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

    let proposedRadio = $("#CreateAs-Proposed");
    if (proposedRadio.is(":checked")) {
        hiddenItems.push('#executed-fieldset');
        hiddenItems.push('#executed-published');
    } else {
        hiddenItems.push('#proposed-fieldset');
        hiddenItems.push('#proposed-published');
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

    // Enforcement type radio buttons listener
    $("input[name='Item.CreateAs']").change(function () {
        if (this.value === "Proposed") {
            switchItems("#executed-fieldset", "#proposed-fieldset");
            switchItems("#executed-published", "#proposed-published");
            enableItemsByClass(".proposed-item");
            disableItemsByClass(".executed-item");
        } else {
            switchItems("#proposed-fieldset", "#executed-fieldset");
            switchItems("#proposed-published", "#executed-published");
            enableItemsByClass(".executed-item");
            disableItemsByClass(".proposed-item");
        }

        setDefaultPublicationDate();
    });

    // Publication status select listener
    $("#Item_Progress").change(function () {
        if ($(this).find(":selected").text() === "Draft") {
            hideItem("#publication-date");
        } else {
            showItem("#publication-date");
        }

        setDefaultPublicationDate();
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
    function setDefaultPublicationDate() {
        if ($("#Item_Progress").find(":selected").text() === "Draft") {
            $(".pub-date").each(function (i, item) {
                $(item).datepicker("setDate", null);
            })
        } else {
            const enforcementType = $("input[name='Item.CreateAs']:checked").val();
            const daysToNextMonday = 8 - (new Date()).getUTCDay();

            if (enforcementType === "Proposed") {
                const pubDateField = $("#Item_ProposedOrderPostedDate");
                if (pubDateField.datepicker("getDate") == null) {
                    pubDateField.datepicker("setDate", daysToNextMonday);
                }
            } else {
                const pubDateField = $("#Item_ExecutedOrderPostedDate");
                if (pubDateField.datepicker("getDate") == null) {
                    pubDateField.datepicker("setDate", daysToNextMonday);
                }
            }
        }
    }

    function showItem(item) {
        return $(item).slideDown(400);
    }

    function hideItem(item) {
        return $(item).slideUp(400);
    }

    function switchItems(hideItem, showItem) {
        return $(hideItem).slideUp(400, function () {
            return $(showItem).slideDown(400);
        });
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
