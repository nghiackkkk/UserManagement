//FUNCTION
function formatDateForm(date) {
    // convert from yyyy-MM-dd to dd-MM-yyyy
    var dateSplit = date.split('-');
    var res = dateSplit[2] + '-' + dateSplit[1] + '-' + dateSplit[0];
    return res;
}

//END FUNCTION
// TIME CHECK TABLE
if (document.getElementById("timecheck-table") != null) {
    var tableTimeCheck = new DataTable("#timecheck-table", {
        columns: [
            { width: '50px', orderable: false },
            { width: '70px', className: 'text-start' },
            { width: '100px', className: 'text-start' },
            { width: '200px' },
            { width: '150px' },
            { width: '100px' },
            { className: 'text-start' },
            null,
            null,
            { width: '100px', orderable: true },
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

    $('#timecheck-table tbody').on('click', 'tr td:not(:last-child)', function () {
        var checkbox = $(this).find('input[type="checkbox"]');

        checkbox.prop('checked', !checkbox.prop('checked'));

        $(this).parent().toggleClass('selected');
    });

    var dataTimeCheck = [
    ];

    $.ajax({
        url: "/Admin/TimeKeeping/API/GetDataTC",
        type: "GET",
        success: function (response) {
            if (response) {
                var datas = response.data;

                datas.forEach(function (data) {
                    dataTimeCheck.push(data);
                });

                dataTimeCheck.forEach((item, index) => {
                    var actionIcons = ``;
                    if (item.reason != null) {
                        var actionIcons = `
                        <div class="d-flex justify-content-around align-items-start">
                            <a class="text-center" data-bs-toggle="modal" data-bs-target="#timecheck-modal" `+ `
                            data-bs-reason="`+ item.reason + `" ` + `
                            data-bs-accepted="`+ item.accepted + `" ` + `
                            data-bs-idAC="`+ item.id_attendanceCheck + `">
                                <i class="icon-edit" data-feather="info"></i>
                            </a>
                        </div>
                    `;
                    }
                    var day = formatDateForm(item.day);
                    console.log(day);
                    tableTimeCheck.row.add([
                        '<input type="checkbox">',
                        index + 1,
                        item.id,
                        item.fullName,
                        item.department,
                        item.position,
                        day,
                        item.time_in,
                        item.time_out,
                        actionIcons
                    ]).draw();
                });

                feather.replace();
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
    function filterTC() {
        // Get data filter
        var dayVal = document.getElementById('day-input').value;
        var depVal = document.getElementById('department-select').value;
        var posVal = document.getElementById('position-select').value;

        // Clear existing table data
        tableTimeCheck.clear();
        console.log(dayVal);
        //Retrieve data and redraw table
        $.ajax({
            url: "/Admin/TimeKeeping/API/FilterTC",
            type: "GET",
            data: {
                day: dayVal,
                dep: depVal,
                pos: posVal
            },
            success: function (response) {
                if (response) {
                    var datas = response.data;
                    datas.forEach(function (data) {

                        const department = data.department !== null ? data.department : "null";
                        const position = data.position !== null ? data.position : "null";

                        const actionIcons = `
                        <div class="d-flex justify-content-around align-items-start">
                            <a class="text-center" data-bs-toggle="modal" data-bs-target="#timecheck-modal" `+ `
                            data-bs-reason="`+ data.reason + `" ` + `
                            data-bs-accepted="`+ data.accepted + `">
                                <i class="icon-edit" data-feather="info"></i>
                            </a>
                        </div>
                    `;

                        tableTimeCheck.row.add([
                            null,
                            data.id,
                            data.fullName,
                            department,
                            position,
                            data.day,
                            data.time_in,
                            data.time_out,
                            actionIcons
                        ]);
                    });

                    tableTimeCheck.draw();
                    feather.replace();
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    }
    function refreshTC() {
        window.location.reload();
    }
    function todayTC() {
        // Get data filter
        var currentDate = new Date();
        var year = currentDate.getFullYear();
        var month = ('0' + (currentDate.getMonth() + 1)).slice(-2); // Adding 1 to month because month index starts from 0
        var day = ('0' + currentDate.getDate()).slice(-2);
        var formattedDate = year + '-' + month + '-' + day;

        var dayVal = formattedDate;
        var depVal = document.getElementById('department-select').value;
        var posVal = document.getElementById('position-select').value;

        // Clear existing table data
        tableTimeCheck.clear();

        //Retrieve data and redraw table
        $.ajax({
            url: "/Admin/TimeKeeping/API/FilterTC",
            type: "GET",
            data: {
                day: dayVal,
                dep: depVal,
                pos: posVal
            },
            success: function (response) {
                if (response) {
                    var datas = response.data;
                    datas.forEach(function (data) {

                        const department = data.department !== null ? data.department : "null";
                        const position = data.position !== null ? data.position : "null";

                        const actionIcons = `
                        <div class="d-flex justify-content-around align-items-start">
                            <a class="text-center" data-bs-toggle="modal" data-bs-target="#timecheck-modal" `+ `
                            data-bs-reason="`+ data.reason + `" ` + `
                            data-bs-accepted="`+ data.accepted + `">
                                <i class="icon-edit" data-feather="info"></i>
                            </a>
                        </div>
                    `;

                        tableTimeCheck.row.add([
                            null,
                            data.id,
                            data.fullName,
                            department,
                            position,
                            data.day,
                            data.time_in,
                            data.time_out,
                            actionIcons
                        ]);
                    });

                    tableTimeCheck.draw();
                    feather.replace();

                    var dateInput = document.getElementById('day-input');
                    dateInput.value = dayVal;
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    }
}
// END TIME CHECK TABLE

// WORKING DAY
if (document.getElementById("workingday-table") != null) {
    var tableworkingday = new DataTable("#workingday-table", {
        columns: [
            { width: '70px', className: 'text-start' },
            { width: '100px', className: 'text-start' },
            null,
            null,
            null,
            null,
            { className: 'text-start' },
            //{ width: '100px', orderable: false },
        ],
        display: 'hover',
        stripeClasses: [],
        rowReorder: {
            selector: 'td:nth-child(2)'
        },
        drawCallback: function (settings) {
            feather.replace();
        },
        responsive: true,

    });
    var dataworkingday = [];

    $.ajax({
        url: "/Admin/TimeKeeping/API/GetDataWD",
        type: "GET",
        success: function (response) {
            if (response) {
                var datas = response.data;

                datas.forEach(function (data) {
                    dataworkingday.push(data);
                });

                dataworkingday.forEach((item, index) => {
                    item.month = item.month < 10 ? `0${item.month}` : item.month;
                    var monthYear = item.month + "-" + item.year;
                    const department = item.department !== null ? item.department : "null";
                    const position = item.position !== null ? item.position : "null";

                    tableworkingday.row.add([
                        index + 1,
                        item.staffId,
                        item.fullName,
                        department,
                        position,
                        monthYear,
                        item.numberChecked,
                    ]).draw();
                });
                feather.replace();
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });

    function filterWD() {
        // Get data filter
        var monVal = document.getElementById('month-select').value;
        var yearVal = document.getElementById('year-inp').value;
        var depVal = document.getElementById('department-select').value;
        var posVal = document.getElementById('position-select').value;

        // Clear existing table data
        tableworkingday.clear();

        //Retrieve data and redraw table
        $.ajax({
            url: "/Admin/TimeKeeping/API/FilterWD",
            type: "GET",
            data: {
                mon: monVal,
                year: yearVal,
                dep: depVal,
                pos: posVal
            },
            success: function (response) {
                if (response) {
                    var datas = response.data;
                    datas.forEach(function (data) {
                        var monthYear = data.month + "/" + data.year;
                        const department = data.department !== null ? data.department : "null";
                        const position = data.position !== null ? data.position : "null";

                        tableworkingday.row.add([
                            null, // This will be auto-incremented by DataTable
                            data.staffId,
                            data.fullName,
                            department,
                            position,
                            monthYear,
                            data.numberChecked
                        ]);
                    });

                    tableworkingday.draw(); // Redraw the DataTable after adding rows
                    feather.replace();
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    }

}
// END WORKING DAY

// EXTRA SHIFT TABLE
if (document.getElementById('shift-table') != null) {
    const table = new DataTable("#shift-table", {
        columns: [
            { width: '50px', className: 'text-start' },
            null,
            null,
            null,
            null,
            { width: '100px' },
        ],
        display: 'hover',
        drawCallback: function (settings) {
            feather.replace();
        }
    });
    var data = [
        { name: "John Doe", timeIn: "09:00 AM", timeOut: "05:00 PM", description: "Morning shift" },
        { name: "John Doe", timeIn: "09:00 AM", timeOut: "05:00 PM", description: "Morning shift" },
        { name: "John Doe", timeIn: "09:00 AM", timeOut: "05:00 PM", description: "Morning shift" },
        { name: "John Doe", timeIn: "09:00 AM", timeOut: "05:00 PM", description: "Morning shift" },
        { name: "John Doe", timeIn: "09:00 AM", timeOut: "05:00 PM", description: "Morning shift" },
        { name: "John Doe", timeIn: "09:00 AM", timeOut: "05:00 PM", description: "Morning shift" },
        { name: "John Doe", timeIn: "09:00 AM", timeOut: "05:00 PM", description: "Morning shift" },
        { name: "John Doe", timeIn: "09:00 AM", timeOut: "05:00 PM", description: "Morning shift" },
        { name: "John Doe", timeIn: "09:00 AM", timeOut: "05:00 PM", description: "Morning shift" },
        { name: "John Doe", timeIn: "09:00 AM", timeOut: "05:00 PM", description: "Morning shift" },
        { name: "John Doe", timeIn: "09:00 AM", timeOut: "05:00 PM", description: "Morning shift" },
        { name: "John Doe", timeIn: "09:00 AM", timeOut: "05:00 PM", description: "Morning shift" },
        { name: "Jane Smith", timeIn: "01:00 PM", timeOut: "09:00 PM", description: "Afternoon shift" }

    ];

    data.forEach(function (item, index) {
        var actionIcons = '<div class="d-flex justify-content-around align-items-center">'
            + '<a class="text-center" data-bs-toggle="modal" data-bs-target="#edit-shift-modal">'
            + '<i data-feather="edit"></i></a>'
            + '<a class="text-center" data-bs-toggle="modal" data-bs-target="#delete-shift-modal">'
            + '<i data-feather="trash"></i></a></div>';
        table.row.add([
            index + 1,
            item.name,
            item.timeIn,
            item.timeOut,
            item.description,
            actionIcons
        ]).draw();
    });
    feather.replace();
}
// END EXTRA SHIFT TABLE

// ASSIGN SHIFT TO DEPARTMENT
if (document.getElementById("assign-shift-table") != null) {
    const tableSTD = new DataTable("#assign-shift-table", {
        columns: [
            { width: '70px', className: 'text-start' },
            { width: '400px', className: 'text-start' },
            { className: 'text-start' },
            null,
            null,
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

    const dataSTD = [
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
        { numberEmp: '100', shift: "A1", timeIn: "8:00:00", timeOut: "17:00:00", department: "HR" },
    ];

    dataSTD.forEach((item, index) => {
        const actionIcons = `
        <div class="d-flex justify-content-around align-items-start">
            <a class="text-center" data-bs-toggle="modal" data-bs-target="#edit-shift-modal">
                <i class="icon-edit" data-feather="edit"></i>
            </a>
        </div>
    `;

        tableSTD.row.add([
            index + 1,
            item.department,
            item.numberEmp,
            item.shift,
            item.timeIn,
            item.timeOut,
            actionIcons
        ]).draw();

    });
    feather.replace();
}
// END ASSIGN SHIFT TO DEPARTMENT

// ASSIGN DEPARTMENT/POSITION
if (document.getElementById('reschedule-table') != null) {
    const tableReschedule = new DataTable("#reschedule-table", {
        columns: [
            { width: '70px', className: 'text-start' },
            { width: '100px', className: 'text-start' },
            null,
            { className: 'text-start' },
            null,
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

    var dataReschedule = [

    ];

    // Get data for table
    $.ajax({
        url: "/Admin/TimeKeeping/API/GetDataAD",
        type: "GET",
        success: function (response) {
            if (response) {
                var datas = response.data;
                datas.forEach(function (data) {
                    dataReschedule.push(data);
                });

                dataReschedule.forEach((item, index) => {
                    const userId = item.userId !== null ? item.userId : "null";
                    const fullName = item.fullName !== null ? item.fullName : "null";
                    const dateOfBirth = item.dateOfBirth !== null ? formatDateForm(item.dateOfBirth) : "null";
                    const department = item.department !== null ? item.department : "null";
                    const position = item.position !== null ? item.position : "null";

                    const actionIcons = `
                    <div class="d-flex justify-content-around align-items-start">
                        <a class="text-center" data-bs-toggle="modal" data-bs-target="#edit-reschedule-modal"`
                        + `data-bs-id="` + userId + `" `
                        + `data-bs-name="` + fullName + `" `
                        + `data-bs-department="` + department + `" `
                        + `data-bs-position="` + position + `" >
                            <i class="icon-edit" data-feather="edit"></i>
                        </a>
                    </div>
                `;

                    tableReschedule.row.add([
                        index + 1,
                        userId,
                        fullName,
                        dateOfBirth,
                        department,
                        position,
                        actionIcons
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
// END ASSIGN DEPARTMENT/POSITION

// USER CHECK IO STATISTIC
if (document.getElementById('checkio-table') != null) {
    var table = new DataTable("#checkio-table", {
        columns: [
            { width: '30px', className: 'text-start' },
            { width: '100px', className: 'text-start' },
            null,
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

    var datas = [
    ];

    $.ajax({
        url: '/User/API/GetDataIO',
        type: "GET",
        success: function (response) {
            if (response) {
                var datas = response.data;

                datas.forEach((data, index) => {
                    if (data.reason != null) {
                        var actionIcons = `
                            <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#user-submitted-modal"`
                            + `data-bs-reason="` + data.reason + `" `
                            + `data-bs-accepted="` + data.accepted + `" `
                            + `data-bs-day="` + data.day + `" `
                            + `data-bs-timein="` + data.timeIn + `" `
                            + `data-bs-timeout="` + data.timeOut + `" `
                            + `>Reason</button>
                        `;
                    } else {
                        var actionIcons = `
                            <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#user-reason-modal" `
                            + `data-bs-reason="` + data.reason + `" `
                            + `data-bs-accepted="` + data.accepted + `" `
                            + `data-bs-day="` + data.day + `" `
                            + `data-bs-timein="` + data.timeIn + `" `
                            + `data-bs-timeout="` + data.timeOut + `" `
                            + `>Reason</button>
                        `;
                    }

                    table.row.add([
                        index + 1,
                        data.day,
                        data.timeIn,
                        data.timeOut,
                        actionIcons
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
// END USER CHECK IO STATISTIC

// ADMIN RQUEST FOR LEAVE
if (document.getElementById('rfl-table') != null) {
    var table = new DataTable("#rfl-table", {
        columns: [
            { width: '30px', className: 'text-start' },
            { width: '100px', className: 'text-start' },
            { width: '200px' },
            { width: '130px' },
            { width: '100px' },
            { width: '150px', className: 'text-start' },
            { width: '150px', className: 'text-start' },
            { width: '470px', className: 'text-start' },
            { orderable: false, className: 'row' },
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

    var datas = [
    ];

    $.ajax({
        url: '/Admin/TimeKeeping/API/GetDataRFL',
        type: "GET",
        success: function (response) {
            if (response) {
                var datas = response.data;

                datas.forEach((data, index) => {
                    var actionIcons = ``;
                    switch (data.accepted) {
                        case 'False':
                            actionIcons = `<div class="col-md-6 p-0">
                                                    <button onclick="changeStatus('Accepted', ` + data.id_absence + `)" class="btn btn-primary">Accept</button>
                                                </div>
                                                <div class="col-md-6 p-0">
                                                    <button onclick="changeStatus('Rejected', ` + data.id_absence + `)" class="btn btn-danger">Reject</button>
                                                </div>`;
                            break;
                        case 'Accepted':
                            actionIcons = `Accepted`;
                            break;
                        case 'Rejected':
                            actionIcons = `Rejected`;
                            break;
                        default:
                            actionIcons = 'Default';
                    }

                    table.row.add([
                        index + 1,
                        data.id_staff,
                        data.fullName,
                        data.department,
                        data.position,
                        formatDateForm(data.absence_from),
                        formatDateForm(data.absence_to),
                        data.reason,
                        actionIcons
                    ]).draw();
                });
                feather.replace();
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
    function changeStatus(status, id_absence) {
        var model = {
            status: status,
            id_absence: id_absence
        };
        $.ajax({
            url: '/Admin/TimeKeeping/API/UpdateStatusRFL',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(model),
            success: function (response) {
                // Xử lý phản hồi thành công từ server
                //window.location.reload();
                dataAJAX();
            },
            error: function (xhr, status, error) {
                // Xử lý lỗi
                console.log(error);
            }
        });
    }
    function dataAJAX() {
        table.clear();
        $.ajax({
            url: '/Admin/TimeKeeping/API/GetDataRFL',
            type: "GET",
            success: function (response) {
                if (response) {
                    var datas = response.data;

                    datas.forEach((data, index) => {
                        var actionIcons = ``;
                        switch (data.accepted) {
                            case 'False':
                                actionIcons = `<div class="col-md-6 p-0">
                                                    <button onclick="changeStatus('Accepted', ` + data.id_absence + `)" class="btn btn-primary">Accept</button>
                                                </div>
                                                <div class="col-md-6 p-0">
                                                    <button onclick="changeStatus('Rejected', ` + data.id_absence + `)" class="btn btn-danger">Reject</button>
                                                </div>`;
                                break;
                            case 'Accepted':
                                actionIcons = `Accepted`;
                                break;
                            case 'Rejected':
                                actionIcons = `Rejected`;
                                break;
                            default:
                                actionIcons = 'Default';
                        }

                        table.row.add([
                            index + 1,
                            data.id_staff,
                            data.fullName,
                            data.department,
                            data.position,
                            data.absence_from,
                            data.absence_to,
                            data.reason,
                            actionIcons
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

    function filterRFL() {
        // Get data filter
        var statusVal = document.getElementById('status-select').value;
        var depVal = document.getElementById('department-select').value;
        var posVal = document.getElementById('position-select').value;

        // Clear existing table data
        table.clear();

        //Retrieve data and redraw table
        $.ajax({
            url: "/Admin/TimeKeeping/API/FilterRFL",
            type: "GET",
            data: {
                sta: statusVal,
                dep: depVal,
                pos: posVal
            },
            success: function (response) {
                if (response) {
                    var datas = response.data;
                    datas.forEach((data, index) => {
                        var actionIcons = ``;
                        switch (data.accepted) {
                            case 'False':
                                actionIcons = `<div class="col-md-6 p-0">
                                                    <button onclick="changeStatus('Accepted', ` + data.id_absence + `)" class="btn btn-primary">Accept</button>
                                                </div>
                                                <div class="col-md-6 p-0">
                                                    <button onclick="changeStatus('Rejected', ` + data.id_absence + `)" class="btn btn-danger">Reject</button>
                                                </div>`;
                                break;
                            case 'Accepted':
                                actionIcons = `Accepted`;
                                break;
                            case 'Rejected':
                                actionIcons = `Rejected`;
                                break;
                            default:
                                actionIcons = 'Default';
                        }

                        table.row.add([
                            index + 1,
                            data.id_staff,
                            data.fullName,
                            data.department,
                            data.position,
                            data.absence_from,
                            data.absence_to,
                            data.reason,
                            actionIcons
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
    function refreshRFL() {
        window.location.reload();
    }

}
// END ADMIN RQUEST FOR LEAVE
