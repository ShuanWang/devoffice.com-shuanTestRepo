

function Link(data) {
    this.id = data.Id;
            this.title = data.Title;
            this.body = data.Body;
            this.externalLink = data.ExternalLink;
            //this.linkText = data.LinkText;
            this.tile1Title = data.Tile1Title;
            this.tile1ExternalLink = data.Tile1ExternalLink;
            this.tile1Thumbnail = data.Tile1Thumbnail;
            this.tile2Title = data.Tile2Title;
            this.tile2ExternalLink = data.Tile2ExternalLink;
            this.tile2Thumbnail = data.Tile2Thumbnail;
            this.sortorder = data.SortOrder;
}

    devOfficeApp.controller('grouplinksController', [
        '$scope', '$log', function($scope, $log) {

            $scope.editlinkHelper = [];
            $scope.imageHelper = [];
            $scope.sortIndices = [];
            $scope.isExternalLink = false;
            $scope.resourceTypeSelector = "";

            var linkCount = 0;
            $scope.links = $.map($scope.rawlinks, function(link) {
                var newlink = new Link(link);
                //var linkTextChanged = false;
                //if (newlink.linkText != null) {
                //    linkTextChanged = true;
                //}
                $scope.editlinkHelper.push({
                    id: newlink.id,
                    title: newlink.title,
                    body: newlink.body,
                    externalLink: newlink.externalLink,
                    //linkText:newlink.linkText,
                    tile1Title: newlink.tile1Title,
                    tile1ExternalLink: newlink.tile1ExternalLink,
                    tile1Thumbnail: newlink.tile1Thumbnail,
                    tile2Title: newlink.tile2Title,
                    tile2ExternalLink: newlink.tile2ExternalLink,
                    tile2Thumbnail: newlink.tile2Thumbnail,
                    //changeLinkText: linkTextChanged
                });

                linkCount++;
                return newlink;
            });
            
            $scope.getlinksAsJSONString = function() {
                return angular.toJson($scope.links);
            }

            $scope.linksAsJSONString = $scope.getlinksAsJSONString();

            $scope.$watch('links', function(newValue, oldValue) {
                $scope.linksAsJSONString = $scope.getlinksAsJSONString();
            }, true);


            //$scope.updateLinkUrlHelper = function(link, index) {
            //    $log.log('blah blah blah...');
            //    $log.log(link.url, index);
            //    createNewImage(link.url, index);
            //}

            //function createNewImage(url, index) {
            //    var img = new Image();
            //    img.onload = function() {
            //        $scope.editlinkHelper[index].url = img;
            //        $log.log('yo', index);
            //        $scope.$apply();
            //    }
            //    img.src = url;
            //    return img;
            //}

            //$scope.addlinkTitle = '';
            //$scope.addlinkType = '';


            // Adding a new item image
            $scope.newImage = null;
            $scope.newImageURL = '';
            $scope.newImageCorrectSize = null;
            $scope.addItemLinkText = '';
            $scope.addItemExternalLink = '';

            //$scope.$watch('newImageURL', function(newURL, oldURL) {
            //    $log.log('new image url changed...', newURL);
            //    var img = new Image();
            //    img.onload = function() {
            //        $scope.newImageCorrectSize = (img.width == 720 && img.height == 472);
            //        $scope.newImage = img;
            //        $log.log('new image: ', $scope.newImage.width, $scope.newImage.height);
            //        $scope.$apply();
            //    }

            //    img.src = newURL;
            //});

            $scope.removelink = function(index) {
                if (!confirm(confirmRemoveMessage)) {
                    return false;
                }
                $scope.links.splice(index, 1);
                $(".links-message").show();

                $scope.linksAsJSONString = $scope.getlinksAsJSONString();
            }
            $scope.addlink = function() {
                //grab the fields
                var title = $("#newItemTitle");
                var body = $("#newItemBody"); //add these
                var link = $("#newItemLink");
                //var linkText = $("#newItemLinkText");
                var tile1Title = $("#newItemTile1Title");
                var tile1Link = $("#newItemTile1Link"); //add these
                //var tile1Image = $("#newItemTile1Image");
                var tile2Title = $("#newItemTile2Title");
                var tile2Link = $("#newItemTile2Link"); //add these
                //var tile2Image = $("#newItemTile2Image");

                //grab the field values
                var titleValue = title.val().trim();
                var bodyValue = body.val().trim();
                var urlValue = link.val().trim();
                //var linkTextValue = 'LEARN MORE';
                //if ($scope.addItemChangeLinkText && linkText.val().trim() != "") {
                //    linkTextValue = linkText.val().trim();
                //} 

                var tile1TitleValue = tile1Title.val().trim();
                var tile1LinkValue = tile1Link.val().trim();
                var tile2TitleValue = tile2Title.val().trim();
                var tile2LinkValue = tile2Link.val().trim();

                //fields validation
                //if (titleValue == "") {
                //    title.parent().addClass("has-error");
                //    return false;
                //}

                //if (typeValue == "") {
                //    type.parent().addClass("has-error");
                //    return false;
                //}

                //if (urlValue == "") {
                //    link.parent().addClass("has-error");
                //    return false;
                //}

                $scope.links.push(new Link({
                    Title: titleValue,
                    Body: bodyValue,
                    ExternalLink: urlValue,
                   // LinkText: linkTextValue,
                    Tile1Title: tile1TitleValue,
                    Tile1ExternalLink: tile1LinkValue,
                    Tile2Title: tile2TitleValue,
                    Tile2ExternalLink: tile2LinkValue,
                    SortOrder: $scope.links.length + 1
                }));

                //reset the field values
                title.val("");
                body.val("");
                link.val("");
              //  linkText.val("");
                tile1Title.val("");
                tile1Link.val("");
                tile2Title.val("");
                tile2Link.val("");
            //    $scope.addItemChangeLinkText = false;
                
                $scope.newImageURL = '';

                $scope.linksAsJSONString = $scope.getlinksAsJSONString();

                $scope.$apply();

                $(".links-message").show();
            }

            $scope.getlinksAsJSONString = function() {
                return angular.toJson($scope.links);
            }
            $scope.updateMediaUrl = function(updatedItem, tileNumber, index) {

                var url = "/Admin/Orchard.MediaLibrary?dialog=True";
                $.colorbox({
                    href: url,
                    iframe: true,
                    reposition: true,
                    width: "100%",
                    height: "100%",
                    onLoad: function() { // hide the scrollbars from the main window
                        $('html, body').css('overflow', 'hidden');
                        $('#cboxClose').remove();
                    },
                    onClosed: function() {
                        $('html, body').css('overflow', '');
                        var selectedData = $.colorbox.selectedData;
                        $log.log(selectedData);

                        // NOTE: when grabbing images from the dialog, the path returned in selectedData[0].resource
                        // is simply the URI, not the full image path. This will cause the preview image not to load.
                        // To work around this, we have to prepend the entire window.location.hostname to ensure we
                        // can load the preview image.
                        var prefix = window.location.protocol + '//' + window.location.hostname;

                        if (selectedData == null) // Dialog cancelled, do nothing
                            return;
                        if (selectedData && updatedItem) {

                            if (tileNumber == 1) {
                                updatedItem.tile1ExternalLink = prefix + selectedData[0].resource;
                            }
                            if (tileNumber == 2) {
                                updatedItem.tile2ExternalLink = prefix + selectedData[0].resource;
                            }

                            $scope.linksAsJSONString = $scope.getlinksAsJSONString();
                        } else {
                            $log.log('adding new image...');
                            if (tileNumber == 1) {
                                $("#newItemTile1Link").val(prefix + selectedData[0].resource);
                            }
                            if (tileNumber == 2) {
                                $("#newItemTile2Link").val(prefix + selectedData[0].resource);
                            }
                           
                            $scope.newImageURL = prefix + selectedData[0].resource;
                        }
                        $scope.$apply();
                    }
                });
            }

            $('.save-button button.primaryAction').click(function() {
                $scope.linksAsJSONString = $scope.getlinksAsJSONString();
                $log.log($scope.linksAsJSONString);
            });


            $("#grouplinksAccordion").sortable({
                axis: 'y',
                containment: 'parent',
                update: function () {

                    $("#grouplinksAccordion .panel.item").each(function (index) {

                        var t = $(this).contents().find('.skypePanelTitle').html();
                        var id = $(this).attr('linkId');
                        var newSortOrder = index + 1;

                        // find item with matching id and update its sort order
                        $scope.links.filter(function(link) {
                            return link.id == id;
                        })[0].sortorder = newSortOrder;
                    });

                    $scope.linksAsJSONString = $scope.getlinksAsJSONString();
                    $scope.$apply();
                }
            });
        }
    ]);

