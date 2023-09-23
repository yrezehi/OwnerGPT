var globalProperties = function () {

    var properties = {};

    function setProperty(key, value) {
        properties = { ...properties, [key]: value };
    }

    function getProperty(key) {

        if (!properties.hasOwnProperty(key)) {
            notification.error(`global property named ${key} can't be found!`);
            return;
        }

        return properties[key];
    }

    return function () {
        return {
            set: setProperty,
            get: getProperty,
        };
    }();
}();