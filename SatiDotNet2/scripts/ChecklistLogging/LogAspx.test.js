const { returnTrue, removeLastChar } = require("./LogAspx"); // Import the function

test("call function and ensure it returns true", () => {
    expect(returnTrue()).toBeTruthy();
});

test("typing in first character", () => {
    expect(removeLastChar("1", "1")).toBe("1");
});

test("5 characters exist, adding another", () => {
    expect(removeLastChar("08/243", "3")).toBeTruthy();
});

test("4 characters exist, 4th character is '/'. Adding '/' character", () => {
    expect(removeLastChar("08//", "/")).toBeTruthy();
});

test("5 characters exist, deleting one", () => {
    expect(removeLastChar("08/24", "Backspace")).toBe("08/2");
});

test("3 characters exist, 3rd character is '/'. Backspacing", () => {
    expect(removeLastChar("08/", "Backspace")).toBe("08");
});

test("typing in 2nd character", () => {
    expect(removeLastChar("04", "4")).toBe("04");
});

test("typing in last character", () => {
    expect(removeLastChar("05/24", "4")).toBe("05/24");
});

test("typing in characters faster than removeLastChar can be called", () => {
    expect(removeLastChar("05/24234234", "4")).toBeTruthy();
});
