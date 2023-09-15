var utility = function () {

    return function () {
        return Object.freeze({
            shortGUID: function () {
                return Math.random().toString(36).slice(-6);
            },
            stringEllipsis: function (string, length) {
                return string.substring(0, length) + "...";
            },
            sizeToReadableString: function (size) {
                var carrier = size == 0 ? 0 : Math.floor(Math.log(size) / Math.log(1024));
                return (size / Math.pow(1024, carrier)).toFixed(2) * 1 + ' ' + ['B', 'kB', 'MB', 'GB', 'TB'][carrier];
            },
            wordCount: function(string) {
                return string.trim().split(/\s+/).length;
            }
        });
    }();
}();
