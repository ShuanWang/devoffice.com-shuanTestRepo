/// <reference path="E:\devofficecom\src\Orchard.Web\Modules\Devoffice.GettingStarted\Views/O365/_RegisterApp.cshtml" />
/* *
 * Wire up the handlers
 */
$(document).ready(function () {
    // register for platform selection, whenever user selects the platform,
    //we will add suggestions for selected platform in the given div id.
    $("#pickPlatform ul li a").click(function () {
        addSuggestions(this.id, "suggestionlistID");
    });

});
//Variables for error handling
var inError = false;
var zipHasContent = false;
var selectedPlatformIndex = undefined; //to identify which platform is selected in reposList

//repos in GitHub
var reposList = {
    "Repo": [
        {
            "Platform": "option-ios",
            "uid": "O365-iOS-Connect-outlook",
            "App": "outlook",
            "CodeSampleName": "O365-iOS-Connect",
            "Description": "This Connect sample for iOS shows how to connect your app to Office 365. Once connected, the sample shows how to send a simple service call. Comes in both Swift and Objective-C",
            "FileName": "O365-iOS-Connect-master\/objective-c\/O365-iOS-Connect\/AuthenticationManager.m",
            "ClientIdStringToReplace": "ENTER_CLIENT_ID_HERE",
            "ClientSecretStringToReplace": "ENTER_CLIENTSECRET_ID_HERE_HackWillNotReplace",
            "RedirectURLStringToReplace": "ENTER_REDIRECT_URI_HERE",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples/O365-iOS-Connect-master.zip",
            "GitHubRepoName": "O365-iOS-Connect.zip",
            "GitHubMasterZipUrl": "https://github.com/OfficeDev/O365-iOS-Connect/archive/master.zip",
            "GitHubRepoUrl": "https://github.com/OfficeDev/O365-iOS-Connect/"
        },
        {
            "Platform": "option-android",
            "uid": "O365-Android-Connect-outlook",
            "App": "outlook",
            "CodeSampleName": "O365-Android-Connect",
            "Description": "This Connect sample for Android shows you how to connect your app to Office 365. It also demonstrates how to issue a simple service call, like sending an email.",
            "FileName": "O365-Android-Connect-master\/app\/src\/main\/java\/com\/microsoft\/office365\/connect\/Constants.java",
            "ClientIdStringToReplace": "<Your client id here>",
            "ClientSecretStringToReplace": "ENTER_CLIENTSECRET_ID_HERE_HackWillNotReplace",
            "RedirectURLStringToReplace": "<Your redirect URI here>",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-Android-Connect-master.zip",
            "GitHubRepoName": "O365-Android-Connect.zip",
            "GitHubMasterZipUrl": "https://github.com/OfficeDev/O365-Android-Connect/archive/master.zip",
            "GitHubRepoUrl": "https://github.com/OfficeDev/O365-Android-Connect"
        },
        {
            "Platform": "option-android",
            "uid": "o365-andorid-snippet-outlook",
            "CodeSampleName": "Office 365 Code Snippets for Android",
            "Description": "This sample for Android is a repository of simple method examples that access email, calendar events, contacts, and files in Office 365. These 'snippet' methods are self contained so you can paste them into your own code or use as reference for learning.",
            "App": "outlook, onedrive",
            "FileName": "O365-Android-Snippets-master\/app\/src\/main\/java\/com\/microsoft\/office365\/snippetapp\/helpers\/Constants.java",
            "ClientIdStringToReplace": "<Your client ID HERE>",
            "ClientSecretStringToReplace": "ENTER_CLIENTSECRET_ID_HERE_HackWillNotReplace",
            "RedirectURLStringToReplace": "<Your redirect URI HERE>",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-Android-Snippets-master.zip",
            "GitHubRepoName": "O365-Android-Snippets",
            "GitHubMasterZipUrl": "https://github.com/OfficeDev/O365-Android-Snippets/archive/master.zip",
            "GitHubRepoUrl": "https://github.com/OfficeDev/O365-Android-Snippets"
        },
        {
            "Platform": "option-dotnet",
            "uid": "option-dotnet-mail-api",
            "App": "outlook",
            "CodeSampleName": "DotNet-tutorial", /* we need to add name */
            "Description": "An ASP.NET MVC tutorial for using the Mail API. ",
            "FileName": "dotnet-tutorial-master\/dotnet-tutorial\/Web.config",
            "ClientIdStringToReplace": "</appSettings>",
            "ClientSecretStringToReplace": "ENTER_CLIENTSECRET_ID_HERE_HackWillNotReplace",
            "RedirectURLStringToReplace": "ENTER_REDIRECT_URI_HERE_HackWillNotReplace",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-Win-tutorial-master.zip",
            "GitHubRepoName": "O365-Win-Tutorial",
            "GitHubMasterZipUrl": "https://github.com/jasonjoh/dotnet-tutorial/archive/master.zip",
            "GitHubRepoUrl": "https://github.com/jasonjoh/dotnet-tutorial"
        },
        {
            "Platform": "option-dotnet",
            "uid": "dotnet-o365-starter-mvc",
            "App": "outlook",
            "CodeSampleName": "Office 365 Starter Project for ASP.NET MVC",
            "Description": "This sample uses the Office 365 API Tools to demonstrate basic operations against the Calendar, Contacts, and Mail service endpoints in Office 365 from a single-tenant ASP.NET MVC application.",
            "FileName": "O365-ASPNETMVC-Start-master\/O365-APIs-Start-ASPNET-MVC\/Web.config",
            "ClientIdStringToReplace": "</appSettings>",
            "ClientSecretStringToReplace": "ENTER_CLIENTSECRET_ID_HERE_HackWillNotReplace",
            "RedirectURLStringToReplace": "ENTER_REDIRECT_URI_HERE_HackWillNotReplace",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-ASPNETMVC-Start-master.zip",
            "GitHubRepoName": "O365-ASPNETMVC-Start",
            "GitHubMasterZipUrl": "https://github.com/OfficeDev/O365-ASPNETMVC-Start/archive/master.zip",
            "GitHubRepoUrl": "https://github.com/OfficeDev/O365-ASPNETMVC-Start"
        },
        {
            "Platform": "option-php",
            "uid": "option-php-outlook",
            "App": "outlook",
            "CodeSampleName": "Simple PHP tutorial",
            "Description": "A simple tutorial for creating a PHP app that uses the Outlook Mail API",
            "FileName": "php-tutorial\/oauth.php",
            "ClientIdStringToReplace": "21a66e5f-74c5-4acb-a0ee-02814e3fe217",
            "ClientSecretStringToReplace": "tqlvN4Skz4Ah7BVcttEpJLxilJ4V0h+EnrSmLAaYfmQ=",
            "RedirectURLStringToReplace": "ENTER_REDIRECT_URI_HERE_HackWillNotReplace",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-PHP-tutorial-master.zip",
            "GitHubRepoName": "O365-PHP-tutorial",
            "GitHubMasterZipUrl": "https://github.com/jasonjoh/php-tutorial/archive/master.zip",
            "GitHubRepoUrl": "https://github.com/jasonjoh/php-tutorial"
        },
        {
            "Platform": "option-node",
            "uid": "option-node-outlook",
            "CodeSampleName": "Simple Node.js tutorial",
            "Description": "A simple Node.js tutorial to use the Mail API.",
            "App": "outlook",
            "FileName": "node-tutorial-master\/authHelper.js",
            "ClientIdStringToReplace": "YOUR CLIENT ID HERE",
            "ClientSecretStringToReplace": "YOUR CLIENT SECRET HERE",
            "RedirectURLStringToReplace": "http://localhost:8000",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-Node-tutorial-master.zip",
            "GitHubRepoName": "O365-Node-tutorial",
            "GitHubMasterZipUrl": "https://github.com/jasonjoh/node-tutorial/archive/master.zip",
            "GitHubRepoUrl": "https://github.com/jasonjoh/node-tutorial"
        },
        {
            "Platform": "option-python",
            "uid": "option-python-outlook",
            "CodeSampleName": "Simple Python tutorial",
            "Description": "A simple tutorial for creating a Python app that uses the Outlook Mail API.",
            "App": "outlook",
            "FileName": "python_tutorial-master\/tutorial\/authhelper.py",
            "ClientIdStringToReplace": "YOUR CLIENT ID",
            "ClientSecretStringToReplace": "YOUR CLIENT SECRET",
            "RedirectURLStringToReplace": "ENTER_REDIRECT_URI_HERE_HackWillNotReplace",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-Python-tutorial-master.zip",
            "GitHubRepoName": "O365-Python-tutorial",
            "GitHubMasterZipUrl": "https://github.com/jasonjoh/python_tutorial/archive/master.zip",
            "GitHubRepoUrl": "https://github.com/jasonjoh/python_tutorial"
        },
        {
            "Platform": "option-ruby",
            "uid": "option-ruby-outlook",
            "CodeSampleName": "O365-tutorial",
            "Description": "A simple guide to writing your first Ruby on Rails app using the Outlook Mail API.", /* we need to add description */
            "App": "outlook",
            "FileName": "o365-tutorial-master\/app\/helpers\/auth_helper.rb",
            "ClientIdStringToReplace": "<YOUR CLIENT ID>",
            "ClientSecretStringToReplace": "<YOUR CLIENT SECRET>",
            "RedirectURLStringToReplace": "ENTER_REDIRECT_URI_HERE_HackWillNotReplace",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "\/Modules\/Devoffice.GettingStarted\/CodeSamples\/O365-Ruby-tutorial-master.zip",
            "GitHubRepoName": "O365-Ruby-tutorial",
            "GitHubMasterZipUrl": "https://github.com/jasonjoh/o365-tutorial/archive/master.zip",
            "GitHubRepoUrl": "https://github.com/jasonjoh/o365-tutorial"
        }
    ]
}

