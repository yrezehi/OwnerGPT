
function getPartialView(viewName, context) {
    fetch(`/GetPartialView?viewName=${viewName}`)
        .then(response => response.text())
        .then(result => {
            context.innerHTML += result;
        });
}