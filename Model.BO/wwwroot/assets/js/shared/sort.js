function SortTable(el, page) {
    const sortState = JSON.parse(sessionStorage.getItem('sortOrder' + page)) || {};
    const currentOrder = el.getAttribute("direction");
    let newSortState = {};
    fetch('./../data/pagesLayout.json')
        .then(response => response.json())
        .then(data => {
            for (const [key, value] of Object.entries(data[page]["Colonnes Tables"])) {
                newSortState[value["DBColumn"]] = null;
            }
        })

    const newOrder = (currentOrder === '0') ? '1' : '0';
    newSortState[el.id] = newOrder;

    if (!sessionStorage.getItem("SortFormData" + page)) {
        sessionStorage.setItem("ShowFilters" + page, "false");
    }
    sessionStorage.setItem('sortOrder' + page, JSON.stringify(newSortState));
    document.getElementById("SortBy").value = el.id;
    document.getElementById("Order").value = newOrder;
    document.getElementById("SearchButton").click();


}


function ShowOrderedRow() {
    const sortOrder = JSON.parse(sessionStorage.getItem('sortOrder' + document.body.dataset.pageTitle));
    if (sortOrder) {
        document.querySelectorAll(".header-label").forEach(label => {
            label.innerHTML = label.title;
        });
        for (const [key, value] of Object.entries(sortOrder)) {
            document.getElementById("label-" + key).innerHTML = `${document.getElementById("label-" + key).title} ${value === '1' ? '⏶' : (value === '0' ? '⏷' : '')}`;
        }
    }
}