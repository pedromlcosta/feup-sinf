function showPopup(){
    $('#modal_login_button').blur();
    $(this).blur();
}


$(document).ready(function() {

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


function loginButtonHandler(event) {
    event.preventDefault();
    event.stopPropagation();
    event.stopImmediatePropagation();
    $(this).blur();

  
    console.log("was clicked!");
    console.log($("#email").val());

    $.ajax({
        url: 'api/login',
        type: 'POST',
        data: 
        {
            email: $("#email").val(),
            password: $("#password").val()
        },
        success: function (data, textStatus, jqXHR) {
            if (typeof data.error === 'undefined') {
                console.log("here");
                console.log(data);

                if (data == 'true') {
                    clearModalErrors();
                    //location.reload();
                    window.location.replace("../../index.php");
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
            console.log("there");
            // Handle errors here
            console.log('ERRORS: ' + textStatus);
            // STOP LOADING SPINNER
        }
    });

}


function clearModalErrors() {
    $("#login_success").empty();
    $("#login_failure").empty();
}


