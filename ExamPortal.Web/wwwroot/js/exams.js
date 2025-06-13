var examToStartId;
var examToRegisterId;

function setExamStartId(examId) {
    examToStartId = examId;
}

function setExamRegisterId(examId) {
    examToRegisterId = examId;
}


function confirmExamStart() {
    $('#startExamModal').modal('hide');
    window.location.href = `/Exams/ExamInterface?examId=${examToStartId}`;
}

function confirmExamRegistration() {
    $.ajax({
        url: "/Exams/RegisterForExam",
        type: "POST",
        data: { examId: examToRegisterId },
        success: function (response) {
            if (response.success) {
                $("#registerExamModal").modal("hide");
                toastr.success("Successfully registered for the exam.")
                examToRegisterId = null;
            }
        },
        error: function (xhr, status, error) {
            if (xhr.responseJSON && xhr.responseJSON.errorCode === "AlreadyRegistered") {
                toastr.error('You are already registered for this exam.');
            } else {
                toastr.error('An unexpected error occurred. Please try again later.');
            }
        }
    });
}


