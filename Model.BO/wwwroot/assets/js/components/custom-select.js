function checkBoxClickHandler(column, value) {
    const checkbox = document.getElementById(`checkbox-item-${value}`);
    const checkboxValueInput = document.getElementById(`checkbox-value-${value}`);
    if (!checkbox || !checkboxValueInput) return;

    const isChecked = checkbox.classList.toggle('feather-check');
    checkboxValueInput.value = isChecked ? value : "";

    const columnCheckboxes = Array.from(document.querySelectorAll(`.checkboxitem.${column}`));
    const allChecked = columnCheckboxes.every(cb => cb.classList.contains('feather-check'));

    const selectAllCheckbox = document.getElementById(`checkbox-item-all-${column}`);
    if (selectAllCheckbox) {
        if (allChecked) {
            selectAllCheckbox.classList.add('feather-check');
        } else {
            selectAllCheckbox.classList.remove('feather-check');
        }
    }

    const resetBtn = document.getElementById("ResetFiltersButton");
    if (resetBtn) resetBtn.style.display = "flex";
}


function selectAllOptions(column) {
    const selectAllCheckbox = document.getElementById(`checkbox-item-all-${column}`);
    if (!selectAllCheckbox) return;

    const isCurrentlyChecked = selectAllCheckbox.classList.contains('feather-check');
    const targetState = !isCurrentlyChecked; 

    document.querySelectorAll(`.checkboxitem.${column}`).forEach(checkbox => {
        const optionId = checkbox.id.split('-').pop();
        const valueInput = document.getElementById(`checkbox-value-${optionId}`);

        if (targetState) {
            checkbox.classList.add('feather-check');
            if (valueInput) valueInput.value = optionId;
        } else {
            checkbox.classList.remove('feather-check');
            if (valueInput) valueInput.value = "";
        }
    });

    selectAllCheckbox.classList.toggle('feather-check');

    const resetBtn = document.getElementById("ResetFiltersButton");
    if (resetBtn) resetBtn.style.display = "flex";
}


function closeSelectMenu(event) {
    const clickedInsideCollapse = event.target.closest('.collapse');

    if (!clickedInsideCollapse) {
        const openMenus = document.querySelectorAll('.collapse.show:not(#collapseActions):not(.collapseNavBar)');

        openMenus.forEach(menu => {
            const bsCollapse = bootstrap.Collapse.getInstance(menu) || new bootstrap.Collapse(menu, { toggle: false });
            bsCollapse.hide();
        });
    }
}