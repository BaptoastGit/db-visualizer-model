function enableFilters() {
    const urlParams = new URLSearchParams(window.location.search);
    if (urlParams.size > 1) {
        loadFilters(urlParams);
    } else if (sessionStorage.getItem("SortFormData" + document.body.dataset.pageTitle)) {
        reloadPageWithSavedFilters()
    } else {
        initializeInputValues();
    }

    //Adds the listener on the search button to save FilteredInputs
    saveFilterOptions();

    displayResetButtonAfterUserChanges();
    document.addEventListener('mousedown', (event) => closeSelectMenu(event));
}



function ResetFilters() {
    sessionStorage.setItem("SortFormData" + document.body.dataset.pageTitle, "");
    window.location.href = window.location.pathname + "?TableName=" + document.body.dataset.pageTitle;

}

const IGNORED_PARAMS = new Set(["SortBy", "Order", "Success", "success"]);

function loadFilters(urlParams) {
    const urlParamsGrouped = Object.fromEntries(
        [...new Set(urlParams.keys())].map(key => [key, urlParams.getAll(key)])
    );

    for (const [key, values] of Object.entries(urlParamsGrouped)) {
        if (IGNORED_PARAMS.has(key) || !values || values.length === 0 || values[0] === "") {
            continue;
        }

        applyFilterLogic(key, values);
    }
}

function applyFilterLogic(key, values) {
    const val0 = values[0];

    if (["eq", "lt", "gt"].includes(val0)) {
        handleOperatorFilter(key, values);
        return;
    }

    if (isDateString(val0)) {
        handleDateFilter(key, values);
        return;
    }

    const textInput = document.getElementById(`input-${key}`);
    if (values.length === 1 && textInput) {
        handleStandardInput(key, val0, textInput);
        return;
    }

    handleCheckboxFilter(key, values);
}

function isDateString(value) {
    return isNaN(Number(value)) && !isNaN(new Date(value).getTime());
}

function handleOperatorFilter(key, values) {
    const operator = values[0];
    const numberValue = values[1];

    if (operator === "gt" && numberValue === "0") return;

    const opInput = document.getElementById(`input-operator-${key}`);
    const valInput = document.getElementById(`input-${key}`);

    if (opInput) opInput.value = operator;
    if (valInput) valInput.value = numberValue;

    showResetButton(key);
}

function handleDateFilter(key, values) {
    const startInput = document.getElementById(`input-start-${key}`);
    const endInput = document.getElementById(`input-end-${key}`);

    if (startInput) startInput.value = values[0];
    if (endInput) endInput.value = values[1];

    const startVal = startInput ? startInput.value : "";
    const endVal = endInput ? endInput.value : "";

    if (startVal !== "2000-01-01T00:00" || endVal !== "9999-12-31T00:00") {
        showResetButton(key);
    }
}

function handleStandardInput(key, value, textInput) {
    textInput.value = value;
    showResetButton(key);
}

function handleCheckboxFilter(key, values) {
    const checkboxes = document.querySelectorAll(`.checkboxitem.${key}`);
    if (checkboxes.length === 0) return;

    let allChecked = true;

    checkboxes.forEach(checkbox => {
        const optionId = checkbox.id.replace("checkbox-item-", "");

        if (values.includes(optionId)) {
            checkbox.classList.add("feather-check");

            const hiddenInput = document.getElementById(`checkbox-value-${checkbox.getAttribute("optionId")}`);
            if (hiddenInput) {
                hiddenInput.value = checkbox.getAttribute("optionId");
            }
        } else {
            allChecked = false;
        }
    });

    const allCheckbox = document.getElementById(`checkbox-item-all-${key}`);

    if (allChecked && allCheckbox) {
        allCheckbox.classList.add("feather-check");
    } else {
        showResetButton(key);
    }
}

function showResetButton(key) {
    document.getElementById("filter-icon-" + key).classList.add("text-danger");
    document.getElementById('ResetFiltersButton').style.display = "flex";
}

function initializeInputValues() {
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
    const formDataJson = sessionStorage.getItem("SortFormData" + document.body.dataset.pageTitle);
    if (!formDataJson) return;

    const formData = JSON.parse(formDataJson);
    const userParams = new URLSearchParams();

    userParams.append("SortBy", document.getElementById("SortBy")?.value || "");
    userParams.append("Order", document.getElementById("Order")?.value || "");

    const currentUrlParams = new URLSearchParams(window.location.search);
    if (currentUrlParams.has("success")) {
        userParams.append("success", currentUrlParams.get("success"));
    }

    for (const [key, value] of Object.entries(formData)) {
        try {
            const valuesArray = JSON.parse(value);
            if (Array.isArray(valuesArray)) {
                valuesArray.forEach(val => userParams.append(key, val));
                userParams.append(key, '');
                userParams.append(key, '');
            } else {
                userParams.append(key, value);
            }
        } catch {
            userParams.append(key, value);
        }
    }

    const newUrl = `${window.location.pathname}?${userParams.toString()}`;
    window.history.pushState({}, "", newUrl);

    loadFilters(new URLSearchParams(window.location.search));
    ReloadTableOnly();
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

        const form = document.getElementById("SortForm");
        const formData = new FormData(form);
        const inputsData = {};

        for (const [key, value] of formData.entries()) {
            if (inputsData[key]) {
                if (!Array.isArray(inputsData[key])) {
                    inputsData[key] = [inputsData[key]];
                }
                inputsData[key].push(value);
            } else {
                inputsData[key] = value;
            }
        }

        for (const key in inputsData) {
            if (Array.isArray(inputsData[key])) {
                inputsData[key] = JSON.stringify(inputsData[key]);
            }
        }
        sessionStorage.setItem("SortFormData" + document.body.dataset.pageTitle, JSON.stringify(inputsData));

        const userParams = new URLSearchParams(formData);
        const newUrl = `${window.location.pathname}?${userParams.toString()}`;
        window.history.pushState({}, "", newUrl);

        loadFilters(new URLSearchParams(window.location.search));
        ReloadTableOnly();
    });
}