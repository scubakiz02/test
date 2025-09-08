class SatiFullScreen extends HTMLElement {
    #inactivityIntervalMs = 60000;
    #_isTabActive = true;
    #lastUserActivityAt;

    constructor() {
        super();
        this.attachShadow({ mode: 'open' }); // Create shadow DOM
    }

    connectedCallback() {
        const self = this;

        this.#startListeningForAutoScroll();

        document.addEventListener("keydown", function (event) {
            //F11 keydown is tracked across Chrome, Edge, and FireFox only when entering full screen mode (not true when exiting full screen mode)
            //Not sure about other browsers
            //tested last on 08/12/2025
            if (event.key === "F11") {
                event.preventDefault(); //prevent default browser full screen mode

                //start automated scroll right away (modify logic to make isAutoScrollTime() function return true)
                const now = new Date();
                const nowMinus60s = new Date(now.getTime() - self.#inactivityIntervalMs);
                self.#lastUserActivityAt = nowMinus60s;

                //remove sati layout
                self.#setSatiLayoutVisibility();
            }
        })

        document.addEventListener("wheel", (event) => this.#userActivity(event)); //wheel event listener is for laptops/desktops
        document.addEventListener("mousemove", (event) => this.#userActivity(event)); //mousemove event listener is for laptops/desktops
        document.addEventListener("touchmove", (event) => this.#userActivity(event)); //touchmove event listener is for touchscreen devices
        document.addEventListener('visibilitychange', function () {
            if (document.visibilityState === 'visible') {
                self.#_isTabActive = true;

                //when user exits tab, auto scroll function is killed (return is executed)
                self.#startListeningForAutoScroll();
            }
            else {
                self.#_isTabActive = false;
            }
        });
    }

    #userActivity() {
        //disable auto scroll by resetting date var and programmatically scroll to top (if auto scroll was enabled)
        this.#lastUserActivityAt = new Date();
    }

    #setSatiLayoutVisibility() {
        const satiLayoutStyleId = "hide-sati-layout-style";
        const isAutoScrollTime = this.#isAutoScrollTime();
        let satiLayoutStyle = document.getElementById(satiLayoutStyleId);

        if (!satiLayoutStyle) {
            if (isAutoScrollTime) {
                //if relevant html style element does not exist, create it to:
                //1) remove sati layout(header, footer, and background)
                //2) adjust background-color of fullscreen backdrop to white (default is black)
                satiLayoutStyle = document.createElement('style');
                satiLayoutStyle.id = satiLayoutStyleId;
                satiLayoutStyle.textContent = `
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
                document.head.appendChild(satiLayoutStyle);
            }
        }
        else {
            if (!isAutoScrollTime) {
                //bring back sati layout (header, footer, and background)
                satiLayoutStyle.parentElement.removeChild(satiLayoutStyle)
            }
        }
    }

    #isAutoScrollTime() {
        const now = new Date();
        const activityDiffMs = Math.abs(now - this.#lastUserActivityAt);

        //make sure scroll wheel or cursor has not been used within the last inactivity interval
        if (activityDiffMs > this.#inactivityIntervalMs) return true;
        return false;
    }

    #startListeningForAutoScroll() {
        //prep environment, then invoke listenForAutoScroll function
        this.#lastUserActivityAt = new Date();
        this.#listenForAutoScroll(10000); //arg 1 indicates time (in milliseconds) to scroll from top to bottom of page
    }

    //NOTES:
    //1) performance.now() returns the time (in milliseconds) since the page started loading
    //2) requestAnimationFrame() schedules the callback to run just before the browser's next repaint cycle, typically matching the display's refresh rate (e.g., 60Hz, 120Hz). 
    //This ensures animations are rendered efficiently and without tearing.
    #listenForAutoScroll(durationMs) {
        const self = this;
        let direction = 1; // 1 = down, -1 = up
        let start = performance.now();

        function scroll(timestamp) {
            const elapsed = timestamp - start;
            const progress = Math.min(elapsed / durationMs, 1);
            let startScroll = 0;
            let endScroll = document.documentElement.scrollHeight - window.innerHeight;

            if (!self.#_isTabActive) return;

            if (self.#isAutoScrollTime()) {
                // Determine the current position based on direction
                let position;
                if (direction === 1) {
                    position = startScroll + (endScroll - startScroll) * progress;
                } else {
                    position = endScroll - (endScroll - startScroll) * progress;
                }

                window.scrollTo(0, position);
            }
            else {
                //prep environment to scroll down from top of page
                direction = 1;
                start = performance.now();
            }

            if (progress < 1) {
                requestAnimationFrame(scroll);
            } else {
                // Switch direction
                direction *= -1;
                start = performance.now();
                requestAnimationFrame(scroll);
            }

            self.#setSatiLayoutVisibility();
        }

        requestAnimationFrame(scroll);
    }

}

customElements.define('sati-full-screen', SatiFullScreen);