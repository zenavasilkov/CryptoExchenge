document.addEventListener('DOMContentLoaded', () => {
    const wrapper = document.getElementById('withdrawFormWrapper');

    function bindDepositSubmit() {
        const form = document.getElementById('withdrawForm');
        if (!form) return;

        form.addEventListener('submit', async function (e) {
            e.preventDefault();
            const formData = new FormData(form);
            const actionUrl = form.getAttribute('action');

            try {
                const response = await fetch(actionUrl, {
                    method: 'POST',
                    body: formData,
                    headers: { 'X-Requested-With': 'XMLHttpRequest' }
                });

                if (response.redirected) {
                    window.location.href = response.url;
                    return;
                }

                const html = await response.text();
                wrapper.innerHTML = html;
                bindDepositSubmit();
            } catch (err) {
                console.error('Ошибка при отправке формы:', err);
            }
        });
    }

    bindDepositSubmit();
});

function showWithdraw() {
    const modal = document.getElementById("withdrawModal");
    if (modal) modal.style.display = "flex";
}

function closeWithdraw() {
    const modal = document.getElementById("withdrawModal");
    if (modal) modal.style.display = "none";
}
 