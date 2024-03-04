var notification = document.getElementById('notification');
var contentMain = document.getElementById('content-main-main');
var notiButtonNO = document.getElementById('btn-noti-no');

// Event close notification pop up
notiButtonNO.addEventListener('click', function () {
    contentMain.classList.remove('blur');
    notification.classList.add('hidden');
})

document.addEventListener("keyup", (event) => {
    if (event.keyCode === 27) {
        contentMain.classList.remove('blur');
        notification.classList.add('hidden');
    }
});

// Check url to hide nav bar for login page
var loginUrl = window.location.href.toString().split("/");
var lastUrl = loginUrl[loginUrl.length - 1];
var haveHome = loginUrl.indexOf('Home');
if (lastUrl === "Login" || haveHome == -1 || lastUrl === "CheckLogin") {
    document.querySelector("header").classList.add("hidden");
    document.getElementById("left-nav").classList.add("hidden");
}

// Check active left navbar
if (lastUrl === "Index") {
    document.getElementById("left-user").classList.add("icon-active");
    document.getElementById("left-trash").classList.remove("icon-active");
} else if (lastUrl == "ShowTrash") {
    document.getElementById("left-trash").classList.add("icon-active");
    document.getElementById("left-user").classList.remove("icon-active");
}

document.addEventListener('onload', (event) => {
    var userErrorField = document.getElementById('username-error');
    if (userErrorField.innerText != '') {
        document.getElementById('username').focus();
    }
})

// Check move to trash or delete
if (lastUrl === 'Index') {
    document.getElementById('btn-noti-yes').classList.add('hidden');
    document.getElementById('btn-move-trash').classList.remove('hidden');
} else if (lastUrl === 'ShowTrash') {
    document.getElementById('btn-noti-yes').classList.remove('hidden');
    document.getElementById('btn-move-trash').classList.add('hidden')
}


// Color row checked
var checkboxes = document.querySelectorAll('input[type="checkbox"]');
checkboxes.forEach(function (checkbox) {
    checkbox.addEventListener('change', function () {
        if (checkbox.checked) {
            checkbox.parentNode.parentNode.classList.add('checked-color');
        } else {
            checkbox.parentNode.parentNode.classList.remove('checked-color');
        }
    });
});

