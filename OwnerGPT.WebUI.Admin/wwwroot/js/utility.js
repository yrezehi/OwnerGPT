var utility = function () {

    return function () {
        return Object.freeze({
            shortGUID: function () {
                return Math.random().toString(36).slice(-6);
            }
        });
    }();
}();
