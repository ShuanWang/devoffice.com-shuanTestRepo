$(document).ready(function () {

    var piTop = $('h1').offset().top;
    var piBottom = $('#layout-footer').height()+ 35;
    $('#side-nav').affix({
        offset: {
            top: piTop,
            bottom: piBottom
        }
    });

    $('body').scrollspy({ target: '#side-nav' });

    $('#left-side-nav a.training-nav-link:last-of-type').on(click, function() {
        $('#side-nav').addClass('affix-bottom');
        $('#side-nav').removeClass('affix');
    });

});
