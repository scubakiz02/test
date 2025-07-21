class SatiAlert extends HTMLElement {
    static get observedAttributes() {
        return ['message']; // Custom attribute to observe
    }

    constructor() {
        super();
        this.attachShadow({ mode: 'open' }); // Create shadow DOM
    }

    connectedCallback() {
        this.parentElement.style.position = "relative"; //to align web component at top right of parent element

        this.render();
    }

    attributeChangedCallback(attr, oldValue, newValue) {
        if (attr === 'message') {
            this.render();
        }
    }

    render() {
        const msg = this.getAttribute('message') || "";
        this.shadowRoot.innerHTML = `
        <style>
            .alert {
                position: absolute;
                top: 0;
                right: 0;
                color: white;
                padding: 6px 8px;
                margin: 2px;
                border-radius: 12px;
                display: flex;
                align-items: center;
                gap: 2px;
                z-index: 9999;
                opacity: 0;
                /* allow underlying clicks through transparent overlay */
                background: transparent;
                pointer-events: none;
                /* allow underlying clicks through transparent overlay */
                transition: opacity 0.5s ease-out, 0.5s ease-out; /* ensure a transition occurs when 'show' css class is removed */
            }

            .error {
                background-color: red;
                color: white;
            }

            .show {
                opacity: 1;
                animation: slideUpFade 0.5s ease-out;
            }

            #alert-message {
                flex-grow: 1;
            }

            @keyframes slideUpFade {
                from {
                    opacity: 0;
                    transform: translateY(10px);
                }

                to {
                    opacity: 1;
                    transform: translateY(0);
                }
            }
        </style>

        <section id="alert-section" class="alert error">
            <span id="alert-icon">⚠️</span>
            <div id="alert-message">${msg}</div>
        </section>
        `;
    }

    show() {
        const alertSection = this.shadowRoot.getElementById("alert-section");

        alertSection.classList.add("show");

        setTimeout(() => {
            alertSection.classList.remove("show");
        }, 3000); //3000ms = 3 seconds
    }
}

customElements.define('sati-alert', SatiAlert);