function openAddStudentModal() {
    $.get("/Student/AddStudent", function (data) {
        $("#addStudentContainer").html(data);
        $("#addStudentModal").modal("show");

    });
}

function fetchStudent() {
    $.ajax({
        url: `/Student/GetStudentListPartial`,
        type: 'GET',
        success: function (data) {
            $("#studentListContainer").html(data);
        }
    })
}

$(document).ready(function () {
    $(document).on("submit", "#addStudentForm", function (event) {
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
                $("#addStudentModal").modal("hide");
                fetchStudent();
                toastr.success("Student added !");
            },
            error: function (xhr, status, error) {
                if (xhr.responseJSON && xhr.responseJSON.errorCode === "DuplicateStudentName") {
                    toastr.error('Student  with the same email already exists.');
                } else {
                    toastr.error('An unexpected error occurred. Please try again later.');
                }
            }
        });
    });
});