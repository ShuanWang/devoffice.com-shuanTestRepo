/* *
 * Wire up the handlers
 */
$(document).ready(function () {
    // register for platform selection
    $("#pickPlatform ul li a").click(function () {
        getRepoList(this.id, "suggestionlistID");
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
            "App": "word", /*TBD need to confirm*/
            "CodeSampleName": "Sample Name", /* we need to add name */
            "Description": "This is a code sample", /* we need to add description */
            "FileName": "O365-iOS-Connect-master\/objective-c\/O365-iOS-Connect\/AuthenticationManager.m",
            "ClientIdStringToReplace": "ENTER_CLIENT_ID_HERE",
            "ClientSecretStringToReplace": "ENTER_CLIENTSECRET_ID_HERE_HackWillNotReplace",
            "RedirectURLStringToReplace": "ENTER_REDIRECT_URI_HERE",
            "SignOnURLStringToReplace": "ENTER_SIGNON_URI_HERE_HackWillNotReplace",
            "LocalZipFile": "../../CodeSamples/O365-iOS-Connect-master.zip",
            "GitHubRepoName": "O365-iOS-Connect.zip",
            "GitHubMasterZipUrl": "https://github.com/OfficeDev/O365-iOS-Connect/archive/master.zip",
            "GitHubRepoUrl": "https://github.com/OfficeDev/O365-iOS-Connect/"
        },
        {
            "Platform": "option-android",
            "App": "word", /*TBD need to confirm*/
            "CodeSampleName": "Sample Name", /* we need to add name */
            "Description": "This is a code sample", /* we need to add description */
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
            "Platform": "option-dotnet",
            "App": "outlook",
            "CodeSampleName": "Sample Name", /* we need to add name */
            "Description": "This is a code sample", /* we need to add description */
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
            "Platform": "option-php",
            "App": "outlook",
            "CodeSampleName": "Sample Name", /* we need to add name */
            "Description": "This is a code sample", /* we need to add description */
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
            "CodeSampleName": "Sample Name", /* we need to add name */
            "Description": "This is a code sample", /* we need to add description */
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
            "CodeSampleName": "Sample Name", /* we need to add name */
            "Description": "This is a code sample", /* we need to add description */
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
            "CodeSampleName": "O365-tutorial", /* we need to add name */
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
function getRepoList(selectedPlatform, divId) {
    showSuggestions(search(selectedPlatform), divId);
}

function showSuggestions(repos, divId) {
    if (repos.length <= 0) {
        alert("No Suggestion found"); // TBD, need to remove, currently for debug purpose only
    }
    var innerHtml = "";
    for (var i = 0; i < repos.length; ++i) {
        innerHtml += "<li>" + getHtml(repos[i]) + "</li>";
    }
    $("#" + divId).html("<ul class='panel-collection'>" + innerHtml + "</ul>");

    $("#codesample-download-button").click(downloadCodeSampleHandler);
}

function getHtml(data) {
    return "<div class='panel panel-default text-center panel-download'>" +
                "<div class='panel-heading'>" +
                    "<div class='panel-title'>" + data.CodeSampleName + "</div>" +
                "</div>" +
               "<div class='panel-body'>" +
                  data.Description +
               "</div>" +
                "<div class='panel-footer'>" + getDownloadButtonHTML(data)+ "</div>" +
            "</div>";
}

function getDownloadButtonHTML(data) {
    return "<a class='btn btn-success' "+ 
        "id='codesample-download-button'"+
        "name='" + data.CodeSampleName + "'" +
        "data-FileName='" + data.FileName + "'" +
        "data-ClientIdStringToReplace='" + data.ClientIdStringToReplace + "'" +
        "data-ClientSecretStringToReplace='" + data.ClientSecretStringToReplace + "'" +
        "data-RedirectURLStringToReplace='" + data.RedirectURLStringToReplace + "'" +
        "data-SignOnURLStringToReplace='" + data.SignOnURLStringToReplace + "'" +
        "data-LocalZipFile='" + data.LocalZipFile + "'" +
        "data-GitHubRepoName='" + data.GitHubRepoName + "'" +
        "data-GitHubMasterZipUrl='" + data.GitHubMasterZipUrl + "'" +
        "data-GitHubRepoUrl='" + data.GitHubRepoUrl + "'" +
        ">" + "<i class='fa fa-download'></i> Download" + "</a>";
}

function downloadCodeSampleHandler()
{
    alert("downloading from " + $(this).attr("data-LocalZipFile"));
}
function codeSamplePackageAndDownload(platformName, clientId, clientSecret, appRedirectUrl, signOnUrl) {
    try {
        ga('send', 'event', 'DownloadCodeSample', 'Begin-' + platformName, platformName, 0);
        _resetFlags();
        _setPlatformSelectedIndex(platformName);

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
        var zipFileName = reposList.Repo[selectedPlatformIndex].LocalZipFile;
        JSZipUtils.getBinaryContent(zipFileName, function (err, data) {
            if (err) {
                throw new Error('ErrorReadingFiles');
            }

            var codeSampleZip = new JSZip(data); //
            for (var nameOfFile in codeSampleZip.files) {
                var file = codeSampleZip.files[nameOfFile]; //may be move it inside If clause

                //if (nameOfFile.indexOf(reposList.Repo[selectedPlatformIndex].FileName) > 0)
                if (nameOfFile === reposList.Repo[selectedPlatformIndex].FileName) {
                    fileContent = file.asText();
                    codeSampleZip.remove(nameOfFile);
                    fileContent = fileContent.replace(reposList.Repo[selectedPlatformIndex].ClientIdStringToReplace, clientId);
                    fileContent = fileContent.replace(reposList.Repo[selectedPlatformIndex].ClientSecretStringToReplace, clientSecret);
                    fileContent = fileContent.replace(reposList.Repo[selectedPlatformIndex].RedirectURLStringToReplace, appRedirectUrl);
                    fileContent = fileContent.replace(reposList.Repo[selectedPlatformIndex].SignOnURLStringToReplace, signOnUrl);
                    codeSampleZip.file(nameOfFile, fileContent);
                }
                    //special case for iOS swift folder
                    if (platformName === 'option-ios') 
                    {
                        if (nameOfFile === 'O365-iOS-Connect-master\/swift\/O365-iOS-Connect-Swift\/AuthenticationManager.swift') {
                        fileContent = file.asText();
                        codeSampleZip.remove(nameOfFile);
                        fileContent = fileContent.replace(reposList.Repo[selectedPlatformIndex].ClientIdStringToReplace, clientId);
                        fileContent = fileContent.replace(reposList.Repo[selectedPlatformIndex].ClientSecretStringToReplace, clientSecret);
                        fileContent = fileContent.replace(reposList.Repo[selectedPlatformIndex].RedirectURLStringToReplace, appRedirectUrl);
                        fileContent = fileContent.replace(reposList.Repo[selectedPlatformIndex].SignOnURLStringToReplace, signOnUrl);
                        codeSampleZip.file(nameOfFile, fileContent);
                    }
            }

            }
            var content = codeSampleZip.generate({ type: "blob" });
            window.saveAs(content, reposList.Repo[selectedPlatformIndex].GitHubRepoName + ".zip");
            ga('send', 'event', 'DownloadCodeSample', 'Success-' + platformName, platformName, 1);
            appInsights.trackEvent("ClientID--" + clientId);
        });
        _progressStatus(100)
    }
    catch (error) {
        _errorHandlerDownloadSample(error);
    }
}

function _setPlatformSelectedIndex(platformSelected) {
    $.each(reposList, function (key, repos) {
        $(repos).each(function (index, repo) {
            if (repo.Platform === platformSelected) {
                selectedPlatformIndex = index;
                return;
            }
        });
    });
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
function _errorHandlerDownloadSample(error) {
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
        location.href = (reposList.Repo[selectedPlatformIndex].GitHubMasterZipUrl);

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