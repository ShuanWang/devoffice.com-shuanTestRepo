(function ($) {
    $(document).ready(function () {

        $("li.filter-category-item").click(function () {

            $(this).toggleClass("active");

            var filterListCount = $(this).parent().parent().find(".filter-list-count");
            var filterCategory = $(this).parent().parent().find("h3");

            //Increment / decrement the filter count
            if ($(this).hasClass("active")) {
                filterListCount.text(Number(filterListCount.text()) + 1);
            } else {
                filterListCount.text(Number(filterListCount.text()) - 1);
            }

            //show / hide the filter count
            if (filterListCount.text() <= 0) {
                filterListCount.hide();
                filterCategory.removeClass("active");
            } else {
                filterListCount.show();
                filterCategory.addClass("active");
            }
        });

        $("button#solution-clearfilter").click(function(e) {
            e.preventDefault();
            clearFilter();
        } );

        $("button#solution-applyfilter").click(function(e) {
             e.preventDefault(); runFilter();
        });

        $(".filter-category a").click(function () {
            $(".filter-category-icon").addClass("icon_expand").removeClass("icon_collapse");
            var icon = $(this).find(".filter-category-icon");
            icon.removeClass("icon_expand").addClass("icon_collapse");
        });

    });

    function clearFilter() {
        //$(".solution-swipe-panel").unslick();
        $("li.filter-category-item.active").removeClass("active"); //De-selecting all selected filters
        $(".filter-list-count").text(0).hide(); //Resetting filter count to 0 and hiding them all
        $(".filter-list-container h3").removeClass("active"); //Removing active class on all filter categories
        $("#tinderesque-solutions").html('').hide();
        //$(".tab-details").hide();
        $("#solutions-three-columns").show();
        //runFilter();
        //retrieveAllSolutions();
    }

    function retrieveAllSolutions() {


        var activeFilters = $("li.filter-category-item.active");

        var filters = [];

        if (activeFilters.length > 0) {
            activeFilters.each(function () { filters.push($.trim($(this).text())); });
        }

        $.ajax({
            type: 'POST',
            url: '/SolutionsFilter/GetSolutionsForVertical',
            data: {
                filters: JSON.stringify(filters),
                __RequestVerificationToken: $("#ForgeryToken").val()
            }
        }).done(function (data) {
            $("#solutions-three-columns").hide();

            $("#tinderesque-solutions").html(data).show();
            //$("#accordion .filter-list > ul").removeClass("in");
            $("#onebyone").addClass("active");
            $(".filter-category-icon").addClass("icon_expand").removeClass("icon_collapse");
            $(".tab-details").show();
            $('body,html').animate({ scrollTop: $("#accordion").offset().top }, 500);
        });
    }
    
    function runFilter() {

        var activeFilters = $("li.filter-category-item.active");

        var filters = [];

        if (activeFilters.length > 0) {
            activeFilters.each(function() { filters.push($.trim($(this).text())); });
        }

        $.ajax({
            type: 'POST',
            url: '/SolutionsFilter/GetSolutionsForHorizontal',
            data: {
                filters: JSON.stringify(filters),
                __RequestVerificationToken: $("#ForgeryToken").val()
            }
        }).done(function (data) {

            $("#solutions-three-columns").hide();

            $("#tinderesque-solutions").html(data).show();
            //$("#accordion .filter-list > ul").removeClass("in");
            $("#onebyone").addClass("active");
            $(".filter-category-icon").addClass("icon_expand").removeClass("icon_collapse");

            var slideCount = $(".solution-swipe-panel").length;

            if (slideCount == 1) {
                $("#tinderesque-solutions").addClass("single-slide");
                $("#results-pane").addClass("single-slide");
                $("#tinderesque-container").slick({
                    adaptiveHeight: true,
                    onInit: getPagingInfo
                });
            } else {
                $("#tinderesque-solutions").removeClass("single-slide");
                $("#results-pane").removeClass("single-slide");
                $("#tinderesque-container").slick({
                    slidesToShow: 1,
                    adaptiveHeight: false,
                    infinite: true,
                    slidesToScroll: 1,
                    centerMode: true,
                    centerPadding: '200px',
                    arrows: true,
                    onInit: getPagingInfo,
                    onAfterChange: getPagingInfo,
                    nextArrow: '<button type="button" class="slick-next">Next</button>',
                    prevArrow: '<button type="button" class="slick-prev">Previous</button>',
                    responsive: [
                        {
                            breakpoint: 1199,
                            settings: {
                                slidesToShow: 3,
                                slidesToScroll: 3,
                                centerPadding: '0px'
                            }
                        },
                        {
                            breakpoint: 679,
                            settings: {
                                slidesToShow: 1,
                                slidesToScroll: 1,
                                centerPadding: '0px'
                            }
                        }
                ]
                });

            }

            function getPagingInfo(slick) {
                var $pagingInfo = $("#results-pane");
                if (slick.slideCount == 1) {
                    $pagingInfo.text("1 of " + slick.slideCount + " result");
                } else {
                    $pagingInfo.text((slick.currentSlide + 1) + " of " + slick.slideCount + " results");
                }
                
            }

            $('body,html').animate({ scrollTop: $("#accordion").offset().top }, 500);

        });

        //todo: loading animation
    }

    $('#allresults').click(function (e) {
        e.preventDefault();
        $(this).parent().addClass("active");

        $(this).addClass("solutionViewActive");
        $("#onebyone").removeClass("solutionViewActive");
        $("#onebyone").parent().removeClass("active");
        $("#solutions-three-columns").hide();
        
        //clearFilter();
        retrieveAllSolutions();
    });

    $("#onebyone").click(function (e) {
        e.preventDefault();
        $('.solutions-box').remove(e);
        $(this).addClass("solutionViewActive");
        $(this).parent().addClass("active");
        $("#allresults").removeClass("solutionViewActive");
        $("#tinderesque-container").remove();

        $("#allresults").parent().removeClass("active");
        $("#solutions-three-columns").hide();
        runFilter();

    });
    
})(jQuery);
