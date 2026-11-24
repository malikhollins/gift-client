(function () {
    "use strict";
    const key = "giftall.appearance";
    const allowed = ["system", "light", "dark"];
    const media = window.matchMedia("(prefers-color-scheme: dark)");

    function getPreference() {
        try {
            const value = window.localStorage.getItem(key);
            return allowed.includes(value) ? value : "system";
        } catch (_) {
            return window.giftAllThemeMemory || "system";
        }
    }

    function applyTheme() {
        const preference = getPreference();
        const actual = preference === "system" ? (media.matches ? "dark" : "light") : preference;
        document.documentElement.setAttribute("data-theme", actual);
        document.documentElement.setAttribute("data-theme-preference", preference);
        document.documentElement.style.colorScheme = actual;
        return actual;
    }

    window.giftAllTheme = {
        getPreference,
        setPreference: function (preference) {
            if (!allowed.includes(preference)) return;
            window.giftAllThemeMemory = preference;
            try { window.localStorage.setItem(key, preference); } catch (_) {}
            applyTheme();
        }
    };

    media.addEventListener("change", applyTheme);
    applyTheme();
})();
