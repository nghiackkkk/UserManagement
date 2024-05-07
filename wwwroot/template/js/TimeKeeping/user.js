document.addEventListener('DOMContentLoaded', function () {

    var qrcode = new QRCode("qrcode-i", {
        text: "http://localhost:5093/User/CheckIO",
        width: 300,
        height: 300,
        colorDark: "#ffffff",
        colorLight: "#3B7DDD",
        correctLevel: QRCode.CorrectLevel.H
    });

});

if (document.getElementById('body-checkio') != null) {

}

if (document.getElementById('body-usertk') != null) {
    const exampleModal = document.getElementById('user-submitted-modal')
    if (exampleModal) {
        exampleModal.addEventListener('show.bs.modal', event => {
            const button = event.relatedTarget;
            const recipient = button.getAttribute('data-bs-reason');
            const accepted = button.getAttribute('data-bs-accepted');
            const day = button.getAttribute('data-bs-day');
            const timein = button.getAttribute('data-bs-timein');
            const timeout = button.getAttribute('data-bs-timeout');

            const modalBodyInput = exampleModal.querySelector('#reason-submitted');
            const modalAccepted = exampleModal.querySelector('#accepted-submitted');
            const modalDay = exampleModal.querySelector('#day-submitted');
            const modalTimein = exampleModal.querySelector('#timein-submitted');
            const modalTimeout = exampleModal.querySelector('#timeout-submitted');

            modalAccepted.innerText = accepted;
            modalBodyInput.innerText = recipient;
            modalDay.innerText = day;
            modalTimein.innerText = timein;
            modalTimeout.innerText = timeout;
        });
    }

    const exampleModal1 = document.getElementById('user-reason-modal')
    if (exampleModal1) {
        exampleModal1.addEventListener('show.bs.modal', event => {
            const button = event.relatedTarget;
            //const recipient = button.getAttribute('data-bs-reason');
            //const accepted = button.getAttribute('data-bs-accepted');
            const day = button.getAttribute('data-bs-day');
            const timein = button.getAttribute('data-bs-timein');
            const timeout = button.getAttribute('data-bs-timeout');

            //const modalBodyInput = exampleModal.querySelector('#reason-submitted');
            //const modalAccepted = exampleModal.querySelector('#accepted-submitted');
            const modalDay = exampleModal1.querySelector('#day-reason');
            const modalTimein = exampleModal1.querySelector('#timein-reason');
            const modalTimeout = exampleModal1.querySelector('#timeout-reason');
            const inpDay = exampleModal1.querySelector('#inp-day');
            const inpTimein = exampleModal1.querySelector('#inp-timein');
            const inpTimeout = exampleModal1.querySelector('#inp-timeout');

            //modalAccepted.innerText = accepted;
            //modalBodyInput.innerText = recipient;
            modalDay.innerText = day;
            modalTimein.innerText = timein;
            modalTimeout.innerText = timeout;
            inpDay.value = day;
            inpTimein.value = timein;
            inpTimeout.value = timeout;
        });
    }

    const exampleModal2 = document.getElementById('user-absence-modal');
    if (exampleModal2) {
        var tableabs = new DataTable("#absence-table", {
            columns: [
                { width: '30px', className: 'text-start' },
                { width: '100px', className: 'text-start' },
                { className: 'text-start' },
                null,
                { width: '100px', orderable: false },
            ],
            display: 'hover',
            stripeClasses: [],
            rowReorder: {
                selector: 'td:nth-child(2)'
            },
            drawCallback: function (settings) {
                feather.replace();
            }
        });

        function fetchDataAndRedrawTable() {
            $.ajax({
                url: '/User/API/GetDataABS',
                type: "GET",
                success: function (response) {
                    if (response) {
                        var datasabs = response.data;
                        tableabs.clear().draw();
                        datasabs.forEach((data, index) => {
                            tableabs.row.add([
                                index + 1,
                                data.day_from,
                                data.day_to,
                                data.reason,
                                data.accepted
                            ]).draw();
                        });
                        feather.replace();
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        }

        // Listen for modal show event
        exampleModal2.addEventListener('show.bs.modal', event => {
            if (document.getElementById('absence-table') != null) {
                fetchDataAndRedrawTable();
            }
        });

    }

}
