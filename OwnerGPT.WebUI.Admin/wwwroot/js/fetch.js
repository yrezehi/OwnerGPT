
var fetchRequest = function () {
    const buildSuccessResponse = function (status, data) {
        return { success: true, status: status, data: data };
    };
    const buildErrorResponse = function (error) {
        return { success: false, error: error };
    };
    return function () {
        async function getRequest(endpoint) {
            return fetch(endpoint, {
                method: 'GET',
            }).then(response => response.json()
            .then((data) => buildSuccessResponse(response.status, data)))
            .catch(error => buildErrorResponse(error));
        }
        async function postRequest(endpoint) {
            return fetch(endpoint, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(body),
            }).then(response => response.json()
            .then((data) => { return buildSuccessResponse(response.status, data) }))
            .catch(error => buildErrorResponse(error));
        }
        async function deleteRequest(endpoint, id) {
            return fetch(endpoint + id, {
                method: 'GET',
            }).then(response => response.json())
            .then((data) => buildSuccessResponse(response.status, data))
            .catch(error => buildErrorResponse(error));
        }
        async function putRequest(endpoint) {
            return fetch(endpoint, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(body),
            }).then(response => response.json())
            .then((data) => buildSuccessResponse(response.status, data))
            .catch(error => buildErrorResponse(error));
        }
        return Object.freeze({
            getRequest,
            postRequest,
            deleteRequest,
            putRequest
        });
    }();
}();
