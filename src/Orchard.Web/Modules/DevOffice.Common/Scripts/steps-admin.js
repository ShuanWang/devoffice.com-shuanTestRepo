$(document).ready(function () {

    $('body').on('click', 'div.collapsible-header', function () {
        $(this).next('div.collapsible-content').slideToggle();
        $(this).children('div.collapsible-toggle').toggleClass('collapsed');
    });

});