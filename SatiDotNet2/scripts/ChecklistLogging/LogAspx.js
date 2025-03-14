function returnTrue() {
    return true;
}

function dateOutput(dateInput, latestChar) {
    let res = "";
    const dateInputLength = dateInput.length;

    if (dateInputLength >= 6 || (dateInputLength === 4 && (latestChar.includes("/") || latestChar == "Unidentified"))){
        //remove all characters past 5 count
        let charsToRemove = dateInput.length > 5 ? - (dateInput.length - 5) : -1;
        res = dateInput.slice(0, charsToRemove); 
    }
    else if (dateInputLength === 2 && latestChar !== "Backspace") {
        res = dateInput += "/";
    }
    else {
        res = dateInput;
    }

    return res;
}

module.exports = { returnTrue, dateOutput };