// Xử lý chọn sao
document.querySelectorAll(".star").forEach(star => {
    star.addEventListener("click", function () {
        let rating = this.getAttribute("data-value");
        document.getElementById("Rating").value = rating;
        document.querySelectorAll(".star").forEach(s => s.style.color = "gray");
        for (let i = 0; i < rating; i++) {
            document.querySelectorAll(".star")[i].style.color = "gold";
        }
    });
});

// Tô màu sao ban đầu nếu có
let rating = parseInt(document.getElementById("Rating")?.value || 0);
if (rating > 0) {
    for (let i = 0; i < rating; i++) {
        document.querySelectorAll(".star")[i].style.color = "gold";
    }
}

// Kiểm tra form đánh giá chính
document.querySelector("form:not(.reply-form)")?.addEventListener("submit", function (e) {
    let rating = parseInt(document.getElementById("Rating")?.value || 0);
    let noiDung = document.getElementById("NoiDung")?.value.trim() || "";
    if (rating === 0 && noiDung === "") {
        e.preventDefault();
        alert("Vui lòng nhập nội dung hoặc chọn số sao để đánh giá.");
    }
});

// Xử lý hiển thị/hủy form trả lời
document.querySelectorAll(".reply-link").forEach(link => {
    link.addEventListener("click", function () {
        const reviewId = this.getAttribute("data-review-id");
        const replyForm = document.getElementById(`reply-form-${reviewId}`);
        if (replyForm) {
            replyForm.style.display = replyForm.style.display === "none" ? "block" : "none";
        }
    });
});

document.querySelectorAll(".cancel-reply").forEach(link => {
    link.addEventListener("click", function () {
        const replyForm = this.closest(".reply-form");
        if (replyForm) {
            replyForm.style.display = "none";
            replyForm.querySelector("textarea").value = ""; // Xóa nội dung textarea khi hủy
        }
    });
});

