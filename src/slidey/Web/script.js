var page = 0;

const Keys = {
    LeftArrow: 37,
    RightArrow: 39,
    Space: 32
};

function loadPage(url) {
    return fetch(url, { method: "GET" }).then(response => response.text());
}

function transition() {
    const url = window.location.href;
    const newPage = parseInt(url.replace(/.*\//, ""));
    loadPage(url).then(html => {
        const newDocument = document.createElement("html");
        newDocument.innerHTML = html;

        document.querySelector("title").innerText = newDocument.querySelector("title").innerText;

        const main = document.querySelector("main");

        const oldNav = document.querySelector("nav");
        const newNav = newDocument.querySelector("nav");

        main.replaceChild(newNav, oldNav);

        const oldContent = document.querySelector("article");
        const newContent = newDocument.querySelector("article");

        main.appendChild(newContent);

        if (newPage > page) {
            newContent.classList.add("slide-in-from-right");
            oldContent.classList.add("slide-out-to-left");
        } else {
            newContent.classList.add("slide-in-from-left");
            oldContent.classList.add("slide-out-to-right");
        }


        oldContent.addEventListener("animationend",
            () => {
                oldContent.parentNode.removeChild(oldContent);
            });
    });

}

function go(href) {
    history.pushState(null, null, href);
    transition();
}

(() => {
    document.addEventListener("DOMContentLoaded", () => {
        page = parseInt(window.location.href.replace(/.*\//, ""));
        document.addEventListener("keydown", (e) => {
            if (e.keyCode === Keys.LeftArrow) {
                go(document.querySelector("#previous-link").href);
            }
            if (e.keyCode === Keys.RightArrow || e.keyCode === Keys.Space) {
                go(document.querySelector("#next-link").href);
            }
        });
        window.addEventListener("popstate", transition);
    });
})();