(function(jQuery) {
    jQuery.fn.lazyload = function(options) {
        var settings = {
            threshold    : 0,
            failurelimit : 0,
            event        : "scroll",
            effect       : "show",
            container    : window
        };    
        if(options) {
            jQuery.extend(settings, options);
        }
        var elements = this;
        if ("scroll" == settings.event) {
            jQuery(settings.container).bind("scroll", function(event) {
                var counter = 0;
                elements.each(function() {
                    if (jQuery.abovethetop(this, settings)) {
                    } else if (!jQuery.belowthefold(this, settings)) {
                            jQuery(this).trigger("appear");
                    } else {
                        if (counter++ > settings.failurelimit) {
                            return false;
                        }
                    }
                    $(this).removeAttr("width").removeAttr("height");
                });
                var temp = jQuery.grep(elements, function(element) {
                    return !element.loaded;
                });
                elements = jQuery(temp);
            });
        }
        
        this.each(function() {
            var self = this;
            if (undefined != jQuery(self).attr("original")){
				self.loaded = false;
				jQuery(self).one("appear", function() {
					if (!this.loaded) {
						jQuery("<img />")
							.bind("load", function() {
								jQuery(self)
									.hide()
									.removeAttr("height")
									.attr("src", jQuery(self).attr("original").replace("202.96.138.158","cdn.verydemo.com"))
									[settings.effect](settings.effectspeed);
								self.loaded = true;
							})
							.attr("src", jQuery(self).attr("original").replace("202.96.138.158","cdn.verydemo.com"));
					};
				});
			}
        });
        jQuery(settings.container).trigger(settings.event);
        return this;
    };
	function checkshow(){}
    jQuery.belowthefold = function(element, settings) {
        if (settings.container === undefined || settings.container === window) {
            var fold = jQuery(window).height()*2 + jQuery(window).scrollTop();
        } else {
            var fold = jQuery(settings.container).offset().top + jQuery(settings.container).height()*2;
        }		
        return fold <= jQuery(element).offset().top - settings.threshold;

    };
	
    jQuery.abovethetop = function(element, settings) {
        if (settings.container === undefined || settings.container === window) {
            var fold = jQuery(window).scrollTop();
        } else {
            var fold = jQuery(settings.container).offset().top;
        }
        return fold >= jQuery(element).offset().top + settings.threshold  + jQuery(element).height();
    };
    jQuery.extend(jQuery.expr[':'], {
        "below-the-fold" : "jQuery.belowthefold(a, {threshold : 0, container: window})",
        "above-the-fold" : "!jQuery.belowthefold(a, {threshold : 0, container: window})"
    });
    
})(jQuery);
$(function(){
	jQuery("#verydemo_content_div img[original]").lazyload({
		effect:"fadeIn"
	});
});