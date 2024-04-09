// Get ther search input by press Enter
document.addEventListener("DOMContentLoaded", function () {
    var searchInput = document.getElementById("searchInput");

    searchInput.addEventListener("keypress", function (event) {
        // Check if Enter key was pressed (keyCode 13)
        if (event.keyCode === 13) {
            search();
        }
    });

    document.getElementById('sideBarTrash').classList.add('active');
});

function checkedRow(td) {

    var row = td.parentNode;
    var checkbox = row.querySelector('input[type="checkbox"]');

    checkbox.checked = !checkbox.checked;

    row.classList.toggle("checked-color");

}
function checkedRowCB(td) {
    var row = td.parentNode.parentNode;
    row.classList.toggle("checked-color");
}
function handleCheck(checkbox) {
    var checkboxes = document.querySelectorAll('input[type="checkbox"]');
    checkboxes.forEach(function (childCheckbox) {
        if (childCheckbox.classList.contains('checkbox-child')) {
            childCheckbox.checked = checkbox.checked;
            if (checkbox.checked) {
                childCheckbox.parentNode.parentNode.classList.add('checked-color');
            } else {
                childCheckbox.parentNode.parentNode.classList.remove('checked-color');
            }
        }
    });
}

// InnerHTML Paginate
function innerPaginate(response, sort = 'latest') {

    var ulPaginate = document.getElementById('ulPaginate');
    ulPaginate.innerHTML = '';
    var paginate = response.paginate.value;
    if (paginate.pageNumber > 1) {
        ulPaginate.innerHTML +=
            `<li class="paginate_button page-item previous" id="datatables-orders_previous">
                            <a href="/Admin/ShowUser?sort=` + sort + `&pageNumber=` + (paginate.pageNumber - 1) + `&pageSize=` + paginate.pageSize + `" aria-controls="datatables-orders" aria-disabled="true" aria-role="link" data-dt-idx="previous" tabindex="0" class="page-link">Previous</a>
                        </li>`;
    } else {
        ulPaginate.innerHTML +=
            `<li class="paginate_button page-item previous disabled" id="datatables-orders_previous">
                            <a href="#" aria-controls="datatables-orders" aria-disabled="true" aria-role="link" data-dt-idx="previous" tabindex="0" class="page-link">Previous</a>
                        </li>`;
    }

    for (let i = 1; i <= paginate.totalPage; i++) {
        if (i != paginate.pageNumber) {
            ulPaginate.innerHTML +=
                `<li class="paginate_button page-item">
                                <a href="/Admin/ShowUser?sort=`+ sort + `&pageNumber=` + i + `&pageSize=` + paginate.pageSize + `" aria-controls="datatables-orders" aria-role="link" aria-current="page" data-dt-idx="0" tabindex="0" class="page-link">` + i + `</a>
                            </li>`;
        } else {
            ulPaginate.innerHTML +=
                `<li class="paginate_button page-item active">
                                <a href="/Admin/ShowUser?sort=`+ sort + `&pageNumber=` + i + `&pageSize=` + paginate.pageSize + `" aria-controls="datatables-orders" aria-role="link" aria-current="page" data-dt-idx="0" tabindex="0" class="page-link">` + i + `</a>
                            </li>`;
        }
    }

    if (paginate.pageNumber < paginate.totalPage) {
        ulPaginate.innerHTML +=
            `<li class="paginate_button page-item next" id="datatables-orders_next">
                <a href="/Admin/ShowUser?sort=` + sort + `&pageNumber=` + (paginate.pageNumber + 1) + `&pageSize=` + paginate.pageSize + `" aria-controls="datatables-orders" aria-role="link" data-dt-idx="next" tabindex="0" class="page-link">Next</a>
            </li>`;
    } else {
        ulPaginate.innerHTML +=
            `<li class="paginate_button page-item next disabled" id="datatables-orders_next">
                <a aria-controls="datatables-orders" aria-role="link" data-dt-idx="next" tabindex="0" class="page-link">Next</a>
            </li>`;
    }
}

