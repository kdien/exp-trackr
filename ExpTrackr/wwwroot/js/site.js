$(function () {
    // Add data table
    $(".data-table").DataTable({
        "aaSorting": [],
        "columnDefs": [
            { "orderable": false, "targets": 0 }
        ],
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
        "responsive": true,
        "info": false
    });

    // Make the whole budget row clickable
    $("#BudgetTable tbody tr")
        .click(function () {
            window.location = $(this).find("a[href*='Expense']").attr("href");
        })
        .hover(function () {
            $(this).toggleClass("hover");
        });

    // Hide create form by default
    $(".create-form").hide();

    // Click plus button to show and hide form
    $(".create-button").click(function () {
        $(this).find("i").toggleClass("rotate");
        $(this).toggleClass("button-click");
        $(".create-form").toggle(400);
        $(".create-form input[type='text']").focus();
    });

    // If form has error then show it by default
    $("form span[class*='field-validation']").each(function () {
        if ($(this).text().length) {
            $(this).closest("li").find("button:first").toggleClass("button-click");
            $(this).closest("li").find("button:first").find("i").toggleClass("rotate");
            $(this).closest("form").show();
            $("html, body").animate({ scrollTop: $(this).offset().top });
        }
    });
});