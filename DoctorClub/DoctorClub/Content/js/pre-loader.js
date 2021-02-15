;(function ($) {
    "use strict";

    /*============= preloader js css =============*/
    var cites = [];
    cites[0] = "This forum is open to all doctors around the world!";
    cites[1] = "Easily find your doctor.";
    cites[2] = "Connecting all the doctors in the world together.";
    cites[3] = "Here we guarantee that you will not be alone";
    cites[4] = "You will discover your answer here!";
    var cite = cites[Math.floor(Math.random() * cites.length)];
    $('#preloader p').text(cite);
    $('#preloader').addClass('loading');

    $(window).on( 'load', function() {
        setTimeout(function () {
            $('#preloader').fadeOut(500, function () {
                $('#preloader').removeClass('loading');
            });
        }, 500);
    })

})(jQuery)