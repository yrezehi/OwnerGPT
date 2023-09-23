var storage = function () {

    function setValue(key, value) {
        window.localStorage.setItem(key, value);
    }

    function getValue(key) {
        return window.localStorage.getItem(key);
    }

    function setObject(key, object) {
        return window.localStorage.setItem(key, JSON.stringify(object));
    }

    function getObject(key) {
        return JSON.parse(window.localStorage.getItem(key));
    }

    function nuke(key) {
        if (key) {
            window.localStorage.clear();
        } else {
            window.localStorage.clear();
        }
    }
    
    return function () {
        return Object.freeze({
            setValue: setValue,
            getValue: getValue,

            setObject: setObject,
            getObject: getObject, 

            nuke: nuke
        });
    }();
}();
