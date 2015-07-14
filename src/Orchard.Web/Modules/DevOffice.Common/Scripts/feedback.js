(function ($) {
    $(document).ready(function () {


        var pks = pks || {};
        pks.app = function () {
            if (document.getElementById("feedbackWidget")) {
                var _feedbackViewModel = pks.feedback();
                ko.applyBindings(_feedbackViewModel, document.getElementById("feedbackWidget"));
            }
        };

        var pks = pks || {};
        pks.feedback = function () {
            var _feedbackView = ko.observable("notSubmitted");
            var _isHelpful = ko.observable();
            var _feedbackContent = ko.observable();
            var _yesClicked = function () {
                _feedbackView("submitting");
                _isHelpful(true);
            };
            var _noClicked = function () {
                _feedbackView("submitting");
                _isHelpful(false);
            };
            var _submitClicked = function () {
                _submitFeedback();
                _feedbackView("submitted");
            };
            var _skipThisClicked = function () {
                _feedbackContent("");
                _submitFeedback();
                _feedbackView("submitted");
            };

            var _submitFeedback = function () {
                var _feedbackItem = {
                    Id: 0,
                    IsHelpful: _isHelpful(),
                    feedbackContent: _feedbackContent(),
                    __RequestVerificationToken: $("#feedbackWidget input[name='__RequestVerificationToken']").val(),
                    DateCreated: new Date()
                };

                $.ajax({
                    url: '/Feedback/PostFeedback',
                    type: 'POST',
                    data: _feedbackItem,
                    success: function (data, textStatus, jqXhr) {

                    },
                    error: function (jqXhr, textStatus, errorThrown) {

                    }
                });

            }
            return {
                yesClicked: _yesClicked,
                noClicked: _noClicked,
                feedbackView: _feedbackView,
                submitClicked: _submitClicked,
                skipThisClicked: _skipThisClicked,
                feedbackContent: _feedbackContent
            }
        };

    });
})(jQuery);