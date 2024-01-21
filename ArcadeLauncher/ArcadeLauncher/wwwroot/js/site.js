// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function scroller() {
    window.scroll({top: 1080, behavior: "smooth",})
}

function scrollBack() {
    window.scroll({ top: 0, behavior: "instant", })
}

window.addEventListener("load", scrollBack)

