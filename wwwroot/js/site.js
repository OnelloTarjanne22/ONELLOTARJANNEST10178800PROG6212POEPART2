document.getElementById('claimForm').addEventListener('submit', function (event) {
    event.preventDefault();
    alert('Claim submitted successfully!');
});

document.getElementById('trackingForm').addEventListener('submit', function (event) {
    event.preventDefault();
    alert('Tracking request sent!');
});

var passConfirm = function () {
    if (document.getElementById("Password").value ==
        document.getElementById("ConfirmPassword").value) {
        document.getElementById("Message").style.color = "Green";
        document.getElementById("Message").innerHTML = "Passwords match!"
    } else {
        document.getElementById("Message").style.color = "Red";
        document.getElementById("Message").innerHTML = "Passwords do NOT match!"
    }
}