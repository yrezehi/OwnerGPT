
var partialViewLoader = function () {

    function appendTo(viewName, parameter, context) {
        fetch(`/PartialView?viewName=${viewName}&parameter=${parameter}`)
            .then(response => response.text())
            .then(result => {
                context.innerHTML += result;
        });
    }

    return function () {
        return Object.freeze({
            appendTo: appendTo,
        });
    }();
}();