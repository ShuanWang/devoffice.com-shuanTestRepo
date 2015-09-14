//Variables for error handling
var inError = false;
var zipHasContent = false;
var selectedPlatformIndex = undefined; //to identify which platform is selected in reposList

//repos in GitHub
var reposList = {
    "Platform": [
        {
            "Name": "option-ios",
            "FileName": "O365-iOS-Connect-master\/objective-c\/O365-iOS-Connect\/AuthenticationManager.m",
            "ClientIdStringToReplace": "ENTER_CLIENT_ID_HERE",
            "ClientSecretStringToReplace": "ENTER_CLIENTSECRET_ID_HERE_HackWillNotReplace",
            "RedirectURLStringToReplace": "ENTER_REDIRECT_URI_HERE",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-iOS-Connect-master.zip",
            "GitHubRepoName": "O365-iOS-Connect",
            "GitHubMasterZipUrl": "https://github.com/OfficeDev/O365-iOS-Connect/archive/master.zip",
            "GitHubRepoUrl": "https://github.com/OfficeDev/O365-iOS-Connect/"
        },
        {
            "Name": "option-android",
            "FileName": "O365-Android-Connect-master\/app\/src\/main\/java\/com\/microsoft\/office365\/connect\/Constants.java",
            "ClientIdStringToReplace": "<Your client id here>",
            "ClientSecretStringToReplace": "ENTER_CLIENTSECRET_ID_HERE_HackWillNotReplace",
            "RedirectURLStringToReplace": "<Your redirect URI here>",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-Android-Connect-master.zip",
            "GitHubRepoName": "O365-Android-Connect",
            "GitHubMasterZipUrl": "https://github.com/OfficeDev/O365-Android-Connect/archive/master.zip",
            "GitHubRepoUrl": "https://github.com/OfficeDev/O365-Android-Connect"
        },
        {
            "Name": "option-dotnet",
            "FileName": "dotnet-tutorial-master\/dotnet-tutorial\/Web.config",
            "ClientIdStringToReplace": "</appSettings>",
            "ClientSecretStringToReplace": "ENTER_CLIENTSECRET_ID_HERE_HackWillNotReplace",
            "RedirectURLStringToReplace": "ENTER_REDIRECT_URI_HERE_HackWillNotReplace",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-Win-tutorial-master.zip",
            "GitHubRepoName": "O365-Win-Tutorial",
            "GitHubMasterZipUrl": "https://github.com/jasonjoh/dotnet-tutorial/archive/v1.zip",
            "GitHubRepoUrl": "https://github.com/jasonjoh/dotnet-tutorial/tree/v1"
        },
        {
            "Name": "option-php",
            "FileName": "O365-PHP-tutorial-master\/php-tutorial\/oauth.php",
            "ClientIdStringToReplace": "21a66e5f-74c5-4acb-a0ee-02814e3fe217",
            "ClientSecretStringToReplace": "tqlvN4Skz4Ah7BVcttEpJLxilJ4V0h+EnrSmLAaYfmQ=",
            "RedirectURLStringToReplace": "ENTER_REDIRECT_URI_HERE_HackWillNotReplace",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-PHP-tutorial-master.zip",
            "GitHubRepoName": "O365-PHP-tutorial",
            "GitHubMasterZipUrl": "https://github.com/jasonjoh/php-tutorial/archive/v1.zip",
            "GitHubRepoUrl": "https://github.com/jasonjoh/php-tutorial/tree/v1"
        },
        {
            "Name": "option-node",
            "FileName": "node-tutorial-master\/authHelper.js",
            "ClientIdStringToReplace": "YOUR CLIENT ID HERE",
            "ClientSecretStringToReplace": "YOUR CLIENT SECRET HERE",
            "RedirectURLStringToReplace": "http://localhost:8000",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-Node-tutorial-master.zip",
            "GitHubRepoName": "O365-Node-tutorial",
            "GitHubMasterZipUrl": "https://github.com/jasonjoh/node-tutorial/archive/v1.zip",
            "GitHubRepoUrl": "https://github.com/jasonjoh/node-tutorial/tree/v1"
        },
        {
            "Name": "option-python",
            "FileName": "python_tutorial-master\/tutorial\/authhelper.py",
            "ClientIdStringToReplace": "YOUR CLIENT ID",
            "ClientSecretStringToReplace": "YOUR CLIENT SECRET",
            "RedirectURLStringToReplace": "ENTER_REDIRECT_URI_HERE_HackWillNotReplace",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-Python-tutorial-master.zip",
            "GitHubRepoName": "O365-Python-tutorial",
            "GitHubMasterZipUrl": "https://github.com/jasonjoh/python_tutorial/archive/v1.zip",
            "GitHubRepoUrl": "https://github.com/jasonjoh/python_tutorial/tree/v1"
        },
        {
            "Name": "option-ruby",
            "FileName": "o365-tutorial-master\/app\/helpers\/auth_helper.rb",
            "ClientIdStringToReplace": "<YOUR CLIENT ID>",
            "ClientSecretStringToReplace": "<YOUR CLIENT SECRET>",
            "RedirectURLStringToReplace": "ENTER_REDIRECT_URI_HERE_HackWillNotReplace",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-Ruby-tutorial-master.zip",
            "GitHubRepoName": "O365-Ruby-tutorial",
            "GitHubMasterZipUrl": "https://github.com/jasonjoh/o365-tutorial/archive/v1.zip",
            "GitHubRepoUrl": "https://github.com/jasonjoh/o365-tutorial/tree/v1"
        }
    ]
}

