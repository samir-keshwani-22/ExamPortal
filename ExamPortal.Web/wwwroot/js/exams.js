var examToStartId;

function setExamStartId(examId) {
    examToStartId = examId;
}


async function confirmExamStart() {

    $('#startExamModal').modal('hide'); 
    window.location.href = `/Exams/ExamInterface?examId=${examToStartId}`;

}
 
