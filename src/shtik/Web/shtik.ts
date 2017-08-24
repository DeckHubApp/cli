namespace Shtik {

    var currentPage = 0;

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
        loadPage(url).then(html => {
            const newDocument = document.createElement("html");
            newDocument.innerHTML = html;

            document.querySelector("title").innerText = newDocument.querySelector("title").innerText;

            replaceNav(newDocument);

            animateTransition(newDocument);

            currentPage = parseInt(window.location.href.replace(/.*\//, ""));
        });
    }

    function replaceNav(newDocument: HTMLElement) {
        const main = document.querySelector("main");

        const oldNav = document.querySelector("nav");
        const newNav = newDocument.querySelector("nav");

        main.replaceChild(newNav, oldNav);
    }

    function animateTransition(newDocument: HTMLElement) {
        const newPage = parseInt(window.location.href.replace(/.*\//, ""));
        const main = document.querySelector("main");
        
        const oldContent = document.querySelector("article");
        const newContent = newDocument.querySelector("article");

        main.appendChild(newContent);

        if (newPage > currentPage) {
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
    }

    function go(href) {
        history.pushState(null, null, href);
        transition();
    }

    (() => {
        document.addEventListener("DOMContentLoaded", () => {
            currentPage = parseInt(window.location.href.replace(/.*\//, ""));
            document.addEventListener("keydown", (e: KeyboardEvent) => {
                let link: HTMLAnchorElement = null;
                if (e.keyCode === Keys.LeftArrow) {
                    link = document.querySelector("a#previous-link") as HTMLAnchorElement;
                }
                else if (e.keyCode === Keys.RightArrow || e.keyCode === Keys.Space) {
                    link = document.querySelector("a#next-link") as HTMLAnchorElement;
                }
                if (!!link) {
                    go(link.href);
                }
            });
            window.addEventListener("popstate", transition);
        });
    })();
}