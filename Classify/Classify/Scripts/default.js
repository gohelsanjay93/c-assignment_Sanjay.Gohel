(function($) { "use strict";


    $(document) .ready(function(){
        //layer slider
        jQuery("#layerslider").layerSlider({
            responsive: false,
            responsiveUnder: 1280,
            layersContainer: 1280,
            skin: 'noskin',
            hoverPrevNext: false,
            skinsPath: '../layerslider/skins/'
        });

        //owl carousel
        var owl = $('.owl-carousel');
        owl.owlCarousel({
            items:10,
            dots:false,
            nav: true,
            loop:true,
            margin:2,
            autoplay:true,
            autoplayTimeout:2500,
            autoplayHoverPause:true,
            responsiveClass:true,
            navText: [
                "<i class='fa fa-angle-left'></i>",
                "<i class='fa fa-angle-right'></i>"
            ],
            responsive: {
                0: {
                    items: 1,
                    dots: true
                },
                600: {
                    items: 2,
                    dots: true
                },
                1000: {
                    items: 4,
                    dots: true
                }
            }

        });
        //tabber
        $("span.tab").on("click", function(){
            if($(this).hasClass("active")!=true)
            {
                $('.tab').removeClass("active");
                $(this).addClass("active");
                var id = $(this).attr("id");
                $('.content').hide();
                $("."+id).show();
            }
        });
        //back to top
        var offset = 220;
        var duration = 500;
        jQuery(window).scroll(function() {
            if (jQuery(this).scrollTop() > offset) {
                jQuery('.back-to-top').fadeIn(duration);
            } else {
                jQuery('.back-to-top').fadeOut(duration);
            }
        });

        jQuery('.back-to-top').on("click", function(event) {
            event.preventDefault();
            jQuery('html, body').animate({scrollTop: 0}, duration);
            return false;
        })

        //accordion
        $(".accordion-tab").on("click", function(){
            if($(this).hasClass("active")!=true)
            {
                $('.accordion-tab').removeClass("active");
                $(this).addClass("active");
                var id = $(this).attr("id");
                $('.accordion-content').slideUp();
                $("."+id).slideDown();
            }
        });

    });



})(jQuery);
