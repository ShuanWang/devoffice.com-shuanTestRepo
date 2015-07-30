var foregeryTokenName = '__RequestVerificationToken';

function AddAntiForgeryToken(data) {
    data.__RequestVerificationToken = getAntiForgeryToken();
    return data;
};

function getAntiForgeryTokenQuery() {
    return '?__RequestVerificationToken=' + getAntiForgeryToken();
}

function getAntiForgeryToken()
{
    var forgeryToken = getCookie(foregeryTokenName);
    if (forgeryToken == "") {
        forgeryToken = $('#o365ForgeryToken').val();
    }
    return antiForgeryToken; // coming from main page
}

function getCookie(cookieName) {
    var name = cookieName + "=";
    var cookieItems = document.cookie.split(';');
    for (var i = 0; i < cookieItems.length; ++i) {
        var cookieItem = cookieItems[i];
        while (cookieItem.charAt(0) == ' ')
            cookieItem = cookieItem.substring(1);
        if (cookieItem.indexOf(name) == 0)
            return cookieItem.substring(name.length, cookieItem.length);
    }
    return "";
}