// Addins.js
// Script used in the AddinsWidget Part

var cardTracker = new CardTracker("o365-progressTrackerContainer", "myNavBar");

$(document).ready(function () {
    cardTracker.init(1.0);

    //early load xl addin iframe
    var xlFrameSrc = "https://onedrive.live.com/embed?cid=8D7305F0A8BFFBF0&resid=8D7305F0A8BFFBF0%21107&authkey=AIvgZ8lq1Fi31DU&em=2&AllowTyping=True&ActiveCell='Sheet1'!A20&wdDownloadButton=True";
    $("#xlframe")
        .css({ "display": "none" })
        .attr("src", xlFrameSrc)
        .height(600)
        .load(iframeLoaded);

    //Do OS specific things
    if (!os().isWindows) {
        console.log('OS: Not windows');
        $(".userHasVS").hide();
        $(".userHasNoVS").show();
    } else {
        console.log('OS: Windows');
        $(".userHasVS").show();
        $(".userHasNoVS").hide();
    }
});

$(document).scroll(function () {
    cardTracker.updateScroll();
});

function iframeLoaded() {
    $("#iframeLoading").removeClass("loading");
}

var taskPane = {
    'title': 'Task Pane',
    'img': { 'excel': '/Modules/Devoffice.GettingStarted/images/exceltaskpane2.PNG', 'word': '/Modules/Devoffice.GettingStarted/images/wiki.PNG', 'powerpoint': '/Modules/Devoffice.GettingStarted/images/pichit.PNG' },
    'text': 'A Task Pane add-in appears next to the document.',
    'overlaytop': { 'excel': 'http://aka.ms/Fb7hmn', 'word': 'http://aka.ms/Rtbsj3', 'powerpoint': 'http://aka.ms/Y4lf7t' },
    'overlaybottom': 'https://github.com/OfficeDev/Add-in-TaskPane-Sample'
};

var content = {
    'title': 'Content',
    'img': { 'excel': '/Modules/Devoffice.GettingStarted/images/excelcontent2.PNG', 'powerpoint': '/Modules/Devoffice.GettingStarted/images/poll.PNG' },
    'text': 'A Content add-in appears in-line with the document.',
    'overlaytop': { 'excel': 'http://aka.ms/Lgaz15', 'powerpoint': 'http://aka.ms/Pzs7nq' },
    'overlaybottom': 'https://github.com/OfficeDev/Add-in-Content-Starter'
};

var compose = {
    'title': 'Compose',
    'img': '/Modules/Devoffice.GettingStarted/images/outlookcompose.PNG',
    'text': 'Appears when the user is composing a new email',
    'overlaytop': 'http://aka.ms/Nahkwl',
    'overlaybottom': 'https://github.com/OfficeDev/Add-in-MailCompose-Sample'
};

var read = {
    'title': 'Read',
    'img': '/Modules/Devoffice.GettingStarted/images/outlookread.PNG',
    'text': 'Appears when the user is reading an email message',
    'overlaytop': 'http://aka.ms/Wwq6m4',
    'overlaybottom': 'https://github.com/OfficeDev/Add-in-MailRead-Sample'
};

function setBuildContent(options) {
    var defaults = { 
        showDefault: true, 
        showGitHub: false,
        linkGitHub: ""
    };
    var options = $.extend({}, defaults, options);

    //Only show the gitHub options for Windows PCs
    options.showDefault = options.showDefault || !os().isWindows;
    options.showGitHub = options.showGitHub && os().isWindows;

    //show/hide the content
    $('#buildDefault').toggle(options.showDefault);
    $('#buildWithGithubSample').toggle(options.showGitHub);

    $('#build-downloadFromGithub').href = options.linkGitHub;

}

function enableOption1() {
    $('#option1button').show();
}

function enableOption2() {
    $('#option2button').show();
}

function setContent(div, html) {
    cardTracker.hideCard("explore");
    div.html(html);
    cardTracker.showCard("explore");
}

