(function () {
    window.onunhandledrejection = event => {
        console.warn(`UNHANDLED PROMISE REJECTION: ${event.reason}`);
    };

    window.onerror = function (message, source, lineNumber, colno, error) {
        console.warn(`UNHANDLED ERROR: ${error.stack}`);
    };

    function upsertError() { }
})();