var globalProperties = function () {
    var properties = {};

    function setProperty(key, value) {
        setProperty = { ...properties, [key]: value };
    }

    return function () {
        return Object.freeze({
            set: setProperty,
            ...properties
        });
    }();
}();