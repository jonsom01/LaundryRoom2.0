var eraser = document.getElementById("eraser");
eraser.addEventListener("click", eraseNumber);
var inputLabel = document.getElementById("numberInput");
var isModal = false;
var time;
var loc = location.toString().substr(10);

loc = loc.substr(loc.indexOf("/") + 1);

if (loc.indexOf("/") > -1)
{
    loc = loc.substr(0, loc.length - 2);
}

var timer = setInterval(update, 5000);
var times = document.getElementsByClassName("time");
var checkButton = document.getElementById("check");
var buttons = document.getElementsByClassName("button");
var numberPad = document.getElementById("numberPad");
var days = document.getElementsByClassName("day");
var dates = document.getElementsByClassName("date");
var date = new Date();
var currentDate = date.getDate();
var daysChanged = false;
document.getElementById("close").addEventListener("click", closeWindow);

for (var i = 0; i < times.length; i++) {
    times[i].addEventListener("click", bookTime);
}

for (var i = 0; i < buttons.length; i++) {
    buttons[i].addEventListener("click", inputNumber);
}

checkButton.addEventListener("click", sendBooking);

window.onload = function () {
    update();
};

function closeWindow() {
    numberPad.style.visibility = "hidden";
    eraser.style.visibility = "hidden";
    var wrapper = document.getElementById("wrapper");
    wrapper.style.opacity = "1";
    isModal = false;
}

function sendBooking(pass) {
    var currTime = document.getElementById(time);
    currTime.style.backgroundImage = 'url("https://bokatvattstugan.online/images/0000.png")';
    currTime.style.opacity = .5;
    if (document.cookie.indexOf("myCookie") === -1) {
        pass = inputLabel.innerText;
    }
    var str = document.cookie.substr(document.cookie.lastIndexOf("myCookie"));
        var cookieTime = str.substr(str.lastIndexOf("e") + 4, 9);
        if (time === cookieTime) {
            document.cookie = "myCookie= ; expires=Thu, 01 Jan 1970 00:00:01 GMT; path=/";
            update();
        }
        else {
            var booking = { "time": time, "user": { "password": pass, "location": loc } };
            $.ajax({
                type: "PUT",
                url: "https://bokatvattstugan.online/api/booking",
                data: JSON.stringify(booking),
                contentType: 'application/json'
            }).done(function () {
                numberPad.style.visibility = "hidden";
                eraser.style.visibility = "hidden";
                var wrapper = document.getElementById("wrapper");
                checkButton.style.backgroundColor = "transparent";
                wrapper.style.opacity = "1";
                isModal = false;
                if (currTime.style.backgroundImage === 'url(https://bokatvattstugan.online/images/tom.jpg)' ||
                    currTime.style.backgroundImage === 'url("https://bokatvattstugan.online/images/tom.jpg")' ||
                    currTime.style.backgroundImage === 'url("https://bokatvattstugan.online/images/0000.png")' ||
                    currTime.style.backgroundImage === 'url(https://bokatvattstugan.online/images/0000.png)') {
                    document.cookie = "myCookie=" + JSON.stringify({ currentTime: time, password: pass }) + ";expires=Thu, 01 Jan 2270 00:00:01 GMT; path=/";
                }
                update();
            }).fail(function () { checkButton.style.backgroundColor = "red"; });
    }
}

function bookTime() {
    if (!isModal) {
        time = this.id;
        if (document.cookie.indexOf("myCookie") !== -1) {
            var str = document.cookie;
            var password = str.substr(str.lastIndexOf(":") + 2, 4);
            sendBooking(password);
        }
        else {
            inputLabel.innerText = "";
            checkButton.style.backgroundColor = "transparent";
            numberPad.style.visibility = "visible";
            var wrapper = document.getElementById("wrapper");
            wrapper.style.opacity = "0.3";
            isModal = true;
        }
    }
    else {
        numberPad.style.visibility = "hidden";
        eraser.style.visibility = "hidden";
        var wrapper = document.getElementById("wrapper");
        wrapper.style.opacity = "1";
        isModal = false;
    }
}


