document.addEventListener("DOMContentLoaded", function () {
    document.getElementById('sideBarHome').classList.add('active');
});

// TOTAL FIELD
var totalrRate = document.getElementById('totalRate');

// PRINT DASBOARD
function printReport() {
    $.ajax({
        url: "/Admin/PrintDasboard",
        dataType: "html",
        success: function (data) {
            // Convert the HTML string into a DOM object
            const parser = new DOMParser();
            const doc = parser.parseFromString(data, "text/html");

            // Choose the element that your content will be rendered to.
            const element = doc.getElementById("report");

            const formattedDate = getCurrentDate();
            var opt = {
                margin: [10, 0, 2, 0],
                filename: 'report' + formattedDate + '.pdf',
                pagebreak: { mode: 'avoid-all'}
            };

            // Choose the element and save the PDF for your user.
            html2pdf().set(opt).from(element).save();
        }
    });
}

function getCurrentDate() {
    const currentDate = new Date();
    const year = currentDate.getFullYear();
    const month = String(currentDate.getMonth() + 1).padStart(2, '0'); // Month is zero-indexed
    const day = String(currentDate.getDate()).padStart(2, '0');
    return `${year}${month}${day}`;
}

