const { returnTrue } = require("./LogAspx"); // Import the function

test("call function and ensure it returns true", () => {
    expect(returnTrue()).toBeTruthy();
});
