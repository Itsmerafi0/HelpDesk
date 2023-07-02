$(document).ready(function () {
    loadTicketDetails();
    
    $('#ticketsTable').on('click', '.status-cell', function () {
        var row = $(this).closest('tr');
        var ticketGuid = row.data('ticket-guid');
        var currentStatus = row.data('status');
        console.log(currentStatus)

        var dropdown = $('<select class="status-dropdown"></select>');
        dropdown.append($('<option value="0">Delivered</option>'));
        dropdown.append($('<option value="1">Accepted</option>'));
        dropdown.append($('<option value="2">Rejected</option>'));
        dropdown.append($('<option value="3">OnProgress</option>'));
        dropdown.append($('<option value="4">Done</option>'));

        dropdown.val(currentStatus);

        $(this).html(dropdown);
    });

    $('#ticketsTable').on('change', '.status-dropdown', function () {
        var row = $(this).closest('tr');
        var ticketGuid = row.data('ticket-guid');

        var newStatus = $(this).val();
        row.data('status', newStatus);

        updateComplaintStatus(ticketGuid, newStatus)
            .then(function (response) {
                row.find('.status-cell').text(newStatus);
                //row.data('status', newStatus);
            })
            .catch(function (error) {
                console.error('Error:', error);
            });
    });

})

function loadTicketDetails() {

    $.ajax({
        url: 'https://localhost:7024/api/Ticket/TicketDetail',
        type: 'GET',
        dataType: 'json',
        headers: {
            'Authorization': 'Bearer ' + jwToken
        },
        success: function(response) {
            var tickets = response.data;
            console.log('Tickets:', tickets); // Console log the response data
            
            var tbody = $('#ticketsTable tbody');

            for (var i = 0; i < tickets.length; i++) {
                var ticket = tickets[i];
                var row = $('<tr></tr>');
                row.data('ticket-guid', ticket.guid);
                row.data('status', ticket.statusLevel);

               /* var guidCell = $('<td></td>').text(ticket.guid);*/
                var ticketIdCell = $('<td></td>').text(ticket.ticketId);
                var requesterCell = $('<td></td>').text(ticket.requester);
                var emailCell = $('<td></td>').text(ticket.email);
                var categoryCell = $('<td></td>').text(ticket.categoryName);
                var subcategoryCell = $('<td></td>').text(ticket.subCategoryName);
                var riskLevelCell = $('<td></td>').html(getRiskLevelLabel(ticket.riskLevel));
                var statusCell = $('<td class="status-cell"></td>').text(getStatusLabel(ticket.statusLevel));
                var descriptionCell = $('<td></td>').text(ticket.description);
                var resolutionNoteCell = $('<td></td>').text(ticket.resolutionNote);

                row.append( ticketIdCell, requesterCell, emailCell, categoryCell, subcategoryCell, riskLevelCell, statusCell, descriptionCell, resolutionNoteCell);
                tbody.append(row);
            }
        },
        error: function(error) {
            console.error('Error:', error);
        }
    });
}


function getRiskLevelLabel(riskLevel) {
    var riskLevelMap = {
        "0": '<span class="badge bg-success">Low</span>',
        "1": '<span class="badge bg-warning text-dark">Medium</span>',
        "2": '<span class="badge bg-high">High</span>'
    };

    return riskLevelMap[riskLevel] || "";
}

function getStatusLabel(status) {
    var statusLevelMap = {
        "0": "Delivered",
        "1": "Accepted",
        "2": "Rejected",
        "3": "In Progress",
        "4": "Done"
    };

    return statusLevelMap[status] || "";
}

function updateComplaintStatus(ticketGuid, newStatus) {
    return $.ajax({
        url: `https://localhost:7024/api/Resolution/UpdateStatus?complainGuid=${ticketGuid}&newStatus=${newStatus}`, // endpoint
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            ticketGuid: ticketGuid,
            newStatus: newStatus
        }),
        dataType: 'json',
                headers: {
            'Authorization': 'Bearer ' + jwToken
        },

    });
}

