// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
if (window.vaccination === undefined) window.vaccination = {};

vaccination = {
    replaceClass(oldClass, newClass) {
        $element = $(`.${oldClass}`);
        $element.removeClass(oldClass);
        $element.addClass(newClass);
    }
};

(function () {

})();