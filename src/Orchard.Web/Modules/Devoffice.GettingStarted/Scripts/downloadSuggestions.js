
var suggestionData = "";
function getSuggestions(platform, divId) {
    $.ajax({
        url: "/GettingStarted/main/CodeSampleSuggestions/"+ platform,
        type: "GET",
        dataType: 'json',
        success: function (result, status, xhr) {
            showSuggestions(result, divId);
        },
        error: function (jqXHR, exception) {
            alert("Holy Crap!!! error encountered");
        }
    })
}

function showSuggestions(suggestionData, divId) {
    if (suggestionData.length <= 0) {
        alert("No Suggestion found");
    }
    var innerHtml = "";
    for (var i = 0; i < suggestionData.length; ++i) {
        innerHtml += "<li>" + getHtml(suggestionData[i]) +"</li>";
    }
    $("#" + divId).html("<ul class='panel-collection'>" +innerHtml + "</ul>");
}

var imageIcon = "https://developers.google.com/_static/ed257da1f5/images/redesign-14/platform-ios.svg";

function getHtml(data) {
    return "<div class='panel panel-default text-center panel-download'>" +
                "<div class='panel-heading'>" +
                    "<div class='panel-title'>"+ data.Name+"</div>" +
                "</div>" +
               "<div class='panel-body'>" +
                  data.Description +
               "</div>" +
                "<div class='panel-footer'>" + "<a class='btn btn-success'>" + "<i class='fa fa-download'></i> Download" + "</a>" + "</div>" +
            "</div>";
    var x = getHeading(data.Name) + getBody(data.Description) + getFooter(data.DownloadLink);
    x = "<div class = 'panel panel-default'" + x + "</div>";
    return x;
}

function getHeading(title) {
    var x = "<div class='panel-heading'>" + "<h3 class='panel-title'>" + title + "</h3></div>";
    return x;
}

function getBody(bodyText) {
    var x = "<div class='panel-body'>" + bodyText + "</div>";
    return x;
}

function getFooter(footerText) {
    var x = "<div class='panel-footer'>" + "<button>" + "Download" + "</button>" + "</div>";
    return x;
}