function codeSamplePackageAndDownload(platformName, clientId, clientSecret, appRedirectUrl, signOnUrl) {
    try {
        ga('send', 'event', 'DownloadCodeSample', 'Begin-' + platformName, platformName, 0);
        _resetFlags();
        _setPlatformSelectedIndex(platformName);

        if (typeof navigator !== "undefined" && /(Safari\/[1-9])/.test(navigator.userAgent) && /(Chrome\/[1-9])/.test(navigator.userAgent)==false)  {
            console.log('Safari does not support :blob for downloading.');
            throw new Error('SafariDownloadNotSupported');
        }

        if (clientId === undefined || clientId === null)
        {
            throw new Error('ClientIdIsUndefnied');
        }
        var clientIdOriginalFormat = clientId;
        $.support.cors = true; //this is required for IE support
        if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
            console.log('The File APIs are not fully supported in this browser.');
            throw new Error('FileAPINotSupported');
        }

        if (typeof navigator !== "undefined" && /MSIE [1-9]\./.test(navigator.userAgent)) {
            console.log('This IE version is not supported, please upgrade your browser.');
            throw new Error('IEUnsupportedVersion');
        }

        //Special case for Windows snippet
        if (platformName === 'option-dotnet') {
            clientId = "<add key=\"ida:ClientId\" value=\""+clientId+"\" />" +  
            "<add key=\"ida:ClientSecret\" value=\""+clientSecret+"\" />" +
            " </appSettings>";
        }
        var zipFileName = reposList.Platform[selectedPlatformIndex].LocalZipFile;
        JSZipUtils.getBinaryContent(zipFileName, function (err, data) {
            if (err) {
                throw new Error('ErrorReadingFiles');
            }

            var codeSampleZip = new JSZip(data); //
            for (var nameOfFile in codeSampleZip.files) {
                var file = codeSampleZip.files[nameOfFile]; //may be move it inside If clause

                //if (nameOfFile.indexOf(reposList.Platform[selectedPlatformIndex].FileName) > 0)
                if (nameOfFile === reposList.Platform[selectedPlatformIndex].FileName) {
                    fileContent = file.asText();
                    codeSampleZip.remove(nameOfFile);
                    fileContent = fileContent.replace(reposList.Platform[selectedPlatformIndex].ClientIdStringToReplace, clientId);
                    fileContent = fileContent.replace(reposList.Platform[selectedPlatformIndex].ClientSecretStringToReplace, clientSecret);
                    fileContent = fileContent.replace(reposList.Platform[selectedPlatformIndex].RedirectURLStringToReplace, appRedirectUrl);
                    fileContent = fileContent.replace(reposList.Platform[selectedPlatformIndex].SignOnURLStringToReplace, signOnUrl);
                    codeSampleZip.file(nameOfFile, fileContent);
                }
                    //special case for iOS swift folder
                    if (platformName === 'option-ios') 
                    {
                        if (nameOfFile === 'O365-iOS-Connect-master\/swift\/O365-iOS-Connect-Swift\/AuthenticationManager.swift') {
                        fileContent = file.asText();
                        codeSampleZip.remove(nameOfFile);
                        fileContent = fileContent.replace(reposList.Platform[selectedPlatformIndex].ClientIdStringToReplace, clientId);
                        fileContent = fileContent.replace(reposList.Platform[selectedPlatformIndex].ClientSecretStringToReplace, clientSecret);
                        fileContent = fileContent.replace(reposList.Platform[selectedPlatformIndex].RedirectURLStringToReplace, appRedirectUrl);
                        fileContent = fileContent.replace(reposList.Platform[selectedPlatformIndex].SignOnURLStringToReplace, signOnUrl);
                        codeSampleZip.file(nameOfFile, fileContent);
                    }
            }

            }
            var content = codeSampleZip.generate({ type: "blob" });
            window.saveAs(content, reposList.Platform[selectedPlatformIndex].GitHubRepoName + ".zip");
            ga('send', 'event', 'DownloadCodeSample', 'Success-' + platformName, platformName, 1);
            appInsights.trackEvent("DownloadCodeSampleWithClientId", { ClientId: clientIdOriginalFormat });
            MscomCustomEvent('ms.InteractionType', '4', 'ms.controlname', 'O365apis', 'ms.ea_action', 'DownloadCodeSample-Success', 'ms.contentproperties', platformName + '-' + clientIdOriginalFormat);
        });
        _progressStatus(100)
    }
    catch (error) {
        _errorHandlerDownloadSample(error);
    }
}

