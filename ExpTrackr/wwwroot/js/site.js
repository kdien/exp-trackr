$(function () {
    // Make the whole budget row clickable
    $("#BudgetTable tbody tr")
        .click(function () {
            window.location = $(this).find("a[href*='Expense']").attr("href");
        })
        .hover(function () {
            $(this).toggleClass("hover");
        });

    // Click plus button to show and hide form
    $(".CreateCategoryButton").click(function () {
        $(this).find("i").toggleClass("rotate");
        $(this).toggleClass("button-click");
        $(".CreateCategoryForm").toggle(400);
        $(".CreateCategoryForm input[type='text']").focus();
    });

    // Click edit button to show and hide form
    $(".EditCategoryButton").click(function (e) {
        e.preventDefault();
        $(".EditCategoryForm").toggle(400);
    });

    // If form has error then show it by default
    $("form span[class*='field-validation-valid']").each(function () {
        if ( $(this).text().length ) {
            $(this).closest("form").show();
            $("html, body").animate({ scrollTop: $(this).offset().top });
        }
    });
});