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

    function setItem(key, value) {
        if (value === Object(value)) {
            setObject(key, value);
        } else {
            setValue(key, value);
        }
    }

    function getItem(key) {
        if (value === Object(value)) {
            return getObject(key, value);
        } else {
            return getValue(key, value);
        }
    }

    function nuke(key) {
        if (key) {
            window.localStorage.removeItem(key);
        } else {
            window.localStorage.clear();
        }
    }
    
    return function () {
        return Object.freeze({
            set: setItem,
            get: getItem,
            nuke: nuke
        });
    }();
}();