/*
 * Searchs for list of suggestion based on the platform
 * 
 * platform=> platform to search for e.g. option-ios
 */
function search(platform) {
    var repos = [];
    if (platform != undefined && platform != "") {
        for (var i = 0; i < reposList.Repo.length; ++i) {
            if (reposList.Repo[i].Platform == platform) {
                repos.push(reposList.Repo[i]);
            }
        }
    }
    return repos;
}

/*
 * returns the repo details  based on the uid
 * 
 * uid=> uid of the repo
 */
function getRepoById(uid) {
    for (var i = 0; i < reposList.Repo.length; ++i) {
        if (reposList.Repo[i].uid === uid) {
            return reposList.Repo[i];
        }
    }
    return null;
}

/*
 * Adds the suggestion for selected platform in the given divid
 * 
 * selectedPlatform=> user selected platform
 * divId=> a div where the panels for multiple download will be added.
 */
function addSuggestions(selectedPlatform, divId) {
    var repos = search(selectedPlatform);
    if (repos.length <= 0) { 
        alert("No Suggestion found"); // TBD, need to remove, currently for debug purpose only
        return;
    }
    var innerHtml = "";
    for (var i = 0; i < repos.length; ++i) {
        innerHtml += "<li>" + getPanelHtml(repos[i]) + "</li>";
    }
    $("#" + divId).html("<ul class='panel-collection'>" + innerHtml + "</ul>");

    // do binding for each download button
    for (var i = 0; i < repos.length; ++i) {
        var btnid = "codesample-download-button-" + repos[i].uid;
        $('#' + btnid).click(downloadCodeSampleHandler);
    }
}


