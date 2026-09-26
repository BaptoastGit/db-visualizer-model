class PageHeader extends HTMLElement {
    connectedCallback() {
        this.style.display = "contents";
        this.innerHTML = `
        <div class="page-header">
                <div class="page-header-left d-flex align-items-center">
                    <div class="page-header-title">
                        <h5 class="m-b-10">Dashboard</h5>
                    </div>
                    <ul class="breadcrumb">
                        <li class="breadcrumb-item"><a href="${window.location.pathname}">${document.body.dataset.pageTitle}</a></li>
                    </ul>
                </div>
                <div class="page-header-right  d-flex ms-auto gap-4" id="pageHeaderRight">
                    <button id="ResetFiltersButton" type="button" class="btn btn-light-info" onclick="ResetFilters()" role="button" style="display: none;">
                        <i class="feather-refresh-ccw me-2"></i>
                        <span>Reinitialiser les filtres</span>
                    </button>
                    <div class="d-flex flex-column justify-content-center">
                        <select id="RowCountButton" class="form-select-sm border-0" onchange="UpdateForm()" >
                            <option value="100">100 lignes</option>
                            <option value="500" selected>500 lignes</option>
                            <option value="1000">1000 lignes</option>
                            <option value="5000">5000 lignes</option>
                        </select>
                    </div>
                </div>
            </div>
        `;


    }

}
customElements.define('page-header', PageHeader);

class SubPageHeader extends HTMLElement {
    connectedCallback() {
        this.style.display = "contents";
        this.innerHTML = `
         <div class="page-header">
                <div class="page-header-left d-flex align-items-center">
                    <div class="page-header-title">
                        <h5 id="page-name-title" class="m-b-10"></h5>
                    </div>
                    <ul class="breadcrumb">
                        <li  class="breadcrumb-item"><a id="parent-page-name" href=""></a></li>
                        <li id="page-name-arborescence" class="breadcrumb-item"></li>
                    </ul>
                </div>
                <div class="page-header-right ms-auto">
                    <div class="page-header-right-items">
                        <div class="d-flex d-md-none">
                            <a class="page-header-right-close-toggle">
                                <i class="feather-arrow-left me-2"></i>
                                <span>Back</span>
                            </a>
                        </div>
                    </div> 
                </div>
            </div>
        `;


    }

}
customElements.define('sub-page-header', SubPageHeader);

class SubSubPageHeader extends HTMLElement {
    connectedCallback() {
        this.style.display = "contents";
        this.innerHTML = `
         <div class="page-header">
                <div class="page-header-left d-flex align-items-center">
                    <div class="page-header-title">
                        <h5 id="page-name-title" class="m-b-10"></h5>
                    </div>
                    <ul class="breadcrumb">
                        <li  class="breadcrumb-item"><a id="parent-page-name" href=""></a></li>
                        <li  class="breadcrumb-item"><a id="page-name" href=""></a></li>
                        <li id="sub-page-name-arborescence" class="breadcrumb-item"></li>

                    </ul>
                </div>
                <div class="page-header-right ms-auto">
                    <div class="page-header-right-items">
                        <div class="d-flex d-md-none">
                            <a class="page-header-right-close-toggle">
                                <i class="feather-arrow-left me-2"></i>
                                <span>Back</span>
                            </a>
                        </div>
                    </div> 
                </div>
            </div>
        `;


    }

}
customElements.define('sub-sub-page-header', SubSubPageHeader);




function UpdateForm() {
    document.getElementById("RowCountForm").value = document.getElementById("RowCountButton").value;
    console.log(document.getElementById("RowCountForm").value);
}