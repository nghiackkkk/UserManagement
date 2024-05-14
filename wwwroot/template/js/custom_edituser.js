function revealPassword() {
    var input = document.getElementById('inputPassword');
    if (input.type === 'password') {
        input.type = 'text';
    } else {
        input.type = 'password';
    }
}

// Count and add attribute name for field study process
function addStudyProcessU() {
    var studyingPr = document.getElementsByClassName('count-studying');
    var countStu = studyingPr.length;

    for (let i = 0; i < countStu; i++) {
        var field = studyingPr[i];
        var startTime = field.querySelector('.st-start-time');
        var endTime = field.querySelector('.st-end-time');
        var school = field.querySelector('.st-school');
        var mode = field.querySelector('.st-mode-study');

        startTime.setAttribute('name', 'StudyProcesses[' + i + '].StartTime');
        endTime.setAttribute('name', 'StudyProcesses[' + i + '].EndTime');
        school.setAttribute('name', 'StudyProcesses[' + i + '].SchoolUniversity');
        mode.setAttribute('name', 'StudyProcesses[' + i + '].ModeOfStudy');
    }
}

// Count and add attribute name for field working process
function addWorkingProcessU() {
    var workingPr = document.getElementsByClassName('count-working');
    var countWork = workingPr.length;

    for (let i = 0; i < countWork; i++) {
        var field = workingPr[i];
        var startTime = field.querySelector('.w-start-time');
        var endTime = field.querySelector('.w-end-time');
        var work = field.querySelector('.w-work');
        var pos = field.querySelector('.w-position');

        startTime.setAttribute('name', 'WorkingProcesses[' + i + '].StartTime');
        endTime.setAttribute('name', 'WorkingProcesses[' + i + '].EndTime');
        work.setAttribute('name', 'WorkingProcesses[' + i + '].WorkingAgency');
        pos.setAttribute('name', 'WorkingProcesses[' + i + '].Position');

    }
}

// Count and add attribute name for field siblings
function addSilbingsU() {
    var sibling = document.getElementsByClassName('each-sibling');
    var countS = sibling.length;

    for (let i = 0; i < countS; i++) {
        var field = sibling[i];
        var startSiblings = i;
        var name = field.querySelector('.s-name');
        var yob = field.querySelector('.s-yob');
        var cr = field.querySelector('.s-cr');
        var co = field.querySelector('.s-co');
        var wa = field.querySelector('.s-wa');
        var rel = field.querySelector('.s-rel');

        name.setAttribute('name', 'Siblings[' + startSiblings + '].FullName');
        yob.setAttribute('name', 'Siblings[' + startSiblings + '].YearOfBirth');
        cr.setAttribute('name', 'Siblings[' + startSiblings + '].CurrentResident');
        co.setAttribute('name', 'Siblings[' + startSiblings + '].CurrentOcupation');
        wa.setAttribute('name', 'Siblings[' + startSiblings + '].WorkingAgency');
        rel.setAttribute('name', 'Siblings[' + startSiblings + '].Realtionship');

    }
}

// Check terms and condition
function checkTermsCondU() {
    var btnSubmit = document.getElementById('btn-submit');
    var checkbox = document.getElementById("checkTC");
    if (checkbox.checked) {
        if (!validateSiblings() || !validateField()) {
            btnSubmit.disabled = true;
            checkbox.checked = false;
        } else {
            // CHeck field have add button
            addStudyProcessU();
            addWorkingProcessU();
            addSilbingsU();
            btnSubmit.disabled = false;
            checkbox.checked = true;
        }
    } else {
        console.log("Checkbox is unchecked");
        btnSubmit.disabled = true;
        checkbox.checked = false;
    }
}

// Validate input typt text
function validateInputText(idInput) {
    var input = document.getElementById(idInput);
    if (input.value.trim() == '') {
        input.classList.add('is-invalid');
        input.focus();
        return false;
    }
    input.classList.remove('is-invalid');
    return true;
}
// Validate Username
function validateUsernameU() {
    var usernameField = document.getElementById('username');
    var value = usernameField.value.trim();
    var url = window.location.href;
    var id = parseInt(url.split('id=')[1]);
    if (isNaN(id)) {
        id = $('#idUser').val();
    }
    if (value != '') {
        $.ajax({
            url: "/Admin/CheckUsernameU",
            type: "GET",
            data: {
                username: value,
                id: id
            },
            success: function (response) {
                if (response.result == false) {
                    // If username exist
                    usernameField.classList.add('is-invalid');
                    document.getElementById('btn-submit').disabled = true;
                } else {
                    usernameField.classList.remove('is-invalid');
                    document.getElementById('btn-submit').disabled = false;
                }
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    }
}

// Validate password
function validatePassword() {
    var regex = /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*]).{3,20}$/;
    var errorField = document.getElementById('inputPassword');
    var newValue = errorField.value;

    if (!regex.test(newValue)) {
        errorField.classList.add('is-invalid');
        document.getElementById('btn-submit').disabled = true;
        if ($('#checkTC').length > 0) {
            document.getElementById("checkTC").checked = false;
        }
    } else {
        errorField.classList.remove('is-invalid');
    }
}

// Validate field
function validateField() {
    var isValid = true;

    isValid &= validateInputText('raAddress');
    isValid &= validateInputText('raCommune');
    isValid &= validateInputText('raDistrict');
    isValid &= validateInputText('raCity');

    isValid &= validateInputText('prAddress');
    isValid &= validateInputText('prCommune');
    isValid &= validateInputText('prDistrict');
    isValid &= validateInputText('prCity');

    isValid &= validateInputText('cultrulaStarndard');
    isValid &= validateInputText('idCard');
    isValid &= validateInputText('regilion');
    isValid &= validateInputText('ethnicGroup');
    isValid &= validateInputText('phoneNumber');
    isValid &= validateInputText('dob');
    isValid &= validateInputText('fullname');

    if (document.getElementById('inputPassword').classList.contains('is-invalid')) {
        isValid = false;
        document.getElementById('inputPassword').focus();
    }

    if (document.getElementById('username').classList.contains('is-invalid')) {
        isValid = false;
        document.getElementById('username').focus();
    }


    return isValid;
}


function validateSiblings() {
    var isValid = true;
    var startTime = document.querySelectorAll('.s-valid');
    startTime.forEach(function (st) {
        if (st.value == '') {
            st.classList.add('is-invalid');
            st.focus();
            isValid = false;
        }
    });

    return isValid
}