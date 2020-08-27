(function($) { "use strict";
    $(document).ready(function(){

        //map
        var map;
        function initialize() {

            var mapOptions = {
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                mapTypeControl: true,
                mapTypeControlOptions: {
                    style: google.maps.MapTypeControlStyle.DEFAULT,
                    position: google.maps.ControlPosition.DEFAULT
                },
                zoom: 16,
                scrollwheel: false,
                zoomControl: false,
                panControl: false,
                streetViewControl: true,
                streetViewControlOptions: {
                    position: google.maps.ControlPosition.DEFAULT
                },
                scaleControl: false,
                overviewMapControl: false,
                center: new google.maps.LatLng(41.8723889, 12.48018019999995)
            };

            map = new google.maps.Map(document.getElementById('map-canvas'),
                mapOptions);

            var mapStyles = [
                {
                    "featureType": "landscape",
                    "stylers": [
                        { "visibility": "on" }
                    ]
                },{
                    "featureType": "water",
                    "stylers": [
                        { "visibility": "on" }
                    ]
                },{
                    "featureType": "water",
                    "elementType": "labels",
                    "stylers": [
                        { "visibility": "on" }
                    ]
                },{
                    "featureType": "administrative",
                    "stylers": [
                        { "visibility": "on" }
                    ]
                },{
                    "featureType": "administrative",
                    "elementType": "labels",
                    "stylers": [
                        { "visibility": "on" }
                    ]
                },{
                    "featureType": "poi",
                    "stylers": [
                        { "visibility": "on" }
                    ]
                },{
                    "featureType": "road",
                    "stylers": [
                        { "visibility": "on" }
                    ]
                },{
                    "featureType": "transit",
                    "stylers": [
                        { "visibility": "on" }
                    ]
                }
            ];

            map.setOptions({styles: mapStyles});

            var transitLayer = new google.maps.TransitLayer();
            transitLayer.setMap(map);
        }

        google.maps.event.addDomListener(window, 'load', initialize);

        function checkResize(){

            var center = map.getCenter();
            google.maps.event.trigger(map, 'resize');
            map.setCenter(center);
        }

        window.onresize = checkResize;

    });
})(jQuery);