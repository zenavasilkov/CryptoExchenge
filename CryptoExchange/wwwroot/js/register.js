document.addEventListener('DOMContentLoaded', function () {
    const modal = document.getElementById('authModal');
    const modalContent = document.getElementById('modalContent');
    const openButtons = document.querySelectorAll('[data-toggle="auth-modal"]');
    const closeModalBtn = document.getElementById('modalClose');

    function initAuthModalEvents() {
        const loginTab = document.getElementById('loginTab');
        const registerTab = document.getElementById('registerTab');
        const loginContent = document.getElementById('loginContent');
        const registerContent = document.getElementById('registerContent');

        if (!loginTab || !registerTab || !loginContent || !registerContent) return;

        loginTab.addEventListener('click', function () {
            loginTab.classList.add('active');
            registerTab.classList.remove('active');
            loginContent.classList.add('active');
            registerContent.classList.remove('active');
        });

        registerTab.addEventListener('click', function () {
            registerTab.classList.add('active');
            loginTab.classList.remove('active');
            registerContent.classList.add('active');
            loginContent.classList.remove('active');
        });
    }

    function initFormSubmitHandlers() {
        const forms = modalContent.querySelectorAll('form');

        forms.forEach(form => {
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
                    } else {
                        const updatedHtml = await response.text();
                        modalContent.innerHTML = updatedHtml;
                        initAuthModalEvents();
                        initFormSubmitHandlers();
                    }
                } catch (err) {
                    console.error('Ошибка отправки формы:', err);
                }
            });
        });
    }

    openButtons.forEach(button => {
        button.addEventListener('click', async function () {
            try {
                const response = await fetch('/Account/RegisterForm');
                const html = await response.text();
                modalContent.innerHTML = html;
                initAuthModalEvents();
                initFormSubmitHandlers();
                modal.style.display = 'flex';
            } catch (err) {
                console.error('Ошибка загрузки формы:', err);
            }
        });
    });

    if (closeModalBtn) {
        closeModalBtn.addEventListener('click', function () {
            modal.style.display = 'none';
            modalContent.innerHTML = '';
        });
    }

    window.addEventListener('click', function (e) {
        if (e.target === modal) {
            modal.style.display = 'none';
            modalContent.innerHTML = '';
        }
    });
});