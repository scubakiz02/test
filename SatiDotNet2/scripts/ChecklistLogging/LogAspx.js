function returnTrue() {
    return true;
}

function removeLastChar(dateInput, latestChar) {
    let res = "";
    const dateInputLength = dateInput.length;

    if (latestChar === "Backspace" || dateInputLength >= 6 || (dateInputLength === 4 && latestChar === "/")){
        //remove all characters past 5 count
        let charsToRemove = dateInput.length > 5 ? - (dateInput.length - 5) : -1;
        res = dateInput.slice(0, charsToRemove); 
    }
    else {
        res = dateInput;
    }

    return res;
}

module.exports = { returnTrue, removeLastChar };