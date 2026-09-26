function enableFilters() {
    const urlParams = new URLSearchParams(window.location.search);
    if (urlParams.size > 1) {
        loadFilters(urlParams);
    } else if (sessionStorage.getItem("SortFormData" + document.body.dataset.pageTitle)) {
        reloadPageWithSavedFilters()
    } else {
        initializeInputValues();
    }

    //Add the listener on the search button to save FilteredInputs
    saveFilterOptions();

    displayResetButtonAfterUserChanges();
    document.addEventListener('mousedown', (event) => closeSelectMenu(event));
}



function ResetFilters() {
    sessionStorage.setItem("SortFormData" + document.body.dataset.pageTitle, "");
    window.location.href = window.location.pathname;

}

function loadFilters(urlParams) {
    const urlParamsGrouped = Object.fromEntries(
        [...new Set(urlParams.keys())].map(key => [key, urlParams.getAll(key)])
    );
    for (const key in urlParamsGrouped) {
        if (key != "SortBy" && key != "Order" && key != "Success" && key != "success") {
            const date = new Date(urlParamsGrouped[key][0]);
            if (urlParamsGrouped[key].length == 1) {
                if (urlParamsGrouped[key][0] != "") {
                    if (document.getElementById("filter-icon-" + key)) document.getElementById("filter-icon-" + key).classList.add("text-danger");
                    if (document.getElementById("input-" + key)) document.getElementById("input-" + key).value = urlParamsGrouped[key][0];
                    else {
                        document.querySelectorAll(`.checkboxitem.${key}`).forEach(checkbox => {
                            if (urlParamsGrouped[key].includes(checkbox.id.replace("checkbox-item-", ""))) {
                                checkbox.classList.add('feather-check');
                                document.getElementById('checkbox-value-' + checkbox.getAttribute("optionId")).value = checkbox.getAttribute("optionId");

                            }
                        })
                    }
                    document.getElementById('ResetFiltersButton').style.display = "flex";

                }

            }
            else if (urlParamsGrouped[key][0] == "eq" || urlParamsGrouped[key][0] == "lt" || urlParamsGrouped[key][0] == "gt") {
                if ((urlParamsGrouped[key][0] != "gt" || urlParamsGrouped[key][1] != "0")) {
                    showResetButton(key);
                    document.getElementById("input-operator-" + key).value = urlParamsGrouped[key][0];
                    document.getElementById("input-" + key).value = urlParamsGrouped[key][1];
                }

            }
            else if (isNaN(Number(urlParamsGrouped[key][0])) && !isNaN(date.getTime())) {

                document.getElementById("input-start-" + key).value = urlParamsGrouped[key][0];
                document.getElementById("input-end-" + key).value = urlParamsGrouped[key][1];

                if (document.getElementById("input-start-" + key).value != "2000-01-01T00:00" || (document.getElementById("input-end-" + key).value != "9999-12-31T00:00")) {
                    showResetButton(key);
                }
            }
            else {
                document.querySelectorAll(`.checkboxitem.${key}`).forEach(checkbox => {
                    if (urlParamsGrouped[key].includes(checkbox.id.replace("checkbox-item-", ""))) {
                        checkbox.classList.add('feather-check');
                        document.getElementById('checkbox-value-' + checkbox.getAttribute("optionId")).value = checkbox.getAttribute("optionId");

                    }
                })
                if (Array.from(document.querySelectorAll(`.checkboxitem.${key}`)).every(checkbox => checkbox.classList.contains('feather-check'))) {
                    document.getElementById(`checkbox-item-all-${key}`).classList.add('feather-check');
                }
                else {
                    showResetButton(key)

                }

            }

        }

    }
}


function showResetButton(key) {
    document.getElementById("filter-icon-" + key).classList.add("text-danger");
    document.getElementById('ResetFiltersButton').style.display = "flex";
}




function initializeInputValues(resetButton, mainCheckbox = "") {
    let currentDateTime = new Date();
    document.querySelectorAll('.date-start').forEach(el => el.value = "2000-01-01T00:00");
    document.querySelectorAll('.date-end').forEach(el => el.value = "9999-12-31T00:00");
    document.querySelectorAll('.date-now').forEach(el => el.value = currentDateTime.toISOString().slice(0, 16));
    document.querySelectorAll('.date-months-before').forEach(el => {
        let tempDateTime = new Date()
        tempDateTime.setMonth(currentDateTime.getMonth() - el.getAttribute("monthsBefore"));
        el.value = tempDateTime.toISOString().slice(0, 16)
    });

    currentDateTime.setMonth(currentDateTime.getMonth() - 1);
    document.querySelectorAll('.date-month-before').forEach(el => el.value = currentDateTime.toISOString().slice(0, 16));


    document.querySelectorAll('.checkboxitem').forEach(function (checkbox) {
        checkbox.classList.add('feather-check');
        document.getElementById('checkbox-value-' + checkbox.id.split('-').pop()).value = checkbox.id.split('-').pop();
    });

    document.querySelectorAll('.checkboxitem-all ').forEach(function (checkbox) {
        checkbox.classList.add('feather-check');
    });


}


