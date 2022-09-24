const { fromEvent,merge } = rxjs;

// Handle user clicks open menu button
let isOpeningMenu = false;

fromEvent(document.getElementById("open-menu-button"), "click").subscribe(() => {
    // Diable the scroll on body
    document.querySelector("body").classList.toggle("overflow-hidden");

    // Menu was not opened
    if (!isOpeningMenu) {
        document.getElementById("menu").classList.toggle("hidden");

        setTimeout(() => {
            document.querySelector("#menu > div").classList.toggle("right-[-100%]");
        }, 100);

        isOpeningMenu = !isOpeningMenu;

        return;
    }

    // Menu was opened
    document.querySelector("#menu > div").classList.toggle("right-[-100%]");

    setTimeout(() => {
        document.getElementById("menu").classList.toggle("hidden");
    }, 150);

    isOpeningMenu = !isOpeningMenu;
});

// Handle user clicks backdrop to close menu
fromEvent(document.getElementById("menu"), "click").subscribe((pointerEvent) => {
    const innerDiv = document.querySelector("#menu > div");
    const zoneX = innerDiv.offsetLeft;

    // If user clicks outside the innerDiv, closes the menu
    if (pointerEvent.clientX < zoneX) {
        document.querySelector("body").classList.remove("overflow-hidden");

        document.querySelector("#menu > div").classList.toggle("right-[-100%]");

        setTimeout(() => {
            document.getElementById("menu").classList.toggle("hidden");
        }, 150);

        isOpeningMenu = !isOpeningMenu;
    }
});


let isExpandingGenreList = false;
const genreList = document.getElementById("genre-list");

// Handle user clicks expand genre button on mobile
fromEvent(document.getElementById("genre-mobile-button"), "click").subscribe(() => {
    document.getElementById("genre-arrow").classList.toggle("rotate-180");

    if (genreList.clientHeight) {
        genreList.style.height = "0";
    } else {
        genreList.style.height = document.querySelector("#genre-list > div").clientHeight + "px";
    }
});

// Handle user switches between login form and register form
const openLoginFormButton = document.getElementById("open-login");
const openRegisterFormButton = document.getElementById("open-register");
const loginForm = document.getElementById("login-form");
const registerForm = document.getElementById("register-form");

const handleOpenLoginForm = () => {
    document.getElementById("authentication-box").classList.add("fixed");
    document.getElementById("authentication-box").classList.remove("hidden");

    openLoginFormButton.classList.remove("opacity-50");
    openRegisterFormButton.classList.add("opacity-50");

    loginForm.classList.remove("hidden");
    registerForm.classList.add("hidden");
};

const handleOpenRegisterForm = () => {
    document.getElementById("authentication-box").classList.add("fixed");
    document.getElementById("authentication-box").classList.remove("hidden");

    openLoginFormButton.classList.add("opacity-50");
    openRegisterFormButton.classList.remove("opacity-50");

    loginForm.classList.add("hidden");
    registerForm.classList.remove("hidden");
};

fromEvent(openLoginFormButton, "click").subscribe(handleOpenLoginForm);

fromEvent(openRegisterFormButton, "click").subscribe(handleOpenRegisterForm);

// Handle user clicks login button both in mobile and desktop
const loginButtons = document.querySelectorAll(".login-button");
const registerButtons = document.querySelectorAll(".register-button");

merge(fromEvent(loginButtons[0], "click"), fromEvent(loginButtons[1], "click")).subscribe(handleOpenLoginForm);
merge(fromEvent(registerButtons[0], "click"), fromEvent(registerButtons[1], "click")).subscribe(handleOpenRegisterForm);

// Close authentication form when user clicks "x" button
fromEvent(document.getElementById("close-form-button"), "click").subscribe(() => {
    document.getElementById("authentication-box").classList.add("hidden")
});

const toastBox = document.getElementById("toast-box");
const handleOpenToastBox = (message) => {
    const toastMessage = document.querySelector("#toast-box p");

    toastBox.classList.add("bottom-[20px]");

    toastMessage.innerHTML = message;
};
const handleCloseToastBox = () => {
    toastBox.classList.remove("bottom-[20px]");
};
fromEvent(document.querySelector("#toast-box button"), "click").subscribe(handleCloseToastBox);