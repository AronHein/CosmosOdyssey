function toggleCard(cardId, indicatorId) {
    const cardBody = document.getElementById(cardId);
    const indicator = document.getElementById(indicatorId);

    cardBody.classList.toggle('active');

    if (cardBody.classList.contains('active')) {
        indicator.innerHTML = '<i class="fas fa-chevron-up"></i>';
    } else {
        indicator.innerHTML = '<i class="fas fa-chevron-down"></i>';
    }
}

function filterRoutes() {
    const filterText = document.getElementById("filterCompany").value.toLowerCase();
    const providers = document.querySelectorAll(".provider-item");

    providers.forEach(provider => {
        const companyName = provider.getAttribute("data-company").toLowerCase();
        if (companyName.includes(filterText)) {
            provider.style.display = "";
        } else {
            provider.style.display = "none";
        }
    });
}
