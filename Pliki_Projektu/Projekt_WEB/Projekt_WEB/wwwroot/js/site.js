const progress = document.querySelector(".scroll-progress");
const nav = document.querySelector(".main-nav");
const navToggle = document.querySelector(".nav-toggle");
const backTop = document.querySelector(".back-to-top");
const themeToggle = document.querySelector(".theme-toggle");

function updateProgress() {
    const max = document.documentElement.scrollHeight - window.innerHeight;
    const value = max > 0 ? (window.scrollY / max) * 100 : 0;

    if (progress) {
        progress.style.width = value + "%";
    }

    if (backTop) {
        backTop.classList.toggle("visible", window.scrollY > 500);
    }
}

function reveal() {
    document.querySelectorAll(".reveal").forEach(function (element) {
        if (element.getBoundingClientRect().top < window.innerHeight - 80) {
            element.classList.add("visible");
        }
    });
}

function animateCounters() {
    document.querySelectorAll("[data-count]").forEach(function (counter) {
        if (counter.dataset.done === "1") {
            return;
        }

        if (counter.getBoundingClientRect().top > window.innerHeight) {
            return;
        }

        counter.dataset.done = "1";
        const target = Number(counter.dataset.count);
        let value = 0;
        const step = Math.max(1, Math.ceil(target / 60));

        const timer = setInterval(function () {
            value += step;

            if (value >= target) {
                value = target;
                clearInterval(timer);
            }

            counter.textContent = value;
        }, 18);
    });
}

function initPublicCharts() {
    if (!window.Chart || !window.chartData) {
        return;
    }

    const disciplineCanvas = document.getElementById("disciplineChart");
    const topCanvas = document.getElementById("topChart");

    if (disciplineCanvas) {
        new Chart(disciplineCanvas, {
            type: "doughnut",
            data: {
                labels: window.chartData.disciplines.labels,
                datasets: [{
                    data: window.chartData.disciplines.values,
                    backgroundColor: ["#38bdf8", "#f97316", "#22c55e", "#facc15"]
                }]
            },
            options: { responsive: true, plugins: { legend: { position: "bottom" } } }
        });
    }

    if (topCanvas) {
        new Chart(topCanvas, {
            type: "bar",
            data: {
                labels: window.chartData.top.labels,
                datasets: [{
                    label: "Punkty",
                    data: window.chartData.top.values,
                    backgroundColor: "#38bdf8"
                }]
            },
            options: { responsive: true, scales: { y: { beginAtZero: true } } }
        });
    }
}

window.initPublicCharts = initPublicCharts;

window.addEventListener("scroll", function () {
    updateProgress();
    reveal();
    animateCounters();
});

window.addEventListener("load", function () {
    updateProgress();
    reveal();
    animateCounters();
});

if (navToggle && nav) {
    navToggle.addEventListener("click", function () {
        nav.classList.toggle("open");
    });
}

if (backTop) {
    backTop.addEventListener("click", function () {
        window.scrollTo({ top: 0, behavior: "smooth" });
    });
}

if (themeToggle) {
    const savedTheme = localStorage.getItem("winter-theme");

    if (savedTheme === "light") {
        document.body.classList.add("light-theme");
    }

    themeToggle.addEventListener("click", function () {
        document.body.classList.toggle("light-theme");
        localStorage.setItem("winter-theme", document.body.classList.contains("light-theme") ? "light" : "dark");
    });
}
