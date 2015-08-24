devOfficeApp.controller('codeSamplesController', ["$scope", "$filter", "$location", "$timeout", function ($scope, $filter, $location, $timeout) {


    if ($location.search()['filters'] != undefined) {
        if ($location.search()['filters'] == "") {
            $scope.selectedTypes = [];
        } else {
            $scope.selectedTypes = $location.search()['filters'].toLowerCase().split(',');
        }
    } else {
        $scope.selectedTypes = [];
    }


    //window.location.pathname = "/code-samples#";
    $scope.itemsToShow = [];
    $scope.itemsPerPage = 15;
    $scope.pagesToShow = 3;
    $scope.currentPage = 1;
    $scope.model = "";
    $scope.topViewedCodeSamples = [];
    $scope.showFirstPage = false;
    $scope.showLastPage = false;
    $scope.pagingRange = [];
    $scope.codeSamplesToShow = $scope.model.CodeSamples;
    $scope.pageCount = function () {
        return Math.ceil($scope.codeSamplesToShow.length / $scope.itemsPerPage);
    };
    $scope.sortedByHighToLow = false;
    $scope.sortedByMostRecent = false;
    $scope.showStartEllipsis = false;
    $scope.showEndEllipsis = false;
    $scope.selectedTypesCount = 0;
    $scope.selectedLanguagesCount = 0;
    $scope.selectedServicesCount = 0;
    $scope.selectedSourcesCount = 0;
    $scope.selectedPlatformsCount = 0;

    $timeout(function () {
        $scope.updateFilterCounts();
    }, 1000);

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
            $scope.codeSamplesToShow.sort(function (a, b) {
                return b.ViewCount - a.ViewCount;
            });
            $scope.sortedByHighToLow = true;
            $("#mostPopularIcon").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_down_grey_13x15.png");//update icon to be down arrow
            $("#mostPopularIconxs").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_down_orange_13x15.png");//update icon to be down arrow
        } else {
            $scope.codeSamplesToShow.sort(function (a, b) {
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
            $scope.codeSamplesToShow.sort(function (a, b) {
                return parseInt(b.DatePublished.substr(6)) - parseInt(a.DatePublished.substr(6));
            });
            $scope.sortedByMostRecent = true;
            $("#mostRecentIcon").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_down_grey_13x15.png");//update icon to be down arrow
            $("#mostRecentIconxs").attr("src", "/Themes/DevOffice/Content/Icons/devOffice_sort_down_orange_13x15.png");//update icon to be down arrow
        } else {
            $scope.codeSamplesToShow.sort(function (a, b) {
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
            for (var j = 1; j <= $scope.pageCount() ; j++) {
                $scope.pagingRange.push(j);
            }
        }

    }

    $scope.$watch('model', function () {

        if ($scope.codeSamplesToShow.length == 0) {
            $scope.codeSamplesToShow = $scope.model.CodeSamples;
        }
        $scope.currListSlice = $scope.codeSamplesToShow.slice(0, $scope.itemsPerPage);
        $scope.orderByViews();
        $scope.handlePaging();

        for (var i = 0, l = $scope.model.CodeSamples.length; i < l; i++) {
            $scope.model.CodeSamples[i].TechnicalTitle = $scope.model.CodeSamples[i].Title.replace(/ /g, "") + "-DetailLink";
            $scope.model.CodeSamples[i].CleanDate = parseInt($scope.model.CodeSamples[i].DatePublished.substr(6));
            if ($scope.model.CodeSamples[i].ExternalLink.indexOf(window.location.host) != -1 || $scope.model.CodeSamples[i].ExternalLink.indexOf('code-samples') === 0) {
                $scope.model.CodeSamples[i].External = "";
            } else {
                $scope.model.CodeSamples[i].External = "_external";
            }
            //if ($scope.model.TopViewed.indexOf($scope.model.CodeSamples[i].Id) != -1) {
            //    $scope.topViewedCodeSamples.push($scope.model.CodeSamples[i]);
            //}
        }

        if (window.innerWidth >= 900) { $scope.pagesToShow = 5; }
        else { $scope.pagesToShow = 3; };
        $scope.handlePaging();

        $("#code-sample-social").attr("data-url", window.location.href);
        $("#sharelink-txt").val(window.location.href);

        $scope.sortedItems = $scope.model.CodeSamples.sort(function (a, b) {
            return b.ViewCount30Days - a.ViewCount30Days;
        });
        for (var j = 0, k = 9; j < k; j++) {
            $scope.topViewedCodeSamples.push($scope.sortedItems[j]);
        }

    });

    $scope.updateSearchResults = function () {
        $scope.getCodeSamplesOfSelectedTypes();
        var filteredCodeSamples = $filter('filter')($scope.codeSamplesToShow, $scope.searchText);
        $scope.codeSamplesToShow = filteredCodeSamples;
        $scope.updatePagination(1);
    }

    $scope.$watch('searchText', function () {
        $scope.updateSearchResults();
    });


    //log a page view to the database
    $scope.updateViewCount = function (itemId, type) {
        $scope.date = new Date();
        $.ajax('/devoffice.common/viewCount/post?itemid=' + itemId + '&type=' + type)
            .error(function (err) {
                console.log("Unable to log view for item " + itemId);

            })
            .success(function () {
            });
    };

    $scope.updatePagination = function (pageNumber) {
        $scope.currentPage = pageNumber;
        var startItem = ($scope.currentPage - 1) * $scope.itemsPerPage;
        $scope.currListSlice = $scope.codeSamplesToShow.slice(startItem, startItem + $scope.itemsPerPage);
        $scope.handlePaging();
    }

    $scope.updateSelectedTypes = function (typeName) {
        var idx = $scope.selectedTypes.indexOf(typeName.toLowerCase());
        // is currently selected
        if (idx > -1) { $scope.selectedTypes.splice(idx, 1); } // is newly selected
        else { $scope.selectedTypes.push(typeName.toLowerCase()); }

        $location.search('filters', $scope.selectedTypes.join(",")); //add the selected types to the url
        $scope.$apply();
        $scope.getCodeSamplesOfSelectedTypes();
        $scope.updatePagination(1);
        updateSharingUrl();
        $scope.updateFilterCounts();
    }

    $scope.getCodeSamplesOfSelectedTypes = function () {
        if ($scope.selectedTypes.length == 0) {
            $scope.codeSamplesToShow = $scope.model.CodeSamples;
        } else {
            $scope.codeSamplesToShow = [];
        }

        $.each($scope.model.CodeSamples, function () {
            var isInSelectedTypes = false;
            $.each(this.TermTypes, function (i, codeSample) {
                if ($scope.selectedTypes.indexOf(codeSample) > -1) {
                    isInSelectedTypes = true;
                }

            });
            if (isInSelectedTypes && $scope.codeSamplesToShow.indexOf(this) == -1) {
                $scope.codeSamplesToShow.push(this);
            }
        });
    }

    $scope.clearFilters = function () {
        $scope.selectedTypes = [];
        $scope.updateSearchResults();
        $("input:checkbox").prop('checked', false);
        $scope.updateFilterCounts();
        $location.search('filters', $scope.selectedTypes.join(","));
    }

    $scope.updateFilterCounts = function () {
        $scope.selectedLanguagesCount = $('#languages input:checkbox:checked').length;
        $scope.selectedProductsCount = $('#products input:checkbox:checked').length;
        $scope.selectedSourcesCount = $('#sources input:checkbox:checked').length;
        $scope.selectedPlatformsCount = $('#platforms input:checkbox:checked').length;
        $scope.selectedServicesCount = $('#services input:checkbox:checked').length;
        $scope.selectedTypesCount = $('#types input:checkbox:checked').length;
    }

    $scope.$on('$viewContentLoaded', function () {
        $scope.updateFilterCounts();
    });

    function updateSharingUrl() {

        $("#code-sample-social").attr("data-url", window.location.href);
        $("#sharelink-txt").val(window.location.href);

        reloadAddThis();
    }

    $(".share-button").click(function (e) {
        e.preventDefault();
        if ($(".sharelink-container").css("display") == "none") {
            $('.sharelink-container').css("display", "block");
        } else {
            $('.sharelink-container').css("display", "none");

        }
        if ($(this).val() != "Choose filters and hit apply") {
            $(this).focus();
            $(this).select();
            updateSharingUrl();
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
