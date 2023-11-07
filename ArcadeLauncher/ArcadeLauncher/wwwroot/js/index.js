const rAF = window.mozRequestAnimationFrame || window.requestAnimationFrame;

var focusableElements = document.querySelectorAll(
    '.play-btn'
);

var current = 0;
var invert = 1;
var savedTimeout = 500;

window.addEventListener('gamepadconnected', function (event) {
    console.log(event);
    removeActive();
    updateLoop();
    if (navigator.getGamepads()[0].id.startsWith("Xinmotek")) {
        invert = -1;
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
    const gamepad = navigator.getGamepads()[0];
    const xBoxButtonA = gamepad.buttons[0];
    const xBoxButtonB = gamepad.buttons[1];
    const xBoxButton1 = gamepad.buttons[2];
    const xBoxButton2 = gamepad.buttons[3];
    const xBoxButton3 = gamepad.buttons[4];
    const xBoxButton4 = gamepad.buttons[5];
    const xBoxButton5 = gamepad.buttons[6];
    const xBoxButton6 = gamepad.buttons[7];
    const xBoxButton7 = gamepad.buttons[8];
    const xBoxButton8 = gamepad.buttons[9];
    const xBoxButton9 = gamepad.buttons[10];
    const xBoxStickLeft = gamepad.axes[0];

    if (navigator.getGamepads().length > 1) {

        const gamepad1 = navigator.getGamepads()[1];
        const xBoxButtonA_1 = gamepad1.buttons[0];
        const xBoxButtonB_1 = gamepad1.buttons[1];
        const xBoxButton1_1 = gamepad1.buttons[2];
        const xBoxButton2_1 = gamepad1.buttons[3];
        const xBoxButton3_1 = gamepad1.buttons[4];
        const xBoxButton4_1 = gamepad1.buttons[5];
        const xBoxButton5_1 = gamepad1.buttons[6];
        const xBoxButton6_1 = gamepad1.buttons[7];
        const xBoxButton7_1 = gamepad1.buttons[8];
        const xBoxButton8_1 = gamepad1.buttons[9];
        const xBoxButton9_1 = gamepad1.buttons[10];
        const xBoxStickLeft_1 = gamepad1.axes[0];

        if (xBoxButtonA_1.pressed) { testLog(11) }
        if (xBoxButtonB_1.pressed) { testLog(12) }
        if (xBoxButton1_1.pressed) { testLog(13) }
        if (xBoxButton2_1.pressed) { testLog(14) }
        if (xBoxButton3_1.pressed) { testLog(15) }
        if (xBoxButton4_1.pressed) { testLog(16) }
        if (xBoxButton5_1.pressed) { testLog(17) }
        if (xBoxButton6_1.pressed) { testLog(18) }
        if (xBoxButton7_1.pressed) { testLog(19) }
        if (xBoxButton8_1.pressed) { testLog(20) }
        if (xBoxButton9_1.pressed) { testLog(21) }
    }

    if (xBoxButtonB.pressed) { testConsole() }
    if (xBoxButtonA.pressed) { testLog(0) }
    if (xBoxButtonB.pressed) { testLog(1) }
    if (xBoxButton1.pressed) { testLog(2) }
    if (xBoxButton2.pressed) { testLog(3) }
    if (xBoxButton3.pressed) { testLog(4) }
    if (xBoxButton4.pressed) { testLog(5) }
    if (xBoxButton5.pressed) { testLog(6) }
    if (xBoxButton6.pressed) { testLog(7) }
    if (xBoxButton7.pressed) { testLog(8) }
    if (xBoxButton8.pressed) { testLog(9) }
    if (xBoxButton9.pressed) { testLog(10) }



    var timeout = 0;
    if (xBoxStickLeft < -0.1) {
        timeout = savedTimeout;
        LowerTimeout();
        prevItem();
    }
    else if (xBoxStickLeft > 0.1) {
        timeout = savedTimeout;
        LowerTimeout();
        nextItem();
    } else {
        savedTimeout = 500;
    }
    setTimeout(() => rAF(updateLoop), timeout);

    focusableElements = document.querySelectorAll(
        '.play-btn'
    );

    if (xBoxButton1.pressed) {
        document.querySelector("#StartText").classList.add("hidden");
        document.querySelector("#MenuText").classList.remove("hidden");
        scroller();
        console.log("test");
    }

}

function LowerTimeout() {
    savedTimeout -= 50;
    if (savedTimeout < 50) {
        savedTimeout = 50;
    }
}


function testConsole() {
    focusableElements[current].click();

}

function testLog(number) {
    console.log("Hallo" + number);
    console.log(focusableElements);
}

function nextItem() {
    focusableElements = document.querySelectorAll(
        '.play-btn'
    );
    OffsetCurrent(1);
    focusableElements[current].focus();
}

function prevItem() {
    focusableElements = document.querySelectorAll(
        '.play-btn'
    );

    OffsetCurrent(-1);
    focusableElements[current].focus();
}

function OffsetCurrent(amount) {
    current = (current - amount * invert) % focusableElements.length;
    if (current < 0) {
        current = focusableElements.length - 1;
    }
}
