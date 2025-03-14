function returnTrue() {
    return true;
}

function haultDateInput(dateInput, latestChar) {
    let res;
    const dateInputLength = dateInput.length;

    if (latestChar === "Backspace") return false;

    if (dateInputLength === 5) {
        res = true;
    }
    else if (dateInputLength === 3 && latestChar === "/") {
        res = true;
    }
    else {
        res = false;
    }

    return res;
}

module.exports = { returnTrue, haultDateInput };