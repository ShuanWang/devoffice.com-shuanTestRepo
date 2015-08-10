var contentFilesJson = {
    "ContentFiles": [
        {
            "Id": "option-ios",
            "SetupFileFullPath": "\/Modules\/Devoffice.GettingStarted\/Documentation\/rest-api\/ios\/setup-ios.html",
            "GettingStartedFileFullPath": "\/Documentation\/rest-api\/ios\/getting-started-Office-365-APIs-ios.html",
            "GithubMDFileName": "rest-api\/ios\/getting-started-Office-365-APIs-ios.md",
            "PostDownloadInstructions": "Thanks! Your project is on its way from GitHub. To continue: Unzip downloaded package, navigate to the swift or objective-c project folder. Run pod install. Open Project in Xcode and run.",
        },
        {
            "Id": "option-android",
            "SetupFileFullPath": "\/Modules\/Devoffice.GettingStarted\/Documentation\/rest-api\/android\/setup-android.html",
            "GettingStartedFileFullPath": "\/Documentation\/rest-api\/android\/getting-started-Office-365-APIs-android.html",
            "GithubMDFileName": "rest-api\/android\/getting-started-Office-365-APIs-android.md",
            "PostDownloadInstructions": "Thanks! Your project is on its way from GitHub. To continue: Unzip downloaded package, open project, and run.",
        },
        {
            "Id": "option-dotnet",
            "SetupFileFullPath": "\/Modules\/Devoffice.GettingStarted\/Documentation\/rest-api\/dotnet\/setup-dotnet.html",
            "GettingStartedFileFullPath": "\/Documentation\/rest-api\/dotnet\/getting-started-Office-365-APIs-dotnet.html",
            "GithubMDFileName": "rest-api\/dotnet\/getting-started-Office-365-APIs-dotnet.md",
            "PostDownloadInstructions": "Thanks! Your project is on its way from GitHub. To continue: Unzip downloaded package, open solution, and run.",
        },
        {
            "Id": "option-javascript",
            "SetupFileFullPath": "\/Modules\/Devoffice.GettingStarted\/Documentation\/rest-api\/javascript\/setup-javascript.html",
            "GettingStartedFileFullPath": "\/Documentation\/rest-api\/javascript\/getting-started-Office-365-APIs-javascript.html",
            "GithubMDFileName": "rest-api\/javascript\/getting-started-Office-365-APIs-javascript.md",
            "PostDownloadInstructions": "Thanks! Your project is on its way from GitHub. To continue: Unzip downloaded package and run",
        },

        {
            "Id": "option-node",
            "SetupFileFullPath": "\/Modules\/Devoffice.GettingStarted\/Documentation\/rest-api\/node\/setup-node.html",
            "GettingStartedFileFullPath": "\/Documentation\/rest-api\/node\/getting-started-Office-365-APIs-node.html",
            "GithubMDFileName": "rest-api\/node\/getting-started-Office-365-APIs-node.md",
            "PostDownloadInstructions": "Thanks! Your project is on its way from GitHub. To continue: Unzip downloaded package, navigate to project folder and run: node index.js.  Then open your browser to the specified port (i.e. http://localhost:8000).",
        },
        {
            "Id": "option-php",
            "SetupFileFullPath": "\/Modules\/Devoffice.GettingStarted\/Documentation\/rest-api\/php\/setup-php.html",
            "GettingStartedFileFullPath": "\/Documentation\/rest-api\/php\/getting-started-Office-365-APIs-php.html",
            "GithubMDFileName": "rest-api\/php\/getting-started-Office-365-APIs-php.md",
            "PostDownloadInstructions": "Thanks! Your project is on its way from GitHub. To continue: Unzip downloaded package, place the folder in document root, restart the php server and access the app at http://localhost/php-tutorial/home.php.",
        },
        {
            "Id": "option-python",
            "SetupFileFullPath": "\/Modules\/Devoffice.GettingStarted\/Documentation\/rest-api\/python\/setup-python.html",
            "GettingStartedFileFullPath": "\/Documentation\/rest-api\/python\/getting-started-Office-365-APIs-python.html",
            "GithubMDFileName": "rest-api\/python\/getting-started-Office-365-APIs-python.md",
            "PostDownloadInstructions": "Thanks! Your project is on its way from GitHub. To continue: Unzip downloaded package, navigate to the folder. Install the <a href='http://www.python-requests.org/en/latest/'>Requests: HTTP for Humans</a> library by running <b>pip install requests</b>. Then run <b>python manage.py runserver</b> to start your server.",
        },
        {
            "Id": "option-ruby",
            "SetupFileFullPath": "\/Modules\/Devoffice.GettingStarted\/Documentation\/rest-api\/ruby\/setup-ruby.html",
            "GettingStartedFileFullPath": "\/Documentation\/rest-api\/ruby\/getting-started-Office-365-APIs-ruby.html",
            "GithubMDFileName": "rest-api\/ruby\/getting-started-Office-365-APIs-ruby.md",
            "PostDownloadInstructions": "Thanks! Your project is on its way from GitHub. To continue: Unzip downloaded package and navigate to your folder. Run <b>bundle install</b>. Then start the server by running <b>rails server</b> and navigate to http://localhost:3000.",
        }
    ]
}
function setDocumentationDivForPlatform(platformId, fileType, divName)
{
    var documentationFullFilePath = "\/Documentation\/onerror.html";
    var postDownloadInstructionsContent = "";

    $.each(contentFilesJson, function (key, contentFiles) {
        $(contentFiles).each(function (index, Id) {
            if (contentFiles[index].Id === platformId) {
                switch (fileType) {
                    case "setupFile":
                        {
                            documentationFullFilePath = contentFiles[index].SetupFileFullPath;
                            break;
                        }
                    case "gettingStartedFile":
                        {
                            documentationFullFilePath = contentFiles[index].GettingStartedFileFullPath;
                            break;
                        }
                    case "postDownloadInstructions":
                        {
                            postDownloadInstructionsContent = contentFiles[index].PostDownloadInstructions;
                            break;
                        }
                    default:
                        documentationFullFilePath = "\/Documentation\/onerror.html";
                }
            }
        });
    });

    if (fileType === 'postDownloadInstructions')
    {
        $('#post-download-instructions').html(postDownloadInstructionsContent);
        $('#post-download-instructions').show();
        return;
    }

    $.ajax({
        url: documentationFullFilePath,
        type: "GET",
        dataType: 'html',
        success: function (result, status, xhr) {
            document.getElementById(divName).innerHTML = result;
        }
    })
}

function redirectEditOnGitHub(platformId)
{
    var gitHubContentLocation = "https://github.com/OfficeDev/office-content/"; //onError it will redirect to master branch

    $.each(contentFilesJson, function (key, contentFiles) {
        $(contentFiles).each(function (index, Id) {
            if (contentFiles[index].Id === platformId) {
                gitHubContentLocation = gitHubContentLocation + "blob/master/" + contentFiles[index].GithubMDFileName;
            }
        });
    });
    window.open(gitHubContentLocation, "_blank");
    ga('send', 'event', 'O365path-Rest', 'EditOnGithub');
}