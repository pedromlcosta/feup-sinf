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

    var email = $("#registerEmail").val();
    var name = $("#registerName").val();
    var nif = $("input[name='nif']").val();
    var address = $("input[name='address']").val();
    var password = $("input[name='password']").val();
    var password_confirmation = $("input[name='password_confirmation']").val();
    var adminCode = $("input[name='admin_code'").val();
    var telemovel = $("input[name='telemovel'").val();
    var localidade = $("input[name='localidade'").val();
    var codigoPostal = $("input[name='cp'").val();


    console.log("Email: " + email);
    console.log("Nome: " + name);
    console.log("nif: " + nif);
    console.log("addr: " + address);
    console.log("pass: " + password);
    console.log("check pass : " + password_confirmation);
    console.log("admin code: " + adminCode);
     console.log("telemovel: " + telemovel );
     console.log("localidade: " + localidade );
     console.log("codigoPostal: " + codigoPostal); 

    var root = location.protocol + '//' + location.host + '/';
    var regex="\\d\\d\\d\\d-\\d\\d\\d";
    var matchResult=codigoPostal.match(regex);
    console.log(matchResult.length);
    console.log(root);
    if (password == password_confirmation && matchResult != null && matchResult.length==1) {

        $.ajax({
            url: root + 'api/clientRegister',
            type: 'POST',
            data:
            {
                email: email,
                name: name,
                nif: nif,
                address: address,
                password: password,
                adminCode: adminCode,
                telemovel: telemovel,
                localidade: localidade,
                cp:codigoPostal
            },
            success: function (data, textStatus, jqXHR) {
                if (typeof data.error === 'undefined') {
                    console.log("here");

                    if (data.registered == 'true') {
                        clearModalErrors();
                        $("#register_failure").prepend("Success");
                        window.location.href = root;
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
         clearModalErrors();
        $("#register_failure").prepend("Password and confirmation don't match.");
    }

}


function clearModalErrors() {
    $("#register_success").empty();
    $("#register_failure").empty();
}


