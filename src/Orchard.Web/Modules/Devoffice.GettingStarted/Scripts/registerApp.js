
// get the default app id uri, it is required to update the uri based on the name entered by the user
var defaultAppIdUri = $("#appIdUriField").val();
$('#appNameField').addClass('highlight');

function registerAppParams() {
    var clientId = null;
    var clientSecret = null;
    var signonUri = null;
    var redirectUri = null;
}

function checkParameter(selector, errorDivSelector, message) {
    var paramValue = $(selector).val();
    if (paramValue == "" || typeof (paramValue) === undefined) {
        $(errorDivSelector).html(message);
        $('#register-button').attr("disabled", "disabled");
        $(selector).addClass('highlight');
        return undefined;
    }
    var pattern = new RegExp(/^.*?(?=[\^#&$\*<>\?\{\|\}]).*$/);
    if (pattern.test(paramValue)) {
        $(errorDivSelector).html("Parameter contains at least one invalid chars");
        $(selector).addClass('highlight');
        return undefined;
    }
    $(selector).removeClass('highlight');
    return paramValue;
}

function checkProtocol(uri, objectToBeHighlightedSelector, errorDivSelector) {
    if (uri == "" || uri == undefined) {
        return;
    }
    var protocolHttp = uri.substr(0, 7);
    var protocolHttps = uri.substr(0, 8);
    if (protocolHttp != "http://" && protocolHttps != "https://") {
        $(errorDivSelector).text("Wrong protocol provided in app id Uri, it should be starting with http:// or https://");
        $('#register-button').attr("disabled", "disabled");
        $(objectToBeHighlightedSelector).addClass('highlight');
        return false;
    }
    if ($(objectToBeHighlightedSelector).hasClass('highlight')) {
        $(objectToBeHighlightedSelector).removeClass('highlight');
    }
    return true;
}

function hideErrorDiv(selector)
{
    $(selector).html("");
    $('#register-button').removeAttr("disabled");
}

$(document).ready(function () {
    $('#app-reg-signin').click(function () {
        ga('send', 'event', 'O365path-Rest', 'Signin-ExistingAccount');
        var registrationCardId = "register-app";
        document.cookie = "current-card=" + registrationCardId + "; path=/";
        window.location.href = "/GettingStarted/Account/SignIn";
    });

    // app name
    $("#appNameField").focusout(function () {
        var appName = checkParameter("#appNameField", "#app-name-error-div", "Please enter the appname.");
        if (appName != undefined) {
            // update the app id Uri
            var appidUri = $("#appIdUriField").val();
            $("#appIdUriField").val(defaultAppIdUri + appName);
        }
    });
    $("#appNameField").focus(function () {
        hideErrorDiv("#app-name-error-div");
    });

    // signon uri
    $("#signOnUrlField").focusout(function () {
        var signonUri = checkParameter("#signOnUrlField", "#sign-onUrl-error-div", "Please enter the sign on uri.");
        checkProtocol(signonUri, "#signOnUrlField", "#sign-onUrl-error-div");
    });
    $("#signOnUrlField").focus(function () {
        hideErrorDiv("#sign-onUrl-error-div");
    });

    $("#appIdUriField").focusout(function () {
        var appidUri = checkParameter("#appIdUriField", "#app-id-uri-error-div", "Please enter the app id uri.");
        checkProtocol(appidUri, "#appIdUriField", "#app-id-uri-error-div");
    });

    $("#appIdUriField").focus(function () {
        hideErrorDiv("#app-id-uri-error-div");
    });

    $("#redirectUriField").focusout(function () {
        var redirectUri = checkParameter("#redirectUriField", "#redirect-uri-error-div", "Please enter the redirect uri.");
        checkProtocol(redirectUri, "#redirectUriField", "#redirect-uri-error-div");
    });

    $("#redirectUriField").focus(function () {
        hideErrorDiv("#redirect-uri-error-div");
    });
});
function registerApp() {
    $('#reg-error_display').hide(); // make sure you hide the error message, if user does the second attempt
    var appType = $('#appTypeField').val();
    var includeCalendar = $('#calendarRead').is(':checked');
    var includeContacts = $('#contactsRead').is(':checked');
    var includeMail = $('#mailRead').is(':checked');
    var includeFiles = $('#filesRead').is(':checked');

    var includeCalendarWrite = $('#calendarWrite').is(':checked');
    var includeContactsWrite = $('#contactsWrite').is(':checked');
    var includeMailWrite = $('#mailWrite').is(':checked');
    var includeFilesWrite = $('#filesWrite').is(':checked');

    var includeMailSend = $('#mailSend').is(':checked');
    var success = false;
    //do a frontend error check
    var appName = checkParameter("#appNameField", "#app-name-error-div", "Please enter the appname.");
    if (appName == undefined) {
        return;
    }
    var signOnUrl = checkParameter("#signOnUrlField", "#sign-onUrl-error-div", "Please enter the sign on uri.");
    success = checkProtocol(signOnUrl, "#signOnUrlField", "#sign-onUrl-error-div");
    if (success == false) {
        return;
    }
    var appIdUri = checkParameter("#appIdUriField", "#app-id-uri-error-div", "Please enter the app id uri.");
    success = checkProtocol(appIdUri, "#appIdUriField", "#app-id-uri-error-div");
    if (success == false) {
        return;
    }
    var redirectUri = checkParameter("#redirectUriField", "#redirect-uri-error-div", "Please enter the redirect uri.");
    success = checkProtocol(redirectUri, "#redirectUriField", "#redirect-uri-error-div");
    if (success == false) {
        return;
    }
    registerAppParams.signonUri = signOnUrl;
    registerAppParams.redirectUri = redirectUri;

    var actionUrl = "/GettingStarted/AppRegistration/RegisterApp" + getAntiForgeryTokenQuery();

    var param = {
        "appName": appName,
        "appType":appType,
        "signOnUri": signOnUrl,
        "appIdUri": appIdUri,
        "redirectUri": redirectUri,
        "includeCalendar": includeCalendar,
        "includeContacts": includeContacts,
        "includeMail": includeMail,
        "includeFiles": includeFiles,
        "includeCalendarWrite": includeCalendarWrite,
        "includeContactsWrite": includeContactsWrite,
        "includeMailWrite": includeMailWrite,
        "includeFilesWrite": includeFilesWrite,
        "includeMailSend": includeMailSend,
        "appId": registerAppParams.clientId,
    }
    $('#register-button').attr("disabled", "disabled");
    $('#registration-progress').addClass('loading');
    var json = JSON.stringify(param);//
    $.ajax({
        url: actionUrl,
        dataType: "json",
        type: "POST",
        contentType:'application/json;charset-utf-8',
        data: json,
        success: function (data, textStatus, xhr) {
            if (registerAppParams.clientId !=null ) {
                /* update case*/
                if (data.error_message != undefined) {
                    $('#registration-result .ms-font-xl').html("<strong>An error as been occured while updating the app</strong>");
                }
                else {
                    $('#registration-result .ms-font-xl').html("<strong>Application has been updated successfully</strong>");
                }
            }
            else if (data.client_id != undefined && data.client_id != "") {
                $('#clientIdField').val(data.client_id);
                if (appType == "Native App") {
                    $("#app-reg-client-secret").hide();
                }
                else {
                    $('#clientSecretField').val(data.client_secret);
                }
                $('#registration-result').removeClass('hidden');
                $('#registration-result').addClass('animated fadeInUp');

                registerAppParams.clientId = data.client_id;
                registerAppParams.clientSecret = data.client_secret;
                cardTracker.removeBlockingCard();
            }
            else {
                $('#reg-error_msg').text(data.error_message);
                $('#reg-error_display').show();
                $('#reg-error_display').addClass('animated fadeInUp');
            }
        },
        error: function (jqXHR, exception) {
            $('#reg-error_msg').text(jqXHR.responseText);
            $('#reg-error_display').show();
            $('#reg-error_display').addClass('animated fadeInUp');
        },
        complete: function (xhr) {
            $('#registration-progress').removeClass('loading');

            $('#register-button').removeAttr("disabled");
        }
    })
}

