(function ($) {
    $(document).ready(function () {

        loadMSDNSearchResults();

        $(".search-showmore").click(function (e) {
            e.preventDefault();
            var currentpage = $(this).parent("ul").find(".next-page");
            var currentContentType = $(this).parent("ul").find(".current-contentType");
            showMoreResults(currentpage, currentContentType);
        });

        $(document).on("click", "li.filter-category-item", function(e) {
            $(this).toggleClass("active");
        });

        $("button#clearButton").click(clearFilter);

        $("button#applyButton").click(function (){
            $('#search-filters li').each(function () {
                var contentType = $(this).attr("data-contentType");
                if ($(this).hasClass("active")) {
                    $("#search-results").find('[data-contentTypeSection="' + contentType + '"]').show();
                } else {
                    $("#search-results").find('[data-contentTypeSection="' + contentType + '"]').hide();
                }
            });
        });

        $('#affix').affix({
            offset: {
                top: function () {
                    return (this.top = $('#layout-header').outerHeight(true) + $('.zone-content .banner-image').outerHeight(true));
                }
            }
        });
        
    });
    
    function clearFilter() {
        $("li.filter-category-item.active").removeClass("active");
        $('[data-contentTypeSection]').show();
    }

    function showMoreResults(currentpage, currentContentType) {
        if (currentpage != null) {
            $.ajax({
                type: 'POST',
                url: '/search/listmore',
                data: {
                    q : $("#q").val(),
                    currentPage: $(currentpage).text(),
                    currentContentType: $(currentContentType).text(),
                    __RequestVerificationToken: $("#ForgeryToken").val()
                }
            }).success(function (response) {
                var currentSection = currentpage.parent("ul");

                var searchMoreLink = currentSection.find("a.search-see-more");
                searchMoreLink.before(response);

                var nextPage = parseInt($(currentpage).text(), 10) + 1;
                $(currentpage).text(nextPage);

                showMore(currentSection);

                if (currentSection.find(".pages").text() < $(currentpage).text()) {
                    searchMoreLink.hide();
                }

            });
        }
        //todo: loading animation
    }

    //Default number of items to show - 6
    var index = 4;

    function showMore(currentSection) {
        var hiddenItems = currentSection.find('li.hide');
        hiddenItems.slice(0, index).removeClass("hide").collapse('show');

        var newHiddenItems = currentSection.find('li.hide').length;
        if (newHiddenItems <= 0) {
            $(this).hide();
        }
    }

    function loadMSDNSearchResults() {
        $('[data-rss]').each(function () {
            getFeed(this)
                .done(writeFeed.bind(this));
        });
    }

    
    // Returns a jQueryPromise object which retrieves feed
    function getFeed(element) {
        var $t = $(element),
            url = $t.attr('data-rss');
        
        var postUrl = document.location.protocol + '//ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=20&callback=?&q=' + encodeURIComponent(url);
        return $.getJSON(postUrl);
    }

    // Writes the feed data to the element 'this'
    function writeFeed(data) {
        var $t = $(this),
            html = [];
            url = $t.attr('data-redirect');

        if (data.responseData && data.responseData.feed && data.responseData.feed.entries && data.responseData.feed.entries.length) {
            var entries = data.responseData.feed.entries,
                a = document.createElement('a');
            a.href = entries[0].link;
            var baseUrl = data.responseData.feed.link || a.hostname;

            html.push("<span></span>");
            for (var i = 0; i < entries.length; i++) {
                var e = entries[i];

                var searchResult =
                    '<li class="col-md-6 col-sm-6 event-box-details collapse in">' +
                        '<div class="code-event">' +
                            '<div class="event-info">' +
                                '<div class="code-title">' +
                                    '<div class="event-icon col-xs-4">' +
                                        '<img src="/Themes/DevOffice/Content/Icons/devoffice_msdn_100x39.png" class="img-responsive">' +
                                    '</div>' +
                                    '<div class="col-xs-8 name cp1">' + e.title + '</div>' +
                                    '<div class="col-xs-8 event-links">' +
                    '<a data-bind="attr: { href: Url }" href="'+ e.link + '" target="@external" role="link" class="pull-right ff-semibold">VIEW<img class="see-more-icon podcast-see-more-icon" src="/Themes/DevOffice/Content/Images/seeMoreIcon.png" role="img" alt="see more arrow"> </a>'+

                                    '</div>' +
                                '</div>' +
                            '</div>' +

                            '<div class="register-now">' +
                                '<div class="desc">'+ e.content + '</div>' +
                            '</div>' +

                        '</div>' +
                    '</div></li>';

                html.push(searchResult);
            }
            
            html.push('<div class="row">');
            html.push(' <a href="' + url + '" target="_blank" class="search-see-more col-md-12 col-xs-12 seeMoreMsdnSearchResults">');
            html.push('  See more posts ');
            html.push('  <img class="see-more-icon" src="/Themes/DevOffice/Content/Images/seeMoreIcon.png" />');
            html.push(' </a>');
            html.push('</div>');

            $("#search-filters").append('<li class="filter-category-item" data-contenttype="msdn">MSDN Search Results (Top 20)</li>');
            $(".search-not-found").hide();
        } else {
            html.push('<p>The RSS feed seems to have not pulled any entries.</p>');
        }

        $t.html(html.join(''));
    }

})(jQuery);