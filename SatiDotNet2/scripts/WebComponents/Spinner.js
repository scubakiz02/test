class Spinner extends HTMLElement {
    self = null;

    constructor() {
        super();
        this.attachShadow({ mode: "open" });

        self = this;

        return this;
    }

    connectedCallback() {
        const self = this;

        if (!self.Overlay) { //asp.net workaround b/c Overlay dynamic creation can be double-booked with asp.net architecture
            this.shadowRoot.innerHTML += `
            <div id="Overlay" class="overlay" style="justify-content: center; align-items: center; display: none; width: 100%; height: 100%; top: 0; left: 0;">
                <div class="spinner"></div>
            </div>
            `;

            this.Overlay = this.shadowRoot.getElementById("Overlay");

            //add media queries to the shadowDom
            const style = document.createElement("style");
            style.textContent += `
            .overlay {
                position: absolute;
                width: 100%;
                height: 100%;
                background-color: black;
                opacity: .5;
                z-index: 999;
            }

            .spinner {
                width: 50px;
                height: 50px;
                border: 6px solid #fff;
                border-top: 6px solid transparent;
                border-radius: 50%;
                animation: spin 1s linear infinite;
            }

            @keyframes spin {
                0% {
                    transform: rotate(0deg);
                }

                100% {
                    transform: rotate(360deg);
                }
            }
            `;
            this.shadowRoot.appendChild(style);

            self.parentElement.style.position = "relative"; //add 'position: relative' css property to parent element of web component

        }
    }

    displaySpin() {
        self.Overlay.style.display = "flex";
    }

    hideSpin() {
        self.Overlay.style.display = "none";
    }
}
customElements.define("sati-spinner", Spinner);