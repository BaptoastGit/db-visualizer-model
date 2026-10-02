
function initializeCollapseHandlers() {
    const navBar = document.getElementById("navbar")
    const collapseButtons = navBar.querySelectorAll('[data-bs-toggle="collapse"]');

    collapseButtons.forEach(button => {
        const collapseId = button.getAttribute('href')?.substring(1);

        const collapseElement = navBar.querySelector(`#${collapseId}`);
        if (collapseElement) {
            collapseElement.addEventListener('show.bs.collapse', () => {
                updateChevrons(button, true);
            });

            collapseElement.addEventListener('hidden.bs.collapse', () => {
                updateChevrons(button, false);
            });

            updateChevrons(button, button.getAttribute('aria-expanded') === 'true');
        }

    });

}

function updateChevrons(button, isExpanded) {
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
        const urlParams = new URLSearchParams(window.location.search);
        currentTab = urlParams.get("TableName") ?? "";

        if (navlink.getAttribute("page") == currentTab && window.location.pathname != "/InDevelopment") {
            navlink.closest(".collapse").classList.add("show");
            navlink.classList.add("active");
            navlink.closest(".navbar-dropdown").classList.add("active");
            navlink.closest(".navbar-dropdown").querySelector(".bi-chevron-down").style.display = "flex";
            navlink.closest(".navbar-dropdown").querySelector(".bi-chevron-right").style.display = "none";

        }
    })
}