$(document).ready(function () {

    // Login form submition
    $('#registerSubmitBtn').on('submit', registernButtonHandler);
    $('#registerSubmitBtn').on('click', registernButtonHandler);
});

function registernButtonHandler(event) {


    event.preventDefault();
    event.stopPropagation();
    event.stopImmediatePropagation();
    $(this).blur();

    var email = $("input[name='email']").val();
    var name = $("input[name='name']").val();
    var nif = $("input[name='nif']").val();
    var address = $("input[name='address']").val();
    var password = $("input[name='password']").val();
    var password_confirmation = $("input[name='password_confirmation']").val();

    

    console.log(email);
    console.log(nif);
    console.log(address);
    console.log(password);
    console.log(password_confirmation);

    var root = location.protocol + '//' + location.host + '/';

    console.log(root);
    if (password == password_confirmation) {

        $.ajax({
            url: root + 'api/clientRegister',
            type: 'POST',
            data:
            {
                email: email,
                name: name,
                nif: nif,
                address: address,
                password: password
            },
            success: function (data, textStatus, jqXHR) {
                if (typeof data.error === 'undefined') {
                    console.log("here");

                    if (data.registered == 'true') {
                        clearModalErrors();
                        $("#register_failure").prepend("Success");
                        //location.reload();
                        //window.location.replace("");
                    } else {
                        clearModalErrors();
                        $("#register_failure").prepend("Error registering.");
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
                    $("#register_failure").prepend("Error registering.");
            }
        });
    } else {
        $("#register_failure").prepend("Password and confirmation don't match.");
    }

}


function clearModalErrors() {
    $("#register_success").empty();
    $("#register_failure").empty();
}


