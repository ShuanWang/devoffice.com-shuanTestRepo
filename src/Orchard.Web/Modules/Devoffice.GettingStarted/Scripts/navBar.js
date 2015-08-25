/*Read me
* How to use:
* create an object of this class in in your html page, providing the ids for the card container, that contains all the cards to be shown
* add the class "cardTrackerContainer" in the cardContainer
* 
* In your HTML file create an object of CardTracker class and call init(), in document ready and register updateScroll() with $(document).scroll
*
*
	Sample Code to put in HTML:
	==========================
	var cardTracker = new CardTracker("o365-progressTrackerContainer","myNavBar");
	$(document).ready(function() {
		cardTracker.init(0.5);
	});
	$(document).scroll(function(){
		cardTracker.updateScroll();
	});	

    Features:
    1. Blocking card:
    if you need to proceed further only when a particular action has been take,
    mark that div as "isblocking= true"
*/

// class: CardTracker
//@cardsContainerID=> id of the card container, that holds all the cards
//@navBarID=> id of the nav bar that is for scroll tracker
function CardTracker(cardsContainerID, navBarID) {
    var doneClassName = "card-done";
    var inScroll = false;
    var cardIDs = [];
    var blockingCards = [];
    var animatedCards = [];
    var navBarListID = "navBarList";
	var navbarAnchorItemIDTag = "navbarAnchorItem";
	var navItemIDTag = "navItem";
	var cardsContainerID = cardsContainerID;
	var navBarID = navBarID;
	var cardTrackerWidth=[-1,-1,49.9,33.9,24.9,20,16.6/* Hack for rest path */,14.9,12.9,11.9];

	this.updateScroll = function() {
		var listItems = $("#"+navBarListID+" li");
		var doctop = $(document).scrollTop();
		var lastCardSaved = false;
		var aniCard = -1;
		//console.log("----------start scroll-----------");
		for (i=0; i<cardIDs.length; i++)
		{
		    if (i == blockingCards[0]+1) {
		        break;
		    }
		    //Show that the card is completed in navbar if it is in view
			var elView = _viewPos(cardIDs[i]);
			var inView = elView.inViewPartial;
			var aboveTop = elView.aboveTop;
			var aboveMiddle = elView.aboveMiddle;
			var dontAnimate =  $("#" + cardIDs[i]).hasClass("dontAnimate");
			//console.log("Card " + cardIDs[i] + " -> inView: " + inView + " aboveTop: " + aboveTop);
			if (aboveMiddle && !dontAnimate) {
			    _animateCard(cardIDs[i]);
		    }
			var item = $(listItems[i]);
			item.toggleClass(doneClassName, aboveMiddle);
        }
		//sticky nav bar
		$("#"+navBarID).toggleClass('navbar-fixed-top', doctop > 70);

	}
	
	//@cardTrackerWidthFactor=> width of the card tracker nav bar
	this.init = function (cardTrackerWidthFactor) {
	    $("#" + navBarID).addClass("cardNavBar");

	    buildCardTracker(cardTrackerWidthFactor);
	    _hideBlockingCards();

	    //Add some animation when user clicks on a nav item
	    $(".navItem").click(function (e) {
	        //e.preventDefault();
	        _showCard($(this).attr("href")); 
	    });

        //Don't animate the first card
	    if (cardIDs.length > 0) {
	        $("#" + cardIDs[0]).addClass("dontAnimate");
	    };

	    //Mark all cards that were in the view on load, so we don't animate them
	    //for (i = 1; i < cardIDs.length; i++) {
	    //    var elView = _viewPos(cardIDs[i]);
	    //    var inView = elView.inViewPartial;
	    //    var aboveTop = elView.aboveTop;
	    //    if (inView || aboveTop) {
	    //        $("#" + cardIDs[i]).addClass("dontAnimate");
	    //    }
	    //}

	    // mark first card done
	    var listItems = $("#" + navBarListID + " li");
	    var item = $(listItems[0]);
	    item.addClass(doneClassName);

	}

	// builds the card tracker
	function buildCardTracker(cardTrackerWidthFactor) {

		var cardContainer = $("#"+cardsContainerID);
		if(cardContainer.length>1) {
			console.log("ERROR: No card container found");
		}

		var navBarContainer=$("#" +navBarID);
		if(navBarContainer.length>1) {
			console.log("ERROR: No nav bar found");
		}
		
		// add a list in navbar container
		navBarContainer.append("<ul id='" + navBarListID +"'></ul>");
		var orderedList = $("#" + navBarListID);
		
		//find all the childNodes in cardContainer containing class ""card""
		var cards = cardContainer.find(".card");
		var width = cardTrackerWidth[cards.length]*cardTrackerWidthFactor;
		if(cards.length ===0) {
			console.log("no cards found in cardContainer");
			return;
		}
		
		// now read each card
		// store the id of each card for later use
		// create an anchor that links to each card e.g. <a href="#cardid"></a>
		// create a list item (i.e. nav bar item) and use anchor link
	    // add the list item to list
		var blockingCardAlreadySeen = false;
		var linkingCardID;
		for(var i=0; i<cards.length; ++i) {
			var name = cards[i].getAttribute("name");
			var cardID = cards[i].getAttribute("id");
			if (cardID === null) {
			    console.log("nullid: Please make sure every card has an id");
			}
			cardIDs.push(cardID);
			var isBlocking = cards[i].getAttribute("isBlocking");
			if (isBlocking === "true") {
			    blockingCards.push(i);
                // add href
			    if (blockingCardAlreadySeen == false) {
			        linkingCardID = cardID;
			        blockingCardAlreadySeen = true;
			    }
            }
			var url;
			var ahrefId = "id = '" + navbarAnchorItemIDTag+"-"+cardID +"'";
			if (blockingCardAlreadySeen ==true) {
			    url = "<a " + ahrefId + " href = " + "'#" + linkingCardID + "' class='navItem'>" + name + "</a>";
			}
			else {
			    url = "<a " + ahrefId + " href = " + "'#" + cardID + "' class='navItem'>" + name + "</a>";
			}
			var navItemId = "id = '" + navItemIDTag + "-" + cardID + "'";
			var listItem = "<li "+ navItemId+" style='width:" + width + "%;'>" + url + "</li>";
			orderedList.append(listItem);
        }
	}

	// this should be called everytime you need to remove the blocking card
	// it shows the cards which are present between current and next blocking card
	// updates the href link in the navbars 
	this.removeBlockingCard=function(scrollNextIntoView) {
	    scrollNextIntoView = typeof scrollNextIntoView === 'boolean' && scrollNextIntoView;

		var startIndex = blockingCards[0] + 1;// work on next one
		var nextBlockingCardID="" ;
		var nextBlockingCardIndex = cardIDs.length+1; // set it to max
		blockingCards.splice(0, 1); // remove first item from the array

		if (blockingCards.length != 0) {
			nextBlockingCardIndex = blockingCards[0];
			nextBlockingCardID = cardIDs[nextBlockingCardIndex];
		}

		var scrolled = false;
		for (; startIndex < cardIDs.length; ++startIndex) {
			if (startIndex <= nextBlockingCardIndex) {
				// set hef to its own card
				var item = $("#" + navbarAnchorItemIDTag + "-" + cardIDs[startIndex]);
				item.attr("href", "#" + cardIDs[startIndex]);
				if (!scrolled) {
				    scrolled = true;
				    if (scrollNextIntoView) {
				        this.showCard(cardIDs[startIndex]);
				    } else {
				        this.showCardNoScroll(cardIDs[startIndex]);
				    }
				} else {
				    $("#" + cardIDs[startIndex]).show();
				}
			} else {
				var item = $("#" + navbarAnchorItemIDTag + "-" + cardIDs[startIndex]);
				item.attr("href", "#" + cardIDs[nextBlockingCardIndex]);
			}
		}
	}

	this.cardIndex=function(cardID) {
	    return cardIDs.indexOf(cardID);
	}

    // Hides all the blocking card for the first time
	this.hideBlockingCards = function (afterIndex) {
	    _hideBlockingCards(afterIndex);
    }

	function _hideBlockingCards(afterIndex) {
	    afterIndex = afterIndex || 0;

	    if (blockingCards.length < 0) {
	        return;
	    }
	    var cardID = cardIDs[blockingCards[afterIndex]];
	    var blockingCardIndex = blockingCards[afterIndex];
	    ++blockingCardIndex;
	    // hide all the cards after blocking card
	    for (; blockingCardIndex <= cardIDs.length; ++blockingCardIndex) {
	        $("#" + cardIDs[blockingCardIndex]).hide();
	    } // end for
	}

    // Scroll the specified card into view
	this.showCard = _showCard = function (id) {
	    //set id to jquery element selector if not already
		id = id[0] == "#" ?  id : "#" + id;
		var card = $(id);

	    //show the card
		card.show();

        //scroll it to just under the navbar
	    var navBar = $("#" + navBarID);	    	
	    var navBarHeight = navBar.height() + 10;
		var scrollTo = card.offset().top - navBarHeight - (!navBar.hasClass('navbar-fixed-top') ? navBarHeight : 0) ;

        //move the scroll to the top and animate it over time
		$("html,body").animate({
		    duration: 1000,
            scrollTop: scrollTo
		});

    }

	this.showCardNoScroll = function (id) {
		id = id[0] == "#" ?  id : "#" + id;	    
		//console.log("showCardNoScroll: " + id);
		$(id).show();
	}

    // Hide the specified card
	this.hideCard = function (id) {
		id = id[0] == "#" ?  id : "#" + id;	    
	    $(id).hide();
	}

	function _animateCard(id) {
	    //animate card

	    //Removing animation for now
		//id = id[0] == "#" ?  id : "#" + id;	    
	    //console.log("animating card " + id);
	    //var aniCard = $(id);
	    //aniCard.addClass("animated fadeInUp dontAnimate");
    }

	this.removeAllBlockingCards = function () {
	    for (var i = 0; i <= blockingCards.length; ++i) {
	        this.removeBlockingCard();
	    }
	}
	
    //Return the height of the document
	function getDocumentHeight() {
	    return Math.max(
            Math.max(document.body.scrollHeight, document.documentElement.scrollHeight),
            Math.max(document.body.offsetHeight, document.documentElement.offsetHeight),
            Math.max(document.body.clientHeight, document.documentElement.clientHeight)
        );
	}

    //Return an object that represents where the element is vertically on the page
	function _viewPos(id) {
	    id = id[0] == "#" ? id : "#" + id;
	    var $el = $(id);
	    var $window = $(window);

	    var docViewTop = $window.scrollTop();
	    var docViewBottom = docViewTop + $(window).height();

	    var elTop = $el.offset().top;
	    var elBottom = elTop + $el.height();
	    var topInView = (elTop >= docViewTop) && (elTop <= docViewBottom);
	    var bottomInView = (elBottom >= docViewTop) && (elBottom <= docViewBottom);
	    var aboveMiddleTopInView = (elTop <= ((docViewBottom - docViewTop) / 2) + docViewTop);  //top of element is past the middle of the window
	    var wholeScreen = (elTop <= docViewTop) && (elBottom >= docViewBottom); //element is taking up entire page and is running above and below window       

	    var elVisible = _isVisible(id);
	    var view = {
	        inViewFull: (topInView && bottomInView && elVisible),
	        inViewPartial: (((topInView || bottomInView) || (wholeScreen)) && elVisible),
	        aboveTop: (elBottom <= docViewTop),
	        belowBottom: (elTop >= docViewBottom),
	        aboveMiddle: aboveMiddleTopInView,
	        wholeScreen: wholeScreen
	    };

	    //console.log(id, JSON.stringify(view));

	    return view;
	}

	//Return whether the element is visible (and its parent elements)
	function _isVisible(id) {
		//set id to jquery element selector if not already
		id = id[0] == "#" ?  id : "#" + id;
	    var el = $(id);

    	return $(el).is(":visible");
	}

	this.isInBlockingList = function (id) {
	    if (blockingCards.length == 0) { return false; }
	    var cardIndex = cardIDs.indexOf(id);
	    var found = blockingCards.indexOf(cardIndex);
	    //console.log("isInBlockingList: " + found, "for: " + id);
	    return (found != -1);
	}
}
