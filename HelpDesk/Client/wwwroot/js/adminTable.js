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
                    row.find('.resolved-by-dropdown').val(newValue);
                    /*row.find('#resolved-by-dropdown').hide(); // Hide the dropdown*/
                    row.data('resolved-by', newValue);
                    console.log('ResolvedBy updated successfully:', response);
                    var statusCell = row.find('.status-cell');
                    statusCell.text('InProgress');
                })
                .catch(function (error) {
                    console.error('Error updating ResolvedBy:', error);
                    var resolvedByDropdown = row.find('.resolved-by-dropdown');
                    resolvedByDropdown.val(resolvedByDropdown.data('current-value'));
                    row.data('resolved-by', resolvedByDropdown.data('current-value'));
                });
        });

        $('#cancelUpdate').off('click').on('click', function () {
            $('#resolvedByModal').modal('hide');
            // Retrieve the resolved-by dropdown value from the data attribute and set it in the dropdown
            var resolvedByDropdown = row.find('.resolved-by-dropdown');
            resolvedByDropdown.val(resolvedByDropdown.data('current-value'));
            row.data('resolved-by', resolvedByDropdown.data('current-value'));
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


                        var ticketIdCell = $('<td></td>').text(ticket.ticketId)
                        var emailCell = $('<td></td>').text(ticket.email);
                        var subcategoryCell = $('<td></td>').text(ticket.subCategoryName);
                        var riskLevelCell = $('<td></td>').html(getRiskLevelLabel(ticket.riskLevel));
                        var statusCell = $('<td class="status-cell"></td>').text(getStatusLabel(ticket.statusLevel));
                        var finishedDate = new Date(ticket.finishedDate);
                        var finishedDate = ticket.finishedDate ? new Date(ticket.finishedDate) : null;
                        var formattedDate = finishedDate ? finishedDate.toLocaleDateString('en-GB', {
                            day: '2-digit',
                            month: '2-digit',
                            year: 'numeric' // Use 'numeric' for 4-digit year
                        }) : "";

                        var finishedDateCell = $('<td class="finished-date-cell"></td>').text(formattedDate);

                        var resolutionNoteCell = $('<td></td>');
                        var descriptionCell = $('<td></td>');
                        var resolvedByCell = $('<td></td>');

                        // Add resolved by dropdown value based on subcategory

                        var resolvedByDropdown = $('<select class="resolved-by-dropdown"></select>');
                        resolvedByDropdown.append($('<option></option>').attr('value', '').text('-- Select Resolved By --'));

                        var subcategory = ticket.subCategoryName;

                        var filteredEmployees = [];
                        if (subcategory === 'Login Issue' || subcategory === 'Forgot Password Issue') {
                            filteredEmployees = employees.filter(function (employee) {
                                return employee.roleName === 'Developer';
                            });
                        } else if (subcategory === 'Parking Reimbursement Issue' || subcategory === 'Transportation Reimbursement Issue' || subcategory === 'Overtime Reimbursement Issue') {
                            filteredEmployees = employees.filter(function (employee) {
                                return employee.roleName === 'Finance';
                            });
                        }

                        filteredEmployees.forEach(function (employee) {
                            resolvedByDropdown.append($('<option></option>').attr('value', employee.employeeGuid).text(employee.fullName));
                        });

                        resolvedByDropdown.data('current-value', ticket.resolvedBy);
                        resolvedByDropdown.val(ticket.resolvedBy);
                        resolvedByCell.append(resolvedByDropdown);
                        row.append(resolvedByCell);

                        // Add attachment

                        var attachmentCell = $('<td></td>');

                        if (ticket.attachment != null) {
                            var attachmentLink = $('<a href="#" data-bs-toggle="modal" data-bs-target="#exampleModal-' + ticket.guid + '"></a>');
                            var attachmentImg = $('<img src="data:image/jpg;base64,' + ticket.attachment + '" width="100px" alt="Image" />');
                            attachmentLink.append(attachmentImg);
                            attachmentCell.append(attachmentLink);

                            var modalDiv = $('<div class="modal fade" id="exampleModal-' + ticket.guid + '" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true"></div>');
                            var modalDialogDiv = $('<div class="modal-dialog"></div>');
                            var modalContentDiv = $('<div class="modal-content"></div>');
                            var modalHeaderDiv = $('<div class="modal-header"></div>');
                            var modalTitle = $('<h5 class="modal-title" id="exampleModalLabel">Image</h5>');
                            var modalCloseButton = $('<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>');
                            var modalBodyDiv = $('<div class="modal-body justify-items-center align-items-center"></div>');
                            var modalBodyImg = $('<img src="data:image/jpg;base64,' + ticket.attachment + '" width="400px" alt="Image" />');
                            var modalFooterDiv = $('<div class="modal-footer"></div>');
                            var closeButton = $('<button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>');

                            modalHeaderDiv.append(modalTitle);
                            modalHeaderDiv.append(modalCloseButton);
                            modalBodyDiv.append(modalBodyImg);
                            modalFooterDiv.append(closeButton);
                            modalContentDiv.append(modalHeaderDiv);
                            modalContentDiv.append(modalBodyDiv);
                            modalContentDiv.append(modalFooterDiv);
                            modalDialogDiv.append(modalContentDiv);
                            modalDiv.append(modalDialogDiv);

                            $('body').append(modalDiv);
                        }

                        /*
                                                var dropdown = $('<select class="status-dropdown"></select>');
                                                dropdown.append($('<option value="0">Delivered</option>'));
                                                dropdown.append($('<option value="1">Accepted</option>'));
                                                dropdown.append($('<option value="2">Rejected</option>'));
                                                dropdown.append($('<option value="3">InProgress</option>'));
                                                dropdown.append($('<option value="4">Done</option>'));
                        
                                                dropdown.val(ticket.statusLevel);
                                                statusCell.html(dropdown);
                        */
                        var descriptionCell = $('<td></td>');
                        var viewDescription = $('<button class="btn btn-primary view-description">Detail</button>');
                        viewDescription.data('note', ticket.description);
                        descriptionCell.append(viewDescription);

                        // Add view to resolution note
                        var resolutionNoteCell = $('<td></td>');
                        var viewButton = $('<button class="btn btn-primary view-button">View</button>');
                        viewButton.data('note', ticket.resolutionNote);
                        resolutionNoteCell.append(viewButton);

                        // Add rejected button
                        var actionCell = $('<td></td>');
                        var rejectButton = $('<button class="btn btn-danger reject-button">Reject</button>');
                        rejectButton.data('ticket-guid', ticket.guid);
                        actionCell.append(rejectButton)


                        row.append(ticketIdCell, emailCell, subcategoryCell, riskLevelCell, statusCell, resolvedByCell, descriptionCell, attachmentCell, resolutionNoteCell, finishedDateCell, actionCell);
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


$(document).on('click', '.reject-button', function () {
    var ticketGuid = $(this).data('ticket-guid');

    $('#confirmationModal').modal('show');

    $('#confirmationModal').data('ticket-guid', ticketGuid);

    rejectButton.prop('disabled', true);
    rejectButton.addClass('disabled');
});


$(document).on('click', '#confirmRejectButton', function () {
    var ticketGuid = $('#confirmationModal').data('ticket-guid');

    updateComplaintStatus(ticketGuid, 1)
        .done(function () {
            var row = $('tr').filter(function () {
                return $(this).data('ticket-guid') === ticketGuid;
            });

            var statusCell = row.find('.status-cell');
            statusCell.text('Rejected');
            $('#confirmationModal').modal('hide');
        })
        .fail(function (error) {
            console.error('Error updating status:', error);
        });
});


$(document).on('click', '.view-description', function () {
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


/*function getStatusDropdown(status) {
    var dropdown = $('<select class="status-dropdown"></select>');
    dropdown.append($('<option value="0">Delivered</option>'));
    dropdown.append($('<option value="1">Accepted</option>'));
    dropdown.append($('<option value="2">Rejected</option>'));
    dropdown.append($('<option value="3">InProgress</option>'));
    dropdown.append($('<option value="4">Done</option>'));

    dropdown.val(status);

    return dropdown;
}*/

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