function daysChange() {
    var day = date.getDay();

    for (var i = 0; i < currentDate -1; i++)
    {
        day--;
        if (day < 0)
        {
            day = 6;
        }
    }

    var month = date.getMonth();
    switch (month) {
        case 0:
        case 2:
        case 4:
        case 6:
        case 7:
        case 9:
        case 11:
            for (i = 0; i < 31; i++) {
                days[i].src = "https://bokatvattstugan.online/images/day" + day + ".png";
                if (day < 6) {
                    day++;
                }
                else {
                    day = 0;
                }
            }
            break;
        case 1:
            if ((date.getFullYear() - 2016) % 4 === 0) {
                for ( i = 0; i < 29; i++) {
                    days[i].src = "https://bokatvattstugan.online/images/day" + day + ".png";
                    if (day < 6) {
                        day++;
                    }
                    else {
                        day = 0;
                    }
                }
            }
            else {
                for ( i = 0; i < 28; i++) {
                    days[i].src = "https://bokatvattstugan.online/images/day" + day + ".png";
                    if (day < 6) {
                        day++;
                    }
                    else {
                        day = 0;
                    }

                }

            }
            break;
        default:
            for (i = 0; i < 30; i++) {
                days[i].src = "https://bokatvattstugan.online/images/day" + day + ".png";
                if (day < 6) {
                    day++;
                }
                else {
                    day = 0;
                }
                break;
            }
    }
    daysChanged = true;
  }

function update() {
    var urlstring = "https://bokatvattstugan.online/api/booking?location=" + loc;
    $.get(urlstring, function (data) {
        var bookings = data;

        if (daysChanged === false || currentDate !== date.getDate())
        {
            currentDate = date.getDate();
            daysChange();
        }
    
        for (var i = 0; i < times.length; i++)
        {
            times[i].style.backgroundImage = 'url(https://bokatvattstugan.online/images/tom.jpg)';
            times[i].style.opacity = 1;
            times[i].style.filter = "";
            times[i].style.boxShadow = "";
        }
        for (var j = 0; j < bookings.length; j++) {
            displayBookings(bookings[j].time, bookings[j].bookerId);
        }
        dates[date.getDate() - 1].style.opacity = "0.5";
        if (document.cookie !== "") {
            var str = document.cookie;
            var time = document.getElementById(str.substr(str.lastIndexOf("e") + 4, 9));
            if (time.style.backgroundImage === 'url("https://bokatvattstugan.online/images/tom.jpg")' ||
                time.style.backgroundImage ===  'url(https://bokatvattstugan.online/images/tom.jpg)' ||
                time.style.backgroundImage === 'url(https://bokatvattstugan.online/images/0000.png)' ||
                time.style.backgroundImage === 'url("https://bokatvattstugan.online/images/0000.png")')
            {
                document.cookie = "myCookie= ; expires=Thu, 01 Jan 1970 00:00:01 GMT; path=/";
                update();
            }
            else{
                time.style.filter = "brightness(150%)";
                time.style.boxShadow = "0px 0px 100px yellow";
            }
        }
    });
}

function displayBookings(time, id) {
    var timeSlot = document.getElementById(time);
    if (timeSlot !== null) {
        timeSlot.style.opacity = 1;
        timeSlot.style.backgroundImage = "url('https://bokatvattstugan.online/images/" + id + ".png')";
    }
}

function inputNumber() {
    var btn = this;
    eraser.style.visibility = "visible";
    inputLabel.innerText += btn.innerText;
}

function eraseNumber() {
    checkButton.style.backgroundColor = "transparent";
    inputLabel.innerText = inputLabel.innerText.substring(0, inputLabel.innerText.length - 1);
    if (inputLabel.innerText.length === 0) {
        eraser.style.visibility = "hidden";
    }
}