function _setPlatformSelectedIndex(platformSelected) {
    $.each(reposList, function (key, repos) {
        $(repos).each(function (index, platform) {
            if (platform.Name === platformSelected) {
                selectedPlatformIndex = index;
                return;
            }
        });
    });
}

function ViewCodeSampleInGithub(platformName) {
    var gitHubRepoLocation = "https://github.com/OfficeDev"; //onError it will redirect to Office Dev repo
    $.each(reposList, function (key, repos) {
        $(repos).each(function (index, platform) {
            if (platform.Name === platformName) {
                gitHubRepoLocation = platform.GitHubRepoUrl;
                return;
            }
        });
    });
    window.open(gitHubRepoLocation, "_blank");
    ga('send', 'event', 'DownloadCodeSample', 'ViewOnGithub-' + platformName);
    MscomCustomEvent('ms.InteractionType', '4', 'ms.controlname', 'O365apis', 'ms.ea_action', 'ViewCodeSampleOnGithub', 'ms.contentproperties', platformName);
}



//To be edited for production, elements name will be different.
function _resetFlags() {
    inError = false; zipHasContent = false; selectedPlatformIndex = undefined;
    //document.getElementById("messageLabel").textContent = ''; document.getElementById("progressBar").textContent = '';
    }

//Need to improve this function to show error in UI. Show download link from GitHub if in Error.
function _errorHandlerDownloadSample(error) {
    var msg;
    switch (error.message) {
        case 'ClientIdIsUndefnied':
            msg = 'Sign-in and register app so we can embed your client id, redirect uri and app secret into your app for you.';
            break;
        case 'SafariDownloadNotSupported':
            msg = "Safari doesn't work well with some of the open source components we use on this page. You may have more success with another browser. For now, we've downloaded the code sample for you, but you'll need to update the client id, client secret (if applicable), and redirect uri in the code yourself. See the readme in the download for more instructions.";
            break;
        case 'FileAPINotSupported':
            msg = 'File APIs are not supported in your browser.';
            break;
        case 'ErrorReadingFiles':
            msg = 'Error Reading file from source.';
            break;
        case 'IEUnsupportedVersion':
            msg = 'IE version less than 10 is not supported.';
            break;
        default:
            msg = 'Unknown Error'
            break;
    }

    if (selectedPlatformIndex != undefined) {
        msg = 'FYI - We downloaded an untouched sample from GitHub. ' + msg;
        location.href = (reposList.Platform[selectedPlatformIndex].GitHubMasterZipUrl);
        MscomCustomEvent('ms.InteractionType', '4', 'ms.controlname', 'O365apis', 'ms.ea_action', 'DownloadCodeSample-Success', 'ms.contentproperties', getCookie("platform"));
        ga('send', 'event', 'DownloadCodeSample', getCookie("platform"));
        $('#post-download-instructions').html(msg)
        $('#post-download-instructions').show();
        //$('#post-download-instructions').addClass('animated fadeInUp');
        return;

    }
    //document.getElementById("messageLabel").textContent = 'CodeSampleDownloadError: ' + msg
    ga('send', 'event', 'DownloadCodeSample', 'Error-' + msg, '', 0);
    MscomCustomEvent('ms.InteractionType', '4', 'ms.controlname', 'O365apis', 'ms.ea_action', 'DownloadCodeSample-Error', 'ms.callresult', error.message);
}


//To Be Deleted: This function will be replaced with actuall progress bar function. This is added for testing only
function _progressStatus(progressBar) {
    if (progressBar >= 100) {
        progressBar = 100;
        setDocumentationDivForPlatform(platformId, 'postDownloadInstructions', 'post-download-instructions');
    }
//document.getElementById("progressBar").textContent = 'Download progress ' + progressBar + ' %.';
}