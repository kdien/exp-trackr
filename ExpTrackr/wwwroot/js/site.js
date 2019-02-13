$(function () {
    // Make the whole budget row clickable
    $("#BudgetTable tbody tr")
        .click(function () {
            window.location = $(this).find("a[href*='Expense']").attr("href");
        })
        .hover(function () {
            $(this).toggleClass("hover");
        });

    $("#CreateCategoryButton").click(function () {
        $(this).find("i").toggleClass("rotate");
        $(this).toggleClass("button-click");
        $("#CreateCategoryForm").toggle(400);
    });
});