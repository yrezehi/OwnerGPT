
var loading = function () {

    return function () {
        return Object.freeze({
            setElement: function () { },
            unsetElement: function () { },
            setSVG: function () { },
            unsetSVG: function () { }
        });
    }();
}();
