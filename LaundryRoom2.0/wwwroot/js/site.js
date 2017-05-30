var inputText = "";
var eraser = document.getElementById("eraser");
var inputLabel = document.getElementById("numberInput");
var isModal = false;
var time;
var loc = location.toString().substr(location.toString().lastIndexOf("/") + 1);
var timer = setInterval(update, 5000);
var times = document.getElementsByClassName("time");
var checkButton = document.getElementById("check");
var buttons = document.getElementsByClassName("button");

for (var i = 0; i < times.length; i++) {
    times[i].addEventListener("click", bookTime);
}

for (var i = 0; i < buttons.length; i++) {
    buttons[i].addEventListener("click", inputNumber);
}

checkButton.addEventListener("click", sendBooking);

window.onload = update;

function sendBooking(pass) {
    if (document.cookie === "") {
        pass = inputLabel.innerText;
    }
        var str = document.cookie;
        var cookieTime = str.substr(str.lastIndexOf("e") + 4, 7);
        if (time === cookieTime) {
            document.cookie = "myCookie= ; expires=Thu, 01 Jan 1970 00:00:01 GMT";
            update();
        }
        else {
            var booking = { "time": time, "user": { "password": pass, "location": loc } };
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
                var currTime = document.getElementById(time);
                if (currTime.style.backgroundImage === 'url("images/tom.jpg")') {
                    document.cookie = "myCookie=" + JSON.stringify({ currentTime: time, password: pass });
                }
                update();
            }).fail(function () { checkButton.style.backgroundColor = "red"; })
    }
}

function bookTime() {
    if (!isModal) {
        time = this.id;
        if (document.cookie !== "") {
            var str = document.cookie;
            var password = str.substr(str.lastIndexOf(":") + 2, 4);
            sendBooking(password);
        }
        else {
            inputText = "";
            inputLabel.innerText = "";
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
    var urlstring = "/api/booking?location=" + loc;
    $.get(urlstring, function (data) {
        var bookings = data;

        for (var i = 0; i < times.length; i++)
        {
            times[i].style.backgroundImage = "url('images/tom.jpg')";
            times[i].style.filter = "";
            times[i].style.boxShadow = "";
        }
        for (var j = 0; j < bookings.length; j++) {
            displayBookings(bookings[j].time, bookings[j].bookerId);
        }
        if (document.cookie !== "") {
            var str = document.cookie;
            var time = document.getElementById(str.substr(str.lastIndexOf("e") + 4, 7));
            if (time.style.backgroundImage !== 'url("images/tom.jpg")') {
                time.style.filter = "brightness(150%)";
                time.style.boxShadow = "0px 0px 100px yellow";
            }
        }
    });
}

function displayBookings(time, id) {

    var timeSlot = document.getElementById(time);
    timeSlot.style.backgroundImage = "url('images/" + id + ".png')";
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