

async function injectTableHeaders(tableHeaderId, addActionColumn, dict, addCheckedColumn = false) {

    if (addCheckedColumn) {
        let checked = document.createElement("th");
        checked.id = "th-checked";
        checked.setAttribute("width", "6%");
        checked.classList.add("resizable-th");
        checked.classList.add("custom-checkbox");
        checked.innerHTML = `
                        <div class="th-wrapper">
                                <div class="checkbox-wrapper-28" style="--size: 22px;">
                                    <input id="tmp-a" type="checkbox" class="promoted-input-checkbox"/>
                                    <svg><use xlink:href="#checkmark-28" /></svg>
                                    <label id="label-tmp-a">
                                    </label>
                                    <svg xmlns="http://www.w3.org/2000/svg" style="display: none">
                                    <symbol id="checkmark-28" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-miterlimit="10" fill="none"  d="M22.9 3.7l-15.2 16.6-6.6-7.1">
                                        </path>
                                    </symbol>
                                    </svg>
                                </div>
                        </div>
                    `;
        document.getElementById(tableHeaderId).appendChild(checked);
        document.getElementById("label-tmp-a").addEventListener("click", (event) => {
            event.target.classList.toggle('checked');
                    if (event.target.classList.contains('checked')) {
                        document.querySelectorAll(".checkbox-labels").forEach(checkbox => {
                            if (!checkbox.classList.contains('checked')) {
                                checkbox.click();
                            }
                        });

                    }
                    else {
                        document.querySelectorAll(".checkbox-labels").forEach(checkbox => {
                            if (checkbox.classList.contains('checked')) {
                                checkbox.click();
                            }
                        });
            }
            document.getElementById("tmp-a").click();

        });
        
          
                            
    }

    for (var key in dict) {
            let th = document.createElement("th");
            th.id = "th-" + key;
            th.classList.add("resizable-th");
            if (!dict[key][0]) {
                th.classList.add("d-none"); th.classList.add("d-lg-table-cell");
            }
            if (dict[key][1] != 0) th.setAttribute("width", dict[key][1]);
            th.innerHTML = `
                <div class="th-wrapper ">
                    <div class="cellule-reduite" style="cursor:pointer;" id="${key}" onclick="SortTable(this, '${document.body.dataset.pageTitle}')" >${key} </div>
                    <div onmousedown="getColumnSize(this)" class="resizer"></div>
                </div>
            `;
            document.getElementById("mainTable").appendChild(th);
    }
    if (addActionColumn) {
        let actions = document.createElement("th");
                actions.id = "th-Actions";
                actions.setAttribute("width", "10%");
                actions.classList.add("resizable-th");
                actions.innerHTML = `
                        <div class="th-wrapper">
                            <div id="Actions" >Actions</div>
                        </div>
                    `;
        document.getElementById(tableHeaderId).appendChild(actions);
    }
        
    
}

