// Toggle ô tìm kiếm mobile
document.addEventListener("DOMContentLoaded", function () {
    var btnToggleSearch = document.getElementById("btnToggleSearch");
    var mobileSearch = document.getElementById("mobileSearch");

    if (btnToggleSearch && mobileSearch) {
        btnToggleSearch.addEventListener("click", function () {
            if (mobileSearch.style.display === "block") {
                mobileSearch.style.display = "none";
            } else {
                mobileSearch.style.display = "block";
            }
        });
    }

    // (Optional) header thu nhỏ khi scroll
    var header = document.querySelector(".bt-header");
    if (header) {
        window.addEventListener("scroll", function () {
            if (window.scrollY > 80) {
                header.classList.add("bt-header--shrink");
            } else {
                header.classList.remove("bt-header--shrink");
            }
        });
    }
});
