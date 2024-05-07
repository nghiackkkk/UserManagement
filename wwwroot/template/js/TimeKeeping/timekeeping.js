document.addEventListener("DOMContentLoaded", function () {
    var url = window.location.href.split("/");
    var lastSegment = url[url.length - 1];

    if ($('#sideBarTK').length > 0) {
        document.getElementById("sideBarTK").classList.toggle("active");
    }

    if (url.includes("TimeCheck")) {
        document.getElementById("tk-timecheck").classList.toggle("active");
    }
    if (url.includes("AssignExtraShift")) {
        document.getElementById("tk-aes").classList.toggle("active");
    }
    if (url.includes("AssignDepartment")) {
        document.getElementById("tk-ad").classList.toggle("active");
    }
    if (url.includes("ExtraShift")) {
        document.getElementById("tk-extrashift").classList.toggle("active");
    }
    if (url.includes("WorkingDay")) {
        document.getElementById("tk-wd").classList.toggle("active");
    }
    if (url.includes("RequestForLeave")) {
        document.getElementById("tk-rfl").classList.toggle("active");
    }

    if ($('#body-qrio').length > 0) {
        var qrcode = new QRCode("qr-code", {
            text: "http://localhost:5093/User/CheckIO",
            width: 500,
            height: 500,
            colorDark: "#000000",
            colorLight: "#ffffff",
            correctLevel: QRCode.CorrectLevel.H
        });
    }
});

if (document.getElementById('live-time') != null) {
    // LIVE TIME
    var span = document.getElementById('live-time');

    function time() {
        var d = new Date();
        var s = d.getSeconds();
        var m = d.getMinutes();
        var h = d.getHours();
        span.textContent =
            ("0" + h).substr(-2) + ":" + ("0" + m).substr(-2) + ":" + ("0" + s).substr(-2);
    }

    setInterval(time, 1000);

    // LIVE DATE
    function formatDate(date) {
        const day = date.toLocaleDateString('en-US', { weekday: 'long' });
        const year = date.getFullYear();
        const month = date.toLocaleDateString('en-US', { month: 'long' });
        const dateStr = date.getDate();

        return `${day}, ${dateStr} ${month} ${year}`;
    }

    const today = new Date();
    const formattedDate = formatDate(today);
    const liveDateSpan = document.getElementById('live-date');
    liveDateSpan.textContent = formattedDate;
}

if (document.getElementById('body-rfl') != null) {
    
}


