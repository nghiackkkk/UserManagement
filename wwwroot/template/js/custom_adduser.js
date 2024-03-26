function addListProcess(type) {
    switch (type) {
        case ('working'):
            var workingField = document.getElementById('workingProcess');
            var formAdd = document.createElement('div');
            formAdd.classList.add('row');
            formAdd.classList.add('mb-3');
            formAdd.innerHTML = ` <div class="col-md-2">
                                                    <label class="form-label" for="siblings">Start time</label>
                                                    <input type="date" class="form-control" id="dateHCMY" placeholder="01230123012033">
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="form-label" for="siblings">End time</label>
                                                    <input type="date" class="form-control" id="dateHCMY" placeholder="01230123012033">
                                                </div>
                                                <div class="col-md-5">
                                                    <label class="form-label" for="siblings">Working Agency</label>
                                                    <input type="text" class="form-control" id="partnerCurrentResident" placeholder="e.g ABC Company">
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="form-label" for="siblings">Position</label>
                                                    <input type="text" class="form-control" id="partnerCurrentResident" placeholder="e.g CEO">
                                                </div>
                                                <div class="col-md-1 d-flex flex-column justify-content-end align-items-center">
                                                    <div class="btn btn-danger" onclick="removeElementProcess(this, 'working')">
                                                        <i class="fa fa-solid fa-minus-circle"></i>
                                                    </div>
                                                </div>`
            workingField.appendChild(formAdd);
            break;
        case ('studying'):
            var workingField = document.getElementById('studyProcess');
            var formAdd = document.createElement('div');
            formAdd.classList.add('row');
            formAdd.classList.add('mb-3');
            formAdd.innerHTML = `<div class="col-md-2">
                                                    <label class="form-label" for="siblings">Start time</label>
                                                    <input type="date" class="form-control" id="dateHCMY" placeholder="01230123012033">
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="form-label" for="siblings">End time</label>
                                                    <input type="date" class="form-control" id="dateHCMY" placeholder="01230123012033">
                                                </div>
                                                <div class="col-md-5">
                                                    <label class="form-label" for="siblings">School/University</label>
                                                    <input type="text" class="form-control" id="partnerCurrentResident" placeholder="e.g ABC Company">
                                                </div>
                                                <div class="col-md-2">
                                                    <label class="form-label" for="siblings">Mode of study</label>
                                                    <input type="text" class="form-control" id="partnerCurrentResident" placeholder="e.g CEO">
                                                </div>
                                                <div class="col-md-1 d-flex flex-column justify-content-end align-items-center">
                                                    <div class="btn btn-danger" onclick="removeElementProcess(this, 'process')">
                                                        <i class="fa fa-solid fa-minus-circle"></i>
                                                    </div>
                                                </div>`
            workingField.appendChild(formAdd);
            break;
        case ('siblings'):
            var workingField = document.getElementById('siblingsInformation');
            var formAdd = document.createElement('div');
            formAdd.classList.add('eachSibling');

            formAdd.innerHTML = `<div class="row mb-3">
                                    <div class="col-md-4">
                                        <label class="form-label" for="partnerName">Name</label>
                                        <input type="text" class="form-control" id="partnerName" placeholder="e.g Alisa Crown">
                                    </div>
                                    <div class="col-md-1">
                                        <label class="form-label" for="partnerYoB">Year of birth</label>
                                        <input type="text" class="form-control" id="partnerYoB" placeholder="e.g 1975">
                                    </div>
                                    <div class="col-md-6">
                                        <label class="form-label" for="partnerCurrentResident">Current Resident</label>
                                        <input type="text" class="form-control" id="partnerCurrentResident" placeholder="e.g 123 That St, London, England">
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
                                        <input type="text" class="form-control" id="partnerCurrentResident" placeholder="e.g CEO">
                                    </div>
                                    <div class="col-md-6">
                                        <label class="form-label" for="partnerCurrentResident">Working Agency</label>
                                        <input type="text" class="form-control" id="partnerCurrentResident" placeholder="e.g ABC Company">
                                    </div>
                                </div>`;
            workingField.appendChild(formAdd);
            break;
    }
}
function removeElementProcess(element, type) {
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