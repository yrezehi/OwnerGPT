var dom = function () {

    function createElementFromSerlized(serlizedElement) {
        var temporaryElement = document.createElement("div");

        temporaryElement.innerHTML += serlizedElement;

        return temporaryElement.firstElementChild;
    }

    return function () {
        return Object.freeze({
            createElement: createElementFromSerlized
        });
    }();
}();
