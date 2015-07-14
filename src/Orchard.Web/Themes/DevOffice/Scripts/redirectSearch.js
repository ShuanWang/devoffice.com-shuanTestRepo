if ((location.pathname == "/code-samples" || location.pathname == "/patterns-and-practices-resources" ||location.pathname =="/training") && location.search != "") {

    if (location.href.indexOf("?filters") != -1) {
        if (location.href.indexOf("#?") == -1) {
            var origin = location.origin;
            var pathname = location.pathname;
            var search = location.search;
            var newUrl = origin + pathname + "#" + search;
            location.replace(newUrl);
        }

    }
}