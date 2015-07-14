(function($) {
    $(document).ready(function () {
        fillFeeds();
    });
    
    // Fills each element on the page with the data-rss attribute with the feed located at the value of the data-rss attribute
    function fillFeeds() {
        $('[data-rss]').each(function () {
            getFeed(this)
                .done(writeFeed.bind(this));
        });
    }

    // Returns a jQueryPromise object which retrieves feed
    function getFeed(element) {
        var $t = $(element),
            url = $t.attr('data-rss');

        return $.getJSON(document.location.protocol + '//ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&callback=?&q=' + encodeURIComponent(url));
    }
    
    // Writes the feed data to the element 'this'
    function writeFeed (data) {
        var $t = $(this),
            html = [];
                
        if (data.responseData && data.responseData.feed && data.responseData.feed.entries && data.responseData.feed.entries.length) {
            var entries = data.responseData.feed.entries,
                a = document.createElement('a');
            a.href = entries[0].link;
            var baseUrl = data.responseData.feed.link || a.hostname;

            for (var i = 0; i < entries.length && i < 10; i++) {
                var e = entries[i];
                html.push('<div class="articleStub">');
                html.push('  <div class="article-title">' + e.title + '</div>');
                //html.push('  <p>' + e.contentSnippet + '</p>');
                html.push('  <a href=' + e.link + ' class="read-more" id="TrendingBlogPost-' + e.title.replace(' ', '', 'gi') + '-DetailLink">Read more');
                html.push('     <span class="glyphicon glyphicon-chevron-right"></span>');
                html.push('  </a>');
                html.push('</div>');
            }

            html.push('<div class="row">');
            html.push(' <a href=' + baseUrl + ' target="_blank" class="col-md-12 col-xs-12 see-more-article">');
            html.push('  See more posts ');
            html.push('  <img class="see-more-icon" src="/Themes/DevOffice/Content/Images/seeMoreIcon.png" />');
            html.push(' </a>');
            html.push('</div>');
        } else {
            html.push('<p>The RSS feed seems to have not pulled any entries.</p>');
        }

        $t.html(html.join(''));
    }
    
})(jQuery);