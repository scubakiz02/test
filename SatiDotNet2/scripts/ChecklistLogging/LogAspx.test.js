const LogAspx = require("./LogAspx"); // Import the function

test("call function and ensure it returns true", () => {
    expect(LogAspx.returnTrue()).toBeTruthy();
});
