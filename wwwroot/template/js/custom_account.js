//Validate password register
function validatePassword() {
    var regex = /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*]).{3,20}$/;
    var newValue = event.target.value;
    var passwordField = document.getElementById('registerPassword');

    if (!regex.test(newValue)) {
        if (!passwordField.classList.contains('is-invalid')) {
            passwordField.classList.add('is-invalid');
        }
    } else {
        passwordField.classList.remove('is-invalid');
    }
}

//Validate username register
function validateUsername() {
    var inputField = document.getElementById('registerUsername');
    var inputValue = inputField.value.trim();

    if (inputValue !== "") {
        $.ajax({
            url: "/Account/CheckUsername",
            type: "GET",
            data: {
                username: inputValue
            },
            success: function (response) {
                if (response) {
                    if (response.isExist == true) {
                        inputField.classList.add('is-invalid');
                        alert(isExist);
                    } else {
                        inputField.classList.remove('is-invalid');
                        
                    }
                } else {
                    inputField.classList.remove('is-invalid');
                }
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        });
    } else {
        console.log("Input field is empty");
    }
}

// Close success full register notification after 5s
setTimeout(function () {
    var successNotification = document.getElementById("successNotification");
    if (successNotification) {
        successNotification.style.display = "none";
    }
}, 5000);