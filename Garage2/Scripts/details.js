(function () {
    // Add event listener for opening and closing details
    $('#detailsTable').on('click', '.details-control', function () {
        var tr = $(this).closest('tr');
        var row = tr.next();

        if (row.hasClass('hidden')) {
            if (!row.hasClass('loaded')) {
                var url = $(this).data('request-url');
                row.load(url);
                row.addClass('loaded');
            }
            row.removeClass('hidden');
        }
        else {
            row.addClass('hidden');
        }
    });
})();