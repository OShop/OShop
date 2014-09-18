(function ($) {
    function DisplayShippingDetails() {
        if ($("#Shipping_RequiresShipping").prop("checked")) {
            $("#shipping-details").show();
        } else {
            $("#shipping-details").hide();
        }
    };

    $("#Shipping_RequiresShipping").on("change", DisplayShippingDetails);

    DisplayShippingDetails();

})(jQuery);