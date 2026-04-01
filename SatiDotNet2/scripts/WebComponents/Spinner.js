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
        let documentBodyDOM = self;

        if (!self.Overlay) { //asp.net workaround b/c Overlay dynamic creation can be double-booked with asp.net architecture
            this.shadowRoot.innerHTML += `
            <div id="Overlay" class="overlay" style="justify-content: center; align-items: center; display: none; top: 0; left: 0;">
                <div class="spinner"></div>
            </div>
            `;

            this.Overlay = this.shadowRoot.getElementById("Overlay");

            //add media queries to the shadowDom
            const style = document.createElement("style");
            style.textContent += `
            .overlay {
                position: fixed; 
                width: 100vw;
                height: 100vh;
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

            //add web component to document body of html standard DOM
            while (documentBodyDOM.nodeName.toLowerCase() !== "body") documentBodyDOM = documentBodyDOM.parentElement;
            documentBodyDOM.appendChild(self);

            documentBodyDOM.style.position = "relative"; //add 'position: relative' css property to parent element of web component
        }
    }

    displaySpin() {
        self.Overlay.style.display = "flex";
        self.parentElement.style.overflow = "hidden"; //lock scrolling for window within standard DOM
    }

    hideSpin() {
        self.Overlay.style.display = "none";
        self.parentElement.style.overflow = ""; //unlock scrolling for window within standard DOM
    }
}
customElements.define("sati-spinner", Spinner);