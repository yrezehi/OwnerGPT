var utility = function () {

    return function () {
        return Object.freeze({
            shortGUID: function () {
                return Math.random().toString(36).slice(-6);
            },
            stringEllipsis(string, length) {
                return string.substring(0, length) + "...";
            }
        });
    }();
}();
