(function ($) {
    $(document).ready(function () {

        setColumnHeights();
        adjustForFeaturedImage();

        //to ensure that the github anchor tags work in our solution
        //GITHUB ANCHOR TAGS
        function scrollToHash() {
            if (location.hash && !document.querySelector(":target")) {
                var href = location.hash;
                //var elements = document.getElementsByName('user-content-' + location.hash.slice(1));
                var elements = $('a[href^="'+ href + '"]');
                if (elements.length > 0) {
                    elements[elements.length - 1].scrollIntoView();
                }
            }
        }
        window.onhashchange = function () {
            scrollToHash();
        }
        window.onload = function () {
            scrollToHash();
        }
        //END GITHUB ANCHOR TAGS

        //set timeout for resize event
        $(window).resize(function () {
            if (this.resizeTO) clearTimeout(this.resizeTO);
            this.resizeTO = setTimeout(function () {
                $(this).trigger('resizeEnd');
            }, 200);
        });

        $(window).bind('resizeEnd', function () {
            adjustForFeaturedImage();
            setColumnHeights();
        });

        //show active tab by hash URL (Getting Started and Showcase pages)
        var hash = window.location.hash.toLowerCase();
        hash && $('ul.nav a[href="' + hash + '"]').tab('show');
    

        //show the tab content when the sub tab is clicked (Getting Started)
        $(".firstblock-subtab").click(function() {
            var tabId = $(this).attr("href");
            $('ul.nav a[href="' + tabId + '"]').tab('show');
        });

        //Default number of items to show - 6 (For Events / Training / Videos / Podcasts)
        var index = 6;

        $('.eventsWidget ul').each(function () {
            $(this).find('li:lt(' + (index) + ')').removeClass("hide").collapse('show');
            $(this).find('li:gt(' + (index - 1) + ')').addClass("hide");
            if ($(this).find('li.hide').length <= 0 || $(this).find('li').length <= 0) {
                $(this).find('.see-more').hide();
            }
        });

        //Click see more to see 6 more items
        $('.showMoreButton').click(function (e) {
            e.preventDefault();
            var listRow = $(this).parent().parent().parent();

            var hiddenItems = listRow.find('ul li.hide');
            hiddenItems.slice(0, index).removeClass("hide").collapse('show');

            var newHiddenItems = listRow.find('ul li.hide').length;
            if (newHiddenItems <= 0) {
                $(this).hide();
            }
        });

        // subnav
        $(".dropdown>a").on("click", function (e) {
            e.preventDefault();
            if ($(this).parent().hasClass("open")) { 
                $(this).parent().removeClass("open");
                $(this).parent().find("#arrow").removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
            } else {
                //only allow one dropdown open at a time
                $(".dropdown").removeClass("open");
                $(".dropdown").find("#arrow").removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
                $(this).parent().addClass("open");
                $(this).parent().find("#arrow").removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
            }
        });

        // video popup
        var $vModal = $('#videoModal'),
        $vContainer = $('#videoContainer'),
        $body = $('body');

        $(document).on("click",".js-video-tile", function (e) {
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

        // scroll filters on left with the page
        $('#affix').affix({
            offset: {
                top: function() {
                    return (this.top = $('#layout-header').outerHeight(true) + $('#content').outerHeight(true));
                }
            }
        });

        // popup share link on filters pages
        $('body').popover({
            selector: '[data-toggle="popover"]',
            trigger:'hover',
            animation: false
        });

        // display share link textarea on click of share link
        //$(".sharelink-container").hide();
        //$(document).on("click", "#shareUrl", function (e) {
        //    e.preventDefault();
        //    $(".sharelink-container").toggle();

        //    if ($(".sharelink-container").is(':visible')) {
        //        $("#sharelink-txt").click();
        //    }
        //});

        $(document).on("click", ".share-email", function (e) {
            e.preventDefault();
            var url = $("#sharelink-txt").html();
            var destination = 'mailto:?subject=Code Samples' + '&body=' + encodeURIComponent(url);
            window.location.href = destination;
        });

        //set top item in left-side scrolling nav to active by default (Podcasts, Training)
        $('#left-side-nav li:first-child').addClass('active');

        //enable "back to top" button on mobile for pages with filters
        if (($(window).height() + 100) < $(document).height()) {
            $('#top-link-block').removeClass('hidden').affix({
                // how far to scroll down before link "slides" into view
                offset: { top: 600 }
            });
        }

        var maxHeight = 0;

        //set the maxHeight based on the tallest of the four columns on the homepage.
        function getMaxHeight() {
            $('.home-widget').each(function () {
                
                if ($(this).height() > maxHeight) {
                    maxHeight = $(this).height();
                }
            });
        }

        // Resizing widget columns on homepage to make them uniform.
        function setColumnHeights() {
            if (window.innerWidth < 1200) {       //remove set heights if you resize to smaller window
                $('.home-widget').css("height", "");
            }
            else {      //...or set column heights if you resize to be in a larger window
                if (maxHeight == 0) { getMaxHeight(); }
                $('.home-widget').height(maxHeight);
            }
        }

        // if there's a featured item image in the banner, adjust container size accordingly
        function adjustForFeaturedImage() {

            if ($('.featuredItemImage') && (window.innerWidth > 699)) {
                var bannerTextBox = $('.featuredItemImage').closest('.featureTextBox');
                bannerTextBox.css('width', 400);
            }
            if ($('.featuredItemImage') && (window.innerWidth <= 699)) {
                var bannerTextBox = $('.featuredItemImage').closest('.featureTextBox');
                bannerTextBox.css('width', '');
            }
        }
       
        if (window.innerWidth >= 1200) {      //set column heights if you're in large window
                getMaxHeight();
               $('.home-widget').height(maxHeight);
        }

    });





})(jQuery);

