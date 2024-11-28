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
