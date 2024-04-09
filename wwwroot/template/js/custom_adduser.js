function uncheckTC() {
    // Uncheck terms and condition checkbox
    var checkbox = document.getElementById("checkTC");
    checkbox.checked = false;
}
function addListProcess(type) {
    uncheckTC();
    switch (type) {
        case ('working'):
            var workingField = document.getElementById('workingProcess');
            var formAdd = document.createElement('div');
            formAdd.classList.add('row');
            formAdd.classList.add('mb-3');
            formAdd.classList.add('count-working');
            formAdd.innerHTML = `<div class="col-md-2">
                                    <label class="form-label" for="siblings">Start time</label>
                                    <input type="date" value=""
                                    class="form-control w-start-time" id="" placeholder="01230123012033">
                                    <div class="invalid-feedback">
                                        Field required.
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <label class="form-label" for="siblings">End time</label>
                                    <input type="date" value=""
                                            class="form-control w-end-time" id="" placeholder="01230123012033">
                                    <div class="invalid-feedback">
                                        Field required.
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <label class="form-label" for="siblings">Working Agency</label>
                                    <input type="text" value=""
                                            class="form-control w-work" id="" placeholder="e.g ABC Company">
                                    <div class="invalid-feedback">
                                        Field required.
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <label class="form-label" for="siblings">Position</label>
                                    <input type="text" value=""
                                            class="form-control w-position" id="" placeholder="e.g CEO">
                                    <div class="invalid-feedback">
                                        Field required.
                                    </div>
                                </div>
                                <div class="col-md-1 d-flex flex-column justify-content-end align-items-center">
                                    <div class="btn btn-danger" onclick="removeElementProcess(this, 'process')">
                                        <i class="fa fa-solid fa-minus-circle"></i>
                                    </div>
                                </div>`;
            workingField.appendChild(formAdd);
            break;
        case ('studying'):
            // Count siblings fields
            var countStu = document.getElementsByClassName('count-studying').length;
            var nextStu = countStu;

            // Insert field below
            var workingField = document.getElementById('studyProcess');
            var formAdd = document.createElement('div');
            formAdd.classList.add('row');
            formAdd.classList.add('mb-3');
            formAdd.classList.add('count-studying');
            formAdd.innerHTML = `<div class="col-md-2">
                                    <label class="form-label" for="siblings">Start time</label>
                                    <input type="date" value=""
                                    class="form-control st-start-time" id="" placeholder="01230123012033">
                                    <div class="invalid-feedback">
                                        Field required.
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <label class="form-label" for="siblings">End time</label>
                                    <input type="date" value=""
                                    class="form-control st-end-time" id="" placeholder="01230123012033">
                                    <div class="invalid-feedback">
                                        Field required.
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <label class="form-label" for="siblings">School/University</label>
                                    <input type="text" value=""
                                    class="form-control st-school" id="" placeholder="e.g ABC Company">
                                    <div class="invalid-feedback">
                                        Field required.
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <label class="form-label" for="siblings">Mode of study</label>
                                    <input type="text" value=""
                                    class="form-control st-mode-study" id="" placeholder="e.g CEO">
                                    <div class="invalid-feedback">
                                        Field required.
                                    </div>
                                </div>
                                <div class="col-md-1 d-flex flex-column justify-content-end align-items-center">
                                    <div class="btn btn-danger" onclick="removeElementProcess(this, 'process')">
                                        <i class="fa fa-solid fa-minus-circle"></i>
                                    </div>
                                </div>`;
            workingField.appendChild(formAdd);
            break;
        case ('siblings'):
            var workingField = document.getElementById('siblingsInformation');
            var formAdd = document.createElement('div');
            formAdd.classList.add('each-sibling');

            formAdd.innerHTML = `<input type="hidden" value="Sibling" class="form-control s-rel" id="">

                                                <div class="row mb-3">
                                                    <div class="col-md-4">
                                                        <label class="form-label" for="partnerName">Name</label>
                                                        <input onblur="blurFieldRequired(this)" value="" type="text" class="form-control s-name" id="" placeholder="e.g Alisa Crown">
                                                        <div class="invalid-feedback">
                                                            Field required.
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <label class="form-label" for="partnerYoB">Year of birth</label>
                                                        <input onblur="blurFieldRequired(this)" value="" type="text" class="form-control s-yob" id="" placeholder="e.g 1975">
                                                        <div class="invalid-feedback">
                                                            Field required.
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="form-label" for="partnerCurrentResident">Current Resident</label>
                                                        <input onblur="blurFieldRequired(this)" 
                                                            value="" type="text" class="form-control s-cr" id="" placeholder="e.g 123 That St, London, England">
                                                        <div class="invalid-feedback">
                                                            Field required.
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1 d-flex flex-column justify-content-end align-items-center">
                                                        <div class="btn btn-danger" onclick="removeElementProcess(this, 'sibling')">
                                                            <i class="fa fa-solid fa-minus-circle"></i>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="col-md-6">
                                                        <label class="form-label" for="partnerCurrentResident">Current Occupation</label>
                                                        <input onblur="blurFieldRequired(this)" value="" type="text" class="form-control s-co" id="" placeholder="e.g CEO">
                                                        <div class="invalid-feedback">
                                                            Field required.
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="form-label" for="partnerCurrentResident">Working Agency</label>
                                                        <input onblur="blurFieldRequired(this)" value="" type="text" class="form-control s-wa" id="" placeholder="e.g ABC Company">
                                                        <div class="invalid-feedback">
                                                            Field required.
                                                        </div>
                                                    </div>
                                                </div>`;
            workingField.appendChild(formAdd);
            break;
    }
}
function removeElementProcess(element, type) {
    uncheckTC();
    switch (type) {
        case 'sibling':
            var parentnode = element.parentNode.parentNode.parentNode;
            parentnode.remove();
            break;
        case 'process':
            var parentnode = element.parentNode.parentNode;
            parentnode.remove();
            break;
        case 'working':
            var parentnode = element.parentNode.parentNode;
            parentnode.remove();
            break;
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

// Count and add attribute name for field study process
function addStudyProcess() {
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
function addWorkingProcess() {
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
function addSilbings() {
    var sibling = document.getElementsByClassName('each-sibling');
    var countS = sibling.length;

    for (let i = 0; i < countS; i++) {
        var field = sibling[i];
        var startSiblings = i + 3;
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
function checkTermsCond() {
    var btnSubmit = document.getElementById('btn-submit');
    var checkbox = document.getElementById("checkTC");
    if (checkbox.checked) {
        if (!validateSiblings() || !validateField()) {
            btnSubmit.disabled = true;
            checkbox.checked = false;
        } else {
            // CHeck field have add button
            addStudyProcess();
            addWorkingProcess();
            addSilbings();
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
function validateUsername() {
    var usernameField = document.getElementById('username');
    var value = usernameField.value.trim();
    if (value != '') {
        $.ajax({
            url: "/Admin/CheckUsername",
            type: "GET",
            data: {
                username: value,
            },
            success: function(response) {
                if (response.result == true) {
                    // If username exist
                    usernameField.classList.add('is-invalid');
                } else {
                    usernameField.classList.remove('is-invalid');

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
        document.getElementById("checkTC").checked = false;
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

    var img = document.getElementById('imgInp');
    if (img.files.length == 0) {
        img.classList.add('is-invalid');
        img.focus();
        isValid = false;
    } else {
        img.classList.remove('is-invalid');

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