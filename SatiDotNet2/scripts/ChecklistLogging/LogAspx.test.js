const { returnTrue, haultDateInput } = require("./LogAspx"); // Import the function

test("call function and ensure it returns true", () => {
    expect(returnTrue()).toBeTruthy();
});

test("call haultDateInput with empty string as arg, and it should return false", () => {
    expect(haultDateInput("", "1")).toBeFalsy();
});

test("5 characters exist, adding another", () => {
    expect(haultDateInput("08/24", "3")).toBeTruthy();
});

test("3 characters exist, 3rd character is '/'. Adding '/' character", () => {
    expect(haultDateInput("08/", "/")).toBeTruthy();
});

test("5 characters exist, deleting one", () => {
    expect(haultDateInput("08/24", "Backspace")).toBeFalsy();
});

test("3 characters exist, 3rd character is '/'. Backspacing", () => {
    expect(haultDateInput("08/", "Backspace")).toBeFalsy();
});

test("typing in 2nd character", () => {
    expect(haultDateInput("0", "4")).toBeFalsy();
});

test("typing in last character", () => {
    expect(haultDateInput("05/2", "4")).toBeFalsy();
});
