(function ($) {
    /*
        Shipping details
        EditorTemplates/Parts/Shipping.cshtml
    */
    function DisplayShippingDetails() {
        if ($("#Shipping_RequiresShipping").prop("checked")) {
            $("#shipping-details").show();
        } else {
            $("#shipping-details").hide();
        }
    };

    $("#Shipping_RequiresShipping").on("change", DisplayShippingDetails);

    DisplayShippingDetails();

    /*
        Order editing utilities
    */
    $(".oshop-editable button[name=edit]").on("click", function () {
        var $container = $(this).parents(".oshop-editable");
        var fieldid = $container.data("fieldid");
        $container.addClass("oshop-edited");
        $container.removeClass("oshop-editable");
        $container.find("#" + fieldid + "_IsUpdated").val("True");
    });
    $(".oshop-editable button[name=cancel]").on("click", function () {
        var $container = $(this).parents(".oshop-edited");
        var fieldid = $container.data("fieldid");
        $container.addClass("oshop-editable");
        $container.removeClass("oshop-edited");
        $container.find("#" + fieldid + "_IsUpdated").val("False");
    });
    $("form").on("submit", function () {
        $(this).find(".oshop-editable > .edit").remove();
    });

})(jQuery);