function selectClient(selectedClient) {
    $('#option1button').hide();
    $('#option2button').hide();

    cardTracker.hideCard("build");
    cardTracker.hideCard("more");

    //Hide the xl addin by default
    $('#xlframe').hide();

    switch (selectedClient) {
        case 'excel':
            $("#iframeLoading").toggle($("#iframeLoading").hasClass("loading"));

            setContent($('#embedContents'), "<h3>Explore the JavaScript API in Excel</h3><p>Below, you'll see an Excel Add-in that we've built. This add-in showcases a selection of the JavaScript API. Click on one of the tiles to explore the relevant method in the API.</p><br><p>You can download this add-in and launch it in your own version of Office from the <a href='https://store.office.com/api-tutorial-for-office-WA104077907.aspx?assetid=WA104077907' target='_blank'>Office Store.</a></p><br>");

            //show the add-in frame, which should be loading in the background
            $('#xlframe').show();

            $('#option1title').text(content.title);
            $('#option1img').attr('src', content.img.excel);
            $('#option1text').text(content.text);
            $('#option1button').attr('onclick', 'selectMore("excel", "content")');

            $('#option2title').text(taskPane.title);
            $('#option2img').attr('src', taskPane.img.excel);
            $('#option2text').text(taskPane.text);
            $('#option2button').attr('onclick', 'selectMore("excel", "taskPane")');

            $('#choose-title').text('Excel supports two types of add-ins. Which one would you like to build?');

            enableOption1();
            enableOption2();

            $("#moreResourcesList").html("<li><a id='more-github' href='#' target='_blank'>Download starter sample</a><div>Download a starter project that you can continue building with using the IDE of your choice.</div></li><li><a href='http://dev.office.com/codesamples#?filters=excel,office%20add-ins' target='_blank'>More code samples</a><div>A list of other useful samples that you can check out to help build your add-ins</div></li><li><a href='https://msdn.microsoft.com/EN-US/library/office/fp142185.aspx' target='_blank'>Reference</a><div>Reference documentation for the JavaScript API for add-ins</div></li><li><a href='https://msdn.microsoft.com/EN-US/library/office/jj220073.aspx' target='_blank'>Design your add-in</a><div>Guidelines and tips to make your add-in gorgeous and easy to use</div></li><li><a href='https://msdn.microsoft.com/EN-US/library/office/fp123515.aspx' target='_blank'>Publish your add-in</a><div>Learn about the various methods you can use to deploy and publish your add-ins</div></li>");
            break;

        case 'outlook':
            setContent($('#embedContents'), "<h3>Explore Outlook Add-ins</h3><p>This short video shows you what an add-in is and gives a brief introduction to the JavaScript API that powers the add-ins model.</p><BR><iframe class='embed-responsive-item embed-responsive-16by9' height='400px' width='100%' src='https://www.youtube.com/embed/Hov8f_VniCc' frameborder='0' allowfullscreen></iframe>");

            $('#option1title').text(compose.title);
            $('#option1img').attr('src', compose.img);
            $('#option1text').text(compose.text);
            $('#option1button').attr('onclick', 'selectMore("outlook", "compose")');

            $('#option2title').text(read.title);
            $('#option2img').attr('src', read.img);
            $('#option2text').text(read.text);
            $('#option2button').attr('onclick', 'selectMore("outlook", "read")');

            $('#choose-title').text('Outlook supports two types of add-ins. Which one would you like to build?');

            enableOption1();
            enableOption2();

            $("#moreResourcesList").html("<li><a id='more-github' href='#' target='_blank'>Download starter sample</a><div>Download a starter project that you can continue building with using the IDE of your choice.</div></li><li><a href='http://dev.office.com/codesamples#?filters=office%20add-ins,outlook' target='_blank'>More code samples</a><div>A list of other useful samples that you can check out to help build your add-ins</div></li><li><a href='https://msdn.microsoft.com/EN-US/library/office/fp142185.aspx' target='_blank'>Reference</a><div>Reference documentation for the JavaScript API for add-ins</div></li><li><a href='https://msdn.microsoft.com/EN-US/library/office/jj220073.aspx' target='_blank'>Design your add-in</a><div>Guidelines and tips to make your add-in gorgeous and easy to use</div></li><li><a href='https://msdn.microsoft.com/EN-US/library/office/fp123515.aspx' target='_blank'>Publish your add-in</a><div>Learn about the various methods you can use to deploy and publish your add-ins</div></li>")
            break;

        case 'powerpoint':

            setContent($('#embedContents'), "<div><h3>Explore PowerPoint Add-ins</h3></div>This short video shows you what a PowerPoint Add-in looks like and gives a brief introduction to the JavaScript API that powers the add-ins model.<BR><BR><iframe class='embed-responsive-item embed-responsive-16by9' height='400px' width='100%' src='https://www.youtube.com/embed/tFq_dl1yUUc' frameborder='0' allowfullscreen></iframe><BR><BR>For a full list of the APIs available, check out the <a href='https://msdn.microsoft.com/EN-US/library/office/fp142185.aspx' target='_blank'>reference documentation</a>.<BR>");

            $('#option1title').text(content.title);
            $('#option1img').attr('src', content.img.powerpoint);
            $('#option1text').text(content.text);
            $('#option1button').attr('onclick', 'selectMore("powerpoint", "content")');

            $('#option2title').text(taskPane.title);
            $('#option2img').attr('src', taskPane.img.powerpoint);
            $('#option2text').text(taskPane.text);
            $('#option2button').attr('onclick', 'selectMore("powerpoint", "taskPane")');

            $('#choose-title').text('PowerPoint supports two types of add-ins. Which one would you like to build?');

            enableOption1();
            enableOption2();

            $("#moreResourcesList").html("<li><a id='more-github' href='#' target='_blank'>Download starter sample</a><div>Download a starter project that you can continue building with using the IDE of your choice.</div></li><li><a href='http://dev.office.com/codesamples#?filters=office%20add-ins,powerpoint' target='_blank'>More code samples</a><div>A list of other useful samples that you can check out to help build your add-ins</div></li><li><a href='https://msdn.microsoft.com/EN-US/library/office/fp142185.aspx' target='_blank'>Reference</a><div>Reference documentation for the JavaScript API for add-ins</div></li><li><a href='https://msdn.microsoft.com/EN-US/library/office/jj220073.aspx' target='_blank'>Design your add-in</a><div>Guidelines and tips to make your add-in gorgeous and easy to use</div></li><li><a href='https://msdn.microsoft.com/EN-US/library/office/fp123515.aspx' target='_blank'>Publish your add-in</a><div>Learn about the various methods you can use to deploy and publish your add-ins</div></li>")
            break;

        case 'word':

            setContent($('#embedContents'), "<div><h3>Explore Word Add-ins</h3></div>This short video shows you what a Word Add-in looks like and gives a brief introduction to the JavaScript API that powers the add-ins model.<BR><BR><iframe class='embed-responsive-item embed-responsive-16by9' height='400px' width='100%' src='https://www.youtube.com/embed/S23rcdX96Wc' frameborder='0' allowfullscreen></iframe><BR><BR>For a full list of the APIs available, check out the <a href='https://msdn.microsoft.com/EN-US/library/office/fp142185.aspx' target='_blank'>reference documentation</a>.<BR>");

            $('#option1title').text(taskPane.title);
            $('#option1img').attr('src', taskPane.img.word);
            $('#option1text').text(taskPane.text);
            $('#option1button').attr('onclick', 'selectMore("word", "taskPane")');

            $('#choose-title').text('You can extend Word with a Task Pane add-in. Click on the button below to learn how to get started building a Task Pane add-in.');

            $('#buildDefault').hide()
            $('#buildWithGithubSample').show();

            enableOption1();
            $('#option2button').hide();

            $("#moreResourcesList").html("<li><a id='more-github' href='#' target='_blank'>Download starter sample</a><div>Download a starter project that you can continue building with using the IDE of your choice.</div></li><li><a href='http://dev.office.com/codesamples#?filters=office%20add-ins,word' target='_blank'>More code samples</a><div>A list of other useful samples that you can check out to help build your add-ins</div></li><li><a href='https://msdn.microsoft.com/EN-US/library/office/fp142185.aspx' target='_blank'>Reference</a><div>Reference documentation for the JavaScript API for add-ins</div></li><li><a href='https://msdn.microsoft.com/EN-US/library/office/jj220073.aspx' target='_blank'>Design your add-in</a><div>Guidelines and tips to make your add-in gorgeous and easy to use</div></li><li><a href='https://msdn.microsoft.com/EN-US/library/office/fp123515.aspx' target='_blank'>Publish your add-in</a><div>Learn about the various methods you can use to deploy and publish your add-ins</div></li>")
            break;
    }
    //removing the blocking card will show the next cards
    //Don't call removeBlockingCard again since it will remove the next blocking card
    if (cardTracker.isInBlockingList("selectapp")) {
        cardTracker.removeBlockingCard();
    } else {
        cardTracker.showCardNoScroll("choosetype");
    }
}

