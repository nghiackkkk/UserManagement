$(document).ready(function () {
    var url = window.location.href;
    var urlWithoutQuery = url.split('?')[0];
    var urlSegments = urlWithoutQuery.split("/");

    var lastTwoSegments = urlSegments.slice(-2).join('/');

    if (lastTwoSegments == "User/TimeKeeping") {
        $('#u-checkio').toggleClass('active');
    } else if (lastTwoSegments == "User/Home") {
        $('#u-profile').toggleClass('active');
    } else if (urlWithoutQuery.indexOf("/User/Profile/")) {
        $('#u-profile-2').toggleClass('active');
        var lastQ = urlWithoutQuery.split('/').slice(-1)[0];
        switch (lastQ) {
            case 'Account':
                $('#up-ac').addClass('active');
                break;
            case 'Personal-Information':
                $('#up-pi').addClass('active');
                break;
            case 'Family-Members':
                $('#up-fm').addClass('active');
                break;
            case 'Study-Process':
                $('#up-sp').addClass('active');
                break;
            case 'Working-Process':
                $('#up-wp').addClass('active');
                break;
            case 'Preview':
                $('#up-pv').addClass('active');
                break;
            default:
                break;
        }
    }
});
function printCard() {
    $.ajax({
        url: "/User/PrintCard",
        type: "GET",
        traditional: true,
        dataType: "html",
        success: function (data) {
            // Convert the HTML string into a DOM object
            const parser = new DOMParser();
            const doc = parser.parseFromString(data, "text/html");

            // Choose the elements that your content will be rendered to.
            const elements = doc.getElementsByClassName('id-card-id');
            const zip = new JSZip();

            var promises = [];
            var opt = {
                margin: [0, 0, 0, 0],
                filename: 'ID_Cards.pdf',
                jsPDF: { unit: 'cm', format: 'letter', orientation: 'l' }
            };

            for (let i = 0; i < elements.length; i++) {
                const element = elements[i];
                const filename = `ID_Card.pdf`;
                const pdfPromise = html2pdf()
                    .set(opt)
                    .from(element)
                    .save();
            }
        }
    });
}
function printProfile() {
    $.ajax({
        url: "/User/PrintProfile",
        type: "GET",
        traditional: true,
        dataType: "html",
        success: function (data) {
            // Convert the HTML string into a DOM object
            const parser = new DOMParser();
            const doc = parser.parseFromString(data, "text/html");

            // Choose the elements that your content will be rendered to.
            const elements = doc.getElementsByClassName('profile-id');
            var opt = {
                margin: [0, 0, 1, 0],
                filename: 'Profile.pdf',
                jsPDF: { unit: 'cm', format: 'A4', orientation: 'p' }
            };

            for (let i = 0; i < elements.length; i++) {
                const element = elements[i];
                const filename = `Profile.pdf`;
                const pdfPromise = html2pdf()
                    .set(opt)
                    .from(element)
                    .save();
            }
        }
    });
}

if ($('#p-account').length > 0) {
    function showHide() {
        var input = $('#inputPassword');
        if (input.attr('type') === 'password') {
            input.prop('type', 'text');
        } else {
            input.attr('type', 'password');
        }
    }

    document.getElementById('avatar').addEventListener('click', function () {
        document.getElementById('inp-avatar').click();
    });

    document.getElementById('inp-avatar').addEventListener('change', function () {
        previewImg();
    });

    function previewImg() {
        const imgInp = document.getElementById('inp-avatar');
        const file = imgInp.files[0]; 

        if (file) {
            const previewImage = document.getElementById('img-show');
            if (previewImage) {
                URL.revokeObjectURL(previewImage.src); 
                previewImage.src = URL.createObjectURL(file);
            } else {
                var img = new Image();
                img.src = URL.createObjectURL(file);
                img.id = "img-show";
                document.getElementById('photo').appendChild(img);
            }
        } else {
            
        }
    }

}

if ($('#previewDiv').length > 0) {
    function renderUrlInDiv(url, targetDivId) {
        $.get(url, function (data) {
            $('#' + targetDivId).html(data);
        });
    }

    var urlToRender = 'http://localhost:5093/Admin/PrintProfile?ids=30';
    var divIdToRenderIn = 'previewDiv'; 
    renderUrlInDiv(urlToRender, divIdToRenderIn);
}
