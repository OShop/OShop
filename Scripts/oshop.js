(function ($) {
    $(".oshop-autoupdate").change(function () {
        $(this).closest("form").submit();
    });

    $(".oshop-confirm").click(function () {
        if (!confirm($(this).data("msg"))) {
            return false;
        }
    });
})(jQuery);

(function ($) {
    $.fn.OShopAddressPreview = function (addressPreviewPath, addressPreviewPlaceholder) {
        var addressSelector = this;
        var UpdatePreview = function () {
            $(addressPreviewPlaceholder).empty();
            $(addressPreviewPlaceholder).load(addressPreviewPath + $(addressSelector).val());
        };
        UpdatePreview();
        this.change(UpdatePreview);

        return this;
    };
}(jQuery));

