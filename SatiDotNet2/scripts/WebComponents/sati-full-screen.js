class SatiFullScreen extends HTMLElement {

    #hideSatiLayoutStyleId = "hide-sati-layout-style";
    #fullscreenWrapper = undefined;

    constructor() {
        super();
        this.attachShadow({ mode: 'open' }); // Create shadow DOM
    }

    connectedCallback() {
        const self = this;

        document.addEventListener('fullscreenchange', function (event) {
            if (self.getAttribute('isSatiLayoutActive') === "true") {
                if (document.fullscreenElement) {
                    //remove sati layout (header, footer, and background)
                    const style = document.createElement('style');
                    style.id = self.#hideSatiLayoutStyleId;
                    style.textContent = `
                        #ctl00_MasterPagePanelTop {
                            display: none;
                        }

                        #ctl00_MasterPagePanelBottom {
                            display: none;
                        }

                        #ctl00_MasterPagePanel {
                            min-width: unset;
                        }

                        .MasterMainBackground {
                            background: none;
                            margin: 0;
                        }`;
                    document.head.appendChild(style);

                }
                else {
                    //bring back sati layout (header, footer, and background)
                    const style = document.getElementById(self.#hideSatiLayoutStyleId);
                    style.parentElement.removeChild(style)

                    //remove fullscreen wrapper
                    const wrapper = self.#fullscreenWrapper;
                    if (!wrapper) return;

                    // Move all children of the wrapper back to wrapper's parent
                    const parent = wrapper.parentNode;
                    while (wrapper.firstChild) {
                        parent.insertBefore(wrapper.firstChild, wrapper);
                    }

                    // Remove the empty wrapper element
                    wrapper.remove();
                    self.#fullscreenWrapper = undefined;
                }
            }

            self.#automatedScroll(10000); //10000ms duration to scroll from top to bottom
        })

        document.addEventListener("keydown", function (event) {
            //F11 keydown is tracked across Chrome, Edge, and FireFox only when entering full screen mode (not true when exiting full screen mode)
            //Not sure about other browsers
            //tested last on 08/12/2025
            if (event.key === "F11") {
                //prevent default browser full screen mode
                event.preventDefault();

                //create full screen wrapper (necessary for automated scroll in programmatically initiated full screen mode)
                if (!self.#fullscreenWrapper) {
                    // Create the fullscreen wrapper div dynamically
                    self.#fullscreenWrapper = document.createElement('div');
                    self.#fullscreenWrapper.id = 'fullscreen-wrapper';

                    // Apply styles to make it fill viewport & scrollable
                    Object.assign(self.#fullscreenWrapper.style, {
                        height: '100vh',
                        overflowY: 'auto'
                    });

                    // Move all children of body into fullscreenWrapper
                    while (document.body.firstChild) {
                        self.#fullscreenWrapper.appendChild(document.body.firstChild);
                    }

                    // Append fullscreenWrapper as the only child of body
                    document.body.appendChild(self.#fullscreenWrapper);
                }
                self.#fullscreenWrapper.requestFullscreen(); //programmatically enter full screen mode

            }
        })
    }

    //NOTES:
    //1) performance.now() returns the time (in milliseconds) since the page started loading
    //2) requestAnimationFrame() schedules the callback to run just before the browser's next repaint cycle, typically matching the display's refresh rate (e.g., 60Hz, 120Hz). 
    //This ensures animations are rendered efficiently and without tearing.
    #automatedScroll(durationMs) {
        const self = this;
        let direction = 1; // 1 = down, -1 = up
        let start = performance.now();

        function scroll(timestamp) {
            const elapsed = timestamp - start;
            const progress = Math.min(elapsed / durationMs, 1);

            if (self.#fullscreenWrapper) {
                let startScroll = 0;
                let endScroll = self.#fullscreenWrapper.scrollHeight - window.innerHeight;

                if (document.fullscreenElement) {
                    // Determine the current position based on direction
                    let position;
                    if (direction === 1) {
                        position = startScroll + (endScroll - startScroll) * progress;
                    } else {
                        position = endScroll - (endScroll - startScroll) * progress;
                    }

                    self.#fullscreenWrapper.scrollTo(0, position);
                }
                else {
                    //prep environment to scroll down from top of page
                    direction = 1;
                    start = performance.now();
                }
            }

            if (progress < 1) {
                requestAnimationFrame(scroll);
            } else {
                // Switch direction
                direction *= -1;
                start = performance.now();
                requestAnimationFrame(scroll);
            }
        }

        requestAnimationFrame(scroll);
    }

}

customElements.define('sati-full-screen', SatiFullScreen);