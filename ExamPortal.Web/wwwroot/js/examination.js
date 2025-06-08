function openAddExamModal() {
  $.get("/Examination/AddExam", function (data) {
    $("#addExamModalPlaceholder").html(data);
    $("#addExamModal").modal("show");
  });
}
