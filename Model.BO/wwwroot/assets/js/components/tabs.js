// Pure CSS

$(document).ready(() => {
    // You should update active tab with JS
    $('.tabs > a.tab').on('click', (event) => {
        $(event.target).parent('.tabs').find('> a.tab.active').removeClass('active');
        $(event.target).addClass('active');
        // Improving UX with smoothing scrolling
        if (!$(event.target.hash).length) return;
        $('body').stop().animate({
            scrollTop: $(event.target.hash).offset().top - $('#tabs').outerHeight()
        }, 300, 'swing');
    });
})