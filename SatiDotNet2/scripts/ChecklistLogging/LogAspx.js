function returnTrue() {
    return true;
}

function removeLastChar(dateInput, latestChar) {
    let res;
    const dateInputLength = dateInput.length;

    if (latestChar === "Backspace") return false;

    if (dateInputLength === 6) {
        res = true;
    }
    else if (dateInputLength === 4 && latestChar === "/") {
        res = true;
    }
    else {
        res = false;
    }

    return res;
}

module.exports = { returnTrue, removeLastChar };