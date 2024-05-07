function printAllCard() {
    $.ajax({
        url: "/Admin/PrintAllCard",
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
                filename: 'ID_Cards.zip', // Change filename to zip file
                jsPDF: { unit: 'cm', format: 'letter', orientation: 'l' }
            };

            for (let i = 0; i < elements.length; i++) {
                // Choose the element and save the PDF for your user.
                const element = elements[i];
                const filename = `ID_Card_${i + 1}.pdf`;

                // Generate PDF and add to zip
                const pdfPromise = html2pdf()
                    .set(opt)
                    .from(element)
                    .outputPdf()
                    .then(function (pdf) {
                        zip.file(filename, pdf, { binary: true });
                    });

                promises.push(pdfPromise);
            }

            // Wait for all promises to resolve
            Promise.all(promises).then(function () {
                // Generate zip file
                zip.generateAsync({ type: "blob" })
                    .then(function (content) {
                        // Offer the zip file for download
                        saveAs(content, "ID_Cards.zip");
                    });
            });
        }
    });
}

function printAllProfile() {
    $.ajax({
        url: "/Admin/PrintAllProfile",
        type: "GET",
        traditional: true,
        dataType: "html",
        success: function (data) {
            // Convert the HTML string into a DOM object
            const parser = new DOMParser();
            const doc = parser.parseFromString(data, "text/html");

            // Choose the elements that your content will be rendered to.
            const elements = doc.getElementsByClassName('profile-id');
            const idss = doc.getElementsByClassName('id-profile');
            const zip = new JSZip();

            var promises = [];
            var opt = {
                margin: [0, 0, 1, 0],
                filename: 'Profiles.zip', // Change filename to zip file
                jsPDF: { unit: 'cm', format: 'A4', orientation: 'p' }
            };

            for (let i = 0; i < elements.length; i++) {
                // Choose the element and save the PDF for your user.
                const element = elements[i];

                const idUser = idss[i].value;
                const filename = `Profile_${idUser}.pdf`;

                // Generate PDF and add to zip
                const pdfPromise = html2pdf()
                    .set(opt)
                    .from(element)
                    .outputPdf()
                    .then(function (pdf) {
                        zip.file(filename, pdf, { binary: true });
                    });

                promises.push(pdfPromise);
            }

            // Wait for all promises to resolve
            Promise.all(promises).then(function () {
                // Generate zip file
                zip.generateAsync({ type: "blob" })
                    .then(function (content) {
                        // Offer the zip file for download
                        saveAs(content, "Profiles.zip");
                    });
            });
        }
    });
}

function exportExcel() {
    window.location.href = "/Admin/ExportExcel";
}

function printQR() {
    $.ajax({
        url: '/Admin/TimeKeeping/PrintQRCheckIO',
        type: "GET",
        traditional: true,
        dataType: "html",
        success: function (data) {
            const parser = new DOMParser();
            const doc = parser.parseFromString(data, "text/html");

            var qrcode = new QRCode(doc.getElementById('qr-code'), {
                text: "http://localhost:5093/User/CheckIO",
                width: 700,
                height: 700,
                colorDark: "#000000",
                colorLight: "#ffffff",
                correctLevel: QRCode.CorrectLevel.H
            });
            // Choose the elements that your content will be rendered to.
            const elements = doc.getElementById('printable');

            var opt = {
                margin: [-2, 0, 3, 0],
                filename: 'QR_CheckIO.pdf',
                jsPDF: { unit: 'in', format: 'a4', orientation: 'p' }
            }

            html2pdf()
                .set(opt)
                .from(elements)
                .save();
        },
        error(xhr, status, error) {
            console.log(error);
        }
    });
}