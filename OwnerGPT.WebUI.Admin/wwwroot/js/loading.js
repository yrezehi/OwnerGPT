
var loading = function () {

    return function () {
        return Object.freeze({
            unload: function () { },
            load: function () { },
        });
    }();
}();
