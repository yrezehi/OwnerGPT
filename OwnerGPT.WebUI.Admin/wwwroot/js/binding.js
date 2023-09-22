var binding = function () {

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

    function bindClickToSubmit(event, context) {
        context.closest("form").submit();
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
            clickToSubmit: function (context) {
                bindEventLisnter(context, "click", function (event) {
                    bindClickToSubmit(event, context);
                });
            },
            clickToTrigger: function (context, callback) {
                bindEventLisnter(context, "click", function (event) {
                    callback(event);
                });
            },
            uploadTrigger: function (context, callback) {
                bindEventLisnter(context, "change", function (event) {
                    callback(event);
                });
            },
            dropTrigger: function (context, callback) {
                ["dragover"].forEach(function (eventName) {
                    bindEventLisnter(context, eventName, function (event) {
                        event.preventDefault();
                    });
                });

                bindEventLisnter(context, "drop", function (event) {
                    callback(event);
                }, false);
            }
        });
    }();
}();
