const form = document.getElementById("submissionForm");

form.addEventListener("submit", async function (event) {

    event.preventDefault();

    const submitButton = document.getElementById("submitButton");
    const statusMessage = document.getElementById("statusMessage");

    const data = {
        name: document.getElementById("name").value,
        email: document.getElementById("email").value,
        phone: document.getElementById("phone").value,
        message: document.getElementById("message").value
    };

    submitButton.disabled = true;
    submitButton.textContent = "Submitting...";

    try {

        const response = await fetch("/Home/Submit", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        });

        const result = await response.json();

        console.log("Server:", result);

        statusMessage.textContent = result.message;

        if (result.success) {
            form.reset();
        }

    } catch (error) {

        console.error(error);

        statusMessage.textContent =
            "Error: " + error.message;
    }

    submitButton.disabled = false;
    submitButton.textContent = "Submit";
});