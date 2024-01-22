const rAF = window.mozRequestAnimationFrame || window.requestAnimationFrame;

var currentIndex = 0;
var currentRow = 0;
var invert = 1;
var rows;
var savedTimeout = 500;
var isplaying = false;

window.addEventListener('gamepadconnected', function (event) {
    console.log(event);
    removeActive();
    if (!isplaying) {
        isplaying = true;
        updateLoop();

    }
});

document.addEventListener('keydown', function (event) {
    if (event.keyCode == 100) {
        nextItem();
    }
    else if (event.keyCode == 102) {
        prevItem();
    }
    else if (event.keyCode == 98) {
        Shiftrow(1);
    }
    else if (event.keyCode == 104) {
        Shiftrow(-1);
    }
    else if (event.keyCode == 13) {
        ShowCatalouge();
    }
});

function removeActive() {
    console.log("check");
    console.log(document.querySelectorAll(".active"));
    if (document.querySelectorAll(".active") > 0) {
        document.querySelectorAll(".reveal").classList.remove("active");
    }

}

function updateLoop() {
    try {

            for (var i = 0; i < navigator.getGamepads().length; i++) {

            const gamepad = navigator.getGamepads()[i];
            const xBoxButtonA = gamepad.buttons[0];
            const xBoxButtonB = gamepad.buttons[1];
            const xBoxButton1 = gamepad.buttons[2];
            const xBoxButton2 = gamepad.buttons[3];
            const xBoxButton3 = gamepad.buttons[4];
            const xBoxButton4 = gamepad.buttons[5];
            const xBoxStickLeft = gamepad.axes[0];

            if (xBoxButton1.pressed) { Launch() }

            var timeout = 0;
            if (xBoxStickLeft < -0.1) {
                timeout = savedTimeout;
                LowerTimeout();
                nextItem();
            }
            else if (xBoxStickLeft > 0.1) {
                timeout = savedTimeout;
                LowerTimeout();
                prevItem();
            } else {
                savedTimeout = 500;
            }
            if (xBoxButton1.pressed) {
                //document.querySelector("#StartText").classList.add("hidden");
                //document.querySelector("#MenuText").classList.remove("hidden");
                ShowCatalouge();
            }
        }

    }
    finally {
        setTimeout(() => rAF(updateLoop), timeout);
    }


}

function ShowCatalouge() {
    scroller();
    QueryDocument();
    focusSelected(false);
}

function LowerTimeout() {
    savedTimeout -= 50;
    if (savedTimeout < 50) {
        savedTimeout = 50;
    }
}


function Launch() {
    QueryDocument();

    if (focusableElements.length > currentIndex) {
        focusableElements[currentIndex].click();
    }

}
function nextItem() {
    QueryDocument();

    OffsetCurrentIndex(1);
    focusSelected();
}

function prevItem() {
    QueryDocument();

    OffsetCurrentIndex(-1);
    focusSelected();
}

function OffsetCurrentIndex(amount) {

    if (isNaN(currentIndex)) {
        currentIndex = 0;
    }

    currentIndex = (currentIndex - amount * invert) % focusableElements.length;
    if (currentIndex < 0) {
        currentIndex = focusableElements.length - 1;
    }
}

function Shiftrow(amount) {

    QueryDocument();

    rows[currentRow].index = currentIndex;

    currentRow = (currentRow + amount) % rows.length;
    if (currentRow < 0)
        currentRow = rows.length - 1;

    console.log("shift row " + rows[currentRow].id);

    currentIndex = rows[currentRow].index;

    QueryDocument();
    OffsetCurrentIndex(0);
    focusSelected();
}

function focusSelected(scroll = true) {
    if (focusableElements.length > 0 && currentIndex < focusableElements.length) {

        rows[currentRow].element.parentElement.scrollTo({
            top: 450 * currentRow,
            left: 0,
            behavior: "smooth",
        });
        console.log("changing focus");
        var focusTo = focusableElements[currentIndex];
        focusTo.focus({
            preventScroll: true
        });
        focusTo.parentElement.scrollTo({
            top: 0,
            left: 170 * currentIndex - (170 / 2),
            behavior: "smooth",
        });

    }
}

function QueryDocument() {

    if (rows == undefined) {
        console.log("Gettings rows")
        rows = Array.from(document.querySelectorAll(".category"));
        rows = rows.map((x) => {
            return ({ id: x.id, index: 0, element: x})
        });
    }

    focusableElements = document.querySelectorAll(
        '#' + rows[currentRow].id +' .play-btn'
    );


}