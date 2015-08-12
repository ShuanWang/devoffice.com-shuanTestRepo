
function TryItOut(elemIDs, elemClasses) {

    var serviceEndpointUris_user;
    var data = [
        {
            "name": "Mail API: Get messages",
            "serviceName":"Mail",
            "serviceEndPointUri": "https://outlook.office365.com/api/v1.0",
            "urlpart": "/me/folders/{0}/messages",
            "serverAction": "/GettingStarted/Proxy/EMail",
            "authToken": "Bearer {token:https://outlook.office365.com/}",
            "parameters": [
                {
                    "Name": "folder_id",
                    "Type": "string",
                    "Value": "sentitems",
                    "Notes": "The target folder ID or well-known name: Inbox, SentItems, Drafts, or DeletedItems."
                }
                ] // end parameters
        }, // end of item
        {
            "name": "Calendar API: Get events",
            "serviceName": "Calendar",
            "serviceEndPointUri": "https://outlook.office365.com/api/v1.0",
            "urlpart": "/me/events",
            "serverAction": "/GettingStarted/Proxy/Events",
            "authToken": "Bearer {token:https://outlook.office365.com/}",
            "parameters": [
            ] // end parameters
        }, // end of item
        {
            "name": "Contacts API: Get all contacts",
            "serviceName": "Contacts",
            "serviceEndPointUri": "https://outlook.office365.com/api/v1.0",
            "urlpart": "/me/contacts",
            "serverAction": "/GettingStarted/Proxy/Contacts",
            "authToken": "Bearer {token:https://outlook.office365.com/}",
            "parameters": [
            ] // end parameters
        }, // end of item
        {
            "name": "Files API: List folder contents",
            "serviceName": "MyFiles",
            "serviceEndPointUri": "https://a830edad9050849NDA1-my.sharepoint.com/_api/v1.0/me",
            "urlpart": "/files/getByPath('{0}')/children",
            "serverAction": "/GettingStarted/Proxy/ListFolderContents",
            "authToken": "Bearer {token:https://a830edad9050849NDA1-my.sharepoint.com/}",
            "parameters": [
                {
                    "Name": "folder-path",
                    "Type": "string",
                    "Value": "Shared%20with%20Everyone",
                    "Notes": "Path of the folder."
                }
            ] // end parameters
        },

    ];

    var elementIDs = elemIDs;
    var elementClasses = elemClasses;
    var serviceMenu = $("#" + elementIDs.services);
    var urlValueElem = $("#" + elementIDs.urlValue);
    var responseBodyElem = $("#" + elementIDs.responseBody);
    var parameterDetailsElem = $("#" + elementIDs.parameterDetails);
    var invokeUrlBtnElem = $("#" + elementIDs.invokeurlBtn);
    var responseCodeContainer = $("." + elementClasses.CodeContainer);
    var sandBoxOptions = $("#" + elementIDs.sandBoxOptions);
    ShowTryItOutServiceMenu(serviceMenu);

    function ShowTryItOutServiceMenu(menuSelector)
    {
        // build product menu
        var appHtml = "";
        for (var i = 0; i < data.length; ++i) {
            var valueHtml = "value = " + i;
            appHtml += "<option value ='" + i + "'>" + data[i]["name"] + "</option>";
        }

        menuSelector.html(appHtml);
    }

    // we need to register events with our handlers

    // register handler on primary menu change
    serviceMenu.on("change", function (event) {
        updateParams();
    });

    // register handler on secondary menu change
    sandBoxOptions.on("change", function (event) {
        updateParams();
    });

    // register handler on clicking the try button
    invokeUrlBtnElem.on("click", function () {
        var url = urlValueElem.text();
        responseBodyElem.text("");
        var dataType = sandBoxOptions.val();
        if (dataType === "sample_data") {
            invokeUrlOnSampleData(url);
        }
        else {
            invokeUrlOnUserData();
        }
        ga('send', 'event', 'O365path-Rest', 'TryOut-' + dataType + '-' + data[serviceMenu.val()]["name"]);
    })

    // Updates the parameter based on menu selection
    // it should be called every time any menu changes happens
    function updateParams() {
        buildParameterTable(parameterDetailsElem);
        $(responseBodyElem).text("");
        $("#tryError").html("");
        updateUrl();
        $("#invokeurlBtn").prop("disabled", false).css("cursor", "pointer");
    }

    function updateUrl() {
        var url = GetUrl(true);
        urlValueElem.html(url);
    }
    // Returns the the URL based on the menu selection
    // @useDefault: true=> uses default parameter values
    // @useDefault: false=> uses user edited parameter values
    function GetUrl(useDefault) {
        var p = serviceMenu.val();
        var dataType = sandBoxOptions.val();
        var json = data[p];
        var serviceEndpoint;
        if (dataType === "sample_data") {
            serviceEndpoint = json["serviceEndPointUri"];
        }
        else {
            /* Get the service end points*/
            if (serviceEndpointUris_user == null) {
                getServiceEndPoint();
            }

            /* since get service end point being async call, we might have not recieved all teh end points*/
            if (serviceEndpointUris_user != null) {
                serviceEndpoint = serviceEndpointUris_user[json["serviceName"]];
            }
        }

        var url = serviceEndpoint + json["urlpart"];
        if (json["parameters"].length > 0) {
            var paramValue = $("#paramTable .textbox").val();
            if (paramValue === undefined || paramValue == null || paramValue == "")
            {
                //TBD: hacky code, currently  it assumes that we are using only one parameter
                paramValue = json["parameters"][0]["Value"];
            }
            url = url.replace("{0}",paramValue);
        }
        return url;
    }

    // builds the parameter table based on the selected menus
    function buildParameterTable(tableSelector)
    {
        var p = serviceMenu.val();
        var parameters = data[p]["parameters"];
        if (parameters.length === 0)
        {
            $(tableSelector).empty();
            return;
        }
        //build header
        var headerHtml = "<thead><td>Name</td><td>Type</td><td>Value</td><td>Notes</td></thead>";
        //build rows
        var rowHtml = "";
        for (var i = 0; i < parameters.length; ++i) {
            var cell1 = "<td>" + parameters[i]["Name"] + "</td>";
            var cell2 = "<td>" + parameters[i]["Type"] + "</td>";
            var cell3 = "<td>" + "<input class='textbox' type='text' value= " + parameters[i]["Value"] + " />"+ "</td>";
            var cell4 = "<td>" + parameters[i]["Notes"] + "</td>";
            rowHtml += "<tr>" + cell1 + cell2 + cell3+cell4+"</tr>";
        }
        var html = "<table id='paramTable' class='table'>" + headerHtml + rowHtml + "</table>";
        var tab = $(tableSelector);
        tab.empty();
        tab.append(html);

        // add error handler
        $("#paramTable .textbox").focusout(function () {
            var paramValue = $("#paramTable .textbox").val();
            if(paramValue =="" || typeof(paramValue)===undefined) {
                $("#tryError").html("Please enter a parameter");
                $("#invokeurlBtn").prop("disabled", true).css("cursor", "default");
                return;
            }
            updateUrl();
            var pattern = new RegExp(/^.*?(?=[\^#&$\*:<>\?/\{\|\}]).*$/);
            if(pattern.test(paramValue)) {
                $("#tryError").html("<bold>parameter contains at least one invalid chars</bold>");
                $("#invokeurlBtn").prop("disabled", true).css("cursor", "default");
            }
        });

        $("#paramTable .textbox").focus(function () {
            $("#tryError").html("");
            $("#invokeurlBtn").prop("disabled", false).css("cursor", "pointer");
            return;
        });
    }

    function getServiceEndPoint()
    {
        var msgHolder = $("#tryError");
        //msgHolder.addClass('loading');;
        msgHolder.html("Getting the service endpoint...");
        ga('send', 'event', 'O365path-Rest', 'Try-it-out-GetServiceEndPoint-Begin');
        $.ajax({
            url: "/gettingstarted/proxy/EndPoints",
            async:false,
            type: 'GET',
            success: function (data, textStatus, xhr) {
                msgHolder.html("");
                serviceEndpointUris_user = data;
                ga('send', 'event', 'O365path-Rest', 'Try-it-out-GetServiceEndPoint-Success');
            },
            error: function (jqXHR, exception) {
                msgHolder.html("<div class='ms-font-color-error ms-font-m'>Encountered error while requesting service endpoint, Please login and try again</div>");
                ga('send', 'event', 'O365path-Rest', 'Try-it-out-GetServiceEndPoint-Error-' + jqXHR.responseText);
            },
            complete: function (xhr) {
            }
        });
    }

    function invokeUrlOnUserData()
    {
        $("#response-container").show("slow");
        invokeUrlBtnElem.prop("disabled", true).css("cursor", "default");
        responseCodeContainer.addClass('loading');

        var p = serviceMenu.val();
        var json = data[p];
        var controller = json["serverAction"];
        var param = $("#paramTable .textbox").val();
        if (typeof (param) != 'undefined')
        {
            controller += ("/" + param);
        }

        var requestHeaders = JSON.parse('{}');
        requestHeaders['Accept'] = 'application/json';
 
        var result = "";
        ga('send', 'event', 'O365path-Rest', 'Try-it-out-invokeUrlOnUserData-' + controller + '-Begin');

        $.ajax({
            url: controller,
            type: 'GET',
            headers: requestHeaders,
            success: function (data, textStatus, xhr) {
                updateResponse(result, data, true);
                resultStatus = xhr.status;
                ga('send', 'event', 'O365path-Rest', 'Try-it-out-invokeUrlOnUserData-' + controller + '-Success');
            },
            error: function (jqXHR, exception) {
                updateResponse(result, "Error...", false);
                if (jqXHR.status === 0) {
                    updateResponse(result, "The request has been cancelled, please login and try again", false);
                    ga('send', 'event', 'O365path-Rest', 'Try-it-out-invokeUrlOnUserData-' + controller + '-Error-Request has been cancelled');
                    return;
                }
                else {
                    updateResponse(result, jqXHR.responseText, false);
                    ga('send', 'event', 'O365path-Rest', 'Try-it-out-invokeUrlOnUserData-' + controller + '-Error-' + jqXHR.responseText);
                    return;
                }
            },
            complete: function (xhr) {
                responseCodeContainer.removeClass('loading');;
                responseBodyElem.jsonView(result);
                // enable the btn
                invokeUrlBtnElem.prop("disabled", false).css("cursor", "pointer");
            }
        });

        function updateResponse(x, data, jsonFormatting) {
            if (jsonFormatting) {
                result += JSON.stringify(data);
            } else {
                result += data;
            }
        }

    }

    // invokes the URL
    function invokeUrlOnSampleData(url)
    {
        $("#response-container").show("slow");

        //disable invoke btn
        invokeUrlBtnElem.prop("disabled", true).css("cursor", "default");
        responseCodeContainer.addClass('loading');
        var p = $(serviceMenu).val(); // p = primary

        var authToken = data[p]["authToken"]; 
        var resultStatus='';
        var requestHeaders = JSON.parse('{}');
        requestHeaders['Accept'] = 'application/json';
        requestHeaders['Authorization'] = authToken;
        requestHeaders['ApiExProxy-FixedAadUser'] = 1;
        var urlToSend = "https://apiexproxy.azurewebsites.net/svc?url=" + url;

        var result ="";
        ga('send', 'event', 'O365path-Rest', 'Try-it-out-invokeUrlOnSampleData-' + url + '-Begin');

        $.ajax({
            url: urlToSend,
            type: 'GET',
            headers: requestHeaders,
            success: function (data, textStatus, xhr) {
                updateResponse(result, data, true);
                resultStatus = xhr.status;
                ga('send', 'event', 'O365path-Rest', 'Try-it-out-invokeUrlOnSampleData-' + url + '-Success');
            },
            error: function(jqXHR, exception) { 
                // Time out 
                if (exception === 'timeout') {
                    updateResponse(result, "Timeout occurred...", false);
                    ga('send', 'event', 'O365path-Rest', 'Try-it-out-invokeUrlOnSampleData-' + url + '-Error-' + 'TimeoutError');
                    return;
                }
                var errorHeader = jqXHR.getResponseHeader('ApiExProxy-Error');
                //Proxy Error or DNS Lookup Failure (502)
                if (jqXHR.status == '0' || (errorHeader != null && errorHeader != '0')) {
                    updateResponse(result, "Proxy not reachable", false);
                    ga('send', 'event', 'O365path-Rest', 'Try-it-out-invokeUrlOnSampleData-' + url + '-Error-' + 'ProxyNotReachable');
                } else {
                    try {
                       // Service Error
                        var jsonResponseText = $.parseJSON(jqXHR.responseText);
                        updateResponse(result, jsonResponseText, true);
                        resultStatus = jqXHR.status;
                        ga('send', 'event', 'O365path-Rest', 'Try-it-out-invokeUrlOnSampleData-' + url + '-Error-' + jsonResponseText);
                    }
                    catch (error) {
                        // Unexpected Service Error 
                        updateResponse(result, "Unexpected Error occured", false);
                        ga('send', 'event', 'O365path-Rest', 'Try-it-out-invokeUrlOnSampleData-' + url + '-Error-' + 'Unexpected Service Error');
                    }
                }
            },
            complete: function(xhr) {
                responseCodeContainer.removeClass('loading');;
                responseBodyElem.jsonView(result);
                // enable the btn
                invokeUrlBtnElem.prop("disabled", false).css("cursor", "pointer");
            }
        });
        
        function updateResponse(x, data, jsonFormatting) {
            if (jsonFormatting) {
                result+=JSON.stringify(data);
            } else {
                result+=data;
            }
        }
    }

    // initialize the parameters for the first time
    updateParams();
    // get service endpoint if user is authenticated
    $.ajax({
        url: "/GettingStarted/Account/IsAuthenticated",
        async: true,
        type: 'GET',
        success: function (data, textStatus, xhr) {
            if (data == "True") {
                getServiceEndPoint();
            }
        }
    });
}
