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
        var $addressSelector = $(this);
        var UpdatePreview = function () {
            $(addressPreviewPlaceholder).empty();
            $(addressPreviewPlaceholder).load(addressPreviewPath + $addressSelector.val());
        };
        UpdatePreview();
        $addressSelector.change(UpdatePreview);

        return this;
    };
    $.fn.OShopStateSelector = function (statesServicePath, countrySelector) {
        var $stateSelectorHolder = $(this);
        var $stateSelectorControl = $(this).children("select");

        var HideEmpty = function() {
            if ($stateSelectorControl.children("option").length > 0) {
                $stateSelectorHolder.show();
            } else {
                $stateSelectorHolder.hide();
            }
        }

        $(countrySelector).change(function () {
            $stateSelectorControl.empty();
            $.getJSON(statesServicePath + $(countrySelector).val(), function (data) {
                $.each(data, function (i, state) {
                    $stateSelectorControl.append("<option value=\"" + state.id + "\">" + state.name + "</option>");
                });
                HideEmpty();
            });
        });

        HideEmpty();

        return this;
    };
}(jQuery));