// InnerHTML users
function innerUser(response) {
    var tbody = document.getElementsByTagName('tbody')[0];
    tbody.innerHTML = ``;
    var users = response.users;

    for (var i = 0; i < users.length; i++) {
        var idStatus = (users[i].status == 'Open') ? 'checked' : '';
        var imgSrc = '';
        if (users[i].coverImage != null) {
            imgSrc = '/uploads/' + users[i].coverImage;
        } else {
            imgSrc = '/img/meobongbong.jpg';
        }
        tbody.innerHTML +=
            `<tr>
                <td>
                    <input onclick="checkedRowCB(this)" id="checkbox-child" class="checkbox-child bigger-checkbox" type="checkbox" />
                </td>
                <td onclick="checkedRow(this)">
                    <img src="`+ imgSrc + `" width="48" height="48" class="rounded-circle me-2" alt="Avatar">
                    `+ users[i].fullName + `
                </td>
                <td onclick="checkedRow(this)">`+ users[i].idCard + `</td>
                <td onclick="checkedRow(this)">`+ users[i].phoneNumber + `</td>
                <td onclick="checkedRow(this)">`+ users[i].dateOfBirth + `</td>
                <td onclick="checkedRow(this)">`+ users[i].username + `</td>
                <td>
                    <div class="form-check form-switch">
                        <input onclick="toggleStatus(@user.Id, this, '@user.Status')" class="form-check-input bigger-checkbox-2" type="checkbox" id="flexSwitchCheckDefault"
                        `+ idStatus + `>
                    </div>
                </td>
                <td class="table-action" onclick="checkedRow(this)">
                    <div class="d-flex justify-content-around align-items-center">
                        <a href="#">
                            <i data-feather="edit"></i>
                        </a>
                        <a href="#">
                            <i data-feather="trash"></i>
                        </a>
                    </div>
                </td>
            </tr>`;
    }
}

// InnerHTML total user
function innerPageSize(pageSize, totalUser) {
    var label = document.getElementById('datatables-orders_info');
    label.innerHTML = ``;
    label.innerHTML = `Showing 1 to ` + pageSize + ` of ` + totalUser + ` entries`;
}

