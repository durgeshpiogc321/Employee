$(function () {
    employee.List();

    $('#search-keyword').bind("keyup", function () {
        let minLength = 3;//Min search charecters
        let searchValue = $(this).val() || '';
        if (searchValue.length >= minLength || searchValue.length === 0)
            employee.List();
    });
});

//Search button click
$(document).on('click', '#btn-search', function () {
    employee.List();
});

//Filter close button click
$(document).on('click', function (e) {
    if ($(e.target).closest(".mvc-grid-popup").length === 0)
        $(".mvc-grid-popup").hide().find('.popup-filter').empty();

    else if ($(e.target).closest(".mvc-grid-cancel").length > 0)
        $(".mvc-grid-popup").hide().find('.popup-filter').empty();
});

//Filter apply button click
$(document).on('click', '.mvc-grid-apply', function () {
    employee.List();
});

var table = $('#data-table');
var hdnSearchColumn = '', hdnSearchColumnValue = '';

var employee = {
    List: function () {

        let columns = [];
        $(table).find('thead tr th').each(function (i, v) {
            let propName = $(v).data('name');
            let propNameLowerCase = $(v).data('name').toLowerCase() || '';
            let column;

            if (propNameLowerCase.includes("srno")) {
                column = {
                    data: null, name: propName, render: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                };
            }
            else if (propNameLowerCase.includes("name")) {
                column = {
                    data: null, name: propName, mRender: function (data, row) {
                        let profilePic = (data.profilePic || '').length == 0 ? '' : `/Uploads/Profile/${data.profilePic}`;
                        return `<div class="d-flex align-items-center">
                                    <div class="flex-shrink-0">
                                      <img src="${profilePic}" onerror="this.onerror=null;this.src='/Uploads/no_image.png'" class="img-fluid rounded-circle border border-1" style="width:35px;">
                                    </div>
                                    <div class="flex-grow-1 ms-1">
                                      <div class="d-flex flex-row align-items-center">
                                        <p class="mb-0">${data.name}</p>                    
                                      </div>                  
                                </div>
                              </div>`;
                    }
                };
            }
            else if (propNameLowerCase.includes("status")) {
                column = {
                    data: null, name: propName, mRender: function (data, row) {
                        let checked = (data.isActive || false) ? 'checked' : '';
                        return ` <input id="chkActive_${data.id}" class="form-check-input" type="checkbox" ${checked} onclick="employee.Active(${data.id},'${data.isActive || false}')">`;
                    }
                };
            }
            else if (propNameLowerCase.includes("action")) {
                column = {
                    data: null, name: propName, mRender: function (data, row) {
                        let checked = (data.isActive || false) ? 'checked' : '';
                        return `<a href="javascript:;" class="me-1" onclick="employee.OpenModal(${data.id})" title="Edit"><img src="/assets/images/edit-icon.png" style="width:20px;"></a>
                                <a href="javascript:;" onclick="employee.Delete(${data.id})" title="Delete"><img src="/assets/images/delete-icon.png" style="width:20px;"></a>
                        `;
                    }
                };
            }
            else {
                column = { data: propNameLowerCase.charAt(0) + propName.slice(1), name: propName };
            }

            columns.push(column);
        });

        let searchColumn = $('.mvc-grid-value').data('filter') || '';
        let searchColumnValue = $('.mvc-grid-value:input').val() || '';

        if (searchColumn.length == 0 && hdnSearchColumn.length > 0) {
            searchColumn = hdnSearchColumn;
            searchColumnValue = hdnSearchColumnValue;
        }

        let data = { email: $('#txtEmail').val(), phone: $('#txtPhone').val(), searchKeyword: $('#search-keyword').val(), searchColumn: searchColumn, searchColumnValue: searchColumnValue };

        let noOrderableColumns = [0, 7, 8];

        tbl.BindDataTable(table, `/Employee/GetAllEmployee/`, data, columns, noOrderableColumns);

        employee.EnableColumnHeaderFilter(); // Add filter button on table header

        $(".mvc-grid-popup").hide().find('.popup-filter').empty();

        if (searchColumnValue) {
            hdnSearchColumn = searchColumn;
            hdnSearchColumnValue = searchColumnValue;
        }

    },

    EnableColumnHeaderFilter: function () {
        let noFilterColumns = [0, 8];
        let filterIcon = `<button type="button" class="mvc-grid-filter" onclick="employee.OpenHeaderFilterPopup(this,event)"><img src="/assets/images/filter-icon.png" style=" opacity:.400;" /></button>`;
        $(table).find('thead tr:eq(0) th').each(function (i, v) {
            if ($(v).find('.mvc-grid-filter').length == 0) {
                let propName = $(v).data('name').toLowerCase();
                let html = $(v).html();
                if (!noFilterColumns.includes(i)) {
                    $(v).html(html + filterIcon);
                }
            }
        });
    },

    OpenHeaderFilterPopup: function (_this, event) {
        event.stopPropagation();
        let gridPopup = $('.mvc-grid-popup');

        $(gridPopup).removeAttr('style');

        $(gridPopup).position({
            my: "left-13 top",
            at: "left bottom",
            of: $(_this),
            collision: "fit"
        });
        let propText = $(_this).closest('th').text();
        let propName = $(_this).closest('th').data('name').toLowerCase();


        if (propName.includes('dob') || propName.includes('createddate')) {
            $('.popup-filter').html('').html(`<div class="popup-group">
                <input type="text" class="mvc-grid-value form-control datetime-picker" value="${propName == hdnSearchColumn ? hdnSearchColumnValue : ''}" data-filter="${propName}" placeholder="${propText}">
            </div>`);

            $(".datetime-picker").datepicker({
                dateFormat: 'd M, yy',
                changeMonth: true,
                changeYear: true,
                minYear: 1950,
            }).attr('readonly', 'readonly');
        }
        else if (propName.includes('status')) {
            $('.popup-filter').html('').html(`<div class="popup-group">
                <select class="mvc-grid-value" data-filter="${propName}">
                    <option value="-1">Select Status</option>
                    <option value="1">Active</option>
                    <option value="0">In-Active</option>                   
                </select>
            </div>`);
        }
        else {
            $('.popup-filter').html('').html(`<div class="popup-group">
                <input type="text" class="mvc-grid-value form-control" value="${propName == hdnSearchColumn ? hdnSearchColumnValue : ''}" data-filter="${propName}" placeholder="${propText}">
            </div>`);
        }

        $(gridPopup).show()
    },

    ClearFilter: function () {
        $(".search-box input:text").val('');
        hdnSearchColumn = '';
        hdnSearchColumnValue = '';
        employee.List();
    },

    ClearAddEditForm: function (_this, event) {
        event.preventDefault();
        let form = $(_this).closest('form');
        $(form).find('input[type="text"]:enabled,textarea:enabled').val('')

        $(form).find('input[type="checkbox"]:enabled').prop('checked', false);
    },

    OpenModal: function (id = 0) {
        toastr.remove();
        $.ajax({
            type: 'GET',
            url: `/Employee/_Create/${id}`,
            success: function (response) {
                $.when($('#modal-body').html('').html(response)).then(function () {

                    $.validator.unobtrusive.parse('form');

                    $(".datetime-picker").datepicker({
                        dateFormat: 'd M, yy',
                        changeMonth: true,
                        changeYear: true,
                        minYear: 1950,
                    }).attr('readonly', 'readonly');

                    if (id == 0) {
                        $('#btn-add-edit').text('Add Employee');
                        $('#modal-title').text('Add Employee');
                    }
                    else {
                        $('#btn-add-edit').text('Update Employee');
                        $('#modal-title').text('Update Employee');

                        $('#Email').attr('disabled', true);
                    }

                    setTimeout(function () {
                        $('#add-edit-modal').modal('show');
                    }, 100)
                });
            }
        });

    },

    CloseModal: function () {
        $('#add-edit-modal').modal('hide');
        $('#modal-body').html('');
        // Remove any remaining backdrop
        setTimeout(function () {
            $('.modal-backdrop').remove();
            $('body').removeClass('modal-open');
        }, 300);
    },

    AddEdit: function () {
        toastr.remove();
        let fid = $('#frm-add-edit');
        if ($(fid).valid()) {
            // Temporarily enable email field for form submission
            $('#Email').attr('disabled', false);
            var frmData = new FormData($(fid)[0]);

            $.ajax({
                type: 'POST',
                url: `/Employee/Create/`,
                data: frmData,
                processData: false,
                contentType: false,
                async: true,
                cache: false,
                success: function (response) {
                    if (response.statusCode == 200) {
                        toastr.success(response.message);
                        employee.CloseModal();
                        setTimeout(function () {
                            employee.List();
                        }, 100);
                    }
                    else {
                        toastr.error(response.message);
                        // Re-disable email field if there's an error
                        $('#Email').attr('disabled', true);
                    }
                },
                error: function (response) {
                    toastr.error("Something went wrong");
                    // Re-disable email field if there's an error
                    $('#Email').attr('disabled', true);
                }
            });
        }
    },

    Delete: function (id) {
        toastr.remove();
        swal({
            title: "Are you sure?",
            text: "You want to delete this employee!",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: 'btn-danger',
            confirmButtonText: 'Yes, Delete!',
            cancelButtonClass: 'btn-light border border-1 me-2',
            cancelButtonText: "Cancel",
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    $.ajax({
                        type: 'POST',
                        url: `/Employee/Delete/`,
                        data: { id: id },
                        async: false,
                        cache: false,
                        success: function (response) {
                            if (response.statusCode == 200) {
                                toastr.success(response.message);
                                setTimeout(function () {
                                    employee.List();
                                }, 1000);
                            }
                            else {
                                toastr.error(response.message);
                            }
                        },
                        error: function (response) {
                            toastr.error("Something went wrong");
                        }
                    });
                }
            });

    },

    Active: function (id, status) {
        toastr.remove();
        let statusMsg = (status || false) ? 'de-active' : 'active';
        swal({
            title: "Are you sure?",
            text: `You want to ${statusMsg} this employee!`,
            type: "warning",
            showCancelButton: true,
            confirmButtonText: 'Yes!',
            cancelButtonClass: 'btn-light border border-1 me-2',
            cancelButtonText: "Cancel",
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    $.ajax({
                        type: 'POST',
                        url: `/Employee/Active/`,
                        data: { id: id },
                        async: false,
                        cache: false,
                        success: function (response) {
                            if (response.statusCode == 200) {
                                toastr.success(response.message);
                                setTimeout(function () {
                                    employee.List();
                                }, 500);
                            }
                            else {
                                toastr.error(response.message);
                            }
                        },
                        error: function (response) {
                            toastr.error("Something went wrong");
                        }
                    });
                }
                else {
                    if ((status || false) == 'true') {

                        $(`#chkActive_${id}`).prop('checked', true);
                    }
                    else {
                        $(`#chkActive_${id}`).prop('checked', false);
                    }
                }
            });

    },
}