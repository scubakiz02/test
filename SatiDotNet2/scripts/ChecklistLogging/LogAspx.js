function returnTrue() {
    return true;
}

function removeLastChar(dateInput, latestChar) {
    let res = "";
    const dateInputLength = dateInput.length;

    if (latestChar === "Backspace"){
        //remove all characters past 5 count
        let charsToRemove = dateInput.length > 5 ? - (dateInput.length - 5) : -1;
        res = dateInput.slice(0, charsToRemove); 
    }
    else if (dateInputLength >= 6) {
        res = true;
    }
    else if (dateInputLength === 4 && latestChar === "/") {
        res = true;
    }
    else {
        res = dateInput;
    }

    return res;
}

module.exports = { returnTrue, removeLastChar };