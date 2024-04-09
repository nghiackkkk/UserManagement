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
function printProfile(){
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