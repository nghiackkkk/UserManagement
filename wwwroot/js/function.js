function showLoading(type) {
    if (type == 'on') {
        document.getElementById('content-main-main').classList.add('blur');
        document.getElementById('notification-2').classList.remove('hidden');
    } else {
        document.getElementById('content-main-main').classList.remove('blur');
        document.getElementById('notification-2').classList.add('hidden');
    }
}
function deleteButton(username, id) {
    let h3 = notification.querySelector('h3');
    h3.textContent = "Are you sure to delete " + username + "?";

    let idH1 = document.createElement('h1');
    idH1.classList.add('hidden');
    idH1.innerText = id;

    notification.appendChild(idH1);
    notification.classList.remove('hidden');
    contentMain.classList.add('blur');
}

function deleteButton2(username, id) {
    let h3 = notification.querySelector('h3');
    h3.textContent = "Move to trash " + username + "?";

    let idH1 = document.createElement('h1');
    idH1.classList.add('hidden');
    idH1.innerText = id;

    notification.appendChild(idH1);
    notification.classList.remove('hidden');
    contentMain.classList.add('blur');
}

function yesDelete() {
    let h1 = notification.querySelector('h1').textContent;
    var userId = parseInt(h1);

    $.ajax({
        url: 'DoDeleteUser',
        type: 'GET',
        data: { id: userId },
        dataType: "json",
        success: function (result) {
            if (result.success) {
                console.log(true);
                window.location.href = "/Home/ShowTrash";
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });

}

function yesMoveToTrash() {
    let h1 = notification.querySelector('h1').textContent;
    var userId = parseInt(h1);

    $.ajax({
        url: "MoveToTrash",
        type: "GET",
        data: {
            ids: userId
        },
        dataType: 'json',
        traditional: true,
        success: function (response) {
            window.location.href = '/Home/Index';
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function handleLock(userId) {
    event.stopPropagation();
    $.ajax({
        url: 'UpdateStatus',
        type: 'GET',
        data: { id: userId },
        dataType: "json",
        success: function (response) {
            if (response.success) {
                //document.getElementsByTagName('tbody')[0].classList.add('hidden');
                var tbodyFirst = document.getElementsByTagName('tbody');
                console.log(tbodyFirst);

                for (var i = 0; i < tbodyFirst.length; i++) {
                    tbodyFirst[i].remove();
                }

                var table = document.getElementsByTagName('table')[0];

                var tbody = document.createElement('tbody');
                var users = response.data;
                tbody.innerHTML = ``;
                users.forEach(function (user) {
                    var btnStatus = ``
                    if (user.status == "Open") {
                        btnStatus = `<td class="prd-edit tall alg-center"><button id="btn-unlock" onclick="handleLock(` + user.id + `)" class="" style="background-color: #27ae60"><i class="fa-solid fa-unlock"></i></button></td>`
                    } else {
                        btnStatus += `<td class="prd-edit tall alg-center"><button id="btn-lock" onclick="handleLock(` + user.id + `)" style="background-color: #f39c12"><i class="fa-solid fa-lock"></i></button></td>`
                    }
                    var userImage = ``;

                    if (user.image != null) {
                        userImage = `<img width="50" height="50" src="/uploads/` + user.image + `" />`;
                    } else {
                        userImage = `<img width="50" height="50" src="/img/meobongbong.jpg" />`;
                    }
                    tbody.innerHTML +=
                        `<tr>
                            <td id="td-checkbox" class="alg-center">
                                <input id="checkbox-child-`+ user.id + `" type="checkbox" />
                            </td>
                            <td onclick="handleToEdit(`+ user.id + `)">
                               `+ userImage + `
                            </td>
                            <td onclick="handleToEdit(`+ user.id + `)" class="prd-text-left">` + user.name + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.age + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.gender + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.number + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.username + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.password + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.priority + `</td>
                            `+ btnStatus + `
                            <td class="prd-edit tall alg-center">
                                <button onclick="deleteButton('`+ user.username + `')" id="del-` + user.id + `" href="" style="background-color: red"><i class="fa fa-solid fa-trash"></i></button>
                            </td>
                        </tr>`;
                });

                table.appendChild(tbody);
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}

function handleToEdit(userId) {
    window.location.href = "/Home/EditUser/" + userId;
}

function handleCheck(checkbox) {
    var checkboxes = document.querySelectorAll('input[type="checkbox"]');
    checkboxes.forEach(function (childCheckbox) {
        childCheckbox.checked = checkbox.checked;
        if (checkbox.checked) {
            childCheckbox.parentNode.parentNode.classList.add('checked-color');
        } else {
            childCheckbox.parentNode.parentNode.classList.remove('checked-color');
        }

    });
}

function validatePhoneNumber() {
    var newValue = event.target.value;
    var regex = /^\d{10}$/;
    var errorField = document.getElementById('phone-number-error');

    if (!regex.test(newValue)) {
        errorField.innerText = "Phone number only have 10 digit character"
    } else {
        errorField.innerText = '';
    }
}

function validatePassword() {
    var regex = /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*]).{3,20}$/;
    var errorField = document.getElementById('password-error');
    var newValue = event.target.value;

    if (!regex.test(newValue)) {
        errorField.innerText = "Password must include uppercase, lowercase, number, 3-10 letter and !@#$%^&*"
    } else {
        errorField.innerText = '';
    }
}

function getAllChecked() {
    var checkboxes = document.querySelectorAll('input[type="checkbox"]');
    var listChecked = [];

    checkboxes.forEach(function (childCheckbox) {
        if (childCheckbox.checked) {
            listChecked.push(parseInt(childCheckbox.id.split('-')[2]));
        }
    });
    listChecked = listChecked.filter(checked => !isNaN(checked));
    //console.log(listChecked[0]);

    $.ajax({
        url: "ResetPassword",
        type: "GET",
        data: {
            ids: listChecked
        },
        dataType: 'json',
        traditional: true,
        success: function (response) {
            //console.log(response);
            if (response.success1) {
                document.getElementsByTagName('tbody')[0].classList.add('hidden');
                var table = document.getElementsByTagName('table')[0];

                var tbody = document.createElement('tbody');
                var users = response.data;
                tbody.innerHTML = ``;
                users.forEach(function (user) {
                    var btnStatus = ``
                    if (user.status == "Open") {
                        btnStatus = `<td class="prd-edit tall alg-center"><button id="btn-unlock" onclick="handleLock(` + user.id + `)" class="" style="background-color: #27ae60"><i class="fa-solid fa-unlock"></i></button></td>`
                    } else {
                        btnStatus += `<td class="prd-edit tall alg-center"><button id="btn-lock" onclick="handleLock(` + user.id + `)" style="background-color: #f39c12"><i class="fa-solid fa-lock"></i></button></td>`
                    }

                    var userImage = ``;
                    if (user.image != null) {
                        userImage = `<img width="50" height="50" src="/uploads/` + user.image + `" />`;
                    } else {
                        userImage = `<img width="50" height="50" src="/img/meobongbong.jpg" />`;
                    }

                    tbody.innerHTML +=
                        `<tr>
                            <td id="td-checkbox" class="alg-center">
                                <input id="checkbox-child-`+ user.id + `" type="checkbox" />
                            </td>
                            <td onclick="handleToEdit(`+ user.id + `)">
                               `+ userImage + `
                            </td>
                            <td onclick="handleToEdit(`+ user.id + `)" class="prd-text-left">` + user.name + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.age + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.gender + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.number + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.username + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.password + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.priority + `</td>
                            `+ btnStatus + `
                            <td class="prd-edit tall alg-center">
                                <button onclick="deleteButton('`+ user.username + `')" id="del-` + user.id + `" href="" style="background-color: red"><i class="fa fa-solid fa-trash"></i></button>
                            </td>
                        </tr>`;
                });

                table.appendChild(tbody);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

// Search function
function searchUser() {
    var input = document.getElementById('search-input').value;
    if (input !== '') {
        $.ajax({
            url: "Search",
            type: "GET",
            dataType: 'json',
            data: { input: input },
            success: function (response) {
                // Remove all tbody before
                var tbodyFirst = document.getElementsByTagName('tbody');
                console.log(tbodyFirst);

                for (var i = 0; i < tbodyFirst.length; i++) {
                    tbodyFirst[i].remove();
                }
                if (response.success == true) {
                    // Append new tbody
                    var table = document.getElementsByTagName('table')[0];

                    var tbody = document.createElement('tbody');
                    var users = response.data;
                    tbody.innerHTML = ``;

                    // Total search
                    var totalSearch = response.data.length;
                    var divTotal = document.querySelector('#count-item > span');
                    divTotal.innerText = '';
                    divTotal.innerText = 'Total: ' + totalSearch;

                    users.forEach(function (user) {
                        var btnStatus = ``
                        if (user.status == "Open") {
                            btnStatus = `<td class="prd-edit tall alg-center"><button id="btn-unlock" onclick="handleLock(` + user.id + `)" class="" style="background-color: #27ae60"><i class="fa-solid fa-unlock"></i></button></td>`
                        } else {
                            btnStatus += `<td class="prd-edit tall alg-center"><button id="btn-lock" onclick="handleLock(` + user.id + `)" style="background-color: #f39c12"><i class="fa-solid fa-lock"></i></button></td>`
                        }
                        var userImage = ``;

                        if (user.image != null) {
                            userImage = `<img width="50" height="50" src="/uploads/` + user.image + `" />`;
                        } else {
                            userImage = `<img width="50" height="50" src="/img/meobongbong.jpg" />`;
                        }
                        tbody.innerHTML +=
                            `<tr>
                            <td id="td-checkbox" class="alg-center">
                                <input id="checkbox-child-`+ user.id + `" type="checkbox" />
                            </td>
                            <td onclick="handleToEdit(`+ user.id + `)">
   `+ userImage + `
</td>
                            <td onclick="handleToEdit(`+ user.id + `)" class="prd-text-left">` + user.name + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.age + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.gender + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.number + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.username + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.password + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.priority + `</td>
                            `+ btnStatus + `
                            <td class="prd-edit tall alg-center">
                                <button onclick="deleteButton('`+ user.username + `')" id="del-` + user.id + `" href="" style="background-color: red"><i class="fa fa-solid fa-trash"></i></button>
                            </td>
                        </tr>`;
                    });

                    table.appendChild(tbody);
                } else {
                    var table = document.getElementsByTagName('table')[0];

                    var h1 = document.createElement('h1');
                    h1.textContent = 'No result!';
                    table.parentNode.insertBefore(h1, table.nextSibling);
                }
            },
            error: function (xhr, status, error) {
                alert(status);
            }
        })
    } else {
        $.ajax({
            url: "GetAllItem",
            type: "GET",
            data: { "sort": 'none' },
            traditional: true,
            dataType: "json",
            success: function (response) {
                // Remove all tbody before
                var tbodyFirst = document.getElementsByTagName('tbody');
                console.log(tbodyFirst);

                for (var i = 0; i < tbodyFirst.length; i++) {
                    tbodyFirst[i].remove();
                }

                // Append new tbody
                var table = document.getElementsByTagName('table')[0];

                var tbody = document.createElement('tbody');
                var users = response;

                // Total search
                var totalSearch = response.length;
                var divTotal = document.querySelector('#count-item > span');
                divTotal.innerText = '';
                divTotal.innerText = 'Total: ' + totalSearch;

                tbody.innerHTML = ``;
                users.forEach(function (user) {
                    var btnStatus = ``
                    if (user.status == "Open") {
                        btnStatus = `<td class="prd-edit tall alg-center"><button id="btn-unlock" onclick="handleLock(` + user.id + `)" class="" style="background-color: #27ae60"><i class="fa-solid fa-unlock"></i></button></td>`
                    } else {
                        btnStatus += `<td class="prd-edit tall alg-center"><button id="btn-lock" onclick="handleLock(` + user.id + `)" style="background-color: #f39c12"><i class="fa-solid fa-lock"></i></button></td>`
                    }
                    var userImage = ``;

                    if (user.image != null) {
                        userImage = `<img width="50" height="50" src="/uploads/` + user.image + `" />`;
                    } else {
                        userImage = `<img width="50" height="50" src="/img/meobongbong.jpg" />`;
                    }
                    tbody.innerHTML +=
                        `<tr>
                            <td id="td-checkbox" class="alg-center">
                                <input id="checkbox-child-`+ user.id + `" type="checkbox" />
                            </td>
                            <td onclick="handleToEdit(`+ user.id + `)">
   `+ userImage + `
</td>
                            <td onclick="handleToEdit(`+ user.id + `)" class="prd-text-left">` + user.name + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.age + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.gender + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.number + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.username + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.password + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.priority + `</td>
                            `+ btnStatus + `
                            <td class="prd-edit tall alg-center">
                                <button onclick="deleteButton('`+ user.username + `')" id="del-` + user.id + `" href="" style="background-color: red"><i class="fa fa-solid fa-trash"></i></button>
                            </td>
                        </tr>`;
                });

                table.appendChild(tbody);

            },
            error: function (xhr, status, error) {
                alert(status);
            }
        })
    }
}

function moveToTrash() {
    var checkboxes = document.querySelectorAll('input[type="checkbox"]');
    var listChecked = [];

    checkboxes.forEach(function (childCheckbox) {
        if (childCheckbox.checked) {
            listChecked.push(parseInt(childCheckbox.id.split('-')[2]));
        }
    });
    listChecked = listChecked.filter(checked => !isNaN(checked));
    //console.log(listChecked);

    $.ajax({
        url: "MoveToTrash",
        type: "GET",
        data: {
            ids: listChecked
        },
        dataType: 'json',
        traditional: true,
        success: function (response) {
            //console.log(response);
            var tbodyFirst = document.getElementsByTagName('tbody');
            console.log(tbodyFirst);

            for (var i = 0; i < tbodyFirst.length; i++) {
                tbodyFirst[i].remove();
            }

            var table = document.getElementsByTagName('table')[0];
            var tbody = document.createElement('tbody');
            var users = response;
            tbody.innerHTML = ``;
            users.forEach(function (user) {
                var btnStatus = ``
                if (user.status == "Open") {
                    btnStatus = `<td class="prd-edit tall alg-center"><button id="btn-unlock" onclick="handleLock(` + user.id + `)" class="" style="background-color: #27ae60"><i class="fa-solid fa-unlock"></i></button></td>`
                } else {
                    btnStatus += `<td class="prd-edit tall alg-center"><button id="btn-lock" onclick="handleLock(` + user.id + `)" style="background-color: #f39c12"><i class="fa-solid fa-lock"></i></button></td>`
                }

                var userImage = ``;
                if (user.image != null) {
                    userImage = `<img width="50" height="50" src="/uploads/` + user.image + `" />`;
                } else {
                    userImage = `<img width="50" height="50" src="/img/meobongbong.jpg" />`;
                }

                tbody.innerHTML +=
                    `<tr>
                            <td id="td-checkbox" class="alg-center">
                                <input id="checkbox-child-`+ user.id + `" type="checkbox" />
                            </td>
                            <td onclick="handleToEdit(`+ user.id + `)">
                               `+ userImage + `
                            </td>
                            <td onclick="handleToEdit(`+ user.id + `)" class="prd-text-left">` + user.name + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.age + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.gender + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.number + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.username + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.password + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.priority + `</td>
                            `+ btnStatus + `
                            <td class="prd-edit tall alg-center">
                                <button onclick="deleteButton('`+ user.username + `')" id="del-` + user.id + `" href="" style="background-color: red"><i class="fa fa-solid fa-trash"></i></button>
                            </td>
                        </tr>`;
            });

            table.appendChild(tbody);

        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function searchDeleteUser() {
    var input = document.getElementById('search-input').value;
    if (input !== '') {
        $.ajax({
            url: "SearchDelete",
            type: "GET",
            dataType: 'json',
            data: { input: input },
            success: function (response) {
                // Remove all tbody before
                var tbodyFirst = document.getElementsByTagName('tbody');
                console.log(tbodyFirst);

                for (var i = 0; i < tbodyFirst.length; i++) {
                    tbodyFirst[i].remove();
                }
                if (response.success == true) {
                    // Append new tbody
                    var table = document.getElementsByTagName('table')[0];

                    var tbody = document.createElement('tbody');
                    var users = response.data;
                    tbody.innerHTML = ``;
                    users.forEach(function (user) {
                        var btnStatus = ``
                        if (user.status == "Open") {
                            btnStatus = `<td class="prd-edit tall alg-center"><button id="btn-unlock" onclick="handleLock(` + user.id + `)" class="" style="background-color: #27ae60"><i class="fa-solid fa-unlock"></i></button></td>`
                        } else {
                            btnStatus += `<td class="prd-edit tall alg-center"><button id="btn-lock" onclick="handleLock(` + user.id + `)" style="background-color: #f39c12"><i class="fa-solid fa-lock"></i></button></td>`
                        }
                        var userImage = ``;

                        if (user.image != null) {
                            userImage = `<img width="50" height="50" src="/uploads/` + user.image + `" />`;
                        } else {
                            userImage = `<img width="50" height="50" src="/img/meobongbong.jpg" />`;
                        }
                        tbody.innerHTML +=
                            `<tr>
                            <td id="td-checkbox" class="alg-center">
                                <input id="checkbox-child-`+ user.id + `" type="checkbox" />
                            </td>
                            <td onclick="handleToEdit(`+ user.id + `)">
   `+ userImage + `
</td>
                            <td onclick="handleToEdit(`+ user.id + `)" class="prd-text-left">` + user.name + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.age + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.gender + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.number + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.username + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.password + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.priority + `</td>
                            <td class="prd-edit tall alg-center">
                                <button onclick="deleteButton('`+ user.username + `')" id="del-` + user.id + `" href="" style="background-color: red"><i class="fa fa-solid fa-trash"></i></button>
                            </td>
                        </tr>`;
                    });

                    table.appendChild(tbody);
                } else {
                    var table = document.getElementsByTagName('table')[0];

                    var h1 = document.createElement('h1');
                    h1.textContent = 'No result!';
                    h1.style.textAlign = "center";
                    h1.id = "no-result";
                    table.parentNode.insertBefore(h1, table.nextSibling);

                }
            },
            error: function (xhr, status, error) {
                alert(status);
            }
        })
    } else {
        if (document.getElementById('no-result')) {
            document.getElementById('no-result').remove();
        }

        $.ajax({
            url: "GetAllItemTrash",
            type: "GET",
            dataType: "json",
            success: function (response) {
                // Remove all tbody before
                var tbodyFirst = document.getElementsByTagName('tbody');
                //console.log(tbodyFirst);

                for (var i = 0; i < tbodyFirst.length; i++) {
                    tbodyFirst[i].remove();
                }

                // Append new tbody
                var table = document.getElementsByTagName('table')[0];

                var tbody = document.createElement('tbody');
                var users = response;
                tbody.innerHTML = ``;
                users.forEach(function (user) {
                    var btnStatus = ``
                    if (user.status == "Open") {
                        btnStatus = `<td class="prd-edit tall alg-center"><button id="btn-unlock" onclick="handleLock(` + user.id + `)" class="" style="background-color: #27ae60"><i class="fa-solid fa-unlock"></i></button></td>`
                    } else {
                        btnStatus += `<td class="prd-edit tall alg-center"><button id="btn-lock" onclick="handleLock(` + user.id + `)" style="background-color: #f39c12"><i class="fa-solid fa-lock"></i></button></td>`
                    }
                    var userImage = ``;

                    if (user.image != null) {
                        userImage = `<img width="50" height="50" src="/uploads/` + user.image + `" />`;
                    } else {
                        userImage = `<img width="50" height="50" src="/img/meobongbong.jpg" />`;
                    }
                    tbody.innerHTML +=
                        `<tr>
                            <td id="td-checkbox" class="alg-center">
                                <input id="checkbox-child-`+ user.id + `" type="checkbox" />
                            </td>
                            <td onclick="handleToEdit(`+ user.id + `)">
   `+ userImage + `
</td>
                            <td onclick="handleToEdit(`+ user.id + `)" class="prd-text-left">` + user.name + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.age + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.gender + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.number + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.username + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.password + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.priority + `</td>
                            <td class="prd-edit tall alg-center">
                                <button onclick="deleteButton('`+ user.username + `')" id="del-` + user.id + `" href="" style="background-color: red"><i class="fa fa-solid fa-trash"></i></button>
                            </td>
                        </tr>`;
                });

                table.appendChild(tbody);

            },
            error: function (xhr, status, error) {
                alert(status);
            }
        })
    }
}

function deletePernament() {
    // Get checked
    var checkboxes = document.querySelectorAll('input[type="checkbox"]');
    var listChecked = [];

    checkboxes.forEach(function (childCheckbox) {
        if (childCheckbox.checked) {
            listChecked.push(parseInt(childCheckbox.id.split('-')[2]));
        }
    });
    //console.log(listChecked);

    $.ajax({
        url: "DoDeleteUsers",
        type: 'GET',
        data: {
            "ids": listChecked
        },
        traditional: 'true',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                window.location.href = "/Home/ShowTrash";
            }
        },
        error: function (xhr, status, error) {
            console.log(status);
        }

    })
}

function restore() {
    // Get checked
    var checkboxes = document.querySelectorAll('input[type="checkbox"]');
    var listChecked = [];

    checkboxes.forEach(function (childCheckbox) {
        if (childCheckbox.checked) {
            listChecked.push(parseInt(childCheckbox.id.split('-')[2]));
        }
    });

    $.ajax({
        url: "RestoreUser",
        type: 'GET',
        data: {
            ids: listChecked
        },
        traditional: true,
        dataType: 'json',
        success: function (response) {


            window.location.href = "/Home/ShowTrash";
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}

function bulkAction() {
    // Get selected value
    var select = document.getElementById('select-bulk');
    var selectedIndex = select.selectedIndex;
    var selectedOption = select.options[selectedIndex];
    var selectedValue = selectedOption.value;


    if (selectedValue === "Reset password") {
        getAllChecked();
    } else if (selectedValue === "Move to trash") {
        moveToTrash();
    } else if (selectedValue === 'Create id card') {
        console.log('Create id card');
        createIdCard();
    }
}

function createIdCard() {
    var checkboxes = document.querySelectorAll('input[type="checkbox"]');
    var listChecked = [];

    checkboxes.forEach(function (childCheckbox) {
        if (childCheckbox.checked) {
            listChecked.push(parseInt(childCheckbox.id.split('-')[2]));
        }
    });
    listChecked = listChecked.filter(checked => !isNaN(checked));

    showLoading('on');

    if (listChecked.length != 0) {
        var idString = listChecked.join(',');
        
        $.ajax({
            url: '../Home/ExportCard1',
            type: 'GET',
            data: { ids: idString },
            xhrFields: {
                responseType: 'blob'
            },
            success: function (data) {
                showLoading('off');
                var blob = new Blob([data]);
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = 'User.pdf';
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);


            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    }
}

function sortAction() {
    var type = document.getElementById("sort-action").value;
    //console.log(type);
    $.ajax({
        url: "GetAllItem",
        type: 'GET',
        data: { sort: type },
        traditional: true,
        success: function (response) {
            // Remove all tbody before
            var tbodyFirst = document.getElementsByTagName('tbody');

            for (var i = 0; i < tbodyFirst.length; i++) {
                tbodyFirst[i].remove();
            }

            // Append new tbody
            var table = document.getElementsByTagName('table')[0];
            var tbody = document.createElement('tbody');
            var users = response;

            // Total search
            var totalSearch = response.length;
            var divTotal = document.querySelector('#count-item > span');
            divTotal.innerText = '';
            divTotal.innerText = 'Total: ' + totalSearch;

            tbody.innerHTML = ``;
            users.forEach(function (user) {
                var btnStatus = ``
                if (user.status == "Open") {
                    btnStatus = `<td class="prd-edit tall alg-center"><button id="btn-unlock" onclick="handleLock(` + user.id + `)" class="" style="background-color: #27ae60"><i class="fa-solid fa-unlock"></i></button></td>`
                } else {
                    btnStatus += `<td class="prd-edit tall alg-center"><button id="btn-lock" onclick="handleLock(` + user.id + `)" style="background-color: #f39c12"><i class="fa-solid fa-lock"></i></button></td>`
                }
                var userImage = ``;

                if (user.image != null) {
                    userImage = `<img width="50" height="50" src="/uploads/` + user.image + `" />`;
                } else {
                    userImage = `<img width="50" height="50" src="/img/meobongbong.jpg" />`;
                }
                tbody.innerHTML +=
                    `<tr>
                            <td id="td-checkbox" class="alg-center">
                                <input id="checkbox-child-`+ user.id + `" type="checkbox" />
                            </td>
                            <td onclick="handleToEdit(`+ user.id + `)">
                               `+ userImage + `
                            </td>
                            <td onclick="handleToEdit(`+ user.id + `)" class="prd-text-left">` + user.name + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.age + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.gender + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.number + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.username + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.password + `</td>
                            <td onclick="handleToEdit(`+ user.id + `)">` + user.priority + `</td>
                            `+ btnStatus + `
                            <td class="prd-edit tall alg-center">
                                <button onclick="deleteButton('`+ user.username + `')" id="del-` + user.id + `" href="" style="background-color: red"><i class="fa fa-solid fa-trash"></i></button>
                            </td>
                        </tr>`;
            });

            table.appendChild(tbody);
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    })
}

function navigate(type) {
    switch (type) {
        case 'users':
            window.location.href = "/Home/Index";
            break;
        case 'trash':
            window.location.href = "/Home/ShowTrash";
            break;
        case 'export':
            window.location.href = "/Home/ExportUserInExcel1?type=All";
            break;
        case 'card':
            window.location.href = "/Home/ShowCard";
            break;
        default:
            window.location.href = "/Home/Index";
    }
}

function exportAction() {
    var type = document.getElementById("export-action").value;
    //console.log(type);
    if (type == "toExcel") {
        window.location.href = "/Home/ExportUserInExcel1?type=OpenLock";
    } else if (type = "toExcelTrash") {
        window.location.href = "/Home/ExportUserInExcel1?type=OnTrash";
    }
}

function previewImg() {
    const blah = document.getElementById("img-show");
    const imgInp = document.getElementById("imgInp");
    const [file] = imgInp.files;
    if (file) {
        if (blah) {
            blah.src = URL.createObjectURL(file);
        } else {
            var img = document.createElement('img');
            img.src = URL.createObjectURL(file);
            img.id = "img-shows";

            document.getElementById('add-photo').appendChild(img);
        }
    }

}