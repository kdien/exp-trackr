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
    $("#CreateCategoryButton").click(function () {
        $(this).find("i").toggleClass("rotate");
        $(this).toggleClass("button-click");
        $("#CreateCategoryForm").toggle(400);
    });

    // If form has error then show it by default
    if ($("form span[class*='field-validation']").html().length > 0) {

        $("form").show();

        $("html, body").animate({
            scrollTop: $("form span[class*='field-validation']").offset().top
        });
    }
});