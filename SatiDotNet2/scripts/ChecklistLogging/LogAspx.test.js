const { returnTrue, dateOutput } = require("./LogAspx"); // Import the function
let e = {
    "key": 0,
    "target": {
        "value": 0
    }
}

test("call function and ensure it returns true", () => {
    expect(returnTrue()).toBeTruthy();
});

describe('standard cases. output from function is as user typed', () => {
    test("typing in first character", () => {
        e.key = "1";
        e.target.value = "1";
        expect(dateOutput("1", e)).toBe("1");
        // expect(dateOutput("1", "1")).toBe("1");
    });

    test("typing in last character", () => {
        expect(dateOutput("05/24", "4")).toBe("05/24");
    });
});

describe("typing in more than 5 characters", () => {
    test("5 characters exist, adding another", () => {
        e.key = "3";
        e.target.value = "08/243";
        expect(dateOutput("08/243", e)).toBe("08/24");
    });
    
    test("typing in characters faster than dateOutput can be called", () => {
        e.key = "4";
        e.target.value = "05/24234234";
        expect(dateOutput("05/24234234", e)).toBe("05/24");
    });
});

describe("manual '/' character entry after programmatic '/' entry", () => {
    test("3 characters exist. Adding '/' character", () => {
        e.key = "/";
        e.target.value = "08//";
        expect(dateOutput("08//", e)).toBe("08/");
    });
    
    test("3 characters exist, adding '/'. On tablets, '/' can be 'Unidentified'", () => {
        e.key = "Unidentified";
        e.target.value = "08//";
        expect(dateOutput("08//", e)).toBe("08/");
    });
})

describe("backspacing", () => {
    test("5 characters exist, deleting one", () => {
        e.key = "Backspace";
        e.target.value = "08/2";
        expect(dateOutput("08/2", e)).toBe("08/2");
    });
    
    test("3 characters exist, 3rd character is '/'. Backspacing", () => {
        e.key = "Backspace";
        e.target.value = "08";
        expect(dateOutput("08", e)).toBe("08");
    });
    
    test("5 characters exist, Backspacing", () => {
        e.key = "Backspace";
        e.target.value = "08/2";
        expect(dateOutput("08/2", e)).toBe("08/2");
    });
})

describe("add '/' programmatically", () => {
    test("user has typed in first 2 digits for month, add '/' programmatically", () => {
        e.key = "5";
        e.target.value = "05";
        expect(dateOutput("05", e)).toBe("05/");
    });
    
    test("typing in 2nd character", () => {
        e.key = "4";
        e.target.value = "04";
        expect(dateOutput("04", e)).toBe("04/");
    });
    
    test("typing in character, but '/' is needed before", () => {
        e.key = "4";
        e.target.value = "040";
        expect(dateOutput("040", e)).toBe("04/0");
    });
})
