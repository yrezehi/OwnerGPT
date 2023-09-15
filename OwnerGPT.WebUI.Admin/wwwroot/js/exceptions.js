(function () {
    window.onunhandledrejection = event => {
        notification.error(event.reason);
        console.warn(`UNHANDLED PROMISE REJECTION: ${event.reason}`);
    };

    window.onerror = function (message, source, lineNumber, colno, error) {
        notification.error(message);
        console.warn(`UNHANDLED ERROR: ${error.stack}`);
    };

    // TODO: send the error stack to the backend logic
    function upsertError() { }
})();