const { returnTrue, dateOutput } = require("./LogAspx"); // Import the function

test("call function and ensure it returns true", () => {
    expect(returnTrue()).toBeTruthy();
});

test("typing in first character", () => {
    expect(dateOutput("1", "1")).toBe("1");
});

test("5 characters exist, adding another", () => {
    expect(dateOutput("08/243", "3")).toBe("08/24");
});

test("4 characters exist, 4th character is '/'. Adding '/' character", () => {
    expect(dateOutput("08//", "08//")).toBe("08/");
});

test("4 characters exist, 4th character is '/'. On tablets, '/' = 'Unidentified'", () => {
    expect(dateOutput("08//", "Unidentified")).toBe("08/");
});

test("5 characters exist, deleting one", () => {
    expect(dateOutput("08/2", "Backspace")).toBe("08/2");
});

test("3 characters exist, 3rd character is '/'. Backspacing", () => {
    expect(dateOutput("08", "Backspace")).toBe("08");
});

test("5 characters exist, Backspacing", () => {
    expect(dateOutput("08/2", "Backspace")).toBe("08/2");
});

test("typing in 2nd character", () => {
    expect(dateOutput("04", "4")).toBe("04/");
});

test("typing in last character", () => {
    expect(dateOutput("05/24", "4")).toBe("05/24");
});

test("typing in characters faster than dateOutput can be called", () => {
    expect(dateOutput("05/24234234", "4")).toBe("05/24");
});

test("user has typed in first 2 digits for month, add '/' programmatically", () => {
    expect(dateOutput("05", "5")).toBe("05/");
});