/*
 * builds the panel html from the given repo
 */
function getPanelHtml(repo) {
    return "<div class='panel panel-default text-center panel-download'>" +
                "<div class='panel-heading'>" +
                    "<div class='panel-title'>" + repo.CodeSampleName + "</div>" +
                "</div>" +
               "<div class='panel-body'>" +
                  repo.Description +
               "</div>" +
                "<div class='panel-footer'>" + getDownloadButtonHTML(repo) + "</div>" +
            "</div>";
}

/*
 * build the download html for the given repo
 */
function getDownloadButtonHTML(repo) {
    return "<a class='btn btn-success' "+ 
        "id='codesample-download-button-" + repo.uid +"'"+ 
        "data-uid='" + repo.uid + "'" +
        ">" + "<i class='fa fa-download'></i> Download" + "</a>";
}



function downloadCodeSampleHandler()
{
    var uid = $(this).attr("data-uid");
    codeSamplePackageAndDownload(uid, registerAppParams.clientId, registerAppParams.clientSecret,
        registerAppParams.redirectUri, registerAppParams.signonUri);
}


/*
 * The core function that downloads the code sample and embeds the client id and other details in
 * code sample
 * 
 * uid=> unique id of the repo that need to be downloaded
 * 
 */
function codeSamplePackageAndDownload(uid, clientId, clientSecret, appRedirectUrl, signOnUrl) {
    var repo = getRepoById(uid);
    if (repo == null) {
        alert("No repo found for the given uid = " + uid); // TBD, need to remove, currently for debug purpose only
        return;
    }
    try {
        var platformName = repo.Platform;
        ga('send', 'event', 'DownloadCodeSample', 'Begin-' + platformName, platformName, 0);
        _resetFlags();

        if (clientId === undefined || clientId === null)
        {
            throw new Error('ClientIdIsUndefnied');
        }
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
        var zipFileName = repo.LocalZipFile;
        JSZipUtils.getBinaryContent(zipFileName, function (err, data) {
            if (err) {
                throw new Error('ErrorReadingFiles');
            }

            var codeSampleZip = new JSZip(data); //
            for (var nameOfFile in codeSampleZip.files) {
                var file = codeSampleZip.files[nameOfFile]; //may be move it inside If clause

                //if (nameOfFile.indexOf(reposList.Repo[selectedPlatformIndex].FileName) > 0)
                if (nameOfFile === repo.FileName) {
                    fileContent = file.asText();
                    codeSampleZip.remove(nameOfFile);
                    fileContent = fileContent.replace(repo.ClientIdStringToReplace, clientId);
                    fileContent = fileContent.replace(repo.ClientSecretStringToReplace, clientSecret);
                    fileContent = fileContent.replace(repo.RedirectURLStringToReplace, appRedirectUrl);
                    fileContent = fileContent.replace(repo.SignOnURLStringToReplace, signOnUrl);
                    codeSampleZip.file(nameOfFile, fileContent);
                }
                    //special case for iOS swift folder
                    if (platformName === 'option-ios') 
                    {
                        if (nameOfFile === 'O365-iOS-Connect-master\/swift\/O365-iOS-Connect-Swift\/AuthenticationManager.swift') {
                        fileContent = file.asText();
                        codeSampleZip.remove(nameOfFile);
                        fileContent = fileContent.replace(repo.ClientIdStringToReplace, clientId);
                        fileContent = fileContent.replace(repo.ClientSecretStringToReplace, clientSecret);
                        fileContent = fileContent.replace(repo.RedirectURLStringToReplace, appRedirectUrl);
                        fileContent = fileContent.replace(repo.SignOnURLStringToReplace, signOnUrl);
                        codeSampleZip.file(nameOfFile, fileContent);
                    }
            }

            }
            var content = codeSampleZip.generate({ type: "blob" });
            window.saveAs(content, repo.GitHubRepoName + ".zip");
            ga('send', 'event', 'DownloadCodeSample', 'Success-' + platformName, platformName, 1);
            appInsights.trackEvent("ClientID--" + clientId);
        });
        _progressStatus(100)
    }
    catch (error) {
        _errorHandlerDownloadSample(error, repo);
    }
}

