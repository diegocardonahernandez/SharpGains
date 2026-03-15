(function () {
    "use strict";

    const STORAGE_PREFIX = "sg_sesion_series_";
    const STORAGE_NOTAS_PREFIX = "sg_sesion_notas_";

    function getRutinaId() {
        const input = document.getElementById("idRutinaSeleccionada");
        return parseInt(input?.value ?? "0", 10) || 0;
    }

    function getSeriesStorageKey() {
        return STORAGE_PREFIX + getRutinaId();
    }

    function getNotasStorageKey() {
        return STORAGE_NOTAS_PREFIX + getRutinaId();
    }

    function readSeries() {
        const raw = localStorage.getItem(getSeriesStorageKey());
        if (!raw) return [];
        try {
            const parsed = JSON.parse(raw);
            return Array.isArray(parsed) ? parsed : [];
        } catch {
            return [];
        }
    }

    function saveSeries(series) {
        localStorage.setItem(getSeriesStorageKey(), JSON.stringify(series));
    }

    function readNotas() {
        return localStorage.getItem(getNotasStorageKey()) ?? "";
    }

    function saveNotas(notas) {
        localStorage.setItem(getNotasStorageKey(), notas ?? "");
    }

    function findIndex(series, idEjercicio, numeroSerie) {
        return series.findIndex(s => s.IdEjercicio === idEjercicio && s.NumeroSerie === numeroSerie);
    }

    function showToast(message, type) {
        const toast = document.getElementById("se-toast");
        const text = document.getElementById("se-toast-text");
        const icon = document.getElementById("se-toast-icon");
        if (!toast || !text || !icon) return;

        toast.className = "se-toast";
        if (type === "success") {
            toast.classList.add("se-toast-success");
            icon.className = "bi bi-check-circle-fill";
        } else {
            toast.classList.add("se-toast-error");
            icon.className = "bi bi-exclamation-triangle-fill";
        }

        text.textContent = message;
        toast.classList.add("se-toast-visible");

        setTimeout(() => toast.classList.remove("se-toast-visible"), 3500);
    }

    function setRowSavedState(row, isSaved) {
        const confirmBtn = row.querySelector(".se-series-confirm");
        const clearBtn = row.querySelector(".se-series-clear");

        row.classList.toggle("is-completed", isSaved);
        confirmBtn?.classList.toggle("is-completed", isSaved);
        confirmBtn?.setAttribute("aria-pressed", isSaved ? "true" : "false");

        if (clearBtn) clearBtn.disabled = !isSaved;

        updateFinishButtonState();
    }

    function getRowData(row) {
        const idEjercicio = parseInt(row.dataset.idEjercicio ?? "0", 10);
        const numeroSerie = parseInt(row.dataset.numeroSerie ?? "0", 10);

        const pesoInput = row.querySelector('[data-field="peso"]');
        const repInput = row.querySelector('[data-field="repeticiones"]');
        const rpeInput = row.querySelector('[data-field="rpe"]');

        const peso = parseFloat((pesoInput?.value ?? "0").toString().replace(",", "."));
        const repeticiones = parseInt(repInput?.value ?? "0", 10);
        const rpeRaw = (rpeInput?.value ?? "").trim();
        const rpe = rpeRaw === "" ? null : parseInt(rpeRaw, 10);

        return {
            idEjercicio: Number.isNaN(idEjercicio) ? 0 : idEjercicio,
            numeroSerie: Number.isNaN(numeroSerie) ? 0 : numeroSerie,
            peso: Number.isNaN(peso) ? 0 : Math.round(Math.max(0, peso) * 100) / 100,
            repeticiones: Number.isNaN(repeticiones) ? 0 : repeticiones,
            rpe: Number.isNaN(rpe) ? null : rpe
        };
    }

    function buildSerie(row) {
        const data = getRowData(row);
        return {
            IdEjercicio: data.idEjercicio,
            NumeroSerie: data.numeroSerie,
            Peso: data.peso,
            Repeticiones: data.repeticiones,
            Rpe: data.rpe
        };
    }

    function upsertSerie(row) {
        const serie = buildSerie(row);
        const series = readSeries();
        const idx = findIndex(series, serie.IdEjercicio, serie.NumeroSerie);

        if (idx >= 0) series[idx] = serie;
        else series.push(serie);

        saveSeries(series);
        setRowSavedState(row, true);
    }

    function clearSerie(row) {
        const idEjercicio = parseInt(row.dataset.idEjercicio ?? "0", 10);
        const numeroSerie = parseInt(row.dataset.numeroSerie ?? "0", 10);

        const series = readSeries();
        const idx = findIndex(series, idEjercicio, numeroSerie);
        if (idx >= 0) {
            series.splice(idx, 1);
            saveSeries(series);
        }

        const pesoInput = row.querySelector('[data-field="peso"]');
        const repInput = row.querySelector('[data-field="repeticiones"]');
        const rpeInput = row.querySelector('[data-field="rpe"]');

        if (pesoInput) pesoInput.value = "";
        if (repInput) repInput.value = "";
        if (rpeInput) rpeInput.value = "";

        setRowSavedState(row, false);
    }

    function hydrateRow(row) {
        const idEjercicio = parseInt(row.dataset.idEjercicio ?? "0", 10);
        const numeroSerie = parseInt(row.dataset.numeroSerie ?? "0", 10);
        const series = readSeries();

        const saved = series.find(s => s.IdEjercicio === idEjercicio && s.NumeroSerie === numeroSerie);
        if (!saved) {
            setRowSavedState(row, false);
            return;
        }

        const pesoInput = row.querySelector('[data-field="peso"]');
        const repInput = row.querySelector('[data-field="repeticiones"]');
        const rpeInput = row.querySelector('[data-field="rpe"]');

        if (pesoInput) pesoInput.value = saved.Peso ?? "";
        if (repInput) repInput.value = saved.Repeticiones ?? "";
        if (rpeInput) rpeInput.value = saved.Rpe ?? "";

        setRowSavedState(row, true);
    }

    function isAllSeriesCompleted() {
        const rows = document.querySelectorAll(".se-series-row");
        if (rows.length === 0) return false;

        for (const row of rows) {
            if (!row.classList.contains("is-completed")) return false;
        }
        return true;
    }

    function updateFinishButtonState() {
        const finishBtn = document.getElementById("se-btn-finish");
        if (!finishBtn) return;

        const completed = isAllSeriesCompleted();
        finishBtn.disabled = !completed;
        finishBtn.setAttribute("aria-disabled", completed ? "false" : "true");
        finishBtn.classList.toggle("is-enabled", completed);
    }

    function setupNotasPersistence() {
        const notasInput = document.getElementById("se-notas-sesion");
        if (!notasInput) return;

        notasInput.value = readNotas();
        notasInput.addEventListener("input", () => saveNotas(notasInput.value));
    }

    function buildFinalizarSesionPayload() {
        const notasInput = document.getElementById("se-notas-sesion");
        return {
            idRutina: getRutinaId(),
            notas: (notasInput?.value ?? "").trim() || null,
            series: readSeries()
                .map(s => ({
                    IdEjercicio: parseInt(s.IdEjercicio, 10),
                    NumeroSerie: parseInt(s.NumeroSerie, 10),
                    Peso: Math.round((parseFloat(s.Peso) || 0) * 100) / 100,
                    Repeticiones: parseInt(s.Repeticiones, 10),
                    Rpe: s.Rpe === null || s.Rpe === "" ? null : parseInt(s.Rpe, 10)
                }))
                .sort((a, b) => a.IdEjercicio - b.IdEjercicio || a.NumeroSerie - b.NumeroSerie)
        };
    }

    async function finalizarSesionAjax() {
        if (!isAllSeriesCompleted()) {
            showToast("Completa todas las series antes de finalizar.", "error");
            return;
        }

        const finishBtn = document.getElementById("se-btn-finish");
        if (!finishBtn) return;

        const originalHtml = finishBtn.innerHTML;
        finishBtn.disabled = true;
        finishBtn.classList.add("is-loading");
        finishBtn.innerHTML = '<i class="bi bi-arrow-repeat se-spin"></i> Finalizando...';

        try {
            const payload = buildFinalizarSesionPayload();

            const response = await fetch("/Sesiones/FinalizarSesion", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (!response.ok) {
                let errorMsg = "No se pudo finalizar la sesión.";
                try {
                    const data = await response.json();
                    if (data?.error) errorMsg = data.error;
                } catch { }
                throw new Error(errorMsg);
            }

            localStorage.removeItem(getSeriesStorageKey());
            localStorage.removeItem(getNotasStorageKey());

            showToast("Sesión finalizada correctamente.", "success");

            finishBtn.classList.remove("is-loading");
            finishBtn.classList.remove("is-enabled");
            finishBtn.classList.add("is-done");
            finishBtn.innerHTML = '<i class="bi bi-check2-circle"></i> Sesión finalizada';
        } catch (error) {
            showToast(error.message || "Error inesperado al finalizar.", "error");
            finishBtn.classList.remove("is-loading");
            finishBtn.disabled = !isAllSeriesCompleted();
            finishBtn.innerHTML = originalHtml;
            updateFinishButtonState();
        }
    }

    function bindRow(row) {
        const confirmBtn = row.querySelector(".se-series-confirm");
        const clearBtn = row.querySelector(".se-series-clear");

        confirmBtn?.addEventListener("click", () => upsertSerie(row));
        clearBtn?.addEventListener("click", () => clearSerie(row));
    }

    document.addEventListener("DOMContentLoaded", () => {
        document.querySelectorAll(".se-series-row").forEach(row => {
            hydrateRow(row);
            bindRow(row);
        });

        setupNotasPersistence();
        updateFinishButtonState();

        const finishBtn = document.getElementById("se-btn-finish");
        finishBtn?.addEventListener("click", finalizarSesionAjax);
    });
})();