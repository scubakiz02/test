function passInsertImage() {
    parent.insertImage();
}

function passCheckNums(input, reset) {
    parent.checkNums(input);

    if (parent.getNumChecks() == true) {
        if (input.value.length > 3) {
            document.getElementById('NinjaListUpdater').click();
        } 
    }
}