function ViewCodeSampleInGithub(platformName) {
    var gitHubRepoLocation = "https://github.com/OfficeDev"; //onError it will redirect to Office Dev repo
    $.each(reposList, function (key, repos) {
        $(repos).each(function (index, repo) {
            if (repo.Platform === platformName) {
                gitHubRepoLocation = platform.GitHubRepoUrl;
                return;
            }
        });
    });
    window.open(gitHubRepoLocation, "_blank");
    ga('send', 'event', 'DownloadCodeSample', 'ViewOnGithub-' + platformName);
}



//To be edited for production, elements name will be different.
function _resetFlags() {
    inError = false; zipHasContent = false; selectedPlatformIndex = undefined;
    //document.getElementById("messageLabel").textContent = ''; document.getElementById("progressBar").textContent = '';
    }

//Need to improve this function to show error in UI. Show download link from GitHub if in Error.
function _errorHandlerDownloadSample(error, repo) {
    var msg;
    switch (error.message) {
        case 'ClientIdIsUndefnied':
            msg = 'Sign-in and register app so we can embed your client id, redirect uri and app secret into your app for you.';
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
        location.href = (repo.GitHubMasterZipUrl);

        $('#post-download-instructions').html(msg)
        $('#post-download-instructions').show();
        //$('#post-download-instructions').addClass('animated fadeInUp');
        return;

    }
    //document.getElementById("messageLabel").textContent = 'CodeSampleDownloadError: ' + msg
    ga('send', 'event', 'DownloadCodeSample', 'Error-' + msg, '', 0);
}


//To Be Deleted: This function will be replaced with actuall progress bar function. This is added for testing only
function _progressStatus(progressBar) {
    if (progressBar >= 100) {
        progressBar = 100;
        setDocumentationDivForPlatform(platformId, 'postDownloadInstructions', 'post-download-instructions');
    }
//document.getElementById("progressBar").textContent = 'Download progress ' + progressBar + ' %.';
}