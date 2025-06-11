// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function getChangePasswordModal() {
    $.ajax({
        url: "/Profile/ChangePassword",
        type: "GET",
        success: function (response) {
            $("#changePasswordContainer").html(response);
            $('#changePasswordModal').modal('show');
        }
    });
}

$(document).on("submit", "#changePasswordForm", function (event) {
    event.preventDefault();
    if (!$(this).valid()) {
        return;
    }
    var form = $(this);
    var formData = form.serialize();
    $.ajax({
        url: form.attr('action'),
        type: form.attr('method'),
        data: formData,
        success: function (response) {
            $("#changePasswordModal").modal("hide");
            toastr.success("Password changed successfully.");
        },
        error: function (xhr, status, error) {
            if (xhr.responseJSON && xhr.responseJSON.errorCode === "IncorrectPassword") {
                toastr.error('Failed to change the password.');
            }
            else if (xhr.responseJSON && xhr.responseJSON.errorCode === "SamePasswordError") {
                toastr.error('New Password cannot be same as the current  one');
            }
            else {
                toastr.error('An unexpected error occurred. Please try again later.');
            }
        }
    });
});