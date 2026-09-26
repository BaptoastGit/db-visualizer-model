function checkBoxClickHandler(column, value) {
    const checkbox = document.getElementById(`checkbox-item-${value}`);
    const checkboxValueInput = document.getElementById(`checkbox-value-${value}`);
    const allCheckboxes = Array.from(document.querySelectorAll('.checkboxitem'));


    checkbox.classList.toggle('feather-check');
    if (checkbox.classList.contains('feather-check')) {
        checkboxValueInput.value = value;
    }
    else {
        checkboxValueInput.value = "";
    }
    if (allCheckboxes.every(checkbox => checkbox.classList.contains('feather-check'))) {
        document.getElementById(`checkbox-item-all-${column}`).classList.add('feather-check');
    }
    else {
        document.getElementById(`checkbox-item-all-${column}`).classList.remove('feather-check');

    }
    document.getElementById("ResetFiltersButton").style.display = "flex";
}


function selectAllOptions(column){
    document.querySelectorAll(`.checkboxitem.${column}`).forEach(function (checkbox) {
        if (document.getElementById(`checkbox-item-all-${column}`).classList.contains('feather-check')) {
            checkbox.classList.remove('feather-check');
            document.getElementById('checkbox-value-' + checkbox.id.split('-').pop()).value = "";

        }

        else {
            checkbox.classList.add('feather-check');
            document.getElementById('checkbox-value-' + checkbox.id.split('-').pop()).value = checkbox.id.split('-').pop();
        }
    });
    document.getElementById(`checkbox-item-all-${column}`).classList.toggle('feather-check');


}

function closeSelectMenu(event) {
    if (Array.from(document.querySelectorAll('.collapse')).every(collapse => !collapse.contains(event.target))) {
        var collapseElementList = [].slice.call(document.querySelectorAll('.collapse.show:not(#collapseActions, .collapseNavBar)'))
        var collapseList = collapseElementList.map(function (collapseEl) {
            return new bootstrap.Collapse(collapseEl)
        })
    }
}


