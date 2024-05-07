if (document.getElementById("edit-reschedule-modal") != null) {
    const exampleModal = document.getElementById('edit-reschedule-modal')
    if (exampleModal) {
        exampleModal.addEventListener('show.bs.modal', event => {
            // Button that triggered the modal
            const button = event.relatedTarget
            // Extract info from data-bs-* attributes
            const bsId = button.getAttribute('data-bs-id');
            const bsName = button.getAttribute('data-bs-name');
            const bsDep = button.getAttribute('data-bs-department');
            const bsPos = button.getAttribute('data-bs-position');

            // Update the modal's content.
            const idSpan = document.getElementById('idUser');
            const nameSpan = document.getElementById('fullname');
            const departmentSel = document.getElementById('department-select');
            const positionSel = document.getElementById('position-select');
            const idInp = document.getElementById('inputId');

            idSpan.innerText = bsId;
            idInp.value = bsId;
            nameSpan.innerText = bsName;
            for (let i = 0; i < departmentSel.options.length; i++) {
                if (departmentSel.options[i].value === bsDep) {
                    departmentSel.options[i].selected = true;
                    break;
                }
            }
            for (let i = 0; i < positionSel.options.length; i++) {
                if (positionSel.options[i].value === bsPos) {
                    positionSel.options[i].selected = true;
                    break;
                }
            }
        })
    }
}

if (document.getElementById("timecheck-modal") != null) {
    const exampleModal = document.getElementById('timecheck-modal');
    var bsIdAc;
    if (exampleModal) {
        exampleModal.addEventListener('show.bs.modal', event => {
            // Button that triggered the modal
            var button = event.relatedTarget

            // Extract info from data-bs-* attributes
            var bsReason = button.getAttribute('data-bs-reason');
            var bsAccepted = button.getAttribute('data-bs-accepted');
            bsIdAC = button.getAttribute('data-bs-idAC');

            // Update the modal's content.
            const idSpan = document.getElementById('reason');
            const idAcp = document.getElementById('accept');

            idAcp.innerText = bsAccepted;
            idSpan.innerText = bsReason;

            var twoButton = '';
            if (bsAccepted == "False") {
                $('#modal-footer .btn-primary').remove(); // Remove Accept button
                $('#modal-footer .btn-danger').remove(); // Remove Reject button
                twoButton = `<button id="btn-acp" onclick="requestLate('Accepted')" type="button" class="btn btn-primary">Accept</button>
                         <button id="btn-rej" onclick="requestLate('Reject')" type="button" class="btn btn-danger">Reject</button>`;
            } else {
                // If bsAccepted is True, remove or disable the buttons
                $('#modal-footer .btn-primary').remove(); // Remove Accept button
                $('#modal-footer .btn-danger').remove(); // Remove Reject button
            }

            // Insert buttons before the Close button
            $('#modal-footer .btn-secondary').before(twoButton);
        });
    }

    function requestLate(status) {
        var idAC = bsIdAC;

        var model = {
            status: status,
            id_absence: idAC
        };
        // ajax
        $.ajax({
            url: '/Admin/TimeKeeping/API/RequestLate',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(model),
            success: function (response) {
                // Xử lý phản hồi thành công từ server
                window.location.reload();
            },
            error: function (xhr, status, error) {
                // Xử lý lỗi
                console.log(error);
            }
        });
    }

}
