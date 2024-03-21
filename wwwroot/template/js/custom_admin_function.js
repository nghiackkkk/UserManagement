function checkedRow(td) {

    var row = td.parentNode;
    var checkbox = row.querySelector('input[type="checkbox"]');

    checkbox.checked = !checkbox.checked;

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
