class Navbar extends HTMLElement {
    connectedCallback() {
        this.innerHTML = `
        <nav class="nxl-navigation sidebar" id="navbar">
        <div class="navbar-wrapper">
            <div class="m-header">
                <a href="${window.appRoot}UserData/Accounts" class="b-brand">
                    <img  src="https://img.icons8.com/?size=100&id=Md3k8zGyt289&format=png&color=000000" style="max-height:50px"></img>
                </a>
            </div>
            <div class="navbar-content">
                <ul class="nxl-navbar">
                    <li class="nxl-item nxl-caption">
                        <label>Navigation</label>
                    </li>
                  
                    <li class="navbar-dropdown">
                        <a type="button" data-bs-toggle="collapse" href="#collapseUserData" role="button" aria-expanded="false" aria-controls="collapseUserData" class="nxl-link nav-link nav-group d-flex justify-content-between">
                                <div>
                                    <span class="nxl-micon"><i class="feather-user"></i></span>
                                    <span class="nxl-mtext">UserData</span>
                                </div>
                                <div class="d-flex justify-content-end">
                                    <i class="bi-chevron-right"></i>
                                    <i class="bi-chevron-down"></i>

                                </div>

                        </a>
                         <div class="collapse collapseNavBar" id="collapseUserData">
                            <div class="nxl-item">
                                <a href="${window.appRoot}UserData/Accounts" class="nxl-link nav-link sub-nav-link">
                                    <span class="nxl-micon fs-12"><i class="feather-loader fs-6"></i></span>
                                    <span class="nxl-mtext fs-12">Accounts</span>
                                </a>
                           </div>
                            <div class="nxl-item pb-1">
                                <a href="${window.appRoot}UserData/Users" class="nxl-link nav-link sub-nav-link">
                                    <span class="nxl-micon fs-12"><i class="feather-airplay fs-6"></i></span>
                                    <span class="nxl-mtext fs-12">Users</span>
                                </a>
                           </div>                       
                        </div>
                    </li>
                     <li class="navbar-dropdown">
                        <a type="button" data-bs-toggle="collapse" href="#collapseMessages" role="button" aria-expanded="false" aria-controls="collapseMessages" class="nxl-link nav-link nav-group d-flex justify-content-between">
                                <div>
                                    <span class="nxl-micon"><i class="bi-chat-left-text"></i></span>
                                    <span class="nxl-mtext">Messages</span>
                                </div>
                                <div class="d-flex justify-content-end">
                                    <i class="bi-chevron-right"></i>
                                    <i class="bi-chevron-down"></i>

                                </div>

                        </a>
                         <div class="collapse collapseNavBar" id="collapseMessages">
                            <div class="nxl-item pb-1">
                                <a href="${window.appRoot}Messages/Messages" class="nxl-link nav-link sub-nav-link">
                                    <span class="nxl-micon fs-12"><i class="bi-chat-left-dots fs-6"></i></span>
                                    <span class="nxl-mtext fs-12">Messages</span>
                                </a>
                           </div>
                        </div>
                    </li>
                    <li class="navbar-dropdown">
                        <a type="button" data-bs-toggle="collapse" href="#collapseOrders" role="button" aria-expanded="false" aria-controls="collapseOrders" class="nxl-link nav-link nav-group d-flex justify-content-between">
                                <div>
                                    <span class="nxl-micon"><i class="feather-compass"></i></span>
                                    <span class="nxl-mtext">Orders</span>
                                </div>
                               <div class="d-flex justify-content-end">
                                    <i class="bi-chevron-right"></i>
                                    <i class="bi-chevron-down"></i>

                                </div>

                        </a>
                         <div class="collapse collapseNavBar" id="collapseOrders">
                            <div class="nxl-item">
                                <a href="${window.appRoot}Orders/Orders" class="nxl-link nav-link sub-nav-link">
                                    <span class="nxl-micon fs-12"><i class="feather-loader fs-6"></i></span>
                                    <span class="nxl-mtext fs-12">Orders</span>
                                </a>
                           </div>
                            <div class="nxl-item">
                                <a href="${window.appRoot}Orders/Products" class="nxl-link nav-link sub-nav-link">
                                    <span class="nxl-micon fs-12"><i class="feather-box fs-6"></i></span>
                                    <span class="nxl-mtext fs-12">Products</span>
                                </a>
                           </div>
                            <div class="nxl-item pb-1">
                                <a href="${window.appRoot}Orders/Prices" class="nxl-link nav-link sub-nav-link">
                                    <span class="nxl-micon fs-12"><i class="feather-credit-card fs-6"></i></span>
                                    <span class="nxl-mtext fs-12">Prices</span>
                                </a>
                           </div>                           
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
        `;

        this.initializeCollapseHandlers();
    }

    initializeCollapseHandlers() {
        const collapseButtons = this.querySelectorAll('[data-bs-toggle="collapse"]');

        collapseButtons.forEach(button => {
            const collapseId = button.getAttribute('href')?.substring(1);
            
                const collapseElement = this.querySelector(`#${collapseId}`);
                if (collapseElement) {
                    collapseElement.addEventListener('show.bs.collapse', () => {
                        this.updateChevrons(button, true);
                    });

                    collapseElement.addEventListener('hidden.bs.collapse', () => {
                        this.updateChevrons(button, false);
                    });

                    this.updateChevrons(button, button.getAttribute('aria-expanded') === 'true');
                }
            
        });
    }

    updateChevrons(button, isExpanded) {
        const chevronRight = button.querySelector('.bi-chevron-right');
        const chevronDown = button.querySelector('.bi-chevron-down');

        if (chevronRight && chevronDown) {
            if (isExpanded) {
                chevronRight.style.display = 'none';
                chevronDown.style.display = 'flex';
            } else {
                chevronRight.style.display = 'flex';
                chevronDown.style.display = 'none';
            }
        }
    }
}
customElements.define('navigation-menu', Navbar);


function addSidebarTogglersListeners() {
    document.getElementById("mobile-collapse").addEventListener("click", function () {
        document.getElementById("navbar").classList.toggle("mob-navigation-active");
    });
    document.body.addEventListener("click", function () {
        const navbar = document.getElementById("navbar");
        if (navbar.classList.contains("mob-navigation-active") && !document.getElementById("mobile-collapse").contains(event.target) && !navbar.contains(event.target)) {
            navbar.classList.toggle("mob-navigation-active");
        }
    });

    enlightCurrentTab();
}

function enlightCurrentTab() {
    Array.from(document.querySelectorAll(".sub-nav-link")).forEach(navlink => {
        const linkPath = new URL(navlink.href, window.location.origin).pathname;
        if (linkPath == window.location.pathname && window.location.pathname != "/InDevelopment") {
            navlink.closest(".collapse").classList.add("show");
            navlink.classList.add("active");
            navlink.closest(".navbar-dropdown").classList.add("active");
            navlink.closest(".navbar-dropdown").querySelector(".bi-chevron-down").style.display = "flex";
            navlink.closest(".navbar-dropdown").querySelector(".bi-chevron-right").style.display = "none";

        }
    })
}