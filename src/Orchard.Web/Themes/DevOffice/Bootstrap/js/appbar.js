//
// L0 classes
//
// 3/2014 - johrob
//
//
//


window.WPCPub = window.WPCPub || {};
window.WPCPub.AppBar = window.WPCPub.AppBar || {};

//
// constructor
//
// initialize variables and place handlers
//
//

var _cpubappbar = function($el, opts) {

	this.$el = $el;
	// this won't be valid until it's been rendered
	this.$cntr = $(".cntr", $el);

	

	this.$el.empty();

	this.tileWidth = this.$el.data('tile-width') || 128;
	this.tileHeight = this.$el.data('tile-height') || 128;
	this.dataSrc = this.$el.data('src') || "js/appdata.js";
	this.docWidth = $(document).width();
	this.lastX = -1;
	this.lastY = -1;
	this.lastXOffset = -1;

//	this.$item_links = this.$items.children('a');


	// set styles
//	this.$items.first().addClass('open');
//	this.$items.not(':first-child').addClass('closed');

	//
	// attach event handlers
	//
//	this.$item_links.on("click", this, this.onClick);

	$(window).on("mousemove", this, this.onMouseMove );

	setInterval($.proxy(this.onTimer, this), 20);
	this.onTimer();



	this.init();

}






_cpubappbar.prototype.init = function() {

	// request appdata
	$.ajax( this.dataSrc, {
		dataType: 'jsonp',
		cache: 'true',
		jsonp: false
	});
}





_cpubappbar.prototype.onDataLoaded = function(data) {
	var $cntr = $('<div>', { 'class': 'cntr'}),
		$row1 = $('<div>', { 'class': 'row'} ),
		$row2 = $('<div>', { 'class': 'row'} );

	//console.log('got appbar data');

	$cntr.append([$row1, $row2]);
	this.$el.append($cntr);

	// this won't be valid until it's been rendered
	this.$cntr = $cntr;

	//console.dir(data);

	var pageWidth = Math.max($(window).width(), 1920);
		pageWidthInTiles = Math.floor(pageWidth / this.tileWidth) + 2,
		totalTiles = data.length,
		rowStyle = {"width": (this.tileWidth * pageWidthInTiles).toString() + "px" };

	$row1.css(rowStyle);
	$row2.css(rowStyle);
	
	for(var i = 0; i < pageWidthInTiles; i++ ) {
		var row1Idx = (i % totalTiles),
			row2Idx = ((i+10) % totalTiles);

		this.renderTile(data[row1Idx], $row1 );
		this.renderTile(data[row2Idx], $row2 );
	}
}

_cpubappbar.prototype.renderTile = function(data, $el) {
	var $tile = $("<div>", { "class": "tile"}),
		$page1 = $("<div>", { "class": "p1" }),
		$page2 = $("<div>", { "class": "p2 cp4"}),
		$icon = $("<div>", {"class": "ai-spr ai" + data.sequence.toString() }),
		$rating_cntr =  $("<div>", {"class": "rating-cntr"});

	$page1.append($icon);
	$page2.append( $("<div>", {"text": data.title, "class": "title" }));
	$page2.append( $("<div>", {"text": "By: " + data.publisher, "class": "publisher" }));
	var width = (data.rating * 10).toString() + "%";
	$rating_cntr.append( $("<div>", {"class": "rating", "style": "width: " + width }));
	$rating_cntr.append( $("<div>", {"class": "l0-spr stars" }));
	$page2.append( $rating_cntr );

	$tile.append([$page1, $page2]);
	$el.append($tile);
}


_cpubappbar.prototype.onMouseMove = function (ev) {
	var self = ev.data;


	self.currentX = ev.pageX;

}

_cpubappbar.prototype.onTimer = function(ev) {

	if(this.lastX  != this.currentX )
	{
		this.lastX = this.currentX;

		var pagePct = this.lastX / this.docWidth,
			adjust = -Math.round(pagePct * this.tileWidth);

		if(adjust != this.lastXOffset)
		{

			this.lastXOffset = adjust;
			// note, this has to be rounded or floored. using any level of precision will invoke subpixel rendered 
			// in some browsers and cause flickering for some things like the rating bar. Yay!
			this.$cntr.css({'margin-left': adjust.toString() + "px"});
		}
	}

}


//
//
//
//
//
//
//
//
//

_cpubappbar.prototype.onClick = function(ev) {
	var self = ev.data,
		$cur = $('.open', self.$el),
		$next = $(ev.target).parents('li'),
		h = 0;

	console.dir(ev);

	// previous
	$cur.removeClass('open');
	$cur.addClass('closed');

	// next one
	$next.css('height', 'auto');
	$next.removeClass('closed');
	$next.addClass('open');

	ev.preventDefault();
}



//
//
//
//
//
//
//
//
//



window.WPCPub.AppBar = _cpubappbar;



//
// Startup
//
//
//
//
//
//
//

(function(){
	//
	WPCPub._appbar = new window.WPCPub.AppBar($('#appbar'));


})();
