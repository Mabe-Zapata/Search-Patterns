// Scroll to element with smooth behavior
window.scrollToElement = function (element) {
    if (element) {
        element.scrollIntoView({
            behavior: 'smooth',
            block: 'nearest',
            inline: 'nearest'
        });
    }
};
