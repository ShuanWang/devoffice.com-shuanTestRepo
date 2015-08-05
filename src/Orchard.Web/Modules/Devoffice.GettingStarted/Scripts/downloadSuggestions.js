
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
        innerHtml += getHtml(suggestionData[i]);
    }
    $("#" + divId).html("<ul class='panel-collection'>" + innerHtml + "</ul");
}

var imageIcon = "https://developers.google.com/_static/ed257da1f5/images/redesign-14/platform-ios.svg";

function getHtml(data) {
    return "<li><div class='panel'>" + data.Name+"</div></li>";
}
