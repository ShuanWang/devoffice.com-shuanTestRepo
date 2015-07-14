$(document).ready(function () {

    $('body').scrollspy({ target: '#side-nav' });

    var form = $('#api-request-form');
    form.find('#ajax-loader').hide();
    form.find('#form-error-message').hide();
    form.removeAttr('action').removeAttr('method');

    var piTop = $('#user-info-contents').offset().top;
    $('#side-nav').affix({
        offset: {
            top: piTop
        }
    });
});

devOfficeApp.controller('devCenterActivityFeedAPIFormController', ['$scope', '$log', function ($scope, $log) {

    var form = $('#api-request-form');
    var cancelLink = form.find("#cancel-link");
    var submitLink = form.find("#submit-link");
    var ajaxLoader = form.find('#ajax-loader');

    var defaultFormObject = formTemplate;
    defaultFormObject.MonthlyUsers = null;
    defaultFormObject.DailyUsers = null;
    defaultFormObject.Current365Customers = null;
    defaultFormObject.Future365Customers = null;
    defaultFormObject.Country = '';
    defaultFormObject.State = '';
    $scope.data = angular.copy(defaultFormObject);

    $scope.cancelClick = function () {
        $scope.data = angular.copy(defaultFormObject);
        ajaxLoader.hide();
    }

    $scope.submitClick = function () {
        var inputs = form.find('input');
        var textAreas = form.find('textarea');
        var selects = form.find('select');
        var isFormValid = true;
        var fields = $.merge(inputs, textAreas);
        fields = $.merge(fields, selects);

        //Using html validation
        for (var i = 0; i < fields.length; i++) {
            if (!fields[i].checkValidity()) {
                isFormValid = false;
                break;
            }
        }

        if (isFormValid) {

            cancelLink.hide();
            submitLink.hide();
            ajaxLoader.show();

            // ajax request to server with all the form data
            $.ajax({
                type: 'POST',
                cache: false,
                contentType: 'application/json; charset=utf-8',
                url: '/ApiRequest?__RequestVerificationToken=' + $('#anti-forgery-token').val(),
                data: angular.toJson($scope.data)
            }).done(function (data) {
                ajaxLoader.hide();
                cancelLink.show();
                submitLink.show();
                if (data && data.Success && !data.Error) {
                    // Successful - redirect to thank you page
                    window.location.href = "/programs/managementactivityapithankyou";
                } else {
                    // Something went wrong. Display error message
                    form.find('#form-error-message').show();
                    if (data.Error) {
                        $log.log(data.Error);
                    }
                }
            });
        }
        return $scope.dataJsonString;
    }
}]);