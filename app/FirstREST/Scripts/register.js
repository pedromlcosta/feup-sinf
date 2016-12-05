


$(document).ready(function () {

    $("#login_modal").on("hidden.bs.modal", function () {

        //spagghetti way to find form or not?
        $(this).find('form')[0].reset();
        clearModalErrors();
    });


    // Login form submition
    $('#login_form').on('submit', loginButtonHandler);
    $('#requestSubmitBtn').on('click', loginButtonHandler);
    $('#modal_login_button').click(showPopup);
});

function showPopup() {
    $('#modal_login_button').blur();
    $(this).blur();
}

function loginButtonHandler(event) {
    event.preventDefault();
    event.stopPropagation();
    event.stopImmediatePropagation();
    $(this).blur();

    var email = $("#email").val();
    var password = $("#password").val();

    var root = location.protocol + '//' + location.host + '/';

    console.log(root);
    $.ajax({
        url: root + 'api/login',
        type: 'POST',
        data:
        {
            email: email,
            password: password
        },
        success: function (data, textStatus, jqXHR) {
            if (typeof data.error === 'undefined') {
                console.log("here");

                if (data.loggedIn == 'true') {
                    clearModalErrors();
                    //location.reload();
                    window.location.replace("");
                } else {
                    clearModalErrors();
                    $("#login_failure").prepend("Username/Password combination not found.");
                }
            } else {
                // Handle errors here
                console.log('ERRORS: ' + data.error);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            // Handle errors here
            console.log('ERRORS: ' + jqXHR.status + " - " + errorThrown);
            clearModalErrors();
            if (jqXHR.status == 400)
                $("#login_failure").prepend("Username/Password combination not found.");
        }
    });

}


function clearModalErrors() {
    $("#login_success").empty();
    $("#login_failure").empty();
}


