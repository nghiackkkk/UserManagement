function deleteButton(buttonId) {
    let h3 = notification.querySelector('h3');
    h3.textContent = "Are you sure to delete user " + buttonId + "?";
    notification.classList.remove('hidden');
    contentMain.classList.add('blur');
}

function yesDelete() {
    let h3 = notification.querySelector('h3').textContent.split(' ');
    let userId = h3[h3.length - 1].replace('?', '');
    userId = parseInt(userId);

    $.ajax({
        url: 'DeleteUserDo',
        type: 'GET',
        data: { id: userId },
        dataType: "json",
        success: function (result) {
            if (result.success) {
                console.log(true);
                window.location.href = "/Home/Index";
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });

}

function handleLock(userId) {
    $.ajax({
        url: 'UpdateStatus',
        type: 'GET',
        data: { id: userId },
        dataType: "json",
        success: function (response) {
            if (response.success == true) {
                console.log(true);
                window.location.href = '/Home/Index';
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}
