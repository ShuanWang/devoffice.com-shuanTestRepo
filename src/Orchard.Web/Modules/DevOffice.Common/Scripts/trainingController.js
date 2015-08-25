devOfficeApp.controller('trainingController', ["$scope", "$filter", "$location", function ($scope, $filter, $location) {

    if ($location.search()['filters'] != undefined) {
        if ($location.search()['filters'] == "") {
            $scope.selectedTypes = [];
        } else {
            $scope.selectedTypes = $location.search()['filters'].toLowerCase().split(',');
        }
    } else {
        $scope.selectedTypes = [];
    }
    $scope.itemsToShow = [];
    $scope.itemsPerPage = 15;
    $scope.pagesToShow = 3;
    $scope.currentPage = 1;
    $scope.model = "";
    $scope.showFirstPage = false;
    $scope.showLastPage = false;
    $scope.pagingRange = [];
    $scope.trainingItemsToShow = $scope.model.PatternsAndPractices;
    $scope.pageCount = function () {
        return Math.ceil($scope.trainingItemsToShow.length / $scope.itemsPerPage);
    };
    $scope.sortedByHighToLow = false;
    $scope.sortedByMostRecent = false;
    $scope.showStartEllipsis = false;
    $scope.showEndEllipsis = false;


    $scope.totalTrainingItems = 0;
    $scope.topViewed = [];

    $(window).resize(function () {
        if (window.innerWidth >= 900) {
            $scope.pagesToShow = 5;
            $scope.handlePaging();
        } else {
            $scope.pagesToShow = 3;
            $scope.handlePaging();
        };

        $scope.$apply();

    });


    $scope.orderByViews = function () {

        if (!$scope.sortedByHighToLow) {
            $scope.trainingItemsToShow.sort(function (a, b) {
                return b.ViewCount - a.ViewCount;
            });
            $scope.sortedByHighToLow = true;
            $("#mostPopularIcon").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_down_grey_13x15.png");//update icon to be down arrow
            $("#mostPopularIconxs").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_down_orange_13x15.png");//update icon to be down arrow
        } else {
            $scope.trainingItemsToShow.sort(function (a, b) {
                return a.ViewCount - b.ViewCount;
            });
            $scope.sortedByHighToLow = false;
            $("#mostPopularIcon").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_up_grey_13x15.png");//update icon to be up arrow
            $("#mostPopularIconxs").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_up_orange_13x15.png");
        }
        $(".sorter.popular").addClass("active");
        $(".sorter.recent").removeClass("active");
        $("#mostRecentIcon").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_updown_13x15.png");//update icon to be default double arrow
        $("#mostRecentIconxs").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_updown_13x15.png");//update icon to be default double arrow

        $scope.updatePagination(1);
    };

    $scope.orderByDate = function () {
        if (!$scope.sortedByMostRecent) {
            $scope.trainingItemsToShow.sort(function (a, b) {
                return parseInt(b.DatePublished.substr(6)) - parseInt(a.DatePublished.substr(6));
            });
            $scope.sortedByMostRecent = true;
            $("#mostRecentIcon").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_down_grey_13x15.png");//update icon to be down arrow
            $("#mostRecentIconxs").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_down_orange_13x15.png");//update icon to be down arrow
        } else {
            $scope.trainingItemsToShow.sort(function (a, b) {
                return parseInt(a.DatePublished.substr(6)) - parseInt(b.DatePublished.substr(6));
            });
            $scope.sortedByMostRecent = false;
            $("#mostRecentIcon").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_up_grey_13x15.png");//update icon to be up arrow
            $("#mostRecentIconxs").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_up_orange_13x15.png");

        }
        $(".sorter.recent").addClass("active");
        $(".sorter.popular").removeClass("active");
        $("#mostPopularIcon").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_updown_13x15.png");//update icon to be default double arrow
        $("#mostPopularIconxs").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_updown_13x15.png");//update icon to be default double arrow
        $scope.updatePagination(1);
    };




    $scope.handlePaging = function () {
        $scope.pagingRange = [];
        $scope.showFirstPage = false;
        $scope.showLastPage = false;
        $scope.showStartEllipsis = false;
        $scope.showEndEllipsis = false;
        if ($scope.pageCount() > $scope.pagesToShow) {
            var start = $scope.currentPage - (Math.floor($scope.pagesToShow / 2));
            var end = $scope.currentPage + (Math.floor($scope.pagesToShow / 2));


            while (start < 1) { //you fell off beginning of list
                start++;
                end++;
            }

            while (end > $scope.pageCount()) { //you fell off end of list
                start--;
                end--;
            }

            if (end < $scope.pageCount()) {
                $scope.showLastPage = true;
                if (end < $scope.pageCount() - 1) {
                    $scope.showEndEllipsis = true;
                }
            }
            if (start > 1) {
                $scope.showFirstPage = true;
                if (start > 2) {
                    $scope.showStartEllipsis = true;
                }
            }
            for (var i = start; i <= end; i++)
                $scope.pagingRange.push(i);

        }
        else {
            for (var i = 1; i <= $scope.pageCount() ; i++) {
                $scope.pagingRange.push(i);
            }
        }
    }

    $scope.$watch('model', function () {

        if ($scope.selectedTypes.length == 0) {
            $scope.selectedTypes = ["introduction to office 365 development"];
        }

        $scope.getItemsOfSelectedTypes();
        $scope.currListSlice = $scope.trainingItemsToShow.slice(0, $scope.itemsPerPage);
        $scope.orderByViews();
        $scope.handlePaging();

        for (var i = 0, l = $scope.model.AllTrainingItems.length; i < l; i++) {
            $scope.model.AllTrainingItems[i].TechnicalTitle = $scope.model.AllTrainingItems[i].Title.replace(/ /g, "") + "-DetailLink";
            if ($scope.model.AllTrainingItems[i].ExternalLink.indexOf(window.location.host) != -1 || $scope.model.AllTrainingItems[i].ExternalLink[0] == ('/')) {
                $scope.model.AllTrainingItems[i].External = "";
            } else {
                $scope.model.AllTrainingItems[i].External = "_external";
            }
            //if ($scope.model.TopViewed.indexOf($scope.model.AllTrainingItems[i].Id) != -1) {
            //    $scope.topViewed.push($scope.model.AllTrainingItems[i]);
            //}
        }
        if (window.innerWidth >= 900) {
            $scope.pagesToShow = 5;
            $scope.handlePaging();
        } else {
            $scope.pagesToShow = 3;
            $scope.handlePaging();
        };
        $("#code-sample-social").attr("data-url", window.location.href);
        $("#sharelink-txt").val(window.location.href);


        $scope.sortedItems = $scope.model.AllTrainingItems.sort(function (a, b) {
            return b.ViewCount30Days - a.ViewCount30Days;
        });
        for (var j = 0, k = 9; j < k; j++) {
            $scope.topViewed.push($scope.sortedItems[j]);
        }
    });



    $scope.updateSearchResults = function () {
        $scope.getItemsOfSelectedTypes();
        var filteredItems = $filter('filter')($scope.trainingItemsToShow, $scope.searchText);
        $scope.trainingItemsToShow = filteredItems;
        $scope.updatePagination(1);
    }



    $scope.$watch('searchText', function (val) {
        if (val != undefined) {
            $scope.updateSearchResults();
        }
    });



    //log a page view to the database
    $scope.updateViewCount = function (itemId, type) {
        $scope.date = new Date();
        $.ajax('/devoffice.common/viewCount/post?itemid=' + itemId + '&type=' + type)
            .error(function (err) {
                console.log("Unable to log view for item " + itemId);

            })
            .success(function () {
                //console.log("I DID A THING! " + itemId + ", " + $scope.date);
            });
    };

    $scope.updatePagination = function (pageNumber) {
        $scope.currentPage = pageNumber;
        var startItem = ($scope.currentPage - 1) * $scope.itemsPerPage;
        $scope.currListSlice = $scope.trainingItemsToShow.slice(startItem, startItem + $scope.itemsPerPage);
        $scope.handlePaging();
    }

    $scope.updateSelectedTypes = function (typeName) {
        $scope.selectedTypes = [typeName];
        $location.search('filters', $scope.selectedTypes.join(",")); //add the selected types to the url

        //each time the selected types change, filter the trainingItemsToShow
        $scope.getItemsOfSelectedTypes();
        $scope.updatePagination(1);
        updateSharingUrl();
        $scope.updateSearchResults();


    }

    $scope.getItemsOfSelectedTypes = function () {

        $scope.trainingItemsToShow = [];
        $.each($scope.model.AllTrainingItems, function () {
            var isInSelectedTypes = false;
            $.each(this.TermsTaggedList, function (i, item) {
                if ($scope.selectedTypes.indexOf(item.toLowerCase()) > -1) {
                    isInSelectedTypes = true;
                }

            });
            if (isInSelectedTypes && $scope.trainingItemsToShow.indexOf(this) == -1) {
                $scope.trainingItemsToShow.push(this);
            }
        });
    }

    $scope.clearFilters = function () {
        $scope.selectedTypes = [];
        $scope.updateSearchResults();
        $("input:checkbox").prop('checked', false);

    }

    function getUrlParameter(param) {
        var pageURL = window.location.search.substring(1);
        var urlVariables = pageURL.split('&');
        for (var i = 0; i < urlVariables.length; i++) {
            var parameterName = urlVariables[i].split('=');
            if (parameterName[0] == param) {
                return decodeURIComponent(parameterName[1]).split(",");
            }
        }
        return [];
    }

    function updateSharingUrl() {

        $("#code-sample-social").attr("data-url", window.location.href);
        $("#sharelink-txt").val(window.location.href);

        reloadAddThis();
    }

    $("#sharelink-txt").click(function () {
        if ($(this).val() != "Choose filters and hit apply") {
            $(this).focus();
            $(this).select();
        }
    });

    function reloadAddThis() {
        if (window.addthis) {
            window.addthis = null;
            window._adr = null;
            window._atc = null;
            window._atd = null;
            window._ate = null;
            window._atr = null;
            window._atw = null;
        }
        $.getScript("http://s7.addthis.com/js/300/addthis_widget.js#pubid=ra-53e944d61ebade5f").done(function () {
            addthis.init();
            addthis.toolbox('#code-sample-social');
            $(".social-share-container").show();
        });
    }


}]);
