$(document).ready(function () {
    loadTicketDetails();

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
        url: 'https://localhost:7024/api/Ticket/TicketDetailfinance',
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
                        //row.data('description', ticket.description);
                        row.data('resolved-by', ticket.resolvedBy)

                        var guidCell = $('<td></td>').text(ticket.guid);
                        var ticketIdCell = $('<td></td>').text(ticket.ticketId);
                        var requesterCell = $('<td></td>').text(ticket.requester);
                        var emailCell = $('<td></td>').text(ticket.email);
                        var attachmentCell = $('<td></td>');
                        var subcategoryCell = $('<td></td>').text(ticket.subCategoryName);
                        var riskLevelCell = $('<td></td>').html(getRiskLevelLabel(ticket.riskLevel));
                        var statusCell = $('<td class="status-cell"></td>').text(getStatusLabel(ticket.statusLevel));
                        var descriptionCell = $('<td class="description-cell"></td>');
                        var resolvedByCell = $('<td class="resolved-by-cell"></td>').text(ticket.resolvedBy);
                        var resolutionNoteCell = $('<td></td>')
                        var finishedDate = new Date(ticket.finishedDate);
                        var finishedDate = ticket.finishedDate ? new Date(ticket.finishedDate) : null;
                        var formattedDate = finishedDate ? finishedDate.toLocaleDateString('en-GB', {
                            day: '2-digit',
                            month: '2-digit',
                            year: 'numeric' // Use 'numeric' for 4-digit year
                        }) : "";

                        var finishedDateCell = $('<td class="finished-date-cell"></td>').text(formattedDate);

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

                        var descriptionCell = $('<td></td>');
                        var viewDescription = $('<button class="btn btn-primary view-description">Detail</button>');
                        viewDescription.data('description', ticket.description);
                        descriptionCell.append(viewDescription);


                        // Add view button to resolution note cell

                        var viewButton = $('<button class="btn btn-primary view-button">View</button>');
                        viewButton.data('ticket-guid', ticket.guid);
                        viewButton.data('note', ticket.resolutionNote);
                        resolutionNoteCell.append(viewButton);

                        // Add button done
                        var actionCell = $('<td></td>');
                        var doneButton = $('<button class="btn btn-success done-button">Done</button>');
                        doneButton.data('ticket-guid', ticket.guid);
                        actionCell.append(doneButton)


                        row.append(ticketIdCell, requesterCell, emailCell, subcategoryCell, riskLevelCell, statusCell, attachmentCell, resolvedByCell, finishedDateCell, descriptionCell, resolutionNoteCell, actionCell);
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

$(document).on('click', '.view-description', function () {
    var desc = $(this).data('description');
    $('#descriptionModal .modal-body').text(desc);
    $('#descriptionModal').modal('show');

    $('#closedescriptionModal').off('click').on('click', function () {
        $('#descriptionModal').modal('hide');
    });
});

$(document).on('click', '.done-button', function () {
    var ticketGuid = $(this).data('ticket-guid');

    $('#confirmationModal').modal('show');

    $('#confirmationModal').data('ticket-guid', ticketGuid);

    rejectButton.prop('disabled', true);
    rejectButton.addClass('disabled');
});

$(document).on('click', '#confirmDoneButton', function () {
    var ticketGuid = $('#confirmationModal').data('ticket-guid');

    updateComplaintStatus(ticketGuid, 3)
        .done(function () {
            var row = $('tr').filter(function () {
                return $(this).data('ticket-guid') === ticketGuid;
            });

            var statusCell = row.find('.status-cell');
            statusCell.text('Done');
            $('#confirmationModal').modal('hide');
        })
        .fail(function (error) {
            console.error('Error updating status:', error);
        });
});

$(document).on('click', '.view-button', function () {
    var note = $(this).data('note');
    $('#resolutionNoteTextarea').val(note);
    $('#resolutionNoteModal').modal('show');
    var viewButton = $(this);

    $('#updateResolutionNoteButton').off('click').on('click', function () {
        var updatedNote = $('#resolutionNoteTextarea').val();
        var ticketGuid = viewButton.data('ticket-guid');

        $.ajax({
            url: `https://localhost:7024/api/Resolution/UpdateResolutionNotes?resolutionGuid=${ticketGuid}&notes=${encodeURIComponent(updatedNote)}`,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify({
                resolutionGuid: ticketGuid,
                notes: updatedNote
            }),
            dataType: 'json',
            success: function (response) {
                console.log('Resolution note updated successfully:', response);
                viewButton.data('note', updatedNote);
                $('#resolutionNoteModal').modal('hide');
            },
            error: function (error) {
                // Handle the error response
                console.error('Error updating resolution note:', error);
            }
        });
    });

    $('#closeResolutionNoteModal').off('click').on('click', function () {
        $('#resolutionNoteModal').modal('hide');
    });
});



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
        "0": "Requested",
        "1": "Rejected",
        "2": "InProgress",
        "3": "Done"
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
            console.log('Status updated successfully:', updateResponse); // Handle success response
        },
        error: function (updateError) {
            console.error('Error updating status:', updateError); // Handle error response
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
            console.log('ResolvedBy updated successfully:', updateResponse); // Handle success response
        },
        error: function (updateError) {
            console.error('Error updating ResolvedBy:', updateError); // Handle error response
        }
    });
}