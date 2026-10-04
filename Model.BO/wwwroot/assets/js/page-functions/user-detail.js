window.onload = function () {
    document.getElementById("parent-page-name").innerHTML = "Users";
    document.getElementById("parent-page-name").href = "TableDashboard?TableName=Users";

    document.getElementById("page-name-title").innerHTML = document.body.dataset.pageTitle;
    document.getElementById("page-name-arborescence").innerHTML = "Détails";

}