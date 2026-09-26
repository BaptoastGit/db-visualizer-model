function makeColumnEditable(columnsName) {
    window.editableRights = true;
    window.editableColumns = columnsName;
    addSaveButton();
    document.querySelectorAll(".edit-pencil").forEach(pencil => {
        pencil.addEventListener('click', (event) => {
            if (event.target.closest("td").querySelector("span").innerText.includes("/")) { //Show date picker if the value is a date
                input = event.target.querySelector("input");
                input.showPicker();
            } else {
                input = event.target.closest("td").querySelector("input");
                input.classList.remove("d-none");
                input.classList.add("d-table-cell");
                event.target.closest("td").querySelector("span").innerText = "";
            }
        });
    });

    rows = document.querySelectorAll(".table-rows");
    rows.forEach(row => {
        row.addEventListener('mouseenter', function () {
            row.querySelectorAll("i:not(.link-icon)").forEach(icon => {
                icon.classList.remove("invisible");
            });
        });
        row.addEventListener('mouseleave', function () {
            row.querySelectorAll("i:not(.link-icon)").forEach(icon => {
                icon.classList.add("invisible");
            });
        });
    });


}

function addSaveButton() {
    pageHeaderRight = document.getElementById("pageHeaderRight");
    pageHeaderRight.innerHTML += `
                        <button id="cancelButton" type="button" class="btn btn-light-secondary d-none" onclick="ResetColumnsValues()" role="button">
                            <i class="feather-x me-2"></i>
                            <span>Annuler les modifications</span>
                        </button>
                        <button id="saveButton" type="button" class="btn btn-light-primary d-none" onclick="SaveColumnsValues()" role="button">
                            <i class="feather-save me-2"></i>
                            <span>Sauvegarder les modifications</span>
                        </button>
                    `;
}


function ResetColumnsValues() {
    window.location.href = window.location.pathname;
}

async function SaveColumnsValues() {
    const requetes = [];
    console.log(document.querySelectorAll(".edit-forms"));
    document.querySelectorAll(".edit-forms").forEach(form => {
        if (form.childElementCount > 2) {
            requetes.push(
                fetch(form.action, {
                    method: "POST",
                    body: new FormData(form)
                })
            );
        }
    });
    console.log(requetes);
    if (requetes.length > 0) {
        try {
            const reponses = await Promise.all(requetes);
            const success = reponses.every(reponse => reponse.ok);
            window.location.href = window.location.pathname;
        }
        catch {
            console.log("Erreur lors de l'envoi des requêtes");
        }
    }
}


function deletableRows() {
    window.adminRights = true;
    rows = document.querySelectorAll(`tr:not(#mainTable)`);
    rows.forEach(row => {
        bin = document.createElement("i");
        bin.classList.add("bi-trash");
        bin.style.position = "absolute";
        bin.style.right = "8px";
        bin.style.top = "50%";
        bin.style.transform = "translateY(-50%)";
        bin.classList.add("invisible");

        const lastCell = row.cells[row.cells.length - 1];
        lastCell.classList.add("position-relative");

        bin.addEventListener("click", (event) => {
            if (event.target.closest("tr").classList.contains("deleted")) {
                event.target.closest("tr").classList.remove("deleted");
                event.target.closest("tr").style.backgroundColor = "";
                document.getElementById("Deleteform").querySelector(`input[name="${event.target.parentElement.parentElement.firstElementChild.innerText}"]`).remove();
            } else {
                event.target.closest("tr").classList.add("deleted");
                event.target.closest("tr").style.backgroundColor = "#f8d7da";
                document.getElementById("cancelButton").classList.remove("d-none");
                document.getElementById("saveButton").classList.remove("d-none");
                document.getElementById("Deleteform").innerHTML += `<input type="hidden" name="${event.target.parentElement.parentElement.firstElementChild.innerText}" value="true"> `;
            }

        });
        lastCell.appendChild(bin);
    });


}
