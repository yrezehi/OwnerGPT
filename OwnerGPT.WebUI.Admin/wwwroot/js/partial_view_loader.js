
var partialViewLoader = function () {

    async function appendTo(viewName, parameter, context) {
        return await fetch(`/GetPartialView?viewName=${viewName}&parameter=${parameter}`)
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