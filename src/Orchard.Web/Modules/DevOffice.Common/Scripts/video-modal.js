(function ($) {
    $(document).ready(function() {
        var $vModal = $('#videoModal'),
        $vContainer = $('#videoContainer'),
        $body = $('body');

        $('.js-video-tile').click(function (e) {
            e.preventDefault();
            $vContainer.html($('#' + $(this).attr('data-videoid') + '_embed').html());
            $vModal.show();
            $body.addClass('modal-open'); // Stops scroll while modal is up
        });
        $vModal.click(function (e) {
            e.preventDefault();
            $vContainer.html('');
            $vModal.hide();
            $body.removeClass('modal-open'); // Allows scoll after modal is closed
        });
        $vContainer.click(function (e) {
            e.preventDefault();
            e.preventBubble();
        });
    });
})(jQuery);