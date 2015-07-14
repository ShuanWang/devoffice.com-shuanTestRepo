$(document).ready(function () {
    $('#devCenter-partner-form .ajax-loader').hide();

    var piTop = $('#partner-contents').offset().top;
    console.log(piTop);
    $('#side-nav').affix({
        offset: {
            top: piTop
        }
    });

    $('body').scrollspy({ target: '#side-nav' });

    $('#devCenter-partner-form form').removeAttr('action').removeAttr('method').attr('id', 'partnerForm');

});

devOfficeApp.controller('devCenterSecretFormController', ['$scope', '$log', function ($scope, $log) {

    $scope.data = {
        FirstName: '',
        LastName: '',
        Email: '',
        Phone: '',
        Company: '',
        Industry: '',
        CompanyAddress: '',
        Country: '',
        State: '',
        City: '',
        Zip: null,
        ProductUrl: '',
        IntegrationType: '',
        Scenarios: '',
        Description: '',
        MonthlyUsers: null,
        DailyUsers: null,
        ViewSessions: null,
        EditSessions: null,
        MsWord: null,
        MsPpt: null,
        MsXl: null,
        NorthAmerica: null,
        SouthAmerica: null,
        Europe: null,
        Asia: null,
        Africa: null,
        Australia: null,
        Distribution: '',
        EncodedImageData: []
    }
    /* FirstName: 'd',
        LastName: 'b',
        Email: 'd@b.com',
        Phone: '123452626',
        Company: 'a',
        Industry: 'education',
        CompanyAddress: 'a',
        Country: 'US',
        State: 'WA',
        City: 'k',
        Zip: 12345,
        ProductUrl: 'http://a.com',
        IntegrationType: 'commercial',
        Scenarios: 'asdf',
        Description: 'asdf',
        MonthlyUsers: 1,
        DailyUsers: 1,
        ViewSessions: 1,
        EditSessions: 1,
        MsWord: 1,
        MsPpt: 1,
        MsXl: 1,
        NorthAmerica: 1,
        SouthAmerica: 1,
        Europe: 1,
        Asia: 1,
        Africa: 1,
        Australia: 1,
        Distribution: 'asdf',
        EncodedImageData: []*/

    var defaultData = {
        FirstName: '',
        LastName: '',
        Email: '',
        Phone: '',
        Company: '',
        Industry: '',
        CompanyAddress: '',
        Country: '',
        State: '',
        City: '',
        Zip: null,
        ProductUrl: '',
        IntegrationType: '',
        Scenarios: '',
        Description: '',
        MonthlyUsers: null,
        DailyUsers: null,
        ViewSessions: null,
        EditSessions: null,
        MsWord: null,
        MsPpt: null,
        MsXl: null,
        NorthAmerica: null,
        SouthAmerica: null,
        Europe: null,
        Asia: null,
        Africa: null,
        Australia: null,
        Distribution: '',
        EncodedImageData: []
    }

    $scope.isFileAPIAvailable = false;
    $scope.areImagesVisible = false;
    $scope.isPhotoUploadInvalid = false;
    $scope.dataJsonString = "";
    $scope.antiForgeryToken = '';

    $scope.resetInvalidPhoto = function () {
        $scope.isPhotoUploadInvalid = false;
    }

    $scope.removePhotos = function () {
        $('#partner-photo-upload').val('');
        $scope.areImagesVisible = false;
        $('#partner-photo-list').empty();
    }
    // remove everything on page load
    $scope.removePhotos();

    // reset the form entirely
    $scope.cancelClick = function () {
        
        $scope.data = defaultData;
        $scope.removePhotos();
        $('#devCenter-partner-form .ajax-loader').hide();
    }

    // prepare form submission - convert all data to JSON string
    $scope.submitClick = function () {

        // check the validity of all fields manually
        var $form = $('#partnerForm');
        var $inputs = $form.children().find('input');
        var $textareas = $form.children().find('textarea');
        var isFormValid = true;
        var fields = $.merge($inputs, $textareas);

        for(var i = 0; i < fields.length; i++) {
            if (!fields[i].checkValidity()) {
                isFormValid = false;
                $log.log('form is invalid, breaking');
                break;
            }
        }

        if (isFormValid) {

            // convert object to JSON string to pass to the api controller
            $scope.dataJsonString = angular.toJson($scope.data);
            //$log.log($scope.dataJsonString);

            $scope.antiForgeryToken = $('#partner-aft').val();

            $('#devCenter-partner-form .ajax-loader').show();

            // ajax request to server with all the form data
            $.ajax({
                type: 'POST',
                cache: false,
                contentType: 'application/json; charset=utf-8',
                url: '/Api?__RequestVerificationToken=' + $scope.antiForgeryToken,
                data: $scope.dataJsonString
            }).done(function(data) {
                //$log.log('request finished');
                //$log.log(data);
                $('#devCenter-partner-form .ajax-loader').hide();
                if (data && data.Success && !data.Error) {
                    // Successful - redirect to thank you page
                    window.location.href = "/Programs/officecloudstoragethankyou";
                } else {
                    // Something went wrong. Display error message
                    $('#devCenter-partner-form #form-error-message').show();
                    if (data.Error) {
                        $log.log(data.Error);
                    }
                }
            });
        }
        return $scope.dataJsonString;
    }

    // initialize the file API state
    function initializeFileApi() {
        // Check for the various File API support.
        if (window.File && window.FileReader && window.FileList && window.Blob) {
            // Great success! All the File APIs are supported.
            console.log('we have a file api');
            $scope.isFileAPIAvailable = true;
        } else {
            console.log('The File APIs are not fully supported in this browser.');
            $scope.isFileAPIAvailable = false;
        }
    }

    initializeFileApi();


    function readBlob(e) {

        var files = document.getElementById('files').files;
        if (!files.length) {
            alert('Please select a file!');
            return;
        }

        var file = files[0];
        var start = 0;
        var stop = file.size - 1;

        var reader = new FileReader();

        // If we use onloadend, we need to check the readyState.
        reader.onloadend = function (evt) {
            if (evt.target.readyState == FileReader.DONE) { // DONE == 2
            }
        };
    }


    $('#partner-photo-upload').change(function (event) {
        var files = event.target.files; // files list is READONLY, so we can't modify it directly - we need to make a copy

        // update scope manually since we're inside jquery
        $scope.$apply(function () {
            $scope.areImagesVisible = files != null && files != 'undefined';
        });

        // files is a FileList of File objects. List some properties.
        for (var i = 0, f; f = files[i]; i++) {

            // Only process image files.
            if (!f.type.match('image.*')) {
                $(this).val(''); // remove the file immediately
                $scope.$apply(function () {
                    $log.log('invalid file type...');
                    $scope.areImagesVisible = false;
                    $scope.isPhotoUploadInvalid = true;
                });
                return;
            }

            var reader = new FileReader();

            // Closure to capture the file information.
            reader.onload = (function (theFile) {
                return function (e) {
                    // Render thumbnail.
                    createImageListHtml(theFile, e, '#partner-photo-list');
                };
            })(f);

            // Read in the image file as a data URL for thumbnail preview
            reader.readAsDataURL(f);
        }
    });

    function createImageListHtml(file, e, appendTo) {
        var fileName = decodeURIComponent(escape(file.name));
        var mb = file.size = (file.size >>> 20) + '.' + (file.size & (2 * 0x3FF));
        var output = [];

        // push everything into our data object
        $scope.data.EncodedImageData.push({ "Filename": fileName, "ImageData": e.target.result });

        output.push(
            '<li class="partner-photo-item" title="', fileName, '">',
            '<div class="partner-photo-title"><strong>', fileName, '</strong></div>',
            '<div>', mb, ' MB', '</div>',
            '<div class="partner-photo-thumb-container">',
            '<img class="partner-photo-thumb" src="', e.target.result, '" title="', fileName, '" />',
            '</div>',
            '</li>'
        );

        $(appendTo).append(output.join(''));
    }
}]);