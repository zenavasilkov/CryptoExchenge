async function loadContent(sectionName) {
    try {
        const response = await fetch(`/Trade/${sectionName}`);
        const html = await response.text();
        document.getElementById("tradeContent").innerHTML = html;

        const sidebarLinks = document.querySelectorAll(".sidebar a");
        sidebarLinks.forEach(link => {
            link.classList.remove("active");
            if (link.getAttribute("onclick")?.toLowerCase().includes(sectionName.toLowerCase())) {
                link.classList.add("active");
            }
        });

        if (sectionName.startsWith("CreateOrder")) {
            if (typeof initCreateOrderForm === "function") {
                initCreateOrderForm();
            }
        }

    } catch (err) {
        console.error("Ошибка загрузки контента:", err);
        document.getElementById("tradeContent").innerHTML = "<p class='text-danger'>Не удалось загрузить данные.</p>";
    }
}

function initCreateOrderForm() {
    const form = document.querySelector("#createOrderForm");

    if (!form) return;
     
    const cleaveInstances = [];
    form.querySelectorAll(".numeric-input").forEach(input => {
        const cleave = new Cleave(input, {
            numeral: true,
            numeralThousandsGroupStyle: 'thousand',
            numeralDecimalMark: '.',
            delimiter: ' ',
            numeralDecimalScale: 8,  
            numeralPositiveOnly: true
        });
        cleaveInstances.push({ input, cleave });
    });

    form.addEventListener("submit", async function (e) {
        e.preventDefault();
         
        cleaveInstances.forEach(({ input, cleave }) => {
            input.value = cleave.getRawValue(); 
        });

        const formData = new FormData(form);

        try {
            const response = await fetch(form.action, {
                method: "POST",
                body: formData,
                headers: {
                    "X-Requested-With": "XMLHttpRequest"
                }
            });

            const contentType = response.headers.get("content-type");

            if (contentType && contentType.indexOf("application/json") !== -1) { 
                const result = await response.json();
                if (result.redirectUrl) {
                    loadContent("MyOrders"); 
                }
            } else { 
                const html = await response.text();
                document.getElementById("tradeContent").innerHTML = html;
                initCreateOrderForm();  
            }

        } catch (err) {
            console.error("Ошибка отправки формы:", err);
            alert("Произошла ошибка при отправке формы.");
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    loadContent("Dashboard");
});

function startTrade(currencyShortName) {
    loadContent(`CreateOrder?shortName=${currencyShortName}`);
}
  