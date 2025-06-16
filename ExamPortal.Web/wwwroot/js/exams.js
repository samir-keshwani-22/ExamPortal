var examToStartId;
var examToRegisterId;

function setExamStartId(examId) {
  examToStartId = examId;
}

function setExamRegisterId(examId) {
  examToRegisterId = examId;
}

function confirmExamStart() {
  $("#startExamModal").modal("hide");
  const element = document.documentElement;

  if (element.requestFullscreen) {
    element
      .requestFullscreen()
      .then(() => {
        // Success - redirect to exam
        window.location.href = `/Exams/ExamInterface?examId=${examToStartId}`;
      })
      .catch((err) => {
        console.log("Fullscreen failed:", err);
        // Still redirect but user will see fullscreen prompt on exam page
        window.location.href = `/Exams/ExamInterface?examId=${examToStartId}`;
      });
  } else if (element.webkitRequestFullscreen) {
    element.webkitRequestFullscreen();
    window.location.href = `/Exams/ExamInterface?examId=${examToStartId}`;
  } else if (element.msRequestFullscreen) {
    element.msRequestFullscreen();
    window.location.href = `/Exams/ExamInterface?examId=${examToStartId}`;
  } else if (element.mozRequestFullScreen) {
    element.mozRequestFullScreen();
    window.location.href = `/Exams/ExamInterface?examId=${examToStartId}`;
  } else {
    // Fullscreen not supported
    window.location.href = `/Exams/ExamInterface?examId=${examToStartId}`;
  }
}

function confirmExamRegistration() {
  $.ajax({
    url: "/Exams/RegisterForExam",
    type: "POST",
    data: { examId: examToRegisterId },
    success: function (response) {
      if (response.success) {
        $("#registerExamModal").modal("hide");
        toastr.success("Successfully registered for the exam.");
        examToRegisterId = null;
      }
    },
    error: function (xhr, status, error) {
      if (
        xhr.responseJSON &&
        xhr.responseJSON.errorCode === "AlreadyRegistered"
      ) {
        toastr.error("You are already registered for this exam.");
      } else {
        toastr.error("An unexpected error occurred. Please try again later.");
      }
    },
  });
}

// Function to enter fullscreen mode
function enterFullscreen() {
  const element = document.documentElement;

  return new Promise((resolve, reject) => {
    if (element.requestFullscreen) {
      element
        .requestFullscreen()
        .then(() => {
          resolve();
        })
        .catch((err) => {
          console.log("Error attempting to enable fullscreen:", err);
          reject(err);
        });
    } else if (element.webkitRequestFullscreen) {
      // Safari
      try {
        element.webkitRequestFullscreen();
        resolve();
      } catch (err) {
        reject(err);
      }
    } else if (element.msRequestFullscreen) {
      // IE/Edge
      try {
        element.msRequestFullscreen();
        resolve();
      } catch (err) {
        reject(err);
      }
    } else if (element.mozRequestFullScreen) {
      // Firefox
      try {
        element.mozRequestFullScreen();
        resolve();
      } catch (err) {
        reject(err);
      }
    } else {
      reject(new Error("Fullscreen not supported"));
    }
  });
}

// Function to exit fullscreen mode
function exitFullscreen() {
  if (document.exitFullscreen) {
    document.exitFullscreen();
  } else if (document.webkitExitFullscreen) {
    // Safari
    document.webkitExitFullscreen();
  } else if (document.msExitFullscreen) {
    // IE/Edge
    document.msExitFullscreen();
  } else if (document.mozCancelFullScreen) {
    // Firefox
    document.mozCancelFullScreen();
  }
}

// Function to check if currently in fullscreen
function isFullscreen() {
  return !!(
    document.fullscreenElement ||
    document.webkitFullscreenElement ||
    document.msFullscreenElement ||
    document.mozFullScreenElement
  );
}

// Function to handle fullscreen change events
function handleFullscreenChange() {
  if (!isFullscreen()) {
    // User exited fullscreen - show warning
    showFullscreenWarning();
  }
}

