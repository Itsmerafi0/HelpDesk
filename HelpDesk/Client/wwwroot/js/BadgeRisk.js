$(document).ready(function () {
    loadTicketDetails();

    $('#ticketsTable').on('change', '.status-dropdown', function () {
        var row = $(this).closest('tr');
        var ticketGuid = row.data('ticket-guid');
        var currentStatus = row.data('status'); // Store the current status
        var newStatus = $(this).val();
        row.data('status', getStatusLabel(newStatus));

        $('#confirmationModal').modal('show');


        $('#confirmStatusUpdate').click(function () {
            $('#confirmationModal').modal('hide');

            updateComplaintStatus(ticketGuid, newStatus)
                .then(function (response) {
                    row.find('.status-cell').html(getStatusDropdown(newStatus));
                    row.data('status', newStatus);
                })
                .catch(function (error) {
                    console.error('Error:', error);
                    row.find('.status-cell').html(getStatusDropdown(currentStatus));
                    row.data('status', currentStatus);
                });
        });

        $('#cancelStatusUpdate').click(function () {
            $('#confirmationModal').modal('hide');
            row.find('.status-cell').html(getStatusDropdown(currentStatus));
            row.data('status', currentStatus);
        });
    });

    $('#ticketsTable').on('change', '.resolved-by-dropdown', function (event) {
        var row = $(this).closest('tr');
        var currentValue = row.data('resolved-by');
        var newValue = $(this).val();

        var ticketGuid = row.data('ticket-guid');

        $('#resolvedByModal').modal('show');

        $('#confirmUpdate').off('click').on('click', function () {
            $('#resolvedByModal').modal('hide');
            updateResolvedBy(ticketGuid, newValue)
                .then(function (response) {
                    row.find('.resolved-by-dropdown').val(newValue); // handle success response
                    row.data('resolved-by', newValue);
                    console.log('ResolvedBy updated successfully:', response);
                })
                .catch(function (error) {
                    console.error('Error updating ResolvedBy:', error);
                    row.find('.resolved-by-dropdown').val(currentValue); // handle error response
                    row.data('resolved-by', currentValue);
                });
        });

        $('#cancelUpdate').off('click').on('click', function () {
            $('#resolvedByModal').modal('hide');
            row.find('.resolved-by-dropdown').val(currentValue); // cancel update
            row.data('resolved-by', currentValue);
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

            $.ajax({
                url: 'https://localhost:7024/api/Employee/DeveloperAndFinanceDetails',
                type: 'GET',
                dataType: 'json',
                success: function (empResponse) {
                    var employees = empResponse.data;

                    for (var i = 0; i < tickets.length; i++) {
                        var ticket = tickets[i];
                        var row = $('<tr></tr>');
                        row.data('ticket-guid', ticket.guid);
                        row.data('status', ticket.statusLevel);

                        var ticketIdCell = $('<td></td>').text(ticket.ticketId);
                        var requesterCell = $('<td></td>').text(ticket.requester);
                        var emailCell = $('<td></td>').text(ticket.email);
                        var categoryCell = $('<td></td>').text(ticket.categoryName);
                        var subcategoryCell = $('<td></td>').text(ticket.subCategoryName);
                        var attachmentCell = $('<td></td>').text(ticket.attachment);
                        var riskLevelCell = $('<td></td>').html(getRiskLevelLabel(ticket.riskLevel));
                        var statusCell = $('<td class="status-cell"></td>').text(getStatusLabel(ticket.statusLevel));
                        var finishedDateCell = $('<td class="finished-date-cell"></td>').text(ticket.finishDate);
                        var resolutionNoteCell = $('<td></td>');
                        var resolvedByCell = $('<td></td>');
                        var resolvedByDropdown = $('<select class="resolved-by-dropdown"></select>');
                        resolvedByDropdown.append($('<option></option>').attr('value', '').text('-- Select Resolved By --'));

                        //var descriptionCell = $('<td></td>').text(ticket.description)
                        employees.forEach(function (employee) {
                            if (employee.roleName === 'Developer' || employee.roleName === 'Finance') {
                                resolvedByDropdown.append($('<option></option>').attr('value', employee.employeeGuid).text(employee.roleName + " - " + employee.fullName));
                            }
                        });

                        resolvedByDropdown.val(ticket.resolvedBy);
                        resolvedByCell.append(resolvedByDropdown);
                        row.append(resolvedByCell);


                        var descriptionText = ticket.description;
                        var shortText = descriptionText.length > 25 ? descriptionText.substring(0, 25) + '...' : descriptionText;
                        var descriptionCell = $('<td></td>').addClass('description-cell');

                        var descriptionContent = $('<span class="description-content"></span>').text(shortText);
                        descriptionCell.append(descriptionContent);

                        if (descriptionText.length > 25) {
                            var showMoreLink = $('<a href="#" class="show-more-link">Show More</a>');
                            var showLessLink = $('<a href="#" class="show-less-link">Show Less</a>');

                            descriptionCell.append(showMoreLink, showLessLink);

                            showMoreLink.data('full-description', descriptionText);
                            showLessLink.data('short-description', shortText);

                            showMoreLink.on('click', descriptionHandler(showMoreLink, showLessLink));
                            showLessLink.on('click', descriptionHandler(showMoreLink, showLessLink));

                            showLessLink.hide();
                        }

                        var dropdown = $('<select class="status-dropdown"></select>');
                        dropdown.append($('<option value="0">Delivered</option>'));
                        dropdown.append($('<option value="1">Accepted</option>'));
                        dropdown.append($('<option value="2">Rejected</option>'));
                        dropdown.append($('<option value="3">InProgress</option>'));
                        dropdown.append($('<option value="4">Done</option>'));

                        dropdown.val(ticket.statusLevel);
                        statusCell.html(dropdown);

                        // Add view to resolution note
                        var resolutionNoteCell = $('<td></td>');
                        var viewButton = $('<button class="btn btn-primary view-button">View</button>');
                        viewButton.data('note', ticket.resolutionNote);
                        resolutionNoteCell.append(viewButton);


                        row.append(ticketIdCell, requesterCell, emailCell, categoryCell, subcategoryCell, riskLevelCell, statusCell, resolvedByCell, descriptionCell, attachmentCell, resolutionNoteCell, finishedDateCell);
                        tbody.append(row);
                        tbody.append(row);
                    }
                },
                error: function (empError) {
                    console.error('Error retrieving developer and finance details:', empError);
                }
            });
        },
        error: function (error) {
            console.error('Error retrieving ticket details:', error);
        }
    });
}

$(document).on('click', '.view-button', function () {
    var note = $(this).data('note');
    $('#resolutionNoteModal .modal-body').text(note);
    $('#resolutionNoteModal').modal('show');

    $('#closeResolutionNoteModal').off('click').on('click', function () {
        $('#resolutionNoteModal').modal('hide');
    });
});


function descriptionHandler(showMoreLink, showLessLink) {
    return function (e) {
        e.preventDefault();
        var $this = $(this);
        var $descriptionCell = $this.parent();
        var $descriptionContent = $descriptionCell.find('.description-content');
        var fullDescription = showMoreLink.data('full-description');
        var shortDescription = showLessLink.data('short-description');

        if ($descriptionContent.text() === shortDescription) {
            $descriptionContent.text(fullDescription);
            $this.hide();
            showLessLink.show();
        } else {
            $descriptionContent.text(shortDescription);
            $this.hide();
            showMoreLink.show();
        }
    };
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

function updateComplaintStatus(resolutionGuid, newStatus) {
    return $.ajax({
        url: `https://localhost:7024/api/Resolution/UpdateStatus?resolutionGuid=${resolutionGuid}&newStatus=${newStatus}`, // endpoint
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify({
            resolutionGuid: resolutionGuid,
            newStatus: newStatus
        }),
        dataType: 'json'
    });
}

function updateResolvedBy(resolutionGuid, resolvedBy) {
    $.ajax({
        url: `https://localhost:7024/api/Resolution/UpdateResolvedBy?resolutionGuid=${resolutionGuid}&resolvedBy=${resolvedBy}`,
        type: 'PUT',
        data: JSON.stringify({
            resolutionGuid: resolutionGuid,
            resolvedBy: resolvedBy
        }),
        dataType: 'json',
        success: function (updateResponse) {
            // Handle the success response
            console.log('ResolvedBy updated successfully:', updateResponse);
        },
        error: function (updateError) {
            // Handle the error response
            console.error('Error updating ResolvedBy:', updateError);
        }
    });
}