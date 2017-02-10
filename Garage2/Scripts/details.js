(function () {
    // Add event listener for opening and closing details
    $('#detailsTable').on('click', '.details-control', function () {
        var tr = $(this).closest('tr');

        if (tr.next().hasClass('hidden')) {
            // This row is already open - close it
            tr.next().removeClass('hidden');
        }
        else {
            tr.next().addClass('hidden');
        }
    });
})()