function showFullscreenWarning() {
  const warningModal = `
    <div class="modal fade" id="fullscreenWarningModal" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content border-warning">
          <div class="modal-header bg-warning text-dark">
            <h5 class="modal-title"><i class="bi bi-exclamation-triangle-fill"></i> Exam Security Notice</h5>
          </div>
          <div class="modal-body text-center">
            <p>You exited fullscreen mode. You must remain in fullscreen during the exam.</p>
            <p class="text-muted small">Click "Re-enter Fullscreen" to continue or "Submit Exam" to end the exam.</p>
          </div>
          <div class="modal-footer justify-content-between">
            <button id="reenterFsBtn" class="btn primary-button-v1"><i class="bi bi-fullscreen"></i> Re-enter Fullscreen</button>
            <button id="submitExamBtn" class="btn btn-danger"><i class="bi bi-box-arrow-right"></i> Submit Exam</button>
          </div>
        </div>   
      </div>
    </div>
  `;

  if (!document.getElementById("fullscreenWarningModal")) {
    document.body.insertAdjacentHTML("beforeend", warningModal);
  }

  const modal = new bootstrap.Modal(
    document.getElementById("fullscreenWarningModal")
  );
  modal.show();

  document.getElementById("reenterFsBtn").onclick = () => {
    enterFullscreen().then(() => modal.hide());
  };

  document.getElementById("submitExamBtn").onclick = () => {
    modal.hide();
    if (typeof submitExam === "function") {
      submitExam();
    } else {
      alert("Please submit your exam through the submit button.");
    }
  };
}

function initializeFullscreen() {
  console.log("Initializing fullscreen mode...");

  showFullscreenPrompt();

  document.addEventListener("fullscreenchange", handleFullscreenChange);
  document.addEventListener("webkitfullscreenchange", handleFullscreenChange);
  document.addEventListener("msfullscreenchange", handleFullscreenChange);
  document.addEventListener("mozfullscreenchange", handleFullscreenChange);

  document.addEventListener("contextmenu", function (e) {
    e.preventDefault();
    return false;
  });

  document.addEventListener("keydown", function (e) {
    if (e.key === "F11") {
      e.preventDefault();
      return false;
    }

    if (e.altKey && e.key === "Tab") {
      e.preventDefault();
      return false;
    }

    if (e.ctrlKey && e.shiftKey && e.key === "I") {
      e.preventDefault();
      return false;
    }

    if (e.key === "F12") {
      e.preventDefault();
      return false;
    }

    if (e.ctrlKey && e.key === "u") {
      e.preventDefault();
      return false;
    }
  });


  document.addEventListener("visibilitychange", function () {
    if (document.hidden) {
      console.log("User switched tab/window - Security violation detected");

    }
  });

  window.addEventListener("beforeprint", function (e) {
    e.preventDefault();
    alert("Printing is not allowed during the exam.");
    return false;
  });

  console.log("Fullscreen mode initialized successfully");
}

function showFullscreenPrompt() {
  if (!isFullscreen()) {
    const fullscreenModal = `
            <div class="modal fade" id="fullscreenModal" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header bg-warning text-dark">
                            <h5 class="modal-title">
                                <i class="bi bi-fullscreen"></i> Fullscreen Required
                            </h5>
                        </div>
                        <div class="modal-body text-center">
                            <i class="bi bi-shield-check fs-1 text-primary mb-3"></i>
                            <h6>Exam Security Notice</h6>
                            <p class="mb-0">For exam security and integrity, you must switch to fullscreen mode to continue.</p>
                            <p class="text-muted small mt-2">This helps prevent unauthorized access to other applications during the exam.</p>
                        </div>
                        <div class="modal-footer justify-content-center">
                            <button type="button" class="btn primary-button-v1" onclick="activateFullscreen()">
                                <i class="bi bi-arrows-fullscreen"></i> Enter Fullscreen
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;

    if (!document.getElementById("fullscreenModal")) {
      document.body.insertAdjacentHTML("beforeend", fullscreenModal);
    }

    const modal = new bootstrap.Modal(
      document.getElementById("fullscreenModal")
    );
    modal.show();
  }
}

function activateFullscreen() {
  enterFullscreen()
    .then(() => {
      const modal = bootstrap.Modal.getInstance(
        document.getElementById("fullscreenModal")
      );
      if (modal) {
        modal.hide();
      }
    })
    .catch((err) => {
      console.error("Failed to enter fullscreen:", err);
      alert(
        "Unable to enter fullscreen mode. Please try manually pressing F11 or contact support."
      );
    });
}
