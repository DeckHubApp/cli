var Shtik;
(function (Shtik) {
    var rightArrow = "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" version=\"1.1\" id=\"Layer_1\" x=\"0px\" y=\"0px\" viewBox=\"0 0 492.004 492.004\" style=\"enable-background:new 0 0 492.004 492.004;\" xml:space=\"preserve\" width=\"32px\" height=\"32px\">\n<g><g><path d=\"M382.678,226.804L163.73,7.86C158.666,2.792,151.906,0,144.698,0s-13.968,2.792-19.032,7.86l-16.124,16.12    c-10.492,10.504-10.492,27.576,0,38.064L293.398,245.9l-184.06,184.06c-5.064,5.068-7.86,11.824-7.86,19.028    c0,7.212,2.796,13.968,7.86,19.04l16.124,16.116c5.068,5.068,11.824,7.86,19.032,7.86s13.968-2.792,19.032-7.86L382.678,265    c5.076-5.084,7.864-11.872,7.848-19.088C390.542,238.668,387.754,231.884,382.678,226.804z\" fill=\"#FFDA44\"/></g></g>\n</svg>";
    var leftArrow = "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" version=\"1.1\" id=\"Layer_1\" x=\"0px\" y=\"0px\" viewBox=\"0 0 492 492\" style=\"enable-background:new 0 0 492 492;\" xml:space=\"preserve\" width=\"32px\" height=\"32px\">\n<g><g><path d=\"M198.608,246.104L382.664,62.04c5.068-5.056,7.856-11.816,7.856-19.024c0-7.212-2.788-13.968-7.856-19.032l-16.128-16.12    C361.476,2.792,354.712,0,347.504,0s-13.964,2.792-19.028,7.864L109.328,227.008c-5.084,5.08-7.868,11.868-7.848,19.084    c-0.02,7.248,2.76,14.028,7.848,19.112l218.944,218.932c5.064,5.072,11.82,7.864,19.032,7.864c7.208,0,13.964-2.792,19.032-7.864    l16.124-16.12c10.492-10.492,10.492-27.572,0-38.06L198.608,246.104z\" fill=\"#FFDA44\"/></g></g>\n</svg>";
    // ReSharper restore InconsistentNaming
    var currentPage = 0;
    var inTransition = false;
    var Keys = {
        LeftArrow: 37,
        RightArrow: 39,
        Space: 32
    };
    function loadPage(url) {
        return fetch(url, { method: "GET" }).then(function (response) { return response.text(); });
    }
    function screenshot() {
        var canvas = document.createElement("canvas");
        canvas.width = 1920;
        canvas.height = 1080;
        rasterizeHTML.drawURL(window.location.href, canvas).then(function () {
            canvas.toBlob(function (b) {
                var headers = new Headers();
                headers.append("Content-Type", "image/jpeg");
                headers.append("Length", b.size.toString());
                fetch("shot/" + currentPage, {
                    method: "POST",
                    body: b,
                    headers: headers
                });
            }, "image/jpeg", 0.9);
            //const link = document.createElement("a");
            //link.download = "x.jpg";
            //link.href = canvas.toDataURL("image/jpeg", 0.9);
            //link.click();
        });
    }
    function transition() {
        var url = window.location.href;
        loadPage(url).then(function (html) {
            var newDocument = document.createElement("html");
            newDocument.innerHTML = html;
            document.querySelector("title").innerText = newDocument.querySelector("title").innerText;
            animateTransition(newDocument);
            currentPage = parseInt(window.location.href.replace(/.*\//, ""));
            replaceNav();
        });
        screenshot();
    }
    function animateTransition(newDocument) {
        inTransition = true;
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
            inTransition = false;
        });
    }
    function go(href) {
        history.pushState(null, null, href);
        transition();
    }
    function replaceNav() {
        var oldNav = document.querySelector("nav");
        if (!!oldNav) {
            oldNav.remove();
        }
        var nav = document.createElement("nav");
        if (currentPage > 0) {
            var previousLink = document.createElement("a");
            previousLink.id = "previous-link";
            previousLink.href = "" + (currentPage - 1);
            previousLink.innerHTML = leftArrow;
            nav.appendChild(previousLink);
        }
        var nextLink = document.createElement("a");
        nextLink.id = "next-link";
        nextLink.href = "" + (currentPage + 1);
        nextLink.innerHTML = rightArrow;
        nav.appendChild(nextLink);
        document.querySelector("main").appendChild(nav);
    }
    (function () {
        document.addEventListener("DOMContentLoaded", function () {
            currentPage = parseInt(window.location.href.replace(/.*\//, ""));
            replaceNav();
            document.addEventListener("keydown", function (e) {
                if (inTransition)
                    return;
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
        screenshot();
    })();
})(Shtik || (Shtik = {}));
//# sourceMappingURL=slidey.js.map