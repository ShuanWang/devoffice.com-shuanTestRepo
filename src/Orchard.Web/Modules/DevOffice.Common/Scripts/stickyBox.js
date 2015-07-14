

    function sticky_relocate() {
        var scroll_top = $(this).scrollTop(); // get scroll position top
        var height_element_parent = $(".zone-content").height(); //get high parent element
        var height_element = $("#sticky").height(); //get high of elemenet
        var position_fixed_max = height_element_parent - height_element; // get the maximum position of the element

        if ($(document).width() >= 900) {

            var position_fixed = scroll_top < $('#sticky-anchor').offset().top
                ? $('#sticky-anchor').offset().top - scroll_top
                : position_fixed_max > scroll_top ? 0 : position_fixed_max - scroll_top;
            $("#sticky").css("top", position_fixed);
            updateFilterMinSize();
        } else {
            
            $("#sticky").css("top", 0);
        }
        
    }

function updateFilterMinSize() {
    $('.code-filter').css("min-height", $("#sticky").height());
}

$(function () {        
      
    $(window).scroll(sticky_relocate);
    sticky_relocate();

    $('.filter-list a').click(updateFilterMinSize);

    //set min size to accomodate for the filter being larger than the results set
    $('.panel').on('hidden.bs.collapse', function (e) {
        if ($(document).width() >= 900) {
            updateFilterMinSize();
        }
    })

    $('.panel').on('show.bs.collapse', function (e) {
        if ($(document).width() >= 900) {
            setTimeout(updateFilterMinSize, 200);
        }
            
    })
        
});
