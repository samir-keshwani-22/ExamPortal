function openAddExamModal() {
  $.get("/Examination/AddExam", function (data) {
    $("#addExamContainer").html(data);
    $("#addExamModal").modal("show");
  });
}


function renderOptions(type) {
  const container = $("#dynamicOptions");
  container.empty();
  if (type === "MCQ") {
    for (let i = 0; i < 4; i++) {
      container.append(`
                  <div class="mb-2">
                      <label class="form-label">Option ${i + 1}</label>
                      <input name="Options[${i}]" class="form-control" placeholder="Option ${i + 1}" />
                  </div>
              `);
    }
    $("#correctOptionDiv").show();
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
    $("#correctOptionDiv").show();
  }
}

$(document).ready(function () {

  // _ExamList.cshtml

  $(document).on("click", ".edit-exam-icon", function () {
    var examId = $(this).data("exam-id");
    debugger
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



  $(document).on("submit", "#questionForm", function (e) {
    e.preventDefault();
    var form = $(this);
    $.ajax({
      url: form.attr("action"),
      type: "POST",
      data: form.serialize(),
      success: function (formPartialHtml) {
        var examId = $("#ExamId").val();
        debugger
        $("#questionFormContainer").html(formPartialHtml);
        $.ajax({
          url: '/Examination/GetQuestionListPartial',
          type: 'GET',
          data: { examId: examId },
          success: function (listPartialHtml) {
            $("#questionListContainer").html(listPartialHtml);
          },
          error: function () {
            alert("An error occurred while loading the question list.");
          }
        });


      },
      error: function () {
        alert("An error occurred");
      }
    });
  })

  // _QuestionList.cshtml

  $(document).on("click", ".delete-question", function () {

  });

  $(document).on("click", ".edit-question", function () {

  });





});