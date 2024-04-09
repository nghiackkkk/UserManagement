// Get ther search input by press Enter
document.addEventListener("DOMContentLoaded", function () {
    var searchInput = document.getElementById("searchInput");

    searchInput.addEventListener("keypress", function (event) {
        // Check if Enter key was pressed (keyCode 13)
        if (event.keyCode === 13) {
            search();
        }
    });

    document.getElementById('sideBarUser').classList.add('active');
});
function checkedRow(td) {

    var row = td.parentNode;
    var checkbox = row.querySelector('input[type="checkbox"]');

    checkbox.checked = !checkbox.checked;

    row.classList.toggle("checked-color");

}
function checkedRowCB(td) {

    var row = td.parentNode.parentNode;
    //var checkbox = row.querySelector('input[type="checkbox"]');

    //checkbox.checked = !checkbox.checked;

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

    url.searchParams.set('sort', sort);
    url.searchParams.set('pageNumber', pageNumber);
    url.searchParams.set('pageSize', value);
    url.searchParams.set('search', s);
    //console.log(url);

    // Rebind data
    $.ajax({
        url: "/Admin/ShowUserJson",
        type: 'GET',
        data: {
            sort: sort,
            pageNumber: pageNumber,
            pageSize: value,
            search: s
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
    url.searchParams.set('sort', value);
    url.searchParams.set('pageNumber', pageNumber);
    url.searchParams.set('pageSize', ps);
    url.searchParams.set('search', s);
    // Rebind data
    $.ajax({
        url: "/Admin/ShowUserJson",
        type: 'GET',
        data: {
            sort: value,
            pageNumber: pageNumber,
            pageSize: ps,
            search: s
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
    window.location.href = "/Admin/SearchUser?search=" + query + "&type=active&sort=latest&pageNumber=1&pageSize=10";
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
    console.log(selectedValue);
    //alert(listIdLe);
    if (listId.length != 0) {
        if (selectedValue === "reset-password") {
            resetPassword(listId);
        } else if (selectedValue === "move-to-trash") {
            for (let i = 0; i < listIdLe; i++) {
                moveToTrash(listId[i]);
            }
        } else if (selectedValue === "print-card") {
            printCard(listId);
        } else if (selectedValue === "print-profile") {
            
            printProfile(listId);
        }
        else {
            window.location.href = "/Admin/ShowUser";
        }
    }
    
}

function printProfile(listId) {
    $.ajax({
        url: "/Admin/PrintProfile",
        type: "GET",
        data: {
            ids: listId
        },
        traditional: true,
        dataType: "html",
        success: function (data) {
            // Convert the HTML string into a DOM object
            const parser = new DOMParser();
            const doc = parser.parseFromString(data, "text/html");

            // Choose the elements that your content will be rendered to.
            const elements = doc.getElementsByClassName('profile-id');
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
                const filename = `Profile_${listId[i]}.pdf`;

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

function printCard(listId) {
    $.ajax({
        url: "/Admin/PrintCard",
        type: "GET",
        data: {
            listId: listId
        },
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
                const filename = `ID_Card_${listId[i]}.pdf`;

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
                        <p class="d-flex justify-content-center align-items-center mb-0">Reset password successfully! This page will reload in <span id="countdown">3</span> seconds.</p>
                    </div>`;

                document.body.insertBefore(divElement, document.getElementById('wrapper'));

                var countdownElement = document.getElementById('countdown');
                var countdownValue = 3;

                var countdownInterval = setInterval(function () {
                    countdownValue--;
                    countdownElement.textContent = countdownValue;
                    if (countdownValue <= 0) {
                        clearInterval(countdownInterval);
                        window.location.reload();
                    }
                }, 1000);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

