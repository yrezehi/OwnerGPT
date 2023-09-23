var storage = function () {

    function setValue(key, value) {
        window.localStorage.setItem(key, value);
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
            nuke: nuke
        });
    }();
}();
