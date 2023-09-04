
function appendPartialView(viewName, parameter, context) {
    fetch(`/GetPartialView?viewName=${viewName}&parameter=${parameter}`)
        .then(response => response.text())
        .then(result => {
            context.innerHTML += result;
        });
}