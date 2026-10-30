// 정적 사이트용 바닐라 JS — Blazor 런타임 없이 동작합니다.
(() => {
    const root = document.documentElement;
    const reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

    /* ---------- Scroll reveal ---------- */
    const revealTargets = document.querySelectorAll("[data-reveal]");
    if ("IntersectionObserver" in window) {
        const io = new IntersectionObserver((entries) => {
            for (const entry of entries) {
                if (!entry.isIntersecting) continue;
                entry.target.classList.add("is-visible");
                io.unobserve(entry.target);
            }
        }, { threshold: 0.15, rootMargin: "0px 0px -40px 0px" });
        revealTargets.forEach((el) => io.observe(el));
    } else {
        revealTargets.forEach((el) => el.classList.add("is-visible"));
    }

    /* ---------- Sky: 저녁 하늘의 별과 하얀 비 ---------- */
    const canvas = document.querySelector(".sky");
    const ctx = canvas?.getContext("2d");
    if (ctx) {
        let w = 0, h = 0, stars = [], rain = [];

        const makeDrop = (anywhere) => ({
            x: Math.random() * (w + h * 0.3),
            y: anywhere ? Math.random() * h : -40 - Math.random() * h * 0.5,
            length: 26 + Math.random() * 54,
            speed: 0.1 + Math.random() * 0.16,
            alpha: 0.18 + Math.random() * 0.4,
        });

        const resize = () => {
            const dpr = Math.min(window.devicePixelRatio || 1, 2);
            w = canvas.clientWidth;
            h = canvas.clientHeight;
            canvas.width = w * dpr;
            canvas.height = h * dpr;
            ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
            stars = Array.from({ length: Math.round(w * h / 22000) }, () => ({
                x: Math.random() * w,
                y: Math.random() * h * 0.8,
                r: 0.6 + Math.random() * 1.3,
                speed: 0.4 + Math.random() * 1.2,
                phase: Math.random() * Math.PI * 2,
            }));
            rain = Array.from({ length: Math.round(w / 34) }, () => makeDrop(true));
        };

        const draw = (t, dt) => {
            ctx.clearRect(0, 0, w, h);
            ctx.fillStyle = "#fff";
            for (const s of stars) {
                ctx.globalAlpha = reduceMotion ? 0.8 : 0.5 + 0.5 * Math.sin(t * 0.001 * s.speed + s.phase);
                ctx.beginPath();
                ctx.arc(s.x, s.y, s.r, 0, Math.PI * 2);
                ctx.fill();
            }
            ctx.lineWidth = 1.2;
            ctx.lineCap = "round";
            for (const d of rain) {
                d.y += d.speed * dt;
                d.x -= d.speed * dt * 0.3;
                if (d.y - d.length > h) Object.assign(d, makeDrop(false));
                const trail = ctx.createLinearGradient(d.x, d.y, d.x + d.length * 0.3, d.y - d.length);
                trail.addColorStop(0, `rgba(255, 255, 255, ${d.alpha})`);
                trail.addColorStop(1, "rgba(255, 255, 255, 0)");
                ctx.globalAlpha = 1;
                ctx.strokeStyle = trail;
                ctx.beginPath();
                ctx.moveTo(d.x, d.y);
                ctx.lineTo(d.x + d.length * 0.3, d.y - d.length);
                ctx.stroke();
            }
            ctx.globalAlpha = 1;
        };

        resize();
        window.addEventListener("resize", () => { resize(); if (reduceMotion) draw(0, 0); });
        if (reduceMotion) {
            draw(0, 0);
        } else {
            let last = performance.now();
            const frame = (now) => {
                draw(now, Math.min(now - last, 50));
                last = now;
                requestAnimationFrame(frame);
            };
            requestAnimationFrame(frame);
        }
    }

    /* ---------- Header state & scroll progress ---------- */
    const header = document.querySelector(".site-header");
    const onScroll = () => {
        const max = root.scrollHeight - window.innerHeight;
        root.style.setProperty("--progress", max > 0 ? (window.scrollY / max).toFixed(4) : "0");
        header?.classList.toggle("is-scrolled", window.scrollY > 40);
    };
    window.addEventListener("scroll", onScroll, { passive: true });
    onScroll();

    /* ---------- Page top ---------- */
    document.querySelector(".page-top")?.addEventListener("click", () => {
        window.scrollTo({ top: 0, behavior: reduceMotion ? "auto" : "smooth" });
    });

    /* ---------- Mobile nav: 링크 클릭 시 닫기 ---------- */
    const navToggle = document.getElementById("nav-toggle");
    document.querySelectorAll(".site-nav a").forEach((a) =>
        a.addEventListener("click", () => { if (navToggle) navToggle.checked = false; }));

    /* ---------- Page transition (커튼 닫힘 → 이동) ---------- */
    document.addEventListener("click", (e) => {
        const a = e.target.closest("a[href]");
        if (!a || reduceMotion || e.defaultPrevented) return;
        if (e.button !== 0 || e.metaKey || e.ctrlKey || e.shiftKey || e.altKey) return;
        if (a.target && a.target !== "_self") return;
        if (a.hasAttribute("download")) return;

        const url = new URL(a.href, location.href);
        if (url.origin !== location.origin) return;
        // 같은 페이지 내 앵커 이동은 부드러운 스크롤만
        if (url.pathname === location.pathname && url.hash) return;

        e.preventDefault();
        root.classList.add("is-leaving");
        setTimeout(() => { location.href = url.href; }, 560);
    });
    // 뒤로가기(bfcache) 복귀 시 커튼 해제
    window.addEventListener("pageshow", (e) => { if (e.persisted) root.classList.remove("is-leaving"); });

    /* ---------- Project card: 포인터를 따라 기우는 홀로그램 카드 ---------- */
    if (!reduceMotion && window.matchMedia("(hover: hover) and (pointer: fine)").matches) {
        document.querySelectorAll(".project-card").forEach((card) => {
            card.addEventListener("pointermove", (e) => {
                const r = card.getBoundingClientRect();
                const x = (e.clientX - r.left) / r.width, y = (e.clientY - r.top) / r.height;
                card.style.setProperty("--mx", `${x * 100}%`);
                card.style.setProperty("--my", `${y * 100}%`);
                card.style.setProperty("--ry", `${(x - 0.5) * 9}deg`);
                card.style.setProperty("--rx", `${(0.5 - y) * 9}deg`);
            });
        });
    }

    /* ---------- Project tag filter ---------- */
    const filterButtons = document.querySelectorAll(".filter__btn");
    const cards = document.querySelectorAll(".project-card[data-tags]");
    filterButtons.forEach((btn) => btn.addEventListener("click", () => {
        const filter = btn.dataset.filter;
        filterButtons.forEach((b) => b.classList.toggle("is-active", b === btn));
        cards.forEach((card) => {
            const tags = card.dataset.tags ? card.dataset.tags.split("|") : [];
            const show = filter === "*" || tags.includes(filter);
            card.classList.toggle("is-hidden", !show);
            card.classList.remove("is-filtered-in");
            if (show) {
                card.classList.add("is-visible");
                void card.offsetWidth; // 애니메이션 재시작
                card.classList.add("is-filtered-in");
            }
        });
    }));
})();
