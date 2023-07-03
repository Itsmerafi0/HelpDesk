$(document).ready(function () {
    loadTicketDetails();

    $('#ticketsTable').on('change', '.status-dropdown', function () {
        var row = $(this).closest('tr');
        var ticketGuid = row.data('ticket-guid');

        var currentStatus = row.data('status'); // Store the current status

        var newStatus = $(this).val();
        row.data('status', getStatusLabel(newStatus));

        $('#confirmationModal').modal('show');

        var currentRow = row;
        var newStatusValue = newStatus;

        $('#confirmStatusUpdate').click(function () {
            $('#confirmationModal').modal('hide');

            updateComplaintStatus(ticketGuid, newStatusValue)
                .then(function (response) {
                    currentRow.find('.status-cell').html(getStatusDropdown(newStatusValue))
                    currentRow.data('status', newStatusValue);
                })
                .catch(function (error) {
                    console.error('Error:', error);
                    currentRow.find('.status-cell').html(getStatusDropdown(currentStatus))
                    currentRow.data('status', currentStatus);
                });
        });

        $('#cancelStatusUpdate').click(function () {
            $('#confirmationModal').modal('hide');
            currentRow.find('.status-cell').html(getStatusDropdown(currentStatus))
            currentRow.data('status', currentStatus);
        });
    });
});


function loadTicketDetails() {

    $.ajax({
        url: 'https://localhost:7024/api/ticket/ticketdetail',
        type: 'GET',
        dataType: 'json',
        headers: {
            'Authorization': 'Bearer ' + jwToken
        },
        success: function (response) {
            var tickets = response.data;
            var tbody = $('#ticketsTable tbody');

            for (var i = 0; i < tickets.length; i++) {
                var ticket = tickets[i];
                var row = $('<tr></tr>');
                row.data('ticket-guid', ticket.guid);
                row.data('status', ticket.statusLevel);

                var guidCell = $('<td></td>').text(ticket.guid);
                var ticketIdCell = $('<td></td>').text(ticket.ticketId);
                var requesterCell = $('<td></td>').text(ticket.requester);
                var emailCell = $('<td></td>').text(ticket.email);
                var categoryCell = $('<td></td>').text(ticket.categoryName);
                var subcategoryCell = $('<td></td>').text(ticket.subCategoryName);
                var riskLevelCell = $('<td></td>').html(getRiskLevelLabel(ticket.riskLevel));
                var statusCell = $('<td class="status-cell"></td>').text(getStatusLabel(ticket.statusLevel));
                var descriptionCell = $('<td></td>').text(ticket.description);
                var resolvedByCell = $('<td></td>').text(ticket.resolvedBy);
                var resolutionNoteCell = $('<td></td>').text(ticket.resolutionNote);

                var dropdown = $('<select class="status-dropdown"></select>');
                dropdown.append($('<option value="0">Delivered</option>'));
                dropdown.append($('<option value="1">Accepted</option>'));
                dropdown.append($('<option value="2">Rejected</option>'));
                dropdown.append($('<option value="3">InProgress</option>'));
                dropdown.append($('<option value="4">Done</option>'));

                dropdown.val(ticket.statusLevel);
                statusCell.html(dropdown);


                row.append(guidCell, ticketIdCell, requesterCell, emailCell, categoryCell, subcategoryCell, riskLevelCell, statusCell,  descriptionCell, resolvedByCell , resolutionNoteCell);
                tbody.append(row);
            }
        },
        error: function (error) {
            console.error('Error:', error);
        }
    })
}

function getStatusDropdown(status) {
    var dropdown = $('<select class="status-dropdown"></select>');
    dropdown.append($('<option value="0">Delivered</option>'));
    dropdown.append($('<option value="1">Accepted</option>'));
    dropdown.append($('<option value="2">Rejected</option>'));
    dropdown.append($('<option value="3">InProgress</option>'));
    dropdown.append($('<option value="4">Done</option>'));

    dropdown.val(status);

    return dropdown;
}

function getRiskLevelLabel(riskLevel) {
    var riskLevelMap = {
        0: '<span class="badge bg-success">Low</span>',
        1: '<span class="badge bg-warning text-dark">Medium</span>',
        2: '<span class="badge bg-danger">High</span>'
    };

    return riskLevelMap[riskLevel] || "";
}

function getStatusLabel(status) {
    var statusLevelMap = {
        "0": "Delivered",
        "1": "Accepted",
        "2": "Rejected",
        "3": "OnProgress",
        "4": "Done"
    };

    return statusLevelMap[status] || "";
}

function updateComplaintStatus(ticketGuid, newStatus) {
    return $.ajax({
        url: `https://localhost:7024/api/Resolution/UpdateStatus?ticketGuid=${ticketGuid}&newStatus=${newStatus}`, // endpoint
        type: 'POST',
        headers: {
            'Authorization': 'Bearer ' + jwToken
        },
        contentType: 'application/json',
        data: JSON.stringify({
            ticketGuid: ticketGuid,
            newStatus: newStatus
        }),
        dataType: 'json'
    });
}

