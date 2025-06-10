var examToDeleteId;
var questionToDeleteId;

function openAddExamModal() {
  $.get("/Examination/AddExam", function (data) {
    $("#addExamContainer").html(data);
    $("#addExamModal").modal("show");
  });
}

function deleteExam(examId) {
  examToDeleteId = examId;
}

function deleteQuestion(questionId) {
  questionToDeleteId = questionId;
}

function fetchExam() {
  $.ajax({
    url: `/Examination/GetExamListPartial`,
    type: 'GET',
    success: function (data) {
      $("#examListContainer").html(data);
    }
  })
}

async function updateQuestionListPartialView(examId) {
  const listPartialHtml = await $.ajax({
    url: '/Examination/GetQuestionListPartial',
    type: 'GET',
    data: { examId: examId }
  });
  $("#questionListContainer").html(listPartialHtml);
}
 
function confirmExamDelete() {
  $.ajax({
    url: '/Examination/DeleteExam',
    type: 'POST',
    data: { id: examToDeleteId },
    success: function (response) {
      if (response.success) {
        $('#deleteExamModal').modal('hide');
        toastr.success('Exam Deleted Successfully');
        examToDeleteId = null;
        fetchExam();
      } else {
        toastr.error('Failed to delete the exam');
      }
    },
    error: function (xhr, status, error) {
      toastr.error('An unexpected error occurred. Please try again later.');
      $('#deleteExamModal').modal('hide');
    }
  });
}

function confirmQuestionDelete() {
  $.ajax({
    url: '/Examination/DeleteQuestion',
    type: 'POST',
    data: { questionId: questionToDeleteId },
    success: function (response) {
      if (response.success) {
        $('#deleteQuestionModal').modal('hide');
        toastr.success('Question Deleted Successfully');
        $(`.questions-container li[data-questionid="${questionToDeleteId}"]`).remove();
        questionToDeleteId = null;
      } else {
        toastr.error('Failed to delete the question');
      }
    },
    error: function (xhr, status, error) {
      toastr.error('An unexpected error occurred. Please try again later.');
      $('#deleteQuestionModal').modal('hide');
    }
  });
}

function checkForDateValidation(startDate, endDate) {
  return startDate < endDate;
}


function renderOptions(type) {
  const container = $("#dynamicOptions");
  // const hasOptions = container.children().length > 0;
  // if (hasOptions) return;
  container.empty();
  if (type === "MCQ") {
    for (let i = 0; i < 4; i++) {
      container.append(`
                  <div class="mb-2">
                      <label class="form-label">Option ${i + 1}</label>
                      <input name="Options[${i}]" class="form-control" placeholder="Option ${i + 1}" required/>
                  </div>
              `);
    }
  } else if (type === "TrueFalse") {
    container.append(`
              <div class="mb-2">
                  <label class="form-label">Option 1</label>
                  <input name="Options[0]" class="form-control" value="True" readonly />
              </div>
              <div class="mb-2">
                  <label class="form-label">Option 2</label>
                  <input name="Options[1]" class="form-control" value="False" readonly />
              </div>
          `);
  }
}

$(document).ready(function () {

  // _ExamList.cshtml

  $(document).on("click", ".edit-exam-icon", function () {
    var examId = $(this).data("exam-id");
    $.ajax({
      url: "/Examination/EditExam",
      type: "GET",
      data: { id: examId },
      success: function (data) {
        $("#editExamContainer").html(data);
        $("#editExamModal").modal("show");
      }
    });

  });

  // _QuestionForm.cshtml   
  $(document).on("change", "#questionTypeSelector", function () {
    renderOptions($(this).val());
  });
 
  $(document).on("submit", "#questionForm", async function (e) {
    e.preventDefault();
    var form = $(this);
    try {
      const formPartialHtml = await $.ajax({
        url: form.attr("action"),
        type: "POST",
        data: form.serialize()
      });
      const examId = $("#ExamId").val();
      $("#questionFormContainer").html(formPartialHtml);

      $("#questionForm input[type=text], #questionForm textarea").val('');
      $("#questionForm select").prop('selectedIndex', 0);
      $("#questionForm input[type=number]").val(0);
      $("#dynamicOptions").empty();
      $("#correctOptionDiv input").val('');

      updateQuestionListPartialView(examId);
    } catch (error) {
      console.log(error);
      alert("An error occured while processing the request");
    }
  })

  // _QuestionList.cshtml

  $(document).on("click", ".delete-question", function () {
    const questionId = $(this).data("questionid");
  });

  $(document).on("click", ".edit-question", function () {
    const questionId = $(this).data("questionid");
    $.ajax({
      url: `/Examination/EditQuestion`,
      type: 'GET',
      data: { questionId: questionId },
      success: function (data) {
        $("#questionFormContainer").html(data);
      }
    })
  });

  $(document).on("submit", "#addExamForm", function (event) {
    event.preventDefault();
    var result = checkForDateValidation($("#startDate").val(), $("#endDate").val());
    if (result == false) {
      toastr.error('Enter the valid start and end date.');
      return;
    }
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
        $("#addExamModal").modal("hide");
        fetchExam();
        toastr.success("Exam added !");
      },
      error: function (xhr, status, error) {
        if (xhr.responseJSON && xhr.responseJSON.errorCode === "DuplicateExamName") {
          toastr.error('Exam  with the same name already exists.');
        } else {
          toastr.error('An unexpected error occurred. Please try again later.');
        }
      }
    });
  });

  $(document).on("submit", "#editExamForm", function (event) {
    event.preventDefault();
    var result = checkForDateValidation($("#startDate").val(), $("#endDate").val());
    if (result == false) {
      toastr.error('Enter the valid start and end date.');
      return;
    }
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
        $("#editExamModal").modal("hide");
        fetchExam();
        toastr.success("Exam data updated !");
      },
      error: function (xhr, status, error) {
        if (xhr.responseJSON && xhr.responseJSON.errorCode === "DuplicateExamName") {
          toastr.error('Exam  with the same name already exists.');
        } else {
          toastr.error('An unexpected error occurred. Please try again later.');
        }
      }
    });
  });


});