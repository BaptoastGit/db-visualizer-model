
window.onload = async function () {

    if (window.location.pathname == '/') {
        nouvelleURL = `/TablePages/TableDashboard?TableName=Accounts`;
        window.history.pushState({}, '', nouvelleURL);
    }
    initializeCollapseHandlers();

    enableFilters();
    enlightCurrentTab();

};


function checkRowBox(id) {
    let allCheckboxes = Array.from(document.querySelectorAll('.checkbox-labels.checked'));
    if (allCheckboxes.length == 0) {
        showActions();
    }
    event.target.classList.toggle('checked');
    allCheckboxes = Array.from(document.querySelectorAll('.checkbox-labels.checked'));
    if (allCheckboxes.length == 0) {
        showActions();
    }
    event.target.parentElement.parentElement.parentElement.querySelector('.Id').classList.toggle('selected');
    event.target.closest('tr').classList.toggle('selected');
    document.getElementById(id).click();
}


function showActions() {
    var collapseElementList = [].slice.call(document.querySelectorAll('.collapse#collapseActions'))
    var collapseList = collapseElementList.map(function (collapseEl) {
        return new bootstrap.Collapse(collapseEl)
    })
}

function ShowMoreRows() {
    select = document.getElementById("input-RowCount");
    if (select.selectedIndex < select.options.length - 1) {
        select.selectedIndex += 1;
        select.dispatchEvent(new Event('change'));
    }
}


function SendSelectedFileNames(Action) {
    selectedFiles = Array.from(document.querySelectorAll('.Id.selected'));
    console.log("Selected files for redeposit:", selectedFiles);
    form = document.getElementById('form');
    form.innerHTML += `<input type="hidden" name="ActionName" value="${Action}">`;
    selectedFiles.forEach(file => {
        form.innerHTML += `<input type="hidden" name="selectedFiles" value="${file.innerText}">`;
    });
    console.log("form.innerHTML:", form.innerHTML);
    document.body.appendChild(form);
    form.submit();
}

function selectAllRows() {
    event.target.classList.toggle('checked');
    if (event.target.classList.contains('checked')) {
        allCheckboxes = Array.from(document.querySelectorAll('.checkbox-labels.checked'));
        if (allCheckboxes.length == 0) {
            showActions();
        }
    document.querySelectorAll(".checkbox-labels").forEach(checkbox => {
            if (!checkbox.classList.contains('checked')) {
                checkbox.classList.toggle('checked');
                checkbox.parentElement.parentElement.parentElement.querySelector('.Id').classList.toggle('selected');
                checkbox.closest('tr').classList.toggle('selected');
                document.getElementById('tmp-' + checkbox.getAttribute('row')).classList.add('checked');
                document.getElementById('tmp-' + checkbox.getAttribute('row')).checked = true;

            }
        });


    }
    else {
        document.querySelectorAll(".checkbox-labels").forEach(checkbox => {
            if (checkbox.classList.contains('checked')) {
                allCheckboxes = Array.from(document.querySelectorAll('.checkbox-labels.checked'));
                if (allCheckboxes.length != 0) {
                    showActions();
                }
                document.querySelectorAll(".checkbox-labels").forEach(checkbox => {
                    if (checkbox.classList.contains('checked')) {
                        checkbox.classList.toggle('checked');
                        checkbox.parentElement.parentElement.parentElement.querySelector('.Id').classList.toggle('selected');
                        checkbox.closest('tr').classList.toggle('selected');
                        document.getElementById('tmp-' + checkbox.getAttribute('row')).classList.remove('checked');
                        document.getElementById('tmp-' + checkbox.getAttribute('row')).checked = false;

                    }
                });            }
        });
    }
    document.getElementById("tmp-a").click();
}
