//ratingDevX - add to a page to have a window appear at bottom for feedback
//
//requires jQuery to be loaded on host page
//
//usage: hosting html page should have 
//		* div element where the ratings div will be loaded
//		* script reference to the ratings.js 
//		* variable that creates a new ratingDevX() object passing the div element name and the folder location where the ratingDevX files live
// 
//testing: scroll to the location of the element where the rating control was initialized and ratings should appear
//      * a cookie is stored if feedback is submitted, so the rating control will not appear again
//      * to clear this cookie, run "o.clearCookie()" from the console
//
// Script.Include("ratingsDevX/rating.js", "ratingsDevX/rating.min.js").AtHead();
// ...
// <script>
//     var o = new ratingDevX("layout-footer", "@Url.Content("~/Modules/Devoffice.GettingStarted/Scripts/ratingsDevX")");
// </script>
//
//
function ratingDevX(divLoad, folder) {
	var ratingHelpfulValue;
	var pageName = document.location.href.split('\\').pop().split('/').pop().split('?')[0].split('#')[0];
	var cookieName = "ratingDevX-" + pageName;
	var ratingObject = {
        page : pageName,
		userPrompted : false,		//did user see the rating window
		isPageHelpful : undefined,	//did they answer Yes (true) or No (false) that the page is useful
		userComments : ""			//user's verbatim comments
	}
	
	this.init = function () {
		$(document).ready(function() {
			var div = $("#" + divLoad).append($("<div id='ratingDevX'></div>"));
			$("#ratingDevX").load(folder +"/rating.html", function(response, status, xhr) {
				setupControlHandlers();
				showRatingIfNeeded();
			});
			$("head").append($("<link rel='stylesheet' href='" + folder + "/rating.css' type='text/css'/>"));
		}); 

		//Load script for telemetry logging	
		initTelemetry();
	}

	function setupControlHandlers() {
		$(document).scroll(function () {
			showRatingIfNeeded();
		})
		
		$("#s_ratingYes").click(function() {
			ratingHelpfulValue = true;	
			$("#s_ratingSection1").hide();
			$("#s_ratingSection2").show();
		})
		
		$("#s_ratingNo").click(function() {
			ratingHelpfulValue = false;		
			$("#s_ratingSection1").hide();
			$("#s_ratingSection2").show();
		})

		$("#s_ratingSubmit").click(function() {
		
			$("#s_ratingSection2").hide();
			$("#s_ratingSection3").show();
			closeRating();
		})

		$("#s_ratingSkipThis").click(function() {
		
			$("#s_ratingSection2").hide();
			$("#s_ratingSection3").show();
			closeRating();
		})
		
		$(".closeSmallWhite").click(function() {
			$("#responsiveRating").removeClass("show");
		})

		function updateCharCount() {
			var maxLength = $(this).attr("maxLength");
			$("#feedbackTextCounter").text(maxLength - $(this).val().length);
		}

		$("#s_ratingText").keyup(updateCharCount).keypress(updateCharCount);
	}

	function closeRating() {
		ratingObject.userPrompted = true;
		ratingObject.isPageHelpful = ratingHelpfulValue;
		ratingObject.userComments = $("#s_ratingText").val();
		writeLog(JSON.stringify(ratingObject));
		document.cookie = cookieName + "=" + ratingHelpfulValue;
		window.setTimeout (function () {$("#responsiveRating").removeClass("show")}, 1000);
	}
	
	this.clearCookie = function() {
		//used while debugging to clear session cookie
		document.cookie = cookieName + "=; expires=Thu, 01 Jan 1970 00:00:01 GMT;";
 
	}

	function hasUserResponded() {
		return document.cookie.indexOf(cookieName) > -1
	}
	
	function showRatingIfNeeded() {
		if (!hasUserResponded()) {
			var pos = viewPos(divLoad);
			if (pos.inViewPartial) {
				$("#responsiveRating").addClass("show");
			}
		}
	}

	    //Return an object that represents where the element is vertically on the page
	function viewPos(id) {
		id = id[0] == "#" ?  id : "#" + id;
		var $el = $(id);
		var $window = $(window);

		var docViewTop = $window.scrollTop();
		var docViewBottom = docViewTop + window.innerHeight;

		var elTop = $el.offset().top;
		var elBottom = elTop + $el.height();
		var topInView =  (elTop >= docViewTop) && (elTop <= docViewBottom);
		var bottomInView = (elBottom >= docViewTop) && (elBottom <= docViewBottom);
		var aboveMiddleTopInView = (elTop <= (docViewBottom - docViewTop) / 2);

		var elVisible = _isVisible(id);

		return { inViewFull: (topInView && bottomInView && elVisible),
		    inViewPartial: ((topInView || bottomInView) && elVisible),
		    aboveTop: (elBottom <= docViewTop),
		    belowBottom: (elTop >= docViewBottom),
		    aboveMiddle: aboveMiddleTopInView
		};
	}
	
		//Return whether the element is visible (and its parent elements)
	function _isVisible(id) {
		//set id to jquery element selector if not already
		id = id[0] == "#" ?  id : "#" + id;
	    var el = $(id);

    	return $(el).is(":visible");
	}

	//JavaScript helper to load javascript library and notify completion 
	function getScript(url, callback) {
		var script = document.createElement("script")
		script.type = "text/javascript";
		if (script.readyState) { //IE
			script.onreadystatechange = function () {
				if (script.readyState == "loaded" || script.readyState == "complete") {
					script.onreadystatechange = null;
					if (typeof callback == "function") callback();
				}
			};
		} else { //Others
			script.onload = function () {
				if (typeof callback == "function") callback();
			};
		}
		script.src = url;
		document.getElementsByTagName("head")[0].appendChild(script);
	}

	function initTelemetry(callback) {
		var telemetryScript = "https://appsforoffice.microsoft.com/telemetry/lib/1.0/hosted/AppsTelemetry.js";
		getScript(telemetryScript, function () {
			//Init telemetry
			var telemetryOptions = {};
			telemetryOptions["appVersion"] = "1.0";
			AppsTelemetry.initialize("ratingDevX", telemetryOptions);				
			if (callback != undefined) {
				callback();
			}
		})		
	}
	
	function writeLog(msg) {
		if (AppsTelemetry != undefined) {
			AppsTelemetry.sendLog(AppsTelemetry.TraceLevel.info, msg);
		}
		else {
			initTelemetry(function() {
				AppsTelemetry.sendLog(AppsTelemetry.TraceLevel.info, msg);
			})
		}
	}	
	
	//call the init function as part of instantiation
	this.init();	
}


