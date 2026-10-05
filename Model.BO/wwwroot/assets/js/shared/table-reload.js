
async function ReloadTableOnly() {
    let formData = new FormData(document.getElementById("SortForm"));

    if (document.getElementById("tempForm")) {
        formData = new FormData(document.getElementById("tempForm"));
    }
    const params = new URLSearchParams(formData);
    params.set("handler", "TableData");

    try {
        window.contentLoaded = false;
        document.querySelectorAll("button").forEach(btn => btn.disabled = true)
        showLoadingIcon();
        const response = await fetch(`?${params.toString()}`, {
            headers: { "X-Requested-With": "XMLHttpRequest" }
        });
        if (!response.ok) throw new Error("Erreur réseau");
        const html = await response.text();
        document.getElementById("mainTableBody").innerHTML = html;
        window.contentLoaded = true;
        console.log("Réussi");
        ShowOrderedRow()
        updateRowCounter();
        if (window.editableRights) {
            console.log(window.editableColumns);
            makeColumnEditable(window.editableColumns)
        }
        if (window.adminRights) {
            deletableRows()
        }
        


    } catch (err) {
        console.error("Erreur lors de la mise à jour du tableau");
    } finally {
        document.querySelectorAll("button").forEach(btn => btn.disabled = false)
        document.getElementById("mainTableBody").style.opacity = 1;
    }
}


function replaceStatusNumbers(data, jsonData, dataType) {
    const statesElements = document.getElementsByClassName(dataType);
    Array.from(statesElements).forEach(stateElement => {
        for (const state of data[jsonData]) {
            if (state.id == stateElement.innerHTML) {
                stateElement.innerHTML = state.name;
            }
        }
    });
}


function updateRowCounter() {
    var rowCount = document.getElementById("campagneList").rows.length - 1;
    var totalCount = document.getElementById("totalCount").value;
    document.getElementById("rowCounter").innerHTML = `Showing ${rowCount} of ${totalCount} result`;
    if (rowCount > 1) {
        document.getElementById("rowCounter").innerHTML += "s";
    }
}


async function showLoadingIcon() {
    const sleep = (ms) => new Promise((resolve) => setTimeout(resolve, ms));
    await sleep(500);
    if (!window.contentLoaded) {

        if (document.getElementById("emptyTableMessageRow")) {
            var loadingRow = document.getElementById("emptyTableMessageRow");
        } else {
            var row = document.createElement("tr");
            row.innerHTML = `
                <td id="emptyTableMessageRow" class="fs-5 d-none d-lg-table-cell" colspan="${document.getElementById("campagneList").rows[0].cells.length}" style="text-align: center;"></td>

        `
            document.getElementById("mainTableBody").innerHTML = "";
            document.getElementById("mainTableBody").appendChild(row);
            loadingRow = document.getElementById("emptyTableMessageRow");

        }
        loadingRow.style.minHeight = "200px";
        loadingRow.innerHTML = `
                                <div class="d-flex justify-content-center">
                                <div class="loader"></div>
                            </div>
    `
    }

}