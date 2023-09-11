var keyboard = function () {

    function bindEnterToSubmit(event, context) {
        if (event.which === 13) {
            if (!event.repeat) {
                context.closest("form").submit();
            }
            event.preventDefault();
        }
    }

    function bindEnterToTrigger(event, callback) {
        if (event.which === 13) {
            if (!event.repeat) {
                callback();
            }
            event.preventDefault();
        }
    }

    function bindEventLisnter(context, event, callback) {
        context.addEventListener(event, callback);
    }

    return function () {
        return Object.freeze({
            enterToSubmit: function (context) {
                bindEventLisnter(context, "keydown", function (event) {
                    bindEnterToSubmit(event, context);
                });
            },
            enterToTrigger: function (context, callback) {
                bindEventLisnter(context, "keydown", function (event) {
                    bindEnterToTrigger(event, callback);
                });
            },
        });
    }();
}();