function selectMore(selectedClient, selectedShape) {
    var buildContentOptions = {};

    cardTracker.hideCard("build");
    cardTracker.hideCard("more");

    switch (selectedShape) {
        case 'content':
            $('#more-github').attr('href', content.overlaybottom);
            if (selectedClient == 'excel') {
                $('#more-playground').attr('href', content.overlaytop.excel);
            }
            if (selectedClient == 'powerpoint') {
                $('#more-playground').attr('href', content.overlaytop.powerpoint);
                buildContentOptions = { showDefault: false, showGitHub: true, linkGitHub: "https://github.com/OfficeDev/Add-in-Content-Starter" }
            }
            break;

        case 'taskPane':
            $('#more-github').attr('href', taskPane.overlaybottom);
            if (selectedClient == 'excel') {
                $('#more-playground').attr('href', taskPane.overlaytop.excel);
            }
            if (selectedClient == 'powerpoint') {
                $('#more-playground').attr('href', taskPane.overlaytop.powerpoint);
                buildContentOptions = { showDefault: false, showGitHub: true, linkGitHub: "https://github.com/OfficeDev/Add-in-TaskPane-Sample" }
            }
            if (selectedClient == 'word') {
                $('#more-playground').attr('href', taskPane.overlaytop.word); 
                buildContentOptions = { showDefault: false, showGitHub: true, linkGitHub: "https://github.com/OfficeDev/Add-in-TaskPane-Sample" }
            }
            break;

        case 'compose':
            $('#more-github').attr('href', compose.overlaybottom);
            $('#more-playground').attr('href', compose.overlaytop);
            break;

        case 'read':
            $('#more-github').attr('href', read.overlaybottom);
            $('#more-playground').attr('href', read.overlaytop);
            break;
    }

    setBuildContent(buildContentOptions);

    //removing the blocking card will show the next cards
    //Don't call removeBlockingCard again since it will remove the next blocking card
    if (cardTracker.isInBlockingList("choosetype")) {
        cardTracker.removeBlockingCard();
    } else {
        cardTracker.showCard("build");
        cardTracker.showCardNoScroll("more");
    }

}

