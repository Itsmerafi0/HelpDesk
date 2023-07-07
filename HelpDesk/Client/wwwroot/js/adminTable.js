$(document).ready(function () {
    var table;

    function loadTicketDetails() {
        $('#ticketsTable').DataTable({
            ajax: {
                url: 'https://localhost:7024/api/ticket/ticketdetail',
                type: 'GET',
                dataType: 'json',
                headers: {
                    'Authorization': 'Bearer ' + jwToken
                },
                dataSrc: 'data'
            },
            columns: [
                { data: 'ticketId' },
                { data: 'subCategoryName' },
                {
                    data: 'riskLevel',
                    render: function (data) {
                        var riskLevelMap = {
                            0: '<span class="badge bg-success">Low</span>',
                            1: '<span class="badge bg-warning text-dark">Medium</span>',
                            2: '<span class="badge bg-danger">High</span>'
                        };
                        return riskLevelMap[data] || '';
                    }
                },
                {
                    data: 'statusLevel',
                    render: function (data) {
                        var statusLevelMap = {
                            '0': '<span class="badge badge-outlined badge-info">Requested</span>',
                            '1': '<span class="badge badge-outlined badge-danger">Rejected</span>',
                            '2': '<span class="badge badge-outlined badge-primary">InProgress</span>',
                            '3': '<span class="badge badge-outlined badge-success">Done</span>'
                        };
                        return statusLevelMap[data] || '';
                    }
                },
                {
                    data: 'resolvedBy',
                    render: function (data) {
                        return '<select class="resolved-by-dropdown" data-current-value="' + data + '"></select>';
                    }
                },
                {
                    data: 'finishedDate',
                    render: function (data) {
                        if (data) {
                            var finishedDate = new Date(data);
                            return finishedDate.toLocaleDateString('en-GB', {
                                day: '2-digit',
                                month: '2-digit',
                                year: 'numeric'
                            });
                        } else {
                            return 'Not finished yet';
                        }
                    }
                },
                {
                    data: null,
                    render: function (data, type, row) {
                        var description = data.description;
                        var attachment = data.attachment;

                        var details = '<i class="fas fa-info-circle description-icon" data-description="' + description + '"></i>';

                        if (attachment) {
                            details += ' <i class="fas fa-image attachment-icon" data-image="' + attachment + '"></i>';
                        }

                        return details;
                    }
                },
                {
                    data: null,
                    render: function (data, type, row) {
                        var resolutionNote = data.resolutionNote;
                        var guid = data.guid;

                        var action = '<i class="fas fa-pencil-alt note-icon" data-note="' + resolutionNote + '"></i>';
                        action += ' <i class="fas fa-times reject-icon text-danger" data-ticket-guid="' + guid + '"></i>';

                        return action;
                    }
                }
            ],
            initComplete: function (settings, json) {
                var tickets = json.data;

                $.ajax({
                    url: 'https://localhost:7024/api/Employee/DeveloperAndFinanceDetails',
                    type: 'GET',
                    dataType: 'json',
                    success: function (empResponse) {
                        var employees = empResponse.data;

                        tickets.forEach(function (ticket) {
                            var resolvedByDropdown = $('select.resolved-by-dropdown[data-current-value="' + ticket.resolvedBy + '"]');
                            var resolutionGuid = ticket.guid;
                            resolvedByDropdown.attr('data-resolution-guid', resolutionGuid);

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

                            resolvedByDropdown.empty();

                            resolvedByDropdown.append($('<option></option>').attr('value', '').text('Select Resolved By'));

                            filteredEmployees.forEach(function (employee) {
                                var option = $('<option></option>').attr('value', employee.employeeGuid).text(employee.fullName);

                                if (ticket.resolvedBy === employee.employeeGuid) {
                                    option.attr('selected', 'selected');
                                }

                                resolvedByDropdown.append(option);
                            });
                        });

                    },
                    error: function (empError) {
                        console.error('Error retrieving developer and finance details:', empError);
                    }
                });
            }
        });
    }

    function reloadDataTable() {
        table.ajax.reload(null, false); // Reload the table data without resetting the current page
    }

    loadTicketDetails();

    setInterval(reloadDataTable, 5000);

});



$(document).on('click', '.description-icon', function () {
    var description = $(this).data('description');
    $('#descriptionModal .modal-body').text(description);
    $('#descriptionModal').modal('show');
});

$(document).on('click', '#descriptionModal .close, #descriptionModal [data-dismiss="modal"]', function () {
    $('#descriptionModal').modal('hide');
});


$(document).on('click', '.note-icon', function () {
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

function updateResolutionNotes(ticketGuid, updatedNote) {
    return $.ajax({
        url: `https://localhost:7024/api/Resolution/UpdateResolutionNotes?resolutionGuid=${ticketGuid}&notes=${encodeURIComponent(updatedNote)}`,
        type: 'PUT',
        contentType: 'application/json',
        dataType: 'json'
    });
}


$(document).on('click', '#noteModal .close, #noteModal [data-dismiss="modal"]', function () {
    $('#noteModal').modal('hide');
});

$(document).on('click', '.attachment-icon', function () {
    var imageData = $(this).data('image');
    var imageSrc = 'data:image/jpg;base64,' + imageData;
    $('#attachmentModal img').attr('src', imageSrc);
    $('#attachmentModal').modal('show');
});

$(document).on('click', '.attachment-icon', function () {
    var imageData = $(this).data('image');
    var imageSrc = 'data:image/jpg;base64,' + imageData;
    $('#attachmentModal img').attr('src', imageSrc);
    $('#attachmentModal').modal('show');
});

$(document).on('click', '#attachmentModal .close, #attachmentModal [data-dismiss="modal"]', function () {
    $('#attachmentModal').modal('hide');
    $('#attachmentModal img').attr('src', '');
});


$(document).on('change', 'select.resolved-by-dropdown', function () {
    var resolutionGuid = $(this).attr('data-resolution-guid');
    var resolvedBy = $(this).val();

    if (resolvedBy !== '') {
        $('#confirmationModal').modal('show');

        $('#confirmButton').attr('data-resolution-guid', resolutionGuid);
        $('#confirmButton').attr('data-resolved-by', resolvedBy);
    }
});

$(document).on('click', '#confirmButton', function () {
    var resolutionGuid = $(this).attr('data-resolution-guid');
    var resolvedBy = $(this).attr('data-resolved-by');

    updateResolvedBy(resolutionGuid, resolvedBy);
    $('#confirmationModal').modal('hide');
});


$(document).on('click', '.reject-icon', function () {
    var resolutionGuid = $(this).attr('data-ticket-guid');

    $('#rejectConfirmationModal').modal('show');

    $('#rejectConfirmButton').attr('data-resolution-guid', resolutionGuid);
});


$(document).on('click', '#rejectConfirmButton', function () {
    var resolutionGuid = $(this).attr('data-resolution-guid');

    updateComplaintStatus(resolutionGuid, 1)
        .done(function () {
            console.log('Status updated successfully');
            $('#rejectConfirmationModal').modal('hide');
        })
        .fail(function (error) {
            console.error('Error updating Status:', error);
        });
});


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