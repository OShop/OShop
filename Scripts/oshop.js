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