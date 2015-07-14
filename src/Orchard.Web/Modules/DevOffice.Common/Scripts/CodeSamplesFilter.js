(function ($) {
    $(document).ready(function () {
        $("img.lazy").lazyload();
        $(".filter-category a").click(function (e) {
            e.preventDefault();
            var icon = $(this).find(".filter-category-icon");
            if (icon.hasClass("icon_expand")) {
                $(".filter-category-icon").removeClass("icon_collapse").addClass("icon_expand");
                icon.addClass("icon_collapse").removeClass("icon_expand");
                
            } else {
                $(".filter-category-icon").removeClass("icon_collapse").addClass("icon_expand");

            }

        });
    });
    
})(jQuery);