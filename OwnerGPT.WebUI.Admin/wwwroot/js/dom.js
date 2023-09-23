var doom = function () {

    function createElementWithIdentifier(prefix, serlizedElement) {
        var identifier = prefix + "-" + utility.shortGUID();
        chatDialogContainer.innerHTML += serlizedElement;
        return document.querySelector(`#${identifier}`);
    }

    return function () {
        return Object.freeze({
            createUniqueElement: createElementWithIdentifier
        });
    }();
}();
