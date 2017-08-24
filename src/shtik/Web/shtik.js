var Shtik;
(function (Shtik) {
    var currentPage = 0;
    var Keys = {
        LeftArrow: 37,
        RightArrow: 39,
        Space: 32
    };
    function loadPage(url) {
        return fetch(url, { method: "GET" }).then(function (response) { return response.text(); });
    }
    function transition() {
        var url = window.location.href;
        loadPage(url).then(function (html) {
            var newDocument = document.createElement("html");
            newDocument.innerHTML = html;
            document.querySelector("title").innerText = newDocument.querySelector("title").innerText;
            replaceNav(newDocument);
            animateTransition(newDocument);
            currentPage = parseInt(window.location.href.replace(/.*\//, ""));
        });
    }
    function replaceNav(newDocument) {
        var main = document.querySelector("main");
        var oldNav = document.querySelector("nav");
        var newNav = newDocument.querySelector("nav");
        main.replaceChild(newNav, oldNav);
    }
    function animateTransition(newDocument) {
        var newPage = parseInt(window.location.href.replace(/.*\//, ""));
        var main = document.querySelector("main");
        var oldContent = document.querySelector("article");
        var newContent = newDocument.querySelector("article");
        main.appendChild(newContent);
        if (newPage > currentPage) {
            newContent.classList.add("slide-in-from-right");
            oldContent.classList.add("slide-out-to-left");
        }
        else {
            newContent.classList.add("slide-in-from-left");
            oldContent.classList.add("slide-out-to-right");
        }
        oldContent.addEventListener("animationend", function () {
            oldContent.parentNode.removeChild(oldContent);
        });
    }
    function go(href) {
        history.pushState(null, null, href);
        transition();
    }
    (function () {
        document.addEventListener("DOMContentLoaded", function () {
            currentPage = parseInt(window.location.href.replace(/.*\//, ""));
            document.addEventListener("keydown", function (e) {
                var link = null;
                if (e.keyCode === Keys.LeftArrow) {
                    link = document.querySelector("a#previous-link");
                }
                else if (e.keyCode === Keys.RightArrow || e.keyCode === Keys.Space) {
                    link = document.querySelector("a#next-link");
                }
                if (!!link) {
                    go(link.href);
                }
            });
            window.addEventListener("popstate", transition);
        });
    })();
})(Shtik || (Shtik = {}));