function reloadPageWithSavedFilters() {
    const formData = JSON.parse(sessionStorage.getItem("SortFormData" + document.body.dataset.pageTitle));
    tempForm = document.createElement("form");
    tempForm.id = "tempForm";
    tempForm.innerHTML += `
     <input type="hidden" name="SortBy" value='${document.getElementById("SortBy").value}'/>
      <input type="hidden" name="Order" value='${document.getElementById("Order").value}'/>
    `
    for (const [key, value] of Object.entries(formData)) {
        try {
            const values = JSON.parse(value);
            for (const [subkey, value] of Object.entries(values)) {
                tempForm.innerHTML += `
                <input type="hidden" name='${key}' value='${value}'/>
                `
            }
            tempForm.innerHTML += `
                <input type="hidden" name='${key}' value=''/>
                <input type="hidden" name='${key}' value=''/>
                `
        } catch {
            tempForm.innerHTML += `<input type="hidden" name='${key}' value='${value}'/>`
        }


    }
    let urlParams = new URLSearchParams(window.location.search);
    if (urlParams.get("success")) {
        tempForm.innerHTML += `<input type="hidden" name="success" value='${urlParams.get("success")}'/>`

    }
    document.body.appendChild(tempForm);
    form = new FormData(tempForm);
    const userParams = new URLSearchParams(form);
    const newUrl = `${window.location.pathname}?${userParams.toString()}`;
    window.history.pushState({}, "", newUrl);

    urlParams = new URLSearchParams(window.location.search);
    loadFilters(urlParams);

    ReloadTableOnly();
    tempForm.remove();
}


function displayResetButtonAfterUserChanges() {
    document.querySelectorAll(".userInput").forEach(input => {
        input.addEventListener("change", function () {
            sessionStorage.setItem("ShowFilters" + document.body.dataset.pageTitle, "true");
            document.getElementById("ResetFiltersButton").style.display = "flex";
        })
    });
}


function saveFilterOptions() {
    document.getElementById("SortForm").addEventListener("submit", function (event) {
        event.preventDefault();
        const inputsData = {};

        multiplesInputsFilters = document.querySelectorAll(".multiple-inputs");
        Array.from(multiplesInputsFilters).forEach(multipleInput => {
            options = Array.from(document.querySelectorAll("[name='" + multipleInput.id.replace("checkbox-all-", "") + "'].feather-check")).map(el => el.getAttribute("optionId"));
            inputsData[multipleInput.id.replace("checkbox-all-", "")] = JSON.stringify(options);
        })

        dateTimeInputsFilters = document.querySelectorAll(".userInput.date-start:not(.globalFilter)");
        Array.from(dateTimeInputsFilters).forEach(dateInput => {
            options = Array.from(document.querySelectorAll("[name='" + dateInput.id.replace("input-start-", "") + "']")).map(el => el.value);
            inputsData[dateInput.id.replace("input-start-", "")] = JSON.stringify(options);
        })

        numberInputsFilters = document.querySelectorAll(".userInput.operator:not(.globalFilter)");
        Array.from(numberInputsFilters).forEach(numberInput => {

            options = Array.from(document.querySelectorAll("[name='" + numberInput.getAttribute("name") + "']")).map(el => el.value);
            inputsData[numberInput.getAttribute("name")] = JSON.stringify(options);
        })

        const inputs = document.querySelectorAll(`.userInput:not(.date-start):not(.date-end):not(.globalFilter):not(.operator, .number)`);
        inputs.forEach((input) => {
            inputsData[input.id.replace("input-", "").replace("operator-", "")] = input.value;
        })

        const globalInputs = document.querySelectorAll(`.globalFilter`);
        globalInputs.forEach((input) => {
            inputsData[input.id] = input.value;
        })

        sessionStorage.setItem("Reloaded", "true");
        sessionStorage.setItem("SortFormData" + document.body.dataset.pageTitle, JSON.stringify(inputsData));

        const form = new FormData(document.getElementById("SortForm"));
        const userParams = new URLSearchParams(form);
        const newUrl = `${window.location.pathname}?${userParams.toString()}`;
        window.history.pushState({}, "", newUrl);

        const urlParams = new URLSearchParams(window.location.search);
        loadFilters(urlParams);

        ReloadTableOnly();
    });
}
