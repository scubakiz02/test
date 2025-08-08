// ============== http request/response flow related functions =============== 

async function prepHttpError(httpRes) {
    let endUserMessage = '';
    try {
        const errorData = await httpRes.json();
        endUserMessage = errorData?.message || JSON.stringify(errorData);
    } catch {
        endUserMessage = await response.text();
    }

    throw {
        devMessage: `GET request failed: ${httpRes.status} ${httpRes.statusText}`,
        endUserMessage: endUserMessage || 'No message from server'
    };
}

function throwHttpError(catchErr) {
    //return detailed object when throwing error
    if (typeof catchErr === 'object' && catchErr.devMessage) {
        // Re-throw structured error
        throw catchErr;
    } else {
        // Catch unexpected errors (e.g., network failure)
        throw {
            devMessage: `GET request failed: ${httpRes.status} ${httpRes.statusText}`,
            endUserMessage: error.message || 'Unexpected error'
        };
    }
}

async function httpGet(url) {
    try {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            await prepHttpError(response);
        }

        return await response.json();
    } catch (error) {
        throwHttpError(error);
    }
}

async function httpPost(url, data) {
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            await prepHttpError(response);
        }

        return await response.json();
    } catch (error) {
        throwHttpError(error);
    }
}

// =============== HTML DOM manipulation function ==================

//traverse through all child elements and invoke callback function on them
function iterateChildren(callback, elem) {
    callback.call(elem);
    for (const child of elem.children) iterateChildren(callback, child);
}

//get asp control supplying standard id rather than asp conglomerated id
function getAspControl(id) {
    return document.querySelector('[id$="' + id + '"]');
}

