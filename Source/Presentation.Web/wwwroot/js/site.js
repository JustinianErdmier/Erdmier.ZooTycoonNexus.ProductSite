window.scrollToSection = function (id) {
    const element = document.getElementById(id);
    if (element) {
        window.scrollTo({ top: element.getBoundingClientRect().top + window.scrollY - 54, behavior: "smooth" }); // 54 px = sticky nav height
    }
};
