class MainFooter extends HTMLElement {
    connectedCallback() {
        this.innerHTML = `
         <footer class="footer">
            <p class="fs-11 text-muted fw-medium text-uppercase mb-0 copyright">
                <span>Copyright ©2026 - Baptiste Gallissian</span>
                <script>
                    document.write(new Date().getFullYear());
                </script>
            </p>
            <div class="d-flex align-items-center gap-4">
                <a href="javascript:void(0);" class="fs-11 fw-semibold text-uppercase">Help</a>
                <a href="javascript:void(0);" class="fs-11 fw-semibold text-uppercase">Terms</a>
                <a href="javascript:void(0);" class="fs-11 fw-semibold text-uppercase">Privacy</a>
            </div>
        </footer>
        `;


    }

}
customElements.define('main-footer', MainFooter);
