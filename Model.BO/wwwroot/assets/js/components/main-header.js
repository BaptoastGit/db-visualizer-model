class MainHeader extends HTMLElement {
    connectedCallback() {
        this.innerHTML = `
        <header class="nxl-header">
        <div class="header-wrapper">
            <!--! [Start] Header Left !-->
            <div class="header-left d-flex align-items-center gap-4">
                <!--! [Start] nxl-head-mobile-toggler !-->
                <a href="javascript:void(0);" class="nxl-head-mobile-toggler" id="mobile-collapse">
                    <div class="hamburger hamburger--arrowturn">
                        <div class="hamburger-box">
                            <div class="hamburger-inner"></div>
                        </div>
                    </div>
                </a>
                <!--! [Start] nxl-head-mobile-toggler !-->
                <!--! [Start] nxl-navigation-toggle !-->
                
                <!--! [End] nxl-navigation-toggle !-->
                <!--! [Start] nxl-lavel-mega-menu-toggle !-->
               
                <!--! [End] nxl-lavel-mega-menu-toggle !-->
            </div>
            <!--! [End] Header Left !-->
            <!--! [Start] Header Right !-->
            <div class="header-right ms-auto">
                <div class="d-flex align-items-center">
                    <div class="nxl-h-item d-none d-sm-flex">
                        <div class="full-screen-switcher">
                            <a href="javascript:void(0);" class="nxl-head-link me-0" onclick="toggleFullScreen()">
                                <i class="feather-maximize maximize"></i>
                                <i class="feather-minimize minimize"></i>
                            </a>
                        </div>
                    </div>
                    <div class="nxl-h-item dark-light-theme">
                        <a href="javascript:void(0);" onclick="toggleDarkMode(this)" class="nxl-head-link me-0 dark-button">
                            <i class="feather-moon"></i>
                        </a>
                    </div>
                    <div class="d-flex align-items-center">
                        <span class="text-secondary" >Utilisateur: ${document.body.dataset.pageUser}</span>
                    </div>
                </div>
            </div>
            <!--! [End] Header Right !-->
        </div>
    </header>
        `;


    }

}
customElements.define('main-header', MainHeader);

function toggleFullScreen() {

    if (document.fullscreenElement == null) {
        document.documentElement.requestFullscreen();
    } else {
        document.exitFullscreen();

    }
return false; 
}

function toggleDarkMode(element) {

    if (document.documentElement.getAttribute('data-bs-theme') == "light") {
        sessionStorage.setItem("theme", "dark");
        element.innerHTML = '<i class="feather-moon"></i>'
        document.documentElement.setAttribute('data-bs-theme', "dark")
    } else {
        document.documentElement.setAttribute('data-bs-theme', "light")
        sessionStorage.setItem("theme", "light");
        element.innerHTML = '<i class="feather-sun"></i>'


    }
    return false;
}