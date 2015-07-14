(function ($) {
    $(document).ready(function () {
       
        var mq = window.matchMedia("(min-width: 540px)");

        if (mq.matches) {
            GetArticles($(".showMoreArticlesButton"), 20);
            $(".showMoreArticlesButtonMobile").hide();
        }
        else {
            GetArticles($(".showMoreArticlesButtonMobile"), 5);
            $(".showMoreArticlesButton").hide();
        }

        $(".showMoreArticlesButton").click(function (e) {
            e.preventDefault();
            GetArticles($(this), 20);
        });

        $(".showMoreArticlesButtonMobile").click(function (e) {
            e.preventDefault();
            GetArticles($(this), 5);
        });

        //have first item in left-side-nav (Podcasts, Training) be active by default
        $('#left-side-nav li:first-child').addClass("active");

        function GetArticles(showMoreButton, numOfArticlesToShow) {
            
            var pageNumber = $(showMoreButton).attr('data-pagenumber');
            var nextPage = parseInt(pageNumber, 10) + 1;
            var totalCount = parseInt($(".totalCount").text(), 10);

            $.ajax({
                type: 'GET',
                url: '/ShowMore/GetMoreArticles/' + nextPage + "/" + numOfArticlesToShow,
                beforeSend: function () {
                    $(".loading").show(); 
                },
                success: function (data) {
                    $(showMoreButton).attr('data-pagenumber', nextPage);
                    $(showMoreButton).before(data);
                    if (nextPage * numOfArticlesToShow >= totalCount) {
                        $(showMoreButton).hide();
                    }
                    $('.pk-ellipsis').ellipsis();
                },
                complete: function () {
                    $(".loading").hide();
                }
            });
        }

    });

})(jQuery);

