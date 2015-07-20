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
    var inScrollMax = 0;
    var cardIDs = [];
    var blockingCards = [];
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
		console.log("----------start scroll-----------");
		for (i=0; i<cardIDs.length; i++)
		{
		    if (i == blockingCards[0]+1) {
		        break;
		    }
		    //UNDONE: is this code used?
		    var top = $("#" + cardIDs[i]).position().top - $("#" + navBarID).height() - 10; 
			var item = $(listItems[i]);
			if(lastCardSaved===false && doctop <= top) {
				savestepToCache(i-1);
				lastCardSaved = true;
			}
		    //Show that the card is completed in navbar if it is in view
			var elView = _viewPos(cardIDs[i]);
			var inView = elView.inViewPartial;
			var aboveTop = elView.aboveTop;
			console.log("Card " + cardIDs[i] + " -> inView: " + inView + " aboveTop: " + aboveTop);
			if (inView && !aboveTop) {
			    _animateCard(cardIDs[i]);
		    }
			item.toggleClass(doneClassName, inView || aboveTop);
		}
		//sticky nav bar
		$("#"+navBarID).toggleClass('navbar-fixed-top', doctop > 70);

	}
	
	//@cardTrackerWidthFactor=> width of the card tracker nav bar
	this.init = function (cardTrackerWidthFactor) {
		$("#" + navBarID).addClass("navBar");

		buildCardTracker(cardTrackerWidthFactor);
		_hideBlockingCards();

		//Add some animation when user clicks on a nav item
		$(".navItem").click(function (e) {
		    //e.preventDefault();
            _showCard($(this).attr("href"));  //should be something like #2
		});
		startFromLastStep();
	}
	
	//@	cardIndex: index of the card to store in cookie
	function savestepToCache(cardIndex) {
		//$.cookie('currentCardIndex', cardIndex, {expires:7,path:'/'});
	}
	
	// returns the card index from cookie
	function getstepFromCache() {
		//return ($.cookie('currentCardIndex'));
	}

	// starts from the last saved state
	// if the last state is not present, it will start from 0.
	function startFromLastStep()
	{
		var idx = getstepFromCache();
		if(idx ===undefined || idx === "0") { /* no cookie found, start from 0*/
			var listItems = $("#"+navBarListID+" li");			
			$(listItems[0]).addClass(doneClassName); // this step is necessary, else, you will not see the the first item in nav bar
			return;
		}
		document.getElementById(navbarAnchorItemIDTag+idx).click();
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
	this.removeBlockingCard=function() {
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
				_showCard(cardIDs[startIndex]);
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
	this.showCard = function (id) {
	    _showCard(id);
	}

	function _showCard(id) {
		//set id to jquery element selector if not already
		id = id[0] == "#" ?  id : "#" + id;
	    var card = $(id);
	    card.show({done: function() {
	    	var navBar = $("#" + navBarID);	    	
	        var navBarHeight = navBar.height();
			var scrollTo = card.offset().top - navBarHeight - (!navBar.hasClass('navbar-fixed-top') ? navBarHeight : 0) ;


			inScrollMax = scrollTo;
			if (!inScroll) {
			    $({ myScrollTop: $(window).scrollTop() }).animate({ myScrollTop: scrollTo }, {
			        duration: 800,
			        easing: 'swing',
			        step: function (val) {
			            inScroll = val < inScrollMax;
			            $(window).scrollTop(val);
			        }
			    });
			}
			//_animateCard(id);
	    	}
	    });
	}

	this.showCardNoScroll = function (id) {
		id = id[0] == "#" ?  id : "#" + id;	    
	    $(id).show();
	}

    // Hide the specified card
	this.hideCard = function (id) {
		id = id[0] == "#" ?  id : "#" + id;	    
	    $(id).hide();
	}

	function _animateCard(id) {
	    //animate card
		id = id[0] == "#" ?  id : "#" + id;	    
	    console.log("animating card " + id);
	    var aniCard = $(id);
	    aniCard.addClass("animated fadeInUp");
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
		id = id[0] == "#" ?  id : "#" + id;
		var $el = $(id);
		var $window = $(window);

		var docViewTop = $window.scrollTop();
		var docViewBottom = docViewTop + $window.height();

		var elTop = $el.offset().top;
		var elBottom = elTop + $el.height();
		var topInView =  (elTop >= docViewTop) && (elTop <= docViewBottom);
		var bottomInView = (elBottom >= docViewTop) && (elBottom <= docViewBottom);

		var elVisible = _isVisible(id);

		return { inViewFull: (topInView && bottomInView && elVisible),
		    inViewPartial: ((topInView || bottomInView) && elVisible),
		    aboveTop: (elBottom <= docViewTop),
		    belowBottom: (elTop >= docViewBottom)
		};
	}

	//Return whether the element is visible (and its parent elements)
	function _isVisible(id) {
		//set id to jquery element selector if not already
		id = id[0] == "#" ?  id : "#" + id;
	    var el = $(id);

    	return $(el).is(":visible");
	}
}
