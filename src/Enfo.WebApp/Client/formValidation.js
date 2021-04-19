// JS dependencies
import $ from 'jquery';
import 'jquery-validation';
import 'jquery-validation-unobtrusive';

const settings = {validClass: "usa-input-success"};
$.validator.setDefaults(settings);
$.validator.unobtrusive.options = settings;
