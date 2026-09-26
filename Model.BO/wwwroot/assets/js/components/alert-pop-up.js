function createAlertPopUp(e, redirectionLink) {
    if (CheckModifications() == true) {
        e.preventDefault();
        popUp = document.createElement("div");
        popUp.classList.add("modal");
        popUp.classList.add("fade");
        popUp.tabindex = "-1";
        popUp.role = "dialog";
        popUp.id = "myModal";
        popUp.innerHTML = `
                          <div class="modal-dialog" role="document">
                            <div class="modal-content">
                              <div class="modal-header">
                                <h5 class="modal-title">Modifications non enregistrées</h5>
                                <button type="button" class="close btn btn-light-brand btn-hover" onclick="Close()">
                                  <span >&times;</span>
                                </button>
                              </div>
                              <div class="modal-body">
                                <span>Voulez-vous enregistrer vos modifications avant de quitter cette page?</span>
                              </div>
                              <div class="modal-footer border-0">
                                <button onclick="SaveModifications()" class="btn btn-primary">Enregistrer et continuer</button>
                                <a type="button" class="btn btn-light-brand" href="${redirectionLink}" data-dismiss="modal">Ignorer les modifications</a>
                              </div>
                            </div>
                          </div>
           
                        `
        document.body.appendChild(popUp);
        var myModal = new bootstrap.Modal(document.getElementById('myModal'), {
            keyboard: false
        })
        myModal.toggle();
    }
}

function CheckModifications(old) {
    const newValues = {};
    document.querySelectorAll(".userInput").forEach(input => newValues[input.id] = input.value);
    return (!(JSON.stringify(newValues) == JSON.stringify(window.oldValues)));
}

function SaveModifications() {
    document.getElementById("form").submit();
}

function Close() {
    var myModal = bootstrap.Modal.getInstance(document.getElementById('myModal'));
    myModal.hide();
}