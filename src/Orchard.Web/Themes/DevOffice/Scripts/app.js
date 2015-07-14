var pks = pks || {};
pks.app = function () {
    if (document.getElementById("feedbackWidget")) {
        var _feedbackViewModel = pks.feedback();
        ko.applyBindings(_feedbackViewModel, document.getElementById("feedbackWidget"));
    }
};

// GLOBAL DEV OFFICE ANGULAR APP DEFINITION
var devOfficeApp = angular.module('devOfficeApp', ['contenteditable']);
