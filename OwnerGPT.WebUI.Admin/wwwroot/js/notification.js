
var notification = function () {
    var notificationIstance = getNotificationIstance();

    function getNotificationIstance() {
        return new Polipop('polipop', {
            layout: 'popups',
            insert: 'before',
            life: 5000,
            icons: false,
            closer: false
        });
    }

    function spawnNotification(content, title, type) {
        notificationIstance.add(
            {
                content: content,
                title: title,
                type: type,
            }
        );
    }

    return function () {
        return Object.freeze({
            error: function (title, content) {
                spawnNotification(title, content, "error");
                console.error("GET STACKTRACK PLEASE");
            },
            information: function (title, content) {
                spawnNotification(title, content, "info");
                console.info(title, content);
            },
            warning: function (title, content) {
                spawnNotification(title, content, "warning");
                console.warn(title, content);
            },
            success: function (title, content) {
                spawnNotification(title, content, "success");
                console.info(title, content);
            }
        });
    }();
}();
