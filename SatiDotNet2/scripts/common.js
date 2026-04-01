class OverdueLogsCache {
    constructor(storage = sessionStorage) {
        this.storage = storage;
    }

    set(key, value) {
        const data = JSON.stringify(value)
        this.storage.setItem(key, data);
    }

    get(key) {
        const data = this.storage.getItem(key);
        return JSON.parse(data);
    }

    getAll() {
        const allData = {};

        for (let i = 0; i < sessionStorage.length; i++) {
            const key = sessionStorage.key(i);
            const value = sessionStorage.getItem(key);

            allData[key] = JSON.parse(value);
        }

        return allData;
    }

    getCount() {
        return sessionStorage.length;
    }

    remove(key) {
        this.storage.removeItem(key);
    }

    clear() {
        this.storage.clear();
    }
}

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

function packageQuerystringParams(obj) {
    let qs = "";

    const qsKeys = Object.keys(obj);
    for (let i = 0; i < qsKeys.length; i++) {
        const qsKey = qsKeys[i];
        const qsValue = obj[qsKey];

        if (i === 0) qs += "?";
        else qs += "&";

        qs += qsKey + "=" + qsValue;
    }

    return qs;
}

async function httpGet(url, qsObj) {
    if (qsObj) url += packageQuerystringParams(qsObj);

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

// =============== HTML DOM manipulation functions ==================

function iterateChildren(callback, elem) {
    //traverse through all child elements and invoke callback function on them
    callback.call(elem);
    for (const child of elem.children) iterateChildren(callback, child);
}

function getAspControl(id) {
    //get asp control supplying standard id rather than asp conglomerated id
    return document.querySelector('[id$="' + id + '"]');
}

function redirectClickTo(clickedElem, targetElem) {
    //you cannot set asp.net event on Panel control
    //as a workaround, this function sets a click event listener on an html element which programmatically clicks a second element to trigger the code-behind event
    clickedElem.addEventListener("click", function () {
        targetElem.click();
    })
}
