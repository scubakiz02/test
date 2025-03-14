const { returnTrue, haultDateInput } = require("./LogAspx"); // Import the function

test("call function and ensure it returns true", () => {
    expect(returnTrue()).toBeTruthy();
});

test("call haultDateInput with empty string as arg, and it should return false", () => {
    expect(haultDateInput("", "1")).toBeFalsy();
});

test("call haultDateInput with 5 char string as arg, and it should be haulted (return true)", () => {
    expect(haultDateInput("08/24", "3")).toBeTruthy();
});

test("call haultDateInput with 3 char string that includes '/' char as arg. It should return true", () => {
    expect(haultDateInput("08/", "/")).toBeTruthy();
});