// Change pageSize
function redirectToLink(pageNumber) {
    // Get current URL
    var url = new URL(window.location.href);
    var value = document.getElementById('datatables-orders-length').value;

    // Set or update the parameter value
    var sort = (url.searchParams.get('sort')) ? url.searchParams.get('sort') : '';
    var s = (url.searchParams.get('search')) ? url.searchParams.get('search') : "";
    var t = 'trash';
    url.searchParams.set('sort', sort);
    url.searchParams.set('pageNumber', pageNumber);
    url.searchParams.set('pageSize', value);
    url.searchParams.set('search', s);
    url.searchParams.set('type', t);
    //console.log(url);

    // Rebind data
    $.ajax({
        url: "/Admin/ShowUserJson",
        type: 'GET',
        data: {
            sort: sort,
            pageNumber: pageNumber,
            pageSize: value,
            search: s,
            type: t
        },
        success: function (response) {
            if (response.result = true) {
                innerUser(response);
                innerPaginate(response, sort);
                innerPageSize(value, response.paginate.value.totalUsers);
                feather.replace();
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });

    // Push the updated URL to the browser history
    window.history.pushState({ path: url.href }, '', url.href);
}

// Change sort
function sortAction(pageNumber) {
    // Get current URL
    var url = new URL(window.location.href);
    var value = document.getElementById('selectSort').value;

    console.log(url);
    // Set or update the parameter value
    var ps = (url.searchParams.get('pageSize')) ? url.searchParams.get('pageSize') : 10;
    var s = (url.searchParams.get('search')) ? url.searchParams.get('search') : "";
    var t = 'trash';

    url.searchParams.set('sort', value);
    url.searchParams.set('pageNumber', pageNumber);
    url.searchParams.set('pageSize', ps);
    url.searchParams.set('search', s);
    url.searchParams.set('type', t);

    // Rebind data
    $.ajax({
        url: "/Admin/ShowUserJson",
        type: 'GET',
        data: {
            sort: value,
            pageNumber: pageNumber,
            pageSize: ps,
            search: s,
            type: t
        },
        success: function (response) {
            if (response.result = true) {
                innerUser(response);
                innerPaginate(response, value);
                innerPageSize(ps, response.paginate.value.totalUsers);
                feather.replace();
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });

    // Push the updated URL to the browser history
    window.history.pushState({ path: url.href }, '', url.href);
}

// Click change status 
function toggleStatus(userId, checkbox, status) {
    $.ajax({
        url: "/Admin/ToggleStatus",
        type: "GET",
        data: {
            id: userId
        },
        success: function (response) {
            if (response) {
                var statusSwitch = checkbox.parentNode;
                var toggleStatus = (status == 'Open') ? 'Lock' : 'Open';
                var isChecked = (toggleStatus == 'Open') ? 'Checked' : '';
                statusSwitch.innerHTML = ``;
                statusSwitch.innerHTML =
                    `<input onclick="toggleStatus(` + userId + `, this, '` + toggleStatus + `')" class="form-check-input bigger-checkbox-2" type="checkbox" id="flexSwitchCheckDefault" ` + isChecked + `>`;
            }
        }
    });
}

function search() {
    var query = document.getElementById("searchInput").value;
    window.location.href = "/Admin/SearchUser?search=" + query + "&type=trash&sort=latest&pageNumber=1&pageSize=10";
}

function moveToTrash(userId) {
    $.ajax({
        url: "/Admin/ToggleTrash",
        type: "POST",
        data: {
            id: userId,
        },
        success: function (response) {
            if (response) {
                window.location.reload();
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

// Bulk action (reset password, move to trash)
function bulkAction() {
    var select = document.getElementById('select-bulk');
    var selectedIndex = select.selectedIndex;
    var selectedOption = select.options[selectedIndex];
    var selectedValue = selectedOption.value;

    var listId = getAllChecked();
    var listIdLe = listId.length;

    //alert(listIdLe);

    if (selectedValue === "reset-password") {
        resetPassword(listId);
    } else if (selectedValue === "move-to-trash") {
        for (let i = 0; i < listIdLe; i++) {
            moveToTrash(listId[i]);
        }
    } else {
        window.location.href = "/Admin/ShowUser";
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
    console.log(listChecked);
    return listChecked;
}

function resetPassword(listId) {
    $.ajax({
        url: "/Admin/ResetPassword",
        type: "POST",
        data: {
            ids: listId
        },
        dataType: 'json',
        traditional: true,
        success: function (response) {
            if (response) {
                var divElement = document.createElement('div');
                divElement.id = 'successNotification';
                divElement.classList.add('alert', 'alert-primary', 'alert-dismissible', 'fixed-top');
                divElement.setAttribute('role', 'alert');
                divElement.innerHTML =
                    `<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    <div class="alert-message d-flex justify-content-center">
                        Reset password successfully!
                    </div>`;

                document.body.insertBefore(divElement, document.getElementById('wrapper'));
                setTimeout(function () {
                    //divElement.classList.add('d-none');
                    window.location.reload();
                }, 3000);

            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

// Restore user
function restore() {
    var listId = getAllChecked();
    var listIdLe = listId.length;

    for (let i = 0; i < listIdLe; i++) {
        moveToTrash(listId[i]);
    }
}

// Delete pernament 
function deletePernament() {
    var listId = getAllChecked();
    $.ajax({
        url: "/Admin/DeletePernament",
        type: "POST",
        data: {
            ids: listId,
        },
        success: function (response) {
            if (response) {
                window.location.reload();
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });

}