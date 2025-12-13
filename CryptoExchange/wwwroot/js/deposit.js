document.addEventListener('DOMContentLoaded', () => {
    const wrapper = document.getElementById('depositFormWrapper');

    function bindDepositSubmit() {
        const form = document.getElementById('depositForm');
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

function showDeposit() {
    const modal = document.getElementById("depositModal");
    if (modal) modal.style.display = "flex";
}

function closeDeposit() {
    const modal = document.getElementById("depositModal");
    if (modal) modal.style.display = "none";
}

function formatCardNumber(input) { 
    let raw = input.value.replace(/\D/g, '').slice(0, 16);
     
    let formatted = raw.match(/.{1,4}/g);
    input.value = formatted ? formatted.join(' ') : '';
}

function formatExpiry(input) { 
    let raw = input.value.replace(/\D/g, '').slice(0, 6);

    let month = raw.slice(0, 2);
    let year = raw.slice(2);

    if (month.length === 2 && year.length > 0) {
        input.value = `${month}/${year}`;
    } else {
        input.value = month;
    }
     
    document.getElementById("expiryError").textContent = "";
}

function validateExpiry(input) {
    const value = input.value.trim();
    const errorSpan = document.getElementById("expiryError");

    const match = /^(\d{2})\/(\d{4})$/.exec(value);
    if (!match) {
        errorSpan.textContent = "Формат должен быть MM/YYYY";
        return false;
    }

    let month = parseInt(match[1], 10);
    let year = parseInt(match[2], 10);

    if (month < 1 || month > 12) {
        errorSpan.textContent = "Месяц должен быть от 01 до 12";
        return false;
    }

    const now = new Date();
    const expiryDate = new Date(year, month - 1, 1);
    const currentDate = new Date(now.getFullYear(), now.getMonth(), 1);

    if (expiryDate < currentDate) {
        errorSpan.textContent = "Карта просрочена";
        return false;
    }

    errorSpan.textContent = "";
    return true;
}

function validateCVV(input) { 
    input.value = input.value.replace(/\D/g, '');

    const error = document.getElementById('cvvError');
    if (input.value.length !== 3) { error.textContent = "CVV должен содержать 3 цифры"; }
    else { error.textContent = ""; }
}

function validateAmount(input) {  
    input.value = input.value.replace(/[^\d.]/g, '');

    const error = document.getElementById('amountError');
    const value = parseFloat(input.value);

    if (isNaN(value) || value < 0.01) { error.textContent = "Минимальная сумма пополнения — 1 USDT"; }
    else { error.textContent = ""; }
}