var studentToDeleteId;
let currentSort = "FirstName";
let sortAsc = true;
function openAddStudentModal() {
    $.get("/Student/AddStudent", function (data) {
        $("#addStudentContainer").html(data);
        $("#addStudentModal").modal("show");

    });
}


function fetchStudent() {
    $.ajax({
        url: `/Student/GetStudentListPartial?sortBy=${currentSort}&ascending=${sortAsc}`,
        type: 'GET',
        success: function (data) {
            $("#studentListContainer").html(data);
            
        }
    });
}

 

function sortTable(column) {
    if (currentSort === column) {
        sortAsc = !sortAsc;
    } else {
        currentSort = column;
        sortAsc = true;
    }

    fetchStudent();
}

function deleteStudent(studentId) {
    studentToDeleteId = studentId;
}

function confirmStudentDelete() {
    $.ajax({
        url: '/Student/DeleteStudent',
        type: 'POST',
        data: { id: studentToDeleteId },
        success: function (response) {
            if (response.success) {
                $('#deleteStudentModal').modal('hide');
                toastr.success('Student Deleted Successfully');
                studentToDeleteId = null;
                fetchStudent();
            } else {
                toastr.error('Failed to delete the student');
            }
        },
        error: function (xhr, status, error) {
            toastr.error('An unexpected error occurred. Please try again later.');
            $('#deleteStudentModal').modal('hide');
        }
    });
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

    $(document).on("click", ".edit-student-icon", function () {
        var studentId = $(this).data("student-id");
        $.ajax({
            url: "/Student/EditStudent",
            type: "GET",
            data: { id: studentId },
            success: function (data) {
                $("#editStudentContainer").html(data);
                $("#editStudentModal").modal("show");
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

    $(document).on("submit", "#editStudentForm", function (event) {
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
                $("#editStudentModal").modal("hide");
                fetchStudent();
                toastr.success("Student data updated !");
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