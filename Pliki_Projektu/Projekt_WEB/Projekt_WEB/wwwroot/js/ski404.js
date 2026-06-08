(function () {
    const canvas = document.getElementById("ski404Canvas");

    if (!canvas) {
        return;
    }

    const ctx = canvas.getContext("2d");
    const panel = document.querySelector(".ski-game-panel");

    const distanceElement = document.getElementById("skiDistance");
    const bestDistanceElement = document.getElementById("skiBestDistance");
    const overlay = document.getElementById("skiGameOverlay");
    const overlayTitle = document.getElementById("skiOverlayTitle");
    const overlayText = document.getElementById("skiOverlayText");
    const startButton = document.getElementById("skiStartButton");
    const restartButton = document.getElementById("skiRestartButton");

    const targetDistance = Number(panel.dataset.targetDistance || 16000);
    const maxSafeDistance = 20000;

    let canvasWidth = canvas.width;
    let canvasHeight = canvas.height;

    let player;
    let obstacles;
    let particles;
    let keys;
    let mobileControls;

    let distance;
    let speed;
    let obstacleTimer;
    let obstacleInterval;
    let gameRunning;
    let gameOver;
    let animationFrameId;
    let lastTime;

    const bestStorageKey = "ski404BestDistance";

    function setupGame() {
        canvasWidth = canvas.width;
        canvasHeight = canvas.height;

        player = {
            x: canvasWidth * 0.5,
            y: canvasHeight * 0.72,
            width: 24,
            height: 34,
            speed: 245
        };

        obstacles = [];
        particles = [];
        keys = {
            up: false,
            down: false,
            left: false,
            right: false
        };

        mobileControls = {
            up: false,
            down: false,
            left: false,
            right: false
        };

        distance = 0;
        speed = 175;
        obstacleTimer = 0;
        obstacleInterval = 980;
        gameRunning = false;
        gameOver = false;
        lastTime = 0;

        updateDistanceText();
        updateBestText();
        drawScene(0);
    }

    function startGame() {
        cancelAnimationFrame(animationFrameId);
        setupGame();
        gameRunning = true;
        hideOverlay();
        lastTime = performance.now();
        animationFrameId = requestAnimationFrame(loop);
    }

    function restartGame() {
        startGame();
    }

    function loop(timestamp) {
        const delta = Math.min((timestamp - lastTime) / 1000, 0.035);
        lastTime = timestamp;

        update(delta);
        drawScene(delta);

        if (gameRunning) {
            animationFrameId = requestAnimationFrame(loop);
        }
    }

    function update(delta) {
        updatePlayer(delta);
        updateDistance(delta);
        updateObstacles(delta);
        updateParticles(delta);
        checkCollisions();

        if (distance >= targetDistance) {
            finishChallenge();
        }
    }

    function updatePlayer(delta) {
        let dx = 0;
        let dy = 0;

        if (keys.left || mobileControls.left) {
            dx -= 1;
        }

        if (keys.right || mobileControls.right) {
            dx += 1;
        }

        if (keys.up || mobileControls.up) {
            dy -= 1;
        }

        if (keys.down || mobileControls.down) {
            dy += 1;
        }

        if (dx !== 0 && dy !== 0) {
            dx *= 0.707;
            dy *= 0.707;
        }

        player.x += dx * player.speed * delta;
        player.y += dy * player.speed * delta;

        player.x = clamp(player.x, 30, canvasWidth - 30);
        player.y = clamp(player.y, 45, canvasHeight - 35);

        particles.push({
            x: player.x + randomBetween(-7, 7),
            y: player.y + 26,
            size: randomBetween(2, 4),
            life: 0.55
        });
    }

    function updateDistance(delta) {
        speed += delta * 5.5;
        distance += speed * delta * 0.22;

        obstacleInterval = Math.max(350, 980 - distance * 0.035);

        updateDistanceText();
        saveBestDistance();
    }

    function updateObstacles(delta) {
        obstacleTimer += delta * 1000;

        if (obstacleTimer >= obstacleInterval) {
            obstacleTimer = 0;
            spawnObstacle();
        }

        for (const obstacle of obstacles) {
            obstacle.y += speed * delta;
            obstacle.rotation += obstacle.rotationSpeed * delta;
        }

        obstacles = obstacles.filter(obstacle => obstacle.y < canvasHeight + 80);
    }

    function updateParticles(delta) {
        for (const particle of particles) {
            particle.y += 90 * delta;
            particle.life -= delta;
        }

        particles = particles.filter(particle => particle.life > 0);
    }

    function spawnObstacle() {
        const type = Math.random() > 0.45 ? "barrier" : "sign";
        const width = type === "barrier" ? randomBetween(52, 96) : 38;
        const height = type === "barrier" ? 18 : 42;

        obstacles.push({
            x: randomBetween(40, canvasWidth - 40),
            y: -60,
            width: width,
            height: height,
            type: type,
            rotation: randomBetween(-0.18, 0.18),
            rotationSpeed: randomBetween(-0.25, 0.25)
        });
    }

    function checkCollisions() {
        const playerBox = {
            x: player.x - player.width * 0.5,
            y: player.y - player.height * 0.5,
            width: player.width,
            height: player.height
        };

        for (const obstacle of obstacles) {
            const obstacleBox = {
                x: obstacle.x - obstacle.width * 0.5,
                y: obstacle.y - obstacle.height * 0.5,
                width: obstacle.width,
                height: obstacle.height
            };

            if (rectsOverlap(playerBox, obstacleBox)) {
                endGame();
                return;
            }
        }
    }

    function drawScene(delta) {
        drawBackground();
        drawParticles();
        drawObstacles();
        drawPlayer();
        drawHud();
    }

    function drawBackground() {
        ctx.clearRect(0, 0, canvasWidth, canvasHeight);

        const gradient = ctx.createLinearGradient(0, 0, 0, canvasHeight);
        gradient.addColorStop(0, "#dff5ff");
        gradient.addColorStop(1, "#f9fdff");

        ctx.fillStyle = gradient;
        ctx.fillRect(0, 0, canvasWidth, canvasHeight);

        ctx.strokeStyle = "rgba(15, 23, 42, 0.08)";
        ctx.lineWidth = 2;

        const offset = (distance * 0.5) % 70;

        for (let y = -70; y < canvasHeight + 70; y += 70) {
            ctx.beginPath();
            ctx.moveTo(canvasWidth * 0.28, y + offset);
            ctx.lineTo(canvasWidth * 0.42, y + 95 + offset);
            ctx.stroke();

            ctx.beginPath();
            ctx.moveTo(canvasWidth * 0.72, y + offset);
            ctx.lineTo(canvasWidth * 0.58, y + 95 + offset);
            ctx.stroke();
        }

        ctx.fillStyle = "rgba(255, 255, 255, 0.7)";

        for (let i = 0; i < 35; i++) {
            const x = (i * 97 + Math.floor(distance * 0.8)) % canvasWidth;
            const y = (i * 53 + Math.floor(distance * 1.1)) % canvasHeight;
            ctx.fillRect(x, y, 3, 3);
        }
    }

    function drawParticles() {
        for (const particle of particles) {
            ctx.globalAlpha = Math.max(particle.life, 0);
            ctx.fillStyle = "#ffffff";
            ctx.fillRect(particle.x, particle.y, particle.size, particle.size);
        }

        ctx.globalAlpha = 1;
    }

    function drawObstacles() {
        for (const obstacle of obstacles) {
            ctx.save();
            ctx.translate(obstacle.x, obstacle.y);
            ctx.rotate(obstacle.rotation);

            if (obstacle.type === "barrier") {
                drawBarrier(obstacle.width, obstacle.height);
            }
            else {
                drawSign(obstacle.width, obstacle.height);
            }

            ctx.restore();
        }
    }

    function drawBarrier(width, height) {
        ctx.fillStyle = "#0f172a";
        ctx.fillRect(-width / 2 - 3, -height / 2 - 3, width + 6, height + 6);

        ctx.fillStyle = "#ffffff";
        ctx.fillRect(-width / 2, -height / 2, width, height);

        const stripeWidth = 12;

        for (let x = -width / 2; x < width / 2; x += stripeWidth * 2) {
            ctx.fillStyle = "#ef4444";
            ctx.fillRect(x, -height / 2, stripeWidth, height);
        }

        ctx.fillStyle = "#0f172a";
        ctx.fillRect(-width / 2 + 4, height / 2, 4, 20);
        ctx.fillRect(width / 2 - 8, height / 2, 4, 20);
    }

    function drawSign(width, height) {
        ctx.fillStyle = "#0f172a";
        ctx.fillRect(-width / 2 - 3, -height / 2 - 3, width + 6, height + 6);

        ctx.fillStyle = "#ef4444";
        ctx.fillRect(-width / 2, -height / 2, width, height * 0.65);

        ctx.fillStyle = "#ffffff";
        ctx.font = "bold 10px monospace";
        ctx.textAlign = "center";
        ctx.fillText("STOP", 0, -4);

        ctx.fillStyle = "#0f172a";
        ctx.fillRect(-3, height * 0.15, 6, height * 0.55);
    }

    function drawPlayer() {
        ctx.save();
        ctx.translate(player.x, player.y);
        ctx.rotate((keys.left || mobileControls.left) ? -0.2 : (keys.right || mobileControls.right) ? 0.2 : 0);

        ctx.imageSmoothingEnabled = false;

        ctx.fillStyle = "#0f172a";
        ctx.fillRect(-16, 15, 32, 5);
        ctx.fillRect(-20, 21, 40, 5);

        ctx.fillStyle = "#fbbf24";
        ctx.fillRect(-7, -27, 14, 14);

        ctx.fillStyle = "#0f172a";
        ctx.fillRect(-9, -29, 18, 18);

        ctx.fillStyle = "#38bdf8";
        ctx.fillRect(-10, -10, 20, 28);

        ctx.fillStyle = "#0f172a";
        ctx.fillRect(-12, -12, 24, 32);

        ctx.fillStyle = "#2563eb";
        ctx.fillRect(-8, -8, 16, 24);

        ctx.fillStyle = "#0f172a";
        ctx.fillRect(-13, 2, 6, 16);
        ctx.fillRect(7, 2, 6, 16);

        ctx.fillStyle = "#ef4444";
        ctx.fillRect(-10, -2, 20, 5);

        ctx.restore();
    }

    function drawHud() {
        const progress = clamp(distance / targetDistance, 0, 1);

        ctx.fillStyle = "rgba(15, 23, 42, 0.72)";
        ctx.fillRect(18, 18, canvasWidth - 36, 18);

        ctx.fillStyle = "#38bdf8";
        ctx.fillRect(18, 18, (canvasWidth - 36) * progress, 18);

        ctx.strokeStyle = "#ffffff";
        ctx.lineWidth = 2;
        ctx.strokeRect(18, 18, canvasWidth - 36, 18);

        ctx.fillStyle = "#0f172a";
        ctx.font = "bold 14px monospace";
        ctx.textAlign = "left";
        ctx.fillText(Math.floor(distance) + " m / " + targetDistance + " m", 22, 56);
    }

    function endGame() {
        gameRunning = false;
        gameOver = true;
        cancelAnimationFrame(animationFrameId);

        showOverlay(
            "Koniec przejazdu",
            "Wpadłeś na zamkniętą trasę. Wynik: " + Math.floor(distance) + " m."
        );
    }

    function finishChallenge() {
        gameRunning = false;
        cancelAnimationFrame(animationFrameId);

        const finalDistance = Math.min(Math.floor(distance), maxSafeDistance);

        window.location.href = "/SkiChallenge/Congratulations?distance=" + finalDistance;
    }

    function showOverlay(title, text) {
        overlayTitle.textContent = title;
        overlayText.textContent = text;
        startButton.textContent = "Zagraj ponownie";
        overlay.classList.remove("is-hidden");
    }

    function hideOverlay() {
        overlay.classList.add("is-hidden");
    }

    function updateDistanceText() {
        distanceElement.textContent = Math.floor(distance) + " m";
    }

    function updateBestText() {
        const best = Number(localStorage.getItem(bestStorageKey) || 0);
        bestDistanceElement.textContent = best + " m";
    }

    function saveBestDistance() {
        const current = Math.floor(distance);
        const best = Number(localStorage.getItem(bestStorageKey) || 0);

        if (current > best) {
            localStorage.setItem(bestStorageKey, current.toString());
            updateBestText();
        }
    }

    function rectsOverlap(a, b) {
        return (
            a.x < b.x + b.width &&
            a.x + a.width > b.x &&
            a.y < b.y + b.height &&
            a.y + a.height > b.y
        );
    }

    function clamp(value, min, max) {
        return Math.min(Math.max(value, min), max);
    }

    function randomBetween(min, max) {
        return Math.random() * (max - min) + min;
    }

    window.addEventListener("keydown", function (event) {
        if (["ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight", "w", "a", "s", "d", "W", "A", "S", "D"].includes(event.key)) {
            event.preventDefault();
        }

        if (event.key === "ArrowLeft" || event.key === "a" || event.key === "A") {
            keys.left = true;
        }

        if (event.key === "ArrowRight" || event.key === "d" || event.key === "D") {
            keys.right = true;
        }

        if (event.key === "ArrowUp" || event.key === "w" || event.key === "W") {
            keys.up = true;
        }

        if (event.key === "ArrowDown" || event.key === "s" || event.key === "S") {
            keys.down = true;
        }
    });

    window.addEventListener("keyup", function (event) {
        if (event.key === "ArrowLeft" || event.key === "a" || event.key === "A") {
            keys.left = false;
        }

        if (event.key === "ArrowRight" || event.key === "d" || event.key === "D") {
            keys.right = false;
        }

        if (event.key === "ArrowUp" || event.key === "w" || event.key === "W") {
            keys.up = false;
        }

        if (event.key === "ArrowDown" || event.key === "s" || event.key === "S") {
            keys.down = false;
        }
    });

    document.querySelectorAll("[data-control]").forEach(function (button) {
        const control = button.dataset.control;

        button.addEventListener("pointerdown", function () {
            mobileControls[control] = true;
        });

        button.addEventListener("pointerup", function () {
            mobileControls[control] = false;
        });

        button.addEventListener("pointerleave", function () {
            mobileControls[control] = false;
        });

        button.addEventListener("pointercancel", function () {
            mobileControls[control] = false;
        });
    });

    startButton.addEventListener("click", startGame);
    restartButton.addEventListener("click", restartGame);

    setupGame();
})();