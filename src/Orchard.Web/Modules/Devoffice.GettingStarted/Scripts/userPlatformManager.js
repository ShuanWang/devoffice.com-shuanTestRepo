// it fetches the user selected platform and updates the page
function updatePlatform(platform) {
    //load content
    if (platform == null || platform == undefined || platform == "#undefined" || platform == "") {
        return;
    }
    
    selectPlatform(platform);
}

// this function will be called when an app has been registered
// successfully, so that user can not change the platform once the
// app has been registered
function disablePlatformSelection() {
    var anchors = $("#pickPlatform ul li a");
    for (var index = 0; index < anchors.length; ++index) {
        anchors[index].disabled = true;
    }
    $("#pickPlatformDisableDiv").show();
}
function SetAppTypeBasedOnPlatform(id) {
    //move setup card to the top so the user can see what they just clicked
    if (id == "option-ios" || id == "option-android") {
        // update the app type in app registration
        $("#appTypeField").val("Native App");

        //disable the items
        $("#signOnUrlFieldGroup").hide();
    }
    else {
        $("#appTypeField").val("Web App");
        $("#signOnUrlFieldGroup").show();
    }
}

function startCodingContentDisplay(selectedItem) {
    console.log(selectedItem.id);
    console.log(platformId);
    $(selectedItem).closest(".tabs").find(".selected").removeClass("selected");
    if (selectedItem.id === 'option-QuickInstructions') {
        setDocumentationDivForPlatform(platformId, "gettingStartedFile", "write-code-from-scratch");
        $("#use-starter-project").hide();
        $("#write-code-from-scratch").show();
        $("#editOnGithub").show();
    }
    else {
        $("#use-starter-project").show();
        $("#write-code-from-scratch").hide();
        $("#editOnGithub").hide();

    }
    $(selectedItem).addClass("selected");
}

function setRedirectUri(platformId) {
    switch (platformId) {
        case "option-ruby":
            $("#redirectUriField").val("http://localhost:3000");
            $("#signOnUrlField").val("http://localhost:3000");
            break;
        case "option-php":
            $("#redirectUriField").val("http://localhost");
            $("#signOnUrlField").val("http://localhost");
            break;
        case "option-dotnet":
            $("#redirectUriField").val("http://localhost:10800");
            $("#signOnUrlField").val("http://localhost:10800");
            break;
        default:
            $("#redirectUriField").val("http://localhost:8000");
            $("#signOnUrlField").val("http://localhost:8000");
    }
}
function selectPlatform(platform) {

    //load content
    if (platform == null || platform == undefined) {
        return;
    }

    if ($("#SetupPlatform:visible").length == 0) {
        cardTracker.showCardNoScroll("SetupPlatform");
    }
    else {
        //remove selected from closes element
        $(platform).closest(".tabs").find(".selected").removeClass("selected");
    }
    $(platform).addClass("selected");

    //track platform clicked on
    platformId = platform.id || platform.replace("#", "");
    $('#post-download-instructions').hide();

    //platformName = platform.innerText;
    //save this platform info  on server
    SetAppTypeBasedOnPlatform(platformId);
    setRedirectUri(platformId);
    if (selectPlatform.FirstTime == true) {
        cardTracker.removeBlockingCard(false);
        selectPlatform.FirstTime = false;
    }

    //fileType = setupFile //Hardcoded as this will not chnage ; divName is also Hardcoded
    setDocumentationDivForPlatform(platformId, "setupFile", "ShowDocumentationDiv");

    var urltosend = "/GettingStarted/Main/platform/" + platformId;

    var dataTosend = {
        "platformid": platformId
    };
    dataTosend = AddAntiForgeryToken(dataTosend);
    /* Note: we dont need to do any error handling here*/
    $.ajax({
        url: urltosend,
        type: "POST",
        data: dataTosend,
    });

    ga('send', 'event', 'O365path-Rest', 'Setup-' + platformId);
}

// add a static proeprty in selectPlatform
selectPlatform.FirstTime = true;
