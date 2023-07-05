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
                    var tickets = response.data.reverse();
                    var employees = empResponse.data;

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
                        var finishedDate = new Date(ticket.finishedDate);
                        var formattedDate = finishedDate.toLocaleDateString('en-GB', {
                            day: '2-digit',
                            month: '2-digit',
                            year: 'numeric' // Use 'numeric' for 4-digit year
                        });

                        var finishedDateCell = $('<td class="finished-date-cell"></td>').text(formattedDate);
                        var resolutionNoteCell = $('<td></td>');
                        var descriptionCell = $('<td></td>');
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


                        var attachmentCell = $('<td></td>');

                        

                        if (ticket.attachment != null) {
                            var attachmentLink = $('<a href="#" data-bs-toggle="modal" data-bs-target="#exampleModal-' + ticket.Guid + '"></a>');
                            var attachmentImg = $('<img src="data:image/jpg;base64,' + ticket.attachment + '" width="100px" alt="Image" />');
                            attachmentLink.append(attachmentImg);
                            attachmentCell.append(attachmentLink);

                            var modalDiv = $('<div class="modal fade" id="exampleModal-' + ticket.Guid + '" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true"></div>');
                            var modalDialogDiv = $('<div class="modal-dialog"></div>');
                            var modalContentDiv = $('<div class="modal-content"></div>');
                            var modalHeaderDiv = $('<div class="modal-header"></div>');
                            var modalTitle = $('<h5 class="modal-title" id="exampleModalLabel">Modal title</h5>');
                            var modalCloseButton = $('<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>');
                            var modalBodyDiv = $('<div class="modal-body justify-items-center align-items-center"></div>');
                            var modalBodyImg = $('<img src="data:image/jpg;base64,' + ticket.attachment + '" width="100px" alt="Image" />');
                            var modalFooterDiv = $('<div class="modal-footer"></div>');
                            var closeButton = $('<button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>');
                            var saveChangesButton = $('<button type="button" class="btn btn-primary">Save changes</button>');

                            modalHeaderDiv.append(modalTitle);
                            modalHeaderDiv.append(modalCloseButton);
                            modalBodyDiv.append(modalBodyImg);
                            modalFooterDiv.append(closeButton);
                            modalFooterDiv.append(saveChangesButton);
                            modalContentDiv.append(modalHeaderDiv);
                            modalContentDiv.append(modalBodyDiv);
                            modalContentDiv.append(modalFooterDiv);
                            modalDialogDiv.append(modalContentDiv);
                            modalDiv.append(modalDialogDiv);

                            $('body').append(modalDiv);
                        }


                        var dropdown = $('<select class="status-dropdown"></select>');
                        dropdown.append($('<option value="0">Delivered</option>'));
                        dropdown.append($('<option value="1">Accepted</option>'));
                        dropdown.append($('<option value="2">Rejected</option>'));
                        dropdown.append($('<option value="3">InProgress</option>'));
                        dropdown.append($('<option value="4">Done</option>'));

                        dropdown.val(ticket.statusLevel);
                        statusCell.html(dropdown);

                        var descriptionCell = $('<td></td>');
                        var viewButton = $('<button class="btn btn-primary view-button">View</button>');
                        viewButton.data('note', ticket.description);
                        descriptionCell.append(viewButton);

                        // Add view to resolution note
                        var resolutionNoteCell = $('<td></td>');
                        var viewButton = $('<button class="btn btn-primary view-button">View</button>');
                        viewButton.data('note', ticket.resolutionNote);
                        resolutionNoteCell.append(viewButton);


                        row.append(ticketIdCell,  emailCell,  subcategoryCell, riskLevelCell, statusCell, resolvedByCell, descriptionCell, attachmentCell, resolutionNoteCell, finishedDateCell);
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
    $('#descriptionModal .modal-body').text(note);
    $('#descriptionModal').modal('show');

    $('#closedescriptionModal').off('click').on('click', function () {
        $('#descriptionModal').modal('hide');
    });
});

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
        dataType: 'json',
        success: function (updateResponse) {
            // Handle the success response
            console.log('Status updated successfully:', updateResponse);
        },
        error: function (updateError) {
            // Handle the error response
            console.error('Error updating Status:', updateError);
        }

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