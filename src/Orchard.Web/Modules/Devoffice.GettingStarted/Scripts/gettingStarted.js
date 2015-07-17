
function AppAndOptionsMenu(appMenuID, optionsMenuID) {
    var data = [
        {
            "primaryItem":
                { "name": "Word" },
            "imagesrc": "TBD",
            "secondaryItems": [
                {
                    "name": "Build an add-in",
                    "path": "addin"
                }
            ]
        },
        {
            "primaryItem":
                { "name": "Excel" },
            "imagesrc": "TBD",
            "secondaryItems": [
                {
                    "name": "Build an add-in",
                    "path": "addin"
                }
            ]
        },
        {
            "primaryItem":
                { "name": "PowerPoint" },
            "imagesrc": "TBD",
            "secondaryItems": [
                {
                    "name": "Build an add-in",
                    "path": "addin"
                }
            ]
        },
        {
            "primaryItem":
                { "name": "Outlook" },
            "imagesrc": "TBD",
            "secondaryItems": [
                {
                    "name": "Build an add-in",
                    "path": "addin"
                },
                {
                    "name": "Connect to Office APIs (Mail, Calender, Contacts)",
                    "path": "rest"
                }
            ]
        },
        {
            "primaryItem":
                { "name": "SharePoint" },
            "imagesrc": "TBD",
            "secondaryItems": [
                {
                    "name": "Build an add-in",
                    "path": "addin"
                }
            ]
        },
        {
            "primaryItem":
                { "name": "OneDrive" },
            "imagesrc": "TBD",
            "secondaryItems": [
                {
                    "name": "Connect to Office APIs (Files)",
                    "path": "rest"
                }
            ]
        }
    ];

    var appMenu = "#" + appMenuID;
    var options = "#" + optionsMenuID;
    MenuItems(appMenuID, optionsMenuID, data);
    this.selectedApp = function () {
        return $(appMenu).val();
    };

    this.selectedOption = function () {
        return $(options).val();
    };

    this.getPath = function () {
        var app = $(appMenu).val();
        var option = $(options).val();
        var path = data[app]["secondaryItems"][option]["path"];
        return path;
    }
}

function MenuItems(primaryMenuID, secondaryMenuID, menuData) {
    var selectedPrimary;
    var selectedSecondary;
    var primaryMenuSelector = "#" + primaryMenuID;
    var secondaryMenuSelector = "#" + secondaryMenuID;
    var data = menuData;

    this.getSelectedPrimary = function () {
        return $(primaryMenuSelector).val();
    }
    this.getSelectedSecondary = function () {
        return $(secondaryMenuSelector).val();
    }
    function buildPrimaryMenu() {

        // build product menu
        var appHtml = "";
        for (var i = 0; i < data.length; ++i) {
            var valueHtml = "value = " + i;
            appHtml += "<option value ='" + i + "'>" + data[i]["primaryItem"]["name"] + "</option>";
        }

        $(primaryMenuSelector).html(appHtml);
    }

    function primaryMenuChange(newPrimary) {
        selectedPrimary = newPrimary;
        // build options menu
        var primaryIndex = $(primaryMenuSelector).val();
        var secondaryItems = data[primaryIndex]["secondaryItems"];
        optionsHtml = "";
        for (var i = 0; i < secondaryItems.length; ++i) {
            optionsHtml += "<option value='" + i + "'>" + secondaryItems[i]["name"] + "</option>";
        }
        $(secondaryMenuSelector).html(optionsHtml);
    }

    function secondaryMenuChange(newSecondary) {
        selectedSecondary = newSecondary;
        console.log("App= " + selectedPrimary + ", options = " + selectedSecondary);
    }

    // do initialization
    // build app menu, select default app as "word" and show its options
    buildPrimaryMenu();
    selectedPrimary = data[0]["primaryItem"]["name"];
    primaryMenuChange(selectedPrimary);

    // register events
    $(primaryMenuSelector).on("change", function (event) {
        primaryMenuChange(event.target.value);
    });

    $(secondaryMenuSelector).on("change", function (event) {
        secondaryMenuChange(event.target.value);
    });


};

function ProductTable(tableId) {

    var tableSelector = "#" + tableId;
    var data = [
    {
        app: "Outlook",
        platform: "Rest APIs for Outlook",
        platformURL: "/home/rest/outlook"
    },
    {
        app: "Excel",
        platform: "Rest APIs for Excel",
        platformURL: "/home/rest/excel"
    },
    {
        app: "PowerPoint",
        platform: "Add-Ins for PowerPoint",
        platformURL: "/home/addin/powerpoint"
    },
    {
        app: "Word",
        platform: "Add-Ins for Word",
        platformURL: "/home/addin/word"
    },
    {
        app: "Excel",
        platform: "Add-Ins for Excel",
        platformURL: "/home/addin/excel"
    }
    ];

    this.build = function () {
        //build header
        var headerHtml = "<thead><td>Product</td><td>Platform</td></thead>";

        //build rows
        var rowHtml = "";
        for (var i = 0; i < data.length; ++i) {
            var cell1 = "<td>" + data[i]["app"] + "</td>";
            var anchor = "<a href=" + data[i]["platformURL"] + ">" + data[i]["platform"] + "</a>";
            var cell2 = "<td>" + anchor + "</td>";
            rowHtml += "<tr>" + cell1 + cell2 + "</tr>";
        }
        var html = "<table class='productsTable'>" + headerHtml + rowHtml + "</table>";
        $(tableSelector).append(html);
    }
}
