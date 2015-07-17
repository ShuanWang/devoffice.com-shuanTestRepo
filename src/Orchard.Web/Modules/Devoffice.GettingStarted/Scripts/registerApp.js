function registerAppParams() {
    var clientId = null;
    var clientSecret = null;
    var signonUri = null;
    var redirectUri = null;
}

$(document).ready(function () {
    $('#app-reg-signin').click(function () {
        ga('send', 'event', 'O365path-Rest', 'Signin-ExistingAccount');
        var registrationCardId = "4";
        document.cookie = "current-card=" + registrationCardId + "; path=/";
        window.location.href = "/Account/_SignIn";
    });
});
function registerApp() {
    var appName = $('#appNameField').val();
    var appType = $('#appTypeField').val();
    var signOnUrl = $('#signOnUrlField').val();
    var appIdUri = $('#appIdUriField').val();
    var redirectUri = $('#redirectUriField').val();
    redirect_Uri = redirectUri;
    var includeCalendar = $('#calendarRead').is(':checked');
    var includeContacts = $('#contactsRead').is(':checked');
    var includeMail = $('#mailRead').is(':checked');
    var includeFiles = $('#filesRead').is(':checked');

    var includeCalendarWrite = $('#calendarWrite').is(':checked');
    var includeContactsWrite = $('#contactsWrite').is(':checked');
    var includeMailWrite = $('#mailWrite').is(':checked');
    var includeFilesWrite = $('#filesWrite').is(':checked');

    var includeMailSend = $('#mailSend').is(':checked');

    //do a frontend error check
    if(
        appName =="" || typeof(appName)===undefined ||
        signOnUrl =="" || typeof(signOnUrl)===undefined ||
        appIdUri =="" || typeof(appIdUri)===undefined ||
        redirectUri =="" || typeof(redirectUri)===undefined
        ) {
        $('#reg-error_msg').text("A required field is missing");
        $('#reg-error_display').show();
        return;
    }
    var protocolHttp = appIdUri.substr(0, 7);
    var protocolHttps = appIdUri.substr(0, 8);
    if (protocolHttp != "http://" && protocolHttps != "https://") {
        $('#reg-error_msg').text("Wrong protocol provided in app id Uri, it should be starting with http:// or https://");
        $('#reg-error_display').show();
        return;
    }

    protocolHttp = signOnUrl.substr(0, 7);
    protocolHttps = signOnUrl.substr(0, 8);
    if (protocolHttp != "http://" && protocolHttps != "https://") {
        $('#reg-error_msg').text("Wrong protocol provided in Sign on Url, it should be starting with http:// or https://");
        $('#reg-error_display').show();
        return;
    }
    registerAppParams.signonUri = signOnUrl;
    protocolHttp = redirectUri.substr(0, 7);
    protocolHttps = redirectUri.substr(0, 8);
    if (protocolHttp != "http://" && protocolHttps != "https://") {
        $('#reg-error_msg').text("Wrong protocol provided in Redirect uri, it should be starting with http:// or https://");
        $('#reg-error_display').show();
        return;
    }
    registerAppParams.redirectUri = redirectUri;

    var actionUrl = "/Home/RegisterApp";

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
    }
    $('#register-button').attr("disabled", "disabled");
    $('registration-progress').show();

    $('#reg-error_display').hide(); // make sure you hide the error message, if user does the sencond attempt
    var json = JSON.stringify(param);//
    $.ajax({
        url: actionUrl,
        dataType: "json",
        type: "POST",
        contentType:'application/json;charset-utf-8',
        data: json,
        success: function (data, textStatus, xhr) {
            $('#reg-error_display').hide();
            if (data.client_id != undefined && data.client_id != "") {
                $('#clientIdField').val(data.client_id);
                if (appType == "Native App") {
                    $("#app-reg-client-secret").hide();
                }
                else {
                    $('#clientSecretField').val(data.client_secret);
                }
                $('#registration-result').removeClass('hidden');
                registerAppParams.clientId = data.client_id;
                registerAppParams.clientSecret = data.client_secret;
                cardTracker.removeBlockingCard();
            }
            else {
                $('#reg-error_msg').text(data.error_message);
                $('#reg-error_display').show();
                $('#register-button').removeAttr("disabled");
            }
        },
        error: function (jqXHR, exception) {
            $('registration-progress').hide();
            $('#reg-error_msg').text(jqXHR.responseText);
            $('#reg-error_display').show();
        },
        complete: function (xhr) {
            $('#register-button').removeAttr("disabled");
        }
    })
}

