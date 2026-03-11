(function () {
    'use strict';

    // ── Client-side collection ─────────────────────────────
    const rutinaEjercicios = [];

    // ── Pending exercise (set when the form opens) ─────────
    let pendingEjercicio = null;

    // ── DOM helpers ────────────────────────────────────────
    const $ = (id) => document.getElementById(id);

    // ── Render the right-panel exercise list ───────────────
    function renderRutina() {
        const list  = $('cr-rutina-list');
        const bdg   = $('cr-rutina-badge');
        const btn   = $('cr-btn-finish');
        const count = rutinaEjercicios.length;

        bdg.textContent = count === 1 ? '1 ejercicio' : count + ' ejercicios';
        bdg.classList.toggle('cr-rutina-badge-active', count > 0);

        btn.disabled = count === 0;

        if (count === 0) {
            list.innerHTML =
                '<div class="cr-rutina-empty" id="cr-rutina-empty-state">' +
                '  <div class="cr-empty-icon-wrap"><i class="bi bi-clipboard2-plus"></i></div>' +
                '  <p class="cr-empty-title">Rutina vacía</p>' +
                '  <p class="cr-empty-text">Añade ejercicios desde el panel izquierdo para empezar a construir tu rutina.</p>' +
                '</div>';
            return;
        }

        list.innerHTML = rutinaEjercicios.map(function (ej, i) {
            var equipTag = ej.equipamiento
                ? '<span class="cr-rutina-tag cr-rutina-tag-equip"><i class="bi bi-tools"></i> ' + ej.equipamiento + '</span>'
                : '';

            return (
                '<div class="cr-rutina-ej" data-index="' + i + '">' +
                '  <span class="cr-rutina-ej-num">' + String(i + 1).padStart(2, '0') + '</span>' +
                '  <div class="cr-rutina-ej-icon"><i class="bi ' + ej.icon + '"></i></div>' +
                '  <div class="cr-rutina-ej-body">' +
                '    <p class="cr-rutina-ej-name">' + ej.nombre + '</p>' +
                '    <div class="cr-rutina-ej-meta">' +
                '      <span class="cr-rutina-tag cr-rutina-tag-grupo">' + ej.grupo + '</span>' +
                       equipTag +
                '    </div>' +
                '    <div class="cr-rutina-ej-stats">' +
                '      <span class="cr-rutina-stat"><i class="bi bi-layers"></i> ' + ej.seriesObjetivo + ' series</span>' +
                '      <span class="cr-rutina-stat"><i class="bi bi-arrow-repeat"></i> ' + ej.repeticionesObjetivo + ' reps</span>' +
                '    </div>' +
                '  </div>' +
                '  <button class="cr-rutina-ej-remove" data-index="' + i + '" title="Quitar">' +
                '    <i class="bi bi-x-lg"></i>' +
                '  </button>' +
                '</div>'
            );
        }).join('');

        list.querySelectorAll('.cr-rutina-ej-remove').forEach(function (removeBtn) {
            removeBtn.addEventListener('click', function () {
                var idx = parseInt(removeBtn.dataset.index, 10);
                rutinaEjercicios.splice(idx, 1);
                rutinaEjercicios.forEach(function (ej, i) { ej.orden = i + 1; });
                renderRutina();
                syncAddButtons();
            });
        });
    }

    // ── Open the inline form ───────────────────────────────
    function openForm(data) {
        if (rutinaEjercicios.some(function (e) { return e.idEjercicio === data.id; })) return;

        pendingEjercicio = data;

        $('cr-form-icon').innerHTML = '<i class="bi ' + data.icon + '"></i>';
        $('cr-form-name').textContent = data.nombre;
        $('cr-form-grupo').textContent = data.grupo;

        $('cr-form-series').value = 4;
        $('cr-form-reps').value = 10;

        $('cr-form-overlay').classList.add('is-open');

        setTimeout(function () { $('cr-form-series').focus(); }, 150);
    }

    // ── Close the form ─────────────────────────────────────
    function closeForm() {
        $('cr-form-overlay').classList.remove('is-open');
        pendingEjercicio = null;
    }

    // ── Confirm: add exercise to collection ────────────────
    function confirmAdd() {
        if (!pendingEjercicio) return;

        var series = parseInt($('cr-form-series').value, 10);
        var reps   = parseInt($('cr-form-reps').value, 10);

        if (!series || series < 1) { $('cr-form-series').focus(); return; }
        if (!reps   || reps   < 1) { $('cr-form-reps').focus();   return; }

        rutinaEjercicios.push({
            idEjercicio:          pendingEjercicio.id,
            nombre:               pendingEjercicio.nombre,
            grupo:                pendingEjercicio.grupo,
            equipamiento:         pendingEjercicio.equipamiento || '',
            icon:                 pendingEjercicio.icon,
            seriesObjetivo:       series,
            repeticionesObjetivo: reps,
            orden:                rutinaEjercicios.length + 1
        });

        closeForm();
        renderRutina();
        syncAddButtons();
    }

    // ── Sync add-button visual state ───────────────────────
    function syncAddButtons() {
        document.querySelectorAll('.cr-ej-add').forEach(function (btn) {
            var inRutina = rutinaEjercicios.some(function (e) { return e.idEjercicio === btn.dataset.id; });
            btn.classList.toggle('cr-ej-add-done', inRutina);
            btn.innerHTML = inRutina
                ? '<i class="bi bi-check-lg"></i>'
                : '<i class="bi bi-plus-lg"></i>';
            btn.title = inRutina ? 'Ya en la rutina' : 'Añadir a la rutina';
        });
    }

    // ── Bind add buttons (called on load + HTMX swap) ──────
    window.bindAddButtons = function () {
        document.querySelectorAll('.cr-ej-add').forEach(function (btn) {
            btn.addEventListener('click', function () {
                openForm({
                    id:           btn.dataset.id,
                    nombre:       btn.dataset.nombre,
                    grupo:        btn.dataset.grupo,
                    equipamiento: btn.dataset.equipamiento,
                    icon:         btn.dataset.icon
                });
            });
        });
        syncAddButtons();
    };

    // ── Toast notification ─────────────────────────────────
    function showToast(message, type) {
        var toast = $('cr-toast');
        var icon  = $('cr-toast-icon');
        var text  = $('cr-toast-text');

        toast.className = 'cr-toast';

        if (type === 'success') {
            toast.classList.add('cr-toast-success');
            icon.className = 'bi bi-check-circle-fill';
        } else {
            toast.classList.add('cr-toast-error');
            icon.className = 'bi bi-exclamation-triangle-fill';
        }

        text.textContent = message;
        toast.classList.add('cr-toast-visible');

        setTimeout(function () {
            toast.classList.remove('cr-toast-visible');
        }, 4000);
    }

    // ── Reset everything after success ─────────────────────
    function resetRutina() {
        rutinaEjercicios.length = 0;
        $('cr-rutina-nombre').value = '';
        renderRutina();
        syncAddButtons();
    }

    // ── Submit routine to server ───────────────────────────
    function submitRutina() {
        var nombre = ($('cr-rutina-nombre') || {}).value || '';
        var btn    = $('cr-btn-finish');

        // Client-side validations
        if (!nombre.trim()) {
            showToast('Dale un nombre a tu rutina antes de guardarla.', 'error');
            $('cr-rutina-nombre').focus();
            return;
        }

        if (rutinaEjercicios.length === 0) {
            showToast('Añade al menos un ejercicio a la rutina.', 'error');
            return;
        }

        // Build payload matching CrearRutinaRequest
        var payload = {
            nombre: nombre.trim(),
            ejercicios: rutinaEjercicios.map(function (ej) {
                return {
                    idEjercicio:          parseInt(ej.idEjercicio, 10),
                    seriesObjetivo:       ej.seriesObjetivo,
                    repeticionesObjetivo: ej.repeticionesObjetivo,
                    orden:                ej.orden
                };
            })
        };

        // Disable button while sending
        btn.disabled = true;
        btn.innerHTML = '<i class="bi bi-arrow-repeat cr-btn-spin"></i> Guardando…';

        fetch('/Rutinas/CrearRutina', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        })
        .then(function (response) {
            if (response.ok) {
                return response.json().then(function (data) {
                    showToast(data.mensaje || '¡Rutina creada correctamente!', 'success');
                    resetRutina();
                });
            } else if (response.status === 401) {
                showToast('Debes iniciar sesión para crear una rutina.', 'error');
            } else {
                return response.json().then(function (data) {
                    showToast(data.error || 'Algo salió mal. Inténtalo de nuevo.', 'error');
                });
            }
        })
        .catch(function () {
            showToast('Error de conexión. Comprueba tu red e inténtalo de nuevo.', 'error');
        })
        .finally(function () {
            btn.innerHTML = '<i class="bi bi-check2-circle"></i> Terminar rutina';
            // Button stays disabled if list is empty (resetRutina already handles it)
            if (rutinaEjercicios.length > 0) {
                btn.disabled = false;
            }
        });
    }

    // ── Init ───────────────────────────────────────────────
    document.addEventListener('DOMContentLoaded', function () {
        window.bindAddButtons();

        // Form events
        $('cr-form-close').addEventListener('click', closeForm);
        $('cr-form-cancel').addEventListener('click', closeForm);
        $('cr-form-confirm').addEventListener('click', confirmAdd);

        $('cr-form-overlay').addEventListener('click', function (e) {
            if (e.target === $('cr-form-overlay')) closeForm();
        });

        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && $('cr-form-overlay').classList.contains('is-open')) {
                closeForm();
            }
        });

        [$('cr-form-series'), $('cr-form-reps')].forEach(function (input) {
            input.addEventListener('keydown', function (e) {
                if (e.key === 'Enter') { e.preventDefault(); confirmAdd(); }
            });
        });

        // Finish button → submit
        $('cr-btn-finish').addEventListener('click', submitRutina);

        // Toast dismiss on click
        $('cr-toast').addEventListener('click', function () {
            $('cr-toast').classList.remove('cr-toast-visible');
        });
    });

})();