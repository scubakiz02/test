function returnTrue() {
    return true;
}

function dateOutput(dateInput, e) {
    let res = "";
    const dateInputLength = dateInput.length;

    if (dateInputLength === 4 && (e.key === '/' || e.key === 'Unidentified')) {
        res = dateInput.slice(0, -1); 
    }
    else if (dateInputLength >= 6){
        //remove all characters past 5 count
        let charsToRemove = dateInput.length > 5 ? - (dateInput.length - 5) : -1;
        res = dateInput.slice(0, charsToRemove); 
    }
    else if ((dateInputLength === 2 || dateInputLength === 3) && e.key !== "Backspace") {
        res = dateInput.slice(0, 2) + "/" + dateInput.slice(2); //add '/' char at index 2 when it is needed
    }
    else {
        res = dateInput;
    }

    return res;
}

module.exports = { returnTrue, dateOutput };