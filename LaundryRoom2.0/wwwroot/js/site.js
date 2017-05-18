var inputText = "";
var eraser = document.getElementById("eraser");
var inputLabel = document.getElementById("numberInput");
var sec = 1;
var isModal = false;
var addressOk = false;
var time;
var address;
var timer = setInterval(update, 10000);
var times = document.getElementsByClassName("time");
var checkButton = document.getElementById("check");
var buttons = document.getElementsByClassName("button");
var date = new Date();
var dailyBookingsErased = false;

for (var i = 0; i < times.length; i++) {
    times[i].addEventListener("click", bookTime);
}

for (var i = 0; i < buttons.length; i++) {
    buttons[i].addEventListener("click", inputNumber);
}

checkButton.addEventListener("click", check);

window.onload = update;

function sendBooking(pass) {

    var booking = {"time": time, "bookerpass":pass, "bookeraddress":address};
    $.ajax({
        type: "PUT",
        url: "/api/booking",
        data: JSON.stringify(booking),
        contentType: 'application/json',
    }).done(function () {
        var numberPad = document.getElementById("numberPad");
        numberPad.style.visibility = "hidden";
        eraser.style.visibility = "hidden";
        var wrapper = document.getElementById("wrapper");
        checkButton.style.backgroundColor = "transparent";
        wrapper.style.opacity = "1";
        isModal = false;
        update();
        }).fail(function () { checkButton.style.backgroundColor = "red"; })
}


function inputNumber() {
    var btn = this;
    eraser.style.visibility = "visible";
    inputText += btn.innerText;
    inputLabel.innerText = inputText;
    eraser.addEventListener("click", eraseNumber);
}

function eraseNumber() {
    checkButton.style.backgroundColor = "transparent";
    inputText = inputText.substring(0, inputText.length - 1);
    inputLabel.innerText = inputText;
    if (inputText.length === 0) {
        eraser.style.visibility = "hidden";
    }
}

function bookTime() {
    if (!isModal) {
        time = this.id;
        addressOk = false;
        inputText = "";
        inputLabel.innerText = "";
        document.getElementById("instructions").innerHTML = "Ange adress (t ex Y14)";
        checkButton.style.backgroundColor = "transparent";
        var numberPad = document.getElementById("numberPad");
        numberPad.style.left = this.offsetLeft - 100;
        window.scrollTo(this.offsetLeft - 600, 0);
        if (this.offsetLeft > 2000) {
            numberPad.style.left = this.offsetLeft - 400;
            window.scrollTo(this.offsetLeft - 600, 0);
        }

        numberPad.style.visibility = "visible";
        var wrapper = document.getElementById("wrapper");
        wrapper.style.opacity = "0.3";
        isModal = true;
    }
    else {

        var numberPad = document.getElementById("numberPad");
        numberPad.style.visibility = "hidden";
        eraser.style.visibility = "hidden";
        var wrapper = document.getElementById("wrapper");
        wrapper.style.opacity = "1";
        isModal = false;
    }
}

function update() {
    var urlstring = "/api/booking";
    $.get(urlstring, function (data) {
        var bookings = data;

        for (var i = 0; i < times.length; i++)
        {
            times[i].style.backgroundImage = "url('images/tom.jpg')";
        }
        for (var i = 0; i < bookings.length; i++) {
            displayBookings(bookings[i].time, bookings[i].bookerId);
        }
    });
}

function check() {
     
    if (!addressOk) {
        address = inputLabel.innerText;
        var urlstring = "/api/booking/checkaddress?address=" + address;
        var jqxhr = $.get(urlstring, function () {
            inputText = "";
            inputLabel.innerText = "";
            eraser.style.visibility = "hidden";
            addressOk = true;
            document.getElementById("instructions").innerHTML = "Ange din kod";
        }).fail(function () { checkButton.style.backgroundColor = "red"; })
    }
    else {
        sendBooking(inputLabel.innerText);
    }
}

function displayBookings(time, id) {

    var timeSlot = document.getElementById(time);
    timeSlot.style.backgroundImage = "url('images/" + id + ".png')";
}