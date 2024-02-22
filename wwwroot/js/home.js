var notification = document.getElementById('notification');
var contentMain = document.getElementById('content-main-main');
var notiButtonNO = document.getElementById('btn-noti-no');

notiButtonNO.addEventListener('click', function () {
    contentMain.classList.remove('blur');
    notification.classList.add('hidden');
})

var loginUrl = window.location.href.toString().split("/");
var haveLogin = loginUrl[loginUrl.length - 1];
if (haveLogin === "Login") {
    document.querySelector("header").classList.add("hidden");
}
console.log(haveLogin);